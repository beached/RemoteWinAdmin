using SyncList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MenuItem = System.Windows.Forms.MenuItem;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {
		private SyncList<ComputerInfo> _dsComputerInfo;
		private Int32 _dsComputerInfoThreadCount;
		private SyncList<CurrentUsers> _dsCurrentUsers;
		private Int32 _dsCurrentUsersThreadCount;
		private SyncList<NetworkInfo> _dsNetworkInfo;
		private Int32 _dsNetworkInfoThreadCount;
		private SyncList<ComputerSoftware> _dsSoftware;
		private Int32 _dsSoftwareThreadCount;

		public FrmMain( ) {
			InitializeComponent( );
			
			// Setup Computer Info grid
			_dsComputerInfo = new SyncList<ComputerInfo>( dgvComputerInfo );
			SetDgvDefaults( dgvComputerInfo );
			DgvHelpers.AddColumn( dgvComputerInfo, @"ComputerName", @"Computer Name" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"ConnectionStatus", @"Connection Status" );
			DgvHelpers.AddDateColumn( dgvComputerInfo, @"LastBootTime", @"Boot Time", false, true, MagicValues.TimeDateStringFormat );
			DgvHelpers.AddColumn( dgvComputerInfo, @"Uptime" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"Version", @"Windows Version" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"Architecture" );
			DgvHelpers.AddDateColumn( dgvComputerInfo, @"InstallDate", "Windows\nInstall Date" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"Manufacturer" );
			DgvHelpers.AddDateColumn( dgvComputerInfo, @"HwReleaseDate", @"Hardware Date" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"SerialNumber", @"Serial Number" );
			DgvHelpers.AddColumn( dgvComputerInfo, @"BiosVersion", @"BIOS Version" );
			DgvHelpers.AddButtonColumn( dgvComputerInfo, @"Shutdown" );

			dgvComputerInfo.DataSource = _dsComputerInfo;

			// Setup Currently logged in users grid
			_dsCurrentUsers = new SyncList<CurrentUsers>( dgvCurrentUsers );
			SetDgvDefaults( dgvCurrentUsers );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"ComputerName", @"Computer Name" );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"ConnectionStatus", @"Connection Status" );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"Domain" );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"UserName", @"UserName" );
			DgvHelpers.AddDateColumn( dgvCurrentUsers, @"LastLogon", @"Last Login", false, true, MagicValues.TimeDateStringFormat );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"LogonDuration", @"Login Duration" );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"Sid", @"SID" );
			DgvHelpers.AddColumn( dgvCurrentUsers, @"ProfileFolder", @"Profile" );

			dgvCurrentUsers.DataSource = _dsCurrentUsers;

			// Setup Network Info
			_dsNetworkInfo = new SyncList<NetworkInfo>( dgvNetworkInfo );
			SetDgvDefaults( dgvNetworkInfo );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"ComputerName", @"Computer Name" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"ConnectionStatus", @"Connection Status" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"Caption" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"Description" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"DhcpEnabled", "DHCP\nEnabled" );
			DgvHelpers.AddDateColumn( dgvNetworkInfo, @"DhcpLeaseObtained", "DHCP\nLease Obtained", false, true, MagicValues.TimeDateStringFormat );
			DgvHelpers.AddDateColumn( dgvNetworkInfo, @"DhcpLeaseExpires", "DHCP\nLease Expires", false, true, MagicValues.TimeDateStringFormat );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"DhcpLeaseTimeLeft", "DHCP\nLease Time Left" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"DhcpServer", "DHCP\nServer" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"DnsDomain", @"DNS Domain" );
			DgvHelpers.AddMultilineColumn( dgvNetworkInfo, @"DnsDomainSuffixSearchOrders", "DNS Domain\nSearch Order" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"DnsEnabledForWinsResolution", "DNS Enabled For\nWINS Resolution" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"DnsHostName", @"DNS Host Name" );
			DgvHelpers.AddMultilineColumn( dgvNetworkInfo, @"DnsServerSearchOrders", "DNS Server\nSearch Order" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"DomainDnsRegistrationEnabled", "Domain DNS\nRegistration Enabled" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"DefaultIpGateways", @"Default IP Gateways" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"FullDnsRegistrationEnabled", "Full DNS\nRegistration Enabled" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"MacAddress", @"MAC Address" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"Index" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"InterfaceIndex", @"Interface Index" );
			DgvHelpers.AddMultilineColumn( dgvNetworkInfo, @"IpAddresses", @"IP Addresses" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"IpConnectionMetric", "IP Connection\nMetric" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"IpEnabled", @"IP Enabled" );
			DgvHelpers.AddCheckedColumn( dgvNetworkInfo, @"WinsEnableLmHostsLookup", "WINS Enable\nLM Hosts Lookup" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"WinsHostLookupFile", "WINS Host\nLookup File" );
			DgvHelpers.AddMultilineColumn( dgvNetworkInfo, @"WinsServers", @"WINS Servers" );
			DgvHelpers.AddColumn( dgvNetworkInfo, @"WinsScopeId", "WINS\nScope ID" );

			dgvNetworkInfo.DataSource = _dsNetworkInfo;

			// Setup software grid
			_dsSoftware = new SyncList<ComputerSoftware>( dgvSoftware );
			SetDgvDefaults( dgvSoftware );
			DgvHelpers.AddColumn( dgvSoftware, @"ComputerName", @"Computer Name" );
			DgvHelpers.AddColumn( dgvSoftware, @"ConnectionStatus", @"Connection Status" );
			DgvHelpers.AddColumn( dgvSoftware, @"Name" );
			DgvHelpers.AddColumn( dgvSoftware, @"Publisher" );
			DgvHelpers.AddColumn( dgvSoftware, @"Version" );
			DgvHelpers.AddDateColumn( dgvSoftware, @"InstallDate", @"Install Date" );
			DgvHelpers.AddColumn( dgvSoftware, @"Size", @"Size(MB)" );
			DgvHelpers.AddButtonColumn( dgvSoftware, @"Uninstall" );
			DgvHelpers.AddLinkColumn( dgvSoftware, @"HelpLink", @"Help Link" );
			DgvHelpers.AddLinkColumn( dgvSoftware, @"UrlInfoAbout", @"About Link" );
			DgvHelpers.AddColumn( dgvSoftware, @"Guid" );

			dgvSoftware.DataSource = _dsSoftware;
		}

		private void FrmMain_Shown( object sender, EventArgs e ) {
			txtComputersNameSoftware.Focus( );
		}
	
		private void ClearComputerInfo( ) {
			lock( _dsComputerInfo ) {
				_dsComputerInfo.Clear( );
			}
			txtFilterComputerInfo.Clear( );
			dgvComputerInfo.DataSource = _dsComputerInfo;
		}

		private void ClearCurrentUsers( ) {
			lock( _dsCurrentUsers ) {
				_dsCurrentUsers.Clear( );
			}
			txtFilterCurrentUsers.Clear( );
			dgvCurrentUsers.DataSource = _dsCurrentUsers;
		}

		private void ClearNetworkInfo( ) {
			lock( _dsNetworkInfo ) {
				_dsNetworkInfo.Clear( );
			}
			txtFilterNetworkInfo.Clear( );
			dgvNetworkInfo.DataSource = _dsNetworkInfo;
		}

		private void ClearSoftware( ) {
			lock( _dsSoftware ) {
				_dsSoftware.Clear( );
			}
			txtFilterSoftware.Clear( );
			dgvSoftware.DataSource = _dsSoftware;
		}

		private string GetGuid( int row ) {
			return DgvHelpers.GetCellString( dgvSoftware, row, @"Guid" );
		}

		private void OnStartQueryComputerInfo( ) {
			gbComputersComputerInfo.Enabled = false;
			txtFilterComputerInfo.Enabled = false;
		}

		private void OnStartQueryCurrentUsers( ) {
			gbComputersCurrentUsers.Enabled = false;
			txtFilterCurrentUsers.Enabled = false;
		}

		private void OnStartQueryNetworkInfo( ) {
			gbComputersNetworkInfo.Enabled = false;
			txtFilterNetworkInfo.Enabled = false;
		}

		private void OnStartQuerySoftware( ) {
			gbComputersSoftware.Enabled = false;
			txtFilterSoftware.Enabled = false;
		}

		private void OnEndQueryComputerInfo( ) {
			gbComputersComputerInfo.Enabled = true;
			txtFilterComputerInfo.Enabled = true;
			MessageBox.Show( @"Computer Info Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		private void OnEndQueryCurrentUsers( ) {
			gbComputersCurrentUsers.Enabled = true;
			txtFilterCurrentUsers.Enabled = true;
			MessageBox.Show( @"Current User Info Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		private void OnEndQueryNetworkInfo( ) {
			gbComputersNetworkInfo.Enabled = true;
			txtFilterNetworkInfo.Enabled = true;
			MessageBox.Show( @"Network Info Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		public void OnEndQueryComputerInfoInvoke( ) {
			Invoke( new Action( OnEndQueryComputerInfo ) );
		}

		public void OnEndQueryCurrentUsersInvoke( ) {
			Invoke( new Action( OnEndQueryCurrentUsers ) );
		}

		public void OnEndQueryNetworkInfoInvoke( ) {
			Invoke( new Action( OnEndQueryNetworkInfo ) );
		}

		public void OnEndQuerySoftwareInvoke( ) {
			Invoke( new Action( OnEndQuerySoftware ) );
		}

		private void OnEndQuerySoftware( ) {
			gbComputersSoftware.Enabled = true;
			txtFilterSoftware.Enabled = true;
			MessageBox.Show( @"Computer Software Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		private void QueryComputerInfo( ) {
			ClearComputerInfo( );
			OnStartQueryComputerInfo( );
			var computerNames = GetComputerNamesFromString( txtComputersComputerInfo.Text );
			if( null == computerNames || !computerNames.Any( ) ) {
				return;
			}
			_dsComputerInfoThreadCount = computerNames.Count( );

			foreach( var computerName in computerNames ) {
				var currentName = computerName;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( currentName ) ) {
							ComputerInfo.GetComputerInfo( currentName, ref _dsComputerInfo );
						} else {
							lock( _dsComputerInfo ) {
								_dsComputerInfo.Add( new ComputerInfo { LocalSystemDateTime = DateTime.Now, ComputerName = currentName, ConnectionStatus = @"Connection Error" } );
							}
						}
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsComputerInfoThreadCount ) ) {
							OnEndQueryComputerInfoInvoke( );
						}
					}
				} ).Start( );
			}
		}

		private void QueryCurrentUsers( ) {
			ClearCurrentUsers( );
			OnStartQueryCurrentUsers( );
			var computerNames = GetComputerNamesFromString( txtComputersCurrentUsers.Text );
			if( null == computerNames || !computerNames.Any( ) ) {
				return;
			}
			_dsCurrentUsersThreadCount = computerNames.Count( );

			foreach( var computerName in computerNames ) {
				var currentName = computerName;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( currentName ) ) {
							CurrentUsers.GetCurrentUsers( currentName, ref _dsCurrentUsers );
						} else {
							lock( _dsCurrentUsers ) {
								_dsCurrentUsers.Add( new CurrentUsers( currentName, @"Connection Error" ) );
							}
						}
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsCurrentUsersThreadCount ) ) {
							OnEndQueryCurrentUsersInvoke( );
						}
					}
				} ).Start( );
			}
		}

		private void QueryNetworkInfo( ) {
			ClearNetworkInfo( );
			OnStartQueryNetworkInfo( );
			var computerNames = GetComputerNamesFromString( txtComputersNetworkInfo.Text );
			if( null == computerNames || !computerNames.Any( ) ) {
				return;
			}
			_dsNetworkInfoThreadCount = computerNames.Count( );

			foreach( var computerName in computerNames ) {
				var currentName = computerName;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( currentName ) ) {
							NetworkInfo.GetNetworkInfo( currentName, ref _dsNetworkInfo );
						} else {
							lock( _dsNetworkInfo ) {
								_dsNetworkInfo.Add( new NetworkInfo( currentName, @"Connection Error" ) );
							}
						}
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsNetworkInfoThreadCount ) ) {
							OnEndQueryNetworkInfoInvoke( );
						}
					}
				} ).Start( );
			}
		}

		private void QuerySoftware( ) {
			ClearSoftware( );
			OnStartQuerySoftware( );
			var computerNames = GetComputerNamesFromString( txtComputersNameSoftware.Text );
			if( null == computerNames || !computerNames.Any( ) ) {
				return;
			}
			_dsSoftwareThreadCount = computerNames.Count( );
			foreach( var value in computerNames ) {
				var computerName = value;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( computerName ) ) {
							ComputerSoftware.FromComputerName( computerName, ref _dsSoftware );
						} else {
							lock( _dsSoftware ) {
								_dsSoftware.Add( new ComputerSoftware( computerName, @"Connection Error" ) );
							}
						}
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsSoftwareThreadCount ) ) {
							OnEndQuerySoftwareInvoke( );
						}
					}
				} ).Start( );
			}
		}

		// Helpers
		private static bool ValidateAndSelect( ref DataGridView dgv, DataGridViewCellMouseEventArgs e ) {
			if( null == e || 0 > e.RowIndex || 0 > e.ColumnIndex || dgv.RowCount <= e.RowIndex || dgv.ColumnCount <= e.ColumnIndex ) {
				return false;
			}
			DgvHelpers.SelectCell( ref dgv, e.RowIndex, e.ColumnIndex );
			return true;
		}

		private static ContextMenu MakeCopyLookupMenus( DataGridView dgv, int rowIndex, int columnIndex, IEnumerable<int> excludedColumns ) {
			if( excludedColumns.Contains( columnIndex ) ) {
				return null;
			}

			var menu = new ContextMenu( );
			if( !String.IsNullOrEmpty( DgvHelpers.GetCellString( dgv, rowIndex, columnIndex ).Trim( ) ) ) {
				menu.MenuItems.Add( new MenuItem( string.Format( @"Copy {0}", DgvHelpers.GetColumnName( dgv, columnIndex ) ), delegate { Clipboard.SetText( dgv.Rows[rowIndex].Cells[columnIndex].Value.ToString( ) ); } ) );
			}

			var lookupMenu = new MenuItem( @"Lookup" );

			var clickedColumnName = dgv.Columns[columnIndex].Name;
			foreach( DataGridViewColumn column in dgv.Columns ) {
				if( MagicValues.FieldsToNotLoopup.Contains( column.Name ) ) {
					continue;
				}
				if( 0 != (@"CanSearch").CompareTo( column.Tag ) ) {
					continue;
				}
				if( string.IsNullOrEmpty( dgv.Rows[rowIndex].Cells[column.Index].Value as string ) ) {
					continue;
				}
				var curCellValue = DgvHelpers.GetCellString( dgv, rowIndex, column.Index );
				var menuName = (clickedColumnName == column.Name && !string.IsNullOrEmpty( curCellValue ) ? @"*" : string.Empty) + column.Name;
				lookupMenu.MenuItems.Add( new MenuItem( menuName, delegate { SearchWeb( curCellValue ); } ) );
			}
			if( 0 < lookupMenu.MenuItems.Count ) {
				menu.MenuItems.Add( lookupMenu );
			}
			return menu;
		}

		private static void FilterText<T>( DataGridView dgv, ref SyncList<T> values, string filter ) {
			if( !string.IsNullOrEmpty( filter ) ) {
				var newList = values.AsEnumerable( ).Where( item => ((IContainsString)item).ContainsString( filter ) ).ToList( );
				dgv.DataSource = newList;
			} else {
				dgv.DataSource = values;
			}
		}

		private static IEnumerable<string> GetComputerNamesFromFile( string fileName ) {
			fileName = MagicValues.StripSurroundingDblQuotes( fileName );
			Debug.Assert( IsFile( fileName ), "File does not exist" );
			return MagicValues.DeleniateComputerList( File.ReadAllText( fileName ) );
		}

		private static List<string> GetComputerNamesFromString( string computerNameInfo ) {
			computerNameInfo = computerNameInfo.Trim( );
			if( string.IsNullOrEmpty( computerNameInfo ) ) {
				return new List<string>( );
			}
			var computerNames = MagicValues.DeleniateComputerList( computerNameInfo );
			var result = new List<string>( );
			foreach( var computerName in computerNames ) {
				if( IsFile( computerName ) ) {
					result.AddRange( GetComputerNamesFromFile( computerName ) );
				} else {
					result.Add( computerName );
				}
			}

			return result.OrderBy( name => name ).Distinct( ).ToList( );
		}

		private static bool IsFile( string path ) {
			return File.Exists( MagicValues.StripSurroundingDblQuotes( path ) );
		}

		private static bool IsLink( DataGridViewCell cell ) {
			return cell != null && cell.GetType( ) == typeof( DataGridViewLinkCell );
		}

		private static void OpenLink( DataGridViewCell cell ) {
			if( null == cell ) {
				return;
			}
			OpenLink( cell.Value.ToString( ) );
		}

		private static void OpenLink( string link ) {
			if( !string.IsNullOrEmpty( link ) ) {
				Process.Start( link );
			}
		}

		private static void SearchWeb( string query ) {
			var strQuery = string.Format( @"https://www.google.ca/search?q={0}", System.Web.HttpUtility.UrlEncode( query ) );
			OpenLink( strQuery );
		}

		private static void SetDgvDefaults( DataGridView dgv ) {
			dgv.AutoGenerateColumns = false;
			dgv.RowHeadersVisible = true;
			dgv.MultiSelect = true;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.ReadOnly = true;
			dgv.AutoResizeColumnHeadersHeight( );
			dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
		}

		// Events
		private void btnQueryComputerInfo_Click( object sender, EventArgs e ) {
			QueryComputerInfo( );
		}

		private void btnQueryCurrentUsers_Click( object sender, EventArgs e ) {
			QueryCurrentUsers( );
		}

		private void btnQueryNetworkInfo_Click( object sender, EventArgs e ) {
			QueryNetworkInfo( );
		}

		private void btnQuerySoftware_Click( object sender, EventArgs e ) {
			QuerySoftware( );
		}

		private void dgvComputerInfo_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgvComputerInfo, e ) ) {
				return;
			}

			var colShutdownIndex = DgvHelpers.GetColumnIndex( dgvComputerInfo, @"Shutdown" );
			switch( e.Button ) {
			case MouseButtons.Left: {
					if( e.ColumnIndex == colShutdownIndex ) {
						var computerName = dgvComputerInfo.Rows[e.RowIndex].Cells[@"Computer Name"].Value.ToString( );
						Debug.Assert( null != computerName, @"ComputerName is null.  This should never happen" );
						using( var csd = new ConfirmShutdownDialog( new ComputerInfo.ShutdownComputerParameters( computerName ) ) ) {
							csd.ShowDialog( );
						}
					}
					break;
				}
			case MouseButtons.Right: {
					var ctxMnu = MakeCopyLookupMenus( dgvComputerInfo, e.RowIndex, e.ColumnIndex, new List<int> {
					colShutdownIndex
				} );
					if( null != ctxMnu && 0 < ctxMnu.MenuItems.Count ) {
						ctxMnu.Show( dgvComputerInfo, dgvComputerInfo.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
					break;
				}
			}
		}

		private void dgvCurrentUsers_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgvCurrentUsers, e ) ) {
				return;
			}
			switch( e.Button ) {
			case MouseButtons.Right: {
					if( !String.IsNullOrEmpty( DgvHelpers.GetCellString( dgvCurrentUsers, e.RowIndex, e.ColumnIndex ).Trim( ) ) ) {
						var ctxMnu = new ContextMenu( );
						ctxMnu.MenuItems.Add( new MenuItem( string.Format( @"Copy {0}", DgvHelpers.GetColumnName( dgvCurrentUsers, e.ColumnIndex ) ), delegate { Clipboard.SetText( dgvCurrentUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString( ) ); } ) );
						ctxMnu.Show( dgvCurrentUsers, dgvCurrentUsers.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
					break;
				}
			}
		}

		private void dgvNetworkInfo_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgvNetworkInfo, e ) ) {
				return;
			}
			var curCell = dgvNetworkInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];

			switch( e.Button ) {
			case MouseButtons.Left: {					
					break;
				}
			case MouseButtons.Right: {
					var ctxMnu = MakeCopyLookupMenus( dgvNetworkInfo, e.RowIndex, e.ColumnIndex, new List<int> { } );
					if( null != ctxMnu && 0 < ctxMnu.MenuItems.Count ) {
						ctxMnu.Show( dgvNetworkInfo, dgvNetworkInfo.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
					break;
				}
			}


		}
	
		private void dgvSoftware_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgvSoftware, e ) ) {
				return;
			}
			var curCell = dgvSoftware.Rows[e.RowIndex].Cells[e.ColumnIndex];
			var colUninstallIndex = DgvHelpers.GetColumnIndex( dgvSoftware, @"Uninstall" );

			switch( e.Button ) {
			case MouseButtons.Left: {
					if( colUninstallIndex == e.ColumnIndex ) {
						dgvSoftware.Enabled = false;
						var oldCursor = Cursor;
						Cursor = Cursors.WaitCursor;
						dgvSoftware.Update( );
						try {
							if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
								return;
							}
							var strGuid = GetGuid( e.RowIndex );
							if( string.IsNullOrEmpty( strGuid ) ) {
								throw new Exception( @"No GUID for selected row. All rows must have a GUID" );
							}
							ComputerSoftware.UninstallGuidOnComputerName( txtComputersNameSoftware.Text, strGuid );
							QuerySoftware( );
						} finally {
							dgvSoftware.Enabled = true;
							Cursor = oldCursor;
							dgvSoftware.Update( );
						}
					} else if( IsLink( curCell ) ) {
						OpenLink( curCell );
						return;
					}
					break;
				}
			case MouseButtons.Right: {
					var ctxMnu = MakeCopyLookupMenus( dgvSoftware, e.RowIndex, e.ColumnIndex, new List<int> {
					colUninstallIndex
				} );
					if( null != ctxMnu && 0 < ctxMnu.MenuItems.Count ) {
						ctxMnu.Show( dgvSoftware, dgvSoftware.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
					break;
				}
			}


		}

		private void txtComputersComputerInfo_Enter( object sender, EventArgs e ) {
			AcceptButton = btnQueryComputerInfo;
		}

		private void txtComputersComputerInfo_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void txtComputersComputerInfo_TextChanged( object sender, EventArgs e ) {
			ClearComputerInfo( );
		}

		private void txtComputersCurrentUsers_Enter( object sender, EventArgs e ) {
			AcceptButton = btnQueryCurrentUsers;
		}

		private void txtComputersCurrentUsers_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void txtComputersCurrentUsers_TextChanged( object sender, EventArgs e ) {
			ClearCurrentUsers( );
		}

		private void txtComputersNetworkInfo_Enter( object sender, EventArgs e ) {
			AcceptButton = btnQueryNetworkInfo;
		}

		private void txtComputersNetworkInfo_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void txtComputersNetworkInfo_TextChanged( object sender, EventArgs e ) {
			ClearNetworkInfo( );
		}
	
		private void txtComputersSoftware_Enter( object sender, EventArgs e ) {
			AcceptButton = btnQuerySoftware;
		}

		private void txtComputersSoftware_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void txtComputersSoftware_TextChanged( object sender, EventArgs e ) {
			ClearSoftware( );
		}

		private void txtFilterComputerInfo_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvComputerInfo, ref _dsComputerInfo, txtFilterComputerInfo.Text.Trim( ) );
		}

		private void txtFilterCurrentUsers_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvCurrentUsers, ref _dsCurrentUsers, txtFilterCurrentUsers.Text );
		}

		private void txtFilterNetworkInfo_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvNetworkInfo, ref _dsNetworkInfo, txtFilterNetworkInfo.Text );
		}

		private void txtFilterSoftware_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvSoftware, ref _dsSoftware, txtFilterSoftware.Text.Trim( ) );
		}
	}
}
