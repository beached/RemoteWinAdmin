using System.Net.NetworkInformation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace Remote_Windows_Administrator {

	public class WmiWin32Product: IComparable {
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string Version { get; set; }
		public DateTime InstallDate { get; set; }
		public float Size { get; set; }

		public string Guid { get; set; }
		public string HelpLink { get; set; }
		public string Comment { get; set; }

		public WmiWin32Product( ) { }

		public bool valid( ) {
			return !string.IsNullOrEmpty( Name ) && !string.IsNullOrEmpty( Guid );
		}

		public static void UninstallGuidOnComputerName( string computerName, string guid ) {
			MessageBox.Show( string.Format( @"Uninstalling {1} from {0}", computerName, guid ) );
			var scope = string.Format( @"\\{0}\root\CIMV2", computerName );
			var query = string.Format( @"SELECT * FROM Win32_Product WHERE IdentifyingNumber='{0}'", guid );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( 1 == objSearch.Get( ).Count );
				foreach( var package in objSearch.Get( ) ) {
					Debug.WriteLine( package.Properties["Name"].Value.ToString( ) );
					//var outParams = package.InvokeMethod( @"Uninstall", null, null );
				}
			}
		}

		public static bool IsAlive( string computerName ) {
			var pingSender = new Ping( );
			try {
				var reply = pingSender.Send( computerName, 5000 );
				if( null != reply && IPStatus.Success == reply.Status ) {
					return true;
				}
				Debug.WriteLine( string.Format( @"Ping Status for '{0}' is {1}", computerName, reply.Status ) );
				return false;
			} catch( Exception ) {
				return false;
			}
		}

		private static bool HasGuid( IEnumerable<WmiWin32Product> values, string guid ) {
			return values.Any( currentValue => guid.Equals( currentValue.Guid, StringComparison.OrdinalIgnoreCase ) );
		}

		private static string GetString( RegistryKey rk, string value ) {
			var result = string.Empty;
			var obj = rk.GetValue( value );
			if( null == obj ) {
				return result;
			}
			result = obj as string;
			result = result.Trim( );
			return result;
		}

		private static DateTime GetDateTime( RegistryKey rk, string value ) {
			var result = DateTime.FromBinary( 0 );
			var strDteTime = GetString( rk, value );
			if( string.IsNullOrEmpty( strDteTime ) ) {
				return result;
			}
			string[] dteFormats = { @"yyyy-MM-dd", @"yyyyMMdd" };
			DateTime.TryParseExact( strDteTime, dteFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result );
			return result;
		}

		private static Int32 GetDword( RegistryKey rk, string value ) {
			return (Int32)rk.GetValue( value, 0 );
		}

		public static IEnumerable<WmiWin32Product> FromComputerName( string computerName ) {
			var result = new List<WmiWin32Product>( );
			try {
				string[] regPaths = { @"SOFTWARE\Wow6432node\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" };
				foreach( var currentPath in regPaths ) {
					using( var regKey = RegistryKey.OpenRemoteBaseKey( RegistryHive.LocalMachine, computerName ).OpenSubKey( currentPath, false ) ) {
						if( null == regKey ) {
							continue;
						}
						foreach( var currentGuid in regKey.GetSubKeyNames( ).Where( currentGuid => currentGuid.StartsWith( @"{" ) && !HasGuid( result, currentGuid ) ) ) {
							using( var regCurrentPackage = regKey.OpenSubKey( currentGuid, false ) ) {
								if( null == regCurrentPackage ) {
									continue;
								}
								var currentProduct = new WmiWin32Product( );
								currentProduct.Guid = currentGuid;
								currentProduct.Name = GetString( regCurrentPackage, @"DisplayName" );
								currentProduct.Publisher = GetString( regCurrentPackage, @"Publisher" );
								currentProduct.Version = GetString( regCurrentPackage, @"DisplayVersion" );
								currentProduct.InstallDate = GetDateTime( regCurrentPackage, @"InstallDate" );
								currentProduct.Size = (float)Math.Round( (float)GetDword( regCurrentPackage, @"EstimatedSize" ) / (float)1024, 2, MidpointRounding.AwayFromZero );
								currentProduct.HelpLink = GetString( regCurrentPackage, @"HelpLink" );
								currentProduct.Comment = GetString( regCurrentPackage, @"Comment" );
								if( currentProduct.valid( ) ) {
									result.Add( currentProduct );
								}
							}
						}
					}
				}
			} catch( System.IO.IOException ) {
				MessageBox.Show( @"Error connecting to computer or another IO Error" );
			} catch( System.UnauthorizedAccessException ) {
				MessageBox.Show( @"You do not have permission to open this computer" );
			} catch( System.Security.SecurityException ) {
				MessageBox.Show( @"Security error while connecting" );
			}			
			return result.OrderBy( x => x.Name ).ToList(  );
		}

		public int CompareTo( object obj ) {
			var other = obj as string;
			return String.Compare( Name, other, StringComparison.OrdinalIgnoreCase );
		}
	}
}
