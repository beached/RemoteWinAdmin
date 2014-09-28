using System.Collections.Generic;
using daw;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteWindowsAdministrator {
	public class CurrentUsers: IDataPageRow {
		public string ComputerName { get; set; }
		public string ConnectionStatus { get; set; }
		public string Domain { get; set; }
		public string UserName { get; set; }
		public DateTime? LastLogon { get; set; }
		public string Sid { get; set; }
		public string ProfileFolder { get; set; }

		public string LogonDuration {
			get {
				return null != LastLogon ? MagicValues.TimeSpanToString( DateTime.Now - LastLogon.Value ) : null;
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
			return (new ValueIsIn( value )).Add( ComputerName ).Add( ConnectionStatus ).Add( Domain ).Add( UserName ).Add( LastLogon ).Add( Sid ).Add( ProfileFolder ).Add( LogonDuration ).IsContained;
		}

		private static void GetLocallyLoggedOnUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			var usersList = new List<CurrentUsers>( );
			using( var regHku = RegistryKey.OpenRemoteBaseKey( RegistryHive.Users, string.Empty ) ) {
				foreach( var currentSid in regHku.GetSubKeyNames( ).Where( IsSid ) ) {
					var cu = new CurrentUsers( computerName ) { Sid = currentSid };
					try {
						if( Win32.WellKnownSids.ContainsKey( currentSid ) ) {
							cu.Domain = computerName;	// Local account
							cu.UserName = Win32.WellKnownSids[currentSid];
						} else {
							GetUserAccountFromSid( ref cu );
						}
						cu.ProfileFolder = RegistryHelpers.GetString( computerName, RegistryHive.LocalMachine, string.Format( @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\{0}", currentSid ), @"ProfileImagePath" );
						cu.LastLogon = GetUsersLogonTimestamp( cu );
					} catch {
						cu = new CurrentUsers( computerName, @"Error" ) { Sid = currentSid };
					}
					usersList.Add( cu );
				}
			}
			result.AddRange( usersList );
		}

		private static Win32.Error GetNetworkUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			Win32.Error res;
			var er = 0;
			var tr = 0;
			var resume = 0;
			var buffer = IntPtr.Zero;
			var usersList = new List<CurrentUsers>( );
			do {
				try {
					res = (Win32.Error)Win32.NetSessionEnum( computerName, null, null, 502, out buffer, -1, ref er, ref tr, ref resume );
					if( res == Win32.Error.ErrorMoreData || res == Win32.Error.Success ) {
						var bufferPtrInt = buffer.ToInt32( );
						for( var i = 0; i < er; i++ ) {
							var sessionInfo = (Win32.SessionInfo502)Marshal.PtrToStructure( new IntPtr( bufferPtrInt ), typeof( Win32.SessionInfo502 ) );
							var userInfo = new CurrentUsers( computerName ) {
								UserName = sessionInfo.userName, LastLogon = DateTime.Now.AddSeconds( -sessionInfo.logonDuration )
							};
							usersList.Add( userInfo );
							bufferPtrInt += Marshal.SizeOf( typeof( Win32.SessionInfo502 ) );
						}

					} else {
						switch( res ) {
						case Win32.Error.ErrorAccessDenied:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Access Denied: {0}", computerName ) );
							break;
						case Win32.Error.ErrorNotEnoughMemory:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Not Enough Memory: {0}", computerName ) );
							break;
						case Win32.Error.ErrorBadNetpath:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Bad Network Path: {0}", computerName ) );
							break;
						case Win32.Error.ErrorNetworkBusy:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Network Busy: {0}", computerName ) );
							break;
						case Win32.Error.ErrorInvalidParameter:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Invalid Parameter: {0}", computerName ) );
							break;
						case Win32.Error.ErrorInsufficientBuffer:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Insufficient Buff: {0}", computerName ) );
							break;
						case Win32.Error.ErrorInvalidLevel:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Invalid Level: {0}", computerName ) );
							break;
						case Win32.Error.ErrorExtendedError:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Exended Error: {0}", computerName ) );
							break;
						case Win32.Error.ErrorNoNetwork:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: No Network: {0}", computerName ) );
							break;
						case Win32.Error.ErrorInvalidHandleState:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Invalid Handle State: {0}", computerName ) );
							break;
						case Win32.Error.NerrBase:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: NERR_BASE: {0}", computerName ) );
							break;
						case Win32.Error.NerrUnknownDevDir:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Unknown Device Directory: {0}", computerName ) );
							break;
						case Win32.Error.NerrDuplicateShare:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Duplicate Share: {0}", computerName ) );
							break;
						case Win32.Error.NerrBufTooSmall:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: Buffer too small: {0}", computerName ) );
							break;
						case Win32.Error.ErrorNoBrowserServersFound:
							Debug.WriteLine( string.Format( @"GetNetworkUsers: No Browser Servers Found: {0}", computerName ) );
							break;
						}
						return res;
					}
				} finally {
					if( IntPtr.Zero != buffer ) {
						Win32.NetApiBufferFree( buffer );
					}
				}
			} while( res == Win32.Error.ErrorMoreData );
			result.AddRange( usersList );
			return Win32.Error.Success;
		}

		private static void GetUserAccountFromSid( ref CurrentUsers user ) {
			var binSid = Win32.StringToBinarySid( user.Sid );
			if( null == binSid ) {
				user.ConnectionStatus = @"Error resolving SID";
				return;
			}
			var name = new StringBuilder( );
			var cchName = (uint)name.Capacity;
			var referencedDomainName = new StringBuilder( );
			var cchReferencedDomainName = (uint)referencedDomainName.Capacity;
			Win32.SidNameUse sidUse;

			var err = Win32.Error.Success;
			if( !Win32.LookupAccountSid( null, binSid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse ) ) {
				err = (Win32.Error)Marshal.GetLastWin32Error( );
				if( Win32.Error.ErrorInsufficientBuffer == err ) {
					name.EnsureCapacity( (int)cchName );
					referencedDomainName.EnsureCapacity( (int)cchReferencedDomainName );
					err = Win32.Error.Success;
					if( !Win32.LookupAccountSid( null, binSid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse ) ) {
						err = (Win32.Error)Marshal.GetLastWin32Error( );
					}
				}
			}
			if( Win32.Error.Success == err ) {
				user.UserName = name.ToString( );
				user.Domain = referencedDomainName.ToString( );
			} else {
				user.ConnectionStatus = @"Error resolving SID";
			}
		}

		public static void GetCurrentUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			Helpers.Assert( null != result, @"result SyncList cannot be null" );
			Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );

			switch( GetNetworkUsers( computerName, ref result ) ) {
			case Win32.Error.Success:
				break;
			case Win32.Error.ErrorMoreData:
				break;
			case Win32.Error.ErrorAccessDenied:
				result.Add( new CurrentUsers( computerName, @"Access Denied" ) );
				//return;
				break;
			default:
				result.Add( new CurrentUsers( computerName, @"Error" ) );
				//return;
				break;
			}
			GetLocallyLoggedOnUsers( computerName, ref result );
		}

		private static DateTime? GetUsersLogonTimestamp( CurrentUsers user ) {
			if( string.IsNullOrEmpty( user.UserName ) || string.IsNullOrEmpty( user.Domain ) || Win32.WellKnownSids.ContainsKey( user.Sid ) ) {
				return null;
			}
			WmiHelpers.ForEachWithScope( user.ComputerName, @"SELECT * FROM Win32_LogonSession", ( obj, scope ) => {
				try {
					var roq = new RelatedObjectQuery( string.Format( @"associators of {{Win32_LogonSession.LogonId='{0}'}} WHERE AssocClass = Win32_LoggedOnUser", WmiHelpers.GetString( obj, @"LogonId" ) ) );
					using( var searcher = new ManagementObjectSearcher( scope, roq ) ) {
						foreach( var mobObj in searcher.Get( ) ) {
							Debug.Assert( null != mobObj, @"WMI Error, null value returned." );
							var mob = (ManagementObject)mobObj;
							var name = WmiHelpers.GetString( mob, @"Name" );
							var domain = WmiHelpers.GetString( mob, @"Domain" );
							if( name.Equals( user.UserName ) && domain.Equals( user.Domain ) ) {
								user.LastLogon = WmiHelpers.GetNullableDate( obj, @"StartTime" );
								return false; // Found, stop loop
							}
						}
					}
				} catch( System.Management.ManagementException ex ) {
					Debug.WriteLine( string.Format( @"Error finding last logon on {0} for {1}\{2}", user.ComputerName, user.Domain, user.UserName ) );
					Debug.WriteLine( ex.Message );
				}
				return true;
			}, false, false );
			return user.LastLogon;
		}

		private static bool IsSid( string sid ) {
			sid = sid.Trim( );
			return sid.StartsWith( @"S-" ) && !sid.EndsWith( @"_Classes" );
		}

	}
}
