using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteWindowsAdministrator {
	public interface IDataPageRow {
		string ComputerName { get; set; }
		ConnectionStatuses ConnectionStatus { get; set; }
		string ConnectionStatusString { get; }
		Guid RowGuid { get; }
		bool ContainsString( string value );
		bool Valid( );
		IDictionary<string, Func<IDataPageRow, bool>> GetActions( );
	}

	public enum ConnectionStatuses {
		Unknown = 0,
		Ok,
		Error,
		AccessDenied,
		ConnectionError,
		AuthorizationError,
		ErrorResolvingSid
	}
}
