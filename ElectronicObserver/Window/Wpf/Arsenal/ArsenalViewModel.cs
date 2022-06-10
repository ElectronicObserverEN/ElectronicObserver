using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Arsenal;

public class ArsenalItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public DateTime? Tag { get; set; }
	public bool Visible { get; set; }
	public string? ToolTip { get; set; }
	public int MaximumWidth { get; set; }

	public Visibility Visibility => Visible.ToVisibility();
	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
}

public class ArsenalItemViewModel : ObservableObject
{
	public ArsenalItemControlViewModel ShipName { get; }
	public ArsenalItemControlViewModel CompletionTime { get; }

	public ArsenalItemViewModel()
	{
		ShipName = new()
		{
			Text = "???",
			// Anchor = AnchorStyles.Left,
			// ForeColor = parent.ForeColor,
			// TextAlign = ContentAlignment.MiddleLeft,
			// Padding = new Padding(0, 1, 0, 1),
			// Margin = new Padding(2, 1, 2, 1),
			// MaximumSize = new Size(60, int.MaxValue),
			// //ShipName.AutoEllipsis = true;
			// ImageAlign = ContentAlignment.MiddleCenter,
			// AutoSize = true,
			Visible = true
		};

		CompletionTime = new()
		{
			Text = "",
			// Anchor = AnchorStyles.Left,
			// ForeColor = parent.ForeColor,
			Tag = null,
			// TextAlign = ContentAlignment.MiddleLeft,
			// Padding = new Padding(0, 1, 0, 1),
			// Margin = new Padding(2, 1, 2, 1),
			// MinimumSize = new Size(60, 10),
			// AutoSize = true,
			Visible = true
		};

		ConfigurationChanged();
	}

	public void Update(int arsenalID)
	{
		KCDatabase db = KCDatabase.Instance;
		ArsenalData arsenal = db.Arsenals[arsenalID];
		bool showShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;

		CompletionTime.BackColor = System.Drawing.Color.Transparent;
		CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		ShipName.ToolTip = null;
		CompletionTime.ToolTip = null;

		if (arsenal == null || arsenal.State == -1)
		{
			//locked
			ShipName.Text = "";
			CompletionTime.Text = "";
			CompletionTime.Tag = null;

		}
		else if (arsenal.State == 0)
		{
			//empty
			ShipName.Text = "----";
			CompletionTime.Text = "";
			CompletionTime.Tag = null;

		}
		else if (arsenal.State == 2)
		{
			//building
			string name = showShipName ? db.MasterShips[arsenal.ShipID].NameEN : "???";
			ShipName.Text = name;
			ShipName.ToolTip = name;
			CompletionTime.Text = DateTimeHelper.ToTimeRemainString(arsenal.CompletionTime);
			CompletionTime.Tag = arsenal.CompletionTime;
			CompletionTime.ToolTip = GeneralRes.TimeToCompletion + ":" + DateTimeHelper.TimeToCSVString(arsenal.CompletionTime);

		}
		else if (arsenal.State == 3)
		{
			//complete!
			string name = showShipName ? db.MasterShips[arsenal.ShipID].NameEN : "???";
			ShipName.Text = name;
			ShipName.ToolTip = name;
			CompletionTime.Text = GeneralRes.Complete + "!";
			CompletionTime.Tag = null;

		}
	}


	public void Refresh(int arsenalID)
	{
		if (CompletionTime.Tag != null)
		{

			var time = (DateTime)CompletionTime.Tag;

			CompletionTime.Text = DateTimeHelper.ToTimeRemainString(time);

			if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && (time - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierConstruction.AccelInterval)
			{
				CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteBG : System.Drawing.Color.Transparent;
				CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteFG : Utility.Configuration.Config.UI.ForeColor;
			}

		}
		else if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && !string.IsNullOrWhiteSpace(CompletionTime.Text))
		{
			//完成しているので
			CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteBG : System.Drawing.Color.Transparent;
			CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteFG : Utility.Configuration.Config.UI.ForeColor;
		}
	}


	public void ConfigurationChanged()
	{
		var config = Utility.Configuration.Config.FormArsenal;

		// ShipName.Font = parent.Font;
		// CompletionTime.Font = parent.Font;
		CompletionTime.BackColor = System.Drawing.Color.Transparent;
		ShipName.ForeColor = CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		ShipName.MaximumWidth = config.MaxShipNameWidth;
	}
}

