namespace ElectronicObserver.Window.Dialog
{
	partial class DialogConstructionRecordViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogConstructionRecordViewer));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label8 = new System.Windows.Forms.Label();
			this.DevelopmentMaterial = new System.Windows.Forms.ComboBox();
			this.IsLargeConstruction = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.EmptyDock = new System.Windows.Forms.ComboBox();
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
			this.ShipCategory = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ShipName = new System.Windows.Forms.ComboBox();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_SecretaryShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material100 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Material1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			resources.ApplyResources(this.statusStrip1, "statusStrip1");
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
			this.statusStrip1.Name = "statusStrip1";
			// 
			// StatusInfo
			// 
			resources.ApplyResources(this.StatusInfo, "StatusInfo");
			this.StatusInfo.Name = "StatusInfo";
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
			this.splitContainer1.Panel1.Controls.Add(this.label8);
			this.splitContainer1.Panel1.Controls.Add(this.DevelopmentMaterial);
			this.splitContainer1.Panel1.Controls.Add(this.IsLargeConstruction);
			this.splitContainer1.Panel1.Controls.Add(this.label7);
			this.splitContainer1.Panel1.Controls.Add(this.EmptyDock);
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
			this.splitContainer1.Panel1.Controls.Add(this.ShipCategory);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.ShipName);
			// 
			// splitContainer1.Panel2
			// 
			resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// DevelopmentMaterial
			// 
			resources.ApplyResources(this.DevelopmentMaterial, "DevelopmentMaterial");
			this.DevelopmentMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.DevelopmentMaterial.FormattingEnabled = true;
			this.DevelopmentMaterial.Name = "DevelopmentMaterial";
			// 
			// IsLargeConstruction
			// 
			resources.ApplyResources(this.IsLargeConstruction, "IsLargeConstruction");
			this.IsLargeConstruction.Checked = true;
			this.IsLargeConstruction.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.IsLargeConstruction.Name = "IsLargeConstruction";
			this.IsLargeConstruction.ThreeState = true;
			this.IsLargeConstruction.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// EmptyDock
			// 
			resources.ApplyResources(this.EmptyDock, "EmptyDock");
			this.EmptyDock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EmptyDock.FormattingEnabled = true;
			this.EmptyDock.Name = "EmptyDock";
			// 
			// ButtonRun
			// 
			resources.ApplyResources(this.ButtonRun, "ButtonRun");
			this.ButtonRun.Name = "ButtonRun";
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			// 
			// MergeRows
			// 
			resources.ApplyResources(this.MergeRows, "MergeRows");
			this.MergeRows.Name = "MergeRows";
			this.MergeRows.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// Recipe
			// 
			resources.ApplyResources(this.Recipe, "Recipe");
			this.Recipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.Recipe.FormattingEnabled = true;
			this.Recipe.Name = "Recipe";
			// 
			// SecretaryName
			// 
			resources.ApplyResources(this.SecretaryName, "SecretaryName");
			this.SecretaryName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryName.FormattingEnabled = true;
			this.SecretaryName.Name = "SecretaryName";
			this.SecretaryName.SelectedIndexChanged += new System.EventHandler(this.SecretaryName_SelectedIndexChanged);
			// 
			// SecretaryCategory
			// 
			resources.ApplyResources(this.SecretaryCategory, "SecretaryCategory");
			this.SecretaryCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SecretaryCategory.FormattingEnabled = true;
			this.SecretaryCategory.Name = "SecretaryCategory";
			this.SecretaryCategory.SelectedIndexChanged += new System.EventHandler(this.SecretaryCategory_SelectedIndexChanged);
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// DateEnd
			// 
			resources.ApplyResources(this.DateEnd, "DateEnd");
			this.DateEnd.Name = "DateEnd";
			// 
			// DateBegin
			// 
			resources.ApplyResources(this.DateBegin, "DateBegin");
			this.DateBegin.Name = "DateBegin";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// ShipCategory
			// 
			resources.ApplyResources(this.ShipCategory, "ShipCategory");
			this.ShipCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ShipCategory.FormattingEnabled = true;
			this.ShipCategory.Name = "ShipCategory";
			this.ShipCategory.SelectedIndexChanged += new System.EventHandler(this.ShipCategory_SelectedIndexChanged);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// ShipName
			// 
			resources.ApplyResources(this.ShipName, "ShipName");
			this.ShipName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.ShipName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ShipName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ShipName.FormattingEnabled = true;
			this.ShipName.Name = "ShipName";
			this.ShipName.SelectedIndexChanged += new System.EventHandler(this.ShipName_SelectedIndexChanged);
			// 
			// RecordView
			// 
			resources.ApplyResources(this.RecordView, "RecordView");
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Recipe,
            this.RecordView_SecretaryShip,
            this.RecordView_Material100,
            this.RecordView_Material20,
            this.RecordView_Material1});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle2;
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
			// RecordView_SecretaryShip
			// 
			resources.ApplyResources(this.RecordView_SecretaryShip, "RecordView_SecretaryShip");
			this.RecordView_SecretaryShip.Name = "RecordView_SecretaryShip";
			this.RecordView_SecretaryShip.ReadOnly = true;
			// 
			// RecordView_Material100
			// 
			resources.ApplyResources(this.RecordView_Material100, "RecordView_Material100");
			this.RecordView_Material100.Name = "RecordView_Material100";
			this.RecordView_Material100.ReadOnly = true;
			// 
			// RecordView_Material20
			// 
			resources.ApplyResources(this.RecordView_Material20, "RecordView_Material20");
			this.RecordView_Material20.Name = "RecordView_Material20";
			this.RecordView_Material20.ReadOnly = true;
			// 
			// RecordView_Material1
			// 
			resources.ApplyResources(this.RecordView_Material1, "RecordView_Material1");
			this.RecordView_Material1.Name = "RecordView_Material1";
			this.RecordView_Material1.ReadOnly = true;
			// 
			// Searcher
			// 
			this.Searcher.WorkerSupportsCancellation = true;
			this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
			this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
			// 
			// DialogConstructionRecordViewer
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Name = "DialogConstructionRecordViewer";
			this.Load += new System.EventHandler(this.DialogConstructionRecordViewer_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.CheckBox IsLargeConstruction;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox EmptyDock;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox Recipe;
		private System.Windows.Forms.ComboBox SecretaryName;
		private System.Windows.Forms.ComboBox SecretaryCategory;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox ShipCategory;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox ShipName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox DevelopmentMaterial;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Recipe;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_SecretaryShip;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material100;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material20;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Material1;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
	}
}
