using System;
using System.Diagnostics;
using System.Globalization;
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

		private static string GetString( ManagementObject mo, string fieldName ) {
			var item = mo[fieldName];
			return null == item ? string.Empty : item.ToString( );
		}

		private static DateTime GetDate( ManagementObject mo, string fieldName ) {
			var strItem = GetString( mo, fieldName );
			var tz = Int32.Parse( strItem.Substring( 21 ) );
			strItem = strItem.Substring( 0, 21 );
			var result = DateTime.ParseExact( strItem, @"yyyyMMddHHmmss.ffffff", CultureInfo.InvariantCulture );
			result = result.AddMinutes( tz );
			return result;
		}

		public static void GetComputerInfo( string computerName, ref SyncList.SyncList<ComputerInfo> result ) {
			var ci = new ComputerInfo { LocalSystemDateTime = DateTime.Now, ComputerName = computerName, Status = @"OK" };
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE", delegate( ManagementObject osItem ) {
					ci.LastBootTime = GetDate( osItem, @"LastBootUpTime" );
					ci.SystemTime = GetDate( osItem, @"LocalDateTime" );
					ci.Version = GetString( osItem, @"Caption" );
					ci.Architecture = GetString( osItem, @"OSArchitecture" );
				} );

				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_BIOS", delegate( ManagementObject biosItem ) {
					ci.Manufacturer = GetString( biosItem, @"Manufacturer" );
					ci.HwReleaseDate = GetDate( biosItem, @"ReleaseDate" );
					ci.SerialNumber = GetString( biosItem, @"SerialNumber" );
					ci.BiosVersion = GetString( biosItem, @"SMBIOSBIOSVersion" );
				} );
			} catch( UnauthorizedAccessException ) {
				ci.Status = @"Access Denied";
			}
			result.Add( ci );
		}

		public static void RebootComputer( string computerName ) {
			try {
				const string query = @"SELECT * FROM Win32_OperatingSystem WHERE Primary=TRUE";
				WmiHelpers.ForEach( computerName,  query, delegate( ManagementObject obj ) {
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
