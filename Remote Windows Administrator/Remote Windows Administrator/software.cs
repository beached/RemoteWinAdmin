using Microsoft.Win32;
using SyncList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {

	public class ComputerSoftware: IDataPageRow {
		public string ComputerName { get; set; }
		public string ConnectionStatus { get; set; }
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string Version { get; set; }
		public DateTime? InstallDate { get; set; }
		public float? Size { get; set; }
		public bool CanRemove { get; set; }
		public bool SystemComponent { get; set; }
		public string Guid { get; set; }
		public string HelpLink { get; set; }
		public string UrlInfoAbout { get; set; }


		public ComputerSoftware( string computerName, string connectionStatus = @"OK" ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add(ComputerName).Add( ConnectionStatus).Add( Name ).Add( Publisher ).Add( Version ).Add( InstallDate ).Add( Size ).Add( Guid ).Add( IsHidden( ) ).Add( HelpLink ).Add( UrlInfoAbout ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( Name ) && !string.IsNullOrEmpty( Guid );
		}

		private static bool HasGuid( IEnumerable<ComputerSoftware> values, string guid ) {
			return values.Any( currentValue => guid.Equals( currentValue.Guid, StringComparison.OrdinalIgnoreCase ) );
		}

		public bool ShouldHide { get { return IsHidden( ); } }

		private bool IsHidden( bool shown = false ) {
			return !shown && SystemComponent;
		}

		public static void FromComputerName( string computerName, ref SyncList<ComputerSoftware> result ) {
			Debug.Assert( null != result, @"result SyncList cannot be null" );
			Debug.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );
			var softwareList = new List<ComputerSoftware>();
			try {
				string[] regPaths = {
					@"SOFTWARE\Wow6432node\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
				};
				foreach( var currentPath in regPaths ) {
					using( var regKey = RegistryKey.OpenRemoteBaseKey( RegistryHive.LocalMachine, computerName ).OpenSubKey( currentPath, false ) ) {
						if( null == regKey ) {
							continue;
						}
						foreach( var currentGuid in regKey.GetSubKeyNames( ).Where( currentValue => currentValue.StartsWith( @"{" ) ).Where( currentGuid => !HasGuid( softwareList, currentGuid ) ) ) {
							using( var curReg = regKey.OpenSubKey( currentGuid, false ) ) {
								if( null == curReg || !string.IsNullOrEmpty( RegistryHelpers.GetString( curReg, @"ParentKeyName" ) ) ) {
									continue;
								}
								var currentProduct = new ComputerSoftware( computerName ) {
									Guid = currentGuid, Name = RegistryHelpers.GetString( curReg, @"DisplayName" ), Publisher = RegistryHelpers.GetString( curReg, @"Publisher" ), Version = RegistryHelpers.GetString( curReg, @"DisplayVersion" ), InstallDate = RegistryHelpers.GetDateTime( curReg, @"InstallDate" ), CanRemove = 0 == RegistryHelpers.GetDword( curReg, @"NoRemove", 0 ), SystemComponent = 1 == RegistryHelpers.GetDword( curReg, @"SystemComponent", 0 )
								};
								{
									var estSize = RegistryHelpers.GetDword( curReg, @"EstimatedSize" );
									if( null != estSize ) {
										currentProduct.Size = (float)Math.Round( (float)estSize / 1024.0, 2, MidpointRounding.AwayFromZero );
									}
								}

								currentProduct.HelpLink = RegistryHelpers.GetString( curReg, @"HelpLink" );
								currentProduct.UrlInfoAbout = RegistryHelpers.GetString( curReg, @"UrlInfoAbout" );
								if( currentProduct.Valid( ) ) {
									softwareList.Add( currentProduct );
								}
							}
						}
					}
				}
			} catch( System.IO.IOException ) {
				result.Add( new ComputerSoftware( computerName, @"Connection Error" ) );
				softwareList.Clear( );
			} catch( UnauthorizedAccessException ) {
				result.Add( new ComputerSoftware( computerName, @"Authorization Error" ) );
				softwareList.Clear( );
			} catch( System.Security.SecurityException ) {
				result.Add( new ComputerSoftware( computerName, @"Authorization Error" ) );
				softwareList.Clear( );
			}
			result.AddRange( softwareList );
		}

		public static void UninstallGuidOnComputerName( string computerName, string guid ) {			
			WmiHelpers.ForEach( computerName, string.Format( @"SELECT * FROM Win32_Product WHERE IdentifyingNumber='{0}'", guid ), obj => {
				Debug.WriteLine( string.Format( @"Uninstalling '{0}' from {1}", obj.Properties["Name"].Value, computerName ) );
				var outParams = obj.InvokeMethod( @"Uninstall", null, null );
				Debug.Assert( outParams != null, @"Return value from uninstall was null.  This is not allowed" );
				var retVal = int.Parse( outParams[@"returnValue"].ToString( ) );
				if( 0 != retVal ) {
					MessageBox.Show( string.Format( @"Error uninstalling '{0}' from {1}. Returned a value of {2}", obj.Properties["Name"].Value, computerName, retVal ), @"Error", MessageBoxButtons.OK );
					return false;	// Stop all on error.  This might be wrong
				}
				return true;
			} );
		}
	}
}
