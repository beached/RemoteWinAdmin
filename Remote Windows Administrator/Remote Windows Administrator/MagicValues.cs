using System;
using System.Collections.Generic;
using System.Linq;

namespace RemoteWindowsAdministrator {
	public static class MagicValues {
		public static string TimeSpanToString( TimeSpan ts ) {
			return string.Format( @"{0}day{1} {2}hrs {3}min", ts.Days, ts.Days != 1 ? @"s" : @"", ts.Hours, ts.Minutes );
		}

		public const string ShortDateFormat = @"yyyy-MM-dd";
		public const string TimeDateStringFormat = @"yyyy-MM-dd HH:mm";

		public static List<string> DeleniateComputerList( string computerList ) {
			return computerList.Split( new[] {
				@";", @",", @"	", @" ", "\r\n", "\n", "\r"
			}, StringSplitOptions.RemoveEmptyEntries ).Distinct( ).Where( item => !string.IsNullOrEmpty( item ) ).ToList(  );
		}

		public static string StripSurroundingDblQuotes( string value ) {
			if( string.IsNullOrEmpty( value ) ) {
				return string.Empty;
			}
			const string dblQuote = "\"";
			if( value.StartsWith( dblQuote ) && value.EndsWith( dblQuote ) ) {
				value = value.Substring( 1, value.Length - 2 );
			}
			return value;
		}

		public static readonly List<string> FieldsToNotLoopup = new List<string>( ) {
			@"Computer Name", @"Connection Status", @"SID"
		};
	}
}
