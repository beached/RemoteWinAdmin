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
		private Int32 _actionDepth;
		private Int32 _dsThreadCount;
		private SyncList<T> _ds;
		private readonly Form _parent;
		public Action<string, SyncList<T>> QueryDataCb { get; set; }
		public DataGridViewColumnCollection Columns { get { return dgv.Columns; } }
		public DataGridViewRowCollection Rows { get { return dgv.Rows; } }
		public string CompletionMessage { get; set; }

		private bool _generateLookupMenu = true;
		public bool GenerateLookupMenu { 
			get { return _generateLookupMenu;  } 
			set { _generateLookupMenu = value; } 
		}

		public Action<DataGridView> SetupColumnsCb { 
			get { return _setupColumnsCb; }
			set {
				_setupColumnsCb = value;
				dgv.Columns.Clear(  );
				_setupColumnsCb( dgv );
			}
		}
		 
		public DataPageControl( Form parent ) {
			_parent = parent;
			_ds = new SyncList<T>( parent );
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
			dgv.AllowUserToAddRows = false;
			dgv.AllowUserToDeleteRows = false;
			dgv.AutoGenerateColumns = false;
			dgv.AutoResizeColumnHeadersHeight( );
			dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
			dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			dgv.MultiSelect = true;
			dgv.ReadOnly = true;
			dgv.RowHeadersVisible = true;
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

			return result.Where( name => !string.IsNullOrEmpty( name.Trim( ) ) ).OrderBy( name => name ).Distinct( ).ToList( );
		}

		private void OnActionStart( ) {
			if( 1 < Interlocked.Increment( ref _actionDepth ) ) {
				return;
			}
			gbComputers.Enabled = false;
			txtFilter.Enabled = false;
			dgv.DataSource = null;
		}

		private void OnActionEnd( ) {
			OnActionEnd( true );
		}
		
		private void OnActionEnd( bool showMessage ) {
			if( 0 < Interlocked.Decrement( ref _actionDepth ) ) {
				return;
			}
			gbComputers.Enabled = true;
			txtFilter.Enabled = true;
			dgv.DataSource = _ds;
			dgv.AutoResizeColumns( DataGridViewAutoSizeColumnsMode.AllCells);
			dgv.AutoResizeRows( DataGridViewAutoSizeRowsMode.AllCells );
			if( !showMessage || string.IsNullOrEmpty( CompletionMessage ) ) {
				return;
			}
			NotificationWindow.NotificationWindow.AddMessage( CompletionMessage );
		}

		public void OnActionEndInvoke( ) {
			if( InvokeRequired ) {
				Invoke( new Action( OnActionEnd ) );
			} else {
				OnActionEnd( );
			}
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
				if( !DgvHelpers.CanSearch( column )) {
					continue;
				}
				if( !column.Visible ) {
					continue;
				}
				var curCellValue = DgvHelpers.GetCellString( dataGridView, rowIndex, column.Index );
				var menuName = column.HeaderText;
				Helpers.Assert( menuName != null, @"All columns must have a header defined" );
				if( 0 == string.Compare( clickedColumnName, column.Name, StringComparison.InvariantCulture ) ) {
					menuName = "*" + menuName;
				}
				lookupMenu.MenuItems.Add( new MenuItem( menuName, delegate { SearchWeb( curCellValue ); } ) );
			}
			if( 0 < lookupMenu.MenuItems.Count ) {
				menu.MenuItems.Add( lookupMenu );
			}
			return menu;
		}

		private static void FilterText<TValue>( DataGridView dgv, ref SyncList<TValue> values, string filter ) where TValue: IDataPageRow {
			if( null == filter ) {
				filter = string.Empty;
			}
			filter = filter.Trim( );
			var filters = filter.Split( ' ' );
			dgv.CurrentCell = null;
			var rowVisibility = Enumerable.Repeat( true, dgv.Rows.Count ).ToList( );

			foreach( var currentFilter in filters ) {
				for( var row = 0; row < rowVisibility.Count; ++row ) {
					if( rowVisibility[row] ) {
						rowVisibility[row] = values[row].ContainsString( currentFilter );
					}
				}
			}
			for( var row = 0; row < rowVisibility.Count && row < dgv.Rows.Count; ++row ) {
				dgv.Rows[row].Visible = rowVisibility[row];
			}
		}

		private static void SearchWeb( string query ) {
			var strQuery = string.Format( @"https://www.google.ca/search?q={0}", System.Web.HttpUtility.UrlEncode( query ) );
			OpenLink( strQuery );
		}

		private static bool IsFile( string path ) {
			return File.Exists( MagicValues.StripSurroundingDblQuotes( path ) );
		}

		public static bool IsLink( DataGridViewCell cell ) {
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

		private void Query( ) {
			if( null == QueryDataCb ) {
				return;
			}
			Clear(  );
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
								var value = new T {
									ComputerName = computerName, ConnectionStatus = ConnectionStatuses.ConnectionError
								};
								_ds.Add( value );
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

		private void btnQuery_Click( object sender, EventArgs e ) {
			if( !string.IsNullOrEmpty( txtComputers.Text.Trim( ) ) ) {
				Query( );
			}
		}

		private void dgv_CellMouseClick( object sender, DataGridViewCellMouseEventArgs e ) {
			if( !ValidateAndSelect( ref dgv, e ) ) {
				return;
			}
			var curCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
			switch( e.Button ) {
			case MouseButtons.Left: {				
				if( IsButton( curCell ) ) {
					OnActionStart(  );
					try {
						var rowGuid = dgv.Rows[e.RowIndex].Cells[@"RowGuid"].Value as Guid?;
						Helpers.Assert( null != rowGuid, @"All IDataPageRows must have a valid RowGuid" );

						var curDsItem = _ds[_ds.Find( @"RowGuid", rowGuid )] as IDataPageRow;

						if( curDsItem.GetActions(  )[(string)curCell.Value]( curDsItem ) ) {
							Query( );
						}
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
