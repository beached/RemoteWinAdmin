using SyncList;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {
		private readonly SyncList<WmiWin32Product> _dataSource = new SyncList<WmiWin32Product>( );

		

		private void ClearData( ) {
			_dataSource.Clear( );
			_dataSource.ResetBindings( );
		}

		private void QueryRemoveComputer( ) {
			ClearData( );
			if( WmiWin32Product.IsAlive( txtComputerName.Text ) ) {
				foreach( var item in WmiWin32Product.FromComputerName( txtComputerName.Text, chkShowHidden.Checked ) ) {
					_dataSource.Add( item );
				}
			} else {
				MessageBox.Show( @"Could not connect to other computer", @"Alert", MessageBoxButtons.OK );
			}
			_dataSource.ResetBindings( );
		}

		private void btnQueryRemoteComputer_Click( object sender, EventArgs e ) {
			QueryRemoveComputer( );
		}

		private void txtComputerName_TextChanged( object sender, EventArgs e ) {
			ClearData( );
		}

		private void chkShowHidden_CheckedChanged( object sender, EventArgs e ) {
			ClearData( );
		}

		private bool IsLink( DataGridViewCell cell ) {
			return cell != null && cell.GetType( ) == typeof( DataGridViewLinkCell );
		}

		private void OpenLink( DataGridViewCell cell ) {
			if( null == cell ) {
				return;
			}
			var strLink = cell.Value.ToString( );
			if( !string.IsNullOrEmpty( strLink ) ) {
				Process.Start( strLink );
			}
		}

		private string GetGuid( int row ) {
			string result = string.Empty;
			if( 0 > row ) {
				return result;
			}
			var rowGuid = dgvInstalledPrograms.Rows[row].Cells["Guid"];
			if( null == rowGuid ) {
				return result;
			}
			var strTmp = rowGuid.Value as string;
			if( !string.IsNullOrEmpty( strTmp ) ) {
				result = strTmp;
			}
			return result;
		}

		private void UnselectAll( ) {
			foreach( DataGridViewRow row in dgvInstalledPrograms.Rows ) {
				row.Selected = false;
			}
		}

		private void SelectRow( int row ) {
			if( 0 > row ) {
				return;
			}
			UnselectAll( );
			dgvInstalledPrograms.Rows[row].Selected = true;
			dgvInstalledPrograms.Update( );
		}

		private void SelectCell( int row, int col ) {
			if( 0 > row || 0 > col ) {
				return;
			}
			UnselectAll( );
			dgvInstalledPrograms.Rows[row].Cells[col].Selected = true;
		}

		private void dgvInstalledPrograms_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( 0 > e.RowIndex || 0 > e.ColumnIndex ) {
				return;
			}
			SelectCell( e.RowIndex, e.ColumnIndex );
			if( MouseButtons.Right != e.Button ) {				
				var curCell = dgvInstalledPrograms.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if( IsLink( curCell ) ) {
					OpenLink( curCell );
				}
				return;
			}
			
			var strGuid = GetGuid( e.RowIndex );
			if( string.IsNullOrEmpty( strGuid ) ) {
				return;
			}			
			
			EventHandler handler = ( Object, eventArgs ) => {
				dgvInstalledPrograms.Enabled = false;
				var oldCursor = Cursor;
				Cursor = Cursors.WaitCursor;
				dgvInstalledPrograms.Update( );
				try {
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return;
					}
					WmiWin32Product.UninstallGuidOnComputerName( txtComputerName.Text, strGuid );
					QueryRemoveComputer( );					
				} finally {
					dgvInstalledPrograms.Enabled = true;
					Cursor = oldCursor;
					dgvInstalledPrograms.Update( );
				}
			};
			var m = new ContextMenu( );
			m.MenuItems.Add( new MenuItem( @"Uninstall", handler ) );

			m.Show( dgvInstalledPrograms, dgvInstalledPrograms.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
		}



		public FrmMain( ) {
			InitializeComponent( );
			dgvInstalledPrograms.AutoGenerateColumns = false;
			dgvInstalledPrograms.RowHeadersVisible = true;
			dgvInstalledPrograms.MultiSelect = true;

			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Name", @"Name" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Publisher", @"Publisher" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Version", @"Version" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeDateColumn( @"InstallDate", @"Install Date" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Size", @"Size(MB)" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Guid", @"Guid", true ) );

			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeLinkColumn( @"HelpLink", @"Help Link" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeLinkColumn( @"UrlInfoAbout", @"About Link" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeCheckedColumn( @"ShouldHide", @"Hidden" ) );
			dgvInstalledPrograms.Columns.Add( DataGridViewHelpers.MakeColumn( @"Comment", @"Comment" ) );

			dgvInstalledPrograms.DataSource = _dataSource;
		}

	}
}
