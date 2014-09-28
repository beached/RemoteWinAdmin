using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteWindowsAdministrator {
	public interface IDataPageRow {
		string ComputerName { get; set; }
		string ConnectionStatus { get; set; }
		bool ContainsString( string value );
		bool Valid( );
	}
}
