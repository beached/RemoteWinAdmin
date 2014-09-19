using System.Globalization;
using System.Windows.Forms;
using daw.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using Microsoft.Win32;

namespace Remote_Windows_Administrator {

	public class WmiWin32Product {
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string Version { get; set; }
		public DateTime InstallDate { get; set; }
		public float Size { get; set; }

		public string Guid { get; set; }
		public string HelpLink { get; set; }
		public string Comment { get; set; }

		public WmiWin32Product( ) { }

		public static void UninstallGuidOnComputerName( string computerName, string guid ) {
			MessageBox.Show( string.Format( @"Uninstalling {1} from {0}", guid, computerName ) );
		}

		public static SyncList<WmiWin32Product> FromComputerName( string computerName ) {
			var result = new SyncList<WmiWin32Product>( );
			try {
				using( var regKey = RegistryKey.OpenRemoteBaseKey( RegistryHive.LocalMachine, computerName ).OpenSubKey( @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false ) ) {
					if( null != regKey ) {
						foreach( var currentGuid in regKey.GetSubKeyNames( ) ) {
							if( currentGuid.StartsWith( @"{" ) ) {
								using( var regCurrentPackage = regKey.OpenSubKey( currentGuid, false ) ) {
									if( null != regCurrentPackage ) {
										var currentProduct = new WmiWin32Product( );
										currentProduct.Guid = currentGuid;
										currentProduct.Name = regCurrentPackage.GetValue( @"DisplayName", string.Empty ) as string;
										currentProduct.Publisher = regCurrentPackage.GetValue( @"Publisher", string.Empty ) as string;
										currentProduct.Version = regCurrentPackage.GetValue( @"DisplayVersion", string.Empty ) as string;
										{
											var strDate = regCurrentPackage.GetValue( @"InstallDate", string.Empty ) as string;
											if( !string.IsNullOrEmpty( strDate ) ) {
												string[] dteFormats = { @"yyyy-MM-dd", @"yyyyMMdd" };
												currentProduct.InstallDate = DateTime.ParseExact( strDate, dteFormats, CultureInfo.InvariantCulture, DateTimeStyles.None );
											}
										}
										{
											var currentSize = regCurrentPackage.GetValue( @"EstimatedSize", 0 );
											if( null != currentSize ) {
												var iSize = (Int32)currentSize;
												currentProduct.Size = (float)iSize/(float)(1024.0*1024.0);
											}
										}
										currentProduct.HelpLink = regCurrentPackage.GetValue( @"HelpLink", string.Empty ) as string;
										currentProduct.Comment = regCurrentPackage.GetValue( @"Comment", string.Empty ) as string;
										result.Add( currentProduct );
									}
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
			return result;
		}
	}
}
