using System.Collections.Generic;
using SyncList;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {
		private SyncList<Win32Product> _dsSoftware;
		private SyncList<ComputerInfo> _dsComputerInfo;
		private Int32 _dsComputerInfoThreadCount;

		public FrmMain( ) {
			InitializeComponent( );
			_dsComputerInfo = new SyncList<ComputerInfo>( dgvComputerInfo );
			_dsSoftware = new SyncList<Win32Product>( dgvSoftware );
			SetDgvDefaults( dgvSoftware );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Name" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Publisher" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Version" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeDateColumn( @"InstallDate", @"Install Date" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Size", @"Size(MB)" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Guid", null, true ) );

			dgvSoftware.Columns.Add( DgvHelpers.MakeLinkColumn( @"HelpLink", @"Help Link" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeLinkColumn( @"UrlInfoAbout", @"About Link" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeCheckedColumn( @"ShouldHide", @"Hidden" ) );
			dgvSoftware.Columns.Add( DgvHelpers.MakeColumn( @"Comment" ) );

			dgvSoftware.DataSource = _dsSoftware;


			SetDgvDefaults( dgvComputerInfo );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"ComputerName", @"Computer Name" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"Status" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeDateColumn( @"LastBootTime", @"Boot Time", false, true, @"yyyy-MM-dd HH:mm" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"Uptime" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"Version", @"Windows Version" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"Architecture" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"Manufacturer" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeDateColumn( @"HwReleaseDate", @"Hardware Date" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"SerialNumber", @"Serial Number" ) );
			dgvComputerInfo.Columns.Add( DgvHelpers.MakeColumn( @"BiosVersion", @"BIOS Version" ) );
			{
				var colReboot = new DataGridViewButtonColumn( );
				colReboot.Name = @"Reboot";
				colReboot.HeaderText = string.Empty;
				colReboot.Text = @"Reboot";
				colReboot.UseColumnTextForButtonValue = true;
				dgvComputerInfo.Columns.Add( colReboot );
			}
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
			Invoke( new Action( OnEndSoftwareQuery ) );
		}

		private void OnStartInfoQuery( ) {
			gbInfoComputerName.Enabled = false;
		}

		private void OnEndInfoQuery( ) {
			gbInfoComputerName.Enabled = true;
			MessageBox.Show( @"Computer Info Query Complete", @"Complete", MessageBoxButtons.OK );
		}

		public void OnEndInfoQueryInvoke( ) {
			Invoke( new Action( OnEndInfoQuery ) );
		}

		private void ClearSofwareData( ) {
			txtFilter.Text = string.Empty;
			_dsSoftware.Clear( );
			dgvSoftware.DataSource = _dsSoftware;
		}

		private void QueryRemoteComputerSoftware( ) {
			ClearSofwareData( );
			OnStartSoftwareQuery( );
			var computerName = txtComputerName.Text.Trim( );
			bool showHidden = chkShowHidden.Checked;
			if( WmiHelpers.IsAlive( computerName ) ) {
				new Thread( ( ) => {
					try {
						Win32Product.FromComputerName( computerName, ref _dsSoftware, showHidden );
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
			OpenLink( cell.Value.ToString( ) );
		}

		private string GetGuid( int row ) {
			return DgvHelpers.GetCellString( dgvSoftware, row, @"Guid" );
		}

		private string GetProgram( int row ) {
			return DgvHelpers.GetCellString( dgvSoftware, row, @"Name" );
		}

		private string GetPublisher( int row ) {
			return DgvHelpers.GetCellString( dgvSoftware, row, @"Publisher" );
		}

		private static void SearchWeb( string query ) {
			var strQuery = string.Format( @"https://www.google.ca/search?q={0}", System.Web.HttpUtility.UrlEncode( query ) );
			OpenLink( strQuery );
		}

		private void dgvInstalledPrograms_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( 0 > e.RowIndex || 0 > e.ColumnIndex ) {
				return;
			}
			DgvHelpers.SelectCell( dgvSoftware, e.RowIndex, e.ColumnIndex );
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

			var m = new ContextMenu( );
			if( !String.IsNullOrEmpty( DgvHelpers.GetCellString( dgvSoftware, e.RowIndex, e.ColumnIndex ).Trim( ) ) ) {
				m.MenuItems.Add( new MenuItem( string.Format( @"Copy {0}", DgvHelpers.GetColumnName( dgvSoftware, e.ColumnIndex ) ), delegate {
					Clipboard.SetText( dgvSoftware.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString( ) );
				} ) );
			}
			m.MenuItems.Add( new MenuItem( @"Uninstall", delegate {
				dgvSoftware.Enabled = false;
				var oldCursor = Cursor;
				Cursor = Cursors.WaitCursor;
				dgvSoftware.Update( );
				try {
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return;
					}
					Win32Product.UninstallGuidOnComputerName( txtComputerName.Text, strGuid );
					QueryRemoteComputerSoftware( );
				} finally {
					dgvSoftware.Enabled = true;
					Cursor = oldCursor;
					dgvSoftware.Update( );
				}
			} ) );

			var lookupMenu = new MenuItem( @"Lookup" );
			lookupMenu.MenuItems.Add( new MenuItem( @"GUID", delegate { SearchWeb( strGuid ); } ) );
			{
				var programName = GetProgram( e.RowIndex );
				if( !string.IsNullOrEmpty( programName ) ) {
					lookupMenu.MenuItems.Add( new MenuItem( @"Program Name", delegate { SearchWeb( programName ); } ) );
				}
			}
			{
				var publisherName = GetPublisher( e.RowIndex );
				if( !string.IsNullOrEmpty( publisherName ) ) {
					lookupMenu.MenuItems.Add( new MenuItem( @"Publisher", delegate { SearchWeb( publisherName ); } ) );
				}
			}
			m.MenuItems.Add( lookupMenu );
			m.Show( dgvSoftware, dgvSoftware.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
		}

		private static void SetDgvDefaults( DataGridView dgv ) {
			dgv.AutoGenerateColumns = false;
			dgv.RowHeadersVisible = true;
			dgv.MultiSelect = true;
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.ReadOnly = true;
		}

		private static void FilterText<T>( DataGridView dgv, ref SyncList<T> values, string filter ) {
			if( !string.IsNullOrEmpty( filter ) ) {
				var newList = values.AsEnumerable( ).Where( item => ((IContainsString)item).ContainsString( filter ) ).ToList( );
				dgv.DataSource = newList;
			} else {
				dgv.DataSource = values;
			}
		}

		private void txtFilter_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvSoftware, ref _dsSoftware, txtFilter.Text.Trim( ) );
		}

		private void FrmMain_Shown( object sender, EventArgs e ) {
			txtComputerName.Focus( );
		}

		private void ClearInfoData( ) {
			_dsComputerInfo.Clear( );
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
			OnStartInfoQuery( );
			var nameSource = txtInfoComputerName.Text.Trim( );
			if( string.IsNullOrEmpty( nameSource ) ) {
				return;
			}
			if( IsFile( nameSource ) ) {
				nameSource = GetComputerNamesFromFile( nameSource );
			}
			var computerNames = nameSource.Split( new[] { @";", @",", @"	", @" ", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries ).Distinct( ).Where( item => !string.IsNullOrEmpty( item ) ).ToArray( );
			_dsComputerInfoThreadCount = computerNames.Count( );

			foreach( var computerName in computerNames ) {
				var currentName = computerName;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( currentName ) ) {
							ComputerInfo.GetComputerInfo( currentName, ref _dsComputerInfo );
						} else {
							// TODO Better logging so that multiples can be done without interruption and threaded
							_dsComputerInfo.Add( new ComputerInfo { LocalSystemDateTime = DateTime.Now, ComputerName = currentName, Status = @"Connection Error" } );
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
			QueryRemoteComputerInfo( );
		}

		private void dgvComputerInfo_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( 0 > e.RowIndex || 0 > e.ColumnIndex ) {
				return;
			}
			DgvHelpers.SelectCell( dgvComputerInfo, e.RowIndex, e.ColumnIndex );
			var rebootIndex = DgvHelpers.GetColumnIndex( dgvComputerInfo, @"Reboot" );
			switch( e.Button ) {
			case MouseButtons.Left: {
					if( e.ColumnIndex == rebootIndex ) {
						var computerName = dgvComputerInfo.Rows[e.RowIndex].Cells[@"Computer Name"].Value.ToString( );
						Debug.Assert( null != computerName, @"ComputerName is null.  This should never happen" );
						if( DialogResult.Yes == MessageBox.Show( string.Format( @"Reboot {0}?", computerName ), @"Question", MessageBoxButtons.YesNo ) ) {
							ComputerInfo.RebootComputer( computerName );
						}
					}
					break;
				}
			case MouseButtons.Right: {
					if( e.ColumnIndex == rebootIndex ) {
						return;
					}
					var m = new ContextMenu( );
					DgvHelpers.AddCopyColumnMenuItem( dgvComputerInfo, ref m, e.RowIndex, e.ColumnIndex );
					if( 0 < m.MenuItems.Count ) {
						m.Show( dgvComputerInfo, dgvComputerInfo.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
					break;
				}
			}
		}

		private void txtInfoComputerName_Enter( object sender, EventArgs e ) {
			AcceptButton = btnInfoQuery;
		}

		private void txtInfoComputerName_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void txtComputerName_Enter( object sender, EventArgs e ) {
			AcceptButton = btnQueryRemoteComputer;
		}

		private void txtComputerName_Leave( object sender, EventArgs e ) {
			AcceptButton = null;
		}

		private void TxtFilterComputerInfo_TextChanged( object sender, EventArgs e ) {
			FilterText( dgvComputerInfo, ref _dsComputerInfo, txtFilterComputerInfo.Text.Trim( ) );
		}
	}
}
