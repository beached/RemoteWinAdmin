using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteWindowsAdministrator {
	public interface IDataPageRow {
		string ComputerName { get; set; }
		string ConnectionStatus { get; set; }
		Guid RowGuid { get; }
		bool ContainsString( string value );
		bool Valid( );
		IDictionary<string, Func<IDataPageRow, bool>> GetActions( );
	}
}
