using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using daw.Collections;

namespace Remote_Windows_Administrator {
	public partial class frmMain: Form {
		private daw.Collections.SyncList<WmiWin32Product> _dataSource = new SyncList<WmiWin32Product>( );

		public frmMain( ) {
			InitializeComponent( );			
		}

		private void btnQueryRemoteComputer_Click( object sender, EventArgs e ) {
			_dataSource.Clear( );
			dgvInstalledPrograms.DataSource = null;
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
			if( MouseButtons.Right == e.Button ) {
				var row_guid = dgvInstalledPrograms.Rows[e.RowIndex].Cells["Guid"];
				var ds = dgvInstalledPrograms.DataSource;
				if( null != row_guid ) {
					var strGuid = row_guid.Value;
					EventHandler handler = ( Object, EventArgs ) => {
						dgvInstalledPrograms.Enabled = false;
						try {

							WmiWin32Product.UninstallGuidOnComputerName( txtComputerName.Text, strGuid );

						} finally {
							dgvInstalledPrograms.Enabled = true;
						}
					};
					var m = new ContextMenu( );
					m.MenuItems.Add( new MenuItem( @"Uninstall", handler ) );
					m.Show( dgvInstalledPrograms, new Point( e.Location.X, e.Location.Y ) );
				}
			}
		}

		private void txtComputerName_TextChanged( object sender, EventArgs e ) {
			dgvInstalledPrograms.DataSource = null;
		}
	}
}
