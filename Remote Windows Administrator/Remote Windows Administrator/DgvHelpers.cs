using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public static class DgvHelpers {
		public static DataGridViewColumn MakeColumn( string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			return SetSearchable( new DataGridViewColumn {
				Name = propertyName, HeaderText = (headerName ?? propertyName), ReadOnly = readOnly, DataPropertyName = propertyName, SortMode = (canSort ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable), CellTemplate = new DataGridViewTextBoxCell( ), Visible = !hidden
			} );
		}

		public static void AddColumn( DataGridView dgv, string propertName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeColumn( propertName, headerName, hidden, canSort, readOnly ) );
		}

		public static void ConvertToMultilineColumn( DataGridViewColumn column ) {
			column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
		}

		public static DataGridViewColumn MakeMultilineColumn( string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var result = MakeColumn( propertyName, headerName, hidden, canSort, readOnly );
			ConvertToMultilineColumn( result );
			return result;
		}

		public static void AddMultilineColumn( DataGridView dgv, string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeMultilineColumn( propertyName, headerName, hidden, canSort, readOnly ) );
		}
		
		public static void ConvertToLinkColumn( DataGridViewColumn column ) {
			var cellTemplate = new DataGridViewLinkCell { TrackVisitedState = false, UseColumnTextForLinkValue = true, LinkBehavior = LinkBehavior.NeverUnderline };
			column.CellTemplate = cellTemplate;
			column.DefaultCellStyle.NullValue = String.Empty;
		}

		public static DataGridViewColumn MakeLinkColumn( string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var col = MakeColumn( propertyName, headerName, hidden, canSort, readOnly );
			ConvertToLinkColumn( col );
			return col;
		}		

		public static void AddLinkColumn( DataGridView dgv, string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeLinkColumn( propertyName, headerName, hidden, canSort, readOnly ) );
		}

		public static DataGridViewColumn MakeButtonColumn( string buttonName, bool hidden = false ) {
			return new DataGridViewButtonColumn {
				Name = buttonName, HeaderText = buttonName, Text = buttonName, UseColumnTextForButtonValue = true, Visible = !hidden, Tag = @"Button"
			};
		}

		public static void AddButtonColumn( DataGridView dgv, string buttonName, bool hidden = false ) {
			dgv.Columns.Add( MakeButtonColumn( buttonName, hidden ) );
		}
 
		public static DataGridViewColumn MakeCheckedColumn( string propertyName, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var col = MakeColumn( propertyName, headerName, hidden, canSort, readOnly );
			col.DefaultCellStyle.NullValue = false;
			col.CellTemplate = new DataGridViewCheckBoxCell( );
			col.Tag = @"CannotSearch";
			return col;
		}

		public static void AddCheckedColumn( DataGridView dgv, string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeCheckedColumn( name, headerName, hidden, canSort, readOnly ) );
		}

		public static DataGridViewColumn MakeDateColumn( string name, string headerName = null, bool hidden = false, bool canSort = true, string dateFormat = @"yyyy-MM-dd", bool readOnly = true ) {
			var col = MakeColumn( name, headerName, hidden, canSort, readOnly );
			col.DefaultCellStyle.Format = dateFormat;
			return col;
		}

		public static void AddDateColumn( DataGridView dgv, string propertyName, string headerName = null, bool hidden = false, bool canSort = true, string dateFormat = @"yyyy-MM-dd", bool readOnly = true ) {
			dgv.Columns.Add( MakeDateColumn( propertyName, headerName, hidden, canSort, dateFormat, readOnly ) );
		}


		public static int GetColumnIndex( DataGridView dgv, string columnName ) {
			Helpers.Assert( null != columnName, "Null column propertName's do not make sense" );
			var dataGridViewColumn = dgv.Columns[columnName];

			Helpers.Assert( null != dataGridViewColumn, "Column names must exist" );			
			return dataGridViewColumn.Index;
		}

		public static string GetCellString( DataGridView dgv, int row, string columnName ) {
			if( 0 > row || dgv.RowCount <= row ) {
				return String.Empty;
			}
			return GetCellString( dgv, row, GetColumnIndex( dgv, columnName ) );
		}

		public static string GetCellString( DataGridView dgv, int row, int col ) {
			var result = String.Empty;
			if( 0 > row || 0 > col || dgv.RowCount <= row || dgv.ColumnCount <= col ) {
				return result;
			}
			var cell = dgv.Rows[row].Cells[col];
			if( null == cell || null == cell.Value ) {
				return result;
			}
			var strTmp = cell.Value.ToString( );
			if( !String.IsNullOrEmpty( strTmp ) ) {
				result = strTmp;
			}
			return result;
		}

		public static string GetColumnName( DataGridView dgv, int column ) {
			Helpers.Assert( 0 <= column && dgv.Columns.Count >= column, @"An invalid column number was specified" );
			return dgv.Columns[column].Name;
		}

		public static void UnselectAll( ref DataGridView dgv ) {
			foreach( DataGridViewRow row in dgv.Rows ) {
				row.Selected = false;
			}
		}

		public static void SelectCell( ref DataGridView dgv, int row, int col ) {
			if( 0 > row || 0 > col || dgv.RowCount <= row || dgv.ColumnCount <= col ) {
				return;
			}
			UnselectAll( ref dgv );
			dgv.Rows[row].Cells[col].Selected = true;
		}


		public static void AddCopyColumnMenuItem( DataGridView dgv, ref MenuItem m, int row, int col ) {
			if( 0 > row || 0 > col || dgv.RowCount <= row || dgv.ColumnCount <= col ) {
				return;
			}
			if( String.IsNullOrEmpty( GetCellString( dgv, row, col ).Trim( ) ) ) {
				return;
			}

			m.MenuItems.Add( new MenuItem( String.Format( @"Copy {0}", GetColumnName( dgv, col ) ), delegate {
				Clipboard.SetText( dgv.Rows[row].Cells[col].Value.ToString( ) );
			} ) );
		}

		public static void AddCopyColumnMenuItem( DataGridView dgv, ref ContextMenu cm, int row, int col ) {
			if( 0 > row || 0 > col || dgv.RowCount <= row || dgv.ColumnCount <= col ) {
				return;
			}
			if( String.IsNullOrEmpty( GetCellString( dgv, row, col ).Trim( ) ) ) {
				return;
			}

			cm.MenuItems.Add( new MenuItem( String.Format( @"Copy {0}", GetColumnName( dgv, col ) ), delegate {
				Clipboard.SetText( dgv.Rows[row].Cells[col].Value.ToString( ) );
			} ) );
		}


		public class UnsupportedTypeException: Exception {
			public UnsupportedTypeException( Type type, string message = @"" ): base( string.Format( @"The type '{0}' is unsupported. {1}", type.FullName, message ) ) { } 
		}

		public static DataGridViewColumn GetColumn( DataGridView dgv, string columnName ) {
			var column = dgv.Columns[columnName];
			Helpers.Assert( null != column, @"Supplied columnName does not exist" );
			return column;
		}

		public enum ColumnTags {
			CanSearch = 0,
			CannotSearch
		};

		public static DataGridViewColumn SetSearchable( DataGridViewColumn column, bool allowSearch = true ) {
			column.Tag = allowSearch ? ColumnTags.CanSearch: ColumnTags.CannotSearch;
			return column;
		}

		public static bool CanSearch( DataGridViewColumn column ) {
			var tag = column.Tag as ColumnTags?;
			if( null != tag ) {
				return tag == ColumnTags.CanSearch;
			}
			SetSearchable( column );	// Undefined means defaults to searchable
			return true;
		}

		public static DataGridViewColumn SetColumnHeader( DataGridViewColumn column, string headerText ) {
			column.HeaderText = headerText;
			return column;
		}

		public static void AddColumnBasedOnDataType( DataGridView dgv, Type columnType, string columnName ) {
			if( Helpers.TypeChecks.IsString( columnType ) || Helpers.TypeChecks.IsNumber( columnType ) ) {
				AddColumn( dgv, columnName, Helpers.CamelToSpace( columnName ) );
			} else if( Helpers.TypeChecks.IsDateTime( columnType ) ) {
				AddDateColumn( dgv, columnName, Helpers.CamelToSpace( columnName ), false, true, MagicValues.TimeDateStringFormat );
			} else if( Helpers.TypeChecks.IsBool( columnType ) ) {
				AddCheckedColumn( dgv, columnName, Helpers.CamelToSpace( columnName ) );
			} else {
				throw new UnsupportedTypeException( columnType, @"Have not accounted for this type in DGVHelpers AddColumnBasedOnType" );
			}
		}

		public static void GenerateAllColumns( DataGridView dgv, Type type, IEnumerable<string> excludedColumnNames = null ) {
			Helpers.Assert( typeof( IDataPageRow ).IsAssignableFrom( type ), @"GenerateAllColums requires Type to implement IDataPageRO" );
			if( null == excludedColumnNames ) {
				excludedColumnNames = new List<string>( );
			}
			foreach( var property in type.GetProperties( ).Where( property => !excludedColumnNames.Contains( property.Name ) ).OrderBy( x => x.Name ) ) {
				if( typeof( Guid ) == property.PropertyType && 0 == string.CompareOrdinal( @"RowGuid", property.Name ) ) {
					AddColumn( dgv, property.Name, Helpers.CamelToSpace( property.Name ), true );
				} else {
					AddColumnBasedOnDataType( dgv, property.PropertyType, property.Name );
				}
			}
		}

		public static void MoveColumnToIndex( DataGridViewColumn column, int index ) {
			column.DisplayIndex = index;
		}
	}
}
