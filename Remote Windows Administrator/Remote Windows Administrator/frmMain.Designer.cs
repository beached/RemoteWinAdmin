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
			this.tpSoftware = new System.Windows.Forms.TabPage();
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblFilter = new System.Windows.Forms.Label();
			this.dgvSoftware = new System.Windows.Forms.DataGridView();
			this.gbComputer = new System.Windows.Forms.GroupBox();
			this.chkShowHidden = new System.Windows.Forms.CheckBox();
			this.txtComputerName = new System.Windows.Forms.TextBox();
			this.btnQueryRemoteComputer = new System.Windows.Forms.Button();
			this.tpComputerInfo = new System.Windows.Forms.TabPage();
			this.dgvComputerInfo = new System.Windows.Forms.DataGridView();
			this.gbInfoComputerName = new System.Windows.Forms.GroupBox();
			this.txtInfoComputerName = new System.Windows.Forms.TextBox();
			this.btnInfoQuery = new System.Windows.Forms.Button();
			this.tcMain.SuspendLayout();
			this.tpSoftware.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSoftware)).BeginInit();
			this.gbComputer.SuspendLayout();
			this.tpComputerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).BeginInit();
			this.gbInfoComputerName.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tpSoftware);
			this.tcMain.Controls.Add(this.tpComputerInfo);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(0, 0);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(1164, 575);
			this.tcMain.TabIndex = 5;
			// 
			// tpSoftware
			// 
			this.tpSoftware.Controls.Add(this.txtFilter);
			this.tpSoftware.Controls.Add(this.lblFilter);
			this.tpSoftware.Controls.Add(this.dgvSoftware);
			this.tpSoftware.Controls.Add(this.gbComputer);
			this.tpSoftware.Location = new System.Drawing.Point(4, 22);
			this.tpSoftware.Name = "tpSoftware";
			this.tpSoftware.Padding = new System.Windows.Forms.Padding(3);
			this.tpSoftware.Size = new System.Drawing.Size(1156, 549);
			this.tpSoftware.TabIndex = 0;
			this.tpSoftware.Text = "Software";
			this.tpSoftware.UseVisualStyleBackColor = true;
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
			// dgvSoftware
			// 
			this.dgvSoftware.AllowUserToAddRows = false;
			this.dgvSoftware.AllowUserToDeleteRows = false;
			this.dgvSoftware.AllowUserToOrderColumns = true;
			this.dgvSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvSoftware.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSoftware.Location = new System.Drawing.Point(7, 60);
			this.dgvSoftware.Name = "dgvSoftware";
			this.dgvSoftware.ReadOnly = true;
			this.dgvSoftware.ShowEditingIcon = false;
			this.dgvSoftware.Size = new System.Drawing.Size(1143, 455);
			this.dgvSoftware.TabIndex = 3;
			this.dgvSoftware.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvInstalledPrograms_CellMouseClick);
			// 
			// gbComputer
			// 
			this.gbComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputer.Controls.Add(this.chkShowHidden);
			this.gbComputer.Controls.Add(this.txtComputerName);
			this.gbComputer.Controls.Add(this.btnQueryRemoteComputer);
			this.gbComputer.Location = new System.Drawing.Point(7, 6);
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
			// tpComputerInfo
			// 
			this.tpComputerInfo.Controls.Add(this.dgvComputerInfo);
			this.tpComputerInfo.Controls.Add(this.gbInfoComputerName);
			this.tpComputerInfo.Location = new System.Drawing.Point(4, 22);
			this.tpComputerInfo.Name = "tpComputerInfo";
			this.tpComputerInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tpComputerInfo.Size = new System.Drawing.Size(1156, 549);
			this.tpComputerInfo.TabIndex = 1;
			this.tpComputerInfo.Text = "Computer Info";
			this.tpComputerInfo.UseVisualStyleBackColor = true;
			// 
			// dgvComputerInfo
			// 
			this.dgvComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvComputerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvComputerInfo.Location = new System.Drawing.Point(6, 59);
			this.dgvComputerInfo.Name = "dgvComputerInfo";
			this.dgvComputerInfo.Size = new System.Drawing.Size(1144, 487);
			this.dgvComputerInfo.TabIndex = 5;
			this.dgvComputerInfo.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComputerInfo_CellContentClick);
			// 
			// gbInfoComputerName
			// 
			this.gbInfoComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbInfoComputerName.Controls.Add(this.txtInfoComputerName);
			this.gbInfoComputerName.Controls.Add(this.btnInfoQuery);
			this.gbInfoComputerName.Location = new System.Drawing.Point(6, 6);
			this.gbInfoComputerName.Name = "gbInfoComputerName";
			this.gbInfoComputerName.Size = new System.Drawing.Size(1140, 47);
			this.gbInfoComputerName.TabIndex = 4;
			this.gbInfoComputerName.TabStop = false;
			this.gbInfoComputerName.Text = "Computer(s)";
			// 
			// txtInfoComputerName
			// 
			this.txtInfoComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInfoComputerName.Location = new System.Drawing.Point(6, 13);
			this.txtInfoComputerName.Name = "txtInfoComputerName";
			this.txtInfoComputerName.Size = new System.Drawing.Size(1047, 20);
			this.txtInfoComputerName.TabIndex = 0;
			// 
			// btnInfoQuery
			// 
			this.btnInfoQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnInfoQuery.Location = new System.Drawing.Point(1059, 11);
			this.btnInfoQuery.Name = "btnInfoQuery";
			this.btnInfoQuery.Size = new System.Drawing.Size(75, 23);
			this.btnInfoQuery.TabIndex = 2;
			this.btnInfoQuery.Text = "&Query";
			this.btnInfoQuery.UseVisualStyleBackColor = true;
			this.btnInfoQuery.Click += new System.EventHandler(this.btnInfoQuery_Click);
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
			this.tpSoftware.ResumeLayout(false);
			this.tpSoftware.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSoftware)).EndInit();
			this.gbComputer.ResumeLayout(false);
			this.gbComputer.PerformLayout();
			this.tpComputerInfo.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).EndInit();
			this.gbInfoComputerName.ResumeLayout(false);
			this.gbInfoComputerName.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpSoftware;
		private System.Windows.Forms.GroupBox gbComputer;
		private System.Windows.Forms.TextBox txtComputerName;
		private System.Windows.Forms.Button btnQueryRemoteComputer;
		private System.Windows.Forms.DataGridView dgvSoftware;
		private System.Windows.Forms.CheckBox chkShowHidden;
		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.TabPage tpComputerInfo;
		private System.Windows.Forms.DataGridView dgvComputerInfo;
		private System.Windows.Forms.GroupBox gbInfoComputerName;
		private System.Windows.Forms.TextBox txtInfoComputerName;
		private System.Windows.Forms.Button btnInfoQuery;
	}
}

