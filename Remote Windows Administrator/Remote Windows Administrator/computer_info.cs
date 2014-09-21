using System;
using System.Management;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public class ComputerInfo {
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

		public static void GetComputerInfo( string computerName, ref SyncList.SyncList<ComputerInfo> result ) {
			var ci = new ComputerInfo { LocalSystemDateTime = DateTime.Now, ComputerName = computerName, Status = @"OK" };
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE", obj => {
					ci.LastBootTime = WmiHelpers.GetDate( obj, @"LastBootUpTime" );
					ci.SystemTime = WmiHelpers.GetDate( obj, @"LocalDateTime" );
					ci.Version = WmiHelpers.GetString( obj, @"Caption" );
					ci.Architecture = WmiHelpers.GetString( obj, @"OSArchitecture" );
				} );

				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_BIOS", obj => {
					ci.Manufacturer = WmiHelpers.GetString( obj, @"Manufacturer" );
					ci.HwReleaseDate = WmiHelpers.GetDate( obj, @"ReleaseDate" );
					ci.SerialNumber = WmiHelpers.GetString( obj, @"SerialNumber" );
					ci.BiosVersion = WmiHelpers.GetString( obj, @"SMBIOSBIOSVersion" );
				} );
			} catch( UnauthorizedAccessException ) {
				ci.Status = @"Access Denied";
			}
			result.Add( ci );
		}

		public static void RebootComputer( string computerName ) {
			try {
				const string query = @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE";
				WmiHelpers.ForEach( computerName,  query, obj => {
					var result = obj.InvokeMethod( @"Reboot", new object[] {} ) as uint?;
					if( 0 != result ) {
						MessageBox.Show( string.Format( @"Failed to reboot {0} with error {1}", computerName, result ) );
					}
				}, true );
			} catch( UnauthorizedAccessException ) {
				MessageBox.Show( string.Format( @"Failed to reboot {0}, permission denied", computerName ) );
			} catch( ManagementException me ) {
				MessageBox.Show( string.Format( "Failed to reboot {0}, WMI Error\n{1}", computerName, me.Message ) );
			} catch( Exception e ) {
				MessageBox.Show( string.Format( "Failed to reboot {0}, unexpected error\n{1}\n{2}", computerName, e.GetType( ).Name, e.Message ) );
			}
		}
	}
}
