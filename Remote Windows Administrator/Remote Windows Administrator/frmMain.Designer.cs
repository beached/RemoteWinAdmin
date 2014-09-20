namespace RemoteWindowsAdministrator {
	partial class FrmMain {
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
			this.tcMain = new System.Windows.Forms.TabControl();
			this.tpAddRemovePrograms = new System.Windows.Forms.TabPage();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblFilter = new System.Windows.Forms.Label();
			this.dgvInstalledPrograms = new System.Windows.Forms.DataGridView();
			this.gbComputer = new System.Windows.Forms.GroupBox();
			this.chkShowHidden = new System.Windows.Forms.CheckBox();
			this.txtComputerName = new System.Windows.Forms.TextBox();
			this.btnQueryRemoteComputer = new System.Windows.Forms.Button();
			this.tcMain.SuspendLayout();
			this.tpAddRemovePrograms.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvInstalledPrograms)).BeginInit();
			this.gbComputer.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tpAddRemovePrograms);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(0, 0);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(1164, 575);
			this.tcMain.TabIndex = 5;
			// 
			// tpAddRemovePrograms
			// 
			this.tpAddRemovePrograms.Controls.Add(this.txtFilter);
			this.tpAddRemovePrograms.Controls.Add(this.lblFilter);
			this.tpAddRemovePrograms.Controls.Add(this.dgvInstalledPrograms);
			this.tpAddRemovePrograms.Controls.Add(this.gbComputer);
			this.tpAddRemovePrograms.Location = new System.Drawing.Point(4, 22);
			this.tpAddRemovePrograms.Name = "tpAddRemovePrograms";
			this.tpAddRemovePrograms.Padding = new System.Windows.Forms.Padding(3);
			this.tpAddRemovePrograms.Size = new System.Drawing.Size(1156, 549);
			this.tpAddRemovePrograms.TabIndex = 0;
			this.tpAddRemovePrograms.Text = "Add/Remove Programs";
			this.tpAddRemovePrograms.UseVisualStyleBackColor = true;
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilter.Location = new System.Drawing.Point(46, 523);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(243, 20);
			this.txtFilter.TabIndex = 6;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// lblFilter
			// 
			this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilter.AutoSize = true;
			this.lblFilter.Location = new System.Drawing.Point(11, 525);
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(29, 13);
			this.lblFilter.TabIndex = 4;
			this.lblFilter.Text = "Filter";
			// 
			// dgvInstalledPrograms
			// 
			this.dgvInstalledPrograms.AllowUserToAddRows = false;
			this.dgvInstalledPrograms.AllowUserToDeleteRows = false;
			this.dgvInstalledPrograms.AllowUserToOrderColumns = true;
			this.dgvInstalledPrograms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvInstalledPrograms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvInstalledPrograms.Location = new System.Drawing.Point(7, 60);
			this.dgvInstalledPrograms.Name = "dgvInstalledPrograms";
			this.dgvInstalledPrograms.ReadOnly = true;
			this.dgvInstalledPrograms.ShowEditingIcon = false;
			this.dgvInstalledPrograms.Size = new System.Drawing.Size(1141, 455);
			this.dgvInstalledPrograms.TabIndex = 3;
			this.dgvInstalledPrograms.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvInstalledPrograms_CellMouseClick);
			// 
			// gbComputer
			// 
			this.gbComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputer.Controls.Add(this.chkShowHidden);
			this.gbComputer.Controls.Add(this.txtComputerName);
			this.gbComputer.Controls.Add(this.btnQueryRemoteComputer);
			this.gbComputer.Location = new System.Drawing.Point(8, 6);
			this.gbComputer.Name = "gbComputer";
			this.gbComputer.Size = new System.Drawing.Size(1140, 47);
			this.gbComputer.TabIndex = 3;
			this.gbComputer.TabStop = false;
			this.gbComputer.Text = "Computer";
			// 
			// chkShowHidden
			// 
			this.chkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowHidden.AutoSize = true;
			this.chkShowHidden.Location = new System.Drawing.Point(1044, 14);
			this.chkShowHidden.Name = "chkShowHidden";
			this.chkShowHidden.Size = new System.Drawing.Size(90, 17);
			this.chkShowHidden.TabIndex = 3;
			this.chkShowHidden.Text = "Show Hidden";
			this.chkShowHidden.UseVisualStyleBackColor = true;
			this.chkShowHidden.CheckedChanged += new System.EventHandler(this.chkShowHidden_CheckedChanged);
			// 
			// txtComputerName
			// 
			this.txtComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputerName.Location = new System.Drawing.Point(6, 13);
			this.txtComputerName.Name = "txtComputerName";
			this.txtComputerName.Size = new System.Drawing.Size(951, 20);
			this.txtComputerName.TabIndex = 0;
			this.txtComputerName.TextChanged += new System.EventHandler(this.txtComputerName_TextChanged);
			// 
			// btnQueryRemoteComputer
			// 
			this.btnQueryRemoteComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQueryRemoteComputer.Location = new System.Drawing.Point(963, 10);
			this.btnQueryRemoteComputer.Name = "btnQueryRemoteComputer";
			this.btnQueryRemoteComputer.Size = new System.Drawing.Size(75, 23);
			this.btnQueryRemoteComputer.TabIndex = 2;
			this.btnQueryRemoteComputer.Text = "&Query";
			this.btnQueryRemoteComputer.UseVisualStyleBackColor = true;
			this.btnQueryRemoteComputer.Click += new System.EventHandler(this.btnQueryRemoteComputer_Click);
			// 
			// FrmMain
			// 
			this.AcceptButton = this.btnQueryRemoteComputer;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1164, 575);
			this.Controls.Add(this.tcMain);
			this.Name = "FrmMain";
			this.Text = "DAW - Remote Windows Administrator";
			this.Shown += new System.EventHandler(this.FrmMain_Shown);
			this.tcMain.ResumeLayout(false);
			this.tpAddRemovePrograms.ResumeLayout(false);
			this.tpAddRemovePrograms.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvInstalledPrograms)).EndInit();
			this.gbComputer.ResumeLayout(false);
			this.gbComputer.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpAddRemovePrograms;
		private System.Windows.Forms.GroupBox gbComputer;
		private System.Windows.Forms.TextBox txtComputerName;
		private System.Windows.Forms.Button btnQueryRemoteComputer;
		private System.Windows.Forms.DataGridView dgvInstalledPrograms;
		private System.Windows.Forms.CheckBox chkShowHidden;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
	}
}

