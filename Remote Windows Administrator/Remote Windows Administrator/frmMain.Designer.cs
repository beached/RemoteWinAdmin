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
			this.txtFilterSoftware = new System.Windows.Forms.TextBox();
			this.lblFilterSoftware = new System.Windows.Forms.Label();
			this.dgvSoftware = new System.Windows.Forms.DataGridView();
			this.gbComputersSoftware = new System.Windows.Forms.GroupBox();
			this.txtComputersNameSoftware = new System.Windows.Forms.TextBox();
			this.btnQuerySoftware = new System.Windows.Forms.Button();
			this.tpComputerInfo = new System.Windows.Forms.TabPage();
			this.txtFilterComputerInfo = new System.Windows.Forms.TextBox();
			this.lblFilterComputerInfo = new System.Windows.Forms.Label();
			this.dgvComputerInfo = new System.Windows.Forms.DataGridView();
			this.gbComputersComputerInfo = new System.Windows.Forms.GroupBox();
			this.txtComputersComputerInfo = new System.Windows.Forms.TextBox();
			this.btnQueryComputerInfo = new System.Windows.Forms.Button();
			this.tpCurrentUsers = new System.Windows.Forms.TabPage();
			this.txtFilterCurrentUsers = new System.Windows.Forms.TextBox();
			this.lblFilterCurrentUsers = new System.Windows.Forms.Label();
			this.dgvCurrentUsers = new System.Windows.Forms.DataGridView();
			this.gbComputersCurrentUsers = new System.Windows.Forms.GroupBox();
			this.txtComputersCurrentUsers = new System.Windows.Forms.TextBox();
			this.btnQueryCurrentUsers = new System.Windows.Forms.Button();
			this.tpNetworkInfo = new System.Windows.Forms.TabPage();
			this.txtFilterNetworkInfo = new System.Windows.Forms.TextBox();
			this.lblFilterNetworkInfo = new System.Windows.Forms.Label();
			this.dgvNetworkInfo = new System.Windows.Forms.DataGridView();
			this.gbComputersNetworkInfo = new System.Windows.Forms.GroupBox();
			this.txtComputersNetworkInfo = new System.Windows.Forms.TextBox();
			this.btnQueryNetworkInfo = new System.Windows.Forms.Button();
			this.tcMain.SuspendLayout();
			this.tpSoftware.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvSoftware)).BeginInit();
			this.gbComputersSoftware.SuspendLayout();
			this.tpComputerInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).BeginInit();
			this.gbComputersComputerInfo.SuspendLayout();
			this.tpCurrentUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentUsers)).BeginInit();
			this.gbComputersCurrentUsers.SuspendLayout();
			this.tpNetworkInfo.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvNetworkInfo)).BeginInit();
			this.gbComputersNetworkInfo.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcMain
			// 
			this.tcMain.Controls.Add(this.tpSoftware);
			this.tcMain.Controls.Add(this.tpComputerInfo);
			this.tcMain.Controls.Add(this.tpCurrentUsers);
			this.tcMain.Controls.Add(this.tpNetworkInfo);
			this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tcMain.Location = new System.Drawing.Point(0, 0);
			this.tcMain.Name = "tcMain";
			this.tcMain.SelectedIndex = 0;
			this.tcMain.Size = new System.Drawing.Size(1164, 575);
			this.tcMain.TabIndex = 5;
			// 
			// tpSoftware
			// 
			this.tpSoftware.Controls.Add(this.txtFilterSoftware);
			this.tpSoftware.Controls.Add(this.lblFilterSoftware);
			this.tpSoftware.Controls.Add(this.dgvSoftware);
			this.tpSoftware.Controls.Add(this.gbComputersSoftware);
			this.tpSoftware.Location = new System.Drawing.Point(4, 22);
			this.tpSoftware.Name = "tpSoftware";
			this.tpSoftware.Padding = new System.Windows.Forms.Padding(3);
			this.tpSoftware.Size = new System.Drawing.Size(1156, 549);
			this.tpSoftware.TabIndex = 0;
			this.tpSoftware.Text = "Software";
			this.tpSoftware.UseVisualStyleBackColor = true;
			// 
			// txtFilterSoftware
			// 
			this.txtFilterSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilterSoftware.Location = new System.Drawing.Point(46, 523);
			this.txtFilterSoftware.Name = "txtFilterSoftware";
			this.txtFilterSoftware.Size = new System.Drawing.Size(243, 20);
			this.txtFilterSoftware.TabIndex = 6;
			this.txtFilterSoftware.TextChanged += new System.EventHandler(this.txtFilterSoftware_TextChanged);
			// 
			// lblFilterSoftware
			// 
			this.lblFilterSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilterSoftware.AutoSize = true;
			this.lblFilterSoftware.Location = new System.Drawing.Point(11, 525);
			this.lblFilterSoftware.Name = "lblFilterSoftware";
			this.lblFilterSoftware.Size = new System.Drawing.Size(29, 13);
			this.lblFilterSoftware.TabIndex = 4;
			this.lblFilterSoftware.Text = "Filter";
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
			// gbComputersSoftware
			// 
			this.gbComputersSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputersSoftware.Controls.Add(this.txtComputersNameSoftware);
			this.gbComputersSoftware.Controls.Add(this.btnQuerySoftware);
			this.gbComputersSoftware.Location = new System.Drawing.Point(7, 6);
			this.gbComputersSoftware.Name = "gbComputersSoftware";
			this.gbComputersSoftware.Size = new System.Drawing.Size(1140, 47);
			this.gbComputersSoftware.TabIndex = 3;
			this.gbComputersSoftware.TabStop = false;
			this.gbComputersSoftware.Text = "Computer(s)";
			// 
			// txtComputersNameSoftware
			// 
			this.txtComputersNameSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputersNameSoftware.Location = new System.Drawing.Point(6, 13);
			this.txtComputersNameSoftware.Name = "txtComputersNameSoftware";
			this.txtComputersNameSoftware.Size = new System.Drawing.Size(1047, 20);
			this.txtComputersNameSoftware.TabIndex = 0;
			this.txtComputersNameSoftware.TextChanged += new System.EventHandler(this.txtComputersSoftware_TextChanged);
			this.txtComputersNameSoftware.Enter += new System.EventHandler(this.txtComputersSoftware_Enter);
			this.txtComputersNameSoftware.Leave += new System.EventHandler(this.txtComputersSoftware_Leave);
			// 
			// btnQuerySoftware
			// 
			this.btnQuerySoftware.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQuerySoftware.Location = new System.Drawing.Point(1059, 13);
			this.btnQuerySoftware.Name = "btnQuerySoftware";
			this.btnQuerySoftware.Size = new System.Drawing.Size(75, 23);
			this.btnQuerySoftware.TabIndex = 2;
			this.btnQuerySoftware.Text = "&Query";
			this.btnQuerySoftware.UseVisualStyleBackColor = true;
			this.btnQuerySoftware.Click += new System.EventHandler(this.btnQuerySoftware_Click);
			// 
			// tpComputerInfo
			// 
			this.tpComputerInfo.Controls.Add(this.txtFilterComputerInfo);
			this.tpComputerInfo.Controls.Add(this.lblFilterComputerInfo);
			this.tpComputerInfo.Controls.Add(this.dgvComputerInfo);
			this.tpComputerInfo.Controls.Add(this.gbComputersComputerInfo);
			this.tpComputerInfo.Location = new System.Drawing.Point(4, 22);
			this.tpComputerInfo.Name = "tpComputerInfo";
			this.tpComputerInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tpComputerInfo.Size = new System.Drawing.Size(1156, 549);
			this.tpComputerInfo.TabIndex = 1;
			this.tpComputerInfo.Text = "Computer Info";
			this.tpComputerInfo.UseVisualStyleBackColor = true;
			// 
			// txtFilterComputerInfo
			// 
			this.txtFilterComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilterComputerInfo.Location = new System.Drawing.Point(41, 520);
			this.txtFilterComputerInfo.Name = "txtFilterComputerInfo";
			this.txtFilterComputerInfo.Size = new System.Drawing.Size(243, 20);
			this.txtFilterComputerInfo.TabIndex = 8;
			this.txtFilterComputerInfo.TextChanged += new System.EventHandler(this.txtFilterComputerInfo_TextChanged);
			// 
			// lblFilterComputerInfo
			// 
			this.lblFilterComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilterComputerInfo.AutoSize = true;
			this.lblFilterComputerInfo.Location = new System.Drawing.Point(6, 522);
			this.lblFilterComputerInfo.Name = "lblFilterComputerInfo";
			this.lblFilterComputerInfo.Size = new System.Drawing.Size(29, 13);
			this.lblFilterComputerInfo.TabIndex = 7;
			this.lblFilterComputerInfo.Text = "Filter";
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
			// gbComputersComputerInfo
			// 
			this.gbComputersComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputersComputerInfo.Controls.Add(this.txtComputersComputerInfo);
			this.gbComputersComputerInfo.Controls.Add(this.btnQueryComputerInfo);
			this.gbComputersComputerInfo.Location = new System.Drawing.Point(6, 6);
			this.gbComputersComputerInfo.Name = "gbComputersComputerInfo";
			this.gbComputersComputerInfo.Size = new System.Drawing.Size(1140, 47);
			this.gbComputersComputerInfo.TabIndex = 4;
			this.gbComputersComputerInfo.TabStop = false;
			this.gbComputersComputerInfo.Text = "Computer(s)";
			// 
			// txtComputersComputerInfo
			// 
			this.txtComputersComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputersComputerInfo.Location = new System.Drawing.Point(6, 13);
			this.txtComputersComputerInfo.Name = "txtComputersComputerInfo";
			this.txtComputersComputerInfo.Size = new System.Drawing.Size(1047, 20);
			this.txtComputersComputerInfo.TabIndex = 0;
			this.txtComputersComputerInfo.TextChanged += new System.EventHandler(this.txtComputersComputerInfo_TextChanged);
			this.txtComputersComputerInfo.Enter += new System.EventHandler(this.txtComputersComputerInfo_Enter);
			this.txtComputersComputerInfo.Leave += new System.EventHandler(this.txtComputersComputerInfo_Leave);
			// 
			// btnQueryComputerInfo
			// 
			this.btnQueryComputerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQueryComputerInfo.Location = new System.Drawing.Point(1059, 13);
			this.btnQueryComputerInfo.Name = "btnQueryComputerInfo";
			this.btnQueryComputerInfo.Size = new System.Drawing.Size(75, 23);
			this.btnQueryComputerInfo.TabIndex = 2;
			this.btnQueryComputerInfo.Text = "&Query";
			this.btnQueryComputerInfo.UseVisualStyleBackColor = true;
			this.btnQueryComputerInfo.Click += new System.EventHandler(this.btnQueryComputerInfo_Click);
			// 
			// tpCurrentUsers
			// 
			this.tpCurrentUsers.Controls.Add(this.txtFilterCurrentUsers);
			this.tpCurrentUsers.Controls.Add(this.lblFilterCurrentUsers);
			this.tpCurrentUsers.Controls.Add(this.dgvCurrentUsers);
			this.tpCurrentUsers.Controls.Add(this.gbComputersCurrentUsers);
			this.tpCurrentUsers.Location = new System.Drawing.Point(4, 22);
			this.tpCurrentUsers.Name = "tpCurrentUsers";
			this.tpCurrentUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tpCurrentUsers.Size = new System.Drawing.Size(1156, 549);
			this.tpCurrentUsers.TabIndex = 2;
			this.tpCurrentUsers.Text = "Current Users";
			this.tpCurrentUsers.UseVisualStyleBackColor = true;
			// 
			// txtFilterCurrentUsers
			// 
			this.txtFilterCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilterCurrentUsers.Location = new System.Drawing.Point(41, 521);
			this.txtFilterCurrentUsers.Name = "txtFilterCurrentUsers";
			this.txtFilterCurrentUsers.Size = new System.Drawing.Size(243, 20);
			this.txtFilterCurrentUsers.TabIndex = 12;
			this.txtFilterCurrentUsers.TextChanged += new System.EventHandler(this.txtFilterCurrentUsers_TextChanged);
			// 
			// lblFilterCurrentUsers
			// 
			this.lblFilterCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilterCurrentUsers.AutoSize = true;
			this.lblFilterCurrentUsers.Location = new System.Drawing.Point(6, 523);
			this.lblFilterCurrentUsers.Name = "lblFilterCurrentUsers";
			this.lblFilterCurrentUsers.Size = new System.Drawing.Size(29, 13);
			this.lblFilterCurrentUsers.TabIndex = 11;
			this.lblFilterCurrentUsers.Text = "Filter";
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
			// gbComputersCurrentUsers
			// 
			this.gbComputersCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputersCurrentUsers.Controls.Add(this.txtComputersCurrentUsers);
			this.gbComputersCurrentUsers.Controls.Add(this.btnQueryCurrentUsers);
			this.gbComputersCurrentUsers.Location = new System.Drawing.Point(6, 7);
			this.gbComputersCurrentUsers.Name = "gbComputersCurrentUsers";
			this.gbComputersCurrentUsers.Size = new System.Drawing.Size(1140, 47);
			this.gbComputersCurrentUsers.TabIndex = 9;
			this.gbComputersCurrentUsers.TabStop = false;
			this.gbComputersCurrentUsers.Text = "Computer(s)";
			// 
			// txtComputersCurrentUsers
			// 
			this.txtComputersCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputersCurrentUsers.Location = new System.Drawing.Point(6, 13);
			this.txtComputersCurrentUsers.Name = "txtComputersCurrentUsers";
			this.txtComputersCurrentUsers.Size = new System.Drawing.Size(1047, 20);
			this.txtComputersCurrentUsers.TabIndex = 0;
			this.txtComputersCurrentUsers.TextChanged += new System.EventHandler(this.txtComputersCurrentUsers_TextChanged);
			this.txtComputersCurrentUsers.Enter += new System.EventHandler(this.txtComputersCurrentUsers_Enter);
			this.txtComputersCurrentUsers.Leave += new System.EventHandler(this.txtComputersCurrentUsers_Leave);
			// 
			// btnQueryCurrentUsers
			// 
			this.btnQueryCurrentUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQueryCurrentUsers.Location = new System.Drawing.Point(1059, 13);
			this.btnQueryCurrentUsers.Name = "btnQueryCurrentUsers";
			this.btnQueryCurrentUsers.Size = new System.Drawing.Size(75, 23);
			this.btnQueryCurrentUsers.TabIndex = 2;
			this.btnQueryCurrentUsers.Text = "&Query";
			this.btnQueryCurrentUsers.UseVisualStyleBackColor = true;
			this.btnQueryCurrentUsers.Click += new System.EventHandler(this.btnQueryCurrentUsers_Click);
			// 
			// tpNetworkInfo
			// 
			this.tpNetworkInfo.Controls.Add(this.txtFilterNetworkInfo);
			this.tpNetworkInfo.Controls.Add(this.lblFilterNetworkInfo);
			this.tpNetworkInfo.Controls.Add(this.dgvNetworkInfo);
			this.tpNetworkInfo.Controls.Add(this.gbComputersNetworkInfo);
			this.tpNetworkInfo.Location = new System.Drawing.Point(4, 22);
			this.tpNetworkInfo.Name = "tpNetworkInfo";
			this.tpNetworkInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tpNetworkInfo.Size = new System.Drawing.Size(1156, 549);
			this.tpNetworkInfo.TabIndex = 3;
			this.tpNetworkInfo.Text = "Network Info";
			this.tpNetworkInfo.UseVisualStyleBackColor = true;
			// 
			// txtFilterNetworkInfo
			// 
			this.txtFilterNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilterNetworkInfo.Location = new System.Drawing.Point(41, 521);
			this.txtFilterNetworkInfo.Name = "txtFilterNetworkInfo";
			this.txtFilterNetworkInfo.Size = new System.Drawing.Size(243, 20);
			this.txtFilterNetworkInfo.TabIndex = 16;
			this.txtFilterNetworkInfo.TextChanged += new System.EventHandler(this.txtFilterNetworkInfo_TextChanged);
			// 
			// lblFilterNetworkInfo
			// 
			this.lblFilterNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilterNetworkInfo.AutoSize = true;
			this.lblFilterNetworkInfo.Location = new System.Drawing.Point(6, 523);
			this.lblFilterNetworkInfo.Name = "lblFilterNetworkInfo";
			this.lblFilterNetworkInfo.Size = new System.Drawing.Size(29, 13);
			this.lblFilterNetworkInfo.TabIndex = 15;
			this.lblFilterNetworkInfo.Text = "Filter";
			// 
			// dgvNetworkInfo
			// 
			this.dgvNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvNetworkInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvNetworkInfo.Location = new System.Drawing.Point(6, 60);
			this.dgvNetworkInfo.Name = "dgvNetworkInfo";
			this.dgvNetworkInfo.Size = new System.Drawing.Size(1144, 455);
			this.dgvNetworkInfo.TabIndex = 14;
			this.dgvNetworkInfo.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvNetworkInfo_CellMouseClick);
			// 
			// gbComputersNetworkInfo
			// 
			this.gbComputersNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputersNetworkInfo.Controls.Add(this.txtComputersNetworkInfo);
			this.gbComputersNetworkInfo.Controls.Add(this.btnQueryNetworkInfo);
			this.gbComputersNetworkInfo.Location = new System.Drawing.Point(6, 7);
			this.gbComputersNetworkInfo.Name = "gbComputersNetworkInfo";
			this.gbComputersNetworkInfo.Size = new System.Drawing.Size(1140, 47);
			this.gbComputersNetworkInfo.TabIndex = 13;
			this.gbComputersNetworkInfo.TabStop = false;
			this.gbComputersNetworkInfo.Text = "Computer(s)";
			// 
			// txtComputersNetworkInfo
			// 
			this.txtComputersNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputersNetworkInfo.Location = new System.Drawing.Point(6, 13);
			this.txtComputersNetworkInfo.Name = "txtComputersNetworkInfo";
			this.txtComputersNetworkInfo.Size = new System.Drawing.Size(1047, 20);
			this.txtComputersNetworkInfo.TabIndex = 0;
			this.txtComputersNetworkInfo.TextChanged += new System.EventHandler(this.txtComputersNetworkInfo_TextChanged);
			this.txtComputersNetworkInfo.Enter += new System.EventHandler(this.txtComputersNetworkInfo_Enter);
			this.txtComputersNetworkInfo.Leave += new System.EventHandler(this.txtComputersNetworkInfo_Leave);
			// 
			// btnQueryNetworkInfo
			// 
			this.btnQueryNetworkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQueryNetworkInfo.Location = new System.Drawing.Point(1059, 13);
			this.btnQueryNetworkInfo.Name = "btnQueryNetworkInfo";
			this.btnQueryNetworkInfo.Size = new System.Drawing.Size(75, 23);
			this.btnQueryNetworkInfo.TabIndex = 2;
			this.btnQueryNetworkInfo.Text = "&Query";
			this.btnQueryNetworkInfo.UseVisualStyleBackColor = true;
			this.btnQueryNetworkInfo.Click += new System.EventHandler(this.btnQueryNetworkInfo_Click);
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
			this.gbComputersSoftware.ResumeLayout(false);
			this.gbComputersSoftware.PerformLayout();
			this.tpComputerInfo.ResumeLayout(false);
			this.tpComputerInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvComputerInfo)).EndInit();
			this.gbComputersComputerInfo.ResumeLayout(false);
			this.gbComputersComputerInfo.PerformLayout();
			this.tpCurrentUsers.ResumeLayout(false);
			this.tpCurrentUsers.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvCurrentUsers)).EndInit();
			this.gbComputersCurrentUsers.ResumeLayout(false);
			this.gbComputersCurrentUsers.PerformLayout();
			this.tpNetworkInfo.ResumeLayout(false);
			this.tpNetworkInfo.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvNetworkInfo)).EndInit();
			this.gbComputersNetworkInfo.ResumeLayout(false);
			this.gbComputersNetworkInfo.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tcMain;
		private System.Windows.Forms.TabPage tpSoftware;
		private System.Windows.Forms.GroupBox gbComputersSoftware;
		private System.Windows.Forms.TextBox txtComputersNameSoftware;
		private System.Windows.Forms.Button btnQuerySoftware;
		private System.Windows.Forms.DataGridView dgvSoftware;
		private System.Windows.Forms.TextBox txtFilterSoftware;
		private System.Windows.Forms.Label lblFilterSoftware;
		private System.Windows.Forms.TabPage tpComputerInfo;
		private System.Windows.Forms.DataGridView dgvComputerInfo;
		private System.Windows.Forms.GroupBox gbComputersComputerInfo;
		private System.Windows.Forms.TextBox txtComputersComputerInfo;
		private System.Windows.Forms.Button btnQueryComputerInfo;
		private System.Windows.Forms.TextBox txtFilterComputerInfo;
		private System.Windows.Forms.Label lblFilterComputerInfo;
		private System.Windows.Forms.TabPage tpCurrentUsers;
		private System.Windows.Forms.TextBox txtFilterCurrentUsers;
		private System.Windows.Forms.Label lblFilterCurrentUsers;
		private System.Windows.Forms.DataGridView dgvCurrentUsers;
		private System.Windows.Forms.GroupBox gbComputersCurrentUsers;
		private System.Windows.Forms.TextBox txtComputersCurrentUsers;
		private System.Windows.Forms.Button btnQueryCurrentUsers;
		private System.Windows.Forms.TabPage tpNetworkInfo;
		private System.Windows.Forms.TextBox txtFilterNetworkInfo;
		private System.Windows.Forms.Label lblFilterNetworkInfo;
		private System.Windows.Forms.DataGridView dgvNetworkInfo;
		private System.Windows.Forms.GroupBox gbComputersNetworkInfo;
		private System.Windows.Forms.TextBox txtComputersNetworkInfo;
		private System.Windows.Forms.Button btnQueryNetworkInfo;
	}
}

