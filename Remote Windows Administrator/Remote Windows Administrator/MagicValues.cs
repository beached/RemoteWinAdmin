using System;

namespace RemoteWindowsAdministrator {
	public static class MagicValues {
		public static string TimeSpanToString( TimeSpan ts ) {
			return string.Format( @"{0}day{1} {2}hrs {3}min", ts.Days, ts.Days != 1 ? @"s" : @"", ts.Hours, ts.Minutes );
		}

		public const string ShortDateFormat = @"yyyy-MM-dd";
		public const string TimeDateStringFormat = @"yyyy-MM-dd HH:mm";

	}
}
