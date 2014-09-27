﻿namespace ElectronicObserver.Window {
	partial class FormMain {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.MainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.StripMenu = new System.Windows.Forms.MenuStrip();
			this.StripMenu_View = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_1 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_2 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_3 = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Fleet_4 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.StripMenu_View_Dock = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_View_Arsenal = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.StripMenu_View_Headquarters = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug = new System.Windows.Forms.ToolStripMenuItem();
			this.StripMenu_Debug_LoadAPIFromFile = new System.Windows.Forms.ToolStripMenuItem();
			this.StripStatus = new System.Windows.Forms.StatusStrip();
			this.StripStatus_Information = new System.Windows.Forms.ToolStripStatusLabel();
			this.StripStatus_Padding = new System.Windows.Forms.ToolStripStatusLabel();
			this.StripStatus_Clock = new System.Windows.Forms.ToolStripStatusLabel();
			this.UIUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.StripMenu.SuspendLayout();
			this.StripStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainDockPanel
			// 
			this.MainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainDockPanel.DockBottomPortion = 150D;
			this.MainDockPanel.DockLeftPortion = 150D;
			this.MainDockPanel.DockRightPortion = 150D;
			this.MainDockPanel.DockTopPortion = 150D;
			this.MainDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.MainDockPanel.Location = new System.Drawing.Point(0, 26);
			this.MainDockPanel.Name = "MainDockPanel";
			this.MainDockPanel.ShowDocumentIcon = true;
			this.MainDockPanel.Size = new System.Drawing.Size(284, 213);
			dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
			tabGradient8.EndColor = System.Drawing.SystemColors.Control;
			tabGradient8.StartColor = System.Drawing.SystemColors.Control;
			tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin2.TabGradient = tabGradient8;
			autoHideStripSkin2.TextFont = new System.Drawing.Font("メイリオ", 9F);
			dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
			tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
			dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
			tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
			dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
			dockPaneStripSkin2.TextFont = new System.Drawing.Font("メイリオ", 9F);
			tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
			tabGradient12.EndColor = System.Drawing.SystemColors.Control;
			tabGradient12.StartColor = System.Drawing.SystemColors.Control;
			tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
			dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
			tabGradient13.EndColor = System.Drawing.SystemColors.InactiveCaption;
			tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient13.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
			dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
			tabGradient14.EndColor = System.Drawing.Color.Transparent;
			tabGradient14.StartColor = System.Drawing.Color.Transparent;
			tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
			dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
			dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
			this.MainDockPanel.Skin = dockPanelSkin2;
			this.MainDockPanel.TabIndex = 0;
			// 
			// StripMenu
			// 
			this.StripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View,
            this.StripMenu_Debug});
			this.StripMenu.Location = new System.Drawing.Point(0, 0);
			this.StripMenu.Name = "StripMenu";
			this.StripMenu.Size = new System.Drawing.Size(284, 26);
			this.StripMenu.TabIndex = 2;
			this.StripMenu.Text = "menuStrip1";
			// 
			// StripMenu_View
			// 
			this.StripMenu_View.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View_Fleet,
            this.toolStripSeparator1,
            this.StripMenu_View_Dock,
            this.StripMenu_View_Arsenal,
            this.toolStripSeparator2,
            this.StripMenu_View_Headquarters});
			this.StripMenu_View.Name = "StripMenu_View";
			this.StripMenu_View.Size = new System.Drawing.Size(62, 22);
			this.StripMenu_View.Text = "表示(&V)";
			// 
			// StripMenu_View_Fleet
			// 
			this.StripMenu_View_Fleet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_View_Fleet_1,
            this.StripMenu_View_Fleet_2,
            this.StripMenu_View_Fleet_3,
            this.StripMenu_View_Fleet_4});
			this.StripMenu_View_Fleet.Name = "StripMenu_View_Fleet";
			this.StripMenu_View_Fleet.Size = new System.Drawing.Size(131, 22);
			this.StripMenu_View_Fleet.Text = "艦隊(&F)";
			// 
			// StripMenu_View_Fleet_1
			// 
			this.StripMenu_View_Fleet_1.Name = "StripMenu_View_Fleet_1";
			this.StripMenu_View_Fleet_1.Size = new System.Drawing.Size(93, 22);
			this.StripMenu_View_Fleet_1.Text = "#&1";
			this.StripMenu_View_Fleet_1.Click += new System.EventHandler(this.StripMenu_View_Fleet_1_Click);
			// 
			// StripMenu_View_Fleet_2
			// 
			this.StripMenu_View_Fleet_2.Name = "StripMenu_View_Fleet_2";
			this.StripMenu_View_Fleet_2.Size = new System.Drawing.Size(93, 22);
			this.StripMenu_View_Fleet_2.Text = "#&2";
			this.StripMenu_View_Fleet_2.Click += new System.EventHandler(this.StripMenu_View_Fleet_2_Click);
			// 
			// StripMenu_View_Fleet_3
			// 
			this.StripMenu_View_Fleet_3.Name = "StripMenu_View_Fleet_3";
			this.StripMenu_View_Fleet_3.Size = new System.Drawing.Size(93, 22);
			this.StripMenu_View_Fleet_3.Text = "#&3";
			this.StripMenu_View_Fleet_3.Click += new System.EventHandler(this.StripMenu_View_Fleet_3_Click);
			// 
			// StripMenu_View_Fleet_4
			// 
			this.StripMenu_View_Fleet_4.Name = "StripMenu_View_Fleet_4";
			this.StripMenu_View_Fleet_4.Size = new System.Drawing.Size(93, 22);
			this.StripMenu_View_Fleet_4.Text = "#&4";
			this.StripMenu_View_Fleet_4.Click += new System.EventHandler(this.StripMenu_View_Fleet_4_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(128, 6);
			// 
			// StripMenu_View_Dock
			// 
			this.StripMenu_View_Dock.Name = "StripMenu_View_Dock";
			this.StripMenu_View_Dock.Size = new System.Drawing.Size(131, 22);
			this.StripMenu_View_Dock.Text = "入渠(&D)";
			this.StripMenu_View_Dock.Click += new System.EventHandler(this.StripMenu_View_Dock_Click);
			// 
			// StripMenu_View_Arsenal
			// 
			this.StripMenu_View_Arsenal.Name = "StripMenu_View_Arsenal";
			this.StripMenu_View_Arsenal.Size = new System.Drawing.Size(131, 22);
			this.StripMenu_View_Arsenal.Text = "工廠(&A)";
			this.StripMenu_View_Arsenal.Click += new System.EventHandler(this.StripMenu_View_Arsenal_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(128, 6);
			// 
			// StripMenu_View_Headquarters
			// 
			this.StripMenu_View_Headquarters.Name = "StripMenu_View_Headquarters";
			this.StripMenu_View_Headquarters.Size = new System.Drawing.Size(131, 22);
			this.StripMenu_View_Headquarters.Text = "司令部(&H)";
			this.StripMenu_View_Headquarters.Click += new System.EventHandler(this.StripMenu_View_Headquarters_Click);
			// 
			// StripMenu_Debug
			// 
			this.StripMenu_Debug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripMenu_Debug_LoadAPIFromFile});
			this.StripMenu_Debug.Name = "StripMenu_Debug";
			this.StripMenu_Debug.Size = new System.Drawing.Size(87, 22);
			this.StripMenu_Debug.Text = "デバッグ(&D)";
			// 
			// StripMenu_Debug_LoadAPIFromFile
			// 
			this.StripMenu_Debug_LoadAPIFromFile.Name = "StripMenu_Debug_LoadAPIFromFile";
			this.StripMenu_Debug_LoadAPIFromFile.Size = new System.Drawing.Size(245, 22);
			this.StripMenu_Debug_LoadAPIFromFile.Text = "ファイルからAPIをロード(&L)...";
			this.StripMenu_Debug_LoadAPIFromFile.Click += new System.EventHandler(this.StripMenu_Debug_LoadAPIFromFile_Click);
			// 
			// StripStatus
			// 
			this.StripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StripStatus_Information,
            this.StripStatus_Padding,
            this.StripStatus_Clock});
			this.StripStatus.Location = new System.Drawing.Point(0, 239);
			this.StripStatus.Name = "StripStatus";
			this.StripStatus.Size = new System.Drawing.Size(284, 23);
			this.StripStatus.TabIndex = 3;
			// 
			// StripStatus_Information
			// 
			this.StripStatus_Information.Name = "StripStatus_Information";
			this.StripStatus_Information.Size = new System.Drawing.Size(105, 18);
			this.StripStatus_Information.Text = "Now Preparing...";
			// 
			// StripStatus_Padding
			// 
			this.StripStatus_Padding.Name = "StripStatus_Padding";
			this.StripStatus_Padding.Size = new System.Drawing.Size(125, 18);
			this.StripStatus_Padding.Spring = true;
			// 
			// StripStatus_Clock
			// 
			this.StripStatus_Clock.Name = "StripStatus_Clock";
			this.StripStatus_Clock.Size = new System.Drawing.Size(39, 18);
			this.StripStatus_Clock.Text = "Clock";
			// 
			// UIUpdateTimer
			// 
			this.UIUpdateTimer.Interval = 1000;
			this.UIUpdateTimer.Tick += new System.EventHandler(this.UIUpdateTimer_Tick);
			// 
			// FormMain
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.MainDockPanel);
			this.Controls.Add(this.StripStatus);
			this.Controls.Add(this.StripMenu);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MainMenuStrip = this.StripMenu;
			this.Name = "FormMain";
			this.Text = "試製七四式電子観測儀";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.StripMenu.ResumeLayout(false);
			this.StripMenu.PerformLayout();
			this.StripStatus.ResumeLayout(false);
			this.StripStatus.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WeifenLuo.WinFormsUI.Docking.DockPanel MainDockPanel;
		private System.Windows.Forms.MenuStrip StripMenu;
		private System.Windows.Forms.StatusStrip StripStatus;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_Debug_LoadAPIFromFile;
		private System.Windows.Forms.Timer UIUpdateTimer;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Information;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Padding;
		private System.Windows.Forms.ToolStripStatusLabel StripStatus_Clock;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_1;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_2;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_3;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Fleet_4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Dock;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Arsenal;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem StripMenu_View_Headquarters;
	}
}

