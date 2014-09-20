using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using SyncList;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {
		private SyncList<WmiWin32Product> _dsSoftware;
		private SyncList<ComputerInfo> _dsComputerInfo;
		private Int32 _dsComputerInfoThreadCount = 0;
		public FrmMain( ) {
			InitializeComponent( );
			_dsComputerInfo = new SyncList<ComputerInfo>( dgvComputerInfo );
			_dsSoftware = new SyncList<WmiWin32Product>( dgvSoftware );
			setDGVDefaults( ref dgvSoftware );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Name" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Publisher" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Version" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeDateColumn( @"InstallDate", @"Install Date" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Size", @"Size(MB)" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Guid", null, true ) );

			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeLinkColumn( @"HelpLink", @"Help Link" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeLinkColumn( @"UrlInfoAbout", @"About Link" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeCheckedColumn( @"ShouldHide", @"Hidden" ) );
			dgvSoftware.Columns.Add( DataGridViewHelpers.MakeColumn( @"Comment" ) );

			dgvSoftware.DataSource = _dsSoftware;


			setDGVDefaults( ref dgvComputerInfo );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"ComputerName", @"Computer Name" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"Status" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeDateColumn( @"LastBootTime", @"Boot Time", false, true, @"yyyy-MM-dd HH:mm" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"Uptime" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"Version", @"Windows Version" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"Architecture" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"Manufacturer" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeDateColumn( @"HwReleaseDate", @"Hardware Date" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"SerialNumber", @"Serial Number" ) );
			dgvComputerInfo.Columns.Add( DataGridViewHelpers.MakeColumn( @"BiosVersion", @"BIOS Version" ) );

			dgvComputerInfo.DataSource = _dsComputerInfo;
		}

		private void OnStartSoftwareQuery( ) {
			gbComputer.Enabled = false;
			txtFilter.Enabled = false;
		}

		private void OnEndSoftwareQuery( ) {
			gbComputer.Enabled = true;
			txtFilter.Enabled = true;
			MessageBox.Show( @"Computer Software Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		public void OnEndSoftwareQueryInvoke( ) {
			this.Invoke( new Action( OnEndSoftwareQuery ) );
		}

		private void OnStartInfoQuery( ) {
			gbInfoComputerName.Enabled = false;
		}

		private void OnEndInfoQuery( ) {
			gbInfoComputerName.Enabled = true;
			MessageBox.Show( @"Computer Info Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		public void OnEndInfoQueryInvoke( ) {
			this.Invoke(  new Action( OnEndInfoQuery ) );
		}

		private void ClearSofwareData( ) {
			txtFilter.Text = string.Empty;
			_dsSoftware.Clear( );
			dgvSoftware.DataSource = _dsSoftware;			
		}

		private void QueryRemoteComputerSoftware( ) {
			ClearSofwareData( );
			OnStartSoftwareQuery(  );
			var computerName = txtComputerName.Text.Trim( );
			bool showHidden = chkShowHidden.Checked;
			if( WmiWin32Product.IsAlive( computerName ) ) {
				new Thread( ( ) => {
					try {
						WmiWin32Product.FromComputerName( computerName, ref _dsSoftware, showHidden );
					} finally {
						_dsSoftware.ResetBindings( );
						OnEndSoftwareQueryInvoke( );						
					}
				} ).Start( );
			} else {
				MessageBox.Show( @"Could not connect to other computer", @"Alert", MessageBoxButtons.OK );
			}
		}

		private void btnQueryRemoteComputer_Click( object sender, EventArgs e ) {
			QueryRemoteComputerSoftware( );
		}

		private void txtComputerName_TextChanged( object sender, EventArgs e ) {
			ClearSofwareData( );
		}

		private void chkShowHidden_CheckedChanged( object sender, EventArgs e ) {
			ClearSofwareData( );
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
			var cell = dgvSoftware.Rows[row].Cells[column];
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
			var cell = dgvSoftware.Rows[row].Cells[columnName];
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
			Debug.Assert( 0 <= column && dgvSoftware.Columns.Count > column, @"An invalid column number was specified" );
			return dgvSoftware.Columns[column].Name;
		}
		
		private void UnselectAll( ) {
			foreach( DataGridViewRow row in dgvSoftware.Rows ) {
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
			dgvSoftware.Rows[row].Cells[col].Selected = true;
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
				var curCell = dgvSoftware.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
				dgvSoftware.Enabled = false;
				var oldCursor = Cursor;
				Cursor = Cursors.WaitCursor;
				dgvSoftware.Update( );
				try {
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return;
					}
					WmiWin32Product.UninstallGuidOnComputerName( txtComputerName.Text, strGuid );
					QueryRemoteComputerSoftware( );					
				} finally {
					dgvSoftware.Enabled = true;
					Cursor = oldCursor;
					dgvSoftware.Update( );
				}
			};

			EventHandler searchGuidHandler = ( Object, eventArgs ) => SearchWeb( strGuid );
 			var m = new ContextMenu( );
			if( !String.IsNullOrEmpty( GetCellString( e.RowIndex, e.ColumnIndex ).Trim( ) ) ) {
				EventHandler copyCellValueHandler = ( Object, eventArgs ) => Clipboard.SetText( dgvSoftware.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString( ) );
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
			m.Show( dgvSoftware, dgvSoftware.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
		}

		private static void setDGVDefaults( ref DataGridView dgv ) {
			dgv.AutoGenerateColumns = false;
			dgv.RowHeadersVisible = true;
			dgv.MultiSelect = true;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.ReadOnly = true;
		}

		private void FilterText( string filter ) {
			try {
				if( !string.IsNullOrEmpty( filter ) ) {
					var newList = _dsSoftware.AsEnumerable( ).Where( item => item.ContainsString( filter ) ).ToList( );
					dgvSoftware.DataSource = newList;
				} else {
					dgvSoftware.DataSource = _dsSoftware;
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

		private void ClearInfoData( ) {
			_dsComputerInfo.Clear(  );
			dgvComputerInfo.DataSource = _dsComputerInfo;
		}

		private static string GetComputerNamesFromFile( string fileName ) {
			if( fileName.StartsWith( "\"" ) && fileName.EndsWith( "\"" ) ) {
				fileName = fileName.Substring( 1, fileName.Length - 2 );
			}
			Debug.Assert( File.Exists( fileName ), "File does not exist" );
			return File.ReadAllText( fileName );
		}

		private static bool IsFile( string path ) {
			if( string.IsNullOrEmpty( path ) ) {
				return false;
			}
			return File.Exists( path ) || File.Exists( path.Substring( 1, path.Length - 2 ) );
		}

		private void QueryRemoteComputerInfo( ) {
			ClearInfoData( );
			OnStartInfoQuery(  );
			var nameSource = txtInfoComputerName.Text.Trim( );
			if( string.IsNullOrEmpty( nameSource ) ) {
				return;
			}
			if( IsFile( nameSource ) ) {
				nameSource = GetComputerNamesFromFile( nameSource );
			}
			var computerNames = nameSource.Split( new[] {@";", @",", @"	", @" ", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries ).Distinct().Where( item => !string.IsNullOrEmpty( item ) ).ToArray(  );
			_dsComputerInfoThreadCount = computerNames.Count( );

			foreach( var computerName in computerNames ) {
				var currentName = computerName;
				new Thread( ( ) => {
					try {
						if( WmiWin32Product.IsAlive( currentName ) ) {
							ComputerInfo.GetComputerInfo( currentName, ref _dsComputerInfo );
						} else {
							// TODO Better logging so that multiples can be done without interruption and threaded
							_dsComputerInfo.Add( new ComputerInfo {LocalSystemDateTime = DateTime.Now, ComputerName = computerName, Status = @"Connection Error"} );
						}
						_dsComputerInfo.ResetBindings( );
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsComputerInfoThreadCount ) ) {
							OnEndInfoQueryInvoke( );
						}
					}
				} ).Start( );
			}
		}

		private void btnInfoQuery_Click( object sender, EventArgs e ) {
			QueryRemoteComputerInfo(  );
		}

	}
}
