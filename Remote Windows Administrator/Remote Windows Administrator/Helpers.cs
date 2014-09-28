using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace daw {
	public class Helpers {
		public static void Assert( bool condition, string message = "" ) {
			if( condition ) {
				return;
			}
			var trace = new StackTrace( 1 );
			MessageBox.Show( String.Format( "{0}\n{1}", message, trace ), @"Assertion Fail", MessageBoxButtons.OK, MessageBoxIcon.Error );
			Application.Exit( );
		}

		public static string StraToStr( IEnumerable<string> stringArray ) {
			var result = String.Empty;
			var isFirst = true;
			foreach( var currentString in stringArray ) {
				if( !isFirst ) {
					result += "\n";
				} else {
					isFirst = false;
				}
				result += currentString;
			}
			return result;
		}

		/// <summary>
		/// Converts a camel case string to one where each capital is prefixed with a
		/// space.
		/// </summary>
		/// <param name="value">Camelcase string.</param>
		/// <returns></returns>
		public static string CamelToSpace( string value ) {
			var result = String.Empty;
			foreach( var currentChar in value.ToCharArray( ) ) {
				if( Char.IsUpper( currentChar ) ) {
					result += @" ";
				}
				result += currentChar;
			}
			return result;
		}

		public class TypeChecks {
			public static bool IsBool( Type type ) {
				return typeof( bool ) == type || typeof( bool? ) == type;
			}

			public static bool IsDateTime( Type type ) {
				return typeof( DateTime ) == type || typeof( DateTime? ) == type;
			}

			public static bool IsString( Type type ) {
				return typeof( string ) == type;
			}

			public static bool IsFloat( Type type ) {
				return typeof( float ) == type || typeof( float? ) == type;
			}

			public static bool IsInt( Type type ) {
				return typeof( int ) == type || typeof( int? ) == type;
			}

			public static bool IsUInt( Type type ) {
				return typeof( uint ) == type || typeof( uint? ) == type;
			}

			public static bool IsShort( Type type ) {
				return typeof( short ) == type || typeof( short? ) == type;
			}

			public static bool IsUShort( Type type ) {
				return typeof( ushort ) == type || typeof( ushort? ) == type;
			}

			public static bool IsNumber( Type type ) {
				return IsFloat( type ) || IsInt( type ) || IsUInt( type ) || IsShort( type ) || IsUShort( type );

			}
		}
	}
}
