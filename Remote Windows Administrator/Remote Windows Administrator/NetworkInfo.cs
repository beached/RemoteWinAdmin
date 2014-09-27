using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncList;

namespace RemoteWindowsAdministrator {
	public class NetworkInfo: IContainsString {
		public string ComputerName { get; set; }
		public string ConnectionStatus { get; set; }

		public NetworkInfo( string computerName, string connectionStatus = @"OK" ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Test( ComputerName ).Test( ConnectionStatus ).IsContained;
		}


		public static void GetNetworkInfo( string computerName, ref SyncList<NetworkInfo> result ) {
			
		}
	}
}
