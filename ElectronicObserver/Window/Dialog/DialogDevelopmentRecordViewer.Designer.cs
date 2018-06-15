namespace ElectronicObserver.Window.Dialog
{
	partial class DialogDevelopmentRecordViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogDevelopmentRecordViewer));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.ButtonRun = new System.Windows.Forms.Button();
			this.MergeRows = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.Recipe = new System.Windows.Forms.ComboBox();
			this.SecretaryName = new System.Windows.Forms.ComboBox();
			this.SecretaryCategory = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.DateEnd = new System.Windows.Forms.DateTimePicker();
			this.DateBegin = new System.Windows.Forms.DateTimePicker();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.EquipmentCategory = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.EquipmentName = new System.Windows.Forms.ComboBox();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_FlagshipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Flagship = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
			this.splitContainer1.Panel1.Controls.Add(this.ButtonRun);
			this.splitContainer1.Panel1.Controls.Add(this.MergeRows);
			this.splitContainer1.Panel1.Controls.Add(this.label6);
			this.splitContainer1.Panel1.Controls.Add(this.Recipe);
			this.splitContainer1.Panel1.Controls.Add(this.SecretaryName);
			this.splitContainer1.Panel1.Controls.Add(this.SecretaryCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label5);
			this.splitContainer1.Panel1.Controls.Add(this.DateEnd);
			this.splitContainer1.Panel1.Controls.Add(this.DateBegin);
			this.splitContainer1.Panel1.Controls.Add(this.label4);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentName);
			this.ToolTipInfo.SetToolTip(this.splitContainer1.Panel1, resources.GetString("splitContainer1.Panel1.ToolTip"));
			// 
			// splitContainer1.Panel2
			// 
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			this.ToolTipInfo.SetToolTip(this.splitContainer1.Panel2, resources.GetString("splitContainer1.Panel2.ToolTip"));
			this.ToolTipInfo.SetToolTip(this.splitContainer1, resources.GetString("splitContainer1.ToolTip"));
			// 
			// ButtonRun
			// 
			resources.ApplyResources(this.ButtonRun, "ButtonRun");
			this.ButtonRun.Name = "ButtonRun";
			this.ToolTipInfo.SetToolTip(this.ButtonRun, resources.GetString("ButtonRun.ToolTip"));
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			// 
			// MergeRows
			// 
			resources.ApplyResources(this.MergeRows, "MergeRows");
			this.MergeRows.Name = "MergeRows";
			this.ToolTipInfo.SetToolTip(this.MergeRows, resources.GetString("MergeRows.ToolTip"));
			this.MergeRows.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.ToolTipInfo.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
			// 
			// Recipe
			// 
			resources.ApplyResources(this.Recipe, "Recipe");
			this.Recipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Recipe.FormattingEnabled = true;
			this.Recipe.Name = "Recipe";
			this.ToolTipInfo.SetToolTip(this.Recipe, resources.GetString("Recipe.ToolTip"));
			// 
			// SecretaryName
			// 
			resources.ApplyResources(this.SecretaryName, "SecretaryName");
			this.SecretaryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryName.FormattingEnabled = true;
			this.SecretaryName.Name = "SecretaryName";
			this.ToolTipInfo.SetToolTip(this.SecretaryName, resources.GetString("SecretaryName.ToolTip"));
			this.SecretaryName.SelectedIndexChanged += new System.EventHandler(this.SecretaryName_SelectedIndexChanged);
			// 
			// SecretaryCategory
			// 
			resources.ApplyResources(this.SecretaryCategory, "SecretaryCategory");
			this.SecretaryCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryCategory.FormattingEnabled = true;
			this.SecretaryCategory.Name = "SecretaryCategory";
			this.ToolTipInfo.SetToolTip(this.SecretaryCategory, resources.GetString("SecretaryCategory.ToolTip"));
			this.SecretaryCategory.SelectedIndexChanged += new System.EventHandler(this.SecretaryCategory_SelectedIndexChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.ToolTipInfo.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
			// 
			// DateEnd
			// 
			resources.ApplyResources(this.DateEnd, "DateEnd");
			this.DateEnd.Name = "DateEnd";
			this.ToolTipInfo.SetToolTip(this.DateEnd, resources.GetString("DateEnd.ToolTip"));
			// 
			// DateBegin
			// 
			resources.ApplyResources(this.DateBegin, "DateBegin");
			this.DateBegin.Name = "DateBegin";
			this.ToolTipInfo.SetToolTip(this.DateBegin, resources.GetString("DateBegin.ToolTip"));
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.ToolTipInfo.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.ToolTipInfo.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.ToolTipInfo.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
			// 
			// EquipmentCategory
			// 
			resources.ApplyResources(this.EquipmentCategory, "EquipmentCategory");
			this.EquipmentCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EquipmentCategory.FormattingEnabled = true;
			this.EquipmentCategory.Name = "EquipmentCategory";
			this.ToolTipInfo.SetToolTip(this.EquipmentCategory, resources.GetString("EquipmentCategory.ToolTip"));
			this.EquipmentCategory.SelectedIndexChanged += new System.EventHandler(this.EquipmentCategory_SelectedIndexChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.ToolTipInfo.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
			// 
			// EquipmentName
			// 
			resources.ApplyResources(this.EquipmentName, "EquipmentName");
			this.EquipmentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EquipmentName.FormattingEnabled = true;
			this.EquipmentName.Name = "EquipmentName";
			this.ToolTipInfo.SetToolTip(this.EquipmentName, resources.GetString("EquipmentName.ToolTip"));
			this.EquipmentName.SelectedIndexChanged += new System.EventHandler(this.EquipmentName_SelectedIndexChanged);
			// 
			// RecordView
			// 
			resources.ApplyResources(this.RecordView, "RecordView");
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Recipe,
            this.RecordView_FlagshipType,
            this.RecordView_Flagship,
            this.RecordView_Detail});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle4;
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ToolTipInfo.SetToolTip(this.RecordView, resources.GetString("RecordView.ToolTip"));
			this.RecordView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RecordView_CellFormatting);
            this.RecordView.SelectionChanged += new System.EventHandler(this.RecordView_SelectionChanged);
            this.RecordView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.RecordView_SortCompare);
			this.RecordView.Sorted += new System.EventHandler(this.RecordView_Sorted);
			// 
			// RecordView_Header
			// 
			resources.ApplyResources(this.RecordView_Header, "RecordView_Header");
			this.RecordView_Header.Name = "RecordView_Header";
			this.RecordView_Header.ReadOnly = true;
			// 
			// RecordView_Name
			// 
			this.RecordView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.RecordView_Name, "RecordView_Name");
			this.RecordView_Name.Name = "RecordView_Name";
			this.RecordView_Name.ReadOnly = true;
			// 
			// RecordView_Date
			// 
			resources.ApplyResources(this.RecordView_Date, "RecordView_Date");
			this.RecordView_Date.Name = "RecordView_Date";
			this.RecordView_Date.ReadOnly = true;
			// 
			// RecordView_Recipe
			// 
			resources.ApplyResources(this.RecordView_Recipe, "RecordView_Recipe");
			this.RecordView_Recipe.Name = "RecordView_Recipe";
			this.RecordView_Recipe.ReadOnly = true;
			// 
			// RecordView_FlagshipType
			// 
			resources.ApplyResources(this.RecordView_FlagshipType, "RecordView_FlagshipType");
			this.RecordView_FlagshipType.Name = "RecordView_FlagshipType";
			this.RecordView_FlagshipType.ReadOnly = true;
			// 
			// RecordView_Flagship
			// 
			resources.ApplyResources(this.RecordView_Flagship, "RecordView_Flagship");
			this.RecordView_Flagship.Name = "RecordView_Flagship";
			this.RecordView_Flagship.ReadOnly = true;
			// 
			// RecordView_Detail
			// 
			this.RecordView_Detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			resources.ApplyResources(this.RecordView_Detail, "RecordView_Detail");
			this.RecordView_Detail.Name = "RecordView_Detail";
			this.RecordView_Detail.ReadOnly = true;
			// 
			// ToolTipInfo
			// 
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusInfo
            // 
            this.StatusInfo.Name = "StatusInfo";
            this.StatusInfo.Size = new System.Drawing.Size(12, 17);
            this.StatusInfo.Text = "-";
            // 
            // Searcher
            // 
            this.Searcher.WorkerSupportsCancellation = true;
            this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
            this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
            // 
            // DialogDevelopmentRecordViewer
            // 
            resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Name = "DialogDevelopmentRecordViewer";
			this.ToolTipInfo.SetToolTip(this, resources.GetString("$this.ToolTip"));
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogDevelopmentRecordViewer_FormClosed);
			this.Load += new System.EventHandler(this.DialogDevelopmentRecordViewer_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ComboBox EquipmentName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox EquipmentCategory;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox Recipe;
		private System.Windows.Forms.ComboBox SecretaryName;
		private System.Windows.Forms.ComboBox SecretaryCategory;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Recipe;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_FlagshipType;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Flagship;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Detail;
	}
}
