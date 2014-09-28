using daw;
using SyncList;
using System;
using System.Collections.Generic;

namespace RemoteWindowsAdministrator {
	public class NetworkInfo: IDataPageRow {
		public string ComputerName { get; set; }
		public string ConnectionStatus { get; set; }
		public DateTime? DhcpLeaseExpires { get; set; }
		public DateTime? DhcpLeaseObtained { get; set; }
		public UInt32 Index { get; set; }
		public UInt32 InterfaceIndex { get; set; }		
		public UInt32? IpConnectionMetric { get; set; }
		public bool DhcpEnabled { get; set; }
		public bool? DnsEnabledForWinsResolution { get; set; }
		public bool? DomainDnsRegistrationEnabled { get; set; }
		public bool? FullDnsRegistrationEnabled { get; set; }	
		public bool? IpEnabled { get; set; }
		public bool? WinsEnableLmHostsLookup { get; set; }
		public string Caption { get; set; }
		public string Description { get; set; }
		public string DhcpServer { get; set; }
		public string DnsDomain { get; set; }	
		public string DnsHostName { get; set; }
		public string MacAddress { get; set; }
		public string WinsHostLookupFile { get; set; }
		public string WinsPrimaryServer { get; set; }
		public string WinsScopeId { get; set; }
		public string WinsSecondaryServer { get; set; }
		public string[] DefaultIpGateway { get; set; }		
		public string[] DnsDomainSuffixSearchOrder { get; set; }
		public string[] DnsServerSearchOrder { get; set; }
		public string[] IpAddress { get; set; }

		public string DefaultIpGatewayString { get { return StraToStr( DefaultIpGateway ); } }
		public string DhcpLeaseTimeLeft {
			get {
				return null == DhcpLeaseExpires ? null : MagicValues.TimeSpanToString( DhcpLeaseExpires.Value - DateTime.Now );
			}
		}
		public string DnsDomainSuffixSearchOrders { get { return StraToStr( DnsDomainSuffixSearchOrder ); } }
		public string DnsServerSearchOrders { get { return StraToStr( DnsServerSearchOrder ); } }
		public string IpAddresses { get { return StraToStr( IpAddress ); } }
		public string WinsServers {
			get {
				var result = new List<string>();
				if( !string.IsNullOrEmpty( WinsPrimaryServer ) ) {
					result.Add( WinsPrimaryServer );
				}
				if( !string.IsNullOrEmpty( WinsSecondaryServer ) ) {
					result.Add( WinsSecondaryServer );
				}
				return StraToStr( result );
			}
		}

		public NetworkInfo( ) {
			ConnectionStatus = @"OK";
		}

		public NetworkInfo( string computerName, string connectionStatus = @"OK" ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add( ComputerName ).Add( ConnectionStatus ).Add( Caption ).Add( DefaultIpGateway ).Add( Description ).Add( DhcpEnabled ).Add( DhcpLeaseExpires ).Add( DhcpLeaseObtained ).Add( DhcpServer ).Add( DnsDomain ).Add( DnsDomainSuffixSearchOrder ).Add( DnsEnabledForWinsResolution ).Add( DnsHostName ).Add( DnsServerSearchOrder ).Add( DomainDnsRegistrationEnabled ).Add( FullDnsRegistrationEnabled ).Add( Index ).Add( InterfaceIndex ).Add( IpAddress ).Add( IpConnectionMetric ).Add( IpEnabled ).Add( MacAddress ).Add( WinsEnableLmHostsLookup ).Add( WinsHostLookupFile ).Add( WinsPrimaryServer ).Add( WinsSecondaryServer ).Add( WinsScopeId ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( ComputerName ) && !string.IsNullOrEmpty( ConnectionStatus );
		}

		private static string StraToStr( IEnumerable<string> stringArray ) {
			var result = string.Empty;
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

		public static void GetNetworkInfo( string computerName, SyncList<NetworkInfo> result ) {
			Helpers.Assert( null != result, @"result SyncList cannot be null" );
			Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"Computer name cannot be empty" );
			var networkInfoList = new List<NetworkInfo>( );
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_NetworkAdapterConfiguration", obj => {
					var ci = new NetworkInfo( computerName );
					ci.Caption = WmiHelpers.GetString( obj, @"Caption" );
					ci.DefaultIpGateway = WmiHelpers.GetStringArray( obj, @"DefaultIPGateway" );
					ci.Description = WmiHelpers.GetString( obj, @"Description" );
					ci.DhcpEnabled = WmiHelpers.GetBoolean( obj, @"DHCPEnabled" );
					ci.DhcpLeaseExpires = WmiHelpers.GetNullableDate( obj, @"DHCPLeaseExpires", true );
					ci.DhcpLeaseObtained = WmiHelpers.GetNullableDate( obj, @"DHCPLeaseObtained", true );
					ci.DhcpServer = WmiHelpers.GetString( obj, @"DHCPServer" );
					ci.DnsDomain = WmiHelpers.GetString( obj, @"DNSDomain" );
					ci.DnsDomainSuffixSearchOrder = WmiHelpers.GetStringArray( obj, @"DNSDomainSuffixSearchOrder" );
					ci.DnsEnabledForWinsResolution = WmiHelpers.GetNullableBoolean( obj, @"DNSEnabledForWINSResolution" );
					ci.DnsHostName = WmiHelpers.GetString( obj, @"DNSHostName" );
					ci.DnsServerSearchOrder = WmiHelpers.GetStringArray( obj, @"DNSServerSearchOrder" );
					ci.DomainDnsRegistrationEnabled = WmiHelpers.GetNullableBoolean( obj, @"DomainDNSRegistrationEnabled" );
					ci.FullDnsRegistrationEnabled = WmiHelpers.GetNullableBoolean( obj, @"FullDNSRegistrationEnabled" );
					ci.Index = WmiHelpers.GetUInt( obj, @"Index" );
					ci.InterfaceIndex = WmiHelpers.GetUInt( obj, @"InterfaceIndex" );
					ci.IpAddress = WmiHelpers.GetStringArray( obj, @"IPAddress" );
					ci.IpConnectionMetric = WmiHelpers.GetNullableUInt( obj, @"IPConnectionMetric" );
					ci.IpEnabled = WmiHelpers.GetNullableBoolean( obj, @"IPEnabled" );
					ci.MacAddress = WmiHelpers.GetString( obj, @"MACAddress" );
					ci.WinsEnableLmHostsLookup = WmiHelpers.GetNullableBoolean( obj, @"WINSEnableLMHostsLookup" );
					ci.WinsHostLookupFile = WmiHelpers.GetString( obj, @"WINSHostLookupFile" );
					ci.WinsPrimaryServer = WmiHelpers.GetString( obj, @"WINSPrimaryServer" );
					ci.WinsSecondaryServer = WmiHelpers.GetString( obj, @"WINSSecondaryServer" );
					ci.WinsScopeId = WmiHelpers.GetString( obj, @"WINSScopeID" );

					networkInfoList.Add( ci );
					return true;
				}, true, false );
			} catch( UnauthorizedAccessException ) {
				result.Add( new NetworkInfo( computerName, @"Authorization Error" ) );
				return;
			} catch( Exception ) {
				result.Add( new NetworkInfo( computerName, @"Error" ) );
				return;
			}
			result.AddRange( networkInfoList );

		}
	}
}
