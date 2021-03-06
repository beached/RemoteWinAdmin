﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public static class Helpers {
// 		public static void Assert( bool condition, string message = "" ) {
// 			if( condition ) {
// 				return;
// 			}
// 			var trace = new StackTrace( 1 );
// 			GlobalLogging.WriteLine( Logging.LogSeverity.Fatal, @"Assertion Failure - {0} - {1}", message, trace );
// 			MessageBox.Show( String.Format( "{0}\n{1}", message, trace ), @"Assertion Fail", MessageBoxButtons.OK, MessageBoxIcon.Error );
// 			Application.Exit( );
// 		}
// 		
		private static void AssertCondition( bool condition, string messageFormat, params object[] messageValues ) {
			if( condition ) {
				return;
			}
			var trace = new StackTrace( 2 );
			var message = string.Format( messageFormat, messageValues );
			GlobalLogging.WriteLine( Logging.LogSeverity.Fatal, @"Assertion Failure - {0} - {1}", message, trace );
			throw new AssertionException( message );			
		}

		/// <summary>
		/// Asserts that the condition returns true
		/// </summary>
		public static void Assert( bool condition, string messageFormat, params object[] messageValues ) {
			AssertCondition( condition, messageFormat, messageValues );
		}

		/// <summary>
		/// Asserts that the string is neither empty or null
		/// </summary>
		public static void AssertString( string value, string messageFormat, params object[] messageValues ) {
			AssertCondition( !string.IsNullOrEmpty( value ), messageFormat, messageValues );
		}

		/// <summary>
		/// Asserts that the value is not null
		/// </summary>
		public static void AssertNotNull( object value, string messageFormat, params object[] messageValues ) {
			AssertCondition( null != value, messageFormat, messageValues );
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
			return result.Trim( );
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
				return typeof( uint ) == type || typeof( uint? ) == type || typeof( UInt64 ) == type || typeof( UInt64? ) == type;
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

			public static bool IsEnum( Type type ) {
				return type.IsEnum;
			}

			public static bool IsStringArray( Type type ) {
				return typeof( string[] ) == type || typeof( IEnumerable<string> ) == type;
			}
		}
	}

	[Serializable]
	public class AssertionException: Exception {
		public AssertionException( ) { }
		public AssertionException( string message ) : base( message ) { }
	}
}
