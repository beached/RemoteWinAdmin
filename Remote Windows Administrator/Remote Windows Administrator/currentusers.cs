using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace RemoteWindowsAdministrator {
	public class CurrentUsers: IContainsString {
		public enum LogonTypes: uint {
			System = 0,
			Interactive = 2,
			Network = 3,
			Batch = 4,
			Service = 5,
			Proxy = 6,
			Unlock = 7,
			NetworkCleartext = 8,
			NewCredentials = 9,
			RemoteInteractive = 10,
			CachedInteractive = 11,
			CachedRemoteInteractive = 12,
			CachedUnlock = 13,
			Unknown = uint.MaxValue
		}

		public static LogonTypes LogonTypeFromInt( uint logonType ) {			
			return (LogonTypes)Enum.ToObject( typeof( LogonTypes ), logonType );
		}
	
		public readonly HashSet<string> ValidStatus = new HashSet<string> { @"OK", @"Error", @"Degraded", @"Unknown", @"Pred Fail", @"Starting", @"Stopping", @"Service" };
		public string ComputerName { get; set; }
		public string Domain { get; set; }
		public string AuthenticationPackage { get; set; }
		public string Caption { get; set; }
		public string LogonId { get; set; }
		public LogonTypes LogonType { get; set; }
		public string Name { get; set; }
		public DateTime? StartTime { get; set; }

		public string Status { get; set; }
		public string ConnectionStatus { get; set; }

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Test( ComputerName ).Test( Domain ).Test( AuthenticationPackage ).Test( Caption ).Test( LogonId ).Test( LogonType.ToString( ) ).Test( Name ).Test( StartTime ).Test( Status ).Test( ConnectionStatus ).IsContained;
		}

		public CurrentUsers( string computerName, string connectionStatus = @"OK" ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
			LogonType = LogonTypes.Unknown;
		}

		public static void GetCurrentUsers( string computerName, ref SyncList.SyncList<CurrentUsers> result ) {
			var cu = new CurrentUsers( computerName );
			try {
				WmiHelpers.ForEachWithScope( computerName, @"SELECT * FROM Win32_LogonSession", ( obj, scope ) => {
					cu.AuthenticationPackage = WmiHelpers.GetString( obj, @"AuthenticationPackage" );
					cu.LogonType = LogonTypeFromInt( WmiHelpers.GetUInt( obj, @"LogonType" ) );
					cu.StartTime = WmiHelpers.GetNullableDate( obj, @"StartTime" );
					cu.LogonId = WmiHelpers.GetString( obj, @"LogonId" );

					var roq = new RelatedObjectQuery( string.Format( @"associators of {{Win32_LogonSession.LogonId='{0}'}} WHERE AssocClass = Win32_LoggedOnUser", cu.LogonId ) );
					using( var searcher = new ManagementObjectSearcher( scope, roq ) ) {
						foreach( var mobObj in searcher.Get( ) ) {
							Debug.Assert( null != mobObj, @"WMI Error, null value returned." );
							var mob = (ManagementObject)mobObj;
							cu.Caption = WmiHelpers.GetString( mob, @"Caption" );
							cu.Name = WmiHelpers.GetString( mob, @"Name" );
							cu.Domain = WmiHelpers.GetString( mob, @"Domain" );
						}
					}
				}, false, false );

			} catch( UnauthorizedAccessException ) {
				cu.ConnectionStatus = @"Authorization Error";
			} catch( Exception ) {
				cu.ConnectionStatus = @"Error";
			}
			result.Add( cu );
		}

	}
}
