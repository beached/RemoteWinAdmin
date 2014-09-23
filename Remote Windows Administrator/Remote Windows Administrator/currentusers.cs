using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace RemoteWindowsAdministrator {
	public class CurrentUsers: IContainsString {
		public string ComputerName { get; set; }
		public string ConnectionStatus { get; set; }
		public string Domain { get; set; }
		public string UserName { get; set; }
		public DateTime? LastLogon { get; set; }
		public string Sid { get; set; }
		public string ProfileFolder { get; set; }

		private DateTime? _dtTimeStamp;

		public string LogonDuration {
			get {
				if( null == LastLogon ) {
					return null;
				}
				if( null == _dtTimeStamp ) {
					_dtTimeStamp = DateTime.Now;
				}
				var duration = _dtTimeStamp.Value - LastLogon.Value;
				return string.Format( @"{0}day{1} {2}hrs {3}min", duration.Days, duration.Days != 1 ? @"s":@"", duration.Hours, duration.Minutes );
			}
		}
					
		public CurrentUsers( string computerName, string connectionStatus = @"OK" ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
		}
		
		/// <summary>
		/// Does a string exist in all string representations of the fields
		/// </summary>
		/// <param name="value">Value to search for</param>
		/// <returns>True if any field contains value, false otherwise</returns>
		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Test( ComputerName ).Test( ConnectionStatus ).Test( Domain ).Test( UserName ).Test( LastLogon ).Test( Sid ).Test( ProfileFolder ).Test( LogonDuration ).IsContained;
		}

		private static void GetLocallyLoggedOnUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			using( var regHku = RegistryKey.OpenRemoteBaseKey( RegistryHive.Users, string.Empty ) ) {
				foreach( var sid in regHku.GetSubKeyNames( ).Where( IsSid ) ) {
					var cu = new CurrentUsers( computerName ) {Sid = sid};
					try {
						if( WellKnownSids.ContainsKey( sid ) ) {
							cu.Domain = computerName;
							cu.UserName = WellKnownSids[sid];
						} else {
							GetUserAccountFromSid( ref cu );
						}
						cu.ProfileFolder = RegistryHelpers.GetString( computerName, RegistryHive.LocalMachine, string.Format( @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\{0}", sid ), @"ProfileImagePath" );
						cu.LastLogon = GetUsersLogonTimestamp( cu );
					} catch {
						cu = new CurrentUsers( computerName, @"Error" ) {Sid = sid};
					}
					result.Add( cu );
				}
			}
		}

		public static void GetCurrentUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			GetLocallyLoggedOnUsers( computerName, ref result );
			//GetRemoteUsers( computerName, ref result );
		}

		[DllImport( "advapi32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		// ReSharper disable once RedundantNameQualifier
		private static extern bool LookupAccountSid( string lpSystemName, [MarshalAs( UnmanagedType.LPArray )] byte[] sid, System.Text.StringBuilder lpName, ref uint cchName, System.Text.StringBuilder referencedDomainName, ref uint cchReferencedDomainName, out SidNameUse peUse );

		private static void GetUserAccountFromSid( ref CurrentUsers user ) {
			const int errorNone = 0;
			const int errorInsufficientBuffer = 122;


			var binSid = StringToBinarySid( user.Sid );
			if( null == binSid ) {
				user.ConnectionStatus = @"Error resolving SID";
				return;
			}
			var name = new StringBuilder( );
			var cchName = (uint)name.Capacity;
			var referencedDomainName = new StringBuilder( );
			var cchReferencedDomainName = (uint)referencedDomainName.Capacity;
			SidNameUse sidUse;

			var err = errorNone;
			if( !LookupAccountSid( null, binSid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse ) ) {
				err = Marshal.GetLastWin32Error( );
				if( errorInsufficientBuffer == err ) {
					name.EnsureCapacity( (int)cchName );
					referencedDomainName.EnsureCapacity( (int)cchReferencedDomainName );
					err = errorNone;
					if( !LookupAccountSid( null, binSid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse ) ) {
						err = Marshal.GetLastWin32Error( );
					}
				}
			}
			if( errorNone == err ) {
				user.UserName = name.ToString( );
				user.Domain = referencedDomainName.ToString( );
			} else {
				user.ConnectionStatus = @"Error resolving SID";
			}
		}

		private static DateTime? GetUsersLogonTimestamp( CurrentUsers user ) {
			if( string.IsNullOrEmpty( user.UserName ) || string.IsNullOrEmpty( user.Domain ) || WellKnownSids.ContainsKey( user.Sid ) ) {
				return null;
			}
			try {
				WmiHelpers.ForEachWithScope( user.ComputerName, @"SELECT * FROM Win32_LogonSession", ( obj, scope ) => {
					var roq = new RelatedObjectQuery( string.Format( @"associators of {{Win32_LogonSession.LogonId='{0}'}} WHERE AssocClass = Win32_LoggedOnUser", user.UserName ) );
					using( var searcher = new ManagementObjectSearcher( scope, roq ) ) {
						foreach( var mobObj in searcher.Get( ) ) {
							Debug.Assert( null != mobObj, @"WMI Error, null value returned." );
							var mob = (ManagementObject)mobObj;
							var name = WmiHelpers.GetString( mob, @"Name" );
							var domain = WmiHelpers.GetString( mob, @"Domain" );
							if( name.Equals( user.UserName ) && domain.Equals( user.Domain ) ) {
								user.LastLogon = WmiHelpers.GetNullableDate( obj, @"StartTime" );
							}
						}
					}
				}, false, false );
			} catch( Exception ex ) {
				Debug.WriteLine( string.Format( @"Error finding last logon on {0} for {1}\{2}", user.ComputerName, user.Domain, user.UserName ) );
				Debug.WriteLine( ex.Message );
			}
			return user.LastLogon;
		}

		private static byte[] StringToBinarySid( string sid ) {
			SecurityIdentifier si;
			try {
				si = new SecurityIdentifier( sid );
			} catch( ArgumentException ex ) {
				Debug.WriteLine( string.Format( "Exception while looking up SID\n{0}", ex.Message ) );
				return null;
			}
			var binSid = new byte[si.BinaryLength];
			si.GetBinaryForm( binSid, 0 );
			return binSid;
		}

		private static bool IsSid( string sid ) {
			sid = sid.Trim( );
			return sid.StartsWith( @"S-" ) && !sid.EndsWith( @"_Classes" );
		}

		public static readonly Dictionary<string, string> WellKnownSids = new Dictionary<string, string> {
			{ @"S-1-0-0", @"NULL" }, 
			{ @"S-1-1-0", @"EVERYONE" }, 
			{ @"S-1-2-0", @"LOCAL" }, 
			{ @"S-1-2-1", @"CONSOLE_LOGON" }, 			
			{ @"S-1-3-0", @"CREATOR_OWNER" }, 
			{ @"S-1-3-1", @"CREATOR_GROUP" }, 
			{ @"S-1-3-2", @"OWNER_SERVER"  }, 
			{ @"S-1-3-3", @"GROUP_SERVER" },
			{ @"S-1-3-4", @"OWNER_RIGHTS" }, 
			{ @"S-1-5", @"NT_AUTHORITY" }, 
			{ @"S-1-5-1", @"DIALUP" }, 
			{ @"S-1-5-2", @"NETWORK" }, 
			{ @"S-1-5-3", @"BATCH" }, 
			{ @"S-1-5-4", @"INTERACTIVE" }, 
			{ @"S-1-5-6", @"SERVICE" }, 
			{ @"S-1-5-7", @"ANONYMOUS" }, 
			{ @"S-1-5-8", @"PROXY" }, 
			{ @"S-1-5-9", @"ENTERPRISE_DOMAIN_CONTROLLERS" }, 
			{ @"S-1-5-10", @"PRINCIPAL_SELF" }, 
			{ @"S-1-5-11", @"AUTHENTICATED_USERS" }, 
			{ @"S-1-5-12", @"RESTRICTED_CODE" }, 
			{ @"S-1-5-13", @"TERMINAL_SERVER_USER" }, 
			{ @"S-1-5-14", @"REMOTE_INTERACTIVE_LOGON" }, 
			{ @"S-1-5-15", @"THIS_ORGANIZATION" }, 
			{ @"S-1-5-17", @"IUSR" }, 
			{ @"S-1-5-18", @"LOCAL_SYSTEM" }, 
			{ @"S-1-5-19", @"LOCAL_SERVICE" }, 
			{ @"S-1-5-20", @"NETWORK_SERVICE" }
		};

		private enum SidNameUse {
			User = 1,
			Group,
			Domain,
			Alias,
			WellKnownGroup,
			DeletedAccount,
			Invalid,
			Unknown,
			Computer
		}
	}
}
