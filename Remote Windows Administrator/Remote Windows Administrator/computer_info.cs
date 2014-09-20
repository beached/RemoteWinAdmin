using System;
using System.CodeDom;
using System.Collections.Generic;
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
			var scope = string.Format( @"\\{0}\root\CIMV2", computerName );
			var query = @"SELECT * FROM Win32_OperatingSystem";			

			var ci = new ComputerInfo {LocalSystemDateTime = DateTime.Now, ComputerName = computerName, Status = @"OK"};
			try {
				using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
					Debug.Assert( 1 == objSearch.Get( ).Count );
					foreach( ManagementObject osItem in objSearch.Get( ) ) {
						Debug.Assert( osItem != null, @"Operating System item in WMI was null.  This is not allowed" );
						ci.LastBootTime = GetDate( osItem, @"LastBootUpTime" );
						ci.SystemTime = GetDate( osItem, @"LocalDateTime" );
						ci.Version = GetString( osItem, @"Caption" );
						ci.Architecture = GetString( osItem, @"OSArchitecture" );
					}
				}

				query = @"SELECT * FROM Win32_BIOS";
				using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
					Debug.Assert( 1 == objSearch.Get( ).Count );
					foreach( ManagementObject biosItem in objSearch.Get( ) ) {
						Debug.Assert( biosItem != null, @"BIOS item in WMI was null.  This is not allowed" );
						ci.Manufacturer = GetString( biosItem, @"Manufacturer" );
						ci.HwReleaseDate = GetDate( biosItem, @"ReleaseDate" );
						ci.SerialNumber = GetString( biosItem, @"SerialNumber" );
						ci.BiosVersion = GetString( biosItem, @"SMBIOSBIOSVersion" );
					}
				}
			} catch( UnauthorizedAccessException ) {
				ci.Status = @"Access Denied";
			}

			result.Add( ci );
		}
	}
}
