using daw;
using SyncList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class DataPageControl<T>: UserControl where T: IDataPageRow, new() {
		private Action<DataGridView> _setupColumnsCb;
		private SyncList<T> _ds;
		private Int32 _dsThreadCount;
		private readonly Form _parent;

		public string CompletionMessage { get; set; }

		public Action<DataGridView> SetupColumnsCb { 
			get { return _setupColumnsCb; }
			set {
				_setupColumnsCb = value;
				dgv.Columns.Clear(  );
				_setupColumnsCb( dgv );
			}
		}
		public Action<string, SyncList<T>> QueryDataCb { get; set; }
		public Func<DataGridView, int, int, bool> OnCellButtonClick { get; set; }

		public DataGridViewColumnCollection Columns { get { return dgv.Columns; } }
		public DataGridViewRowCollection Rows { get { return dgv.Rows; } }
		
		/// <summary>
		/// All columns tagged with string "CanSearch" will be searchable on Google
		/// </summary>
		public bool GenerateLookupMenu { get; set; }
		 
		public DataPageControl( Form parent, SyncList<T> dataSource ) {
			_parent = parent;
			_ds = dataSource;
			InitializeComponent( );
			SetDgvDefaults( dgv );
		}

		private void Clear( ) {
			lock( _ds ) {
				_ds.Clear( );
			}
			txtFilter.Clear(  );
			dgv.DataSource = _ds;
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

		private static IEnumerable<string> GetComputerNamesFromFile( string fileName ) {
			fileName = MagicValues.StripSurroundingDblQuotes( fileName );
			Helpers.Assert( IsFile( fileName ), "File does not exist" );
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

		private void OnActionStart( ) {
			gbComputers.Enabled = false;
			txtFilter.Enabled = false;
		}

		private void OnActionEnd( ) {
			OnActionEnd( true );
		}
		
		private void OnActionEnd( bool showMessage ) {
			gbComputers.Enabled = true;
			txtFilter.Enabled = true;
			if( showMessage && !string.IsNullOrEmpty( CompletionMessage ) ) {
				MessageBox.Show( CompletionMessage, @"Complete", MessageBoxButtons.OK );
			}
		}

		public void OnActionEndInvoke( ) {
			Invoke( new Action( OnActionEnd ) );
		}

		private ContextMenu MakeCopyLookupMenus( DataGridView dataGridView, int rowIndex, int columnIndex ) {
			var menu = new ContextMenu( );
			if( !String.IsNullOrEmpty( DgvHelpers.GetCellString( dataGridView, rowIndex, columnIndex ).Trim( ) ) ) {
				menu.MenuItems.Add( new MenuItem( string.Format( @"Copy {0}", DgvHelpers.GetColumnName( dataGridView, columnIndex ) ), delegate { Clipboard.SetText( dataGridView.Rows[rowIndex].Cells[columnIndex].Value.ToString( ) ); } ) );
			}

			if( !GenerateLookupMenu ) {
				return menu;
			}

			var lookupMenu = new MenuItem( @"Lookup" );

			var clickedColumnName = dataGridView.Columns[columnIndex].Name;
			foreach( DataGridViewColumn column in dataGridView.Columns ) {
				if( MagicValues.FieldsToNotLoopup.Contains( column.Name ) ) {
					continue;
				}
				if( 0 != (@"CanSearch").CompareTo( column.Tag ) ) {
					continue;
				}
				if( string.IsNullOrEmpty( dataGridView.Rows[rowIndex].Cells[column.Index].Value as string ) ) {
					continue;
				}
				var curCellValue = DgvHelpers.GetCellString( dataGridView, rowIndex, column.Index );
				var menuName = (clickedColumnName == column.Name && !string.IsNullOrEmpty( curCellValue ) ? @"*" : string.Empty) + column.Name;
				lookupMenu.MenuItems.Add( new MenuItem( menuName, delegate { SearchWeb( curCellValue ); } ) );
			}
			if( 0 < lookupMenu.MenuItems.Count ) {
				menu.MenuItems.Add( lookupMenu );
			}
			return menu;
		}

		private static void FilterText<U>( DataGridView dgv, ref SyncList<U> values, string filter ) where U: IDataPageRow {
			// TODO replace with filtering in SyncList to maintain data binding
			if( !string.IsNullOrEmpty( filter ) ) {
				var filteredList = values.AsEnumerable( ).Where( item => item.ContainsString( filter ) ).ToList( );
				dgv.DataSource = filteredList;
			} else {
				dgv.DataSource = values;
			}
		}

		private static void SearchWeb( string query ) {
			var strQuery = string.Format( @"https://www.google.ca/search?q={0}", System.Web.HttpUtility.UrlEncode( query ) );
			OpenLink( strQuery );
		}

		private static bool IsFile( string path ) {
			return File.Exists( MagicValues.StripSurroundingDblQuotes( path ) );
		}

		private static bool IsLink( DataGridViewCell cell ) {
			return cell != null && cell.GetType( ) == typeof( DataGridViewLinkCell );
		}

		private static bool IsButton( DataGridViewCell cell ) {
			if( 0 == string.Compare( cell.Tag as string, @"Button", StringComparison.Ordinal ) ) {
				return true;
			}
			return cell is DataGridViewButtonCell;
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

		private static bool ValidateAndSelect( ref DataGridView dgv, DataGridViewCellMouseEventArgs e ) {
			if( null == e || 0 > e.RowIndex || 0 > e.ColumnIndex || dgv.RowCount <= e.RowIndex || dgv.ColumnCount <= e.ColumnIndex ) {
				return false;
			}
			DgvHelpers.SelectCell( ref dgv, e.RowIndex, e.ColumnIndex );
			return true;
		}

		// Events
		private void txtComputers_Enter( object sender, EventArgs e ) {
			_parent.AcceptButton = btnQuery;
		}

		private void txtComputers_Leave( object sender, EventArgs e ) {
			_parent.AcceptButton = null;
		}

		private void txtComputers_TextChanged( object sender, EventArgs e ) {
			Clear( );
		}

		private void btnQuery_Click( object sender, EventArgs e ) {
			if( null == QueryDataCb ) {
				return;
			}
			OnActionStart( );
			var computerNames = GetComputerNamesFromString( txtComputers.Text );
			_dsThreadCount = computerNames.Count;

			foreach( var currentName in computerNames ) {
				var computerName = currentName;
				new Thread( ( ) => {
					try {
						if( WmiHelpers.IsAlive( computerName ) ) {
							QueryDataCb( computerName, _ds );
						} else {
							lock( _ds ) {
								_ds.Add( new T {
									ComputerName = computerName, ConnectionStatus = @"Connection Error"
								} );
							}
						}
					} finally {
						if( 0 >= Interlocked.Decrement( ref _dsThreadCount ) ) {
							OnActionEndInvoke( );
						}
					}
				} ).Start( );
			}
		}

		private void dgv_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgv, e ) ) {
				return;
			}
			var curCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
			switch( e.Button ) {
			case MouseButtons.Left: {				
				if( null != OnCellButtonClick && IsButton( curCell ) ) {
					OnActionStart(  );
					try {
						OnCellButtonClick( dgv, e.RowIndex, e.ColumnIndex );
					} finally {
						OnActionEnd( false );
					}
				} else if( IsLink( curCell ) ) {
					OpenLink( curCell );
				}
				break;
			}
			case MouseButtons.Right: {
				var ctxMnu = MakeCopyLookupMenus( dgv, e.RowIndex, e.ColumnIndex );
					if( null != ctxMnu && 0 < ctxMnu.MenuItems.Count ) {
						ctxMnu.Show( dgv, dgv.PointToClient( new Point( Cursor.Position.X, Cursor.Position.Y ) ) );
					}
				break;
			}
			}
		}

		private void txtFilter_TextChanged( object sender, EventArgs e ) {
			FilterText( dgv, ref _ds, txtFilter.Text.Trim( ) );
		}

	}
}
