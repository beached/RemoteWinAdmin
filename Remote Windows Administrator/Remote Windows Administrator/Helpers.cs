using System.Diagnostics;
using System.Windows.Forms;

namespace daw {
	public class Helpers {
		public static void Assert( bool condition, string message = "" ) {
			if( condition ) {
				return;
			}
			var trace = new StackTrace( 1 );
			MessageBox.Show( string.Format( "{0}\n{1}", message, trace ), @"Assertion Fail", MessageBoxButtons.OK, MessageBoxIcon.Error );
			Application.Exit( );
		}
	}
}
