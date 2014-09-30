using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace RemoteWindowsAdministrator {
	public sealed class DprCurrentUsers: IDataPageRow {
		public enum LogonTypes {
			Unknown = 0,
			Local,
			Share
		}

		private string _computerName;
		public string ComputerName {
			get {
				Helpers.Assert( !string.IsNullOrEmpty( _computerName ), @"Computer name is mandatory and must be set" );
				return _computerName;
			}
			set {
				Helpers.Assert( !string.IsNullOrEmpty( value ), @"Attempt to set ComputerName to a null or empty value" );
				_computerName = value;
			}
		}

		public ConnectionStatuses ConnectionStatus { get; set; }
		public string ConnectionStatusString {
			get { return Helpers.CamelToSpace( ConnectionStatus.ToString( ) ); }
		}


		public DateTime? LastLogon { get; private set; }
		public string Domain { get; private set; }
		public string ProfileFolder { get; private set; }
		public string Sid { get; private set; }
		public string UserName { get; private set; }
		public LogonTypes LogonType {
			get;
			private set;
		}
		public string LogonDuration {
			get {
				return null != LastLogon ? MagicValues.TimeSpanToString( DateTime.Now - LastLogon.Value ) : null;
			}
		}
		public Guid RowGuid {
			get;
			private set;
		}
		public IDictionary<string, Func<IDataPageRow, bool>> GetActions( ) {
			return SetupActions( );
		}

		public DprCurrentUsers( ) {
			ConnectionStatus = ConnectionStatuses.Ok;
			RowGuid = Guid.NewGuid( );
		}

		public DprCurrentUsers( string computerName, ConnectionStatuses connectionStatus = ConnectionStatuses.Ok ) {			
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
			RowGuid = Guid.NewGuid( );
		}

		public static IDictionary<string, Func<IDataPageRow, bool>> SetupActions( ) {
			return new Dictionary<string, Func<IDataPageRow, bool>>( );
		}


		/// <summary>
		/// Does a string exist in all string representations of the fields
		/// </summary>
		/// <param name="value">Value to search for</param>
		/// <returns>True if any field contains value, false otherwise</returns>
		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add( ComputerName ).Add( ConnectionStatus ).Add( Domain ).Add( UserName ).Add( LastLogon ).Add( Sid ).Add( ProfileFolder ).Add( LogonDuration ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( ComputerName );
		}

		private static void GetLocallyLoggedOnUsers( string computerName, SyncList.SyncList<DprCurrentUsers> result ) {
			var usersList = new List<DprCurrentUsers>( );
			using( var regHku = RegistryKey.OpenRemoteBaseKey( RegistryHive.Users, string.Empty ) ) {
				foreach( var currentSid in regHku.GetSubKeyNames( ).Where( IsSid ) ) {
					var cu = new DprCurrentUsers( computerName ) { Sid = currentSid };
					try {
						if( Win32.WellKnownSids.ContainsKey( currentSid ) ) {
							cu.Domain = computerName;	// Local account
							cu.UserName = Win32.WellKnownSids[currentSid];
						} else {
							GetUserAccountFromSid( ref cu );
						}
						cu.ProfileFolder = RegistryHelpers.GetString( computerName, RegistryHive.LocalMachine, string.Format( @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList\{0}", currentSid ), @"ProfileImagePath" );
						cu.LastLogon = GetUsersLogonTimestamp( cu );					
					} catch( Exception ex ) {
						GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"Exception - {0} - {1}", ex.TargetSite, ex.Message );
						cu = new DprCurrentUsers( computerName, ConnectionStatuses.Error ) { Sid = currentSid };
					}
					cu.LogonType = LogonTypes.Local;
					usersList.Add( cu );
				}
			}
			result.AddRange( usersList );
		}

		private static Win32.Error GetNetworkUsers( string computerName, ref SyncList.SyncList<DprCurrentUsers> result ) {
			Win32.Error res;
			var er = 0;
			var tr = 0;
			var resume = 0;
			var buffer = IntPtr.Zero;
			var usersList = new List<DprCurrentUsers>( );
			do {
				try {
					res = (Win32.Error)Win32.NetSessionEnum( computerName, null, null, 502, out buffer, -1, ref er, ref tr, ref resume );
					if( res == Win32.Error.ErrorMoreData || res == Win32.Error.Success ) {
						var bufferPtrInt = buffer.ToInt32( );
						for( var i = 0; i < er; i++ ) {
							var sessionInfo = (Win32.SessionInfo502)Marshal.PtrToStructure( new IntPtr( bufferPtrInt ), typeof( Win32.SessionInfo502 ) );
							var cu = new DprCurrentUsers( computerName ) {
								UserName = sessionInfo.userName, LastLogon = DateTime.Now.AddSeconds( -sessionInfo.logonDuration ), LogonType = LogonTypes.Share
							};
							usersList.Add( cu );
							bufferPtrInt += Marshal.SizeOf( typeof( Win32.SessionInfo502 ) );
						}

					} else {
						switch( res ) {
						case Win32.Error.ErrorAccessDenied:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Access Denied: {0}", computerName );
							break;
						case Win32.Error.ErrorNotEnoughMemory:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Not Enough Memory: {0}", computerName );
							break;
						case Win32.Error.ErrorBadNetpath:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Bad Network Path: {0}", computerName );
							break;
						case Win32.Error.ErrorNetworkBusy:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Network Busy: {0}", computerName );
							break;
						case Win32.Error.ErrorInvalidParameter:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Invalid Parameter: {0}", computerName );
							break;
						case Win32.Error.ErrorInsufficientBuffer:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Insufficient Buff: {0}", computerName );
							break;
						case Win32.Error.ErrorInvalidLevel:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Invalid Level: {0}", computerName );
							break;
						case Win32.Error.ErrorExtendedError:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Exended Error: {0}", computerName );
							break;
						case Win32.Error.ErrorNoNetwork:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: No Network: {0}", computerName );
							break;
						case Win32.Error.ErrorInvalidHandleState:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Invalid Handle State: {0}", computerName );
							break;
						case Win32.Error.NerrBase:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: NERR_BASE: {0}", computerName );
							break;
						case Win32.Error.NerrUnknownDevDir:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Unknown Device Directory: {0}", computerName );
							break;
						case Win32.Error.NerrDuplicateShare:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Duplicate Share: {0}", computerName );
							break;
						case Win32.Error.NerrBufTooSmall:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: Buffer too small: {0}", computerName );
							break;
						case Win32.Error.ErrorNoBrowserServersFound:
							GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"GetNetworkUsers: No Browser Servers Found: {0}", computerName );
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

		private static void GetUserAccountFromSid( ref DprCurrentUsers user ) {
			var binSid = Win32.StringToBinarySid( user.Sid );
			if( null == binSid ) {
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"DgvHelpers - GetNetworkUsers - Error Resolving SID" );
				user.ConnectionStatus = ConnectionStatuses.ErrorResolvingSid;
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
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"DgvHelpers - GetNetworkUsers - Error Resolving SID - Error ", err );
				user.ConnectionStatus = ConnectionStatuses.ErrorResolvingSid;
			}
		}

		public static void Generate( string computerName, SyncList.SyncList<DprCurrentUsers> result ) {
			Helpers.Assert( null != result, @"result SyncList cannot be null" );
			Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );

			switch( GetNetworkUsers( computerName, ref result ) ) {
			case Win32.Error.Success:
				break;
			case Win32.Error.ErrorMoreData:
				break;
			case Win32.Error.ErrorAccessDenied:
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"DprCurrentUsers - Generate - Access Denied for {0}", computerName );
				result.Add( new DprCurrentUsers( computerName, ConnectionStatuses.AccessDenied ) );
				//return;
				break;
			default:
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"DprCurrentUsers - Generate - Unknown Error for {0}", computerName );
				result.Add( new DprCurrentUsers( computerName, ConnectionStatuses.Error ) );
				//return;
				break;
			}
			GetLocallyLoggedOnUsers( computerName, result );
			ValidateUniqueness( result );			
		}

		public static void ValidateUniqueness( SyncList.SyncList<DprCurrentUsers> rows ) {
			var guids = new HashSet<System.Guid>( );
			foreach( var item in rows ) {
				Helpers.Assert( !guids.Contains( item.RowGuid ), @"RowGuid's must be unique" );
				guids.Add( item.RowGuid );
			}
		}

		private static DateTime? GetUsersLogonTimestamp( DprCurrentUsers user ) {
			if( string.IsNullOrEmpty( user.UserName ) || string.IsNullOrEmpty( user.Domain ) || Win32.WellKnownSids.ContainsKey( user.Sid ) ) {
				return null;
			}
			WmiHelpers.ForEachWithScope( user.ComputerName, @"SELECT * FROM Win32_LogonSession", ( obj, scope ) => {
				try {
					var roq = new RelatedObjectQuery( string.Format( @"associators of {{Win32_LogonSession.LogonId='{0}'}} WHERE AssocClass = Win32_LoggedOnUser", WmiHelpers.GetString( obj, @"LogonId" ) ) );
					using( var searcher = new ManagementObjectSearcher( scope, roq ) ) {
						foreach( var mobObj in searcher.Get( ) ) {
							Helpers.Assert( null != mobObj, @"WMI Error, null value returned." );
							var mob = (ManagementObject)mobObj;
							var name = WmiHelpers.GetString( mob, @"Name" );
							var domain = WmiHelpers.GetString( mob, @"Domain" );
							if( !name.Equals( user.UserName ) || !domain.Equals( user.Domain ) ) {
								continue;
							}
							user.LastLogon = WmiHelpers.GetNullableDate( obj, @"StartTime" );
							return false; // Found, stop loop
						}
					}
				} catch( System.Management.ManagementException ex ) {
					GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"Error finding last logon on {0} for {1}\{2}\n{3}", user.ComputerName, user.Domain, user.UserName, ex.Message );
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
