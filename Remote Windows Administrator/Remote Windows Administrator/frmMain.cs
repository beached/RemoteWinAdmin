using System.Collections.Generic;
using System.Linq;
using SyncList;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {
		private readonly SyncList<WmiWin32Product> _dataSource = new SyncList<WmiWin32Product>( );

		

		private void ClearData( ) {
			_dataSource.Clear( );
			dgvInstalledPrograms.DataSource = _dataSource;			
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
			var filter = txtFilter.Text.Trim( );
			if( !string.IsNullOrEmpty( filter ) ) {
				FilterText( filter );
			}
		}

		private void txtComputerName_TextChanged( object sender, EventArgs e ) {
			ClearData( );
		}

		private void chkShowHidden_CheckedChanged( object sender, EventArgs e ) {
			ClearData( );
		}

		private static bool IsLink( DataGridViewCell cell ) {
			return cell != null && cell.GetType( ) == typeof( DataGridViewLinkCell );
		}

		private static void OpenLink( string link ) {
			if( !string.IsNullOrEmpty( link ) ) {
				Process.Start( link );
			}		
		}

		private static void OpenLink( DataGridViewCell cell ) {
			if( null == cell ) {
				return;
			}
			OpenLink( cell.Value.ToString(  ) );
		}

		private string GetCellString( int row, int column ) {
			var result = string.Empty;
			if( 0 > row ) {
				return result;
			}
			var cell = dgvInstalledPrograms.Rows[row].Cells[column];
			if( null == cell || null == cell.Value ) {
				return result;
			}
			var strTmp = cell.Value.ToString(  );
			if( !string.IsNullOrEmpty( strTmp ) ) {
				result = strTmp;
			}
			return result;
		}

		private string GetCellString( int row, string columnName ) {			
			var result = string.Empty;
			if( 0 > row ) {
				return result;
			}
			var cell = dgvInstalledPrograms.Rows[row].Cells[columnName];
			if( null == cell || null == cell.Value ) {
				return result;
			}
			var strTmp = cell.Value.ToString(  );
			if( !string.IsNullOrEmpty( strTmp ) ) {
				result = strTmp;
			}
			return result;
		}


		private string GetGuid( int row ) {
			return GetCellString( row, @"Guid" );
		}

		private string GetProgram( int row ) {
			return GetCellString( row, @"Name" );
		}

		private string GetPublisher( int row ) {
			return GetCellString( row, @"Publisher" );
		}

		private string GetColumnName( int column ) {
			Debug.Assert( 0 <= column && dgvInstalledPrograms.Columns.Count > column, @"An invalid column number was specified" );
			return dgvInstalledPrograms.Columns[column].Name;
		}
		
		private void UnselectAll( ) {
			foreach( DataGridViewRow row in dgvInstalledPrograms.Rows ) {
				row.Selected = false;
			}
		}

// 		private void SelectRow( int row ) {
// 			if( 0 > row ) {
// 				return;
// 			}
// 			UnselectAll( );
// 			dgvInstalledPrograms.Rows[row].Selected = true;
// 			dgvInstalledPrograms.Update( );
// 		}

		private void SelectCell( int row, int col ) {
			if( 0 > row || 0 > col ) {
				return;
			}
			UnselectAll( );
			dgvInstalledPrograms.Rows[row].Cells[col].Selected = true;
		}

		private void SearchWeb( string query ) {
			var strQuery = string.Format( @"https://www.google.ca/search?q={0}", System.Web.HttpUtility.UrlEncode( query ) );
			OpenLink( strQuery );
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
			
			EventHandler uninstallHandler = ( Object, eventArgs ) => {
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

			EventHandler searchGuidHandler = ( Object, eventArgs ) => SearchWeb( strGuid );
 			var m = new ContextMenu( );
			if( !String.IsNullOrEmpty( GetCellString( e.RowIndex, e.ColumnIndex ).Trim( ) ) ) {
				EventHandler copyCellValueHandler = ( Object, eventArgs ) => Clipboard.SetText( dgvInstalledPrograms.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString( ) );
				m.MenuItems.Add( new MenuItem( string.Format( @"Copy {0}", GetColumnName( e.ColumnIndex ) ), copyCellValueHandler ) );
			}
			m.MenuItems.Add( new MenuItem( @"Uninstall", uninstallHandler ) );
			var lookupMenu = new MenuItem( @"Lookup" );
			lookupMenu.MenuItems.Add( new MenuItem( @"GUID", searchGuidHandler ) );
			{
				var programName = GetProgram( e.RowIndex );
				if( !string.IsNullOrEmpty( programName ) ) {
					EventHandler searchProgramHandler = ( Object, eventArgs ) => SearchWeb( programName );
					lookupMenu.MenuItems.Add( new MenuItem( @"Program Name", searchProgramHandler ) );
				}
			}
			{
				var publisherName = GetPublisher( e.RowIndex );
				if( !string.IsNullOrEmpty( publisherName ) ) {
					EventHandler searchPublisherHandler = ( Object, eventArgs ) => SearchWeb( publisherName );
					lookupMenu.MenuItems.Add( new MenuItem( @"Publisher", searchPublisherHandler ) );
				}
			}
			m.MenuItems.Add( lookupMenu );
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

		private void FilterText( string filter ) {
			try {
				if( !string.IsNullOrEmpty( filter ) ) {
					var newList = _dataSource.AsEnumerable( ).Where( item => item.ContainsString( filter ) ).ToList( );
					dgvInstalledPrograms.DataSource = newList;
				} else {
					dgvInstalledPrograms.DataSource = _dataSource;
				}

			} catch( Exception ex ) {
				new ToolTip( ).SetToolTip( txtFilter, ex.Message );
			}			
		}

		private void txtFilter_TextChanged( object sender, EventArgs e ) {
			FilterText( txtFilter.Text.Trim( ) );
		}

		private void FrmMain_Shown( object sender, EventArgs e ) {
			txtComputerName.Focus( );
		}

	}
}
