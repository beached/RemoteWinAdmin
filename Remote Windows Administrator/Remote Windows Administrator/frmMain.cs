using System.Collections.Generic;
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
			AddDataPageToTabControl( @"Software", tcMain, new DataPageControl<PtComputerSoftware>( this ) {
				CompletionMessage = @"Computer Software Query Complete", GenerateLookupMenu = true,
				QueryDataCb = PtComputerSoftware.Generate, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateColumns( dgv, typeof( PtComputerSoftware ), new List<string>( ) {
						@"ComputerName", @"ConnectionStatus", @"Name", @"Publisher", @"Version", @"InstallDate", @"Size", @"HelpLink", @"UrlInfoAbout", @"Guid"
					} );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv,@"HelpLink" ) );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv, @"UrlInfoAbout" ) );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"Guid" ), @"GUID" );
					DgvHelpers.AddButtonColumn( dgv, @"Uninstall" );
				},
				OnCellButtonClick = delegate( DataGridView dgv, int rowIndex, int columnIndex ) {
					Helpers.Assert( null != dgv && 0 <= rowIndex && rowIndex < dgv.RowCount && 0 <= columnIndex && columnIndex < dgv.ColumnCount );
					var cell = dgv.Rows[rowIndex].Cells[columnIndex];
					var cellValue = cell.Value as string;
					if( string.IsNullOrEmpty( cellValue ) || 0 != string.Compare( cellValue, @"Uninstall", StringComparison.Ordinal ) ) {
						return false;
					}
					var guid = DgvHelpers.GetCellString( dgv, rowIndex, @"Guid" );
					Helpers.Assert( !string.IsNullOrEmpty( guid ), @"No GUID for selected row.  This should never happen" );
					if( DialogResult.Yes != MessageBox.Show( @"Are you sure?", @"Alert", MessageBoxButtons.YesNo ) ) {
						return false;
					}
					var computerName = DgvHelpers.GetCellString( dgv, rowIndex, @"Computer Name" );
					Helpers.Assert( !string.IsNullOrEmpty( guid ), @"No Computer Name for selected row.  This should never happen" );
					PtComputerSoftware.UninstallGuidOnComputerName( computerName, guid );
					return true;
				}
			} );
		}

		private void SetupComputerInfoTab( ) {
			AddDataPageToTabControl( "Computer Info", tcMain, new DataPageControl<PtComputerInfo>( this ) {
				CompletionMessage = @"Computer Info Query Complete", GenerateLookupMenu = true,
				QueryDataCb = PtComputerInfo.Generate, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateColumns( dgv, typeof( PtComputerInfo ), new List<string>( ) {
						@"ComputerName", @"ConnectionStatus", @"LastBootTime", @"Uptime", @"Version", @"Architecture",
						@"InstallDate", @"Manufacturer", @"HwReleaseDate", @"SerialNumber", @"BiosVersion"
					} );
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
					using( var csd = new ConfirmShutdownDialog( new PtComputerInfo.ShutdownComputerParameters( computerName ) ) ) {
						csd.ShowDialog( );
					}
					return false;
				}
			} );
		}

		private void SetupCurrentUsersTab( ) {
			AddDataPageToTabControl( "Current Users", tcMain, new DataPageControl<PtCurrentUsers>( this ) {
				CompletionMessage = @"Current User Query Complete", GenerateLookupMenu = false,
				QueryDataCb = PtCurrentUsers.Generate, SetupColumnsCb = dgv => DgvHelpers.GenerateColumns( dgv, typeof( PtCurrentUsers ), new List<string>( ) {
					@"ComputerName", @"ConnectionStatus", @"Domain", @"UserName", @"LastLogon", @"Sid", @"ProfileFolder"
				} )
			} );
		}

		private void SetupNetworkInfoTab( ) {
			AddDataPageToTabControl( "Network Info", tcMain, new DataPageControl<PtNetworkInfo>( this ) {
				CompletionMessage = @"Network Info Query Complete", GenerateLookupMenu = true,
				QueryDataCb = PtNetworkInfo.Generate, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateColumns( dgv, typeof( PtNetworkInfo ), new List<string>( ) {
						@"ComputerName", @"ConnectionStatus", @"Caption", @"Description", @"DhcpEnabled", @"DhcpLeaseObtained", @"DhcpLeaseExpires", @"DhcpLeaseTimeLeft", @"DhcpServer", @"DnsDomain", @"DnsDomainSuffixSearchOrders", @"DnsEnabledForWinsResolution", @"DnsHostName", @"DnsServerSearchOrders", @"DomainDnsRegistrationEnabled", @"DefaultIpGateways", @"FullDnsRegistrationEnabled", @"MacAddress", @"Index", @"InterfaceIndex", @"IpAddresses", @"IpConnectionMetric", @"IpEnabled", @"WinsEnableLmHostsLookup", @"WinsHostLookupFile", @"WinsServers", @"WinsScopeId"
					} );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DnsServerSearchOrders" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DefaultIpGateways" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"IpAddresses" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"WinsServers" ) );
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
