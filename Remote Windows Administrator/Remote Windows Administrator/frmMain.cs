using System.Collections.Generic;
using System.IO;
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
			AddDataPageToTabControl( @"Software", tcMain, new DataPageControl<DprComputerSoftware>( this ) {
				CompletionMessage = @"Computer Software Query Complete", 
				QueryDataCb = DprComputerSoftware.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( DprComputerSoftware ), new List<string> { @"ConnectionStatus" } );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv,@"HelpLink" ) );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv, @"UrlInfoAbout" ) );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"Guid" ), @"GUID" );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"Size" ), @"Size(MB)" );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"ConnectionStatusString" ), @"Connection Status" );
					MoveStatusColumnsFirst( dgv );
					foreach( var actionName in DprComputerSoftware.SetupActions(  ).Keys ) {
						DgvHelpers.AddButtonColumn( dgv, actionName );
					}					
				}
			} );
		}

		private void SetupComputerInfoTab( ) {
			AddDataPageToTabControl( "Computer Info", tcMain, new DataPageControl<DprComputerInfo>( this ) {
				QueryDataCb = DprComputerInfo.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( DprComputerInfo ), new List<string> { @"ConnectionStatus" } );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"ConnectionStatusString" ), @"Connection Status" );
					MoveStatusColumnsFirst( dgv );
					foreach( var actionName in DprComputerInfo.SetupActions( ).Keys ) {
						DgvHelpers.AddButtonColumn( dgv, actionName );
					}
				}
			} );
		}

		private void SetupCurrentUsersTab( ) {
			AddDataPageToTabControl( "Current Users", tcMain, new DataPageControl<DprCurrentUsers>( this ) {
				GenerateLookupMenu = false, 
				QueryDataCb = DprCurrentUsers.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
				DgvHelpers.GenerateAllColumns( dgv, typeof( DprCurrentUsers ) );
				DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"ConnectionStatusString" ), @"Connection Status" );
				MoveStatusColumnsFirst( dgv );
				foreach( var actionName in DprCurrentUsers.SetupActions( ).Keys ) {
					DgvHelpers.AddButtonColumn( dgv, actionName );
				}
			}} );
		}

		private void SetupNetworkInfoTab( ) {
			AddDataPageToTabControl( "Network Info", tcMain, new DataPageControl<DprNetworkInfo>( this ) {
				QueryDataCb = DprNetworkInfo.Generate, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( DprNetworkInfo ), new List<string> {@"DefaultIpGateway", @"DnsDomainSuffixSearchOrder", @"DnsServerSearchOrder", @"IpAddress", @"ConnectionStatus"} );
					MoveStatusColumnsFirst( dgv );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DefaultIpGateways" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DnsDomainSuffixSearchOrders" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DnsServerSearchOrders" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"IpAddresses" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"WinsServers" ) );
					DgvHelpers.MoveColumnToIndex( DgvHelpers.GetColumn( dgv, @"Description" ), 2 );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"ConnectionStatusString" ), @"Connection Status" );
					DgvHelpers.SetVisible( DgvHelpers.GetColumn( dgv, @"InterfaceIndex" ), false );
					DgvHelpers.SetVisible( DgvHelpers.GetColumn( dgv, @"SettingId" ), false );
					foreach( var actionName in DprNetworkInfo.SetupActions( ).Keys ) {
						DgvHelpers.AddButtonColumn( dgv, actionName );
					}
				}
			} );			
		}

		// Helpers
		private static void AddDataPageToTabControl<T>( string name, TabControl tabControl, DataPageControl<T> dataPageControl ) where T: IDataPageRow, new( ) {
			var page = new TabPage( name );
			dataPageControl.Dock = DockStyle.Fill;
			page.Controls.Add( dataPageControl );
			dataPageControl.CompletionMessage = string.Format( @"{0} Query Complete", name );
			tabControl.TabPages.Add( page );
		}

		private static void MoveStatusColumnsFirst( DataGridView dgv ) {
			DgvHelpers.MoveColumnToIndex( DgvHelpers.GetColumn( dgv, @"ComputerName" ), 0 );
			DgvHelpers.MoveColumnToIndex( DgvHelpers.GetColumn( dgv, @"ConnectionStatusString" ), 1 );
		}
	}

}
