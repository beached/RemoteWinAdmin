using System.Collections.Generic;
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
				CompletionMessage = @"Computer Software Query Complete", 
				QueryDataCb = PtComputerSoftware.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( PtComputerSoftware ) );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv,@"HelpLink" ) );
					DgvHelpers.ConvertToLinkColumn( DgvHelpers.GetColumn( dgv, @"UrlInfoAbout" ) );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"Guid" ), @"GUID" );
					DgvHelpers.SetColumnHeader( DgvHelpers.GetColumn( dgv, @"Size" ), @"Size(MB)" );
					MoveStatusColumnsFirst( dgv );
					foreach( var actionName in PtComputerSoftware.SetupActions(  ).Keys ) {
						DgvHelpers.AddButtonColumn( dgv, actionName );
					}					
				}
			} );
		}

		private void SetupComputerInfoTab( ) {
			AddDataPageToTabControl( "Computer Info", tcMain, new DataPageControl<PtComputerInfo>( this ) {
				QueryDataCb = PtComputerInfo.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( PtComputerInfo) );
					MoveStatusColumnsFirst( dgv );
					foreach( var actionName in PtComputerInfo.SetupActions( ).Keys ) {
						DgvHelpers.AddButtonColumn( dgv, actionName );
					}
				}
			} );
		}

		private void SetupCurrentUsersTab( ) {
			AddDataPageToTabControl( "Current Users", tcMain, new DataPageControl<PtCurrentUsers>( this ) {
				GenerateLookupMenu = false, 
				QueryDataCb = PtCurrentUsers.Generate, 
				SetupColumnsCb = delegate( DataGridView dgv ) {
				DgvHelpers.GenerateAllColumns( dgv, typeof( PtCurrentUsers ) );
				MoveStatusColumnsFirst( dgv );
				foreach( var actionName in PtCurrentUsers.SetupActions( ).Keys ) {
					DgvHelpers.AddButtonColumn( dgv, actionName );
				}
			}} );
		}

		private void SetupNetworkInfoTab( ) {
			AddDataPageToTabControl( "Network Info", tcMain, new DataPageControl<PtNetworkInfo>( this ) {
				QueryDataCb = PtNetworkInfo.Generate, SetupColumnsCb = delegate( DataGridView dgv ) {
					DgvHelpers.GenerateAllColumns( dgv, typeof( PtNetworkInfo ), new List<string>( ) {@"DefaultIpGateway", @"DnsDomainSuffixSearchOrder", @"DnsServerSearchOrder", @"IpAddress"} );
					MoveStatusColumnsFirst( dgv );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DnsServerSearchOrders" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"DefaultIpGateways" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"IpAddresses" ) );
					DgvHelpers.ConvertToMultilineColumn( DgvHelpers.GetColumn( dgv, @"WinsServers" ) );
					DgvHelpers.MoveColumnToIndex( DgvHelpers.GetColumn( dgv, @"Description" ), 2 );
					foreach( var actionName in PtNetworkInfo.SetupActions( ).Keys ) {
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
			DgvHelpers.MoveColumnToIndex( DgvHelpers.GetColumn( dgv, @"ConnectionStatus" ), 1 );
		}
	}

}
