﻿using Microsoft.Win32;
using SyncList;
using System;
using System.Collections.Generic;

namespace RemoteWindowsAdministrator {
	public sealed class DprNetworkInfo: IDataPageRow {
		private string _computerName;
		public string ComputerName {
			get {
				Helpers.AssertString( _computerName, @"Computer name is mandatory and must be set" );
				return _computerName;
			}
			set {
				Helpers.AssertString( value, @"Attempt to set ComputerName to a null or empty value" );
				_computerName = value;
			}
		}


		public ConnectionStatuses ConnectionStatus { get; set; }
		public string ConnectionStatusString {
			get { return Helpers.CamelToSpace( ConnectionStatus.ToString( ) ); }
		}


		public DateTime? DhcpLeaseExpires { get; set; }
		public DateTime? DhcpLeaseObtained { get; set; }
		public uint Index { get; set; }
		public uint InterfaceIndex { get; set; }
		public uint? IpConnectionMetric { get; set; }
		public bool DhcpEnabled { get; set; }
		public bool? DnsEnabledForWinsResolution { get; set; }
		public bool? DomainDnsRegistrationEnabled { get; set; }
		public bool? FullDnsRegistrationEnabled { get; set; }
		public bool? IpEnabled { get; set; }
		public bool? WinsEnableLmHostsLookup { get; set; }
		public string Description { get; set; }
		public string DhcpServer { get; set; }
		public string DnsDomain { get; set; }
		public string DnsHostName { get; set; }
		public string MacAddress { get; set; }
		public string SettingId { get; set; }
		public string WinsHostLookupFile { get; set; }
		public string WinsPrimaryServer { get; set; }
		public string WinsScopeId { get; set; }
		public string WinsSecondaryServer { get; set; }
		public string[] DefaultIpGateway { get; set; }
		public string[] DnsDomainSuffixSearchOrder { get; set; }
		public string[] DnsServerSearchOrder { get; set; }
		public string[] IpAddress { get; set; }

		public string DefaultIpGateways { get { return Helpers.StraToStr( DefaultIpGateway ); } }
		public string DhcpLeaseTimeLeft {
			get {
				return null == DhcpLeaseExpires ? null : MagicValues.TimeSpanToString( DhcpLeaseExpires.Value - DateTime.Now );
			}
		}
		public string DnsDomainSuffixSearchOrders { get { return Helpers.StraToStr( DnsDomainSuffixSearchOrder ); } }
		public string DnsServerSearchOrders { get { return Helpers.StraToStr( DnsServerSearchOrder ); } }
		public string IpAddresses { get { return Helpers.StraToStr( IpAddress ); } }
		public string WinsServers {
			get {
				var result = new List<string>( );
				if( !string.IsNullOrEmpty( WinsPrimaryServer ) ) {
					result.Add( WinsPrimaryServer );
				}
				if( !string.IsNullOrEmpty( WinsSecondaryServer ) ) {
					result.Add( WinsSecondaryServer );
				}
				return Helpers.StraToStr( result );
			}
		}
		public Guid RowGuid {
			get;
			private set;
		}

		public IDictionary<string, Func<IDataPageRow, bool>> GetActions( ) {
			return SetupActions( );
		}

		public DprNetworkInfo( ) {
			ConnectionStatus = ConnectionStatuses.Ok;
			RowGuid = Guid.NewGuid( );
		}

		public DprNetworkInfo( string computerName, ConnectionStatuses connectionStatus = ConnectionStatuses.Ok ) {
			ComputerName = computerName;
			ConnectionStatus = connectionStatus;
			RowGuid = Guid.NewGuid( );
		}

