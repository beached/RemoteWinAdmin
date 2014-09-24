using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace RemoteWindowsAdministrator {
	public class RegistryHelpers {
		public static string GetString( RegistryKey rk, string value ) {
			if( null == rk ) {
				return null;
			}			
			var obj = rk.GetValue( value );
			if( null == obj ) {
				return String.Empty;
			}
			var result = obj as string;
			if( result == null ) {
				return String.Empty;
			}
			result = result.Trim( );
			return result;
		}

		public static string GetString( string computerName, RegistryHive rh, string registryPath, string valueName ) {
			using( var reg = RegistryKey.OpenRemoteBaseKey( rh, computerName ) ) {
				using( var regFolder = reg.OpenSubKey( registryPath ) ) {
					var result = GetString( regFolder, valueName );
					return result;
				}
			}
		}

		public static DateTime? GetDateTime( RegistryKey rk, string value, string[] dateFormats = null ) {
			DateTime? result = null;
			var strDteTime = GetString( rk, value );
			if( String.IsNullOrEmpty( strDteTime ) ) {
				return null;
			}
			if( null == dateFormats ) {
				// There doesn't seem to be a standard.  I maybe ignorant of this though
				dateFormats = new string[] { @"yyyy-MM-dd", @"yyyyMMdd", @"MM-dd-yyyy" };
			}
			DateTime tmp;
			if( DateTime.TryParseExact( strDteTime, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp ) ) {
				result = tmp;
			}
			return result;
		}

		public static Int32? GetDword( RegistryKey rk, string value, Int32? defaultValue = null ) {
			var result = rk.GetValue( value ) as Int32?;
			if( null == result && null != defaultValue ) {
				result = defaultValue;
			}
			return result;
		}
	}
}
