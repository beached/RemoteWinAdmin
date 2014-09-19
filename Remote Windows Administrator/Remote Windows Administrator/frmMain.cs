using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Remote_Windows_Administrator {
	public partial class frmMain: Form {
		private readonly daw.Collections.SyncList<WmiWin32Product> _dataSource = new daw.Collections.SyncList<WmiWin32Product>( );

		private static DataGridViewColumn MakeColumn( string name, string headerName ) {
			return new DataGridViewColumn {Name = headerName, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true, DataPropertyName = name, SortMode = DataGridViewColumnSortMode.NotSortable, CellTemplate = new DataGridViewTextBoxCell(  )};
		}

		public frmMain( ) {
			InitializeComponent( );
			dgvInstalledPrograms.AutoGenerateColumns = false;
			dgvInstalledPrograms.RowHeadersVisible = false;

			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Name", @"Name" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Publisher", @"Publisher" ) );
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Version", @"Version" ) );
			{
				var colDate = MakeColumn( @"InstallDate", @"Install Date" );
				colDate.DefaultCellStyle.Format = @"yyyy/MM/dd";
				colDate.DefaultCellStyle.NullValue = DateTime.FromBinary( 0 );
				dgvInstalledPrograms.Columns.Add( colDate );
			}
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Size", @"Size(MB)" ) );
			{
				var colGuid = MakeColumn( @"Guid", @"Guid" );
				colGuid.Visible = false;
				dgvInstalledPrograms.Columns.Add( colGuid );
			}
			{
				var colHelpLink = MakeColumn( @"HelpLink", @"Help Link" );
				var cellTemplate = new DataGridViewLinkCell( );
				cellTemplate.TrackVisitedState = false;
				cellTemplate.UseColumnTextForLinkValue = true;
				cellTemplate.LinkBehavior = LinkBehavior.NeverUnderline;
				
				colHelpLink.CellTemplate = cellTemplate;
				
				dgvInstalledPrograms.Columns.Add( colHelpLink );
			}
			dgvInstalledPrograms.Columns.Add( MakeColumn( @"Comment", @"Comment" ) );
			
			dgvInstalledPrograms.DataSource = _dataSource;								
		}

		private void btnQueryRemoteComputer_Click( object sender, EventArgs e ) {
			_dataSource.Clear( );
			
			foreach( var item in WmiWin32Product.FromComputerName( txtComputerName.Text ) ) {
				_dataSource.Add( item );
			}

			_dataSource.ResetBindings( );
		}

		private void dgvInstalledPrograms_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( MouseButtons.Right != e.Button ) {
				var curCol = dgvInstalledPrograms.Columns[e.ColumnIndex];
				var colHelp = dgvInstalledPrograms.Columns["Help Link"];
				Debug.Assert( null != colHelp, "HelpLink column is missing" );

				if( curCol.CellType == null || curCol.CellType != colHelp.CellType ) {
					return;
				}
				var strLink = dgvInstalledPrograms.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString( );
				if( !string.IsNullOrEmpty( strLink ) ) {
					Process.Start( strLink );
				}
				return;
			}
			var rowGuid = dgvInstalledPrograms.Rows[e.RowIndex].Cells["Guid"];
			var ds = dgvInstalledPrograms.DataSource;
			if( null == rowGuid ) {
				return;
			}
			dgvInstalledPrograms.Rows[e.RowIndex].Selected = true;
			dgvInstalledPrograms.Update(  );
			var strGuid = rowGuid.Value as string;			
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
