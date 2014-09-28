namespace RemoteWindowsAdministrator {
	partial class DataPageControl<T> {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( ) {
			this.txtFilter = new System.Windows.Forms.TextBox();
			this.lblFilter = new System.Windows.Forms.Label();
			this.dgv = new System.Windows.Forms.DataGridView();
			this.gbComputers = new System.Windows.Forms.GroupBox();
			this.txtComputers = new System.Windows.Forms.TextBox();
			this.btnQuery = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
			this.gbComputers.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtFilter.Location = new System.Drawing.Point(40, 420);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(243, 20);
			this.txtFilter.TabIndex = 20;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// lblFilter
			// 
			this.lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFilter.AutoSize = true;
			this.lblFilter.Location = new System.Drawing.Point(5, 422);
			this.lblFilter.Name = "lblFilter";
			this.lblFilter.Size = new System.Drawing.Size(29, 13);
			this.lblFilter.TabIndex = 19;
			this.lblFilter.Text = "Filter";
			// 
			// dgv
			// 
			this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv.Location = new System.Drawing.Point(3, 56);
			this.dgv.Name = "dgv";
			this.dgv.Size = new System.Drawing.Size(732, 358);
			this.dgv.TabIndex = 18;
			this.dgv.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseClick);
			// 
			// gbComputers
			// 
			this.gbComputers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbComputers.Controls.Add(this.txtComputers);
			this.gbComputers.Controls.Add(this.btnQuery);
			this.gbComputers.Location = new System.Drawing.Point(3, 3);
			this.gbComputers.Name = "gbComputers";
			this.gbComputers.Size = new System.Drawing.Size(732, 47);
			this.gbComputers.TabIndex = 17;
			this.gbComputers.TabStop = false;
			this.gbComputers.Text = "Computer(s)";
			// 
			// txtComputers
			// 
			this.txtComputers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtComputers.Location = new System.Drawing.Point(6, 13);
			this.txtComputers.Name = "txtComputers";
			this.txtComputers.Size = new System.Drawing.Size(639, 20);
			this.txtComputers.TabIndex = 0;
			this.txtComputers.TextChanged += new System.EventHandler(this.txtComputers_TextChanged);
			this.txtComputers.Enter += new System.EventHandler(this.txtComputers_Enter);
			this.txtComputers.Leave += new System.EventHandler(this.txtComputers_Leave);
			// 
			// btnQuery
			// 
			this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQuery.Location = new System.Drawing.Point(651, 13);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(75, 23);
			this.btnQuery.TabIndex = 2;
			this.btnQuery.Text = "&Query";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// DataPageControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.lblFilter);
			this.Controls.Add(this.dgv);
			this.Controls.Add(this.gbComputers);
			this.MinimumSize = new System.Drawing.Size(300, 200);
			this.Name = "DataPageControl";
			this.Size = new System.Drawing.Size(738, 443);
			((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
			this.gbComputers.ResumeLayout(false);
			this.gbComputers.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtFilter;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.DataGridView dgv;
		private System.Windows.Forms.GroupBox gbComputers;
		private System.Windows.Forms.TextBox txtComputers;
		private System.Windows.Forms.Button btnQuery;
	}
}
