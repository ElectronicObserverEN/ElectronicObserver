namespace ElectronicObserver.Window.Dialog
{
	partial class DialogDropRecordViewer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogDropRecordViewer));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.ItemName = new System.Windows.Forms.ComboBox();
			this.ShipName = new System.Windows.Forms.ComboBox();
			this.EquipmentName = new System.Windows.Forms.ComboBox();
			this.DateBegin = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.DateEnd = new System.Windows.Forms.DateTimePicker();
			this.RankS = new System.Windows.Forms.CheckBox();
			this.RankA = new System.Windows.Forms.CheckBox();
			this.RankB = new System.Windows.Forms.CheckBox();
			this.RankX = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.MapDifficulty = new System.Windows.Forms.ComboBox();
			this.ButtonRun = new System.Windows.Forms.Button();
			this.RecordView = new System.Windows.Forms.DataGridView();
			this.RecordView_Header = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Map = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankA = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RecordView_RankB = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.IsBossOnly = new System.Windows.Forms.CheckBox();
			this.MapAreaID = new System.Windows.Forms.ComboBox();
			this.MapInfoID = new System.Windows.Forms.ComboBox();
			this.MapCellID = new System.Windows.Forms.ComboBox();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.MergeRows = new System.Windows.Forms.CheckBox();
			this.LabelShipName = new ElectronicObserver.Window.Control.ImageLabel();
			this.LabelItemName = new ElectronicObserver.Window.Control.ImageLabel();
			this.LabelEquipmentName = new ElectronicObserver.Window.Control.ImageLabel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusInfo = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
			this.Searcher = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			// ItemName
			//
			this.ItemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ItemName.FormattingEnabled = true;
			resources.ApplyResources(this.ItemName, "ItemName");
			this.ItemName.Name = "ItemName";
			this.ToolTipInfo.SetToolTip(this.ItemName, resources.GetString("ItemName.ToolTip"));
			//
			// ShipName
			//
			this.ShipName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.ShipName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ShipName.FormattingEnabled = true;
			resources.ApplyResources(this.ShipName, "ShipName");
			this.ShipName.Name = "ShipName";
			this.ToolTipInfo.SetToolTip(this.ShipName, resources.GetString("ShipName.ToolTip"));
			//
			// EquipmentName
			//
			this.EquipmentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.EquipmentName, "EquipmentName");
			this.EquipmentName.FormattingEnabled = true;
			this.EquipmentName.Name = "EquipmentName";
			this.ToolTipInfo.SetToolTip(this.EquipmentName, resources.GetString("EquipmentName.ToolTip"));
			//
			// DateBegin
			//
			resources.ApplyResources(this.DateBegin, "DateBegin");
			this.DateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.DateBegin.Name = "DateBegin";
			this.ToolTipInfo.SetToolTip(this.DateBegin, resources.GetString("DateBegin.ToolTip"));
			//
			// label2
			//
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			//
			// label3
			//
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			//
			// DateEnd
			//
			resources.ApplyResources(this.DateEnd, "DateEnd");
			this.DateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.DateEnd.Name = "DateEnd";
			this.ToolTipInfo.SetToolTip(this.DateEnd, resources.GetString("DateEnd.ToolTip"));
			//
			// RankS
			//
			resources.ApplyResources(this.RankS, "RankS");
			this.RankS.Checked = true;
			this.RankS.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankS.Name = "RankS";
			this.ToolTipInfo.SetToolTip(this.RankS, resources.GetString("RankS.ToolTip"));
			this.RankS.UseVisualStyleBackColor = true;
			//
			// RankA
			//
			resources.ApplyResources(this.RankA, "RankA");
			this.RankA.Checked = true;
			this.RankA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankA.Name = "RankA";
			this.ToolTipInfo.SetToolTip(this.RankA, resources.GetString("RankA.ToolTip"));
			this.RankA.UseVisualStyleBackColor = true;
			//
			// RankB
			//
			resources.ApplyResources(this.RankB, "RankB");
			this.RankB.Checked = true;
			this.RankB.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankB.Name = "RankB";
			this.ToolTipInfo.SetToolTip(this.RankB, resources.GetString("RankB.ToolTip"));
			this.RankB.UseVisualStyleBackColor = true;
			//
			// RankX
			//
			resources.ApplyResources(this.RankX, "RankX");
			this.RankX.Checked = true;
			this.RankX.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RankX.Name = "RankX";
			this.ToolTipInfo.SetToolTip(this.RankX, resources.GetString("RankX.ToolTip"));
			this.RankX.UseVisualStyleBackColor = true;
			//
			// label1
			//
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			//
			// MapDifficulty
			//
			this.MapDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapDifficulty.FormattingEnabled = true;
			resources.ApplyResources(this.MapDifficulty, "MapDifficulty");
			this.MapDifficulty.Name = "MapDifficulty";
			this.ToolTipInfo.SetToolTip(this.MapDifficulty, resources.GetString("MapDifficulty.ToolTip"));
			//
			// ButtonRun
			//
			resources.ApplyResources(this.ButtonRun, "ButtonRun");
			this.ButtonRun.Name = "ButtonRun";
			this.ButtonRun.UseVisualStyleBackColor = true;
			this.ButtonRun.Click += new System.EventHandler(this.ButtonRun_Click);
			//
			// RecordView
			//
			this.RecordView.AllowUserToAddRows = false;
			this.RecordView.AllowUserToDeleteRows = false;
			this.RecordView.AllowUserToResizeRows = false;
			this.RecordView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.RecordView.ColumnHeadersVisible = false;
			this.RecordView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RecordView_Header,
            this.RecordView_Name,
            this.RecordView_Date,
            this.RecordView_Map,
            this.RecordView_Rank,
            this.RecordView_RankS,
            this.RecordView_RankA,
            this.RecordView_RankB});
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(204)))));
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RecordView.DefaultCellStyle = dataGridViewCellStyle5;
			resources.ApplyResources(this.RecordView, "RecordView");
			this.RecordView.Name = "RecordView";
			this.RecordView.ReadOnly = true;
			this.RecordView.RowHeadersVisible = false;
			this.RecordView.RowTemplate.Height = 21;
			this.RecordView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.RecordView.Size = new System.Drawing.Size(624, 315);
			this.RecordView.TabIndex = 1;
			this.RecordView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.RecordView_CellDoubleClick);
			this.RecordView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.RecordView_CellFormatting);
			this.RecordView.SelectionChanged += new System.EventHandler(this.RecordView_SelectionChanged);
			this.RecordView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.RecordView_SortCompare);
			this.RecordView.Sorted += new System.EventHandler(this.RecordView_Sorted);
			//
			// RecordView_Header
			//
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_Header.DefaultCellStyle = dataGridViewCellStyle1;
			this.RecordView_Header.HeaderText = "";
			this.RecordView_Header.Name = "RecordView_Header";
			this.RecordView_Header.ReadOnly = true;
			this.RecordView_Header.Width = 50;
			//
			// RecordView_Name
			//
			this.RecordView_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.RecordView_Name.HeaderText = "Name";
			this.RecordView_Name.Name = "RecordView_Name";
			this.RecordView_Name.ReadOnly = true;
			//
			// RecordView_Date
			//
			this.RecordView_Date.HeaderText = "Date";
			this.RecordView_Date.Name = "RecordView_Date";
			this.RecordView_Date.ReadOnly = true;
			this.RecordView_Date.Width = 150;
			//
			// RecordView_Map
			//
			this.RecordView_Map.HeaderText = "Map";
			this.RecordView_Map.Name = "RecordView_Map";
			this.RecordView_Map.ReadOnly = true;
			this.RecordView_Map.Width = 120;
			//
			// RecordView_Rank
			//
			this.RecordView_Rank.HeaderText = "Rank";
			this.RecordView_Rank.Name = "RecordView_Rank";
			this.RecordView_Rank.ReadOnly = true;
			this.RecordView_Rank.Width = 40;
			//
			// RecordView_RankS
			//
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankS.DefaultCellStyle = dataGridViewCellStyle2;
			this.RecordView_RankS.HeaderText = "S-rank";
			this.RecordView_RankS.Name = "RecordView_RankS";
			this.RecordView_RankS.ReadOnly = true;
			//
			// RecordView_RankA
			//
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankA.DefaultCellStyle = dataGridViewCellStyle3;
			this.RecordView_RankA.HeaderText = "A-rank";
			this.RecordView_RankA.Name = "RecordView_RankA";
			this.RecordView_RankA.ReadOnly = true;
			//
			// RecordView_RankB
			//
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.RecordView_RankB.DefaultCellStyle = dataGridViewCellStyle4;
			this.RecordView_RankB.HeaderText = "B-rank";
			this.RecordView_RankB.Name = "RecordView_RankB";
			this.RecordView_RankB.ReadOnly = true;
			//
			// IsBossOnly
			//
			resources.ApplyResources(this.IsBossOnly, "IsBossOnly");
			this.IsBossOnly.Checked = true;
			this.IsBossOnly.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.IsBossOnly.Name = "IsBossOnly";
			this.IsBossOnly.ThreeState = true;
			this.IsBossOnly.UseVisualStyleBackColor = true;
			//
			// MapAreaID
			//
			this.MapAreaID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapAreaID.FormattingEnabled = true;
			resources.ApplyResources(this.MapAreaID, "MapAreaID");
			this.MapAreaID.Name = "MapAreaID";
			this.ToolTipInfo.SetToolTip(this.MapAreaID, resources.GetString("MapAreaID.ToolTip"));
			this.MapAreaID.SelectedIndexChanged += new System.EventHandler(this.MapAreaID_SelectedIndexChanged);
			//
			// MapInfoID
			//
			this.MapInfoID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapInfoID.FormattingEnabled = true;
			resources.ApplyResources(this.MapInfoID, "MapInfoID");
			this.MapInfoID.Name = "MapInfoID";
			this.ToolTipInfo.SetToolTip(this.MapInfoID, resources.GetString("MapInfoID.ToolTip"));
			this.MapInfoID.SelectedIndexChanged += new System.EventHandler(this.MapAreaID_SelectedIndexChanged);
			//
			// MapCellID
			//
			this.MapCellID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MapCellID.FormattingEnabled = true;
			resources.ApplyResources(this.MapCellID, "MapCellID");
			this.MapCellID.Name = "MapCellID";
			this.ToolTipInfo.SetToolTip(this.MapCellID, resources.GetString("MapCellID.ToolTip"));
			//
			// splitContainer1
			//
			resources.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Name = "splitContainer1";
			//
			// splitContainer1.Panel1
			//
			this.splitContainer1.Panel1.Controls.Add(this.MergeRows);
			this.splitContainer1.Panel1.Controls.Add(this.MapCellID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelShipName);
			this.splitContainer1.Panel1.Controls.Add(this.MapInfoID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelItemName);
			this.splitContainer1.Panel1.Controls.Add(this.MapAreaID);
			this.splitContainer1.Panel1.Controls.Add(this.LabelEquipmentName);
			this.splitContainer1.Panel1.Controls.Add(this.IsBossOnly);
			this.splitContainer1.Panel1.Controls.Add(this.ItemName);
			this.splitContainer1.Panel1.Controls.Add(this.ButtonRun);
			this.splitContainer1.Panel1.Controls.Add(this.ShipName);
			this.splitContainer1.Panel1.Controls.Add(this.MapDifficulty);
			this.splitContainer1.Panel1.Controls.Add(this.EquipmentName);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1.Controls.Add(this.DateBegin);
			this.splitContainer1.Panel1.Controls.Add(this.RankX);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.RankB);
			this.splitContainer1.Panel1.Controls.Add(this.DateEnd);
			this.splitContainer1.Panel1.Controls.Add(this.RankA);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.RankS);
			//
			// splitContainer1.Panel2
			//
			this.splitContainer1.Panel2.Controls.Add(this.RecordView);
			//
			// MergeRows
			//
			resources.ApplyResources(this.MergeRows, "MergeRows");
			this.MergeRows.Name = "MergeRows";
			this.ToolTipInfo.SetToolTip(this.MergeRows, resources.GetString("MergeRows.ToolTip"));
			this.MergeRows.UseVisualStyleBackColor = true;
			//
			// LabelShipName
			//
			resources.ApplyResources(this.LabelShipName, "LabelShipName");
			this.LabelShipName.BackColor = System.Drawing.Color.Transparent;
			this.LabelShipName.Name = "LabelShipName";
			//
			// LabelItemName
			//
			resources.ApplyResources(this.LabelItemName, "LabelItemName");
			this.LabelItemName.BackColor = System.Drawing.Color.Transparent;
			this.LabelItemName.Name = "LabelItemName";
			//
			// LabelEquipmentName
			//
			resources.ApplyResources(this.LabelEquipmentName, "LabelEquipmentName");
			this.LabelEquipmentName.BackColor = System.Drawing.Color.Transparent;
			this.LabelEquipmentName.Name = "LabelEquipmentName";
			//
			// statusStrip1
			//
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusInfo});
			this.statusStrip1.Location = new System.Drawing.Point(0, 419);
			this.statusStrip1.Name = "statusStrip1";
			//
			// StatusInfo
			//
			this.StatusInfo.Name = "StatusInfo";
			resources.ApplyResources(this.StatusInfo, "StatusInfo");
			//
			// ToolTipInfo
			//
			this.ToolTipInfo.AutoPopDelay = 30000;
			this.ToolTipInfo.InitialDelay = 500;
			this.ToolTipInfo.ReshowDelay = 100;
			this.ToolTipInfo.ShowAlways = true;
			//
			// Searcher
			//
			this.Searcher.WorkerSupportsCancellation = true;
			this.Searcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Searcher_DoWork);
			this.Searcher.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Searcher_RunWorkerCompleted);
			//
			// DialogDropRecordViewer
			//
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.statusStrip1);
			this.Name = "DialogDropRecordViewer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogDropRecordViewer_FormClosed);
			this.Load += new System.EventHandler(this.DialogDropRecordViewer_Load);
			((System.ComponentModel.ISupportInitialize)(this.RecordView)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox IsBossOnly;
		private System.Windows.Forms.Button ButtonRun;
		private System.Windows.Forms.ComboBox MapDifficulty;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox RankX;
		private System.Windows.Forms.CheckBox RankB;
		private System.Windows.Forms.CheckBox RankA;
		private System.Windows.Forms.CheckBox RankS;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DateTimePicker DateEnd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker DateBegin;
		private System.Windows.Forms.ComboBox EquipmentName;
		private System.Windows.Forms.ComboBox ShipName;
		private System.Windows.Forms.ComboBox ItemName;
		private Control.ImageLabel LabelEquipmentName;
		private Control.ImageLabel LabelItemName;
		private Control.ImageLabel LabelShipName;
		private System.Windows.Forms.DataGridView RecordView;
		private System.Windows.Forms.ComboBox MapCellID;
		private System.Windows.Forms.ComboBox MapInfoID;
		private System.Windows.Forms.ComboBox MapAreaID;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.CheckBox MergeRows;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ToolStripStatusLabel StatusInfo;
		private System.ComponentModel.BackgroundWorker Searcher;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Header;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Map;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_Rank;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankS;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankA;
		private System.Windows.Forms.DataGridViewTextBoxColumn RecordView_RankB;
	}
}
