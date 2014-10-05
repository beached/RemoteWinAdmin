using Microsoft.Win32;
using SyncList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {

	public sealed class DprComputerSoftware: IDataPageRow {
		private string _computerName;
		public string ComputerName {
			get {
				Helpers.Assert( !string.IsNullOrEmpty( _computerName ), @"Computer name is mandatory and must be set" );
				return _computerName;
			}
			set {
				Helpers.Assert( !string.IsNullOrEmpty( value ), @"Attempt to set ComputerName to a null or empty value" );
				_computerName = value;
			}
		}

		public ConnectionStatuses ConnectionStatus { get; set; }
		public string ConnectionStatusString {
			get { return Helpers.CamelToSpace( ConnectionStatus.ToString( ) ); }
		}


		public DateTime? InstallDate { get; set; }
		public bool CanRemove { get; set; }
		public bool SystemComponent { get; set; }
		public float? Size { get; set; }
		public string Guid { get; set; }
		public string HelpLink { get; set; }
		public string Name { get; set; }
		public string Publisher { get; set; }
		public string UrlInfoAbout { get; set; }
		public string Version { get; set; }
		public System.Guid RowGuid {
			get;
			private set;
		}
		public IDictionary<string, Func<IDataPageRow, bool>> GetActions( ) {
			return SetupActions( );
		}

		public DprComputerSoftware( ) {
			ConnectionStatus = ConnectionStatuses.Ok;
			RowGuid = System.Guid.NewGuid( );
		}

		public DprComputerSoftware( string computerName, ConnectionStatuses connectionStatus = ConnectionStatuses.Ok ) {			
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
			RowGuid = System.Guid.NewGuid( );
		}

		public static Dictionary<string, Func<IDataPageRow, bool>> SetupActions( ) {
			var result = new Dictionary<string, Func<IDataPageRow, bool>> {{@"Uninstall", delegate( IDataPageRow rowObj ) {
				var row = rowObj as DprComputerSoftware;
				Helpers.Assert( null != row, @"PtComputerSoftware Action called with another class as second parameter" );
				Helpers.Assert( !string.IsNullOrEmpty( row.Guid ), @"Guid is empty or null, it is a mandatory field" );
				if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
					return false;
				}
				UninstallGuidOnComputerName( row.ComputerName, row.Guid );
				return true;
			}}};

			return result;
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add(ComputerName).Add( ConnectionStatus).Add( Name ).Add( Publisher ).Add( Version ).Add( InstallDate ).Add( Size ).Add( Guid ).Add( HelpLink ).Add( UrlInfoAbout ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( Name ) && !string.IsNullOrEmpty( Guid ) && !string.IsNullOrEmpty( ComputerName );
		}

		private static bool HasGuid( IEnumerable<DprComputerSoftware> values, string guid ) {
			return values.Any( currentValue => guid.Equals( currentValue.Guid, StringComparison.OrdinalIgnoreCase ) );
		}

// 		public bool ShouldHide { get { return IsHidden( ); } }
// 
// 		private bool IsHidden( bool shown = false ) {
// 			return !shown && SystemComponent;
// 		}

		public static void Generate( string computerName, SyncList<DprComputerSoftware> result ) {
			Debug.Assert( null != result, @"result SyncList cannot be null" );
			Debug.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );
			var softwareList = new List<DprComputerSoftware>();
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
								var currentProduct = new DprComputerSoftware( computerName ) {
									Guid = currentGuid, 
									Name = RegistryHelpers.GetString( curReg, @"DisplayName" ), 
									Publisher = RegistryHelpers.GetString( curReg, @"Publisher" ), 
									Version = RegistryHelpers.GetString( curReg, @"DisplayVersion" ), 
									InstallDate = RegistryHelpers.GetDateTime( curReg, @"InstallDate" ), 
									CanRemove = 0 == RegistryHelpers.GetDword( curReg, @"NoRemove", 0 ), 
									SystemComponent = 1 == RegistryHelpers.GetDword( curReg, @"SystemComponent", 0 )
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
				result.Add( new DprComputerSoftware( computerName, ConnectionStatuses.ConnectionError ) );
				softwareList.Clear( );
			} catch( UnauthorizedAccessException ) {
				result.Add( new DprComputerSoftware( computerName, ConnectionStatuses.AuthorizationError ) );
				softwareList.Clear( );
			} catch( System.Security.SecurityException ) {
				result.Add( new DprComputerSoftware( computerName, ConnectionStatuses.AuthorizationError ) );
				softwareList.Clear( );
			}
			result.AddRange( softwareList );
			ValidateUniqueness( result );
		}

		public static void ValidateUniqueness( SyncList<DprComputerSoftware> rows ) {
			var guids = new HashSet<System.Guid>( );
			foreach( var item in rows ) {
				Helpers.Assert( !guids.Contains( item.RowGuid ), @"RowGuid's must be unique" );
				guids.Add( item.RowGuid );
			}
		}

		public static void UninstallGuidOnComputerName( string computerName, string guid ) {			
			WmiHelpers.ForEach( computerName, string.Format( @"SELECT * FROM Win32_Product WHERE IdentifyingNumber='{0}'", guid ), obj => {
				GlobalLogging.WriteLine( Logging.LogSeverity.Info, @"Uninstalling '{0}' from {1}", obj.Properties["Name"].Value, computerName ) ;
				var outParams = obj.InvokeMethod( @"Uninstall", null, null );
				Debug.Assert( outParams != null, @"Return value from uninstall was null.  This is not allowed" );
				var retVal = int.Parse( outParams[@"returnValue"].ToString( ) );
				if( 0 != retVal ) {
					var message = string.Format( @"Error uninstalling '{0}' from {1}. Returned a value of {2}", obj.Properties["Name"].Value, computerName, retVal );					
					GlobalLogging.WriteLine( Logging.LogSeverity.Warning, message );
					NotificationWindow.NotificationWindow.AddErrorMessage( message );
					return false;	// Stop all on error.  This might be wrong
				}
				return true;
			} );
		}
	}
}