public class ArsenalViewModel : AnchorableViewModel
{
	public FormArsenalTranslationViewModel FormArsenal { get; }
	public List<ArsenalItemViewModel> Arsenals { get; }
	public bool ShowShipName { get; set; }
	private int _buildingID;

	public ArsenalViewModel() : base("Arsenal", "Arsenal",
		ImageSourceIcons.GetIcon(IconContent.FormArsenal))
	{
		FormArsenal = Ioc.Default.GetService<FormArsenalTranslationViewModel>()!;

		Title = FormArsenal.Title;
		FormArsenal.PropertyChanged += (_, _) => Title = FormArsenal.Title;

		Arsenals = new()
		{
			new(),
			new(),
			new(),
			new(),
		};

		APIObserver o = APIObserver.Instance;

		o.ApiReqKousyou_CreateShip.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShipSpeedChange.RequestReceived += Updated;

		o.ApiGetMember_KDock.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiGetMember_RequireInfo.ResponseReceived += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

		ConfigurationChanged();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowShipName)) return;

			Utility.Configuration.Config.FormArsenal.ShowShipName = ShowShipName;
			UpdateUI();
		};
	}

	void Updated(string apiname, dynamic data)
	{

		if (_buildingID != -1 && apiname == "api_get_member/kdock")
		{

			ArsenalData arsenal = KCDatabase.Instance.Arsenals[_buildingID];
			IShipDataMaster ship = KCDatabase.Instance.MasterShips[arsenal.ShipID];
			string name;

			if (Utility.Configuration.Config.Log.ShowSpoiler && Utility.Configuration.Config.FormArsenal.ShowShipName)
			{

				name = string.Format("{0} {1}", ship.ShipTypeName, ship.NameWithClass);

			}
			else
			{

				name = GeneralRes.ShipGirl;
			}

			Utility.Logger.Add(2, string.Format(GeneralRes.ArsenalLog,
				_buildingID,
				name,
				arsenal.Fuel,
				arsenal.Ammo,
				arsenal.Steel,
				arsenal.Bauxite,
				arsenal.DevelopmentMaterial,
				KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel
			));

			_buildingID = -1;
		}

		if (apiname == "api_req_kousyou/createship")
		{
			_buildingID = int.Parse(data["api_kdock_id"]);
		}

		UpdateUI();

	}

	void UpdateUI()
	{
		// if (ControlArsenal == null) return;

		// TableArsenal.SuspendLayout();
		// TableArsenal.RowCount = KCDatabase.Instance.Arsenals.Values.Count(a => a.State != -1);
		for (int i = 0; i < Arsenals.Count; i++)
		{
			Arsenals[i].Update(i + 1);
		}

		// TableArsenal.ResumeLayout();
	}

	void UpdateTimerTick()
	{
		//TableArsenal.SuspendLayout();
		for (int i = 0; i < Arsenals.Count; i++)
			Arsenals[i].Refresh(i + 1);
		//TableArsenal.ResumeLayout();
	}


	void ConfigurationChanged()
	{
		// Font = Utility.Configuration.Config.UI.MainFont;
		ShowShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;

		// if (ControlArsenal == null) return;

		// TableArsenal.SuspendLayout();

		foreach (var c in Arsenals)
			c.ConfigurationChanged();

		// ControlHelper.SetTableRowStyles(TableArsenal, ControlHelper.GetDefaultRowStyle());

		// TableArsenal.ResumeLayout();
	}

}
