using daw;
using SyncList;
using System;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class FrmMain: Form {

		public FrmMain( ) {
			InitializeComponent( );
			SetupSoftwareTab( );
			SetupComputerInfoTab( );
			SetupCurrentUsersTab( );
			SetupNetworkInfoTab( );
		}

		private void SetupSoftwareTab( ) {
			AddDataPageToTabControl( @"Software", tcMain, new DataPageControl<ComputerSoftware>( this, new SyncList<ComputerSoftware>( this ) ) {
				CompletionMessage = @"Computer Software Query Complete", GenerateLookupMenu = true,
				QueryDataCb = ComputerSoftware.GetComputerSoftware, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.AddColumn( dgv, @"ComputerName", @"Computer Name" );
					DgvHelpers.AddColumn( dgv, @"ConnectionStatus", @"Connection Status" );
					DgvHelpers.AddColumn( dgv, @"Name" );
					DgvHelpers.AddColumn( dgv, @"Publisher" );
					DgvHelpers.AddColumn( dgv, @"Version" );
					DgvHelpers.AddDateColumn( dgv, @"InstallDate", @"Install Date" );
					DgvHelpers.AddColumn( dgv, @"Size", @"Size(MB)" );
					DgvHelpers.AddButtonColumn( dgv, @"Uninstall" );
					DgvHelpers.AddLinkColumn( dgv, @"HelpLink", @"Help Link" );
					DgvHelpers.AddLinkColumn( dgv, @"UrlInfoAbout", @"About Link" );
					DgvHelpers.AddColumn( dgv, @"Guid", @"GUID" );
				},
				OnCellButtonClick = delegate( DataGridView dgv, int rowIndex, int columnIndex ) {
					Helpers.Assert( null != dgv && 0 <= rowIndex && rowIndex < dgv.RowCount && 0 <= columnIndex && columnIndex < dgv.ColumnCount );
					var cell = dgv.Rows[rowIndex].Cells[columnIndex];
					var cellValue = cell.Value as string;
					if( string.IsNullOrEmpty( cellValue ) || 0 != string.Compare( cellValue, @"Uninstall", StringComparison.Ordinal ) ) {
						return false;
					}
					var guid = DgvHelpers.GetCellString( dgv, rowIndex, @"GUID" );
					Helpers.Assert( !string.IsNullOrEmpty( guid ), @"No GUID for selected row.  This should never happen" );
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return false;
					}
					var computerName = DgvHelpers.GetCellString( dgv, rowIndex, @"Computer Name" );
					Helpers.Assert( !string.IsNullOrEmpty( guid ), @"No Computer Name for selected row.  This should never happen" );
					ComputerSoftware.UninstallGuidOnComputerName( computerName, guid );
					return true;
				}
			} );
		}

		private void SetupComputerInfoTab( ) {
			AddDataPageToTabControl( "Computer Info", tcMain, new DataPageControl<ComputerInfo>( this, new SyncList<ComputerInfo>( this ) ) {
				CompletionMessage = @"Computer Info Query Complete", GenerateLookupMenu = true,
				QueryDataCb = ComputerInfo.GetComputerInfo, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.AddColumn( dgv, @"ComputerName", @"Computer Name" );
					DgvHelpers.AddColumn( dgv, @"ConnectionStatus", @"Connection Status" );
					DgvHelpers.AddDateColumn( dgv, @"LastBootTime", @"Boot Time", false, true, MagicValues.TimeDateStringFormat );
					DgvHelpers.AddColumn( dgv, @"Uptime" );
					DgvHelpers.AddColumn( dgv, @"Version", @"Windows Version" );
					DgvHelpers.AddColumn( dgv, @"Architecture" );
					DgvHelpers.AddDateColumn( dgv, @"InstallDate", "Windows\nInstall Date" );
					DgvHelpers.AddColumn( dgv, @"Manufacturer" );
					DgvHelpers.AddDateColumn( dgv, @"HwReleaseDate", @"Hardware Date" );
					DgvHelpers.AddColumn( dgv, @"SerialNumber", @"Serial Number" );
					DgvHelpers.AddColumn( dgv, @"BiosVersion", @"BIOS Version" );
					DgvHelpers.AddButtonColumn( dgv, @"Shutdown" );
				},
				OnCellButtonClick = delegate( DataGridView dgv, int rowIndex, int columnIndex ) {
					Helpers.Assert( null != dgv && 0 <= rowIndex && rowIndex < dgv.RowCount && 0 <= columnIndex && columnIndex < dgv.ColumnCount );

					var cellValue = DgvHelpers.GetCellString( dgv, rowIndex, columnIndex );
					if( string.IsNullOrEmpty( cellValue ) || 0 != string.Compare( cellValue, @"Shutdown", StringComparison.Ordinal ) ) {
						return false;
					}

					var computerName = DgvHelpers.GetCellString( dgv, rowIndex, @"Computer Name" );
					Helpers.Assert( !string.IsNullOrEmpty( computerName ), @"Computer Name is null or empty.  This should never happen" );
					using( var csd = new ConfirmShutdownDialog( new ComputerInfo.ShutdownComputerParameters( computerName ) ) ) {
						csd.ShowDialog( );
					}
					return false;
				}
			} );
		}

		private void SetupCurrentUsersTab( ) {
			AddDataPageToTabControl( "Current Users", tcMain, new DataPageControl<CurrentUsers>( this, new SyncList<CurrentUsers>( this ) ) {
				CompletionMessage = @"Current User Query Complete", GenerateLookupMenu = false,
				QueryDataCb = CurrentUsers.GetCurrentUsers, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.AddColumn( dgv, @"ComputerName", @"Computer Name" );
					DgvHelpers.AddColumn( dgv, @"ConnectionStatus", @"Connection Status" );
					DgvHelpers.AddColumn( dgv, @"Domain" );
					DgvHelpers.AddColumn( dgv, @"UserName", @"UserName" );
					DgvHelpers.AddDateColumn( dgv, @"LastLogon", @"Last Login", false, true, MagicValues.TimeDateStringFormat );
					DgvHelpers.AddColumn( dgv, @"LogonDuration", @"Login Duration" );
					DgvHelpers.AddColumn( dgv, @"Sid", @"SID" );
					DgvHelpers.AddColumn( dgv, @"ProfileFolder", @"Profile" );
				}
			} );
		}

		private void SetupNetworkInfoTab( ) {
			AddDataPageToTabControl( "Network Info", tcMain, new DataPageControl<NetworkInfo>( this, new SyncList<NetworkInfo>( this ) ) {
				CompletionMessage = @"Network Info Query Complete", GenerateLookupMenu = true,
				QueryDataCb = NetworkInfo.GetNetworkInfo, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.AddColumn( dgv, @"ComputerName", @"Computer Name" );
					DgvHelpers.AddColumn( dgv, @"ConnectionStatus", @"Connection Status" );
					DgvHelpers.AddColumn( dgv, @"Caption" );
					DgvHelpers.AddColumn( dgv, @"Description" );
					DgvHelpers.AddCheckedColumn( dgv, @"DhcpEnabled", "DHCP\nEnabled" );
					DgvHelpers.AddDateColumn( dgv, @"DhcpLeaseObtained", "DHCP\nLease Obtained", false, true, MagicValues.TimeDateStringFormat );
					DgvHelpers.AddDateColumn( dgv, @"DhcpLeaseExpires", "DHCP\nLease Expires", false, true, MagicValues.TimeDateStringFormat );
					DgvHelpers.AddColumn( dgv, @"DhcpLeaseTimeLeft", "DHCP\nLease Time Left" );
					DgvHelpers.AddColumn( dgv, @"DhcpServer", "DHCP\nServer" );
					DgvHelpers.AddColumn( dgv, @"DnsDomain", @"DNS Domain" );
					DgvHelpers.AddMultilineColumn( dgv, @"DnsDomainSuffixSearchOrders", "DNS Domain\nSearch Order" );
					DgvHelpers.AddCheckedColumn( dgv, @"DnsEnabledForWinsResolution", "DNS Enabled For\nWINS Resolution" );
					DgvHelpers.AddColumn( dgv, @"DnsHostName", @"DNS Host Name" );
					DgvHelpers.AddMultilineColumn( dgv, @"DnsServerSearchOrders", "DNS Server\nSearch Order" );
					DgvHelpers.AddCheckedColumn( dgv, @"DomainDnsRegistrationEnabled", "Domain DNS\nRegistration Enabled" );
					DgvHelpers.AddColumn( dgv, @"DefaultIpGateways", @"Default IP Gateways" );
					DgvHelpers.AddCheckedColumn( dgv, @"FullDnsRegistrationEnabled", "Full DNS\nRegistration Enabled" );
					DgvHelpers.AddColumn( dgv, @"MacAddress", @"MAC Address" );
					DgvHelpers.AddColumn( dgv, @"Index" );
					DgvHelpers.AddColumn( dgv, @"InterfaceIndex", @"Interface Index" );
					DgvHelpers.AddMultilineColumn( dgv, @"IpAddresses", @"IP Addresses" );
					DgvHelpers.AddColumn( dgv, @"IpConnectionMetric", "IP Connection\nMetric" );
					DgvHelpers.AddCheckedColumn( dgv, @"IpEnabled", @"IP Enabled" );
					DgvHelpers.AddCheckedColumn( dgv, @"WinsEnableLmHostsLookup", "WINS Enable\nLM Hosts Lookup" );
					DgvHelpers.AddColumn( dgv, @"WinsHostLookupFile", "WINS Host\nLookup File" );
					DgvHelpers.AddMultilineColumn( dgv, @"WinsServers", @"WINS Servers" );
					DgvHelpers.AddColumn( dgv, @"WinsScopeId", "WINS\nScope ID" );
				}
			} );			
		}

		// Helpers
		private static void AddDataPageToTabControl<T>( string name, TabControl tabControl, DataPageControl<T> dataPageControl ) where T: IDataPageRow, new( ) {
			var page = new TabPage( name );
			dataPageControl.Dock = DockStyle.Fill;
			page.Controls.Add( dataPageControl );
			tabControl.TabPages.Add( page );
		}	
	}
}
