using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Net.NetworkInformation;

namespace RemoteWindowsAdministrator {
	public sealed class WmiHelpers {
		public static bool IsAlive( string computerName, int timeoutMs = 3000 ) {
			try {
				using( var pingSender = new Ping( ) ) {
					var reply = pingSender.Send( computerName, timeoutMs );
					if( null != reply && IPStatus.Success == reply.Status ) {
						return true;
					}
					Debug.Assert( reply != null, "Ping reply was null, this shouldn't happen" );
					Debug.WriteLine( string.Format( @"Ping Status for '{0}' is {1}", computerName, reply.Status ) );
					return false;
				}
			} catch( Exception ) {
				return false;
			}
		}

		public static void ForEach( string computerName, string queryString, Action<ManagementObject> action, bool needPrivileges = false, bool expectOne = true ) {
			var conOpt = new ConnectionOptions { Impersonation = ImpersonationLevel.Impersonate, EnablePrivileges = needPrivileges };
			var scope = new ManagementScope( string.Format( @"\\{0}\root\CIMV2", computerName ), conOpt );
			var query = new ObjectQuery( queryString );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( !expectOne || 1 == objSearch.Get( ).Count, string.Format( @"Only expecting one result, {0} found", objSearch.Get( ).Count ) );
				foreach( var obj in objSearch.Get( ) ) {
					Debug.Assert( null != obj, @"WMI Error, null value returned." );
					action( (ManagementObject)obj );
				}
			}
		}

		public static void ForEachWithScope( string computerName, string queryString, Action<ManagementObject, ManagementScope> action, bool needPrivileges = false, bool expectOne = true ) {
			var conOpt = new ConnectionOptions { Impersonation = ImpersonationLevel.Impersonate, EnablePrivileges = needPrivileges };
			var scope = new ManagementScope( string.Format( @"\\{0}\root\CIMV2", computerName ), conOpt );
			var query = new ObjectQuery( queryString );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( !expectOne || 1 == objSearch.Get( ).Count, string.Format( @"Only expecting one result, {0} found", objSearch.Get( ).Count ) );
				foreach( var obj in objSearch.Get( ) ) {
					Debug.Assert( null != obj, @"WMI Error, null value returned." );
					action( (ManagementObject)obj, scope );
				}
			}
		}

		public static string GetString( ManagementObject mo, string fieldName ) {
			var item = mo[fieldName];
			return null == item ? string.Empty : item.ToString( );
		}

		public static int? GetNullableInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as int?;
		}

		public static int GetInt( ManagementBaseObject mo, string fieldName ) {
			return (int)mo[fieldName];
		}

		public static uint? GetNullableUInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as uint?;
		}

		public static uint GetUInt( ManagementBaseObject mo, string fieldName ) {
			return (uint)mo[fieldName];
		}


		public static DateTime GetDate( ManagementObject mo, string fieldName ) {
			var strItem = GetString( mo, fieldName );
			var tz = Int32.Parse( strItem.Substring( 21 ) );
			strItem = strItem.Substring( 0, 21 );
			var result = DateTime.ParseExact( strItem, @"yyyyMMddHHmmss.ffffff", CultureInfo.InvariantCulture );
			result = result.AddMinutes( tz );
			return result;
		}

		public static DateTime? GetNullableDate( ManagementObject mo, string fieldName ) {
			var strItem = GetString( mo, fieldName );
			if( string.IsNullOrEmpty( strItem ) ) {
				return null;
			}
			var tz = Int32.Parse( strItem.Substring( 21 ) );
			strItem = strItem.Substring( 0, 21 );
			var result = DateTime.ParseExact( strItem, @"yyyyMMddHHmmss.ffffff", CultureInfo.InvariantCulture );
			result = result.AddMinutes( tz );
			return result;
		}


	}
}