		public static IDictionary<string, Func<IDataPageRow, bool>> SetupActions( ) {
			return new Dictionary<string, Func<IDataPageRow, bool>> {
			{@"Renew Lease", delegate( IDataPageRow rowObj ) {
				var row = rowObj as DprNetworkInfo;
				Helpers.AssertNotNull( row, @"PtNetworkInfo Action called with another class as second parameter" );
				RunWin32ConfigurationFunction( row.ComputerName, row.InterfaceIndex, @"RenewDHCPLease" );
				return true;
			}},
			{@"Enable DHCP",delegate( IDataPageRow rowObj ) {
				var row = rowObj as DprNetworkInfo;
				Helpers.AssertNotNull( row, @"PtNetworkInfo Action called with another class as second parameter" );
				if( !SetDnsServers( row.ComputerName, row.SettingId ) ) {
					NotificationWindow.NotificationWindow.AddErrorMessage( @"Error setting DNS to use DHCP" );
				}
				RunWin32ConfigurationFunction( row.ComputerName, row.InterfaceIndex, @"EnableDHCP" );
				return true;
			}}
			};
		}

		public bool ContainsString( string value ) {
			return (new ValueIsIn( value )).Add( ComputerName ).Add( ConnectionStatus ).Add( DefaultIpGateway ).Add( Description ).Add( DhcpEnabled ).Add( DhcpLeaseExpires ).Add( DhcpLeaseObtained ).Add( DhcpServer ).Add( DnsDomain ).Add( DnsDomainSuffixSearchOrder ).Add( DnsEnabledForWinsResolution ).Add( DnsHostName ).Add( DnsServerSearchOrder ).Add( DomainDnsRegistrationEnabled ).Add( FullDnsRegistrationEnabled ).Add( Index ).Add( InterfaceIndex ).Add( IpAddress ).Add( IpConnectionMetric ).Add( IpEnabled ).Add( MacAddress ).Add( WinsEnableLmHostsLookup ).Add( WinsHostLookupFile ).Add( WinsPrimaryServer ).Add( WinsSecondaryServer ).Add( WinsScopeId ).IsContained;
		}

		public bool Valid( ) {
			return !string.IsNullOrEmpty( ComputerName );
		}

		public static void Generate( string computerName, SyncList<DprNetworkInfo> result ) {
			Helpers.AssertNotNull( result, @"result SyncList cannot be null" );
			Helpers.AssertString( computerName, @"Computer name cannot be empty" );
			var networkInfoList = new List<DprNetworkInfo>( );
			try {
				WmiHelpers.ForEach( computerName, @"SELECT * FROM Win32_NetworkAdapterConfiguration", obj => {
					var ci = new DprNetworkInfo( computerName );
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
					ci.SettingId = WmiHelpers.GetString( obj, @"SettingID" );
					ci.WinsEnableLmHostsLookup = WmiHelpers.GetNullableBoolean( obj, @"WINSEnableLMHostsLookup" );
					ci.WinsHostLookupFile = WmiHelpers.GetString( obj, @"WINSHostLookupFile" );
					ci.WinsPrimaryServer = WmiHelpers.GetString( obj, @"WINSPrimaryServer" );
					ci.WinsSecondaryServer = WmiHelpers.GetString( obj, @"WINSSecondaryServer" );
					ci.WinsScopeId = WmiHelpers.GetString( obj, @"WINSScopeID" );

					networkInfoList.Add( ci );
					return true;
				}, true, false );
			} catch( UnauthorizedAccessException ) {
				result.Add( new DprNetworkInfo( computerName, ConnectionStatuses.AuthorizationError ) );
				return;
			} catch( Exception ) {
				result.Add( new DprNetworkInfo( computerName, ConnectionStatuses.Error ) );
				return;
			}
			result.AddRange( networkInfoList );
			ValidateUniqueness( result );
		}

		public static void ValidateUniqueness( SyncList<DprNetworkInfo> rows ) {
			var guids = new HashSet<Guid>( );
			foreach( var item in rows ) {
				Helpers.Assert( !guids.Contains( item.RowGuid ), @"RowGuid's must be unique" );
				guids.Add( item.RowGuid );
			}
		}


