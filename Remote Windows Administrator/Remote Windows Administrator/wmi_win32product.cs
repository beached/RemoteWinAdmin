using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Remote_Windows_Administrator {

	public class WmiWin32Product: IComparable {
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string Version { get; set; }
		public DateTime? InstallDate { get; set; }
		public float? Size { get; set; }
		public bool CanRemove { get; set; }
		public bool SystemComponent { get; set; }
		public string Guid { get; set; }
		public string HelpLink { get; set; }
		public string Comment { get; set; }
		public string UrlInfoAbout { get; set; }

		public WmiWin32Product( ) { }

		public bool valid( ) {
			return !string.IsNullOrEmpty( Name ) && !string.IsNullOrEmpty( Guid );
		}

		public static void UninstallGuidOnComputerName( string computerName, string guid ) {
			var scope = string.Format( @"\\{0}\root\CIMV2", computerName );
			var query = string.Format( @"SELECT * FROM Win32_Product WHERE IdentifyingNumber='{0}'", guid );
			using( var objSearch = new ManagementObjectSearcher( scope, query ) ) {
				Debug.Assert( 1 == objSearch.Get( ).Count );
				foreach( ManagementObject package in objSearch.Get( ) ) {
					Debug.WriteLine( string.Format( @"Uninstalling '{0}' from {1}", package.Properties["Name"].Value.ToString( ), computerName ) );
					var outParams = package.InvokeMethod( @"Uninstall", null, null );
					var retVal = Int32.Parse( outParams["returnValue"].ToString( ) );
					if( 0 != retVal ) {
						MessageBox.Show( string.Format( @"Error uninstalling '{0}' from {1}. Returned a value of {2}", package.Properties["Name"].Value.ToString( ), computerName, retVal ), @"Error", MessageBoxButtons.OK );
					}
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

		private static DateTime? GetDateTime( RegistryKey rk, string value ) {
			DateTime? result = null;
			var strDteTime = GetString( rk, value );
			if( string.IsNullOrEmpty( strDteTime ) ) {
				return result;
			}
			string[] dteFormats = { @"yyyy-MM-dd", @"yyyyMMdd", @"MM-dd-yyyy" };
			DateTime tmp;
			if( DateTime.TryParseExact( strDteTime, dteFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp ) ) {
				result = tmp;
			}
			return result;
		}

		private static Int32? GetDword( RegistryKey rk, string value, Int32? defaultValue = null ) {
			Int32? result = rk.GetValue( value ) as Int32?;
			if( null == result && null != defaultValue ) {
				result = defaultValue;
			}
			return result;
		}

		private static bool? GetBoolean( RegistryKey rk, string value, bool? defaultValue = null ) {
			bool? result = rk.GetValue( value ) as bool?;
			if( null == result && null != defaultValue ) {
				result = defaultValue;
			}
			return result;
		}

		public bool ShouldHide { get { return IsHidden( ); } }

		private bool IsHidden( bool shown = false ) {
			return !shown && (SystemComponent || !CanRemove);
		}

		public static IEnumerable<WmiWin32Product> FromComputerName( string computerName, bool showHidden = false ) {
			var result = new List<WmiWin32Product>( );
			try {
				string[] regPaths = { @"SOFTWARE\Wow6432node\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" };
				foreach( var currentPath in regPaths ) {
					using( var regKey = RegistryKey.OpenRemoteBaseKey( RegistryHive.LocalMachine, computerName ).OpenSubKey( currentPath, false ) ) {
						if( null == regKey ) {
							continue;
						}
						foreach( var curGuid in regKey.GetSubKeyNames( ).Where( currentGuid => currentGuid.StartsWith( @"{" ) && !HasGuid( result, currentGuid ) ) ) {
							using( var curReg = regKey.OpenSubKey( curGuid, false ) ) {
								if( null == curReg || !string.IsNullOrEmpty( GetString( curReg, @"ParentKeyName" ) ) ) {
									continue;
								}
								var currentProduct = new WmiWin32Product( );
								currentProduct.Guid = curGuid;
								currentProduct.Name = GetString( curReg, @"DisplayName" );
								currentProduct.Publisher = GetString( curReg, @"Publisher" );
								currentProduct.Version = GetString( curReg, @"DisplayVersion" );
								currentProduct.InstallDate = GetDateTime( curReg, @"InstallDate" );
								currentProduct.CanRemove = 0 == GetDword( curReg, @"NoRemove", 0 );
								currentProduct.SystemComponent = 1 == GetDword( curReg, @"SystemComponent", 0 );
								{
									var estSize = GetDword( curReg, @"EstimatedSize" );
									if( null != estSize ) {
										currentProduct.Size = (float)Math.Round( (float)estSize / (float)1024, 2, MidpointRounding.AwayFromZero );
									}
								}
								
								currentProduct.HelpLink = GetString( curReg, @"HelpLink" );
								currentProduct.Comment = GetString( curReg, @"Comment" );
								currentProduct.UrlInfoAbout = GetString( curReg, @"UrlInfoAbout" );
								if( currentProduct.valid( ) && !currentProduct.IsHidden( showHidden ) ) {
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
