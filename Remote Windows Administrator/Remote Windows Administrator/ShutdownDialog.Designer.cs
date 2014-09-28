namespace RemoteWindowsAdministrator {
	partial class ConfirmShutdownDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if( disposing && (components != null) ) {
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( ) {
			this.txtShutdown = new System.Windows.Forms.Label();
			this.txtComputer = new System.Windows.Forms.TextBox();
			this.cmbTypeOfShutdown = new System.Windows.Forms.ComboBox();
			this.lblTypeOfShutdown = new System.Windows.Forms.Label();
			this.lblTimeout = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkForced = new System.Windows.Forms.CheckBox();
			this.numTimeout = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
			this.SuspendLayout();
			// 
			// txtShutdown
			// 
			this.txtShutdown.AutoSize = true;
			this.txtShutdown.Location = new System.Drawing.Point(14, 16);
			this.txtShutdown.Name = "txtShutdown";
			this.txtShutdown.Size = new System.Drawing.Size(75, 13);
			this.txtShutdown.TabIndex = 0;
			this.txtShutdown.Text = "Shutting down";
			// 
			// txtComputer
			// 
			this.txtComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputer.Location = new System.Drawing.Point(95, 13);
			this.txtComputer.Name = "txtComputer";
			this.txtComputer.ReadOnly = true;
			this.txtComputer.Size = new System.Drawing.Size(170, 20);
			this.txtComputer.TabIndex = 1;
			// 
			// cmbTypeOfShutdown
			// 
			this.cmbTypeOfShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbTypeOfShutdown.FormattingEnabled = true;
			this.cmbTypeOfShutdown.Location = new System.Drawing.Point(95, 39);
			this.cmbTypeOfShutdown.Name = "cmbTypeOfShutdown";
			this.cmbTypeOfShutdown.Size = new System.Drawing.Size(170, 21);
			this.cmbTypeOfShutdown.TabIndex = 2;
			// 
			// lblTypeOfShutdown
			// 
			this.lblTypeOfShutdown.AutoSize = true;
			this.lblTypeOfShutdown.Location = new System.Drawing.Point(14, 43);
			this.lblTypeOfShutdown.Name = "lblTypeOfShutdown";
			this.lblTypeOfShutdown.Size = new System.Drawing.Size(31, 13);
			this.lblTypeOfShutdown.TabIndex = 3;
			this.lblTypeOfShutdown.Text = "Type";
			// 
			// lblTimeout
			// 
			this.lblTimeout.AutoSize = true;
			this.lblTimeout.Location = new System.Drawing.Point(14, 71);
			this.lblTimeout.Name = "lblTimeout";
			this.lblTimeout.Size = new System.Drawing.Size(97, 13);
			this.lblTimeout.TabIndex = 4;
			this.lblTimeout.Text = "Timeoout(seconds)";
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(14, 97);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(50, 13);
			this.lblMessage.TabIndex = 7;
			this.lblMessage.Text = "Message";
			// 
			// txtMessage
			// 
			this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtMessage.Location = new System.Drawing.Point(70, 94);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.Size = new System.Drawing.Size(195, 20);
			this.txtMessage.TabIndex = 8;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Location = new System.Drawing.Point(108, 124);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(189, 124);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkForced
			// 
			this.chkForced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkForced.AutoSize = true;
			this.chkForced.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkForced.Location = new System.Drawing.Point(205, 70);
			this.chkForced.Name = "chkForced";
			this.chkForced.Size = new System.Drawing.Size(59, 17);
			this.chkForced.TabIndex = 11;
			this.chkForced.Text = "Forced";
			this.chkForced.UseVisualStyleBackColor = true;
			// 
			// numTimeout
			// 
			this.numTimeout.Location = new System.Drawing.Point(118, 67);
			this.numTimeout.Name = "numTimeout";
			this.numTimeout.Size = new System.Drawing.Size(81, 20);
			this.numTimeout.TabIndex = 12;
			// 
			// ConfirmShutdownDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(277, 155);
			this.ControlBox = false;
			this.Controls.Add(this.numTimeout);
			this.Controls.Add(this.chkForced);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtMessage);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.lblTimeout);
			this.Controls.Add(this.lblTypeOfShutdown);
			this.Controls.Add(this.cmbTypeOfShutdown);
			this.Controls.Add(this.txtComputer);
			this.Controls.Add(this.txtShutdown);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfirmShutdownDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Shutdown Confirmation";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ConfirmShutdownDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label txtShutdown;
		private System.Windows.Forms.TextBox txtComputer;
		private System.Windows.Forms.ComboBox cmbTypeOfShutdown;
		private System.Windows.Forms.Label lblTypeOfShutdown;
		private System.Windows.Forms.Label lblTimeout;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox chkForced;
		private System.Windows.Forms.NumericUpDown numTimeout;
	}
}