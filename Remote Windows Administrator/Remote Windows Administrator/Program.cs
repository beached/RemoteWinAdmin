using System;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	static class Program {
		[STAThread]
		static void Main( ) {
			Application.EnableVisualStyles( );
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new FrmMain( ) );
		}
	}
}
