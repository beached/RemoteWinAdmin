using System;
using System.Management;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public class ComputerInfo: IContainsString {
		public string ComputerName { get; set; }
		public DateTime? LocalSystemDateTime { get; set; }
		public DateTime? LastBootTime { get; set; }
		public DateTime? SystemTime { get; set; }
		public string Version { get; set; }
		public string Architecture { get; set; }
		public string Manufacturer { get; set; }
		public DateTime? HwReleaseDate { get; set; }
		public string SerialNumber { get; set; }
		public string BiosVersion { get; set; }
		public string Status { get; set; }
		public string Uptime {
			get {
				if( null == SystemTime || null == LastBootTime ) {
					return null;
				}
				var span = SystemTime.Value - LastBootTime.Value;
				return string.Format( @"{0} days {1}:{2}hrs", span.Days, span.Hours, span.Minutes );
			}
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Test( ComputerName ).Test( LocalSystemDateTime ).Test( LastBootTime ).Test( SystemTime ).Test( Version ).Test( Architecture ).Test( Manufacturer ).Test( HwReleaseDate ).Test( SerialNumber ).Test( BiosVersion ).Test( Status ).Test( Uptime ).IsContained;
		}

		public static void GetComputerInfo( string computerName, ref SyncList.SyncList<ComputerInfo> result ) {
			var ci = new ComputerInfo { LocalSystemDateTime = DateTime.Now, ComputerName = computerName, Status = @"OK" };
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE", obj => {
					ci.LastBootTime = WmiHelpers.GetDate( obj, @"LastBootUpTime" );
					ci.SystemTime = WmiHelpers.GetDate( obj, @"LocalDateTime" );
					ci.Version = WmiHelpers.GetString( obj, @"Caption" );
					ci.Architecture = WmiHelpers.GetString( obj, @"OSArchitecture" );
					return true;
				} );

				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_BIOS", obj => {
					ci.Manufacturer = WmiHelpers.GetString( obj, @"Manufacturer" );
					ci.HwReleaseDate = WmiHelpers.GetDate( obj, @"ReleaseDate" );
					ci.SerialNumber = WmiHelpers.GetString( obj, @"SerialNumber" );
					ci.BiosVersion = WmiHelpers.GetString( obj, @"SMBIOSBIOSVersion" );
					return true;
				} );
			} catch( UnauthorizedAccessException ) {
				ci.Status = @"Authorization Error";
			} catch( Exception ) {
				ci.Status = @"Error";
			}
			result.Add( ci );
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
				if( string.IsNullOrEmpty( computerName ) ) {
					throw new ArgumentNullException( @"computerName", @"compueterName cannot be empty or null" );
				}
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
						MessageBox.Show( string.Format( @"Failed to reboot {0} with error {1}", parameters.ComputerName, result ) );
					}
					return true;
				}, true );
			} catch( UnauthorizedAccessException ) {
				MessageBox.Show( string.Format( @"Failed to reboot {0}, permission denied", parameters.ComputerName ) );
			} catch( ManagementException e ) {
				MessageBox.Show( string.Format( "Failed to reboot {0}, WMI Error\n{1}", parameters.ComputerName, e.Message ) );
			} catch( Exception e ) {
				MessageBox.Show( string.Format( "Failed to reboot {0}, unexpected error\n{1}\n{2}", parameters.ComputerName, e.GetType( ).Name, e.Message ) );
			}
		}
	}
}
