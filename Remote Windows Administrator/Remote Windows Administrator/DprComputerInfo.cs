using System;
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public sealed class DprComputerInfo: IDataPageRow {
		private string _computerName;
		public string ComputerName {
			get { return _computerName; }
			set {
				Helpers.Assert( !string.IsNullOrEmpty( value ), @"Attempt to set ComputerName to a null or empty value" );
				_computerName = value;
			}
		}

		public ConnectionStatuses ConnectionStatus { get; set; }

		public string ConnectionStatusString {
			get { return Helpers.CamelToSpace( ConnectionStatus.ToString( ) ); }
		}

		public DateTime? HwReleaseDate { get; set; }
		public DateTime? InstallDate { get; set; }
		public DateTime? LastBootTime { get; set; }
		public DateTime? LocalSystemDateTime { get; set; }
		public DateTime? SystemTime { get; set; }
		public string Architecture { get; set; }
		public string BiosVersion { get; set; }
		public string Manufacturer { get; set; }
		public string SerialNumber { get; set; }
		public string Version { get; set; }
		public string Uptime {
			get {
				if( null == SystemTime || null == LastBootTime ) {
					return null;
				}
				var span = SystemTime.Value - LastBootTime.Value;
				return MagicValues.TimeSpanToString( span );
			}
		}
		public Guid RowGuid {
			get;
			private set;
		}

		public IDictionary<string, Func<IDataPageRow, bool>> GetActions( ) {
			return SetupActions( );
		}

		public DprComputerInfo( ) {
			ConnectionStatus = ConnectionStatuses.Ok;
			Helpers.Assert( !string.IsNullOrEmpty( ComputerName ), @"ComputerName is required" );
			RowGuid = Guid.NewGuid( );
		}

		public DprComputerInfo( string computerName, ConnectionStatuses connectionStatus = ConnectionStatuses.Ok ) {			
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
			Helpers.Assert( !string.IsNullOrEmpty( ComputerName ), @"ComputerName is required" );
			RowGuid = Guid.NewGuid( );
		}

		public static IDictionary<string, Func<IDataPageRow, bool>> SetupActions( ) {
			return new Dictionary<string, Func<IDataPageRow, bool>>( ) {{@"Shutdown", delegate( IDataPageRow rowObj ) {
				var row = rowObj as DprComputerInfo;
				Helpers.Assert( null != row, @"PtComputerSoftware Action called with another class as second parameter" );
				using( var csd = new ConfirmShutdownDialog( new DprComputerInfo.ShutdownComputerParameters( row.ComputerName ) ) ) {
					csd.ShowDialog( );
				}
				return false;
			}}};
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add( ComputerName ).Add( LocalSystemDateTime ).Add( LastBootTime ).Add( SystemTime ).Add( InstallDate ).Add( Version ).Add( Architecture ).Add( Manufacturer ).Add( HwReleaseDate ).Add( SerialNumber ).Add( BiosVersion ).Add( ConnectionStatus ).Add( Uptime ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( ComputerName );
		}

		public static void Generate( string computerName, SyncList.SyncList<DprComputerInfo> result ) {
			Helpers.Assert( null != result, @"result SyncList cannot be null" );
			Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );
			var ci = new DprComputerInfo( computerName ) { LocalSystemDateTime = DateTime.Now };
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE", obj => {
					ci.LastBootTime = WmiHelpers.GetNullableDate( obj, @"LastBootUpTime" );
					ci.SystemTime = WmiHelpers.GetNullableDate( obj, @"LocalDateTime" );
					ci.Version = WmiHelpers.GetString( obj, @"Caption" );
					ci.Architecture = WmiHelpers.GetString( obj, @"OSArchitecture" );
					ci.InstallDate = WmiHelpers.GetNullableDate( obj, @"InstallDate" );
					return true;
				} );

				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_BIOS", obj => {
					ci.Manufacturer = WmiHelpers.GetString( obj, @"Manufacturer" );
					ci.HwReleaseDate = WmiHelpers.GetNullableDate( obj, @"ReleaseDate" );
					ci.SerialNumber = WmiHelpers.GetString( obj, @"SerialNumber" );
					ci.BiosVersion = WmiHelpers.GetString( obj, @"SMBIOSBIOSVersion" );
					return true;
				} );
			} catch( UnauthorizedAccessException uae ) {
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"Exception - {0} - {1}", uae.TargetSite, uae.Message );
				ci.ConnectionStatus = ConnectionStatuses.AuthorizationError;
			} catch( Exception ex ) {
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, @"Exception - {0} - {1}", ex.TargetSite, ex.Message );
				ci.ConnectionStatus = ConnectionStatuses.Error;
			}
			result.Add( ci );
			ValidateUniqueness( result );
		}

		public static void ValidateUniqueness( SyncList.SyncList<DprComputerInfo> rows ) {
			var guids = new HashSet<System.Guid>( );
			foreach( var item in rows ) {
				Helpers.Assert( !guids.Contains( item.RowGuid ), @"RowGuid's must be unique" );
				guids.Add( item.RowGuid );
			}
		}

		public class ShutdownComputerParameters {
			public string ComputerName { get; set; }
			public UInt32 Timeout { get; set; }

			public enum ShutdownTypes: short {
				Logoff = 0,
				Shutdown = 1,
				Reboot = 2,
				PowerOff = 8
			};

			public static ShutdownTypes ShutdownTypesFromShort( short shutdownType ) {
				return (ShutdownTypes)Enum.ToObject( typeof( ShutdownTypes ), shutdownType );
			}

			internal short Flags {
				get {
					var result = (short)ShutdownType;
					result += Forced ? (short)4 : (short)0;
					return result;
				}
			}

			public ShutdownTypes ShutdownType { get; set; }
			public string Comment { get; set; }
			public bool Forced { get; set; }

			public ShutdownComputerParameters( string computerName ) {
				Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"computerName cannot be empty or null" );
				ComputerName = computerName;
				Timeout = 120;
				ShutdownType = ShutdownTypes.Reboot;
				Comment = @"System Maintenance";
				Forced = false;
			}
		}

		public static void ShutdownComputer( ShutdownComputerParameters parameters ) {
			try {
				const string query = @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE";

				WmiHelpers.ForEach( parameters.ComputerName,  query, obj => {
					var inParams = obj.GetMethodParameters( @"Win32ShutdownTracker" );
					inParams[@"Comment"] = parameters.Comment;
					inParams[@"Flags"] = parameters.Flags;
					inParams[@"ReasonCode"] = 1;	// Maintenance
					inParams[@"Timeout"] = parameters.Timeout;
					var outParams = obj.InvokeMethod( @"Win32ShutdownTracker", inParams, null );
					var result = (null == outParams ? null : outParams[@"ReturnValue"]) as uint?;
					if( null == result || 0 != result ) {
						var message = string.Format( @"Failed to reboot {0} with error {1}", parameters.ComputerName, result );
						GlobalLogging.WriteLine( Logging.LogSeverity.Error, message );
						NotificationWindow.NotificationWindow.AddErrorMessage( message );
					}
					return true;
				}, true );
			} catch( UnauthorizedAccessException ) {
				var message = string.Format( @"Failed to reboot {0}, permission denied", parameters.ComputerName );
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, message );
				NotificationWindow.NotificationWindow.AddErrorMessage( message );
			} catch( ManagementException e ) {
				var message = string.Format( "Failed to reboot {0}, WMI Error\n{1}", parameters.ComputerName, e.Message );
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, message );
				NotificationWindow.NotificationWindow.AddErrorMessage( message );
			} catch( Exception e ) {
				var message = string.Format( "Failed to reboot {0}, unexpected error\n{1}\n{2}", parameters.ComputerName, e.GetType( ).Name, e.Message );
				GlobalLogging.WriteLine( Logging.LogSeverity.Error, message );
				NotificationWindow.NotificationWindow.AddErrorMessage( message );
			}
		}
	}
}
