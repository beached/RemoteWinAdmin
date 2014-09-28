using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using daw;
using RemoteWindowsAdministrator;

namespace SyncList {
	public static class DgvHelpers {
// 		private static void Assert( bool condition, string message ) {
// 			if( condition ) {
// 				return;
// 			}
// 			var trace = new StackTrace( 1 );
// 			MessageBox.Show( String.Format( "{0}\n{1}", message, trace ), @"Assertion Fail", MessageBoxButtons.OK, MessageBoxIcon.Error );
// 			Application.Exit( );
// 		}

		public static DataGridViewColumn MakeColumn( string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			return SetSearchable( new DataGridViewColumn {
				Name = name, HeaderText = (headerName ?? name), ReadOnly = readOnly, DataPropertyName = name, SortMode = (canSort ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable), CellTemplate = new DataGridViewTextBoxCell( ), Visible = !hidden
			} );
		}

		public static void AddColumn( DataGridView dgv, string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeColumn( name, headerName, hidden, canSort, readOnly ) );
		}

		public static void ConvertToMultilineColumn( DataGridViewColumn column ) {
			column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
		}

		public static DataGridViewColumn MakeMultilineColumn( string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var result = MakeColumn( name, headerName, hidden, canSort, readOnly );
			ConvertToMultilineColumn( result );
			return result;
		}

		public static void AddMultilineColumn( DataGridView dgv, string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeMultilineColumn( name, headerName, hidden, canSort, readOnly ) );
		}
		
		public static void ConvertToLinkColumn( DataGridViewColumn column ) {
			var cellTemplate = new DataGridViewLinkCell { TrackVisitedState = false, UseColumnTextForLinkValue = true, LinkBehavior = LinkBehavior.NeverUnderline };
			column.CellTemplate = cellTemplate;
			column.DefaultCellStyle.NullValue = String.Empty;
		}

		public static DataGridViewColumn MakeLinkColumn( string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var col = MakeColumn( name, headerName, hidden, canSort, readOnly );
			ConvertToLinkColumn( col );
			return col;
		}		

		public static void AddLinkColumn( DataGridView dgv, string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			dgv.Columns.Add( MakeLinkColumn( name, headerName, hidden, canSort, readOnly ) );
		}

		public static DataGridViewColumn MakeButtonColumn( string name, bool hidden = false ) {
			return new DataGridViewButtonColumn {
				Name = name, HeaderText = name, Text = name, UseColumnTextForButtonValue = true, Visible = !hidden, Tag = @"Button"
			};
		}

		public static void AddButtonColumn( DataGridView dgv, string name, bool hidden = false ) {
			dgv.Columns.Add( MakeButtonColumn( name, hidden ) );
		}
 
		public static DataGridViewColumn MakeCheckedColumn( string name, string headerName = null, bool hidden = false, bool canSort = true, bool readOnly = true ) {
			var col = MakeColumn( name, headerName, hidden, canSort, readOnly );
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

		public static void AddDateColumn( DataGridView dgv, string name, string headerName = null, bool hidden = false, bool canSort = true, string dateFormat = @"yyyy-MM-dd", bool readOnly = true ) {
			dgv.Columns.Add( MakeDateColumn( name, headerName, hidden, canSort, dateFormat, readOnly ) );
		}


		public static int GetColumnIndex( DataGridView dgv, string columnName ) {
			Helpers.Assert( null != columnName, "Null column name's do not make sense" );
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

		public static DataGridViewColumn GetColumn( DataGridView dgv, string columnName, bool allowNull = false ) {
			var column = dgv.Columns[columnName];
			Helpers.Assert( !(null == column && !allowNull), @"Supplied columnName does not exist" );
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

		public static void GenerateColumns( DataGridView dgv, Type type, IList<string> columnNames ) {						
			foreach( var columnName in columnNames ) {				
				var property = type.GetProperty( columnName );
				Helpers.Assert( null != property, @"Property names must exist in type specified" );
				var columnType = property.PropertyType;
				if( Helpers.TypeChecks.IsString( columnType ) || Helpers.TypeChecks.IsNumber( columnType ) ) {
					
					AddColumn( dgv, columnName, Helpers.CamelToSpace( columnName ) );
				} else if( Helpers.TypeChecks.IsDateTime( columnType ) ) {
					AddDateColumn( dgv, columnName, Helpers.CamelToSpace( columnName ), false, true, MagicValues.TimeDateStringFormat );
				} else if( Helpers.TypeChecks.IsBool( columnType ) ) {
					AddCheckedColumn( dgv, columnName, Helpers.CamelToSpace( columnName ) );
				} else {
					throw new UnsupportedTypeException( type, @"Have not accounted for this type in DGVHelpers GenerateColumns" );
				}
			} 
		}
	}
}
