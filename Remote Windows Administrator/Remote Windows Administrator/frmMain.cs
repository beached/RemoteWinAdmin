using System;
using System.Drawing;
using System.Windows.Forms;

namespace Remote_Windows_Administrator {
	public partial class frmMain: Form {
		private readonly daw.Collections.SyncList<WmiWin32Product> _dataSource = new daw.Collections.SyncList<WmiWin32Product>( );

		public frmMain( ) {
			InitializeComponent( );
			dgvInstalledPrograms.DataSource = _dataSource;
		}

		private void btnQueryRemoteComputer_Click( object sender, EventArgs e ) {
			dgvInstalledPrograms.DataSource = null;
			_dataSource.Clear( );
			foreach( var item in WmiWin32Product.FromComputerName( txtComputerName.Text ) ) {
				_dataSource.Add( item );
			}
			dgvInstalledPrograms.DataSource = _dataSource;

			foreach( DataGridViewColumn curCol in dgvInstalledPrograms.Columns ) {
				curCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			}
			using( var col = dgvInstalledPrograms.Columns[@"InstallDate"] ) {
				col.HeaderText = @"Install Date";
				col.DefaultCellStyle.Format = @"yyyy/MM/dd";
			}
		}

		private void dgvInstalledPrograms_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( MouseButtons.Right != e.Button ) {
				return;
			}
			var rowGuid = dgvInstalledPrograms.Rows[e.RowIndex].Cells["Guid"];
			var ds = dgvInstalledPrograms.DataSource;
			if( null == rowGuid ) {
				return;
			}
			var strGuid = rowGuid.Value as string;
			dgvInstalledPrograms.Rows[e.RowIndex].Selected = true;
			EventHandler handler = ( Object, EventArgs ) => {
				dgvInstalledPrograms.Enabled = false;
				try {
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return;
					}
					WmiWin32Product.UninstallGuidOnComputerName( txtComputerName.Text, strGuid );
					btnQueryRemoteComputer_Click( null, null );
				} finally {
					dgvInstalledPrograms.Enabled = true;
				}
			};
			var m = new ContextMenu( );
			m.MenuItems.Add( new MenuItem( @"Uninstall", handler ) );
			m.Show( dgvInstalledPrograms, dgvInstalledPrograms.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
		}

		private void txtComputerName_TextChanged( object sender, EventArgs e ) {
			_dataSource.Clear( );
		}
	}
}
