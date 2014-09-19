using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Remote_Windows_Administrator {
	public partial class frmMain: Form {
		private readonly daw.Collections.SyncList<WmiWin32Product> _dataSource = new daw.Collections.SyncList<WmiWin32Product>( );

		private static DataGridViewColumn MakeColumn( string name, string headerName, bool hidden = false, bool canSort = true ) {
			return new DataGridViewColumn {Name = headerName, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true, DataPropertyName = name, SortMode = (canSort ? DataGridViewColumnSortMode.Automatic: DataGridViewColumnSortMode.NotSortable), CellTemplate = new DataGridViewTextBoxCell(  ), Visible = !hidden};
		}

		public static DataGridViewColumn MakeLinkColumn( string name, string headerName, bool hidden = false, bool canSort = true ) {							
			var cellTemplate = new DataGridViewLinkCell( );
			cellTemplate.TrackVisitedState = false;
			cellTemplate.UseColumnTextForLinkValue = true;
			cellTemplate.LinkBehavior = LinkBehavior.NeverUnderline;			
			var col = MakeColumn( name, headerName, hidden, canSort );
			col.DefaultCellStyle.NullValue = string.Empty;
			col.CellTemplate = cellTemplate;
			return col;				
		}

		public static DataGridViewColumn MakeCheckedColumn( string name, string headerName, bool hidden = false, bool canSort = true ) {
			var col = MakeColumn( name, headerName, hidden, canSort );
			col.DefaultCellStyle.NullValue = false;
			col.CellTemplate = new DataGridViewCheckBoxCell( );
			return col;
		}

		public static DataGridViewColumn MakeDateColumn( string name, string headerName, bool hidden = false, bool canSort = true ) {
			var col = MakeColumn( @"InstallDate", @"Install Date", hidden, canSort );
			col.DefaultCellStyle.Format = @"yyyy/MM/dd";
			return col;
		}		

		private void clearData( ) {
			_dataSource.Clear( );
			_dataSource.ResetBindings( );
		}

		private void QueryRemoveComputer( ) {
			clearData( );
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
			clearData( );
		}

		private void chkShowHidden_CheckedChanged( object sender, EventArgs e ) {
			clearData( );
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

		private string getGuid( int row ) {
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
			
			var strGuid = getGuid( e.RowIndex );
			if( string.IsNullOrEmpty( strGuid ) ) {
				return;
			}			
			
			EventHandler handler = ( Object, EventArgs ) => {
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



		public frmMain( ) {
			InitializeComponent( );
			dgvInstalledPrograms.AutoGenerateColumns = false;
			dgvInstalledPrograms.RowHeadersVisible = true;
			dgvInstalledPrograms.MultiSelect = true;

			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Name", @"Name" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Publisher", @"Publisher" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Version", @"Version" ) );
			dgvInstalledPrograms.Columns.Add( MakeDateColumn( @"InstallDate", @"Install Date" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Size", @"Size(MB)" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Guid", @"Guid", true ) );

			dgvInstalledPrograms.Columns.Add( MakeLinkColumn( @"HelpLink", @"Help Link" ) );
			dgvInstalledPrograms.Columns.Add( MakeLinkColumn( @"UrlInfoAbout", @"About Link" ) );
			dgvInstalledPrograms.Columns.Add( MakeCheckedColumn( @"ShouldHide", @"Hidden" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Comment", @"Comment" ) );

			dgvInstalledPrograms.DataSource = _dataSource;
		}

	}
}
