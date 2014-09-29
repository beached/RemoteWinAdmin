using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RemoteWindowsAdministrator {
	public partial class ConfirmShutdownDialog: Form {
		private readonly DprComputerInfo.ShutdownComputerParameters _shutdownParameters;

		public ConfirmShutdownDialog( DprComputerInfo.ShutdownComputerParameters shutdownParameters ) {
			_shutdownParameters = shutdownParameters;
			InitializeComponent( );
		}

		private void LoadValues( DprComputerInfo.ShutdownComputerParameters parameters ) {
			txtComputer.Text = parameters.ComputerName;

			foreach( var shutdownType in (DprComputerInfo.ShutdownComputerParameters.ShutdownTypes[])Enum.GetValues( typeof( DprComputerInfo.ShutdownComputerParameters.ShutdownTypes ) ) ) {
				cmbTypeOfShutdown.Items.Add( shutdownType.ToString( ) );
			}
			cmbTypeOfShutdown.SelectedItem = parameters.ShutdownType.ToString( );

			chkForced.Checked = parameters.Forced;
			numTimeout.Minimum = UInt32.MinValue;
			numTimeout.Maximum = UInt32.MaxValue;
			numTimeout.Value = parameters.Timeout;
			txtMessage.Text = parameters.Comment;
		}

		private void BindControlsToValues( ) {
			cmbTypeOfShutdown.SelectedIndexChanged += ( o, args ) => DprComputerInfo.ShutdownComputerParameters.ShutdownTypesFromShort( (short)cmbTypeOfShutdown.SelectedIndex );
			chkForced.CheckedChanged += ( o, args ) => _shutdownParameters.Forced = chkForced.Checked;
			numTimeout.ValueChanged += ( o, args ) => _shutdownParameters.Timeout = Decimal.ToUInt32( numTimeout.Value );
			txtMessage.TextChanged += ( o, args ) => _shutdownParameters.Comment = txtMessage.Text.Trim( );
		}

		private void ConfirmShutdownDialog_Load( object sender, EventArgs e ) {
			BindControlsToValues(  );
			LoadValues( _shutdownParameters );
		}

		private void btnOK_Click( object sender, EventArgs e ) {
			var message = string.Format( @"Are you sure that you want to do a {0}{1} on {2} in {3} seconds?", _shutdownParameters.Forced ? " forced " : string.Empty, _shutdownParameters.ShutdownType.ToString( ), _shutdownParameters.ComputerName, _shutdownParameters.Timeout );
			if( DialogResult.Yes == MessageBox.Show( message, @"Confirm", MessageBoxButtons.YesNo ) ) {
				DprComputerInfo.ShutdownComputer( _shutdownParameters );
			}
		}

		private void btnCancel_Click( object sender, EventArgs e ) {
			Close(  );
		}
	}

}
