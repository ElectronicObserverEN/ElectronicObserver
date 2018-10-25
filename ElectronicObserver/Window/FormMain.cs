using Codeplex.Data;
using ElectronicObserver.Properties;
using ElectronicObserver.Data;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Utility.Mathematics;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	public partial class FormMain : Form
	{

		#region Properties

		public DockPanel MainPanel => MainDockPanel;
		public FormWindowCapture WindowCapture => fWindowCapture;

		private int ClockFormat;

		/// <summary>
		/// 音量設定用フラグ
		/// -1 = 無効, そうでなければ現在の試行回数
		/// </summary>
		private int _volumeUpdateState = 0;

		private DateTime _prevPlayTimeRecorded = DateTime.MinValue;

		#endregion

		//Singleton
		public static FormMain Instance;


		#region Forms

		public List<DockContent> SubForms { get; private set; }

		public FormFleet[] fFleet;
		public FormDock fDock;
		public FormArsenal fArsenal;
		public FormHeadquarters fHeadquarters;
		public FormInformation fInformation;
		public FormCompass fCompass;
		public FormLog fLog;
		public FormQuest fQuest;
		public FormBattle fBattle;
		public FormFleetOverview fFleetOverview;
		public FormShipGroup fShipGroup;
		public FormBrowserHost fBrowser;
		public FormWindowCapture fWindowCapture;
		public FormXPCalculator fXPCalculator;
		public FormBaseAirCorps fBaseAirCorps;
		public FormJson fJson;

		#endregion

		public DynamicTranslator Translator { get; private set; }




		public FormMain()
		{
			CultureInfo c = CultureInfo.CurrentCulture;
			CultureInfo ui = CultureInfo.CurrentUICulture;
			if (c.Name != "en-US" && c.Name != "ja-JP")
			{
				c = new CultureInfo("en-US");
			}
			if (ui.Name != "en-US" && ui.Name != "ja-JP")
			{
				ui = new CultureInfo("en-US");
			}
			Thread.CurrentThread.CurrentCulture = c;
			Thread.CurrentThread.CurrentUICulture = ui;
			Translator = new DynamicTranslator();
			Instance = this;
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			InitializeComponent();

			this.Text = SoftwareInformation.SoftwareNameEnglish;
		}

		private async void FormMain_Load(object sender, EventArgs e)
		{

			if (!Directory.Exists("Settings"))
				Directory.CreateDirectory("Settings");


			Utility.Configuration.Instance.Load(this);

			this.MainDockPanel.Styles = Configuration.Config.UI.DockPanelSuiteStyles;
			this.MainDockPanel.Theme = new WeifenLuo.WinFormsUI.Docking.VS2012Theme();
			this.BackColor = this.StripMenu.BackColor = Utility.Configuration.Config.UI.BackColor;
			this.ForeColor = this.StripMenu.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			this.StripStatus.BackColor = Utility.Configuration.Config.UI.StatusBarBackColor;
			this.StripStatus.ForeColor = Utility.Configuration.Config.UI.StatusBarForeColor;

			Utility.Logger.Instance.LogAdded += new Utility.LogAddedEventHandler((Utility.Logger.LogData data) =>
			{
				if (InvokeRequired)
				{
					// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
					// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
					// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
					Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
				}
				else
				{
					Logger_LogAdded(data);
				}
			});
			Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

			Utility.Logger.Add(2, SoftwareInformation.SoftwareNameEnglish + " is starting...");


			ResourceManager.Instance.Load();
			RecordManager.Instance.Load();
			KCDatabase.Instance.Load();
			NotifierManager.Instance.Initialize(this);
			SyncBGMPlayer.Instance.ConfigurationChanged();

			#region Icon settings
			Icon = ResourceManager.Instance.AppIcon;

			StripMenu_File_Configuration.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];

			StripMenu_View_Fleet.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
			StripMenu_View_FleetOverview.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
			StripMenu_View_ShipGroup.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup];
			StripMenu_View_Dock.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDock];
			StripMenu_View_Arsenal.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal];
			StripMenu_View_Headquarters.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters];
			StripMenu_View_Quest.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest];
			StripMenu_View_Information.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
			StripMenu_View_Compass.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass];
			StripMenu_View_Battle.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle];
			StripMenu_View_Browser.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser];
			StripMenu_View_Log.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog];
			StripMenu_WindowCapture.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormWindowCapture];
			StripMenu_View_BaseAirCorps.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
			StripMenu_View_Json.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormJson];

			StripMenu_Tool_EquipmentList.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];
			StripMenu_Tool_DropRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDropRecord];
			StripMenu_Tool_DevelopmentRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDevelopmentRecord];
			StripMenu_Tool_ConstructionRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConstructionRecord];
			StripMenu_Tool_ResourceChart.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart];
			StripMenu_Tool_AlbumMasterShip.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip];
			StripMenu_Tool_AlbumMasterEquipment.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumEquipment];
			StripMenu_Tool_AntiAirDefense.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAntiAirDefense];
			StripMenu_Tool_FleetImageGenerator.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetImageGenerator];
			StripMenu_Tool_BaseAirCorpsSimulation.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
			StripMenu_Tool_ExpChecker.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpChecker];

			StripMenu_Help_Help.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
			StripMenu_Help_Version.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AppIcon];
			#endregion


			APIObserver.Instance.Start(Utility.Configuration.Config.Connection.Port, this);


			MainDockPanel.Extender.FloatWindowFactory = new CustomFloatWindowFactory();


			SubForms = new List<DockContent>();

			//form init
			//注：一度全てshowしないとイベントを受け取れないので注意
			fFleet = new FormFleet[4];
			for (int i = 0; i < fFleet.Length; i++)
			{
				SubForms.Add(fFleet[i] = new FormFleet(this, i + 1));
			}

			SubForms.Add(fDock = new FormDock(this));
			SubForms.Add(fArsenal = new FormArsenal(this));
			SubForms.Add(fHeadquarters = new FormHeadquarters(this));
			SubForms.Add(fInformation = new FormInformation(this));
			SubForms.Add(fCompass = new FormCompass(this));
			SubForms.Add(fLog = new FormLog(this));
			SubForms.Add(fQuest = new FormQuest(this));
			SubForms.Add(fBattle = new FormBattle(this));
			SubForms.Add(fFleetOverview = new FormFleetOverview(this));
			SubForms.Add(fShipGroup = new FormShipGroup(this));
			SubForms.Add(fBrowser = new FormBrowserHost(this));
			SubForms.Add(fWindowCapture = new FormWindowCapture(this));
			SubForms.Add(fXPCalculator = new FormXPCalculator(this));
			SubForms.Add(fBaseAirCorps = new FormBaseAirCorps(this));
			SubForms.Add(fJson = new FormJson(this));

			ConfigurationChanged();     //設定から初期化

			LoadLayout(Configuration.Config.Life.LayoutFilePath);



			SoftwareInformation.CheckUpdate();

			// デバッグ: 開始時にAPIリストを読み込む
			if (Configuration.Config.Debug.LoadAPIListOnLoad)
			{

				try
				{

					await Task.Factory.StartNew(() => LoadAPIList(Configuration.Config.Debug.APIListPath));

					Activate();     // 上記ロードに時間がかかるとウィンドウが表示されなくなることがあるので
				}
				catch (Exception ex)
				{

					Utility.Logger.Add( 3, LoggerRes.FailedLoadAPI + ex.Message );
				}
			}

			APIObserver.Instance.ResponseReceived += (a, b) => UpdatePlayTime();


			// 🎃
			if (DateTime.Now.Month == 10 && DateTime.Now.Day == 31)
			{
				APIObserver.Instance.APIList["api_port/port"].ResponseReceived += CallPumpkinHead;
			}

			// 完了通知（ログインページを開く）
			fBrowser.InitializeApiCompleted();

			UIUpdateTimer.Start();


			Utility.Logger.Add(3, Resources.StartupComplete);

		}


		private void FormMain_Shown(object sender, EventArgs e)
		{
			// Load で設定すると無視されるかバグる(タスクバーに出なくなる)のでここで設定
			TopMost = Utility.Configuration.Config.Life.TopMost;
			// HACK: タスクバーに表示されなくなる不具合への応急処置　効くかは知らない
			ShowInTaskbar = false;
			ShowInTaskbar = true;

		}

		// Toggle TopMost of Main Form back and forth to workaround a .Net Bug: KB2756203 (~win7) / KB2769674 (win8~)
		private void FormMain_RefreshTopMost()
		{
			TopMost = !TopMost;
			TopMost = !TopMost;
		}


		private void ConfigurationChanged()
		{

			var c = Utility.Configuration.Config;

			StripMenu_Debug.Enabled = StripMenu_Debug.Visible =
			StripMenu_View_Json.Enabled = StripMenu_View_Json.Visible =
				c.Debug.EnableDebugMenu;

			StripStatus.Visible = c.Life.ShowStatusBar;

			// Load で TopMost を変更するとバグるため(前述)
			if (UIUpdateTimer.Enabled)
				TopMost = c.Life.TopMost;

			ClockFormat = c.Life.ClockFormat;

			Font = c.UI.MainFont;
			//StripMenu.Font = Font;
			StripStatus.Font = Font;
			MainDockPanel.Skin.AutoHideStripSkin.TextFont = Font;
			MainDockPanel.Skin.DockPaneStripSkin.TextFont = Font;


			foreach (var f in SubForms)
			{
				f.BackColor = this.BackColor;
				f.ForeColor = this.ForeColor;
				if (f is FormShipGroup)
				{ // 暂时不对舰队编成窗口应用主题
					f.BackColor = SystemColors.Control;
					f.ForeColor = SystemColors.ControlText;
				}
			}

			StripStatus_Information.BackColor = System.Drawing.Color.Transparent;
			StripStatus_Information.Margin = new Padding(-1, 1, -1, 0);


			if ( c.Life.LockLayout )
			{
				MainDockPanel.AllowChangeLayout = false;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			}
			else
			{
				MainDockPanel.AllowChangeLayout = true;
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			}

			StripMenu_File_Layout_LockLayout.Checked = c.Life.LockLayout;
			MainDockPanel.CanCloseFloatWindowInLock = c.Life.CanCloseFloatWindowInLock;

			StripMenu_File_Layout_TopMost.Checked = c.Life.TopMost;

			StripMenu_File_Notification_MuteAll.Checked = Notifier.NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

			if (!c.Control.UseSystemVolume)
				_volumeUpdateState = -1;
		}






		private void StripMenu_Debug_LoadAPIFromFile_Click(object sender, EventArgs e)
		{

			/*/
			using ( var dialog = new DialogLocalAPILoader() ) {

				if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
					if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

						if ( dialog.IsResponse ) {
							APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
						}
						if ( dialog.IsRequest ) {
							APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
						}

					}
				}
			}
			/*/
			new DialogLocalAPILoader2().Show(this);
			//*/
		}



		private void UIUpdateTimer_Tick(object sender, EventArgs e)
		{

			SystemEvents.OnUpdateTimerTick();

			// 東京標準時
			var now = DateTime.UtcNow + new TimeSpan(9, 0, 0);

			switch ( ClockFormat ) {
				case 0:	//時計表示
					var pvpReset = now.Date.AddHours( 3 );
					while (pvpReset < now)
						pvpReset = pvpReset.AddHours( 12 );
					var pvpTimer = pvpReset - now;

					var questReset = now.Date.AddHours( 5 );
					if (questReset < now)
						questReset = questReset.AddHours( 24 );
					var questTimer = questReset - now;

					DateTime maintDate = now;
					TimeSpan maintTimer = now - now;
					if (SoftwareUpdater.MaintState != 0)
					{
						maintDate = DateTimeHelper.CSVStringToTime(SoftwareUpdater.MaintDate);
						if (maintDate < now)
							maintDate = now;
						maintTimer = maintDate - now;
					}

					string maintState;
					switch (SoftwareUpdater.MaintState)
					{
						default:
							maintState = string.Empty;
							break;
						case 1:
							if (maintDate > now)
							{
								maintState = string.Format("Event starts in: {0:D2}:{1:D2}:{2:D2}",
									(int)maintTimer.TotalHours, maintTimer.Minutes, maintTimer.Seconds);
							}
							else
								maintState = "Event has started!";
							break;
						case 2:
							if (maintDate > now)
							{
								maintState = string.Format("Event ends in: {0:D2}:{1:D2}:{2:D2}",
									(int) maintTimer.TotalHours, maintTimer.Minutes, maintTimer.Seconds);
							}
							else
								maintState = "Event period has ended.";
							break;
						case 3:
							if (maintDate > now)
							{
								maintState = string.Format("Next maintenance: {0:D2}:{1:D2}:{2:D2}",
									(int)maintTimer.TotalHours, maintTimer.Minutes, maintTimer.Seconds);
							}
							else
								maintState = "Maintenance has started.";
							break;
					}

					var resetMsg = string.Format( "Next PVP Reset: {0:D2}:{1:D2}:{2:D2}\r\n" +
					                                 "Next Quest Reset: {3:D2}:{4:D2}:{5:D2}\r\n" +
													 "{6}",
						(int)pvpTimer.TotalHours, pvpTimer.Minutes, pvpTimer.Seconds,
						(int)questTimer.TotalHours, questTimer.Minutes, questTimer.Seconds,
						maintState);

					StripStatus_Clock.Text = now.ToString( "HH\\:mm\\:ss" );
					StripStatus_Clock.ToolTipText = now.ToString( "yyyy\\/MM\\/dd (ddd)\r\n" ) + resetMsg;

					break;

				case 1: //演習更新まで
					{
						var border = now.Date.AddHours(3);
						while (border < now)
							border = border.AddHours(12);

						var ts = border - now;
						StripStatus_Clock.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
						StripStatus_Clock.ToolTipText = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

					}
					break;

				case 2: //任務更新まで
					{
						var border = now.Date.AddHours(5);
						if (border < now)
							border = border.AddHours(24);

						var ts = border - now;
						StripStatus_Clock.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
						StripStatus_Clock.ToolTipText = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

					}
					break;
			}


			// WMP コントロールによって音量が勝手に変えられてしまうため、前回終了時の音量の再設定を試みる。
			// 10回試行してダメなら諦める(例外によるラグを防ぐため)
			// 起動直後にやらないのはちょっと待たないと音量設定が有効にならないから
			if (_volumeUpdateState != -1 && _volumeUpdateState < 10 && Utility.Configuration.Config.Control.UseSystemVolume)
			{

				try
				{
					uint id = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
					float volume = Utility.Configuration.Config.Control.LastVolume;
					bool mute = Utility.Configuration.Config.Control.LastIsMute;

					BrowserLib.VolumeManager.SetApplicationVolume(id, volume);
					BrowserLib.VolumeManager.SetApplicationMute(id, mute);

					SyncBGMPlayer.Instance.SetInitialVolume((int)(volume * 100));
					foreach (var not in NotifierManager.Instance.GetNotifiers())
						not.SetInitialVolume((int)(volume * 100));

					_volumeUpdateState = -1;

				}
				catch (Exception)
				{

					_volumeUpdateState++;
				}
			}

		}




		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{

			if ( Utility.Configuration.Config.Life.ConfirmOnClosing ) {
				if ( MessageBox.Show( "Are you sure you want to exit?", "Electronic Observer", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
					== System.Windows.Forms.DialogResult.No ) {
					e.Cancel = true;
					return;
				}
			}


			Utility.Logger.Add( 2, SoftwareInformation.SoftwareNameEnglish + Resources.IsClosing );

			UIUpdateTimer.Stop();

			fBrowser.CloseBrowser();

			UpdatePlayTime();


			SystemEvents.OnSystemShuttingDown();


			SaveLayout(Configuration.Config.Life.LayoutFilePath);


			// 音量の保存
			{
				try
				{
					uint id = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
					Utility.Configuration.Config.Control.LastVolume = BrowserLib.VolumeManager.GetApplicationVolume(id);
					Utility.Configuration.Config.Control.LastIsMute = BrowserLib.VolumeManager.GetApplicationMute(id);

				}
				catch (Exception)
				{
					/* ぷちっ */
				}

			}
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{

			NotifierManager.Instance.ApplyToConfiguration();
			Utility.Configuration.Instance.Save();
			RecordManager.Instance.SavePartial();
			KCDatabase.Instance.Save();
			APIObserver.Instance.Stop();


			Utility.Logger.Add( 2, Resources.ClosingComplete );

			if (Utility.Configuration.Config.Log.SaveLogFlag)
				Utility.Logger.Save();

		}



		private IDockContent GetDockContentFromPersistString(string persistString)
		{

			switch (persistString)
			{
				case "Fleet #1":
					return fFleet[0];
				case "Fleet #2":
					return fFleet[1];
				case "Fleet #3":
					return fFleet[2];
				case "Fleet #4":
					return fFleet[3];
				case "Dock":
					return fDock;
				case "Arsenal":
					return fArsenal;
				case "HeadQuarters":
					return fHeadquarters;
				case "Information":
					return fInformation;
				case "Compass":
					return fCompass;
				case "Log":
					return fLog;
				case "Quest":
					return fQuest;
				case "Battle":
					return fBattle;
				case "FleetOverview":
					return fFleetOverview;
				//case "ShipGroup":
				//	return fShipGroup;
				case "Browser":
					return fBrowser;
				case "WindowCapture":
					return fWindowCapture;
				case "BaseAirCorps":
					return fBaseAirCorps;
				case "Json":
					return fJson;
				default:
					if (persistString.StartsWith("ShipGroup"))
					{
						fShipGroup.ConfigureFromPersistString(persistString);
						return fShipGroup;
					}
					if (persistString.StartsWith(FormIntegrate.PREFIX))
					{
						return FormIntegrate.FromPersistString(this, persistString);
					}
					return null;
			}

		}



		private void LoadSubWindowsLayout(Stream stream)
		{

			try
			{

				if (stream != null)
				{

					// 取り込んだウィンドウは一旦デタッチして閉じる
					fWindowCapture.CloseAll();

					foreach (var f in SubForms)
					{
						f.Show(MainDockPanel, DockState.Document);
						f.DockPanel = null;
					}

					MainDockPanel.LoadFromXml(stream, new DeserializeDockContent(GetDockContentFromPersistString));


					fWindowCapture.AttachAll();

				}
				else
				{

					foreach (var f in SubForms)
						f.Show(MainDockPanel);


					foreach (var x in MainDockPanel.Contents)
					{
						x.DockHandler.Hide();
					}
				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedLoadSubLayout);
			}

		}


		private void SaveSubWindowsLayout(Stream stream)
		{

			try
			{

				MainDockPanel.SaveAsXml(stream, Encoding.UTF8);

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedSaveLayout);
			}

		}



		private void LoadLayout(string path)
		{

			try
			{
				using (var archive = new ZipArchive(File.OpenRead(path), ZipArchiveMode.Read))
				{
					MainDockPanel.SuspendLayout(true);

					WindowPlacementManager.LoadWindowPlacement(this, archive.GetEntry("WindowPlacement.xml").Open());
					LoadSubWindowsLayout(archive.GetEntry("SubWindowLayout.xml").Open());
				}

				Utility.Logger.Add(2, "Successfully loaded window layout from " + path);

			}
			catch (FileNotFoundException)
			{

				Utility.Logger.Add(3, string.Format(Resources.NoLayoutFound));
				MessageBox.Show(Resources.InitLayout, Resources.NoLayoutFound,
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				fBrowser.Show(MainDockPanel);

			}
			catch (DirectoryNotFoundException)
			{

				Utility.Logger.Add(3, string.Format(Resources.NoLayoutFound));
				MessageBox.Show(Resources.InitLayout, Resources.NoLayoutFound,
					MessageBoxButtons.OK, MessageBoxIcon.Information);

				fBrowser.Show(MainDockPanel);

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, LoggerRes.FailedLoadLayout);

			}
			finally
			{

				MainDockPanel.ResumeLayout(true, true);
			}

		}

		private void SaveLayout(string path)
		{

			try
			{

				CreateParentDirectories(path);

				using (var archive = new ZipArchive(File.Open(path, FileMode.Create), ZipArchiveMode.Create))
				{

					using (var layoutstream = archive.CreateEntry("SubWindowLayout.xml").Open())
					{
						SaveSubWindowsLayout(layoutstream);
					}
					using (var placementstream = archive.CreateEntry("WindowPlacement.xml").Open())
					{
						WindowPlacementManager.SaveWindowPlacement(this, placementstream);
					}
				}


				Utility.Logger.Add( 2, string.Format(Resources.LayoutSaved, path) );

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport( ex, LoggerRes.FailedSaveLayout );
			}

		}

		private void CreateParentDirectories(string path)
		{

			var parents = Path.GetDirectoryName(path);

			if (!String.IsNullOrEmpty(parents))
			{
				Directory.CreateDirectory(parents);
			}

		}



		void Logger_LogAdded(Utility.Logger.LogData data)
		{

			StripStatus_Information.Text = data.Message.Replace("\r", " ").Replace("\n", " ");

		}


		private void StripMenu_Help_Version_Click(object sender, EventArgs e)
		{

			using (var dialog = new DialogVersion())
			{
				dialog.ShowDialog(this);
			}

		}

		private void StripMenu_File_Configuration_Click(object sender, EventArgs e)
		{

			UpdatePlayTime();

			using (var dialog = new DialogConfiguration(Utility.Configuration.Config))
			{
				if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				{

					dialog.ToConfiguration(Utility.Configuration.Config);
					Utility.Configuration.Instance.OnConfigurationChanged();

				}
			}
		}

		private void StripMenu_File_Close_Click(object sender, EventArgs e)
		{
			Close();
		}


		private void StripMenu_File_SaveData_Save_Click(object sender, EventArgs e)
		{

			RecordManager.Instance.SaveAll();

		}

		private void StripMenu_File_SaveData_Load_Click(object sender, EventArgs e)
		{

			if ( MessageBox.Show( Resources.AskLoad, "Confirmation",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 )
				== System.Windows.Forms.DialogResult.Yes ) {

				RecordManager.Instance.Load();
			}

		}



		private async void StripMenu_Debug_LoadInitialAPI_Click(object sender, EventArgs e)
		{

			using (OpenFileDialog ofd = new OpenFileDialog())
			{

				ofd.Title = "Load API List";
				ofd.Filter = "API List|*.txt|File|*";
				ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;
				if (!string.IsNullOrWhiteSpace(Utility.Configuration.Config.Debug.APIListPath))
					ofd.FileName = Utility.Configuration.Config.Debug.APIListPath;

				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{

					try
					{

						await Task.Factory.StartNew(() => LoadAPIList(ofd.FileName));

					}
					catch (Exception ex)
					{

						MessageBox.Show( "Failed to load API List.\r\n" + ex.Message, "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error );

					}

				}

			}

		}



		private void LoadAPIList(string path)
		{

			string parent = Path.GetDirectoryName(path);

			using (StreamReader sr = new StreamReader(path))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{

					bool isRequest = false;
					{
						int slashindex = line.IndexOf('/');
						if (slashindex != -1)
						{

							switch (line.Substring(0, slashindex).ToLower())
							{
								case "q":
								case "request":
									isRequest = true;
									goto case "s";
								case "":
								case "s":
								case "response":
									line = line.Substring(Math.Min(slashindex + 1, line.Length));
									break;
							}

						}
					}

					if (APIObserver.Instance.APIList.ContainsKey(line))
					{
						APIBase api = APIObserver.Instance.APIList[line];

						if (isRequest ? api.IsRequestSupported : api.IsResponseSupported)
						{

							string[] files = Directory.GetFiles(parent, string.Format("*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace('/', '@')), SearchOption.TopDirectoryOnly);

							if (files.Length == 0)
								continue;

							Array.Sort(files);

							using (StreamReader sr2 = new StreamReader(files[files.Length - 1]))
							{
								if (isRequest)
								{
									Invoke((Action)(() =>
									{
										APIObserver.Instance.LoadRequest("/kcsapi/" + line, sr2.ReadToEnd());
									}));
								}
								else
								{
									Invoke((Action)(() =>
									{
										APIObserver.Instance.LoadResponse("/kcsapi/" + line, sr2.ReadToEnd());
									}));
								}
							}

							//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
						}
					}
				}

			}

		}





		private void StripMenu_Debug_LoadRecordFromOld_Click(object sender, EventArgs e)
		{

			if ( KCDatabase.Instance.MasterShips.Count == 0 ) {
				MessageBox.Show( "Please load normal api_start2 first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}


			using (OpenFileDialog ofd = new OpenFileDialog())
			{

				ofd.Title = "Build Record from Old api_start2";
				ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{

					try
					{

						using (StreamReader sr = new StreamReader(ofd.FileName))
						{

							dynamic json = DynamicJson.Parse(sr.ReadToEnd().Remove(0, 7));

							foreach (dynamic elem in json.api_data.api_mst_ship)
							{
								if (elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey((int)elem.api_id) && KCDatabase.Instance.MasterShips[(int)elem.api_id].Name == elem.api_name)
								{
									RecordManager.Instance.ShipParameter.UpdateParameter((int)elem.api_id, 1, (int)elem.api_tais[0], (int)elem.api_tais[1], (int)elem.api_kaih[0], (int)elem.api_kaih[1], (int)elem.api_saku[0], (int)elem.api_saku[1]);

									int[] defaultslot = Enumerable.Repeat(-1, 5).ToArray();
									((int[])elem.api_defeq).CopyTo(defaultslot, 0);
									RecordManager.Instance.ShipParameter.UpdateDefaultSlot((int)elem.api_id, defaultslot);
								}
							}
						}

					}
					catch (Exception ex)
					{

						MessageBox.Show( "Failed to load API.\r\n" + ex.Message, "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error );
					}
				}
			}
		}


		private void StripMenu_Debug_LoadDataFromOld_Click(object sender, EventArgs e)
		{

			if ( KCDatabase.Instance.MasterShips.Count == 0 ) {
				MessageBox.Show( "Please load normal api_start2 first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}


			using (OpenFileDialog ofd = new OpenFileDialog())
			{

				ofd.Title = "Restore Abyssal Data from Old api_start2";
				ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";
				ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

				if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{

					try
					{

						using (StreamReader sr = new StreamReader(ofd.FileName))
						{

							dynamic json = DynamicJson.Parse(sr.ReadToEnd().Remove(0, 7));

							foreach (dynamic elem in json.api_data.api_mst_ship)
							{

								var ship = KCDatabase.Instance.MasterShips[(int)elem.api_id];

								if (elem.api_name != "なし" && ship != null && ship.IsAbyssalShip)
								{

									KCDatabase.Instance.MasterShips[(int)elem.api_id].LoadFromResponse("api_start2", elem);
								}
							}
						}

						Utility.Logger.Add(1, "Restored data from old api_start2");

					}
					catch (Exception ex)
					{

						MessageBox.Show("Failed to load API.\r\n" + ex.Message, "Error",
							MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}

		}


		private void StripMenu_Tool_AlbumMasterShip_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show("Ship data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}
			else
			{
				var dialogAlbumMasterShip = new DialogAlbumMasterShip();
				FormMain_RefreshTopMost();
				dialogAlbumMasterShip.Show(this);
			}

		}

		private void StripMenu_Tool_AlbumMasterEquipment_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterEquipments.Count == 0)
			{
				MessageBox.Show("Equipment data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}
			else
			{
				var dialogAlbumMasterEquipment = new DialogAlbumMasterEquipment();
				FormMain_RefreshTopMost();
				dialogAlbumMasterEquipment.Show(this);
			}

		}


		private async void StripMenu_Debug_DeleteOldAPI_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("This will delete old API data.\r\nAre you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
				== System.Windows.Forms.DialogResult.Yes)
			{

				try
				{

					int count = await Task.Factory.StartNew(() => DeleteOldAPI());

					MessageBox.Show("Delete successful.\r\n" + count + " files deleted.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}
				catch (Exception ex)
				{

					MessageBox.Show("Failed to delete.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}


			}

		}

		private int DeleteOldAPI()
		{


			//適当極まりない
			int count = 0;

			var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

			foreach (string s in Directory.EnumerateFiles(Utility.Configuration.Config.Connection.SaveDataPath, "*.json", SearchOption.TopDirectoryOnly))
			{

				int start = s.IndexOf('@');
				int end = s.LastIndexOf('.');

				start--;
				string key = s.Substring(start, end - start + 1);
				string date = s.Substring(0, start);


				if (!apilist.ContainsKey(key))
				{
					apilist.Add(key, new List<KeyValuePair<string, string>>());
				}
				apilist[key].Add(new KeyValuePair<string, string>(date, s));
			}

			foreach (var l in apilist.Values)
			{
				var l2 = l.OrderBy(el => el.Key).ToList();
				for (int i = 0; i < l2.Count - 1; i++)
				{
					File.Delete(l2[i].Value);
					count++;
				}
			}

			return count;
		}



		private void StripMenu_Tool_EquipmentList_Click(object sender, EventArgs e)
		{

			var dialogEquipmentList = new DialogEquipmentList();
			FormMain_RefreshTopMost();
			dialogEquipmentList.Show(this);

		}


		private async void StripMenu_Debug_RenameShipResource_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show("Ship data is not loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (MessageBox.Show("通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
				"対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
				"続行しますか？", "艦船リソースをリネーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				== System.Windows.Forms.DialogResult.Yes)
			{

				string path = null;

				using (FolderBrowserDialog dialog = new FolderBrowserDialog())
				{
					dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
					if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						path = dialog.SelectedPath;
					}
				}

				if (path == null) return;



				try
				{

					int count = await Task.Factory.StartNew(() => RenameShipResource(path));

					MessageBox.Show(string.Format("リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count), "処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information);


				}
				catch (Exception ex)
				{

					Utility.ErrorReporter.SendErrorReport(ex, "艦船リソースのリネームに失敗しました。");
					MessageBox.Show("艦船リソースのリネームに失敗しました。\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

				}



			}

		}


		private int RenameShipResource(string path)
		{

			int count = 0;

			foreach (var p in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
			{

				string name = Path.GetFileName(p);

				foreach (var ship in KCDatabase.Instance.MasterShips.Values)
				{

					if (name.Contains(ship.ResourceName))
					{

						name = name.Replace(ship.ResourceName, string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

						try
						{

							File.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
							count++;
							break;

						}
						catch (IOException)
						{
							//ファイルが既に存在する：＊にぎりつぶす＊
						}

					}

				}

			}

			foreach (var p in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
			{

				string name = Path.GetFileName(p);      //GetDirectoryName だと親フォルダへのパスになってしまうため

				foreach (var ship in KCDatabase.Instance.MasterShips.Values)
				{

					if (name.Contains(ship.ResourceName))
					{

						name = name.Replace(ship.ResourceName, string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

						try
						{

							Directory.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
							count++;
							break;

						}
						catch (IOException)
						{
							//フォルダが既に存在する：＊にぎりつぶす＊
						}
					}

				}

			}


			return count;
		}


		private void StripMenu_Help_Help_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("This will open the EO wiki with your browser.\r\nAre you sure?", "Help",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				== System.Windows.Forms.DialogResult.Yes)
			{

				System.Diagnostics.Process.Start("https://github.com/silfumus/ElectronicObserver/wiki");
			}

		}

		private void StripMenu_Help_Issue_Click(object sender, EventArgs e)
		{

			if (MessageBox.Show("This will open a page with your browser.\r\nAre you sure?", "Report A Problem",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
				== System.Windows.Forms.DialogResult.Yes)
			{

				System.Diagnostics.Process.Start("https://gitreports.com/issue/silfumus/ElectronicObserver");
			}

		}

		private void StripMenu_Help_Update_Click( object sender, EventArgs e ) {
			SoftwareInformation.CheckUpdate();
		}


		private void SeparatorWhitecap_Click(object sender, EventArgs e)
		{
			new DialogWhitecap().Show(this);
		}



		private void StripMenu_File_Layout_Load_Click(object sender, EventArgs e)
		{

			LoadLayout(Utility.Configuration.Config.Life.LayoutFilePath);

		}

		private void StripMenu_File_Layout_Save_Click(object sender, EventArgs e)
		{

			SaveLayout(Utility.Configuration.Config.Life.LayoutFilePath);

		}

		private void StripMenu_File_Layout_Open_Click(object sender, EventArgs e)
		{

			using (var dialog = new OpenFileDialog())
			{

				dialog.Filter = "Layout Archive|*.zip|File|*";
				dialog.Title = "Open Layout File";


				PathHelper.InitOpenFileDialog(Utility.Configuration.Config.Life.LayoutFilePath, dialog);

				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{

					Utility.Configuration.Config.Life.LayoutFilePath = PathHelper.GetPathFromOpenFileDialog(dialog);
					LoadLayout(Utility.Configuration.Config.Life.LayoutFilePath);

				}

			}

		}

		private void StripMenu_File_Layout_Change_Click(object sender, EventArgs e)
		{

			using (var dialog = new SaveFileDialog())
			{

				dialog.Filter = "Layout Archive|*.zip|File|*";
				dialog.Title = "Save Layout As";


				PathHelper.InitSaveFileDialog(Utility.Configuration.Config.Life.LayoutFilePath, dialog);

				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{

					Utility.Configuration.Config.Life.LayoutFilePath = PathHelper.GetPathFromSaveFileDialog(dialog);
					SaveLayout(Utility.Configuration.Config.Life.LayoutFilePath);

				}
			}

		}


		private void StripMenu_Tool_ResourceChart_Click(object sender, EventArgs e)
		{

			var dialogResourceChart = new DialogResourceChart();
			FormMain_RefreshTopMost();
			dialogResourceChart.Show(this);

		}

		private void StripMenu_Tool_DropRecord_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (RecordManager.Instance.ShipDrop.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			new Dialog.DialogDropRecordViewer().Show(this);

		}


		private void StripMenu_Tool_DevelopmentRecord_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (RecordManager.Instance.Development.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoDevData, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			new Dialog.DialogDevelopmentRecordViewer().Show(this);

		}

		private void StripMenu_Tool_ConstructionRecord_Click(object sender, EventArgs e)
		{

			if (KCDatabase.Instance.MasterShips.Count == 0)
			{
				MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (RecordManager.Instance.Construction.Record.Count == 0)
			{
				MessageBox.Show(GeneralRes.NoBuildData, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			new Dialog.DialogConstructionRecordViewer().Show(this);

		}

		private void StripMenu_Tool_AntiAirDefense_Click(object sender, EventArgs e)
		{

			new Dialog.DialogAntiAirDefense().Show(this);

		}

		private void StripMenu_Tool_FleetImageGenerator_Click(object sender, EventArgs e)
		{

			new Dialog.DialogFleetImageGenerator(1).Show(this);
		}

		private void StripMenu_Tool_BaseAirCorpsSimulation_Click(object sender, EventArgs e)
		{

			new Dialog.DialogBaseAirCorpsSimulation().Show(this);
		}


		private void StripMenu_File_Layout_LockLayout_Click(object sender, EventArgs e)
		{

			Utility.Configuration.Config.Life.LockLayout = StripMenu_File_Layout_LockLayout.Checked;
			ConfigurationChanged();

		}

		private void StripMenu_File_Layout_TopMost_Click(object sender, EventArgs e)
		{

			Utility.Configuration.Config.Life.TopMost = StripMenu_File_Layout_TopMost.Checked;
			ConfigurationChanged();

		}


		private void StripMenu_File_Notification_MuteAll_Click(object sender, EventArgs e)
		{
			bool isSilenced = StripMenu_File_Notification_MuteAll.Checked;

			foreach (var n in NotifierManager.Instance.GetNotifiers())
				n.IsSilenced = isSilenced;
		}

		private void StripMenu_Tool_ExpChecker_Click(object sender, EventArgs e)
		{
			new Dialog.DialogExpChecker().Show(this);
		}



		private void CallPumpkinHead(string apiname, dynamic data)
		{
			new DialogHalloween().Show(this);
			APIObserver.Instance.APIList["api_port/port"].ResponseReceived -= CallPumpkinHead;
		}


		private void StripMenu_WindowCapture_AttachAll_Click(object sender, EventArgs e)
		{
			fWindowCapture.AttachAll();
		}

		private void StripMenu_WindowCapture_DetachAll_Click(object sender, EventArgs e)
		{
			fWindowCapture.DetachAll();
		}



		private void UpdatePlayTime()
		{
			var c = Utility.Configuration.Config.Log;
			DateTime now = DateTime.Now;

			double span = (now - _prevPlayTimeRecorded).TotalSeconds;
			if (span < c.PlayTimeIgnoreInterval)
			{
				c.PlayTime += span;
			}

			_prevPlayTimeRecorded = now;
		}




		#region フォーム表示

		/// <summary>
		/// 子フォームを表示します。既に表示されている場合はフォームをある点に移動します。（失踪対策）
		/// </summary>
		/// <param name="form"></param>
		private void ShowForm(DockContent form)
		{
			if (form.IsFloat && form.Visible)
			{
				form.FloatPane.FloatWindow.Location = new Point(128, 128);
			}

			form.Show(MainDockPanel);
		}

		private void StripMenu_View_Fleet_1_Click(object sender, EventArgs e)
		{
			ShowForm(fFleet[0]);
		}

		private void StripMenu_View_Fleet_2_Click(object sender, EventArgs e)
		{
			ShowForm(fFleet[1]);
		}

		private void StripMenu_View_Fleet_3_Click(object sender, EventArgs e)
		{
			ShowForm(fFleet[2]);
		}

		private void StripMenu_View_Fleet_4_Click(object sender, EventArgs e)
		{
			ShowForm(fFleet[3]);
		}

		private void StripMenu_View_Dock_Click(object sender, EventArgs e)
		{
			ShowForm(fDock);
		}

		private void StripMenu_View_Arsenal_Click(object sender, EventArgs e)
		{
			ShowForm(fArsenal);
		}

		private void StripMenu_View_Headquarters_Click(object sender, EventArgs e)
		{
			ShowForm(fHeadquarters);
		}

		private void StripMenu_View_Information_Click(object sender, EventArgs e)
		{
			ShowForm(fInformation);
		}

		private void StripMenu_View_Compass_Click(object sender, EventArgs e)
		{
			ShowForm(fCompass);
		}

		private void StripMenu_View_Log_Click(object sender, EventArgs e)
		{
			ShowForm(fLog);
		}

		private void StripMenu_View_Quest_Click(object sender, EventArgs e)
		{
			ShowForm(fQuest);
		}

		private void StripMenu_View_Battle_Click(object sender, EventArgs e)
		{
			ShowForm(fBattle);
		}

		private void StripMenu_View_FleetOverview_Click(object sender, EventArgs e)
		{
			ShowForm(fFleetOverview);
		}

		private void StripMenu_View_ShipGroup_Click(object sender, EventArgs e)
		{
			ShowForm(fShipGroup);
		}

		private void StripMenu_View_Browser_Click(object sender, EventArgs e)
		{
			ShowForm(fBrowser);
		}

		private void StripMenu_WindowCapture_SubWindow_Click(object sender, EventArgs e)
		{
			ShowForm(fWindowCapture);
		}

		private void StripMenu_View_XPCalculator_Click(object sender, EventArgs e)
		{
			ShowForm(fXPCalculator);
		}

		private void StripMenu_View_BaseAirCorps_Click(object sender, EventArgs e)
		{
			ShowForm(fBaseAirCorps);
		}

		private void StripMenu_View_Json_Click(object sender, EventArgs e)
		{
			ShowForm(fJson);
		}





		#endregion


	}
}
