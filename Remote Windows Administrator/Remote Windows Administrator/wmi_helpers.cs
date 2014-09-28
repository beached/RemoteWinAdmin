using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Net.NetworkInformation;
using daw;

namespace RemoteWindowsAdministrator {
	public sealed class WmiHelpers {
		/// <summary>
		/// Checks if the hostname is resolvable and pingable
		/// </summary>
		/// <param name="computerName">The test host system</param>
		/// <param name="timeoutMs">How long to wait for a ping replay in ms</param>
		/// <returns></returns>
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

		public static void ForEach( string computerName, string queryString, Func<ManagementObject,bool> func, bool needPrivileges = false, bool expectOne = true ) {
			var conOpt = new ConnectionOptions { Impersonation = ImpersonationLevel.Impersonate, EnablePrivileges = needPrivileges };
			var scope = new ManagementScope( string.Format( @"\\{0}\root\CIMV2", computerName ), conOpt );
			var query = new ObjectQuery( queryString );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( !expectOne || 1 == objSearch.Get( ).Count, string.Format( @"Only expecting one result, {0} found", objSearch.Get( ).Count ) );
				foreach( var obj in objSearch.Get( ) ) {
					Debug.Assert( null != obj, @"WMI Error, null value returned." );
					if( !func( (ManagementObject)obj ) ) {
						break;
					}
				}
			}
		}

		public static void ForEachWithScope( string computerName, string queryString, Func<ManagementObject, ManagementScope, bool> func, bool needPrivileges = false, bool expectOne = true ) {
			var conOpt = new ConnectionOptions { Impersonation = ImpersonationLevel.Impersonate, EnablePrivileges = needPrivileges };
			var scope = new ManagementScope( string.Format( @"\\{0}\root\CIMV2", computerName ), conOpt );
			var query = new ObjectQuery( queryString );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( !expectOne || 1 == objSearch.Get( ).Count, string.Format( @"Only expecting one result, {0} found", objSearch.Get( ).Count ) );
				foreach( var obj in objSearch.Get( ) ) {
					Debug.Assert( null != obj, @"WMI Error, null value returned." );
					if( !func( (ManagementObject)obj, scope ) ) {
						break;
					}
				}
			}
		}

		public static string GetString( ManagementObject mo, string fieldName ) {
			var item = mo[fieldName];
			return null == item ? string.Empty : item.ToString( );
		}

		public static string[] GetStringArray( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return null == item ? new string[] { } : item as string[];
		}

		public static int? GetNullableInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as int?;
		}

		public static int GetInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			Helpers.Assert( null != item, @"GetInt cannot retrieve null values" );
			return (int)item;
		}

		public static uint? GetNullableUInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as uint?;
		}

		public static uint GetUInt( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			Helpers.Assert( null != item, @"GetUInt cannot retrieve null values" );
			return (uint)item;
		}

		public static ushort GetUShort( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			Helpers.Assert( null != item, @"GetUShort cannot retrieve null values" );
			return (ushort)item;
		}

		public static ushort? GetNullableUShort( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as ushort?;
		}

		public static bool GetBoolean( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			Helpers.Assert( null != item, @"GetBoolean cannot retrieve null values" );
			return (bool)item;

		}

		public static bool? GetNullableBoolean( ManagementBaseObject mo, string fieldName ) {
			var item = mo[fieldName];
			return item as bool?;
		}


		/// <summary>
		/// Converts a date/time string in the format yyyyMMddHHmmss.ffffff tttt where
		/// tttt is a signed integer representing the timezone offset in minutes to a
		/// DateTime object
		/// </summary>
		public static DateTime DateTimeFromMsDateTimeString( string value, bool isLocalTime = false ) {
			var tz = Int32.Parse( value.Substring( 21 ) );
			value = value.Substring( 0, 21 );
			var result = DateTime.ParseExact( value, @"yyyyMMddHHmmss.ffffff", CultureInfo.InvariantCulture );
			if( !isLocalTime ) {
				result = result.AddMinutes( tz );
			}
			return result;
		}

		public static DateTime GetDate( ManagementObject mo, string fieldName, bool isLocalTime = false ) {
			var item = mo[fieldName];
			Helpers.Assert( null == item, @"GetDate cannot retrieve null items. Use GetNullableDate" );
			return DateTimeFromMsDateTimeString( item as string, isLocalTime );
		}

		public static DateTime? GetNullableDate( ManagementObject mo, string fieldName, bool isLocalTime = false ) {
			var item = mo[fieldName];
			if( null == item ) {
				return null;
			}
			return DateTimeFromMsDateTimeString( item as string, isLocalTime );
		}


	}
}