		public enum NetworkAdapterConfigurationReturnCodes: uint {
			Sucessful = 0,
			SucessfulRebootRequired = 1,
			MethodNotSupported = 64,
			UnknownFailure = 65,
			InvalidSubnetMask = 66,
			ErrorWhileProcessing = 67,
			InvalidInputParameter = 68,
			MoreThanFiveGatewaysSpecified = 69,
			InvalidIpAddress = 70,
			InvalidGatewayIpAddress = 71,
			RegistryError = 72,
			InvalidDomainName = 73,
			InvalidHostName = 74,
			NoWinsServersSpecified = 75,
			InvalidFile = 76,
			InvalidSystemPath = 77,
			FileCopyFailed = 78,
			InvalidSecurityParameter = 79,
			UnableToConfigureTcpIpService = 80,
			UnableToConfigureDhcpService = 81,
			UnableToRenewDhcpLease = 82,
			UnableToReleaseDhcpLease = 83,
			IpNotEnabledOnAdapter = 84,
			IpxNotEnabledOnAdapter = 85,
			FrameOrNetworkNumberBoundsError = 86,
			InvalidFrameType = 87,
			InvalidNetworkNumber = 88,
			DuplicateNetworkNumber = 89,
			ParameterOutOfBounds = 90,
			AccessDenied = 91,
			OutOfMemory = 92,
			AlreadyExists = 93,
			PathFileOrObjectNotFound = 94,
			UnableToNotifyService = 95,
			UnableToNotifyDnsService = 96,
			InterfaceNotConfigurable = 97,
			NotAllDhcpLeasesCouldBeReleasedOrRenewed = 98,
			DhcpNotEnabledOnAdapter = 100
		}

		private static void RunWin32ConfigurationFunction( string computerName, uint interfaceIndex, string functionName ) {
			Helpers.AssertString( computerName, @"computerName is not specified, it is required" );
			Helpers.AssertString( functionName, @"functionName is not specified, it is required" );			
			WmiHelpers.ForEach( computerName, string.Format( @"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE InterfaceIndex={0}", interfaceIndex ), obj => {
				var outParams = obj.InvokeMethod( functionName, null, null );
				Helpers.AssertNotNull( outParams, @"Return value from {0} was null.  This is not allowed", functionName );
				var retVal = (NetworkAdapterConfigurationReturnCodes)uint.Parse( outParams[@"returnValue"].ToString( ) );
				switch( retVal ) {
				case NetworkAdapterConfigurationReturnCodes.Sucessful:
					return true;
				case NetworkAdapterConfigurationReturnCodes.SucessfulRebootRequired:
					NotificationWindow.NotificationWindow.AddErrorMessage( @"Computer '{0}' successfully ran {1} but requires a reboot.", computerName, functionName );
					return true;
				default:
					NotificationWindow.NotificationWindow.AddErrorMessage( @"Error running {0} on '{1}'.  Returned a value of {2}", functionName, computerName, retVal );
					return false;
				}
			} );
		}

		private static bool ValidateNameServer( string nameServer ) {
			var parts = nameServer.Split( '.' );
			if( 4 != parts.Length ) {
				return false;
			}
			foreach( var part in parts ) {
				int currentValue;
				if( !int.TryParse( part, out currentValue ) ) {
					return false;
				}
				if( !(0 <= currentValue && currentValue <= 255) ) {
					return false;
				}
			}
			return true;
		}

		private static bool SetDnsServers( string computerName, string settingId, IList<string> nameServers = null ) {
			Helpers.AssertString( computerName, @"computerName is not specified, it is required" );
			Helpers.AssertString( settingId, @"settingId is not specified, it is required" );
			if( null == nameServers ) {
				nameServers = new List<string>( );
			}
			var allNameServers = string.Empty;
			var isFirst = true;
			foreach( var nameServer in nameServers ) {
				Helpers.Assert( ValidateNameServer( nameServer ), @"Nameserver '{0}' is not valid", nameServer );
				if( isFirst ) {
					isFirst = false;
				} else {
					allNameServers += @",";
				}
				allNameServers += nameServer;
			}
			var registryPath = string.Format( @"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\{0}", settingId );
			using( var regKey = RegistryKey.OpenRemoteBaseKey( RegistryHive.LocalMachine, computerName ).OpenSubKey( registryPath, true ) ) {
				if( null == regKey ) {
					return false;
				}
				try {
					regKey.SetValue( @"NameServer", allNameServers );
				} catch {
					return false;
				}
			}
			return true;
		}
	}
}
