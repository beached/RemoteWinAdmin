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
			this.txtSoftwareFilter = new System.Windows.Forms.TextBox();
			this.lblSoftwareFilter = new System.Windows.Forms.Label();
			this.dgvSoftware = new System.Windows.Forms.DataGridView();
			this.gbComputer = new System.Windows.Forms.GroupBox();
			this.chkSoftwareShowHidden = new System.Windows.Forms.CheckBox();
			this.txtSoftwareComputerName = new System.Windows.Forms.TextBox();
			this.btnQuerySoftware = new System.Windows.Forms.Button();
			this.tpComputerInfo = new System.Windows.Forms.TabPage();
			this.txtComputerInfoFilter = new System.Windows.Forms.TextBox();
			this.lblComputerInfoFilter = new System.Windows.Forms.Label();
			this.dgvComputerInfo = new System.Windows.Forms.DataGridView();
			this.gbComputerInfoComputers = new System.Windows.Forms.GroupBox();
			this.txtComputerInfoComputer = new System.Windows.Forms.TextBox();
			this.btnComputerInfoQuery = new System.Windows.Forms.Button();
			this.tpCurrentUsers = new System.Windows.Forms.TabPage();
			this.txtCurrentUsersFilter = new System.Windows.Forms.TextBox();
			this.lblCurrentUsersFilter = new System.Windows.Forms.Label();
			this.dgvCurrentUsers = new System.Windows.Forms.DataGridView();
			this.gbCurrentUsersComputer = new System.Windows.Forms.GroupBox();
			this.txtCurrentUsersComputer = new System.Windows.Forms.TextBox();
			this.btnQueryCurrentUsers = new System.Windows.Forms.Button();
			this.tcMain.SuspendLayout();
			this.tpSoftware.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSoftware)).BeginInit();
			this.gbComputer.SuspendLayout();
			this.tpComputerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).BeginInit();
			this.gbComputerInfoComputers.SuspendLayout();
			this.tpCurrentUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentUsers)).BeginInit();
			this.gbCurrentUsersComputer.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tpSoftware);
			this.tcMain.Controls.Add(this.tpComputerInfo);
			this.tcMain.Controls.Add(this.tpCurrentUsers);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(0, 0);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(1164, 575);
			this.tcMain.TabIndex = 5;
			// 
			// tpSoftware
			// 
			this.tpSoftware.Controls.Add(this.txtSoftwareFilter);
			this.tpSoftware.Controls.Add(this.lblSoftwareFilter);
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
			// txtSoftwareFilter
			// 
			this.txtSoftwareFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtSoftwareFilter.Location = new System.Drawing.Point(46, 523);
			this.txtSoftwareFilter.Name = "txtSoftwareFilter";
			this.txtSoftwareFilter.Size = new System.Drawing.Size(243, 20);
			this.txtSoftwareFilter.TabIndex = 6;
			this.txtSoftwareFilter.TextChanged += new System.EventHandler(this.txtSoftwareFilter_TextChanged);
			// 
			// lblSoftwareFilter
			// 
			this.lblSoftwareFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblSoftwareFilter.AutoSize = true;
			this.lblSoftwareFilter.Location = new System.Drawing.Point(11, 525);
			this.lblSoftwareFilter.Name = "lblSoftwareFilter";
			this.lblSoftwareFilter.Size = new System.Drawing.Size(29, 13);
			this.lblSoftwareFilter.TabIndex = 4;
			this.lblSoftwareFilter.Text = "Filter";
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
			this.dgvSoftware.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSoftware_CellMouseClick);
			// 
			// gbComputer
			// 
			this.gbComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputer.Controls.Add(this.chkSoftwareShowHidden);
			this.gbComputer.Controls.Add(this.txtSoftwareComputerName);
			this.gbComputer.Controls.Add(this.btnQuerySoftware);
			this.gbComputer.Location = new System.Drawing.Point(7, 6);
			this.gbComputer.Name = "gbComputer";
			this.gbComputer.Size = new System.Drawing.Size(1140, 47);
			this.gbComputer.TabIndex = 3;
			this.gbComputer.TabStop = false;
			this.gbComputer.Text = "Computer(s)";
			// 
			// chkSoftwareShowHidden
			// 
			this.chkSoftwareShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSoftwareShowHidden.AutoSize = true;
			this.chkSoftwareShowHidden.Location = new System.Drawing.Point(1044, 14);
			this.chkSoftwareShowHidden.Name = "chkSoftwareShowHidden";
			this.chkSoftwareShowHidden.Size = new System.Drawing.Size(90, 17);
			this.chkSoftwareShowHidden.TabIndex = 3;
			this.chkSoftwareShowHidden.Text = "Show Hidden";
			this.chkSoftwareShowHidden.UseVisualStyleBackColor = true;
			this.chkSoftwareShowHidden.CheckedChanged += new System.EventHandler(this.chkShowHidden_CheckedChanged);
			// 
			// txtSoftwareComputerName
			// 
			this.txtSoftwareComputerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSoftwareComputerName.Location = new System.Drawing.Point(6, 13);
			this.txtSoftwareComputerName.Name = "txtSoftwareComputerName";
			this.txtSoftwareComputerName.Size = new System.Drawing.Size(951, 20);
			this.txtSoftwareComputerName.TabIndex = 0;
			this.txtSoftwareComputerName.TextChanged += new System.EventHandler(this.txtSoftwareComputer_TextChanged);
			this.txtSoftwareComputerName.Enter += new System.EventHandler(this.txtSoftwareComputer_Enter);
			this.txtSoftwareComputerName.Leave += new System.EventHandler(this.txtSoftwareComputer_Leave);
			// 
			// btnQuerySoftware
			// 
			this.btnQuerySoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQuerySoftware.Location = new System.Drawing.Point(963, 10);
			this.btnQuerySoftware.Name = "btnQuerySoftware";
			this.btnQuerySoftware.Size = new System.Drawing.Size(75, 23);
			this.btnQuerySoftware.TabIndex = 2;
			this.btnQuerySoftware.Text = "&Query";
			this.btnQuerySoftware.UseVisualStyleBackColor = true;
			this.btnQuerySoftware.Click += new System.EventHandler(this.btnQuerySoftware_Click);
			// 
			// tpComputerInfo
			// 
			this.tpComputerInfo.Controls.Add(this.txtComputerInfoFilter);
			this.tpComputerInfo.Controls.Add(this.lblComputerInfoFilter);
			this.tpComputerInfo.Controls.Add(this.dgvComputerInfo);
			this.tpComputerInfo.Controls.Add(this.gbComputerInfoComputers);
			this.tpComputerInfo.Location = new System.Drawing.Point(4, 22);
			this.tpComputerInfo.Name = "tpComputerInfo";
			this.tpComputerInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tpComputerInfo.Size = new System.Drawing.Size(1156, 549);
			this.tpComputerInfo.TabIndex = 1;
			this.tpComputerInfo.Text = "Computer Info";
			this.tpComputerInfo.UseVisualStyleBackColor = true;
			// 
			// txtComputerInfoFilter
			// 
			this.txtComputerInfoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtComputerInfoFilter.Location = new System.Drawing.Point(41, 520);
			this.txtComputerInfoFilter.Name = "txtComputerInfoFilter";
			this.txtComputerInfoFilter.Size = new System.Drawing.Size(243, 20);
			this.txtComputerInfoFilter.TabIndex = 8;
			this.txtComputerInfoFilter.TextChanged += new System.EventHandler(this.txtComputerInfoFilter_TextChanged);
			// 
			// lblComputerInfoFilter
			// 
			this.lblComputerInfoFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblComputerInfoFilter.AutoSize = true;
			this.lblComputerInfoFilter.Location = new System.Drawing.Point(6, 522);
			this.lblComputerInfoFilter.Name = "lblComputerInfoFilter";
			this.lblComputerInfoFilter.Size = new System.Drawing.Size(29, 13);
			this.lblComputerInfoFilter.TabIndex = 7;
			this.lblComputerInfoFilter.Text = "Filter";
			// 
			// dgvComputerInfo
			// 
			this.dgvComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvComputerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvComputerInfo.Location = new System.Drawing.Point(6, 59);
			this.dgvComputerInfo.Name = "dgvComputerInfo";
			this.dgvComputerInfo.Size = new System.Drawing.Size(1144, 455);
			this.dgvComputerInfo.TabIndex = 5;
			this.dgvComputerInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvComputerInfo_CellMouseClick);
			// 
			// gbComputerInfoComputers
			// 
			this.gbComputerInfoComputers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputerInfoComputers.Controls.Add(this.txtComputerInfoComputer);
			this.gbComputerInfoComputers.Controls.Add(this.btnComputerInfoQuery);
			this.gbComputerInfoComputers.Location = new System.Drawing.Point(6, 6);
			this.gbComputerInfoComputers.Name = "gbComputerInfoComputers";
			this.gbComputerInfoComputers.Size = new System.Drawing.Size(1140, 47);
			this.gbComputerInfoComputers.TabIndex = 4;
			this.gbComputerInfoComputers.TabStop = false;
			this.gbComputerInfoComputers.Text = "Computer(s)";
			// 
			// txtComputerInfoComputer
			// 
			this.txtComputerInfoComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputerInfoComputer.Location = new System.Drawing.Point(6, 13);
			this.txtComputerInfoComputer.Name = "txtComputerInfoComputer";
			this.txtComputerInfoComputer.Size = new System.Drawing.Size(1047, 20);
			this.txtComputerInfoComputer.TabIndex = 0;
			this.txtComputerInfoComputer.TextChanged += new System.EventHandler(this.txtComputerInfoComputer_TextChanged);
			this.txtComputerInfoComputer.Enter += new System.EventHandler(this.txtComputerInfoComputer_Enter);
			this.txtComputerInfoComputer.Leave += new System.EventHandler(this.txtComputerInfoComputer_Leave);
			// 
			// btnComputerInfoQuery
			// 
			this.btnComputerInfoQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnComputerInfoQuery.Location = new System.Drawing.Point(1059, 11);
			this.btnComputerInfoQuery.Name = "btnComputerInfoQuery";
			this.btnComputerInfoQuery.Size = new System.Drawing.Size(75, 23);
			this.btnComputerInfoQuery.TabIndex = 2;
			this.btnComputerInfoQuery.Text = "&Query";
			this.btnComputerInfoQuery.UseVisualStyleBackColor = true;
			this.btnComputerInfoQuery.Click += new System.EventHandler(this.btnQueryComputerInfo_Click);
			// 
			// tpCurrentUsers
			// 
			this.tpCurrentUsers.Controls.Add(this.txtCurrentUsersFilter);
			this.tpCurrentUsers.Controls.Add(this.lblCurrentUsersFilter);
			this.tpCurrentUsers.Controls.Add(this.dgvCurrentUsers);
			this.tpCurrentUsers.Controls.Add(this.gbCurrentUsersComputer);
			this.tpCurrentUsers.Location = new System.Drawing.Point(4, 22);
			this.tpCurrentUsers.Name = "tpCurrentUsers";
			this.tpCurrentUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tpCurrentUsers.Size = new System.Drawing.Size(1156, 549);
			this.tpCurrentUsers.TabIndex = 2;
			this.tpCurrentUsers.Text = "Current Users";
			this.tpCurrentUsers.UseVisualStyleBackColor = true;
			// 
			// txtCurrentUsersFilter
			// 
			this.txtCurrentUsersFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtCurrentUsersFilter.Location = new System.Drawing.Point(41, 521);
			this.txtCurrentUsersFilter.Name = "txtCurrentUsersFilter";
			this.txtCurrentUsersFilter.Size = new System.Drawing.Size(243, 20);
			this.txtCurrentUsersFilter.TabIndex = 12;
			this.txtCurrentUsersFilter.TextChanged += new System.EventHandler(this.txtCurrentUsersFilter_TextChanged);
			// 
			// lblCurrentUsersFilter
			// 
			this.lblCurrentUsersFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblCurrentUsersFilter.AutoSize = true;
			this.lblCurrentUsersFilter.Location = new System.Drawing.Point(6, 523);
			this.lblCurrentUsersFilter.Name = "lblCurrentUsersFilter";
			this.lblCurrentUsersFilter.Size = new System.Drawing.Size(29, 13);
			this.lblCurrentUsersFilter.TabIndex = 11;
			this.lblCurrentUsersFilter.Text = "Filter";
			// 
			// dgvCurrentUsers
			// 
			this.dgvCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvCurrentUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentUsers.Location = new System.Drawing.Point(6, 60);
			this.dgvCurrentUsers.Name = "dgvCurrentUsers";
			this.dgvCurrentUsers.Size = new System.Drawing.Size(1144, 455);
			this.dgvCurrentUsers.TabIndex = 10;
			this.dgvCurrentUsers.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCurrentUsers_CellMouseClick);
			// 
			// gbCurrentUsersComputer
			// 
			this.gbCurrentUsersComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbCurrentUsersComputer.Controls.Add(this.txtCurrentUsersComputer);
			this.gbCurrentUsersComputer.Controls.Add(this.btnQueryCurrentUsers);
			this.gbCurrentUsersComputer.Location = new System.Drawing.Point(6, 7);
			this.gbCurrentUsersComputer.Name = "gbCurrentUsersComputer";
			this.gbCurrentUsersComputer.Size = new System.Drawing.Size(1140, 47);
			this.gbCurrentUsersComputer.TabIndex = 9;
			this.gbCurrentUsersComputer.TabStop = false;
			this.gbCurrentUsersComputer.Text = "Computer(s)";
			// 
			// txtCurrentUsersComputer
			// 
			this.txtCurrentUsersComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtCurrentUsersComputer.Location = new System.Drawing.Point(6, 13);
			this.txtCurrentUsersComputer.Name = "txtCurrentUsersComputer";
			this.txtCurrentUsersComputer.Size = new System.Drawing.Size(1047, 20);
			this.txtCurrentUsersComputer.TabIndex = 0;
			this.txtCurrentUsersComputer.TextChanged += new System.EventHandler(this.txtCurrentUsersComputer_TextChanged);
			this.txtCurrentUsersComputer.Enter += new System.EventHandler(this.txtCurrentUsersComputer_Enter);
			this.txtCurrentUsersComputer.Leave += new System.EventHandler(this.txtCurrentUsersComputer_Leave);
			// 
			// btnQueryCurrentUsers
			// 
			this.btnQueryCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQueryCurrentUsers.Location = new System.Drawing.Point(1059, 11);
			this.btnQueryCurrentUsers.Name = "btnQueryCurrentUsers";
			this.btnQueryCurrentUsers.Size = new System.Drawing.Size(75, 23);
			this.btnQueryCurrentUsers.TabIndex = 2;
			this.btnQueryCurrentUsers.Text = "&Query";
			this.btnQueryCurrentUsers.UseVisualStyleBackColor = true;
			this.btnQueryCurrentUsers.Click += new System.EventHandler(this.btnQueryCurrentUsers_Click);
			// 
			// FrmMain
			// 
			this.AcceptButton = this.btnQuerySoftware;
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
			this.tpComputerInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).EndInit();
			this.gbComputerInfoComputers.ResumeLayout(false);
			this.gbComputerInfoComputers.PerformLayout();
			this.tpCurrentUsers.ResumeLayout(false);
			this.tpCurrentUsers.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentUsers)).EndInit();
			this.gbCurrentUsersComputer.ResumeLayout(false);
			this.gbCurrentUsersComputer.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpSoftware;
		private System.Windows.Forms.GroupBox gbComputer;
		private System.Windows.Forms.TextBox txtSoftwareComputerName;
		private System.Windows.Forms.Button btnQuerySoftware;
		private System.Windows.Forms.DataGridView dgvSoftware;
		private System.Windows.Forms.CheckBox chkSoftwareShowHidden;
		private System.Windows.Forms.TextBox txtSoftwareFilter;
		private System.Windows.Forms.Label lblSoftwareFilter;
		private System.Windows.Forms.TabPage tpComputerInfo;
		private System.Windows.Forms.DataGridView dgvComputerInfo;
		private System.Windows.Forms.GroupBox gbComputerInfoComputers;
		private System.Windows.Forms.TextBox txtComputerInfoComputer;
		private System.Windows.Forms.Button btnComputerInfoQuery;
		private System.Windows.Forms.TextBox txtComputerInfoFilter;
		private System.Windows.Forms.Label lblComputerInfoFilter;
		private System.Windows.Forms.TabPage tpCurrentUsers;
		private System.Windows.Forms.TextBox txtCurrentUsersFilter;
		private System.Windows.Forms.Label lblCurrentUsersFilter;
		private System.Windows.Forms.DataGridView dgvCurrentUsers;
		private System.Windows.Forms.GroupBox gbCurrentUsersComputer;
		private System.Windows.Forms.TextBox txtCurrentUsersComputer;
		private System.Windows.Forms.Button btnQueryCurrentUsers;
	}
}

