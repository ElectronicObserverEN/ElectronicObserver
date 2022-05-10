using System;
using System.Collections.Generic;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.Dock;

public class DockItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public string? ToolTip { get; set; }
	public DateTime? Tag { get; set; }
	public int MaximumWidth { get; set; }

	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
}

public class DockItemViewModel : ObservableObject
{
	public DockItemControlViewModel ShipName { get; }
	public DockItemControlViewModel RepairTime { get; }

	public DockItemViewModel()
	{
		ShipName = new()
		{
			Text = "???",
			/*
			Anchor = AnchorStyles.Left,
			ForeColor = parent.ForeColor,
			TextAlign = ContentAlignment.MiddleLeft,
			Padding = new Padding(0, 1, 0, 1),
			Margin = new Padding(2, 1, 2, 1),
			MaximumSize = new Size(60, int.MaxValue),
			//ShipName.AutoEllipsis = true;
			ImageAlign = ContentAlignment.MiddleCenter,
			AutoSize = true,
			Visible = true
			*/
		};

		RepairTime = new()
		{
			Text = "",
			/*
			Anchor = AnchorStyles.Left,
			ForeColor = parent.ForeColor,
			Tag = null,
			TextAlign = ContentAlignment.MiddleLeft,
			Padding = new Padding(0, 1, 0, 1),
			Margin = new Padding(2, 1, 2, 1),
			MinimumSize = new Size(60, 10),
			AutoSize = true,
			Visible = true
			*/
		};

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}
	
	//データ更新時
	public void Update(int dockID)
	{
		KCDatabase db = KCDatabase.Instance;
		DockData dock = db.Docks[dockID];

		RepairTime.BackColor = System.Drawing.Color.Transparent;
		RepairTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		ShipName.ToolTip = null;
		RepairTime.ToolTip = null;

		if (dock == null || dock.State == -1)
		{
			//locked
			ShipName.Text = "";
			RepairTime.Text = "";
			RepairTime.Tag = null;
		}
		else if (dock.State == 0)
		{
			//empty
			ShipName.Text = "----";
			RepairTime.Text = "";
			RepairTime.Tag = null;
		}
		else
		{
			//repairing
			ShipName.Text = db.Ships[dock.ShipID].Name;
			ShipName.ToolTip = db.Ships[dock.ShipID].NameWithLevel;
			RepairTime.Text = DateTimeHelper.ToTimeRemainString(dock.CompletionTime);
			RepairTime.Tag = dock.CompletionTime;
			RepairTime.ToolTip = GeneralRes.TimeToCompletion + ":" + DateTimeHelper.TimeToCSVString(dock.CompletionTime);
		}
	}

	//タイマー更新時
	public void Refresh(int dockID)
	{
		if (RepairTime.Tag == null) return;

		var time = (DateTime)RepairTime.Tag;

		RepairTime.Text = DateTimeHelper.ToTimeRemainString(time);

		if (Utility.Configuration.Config.FormDock.BlinkAtCompletion && (time - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierRepair.AccelInterval)
		{
			RepairTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Dock_RepairFinishedBG : System.Drawing.Color.Transparent;
			RepairTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Dock_RepairFinishedFG : Utility.Configuration.Config.UI.ForeColor;
		}
	}

	private void ConfigurationChanged()
	{
		ShipName.MaximumWidth = Utility.Configuration.Config.FormDock.MaxShipNameWidth;
	}
}

public class DockViewModel : AnchorableViewModel
{
	public FormDockTranslationViewModel FormDock { get; }
	public List<DockItemViewModel> Docks { get; }

	public DockViewModel() : base("Dock", "Dock",
		ImageSourceIcons.GetIcon(IconContent.FormDock))
	{
		FormDock = Ioc.Default.GetService<FormDockTranslationViewModel>()!;

		Title = FormDock.Title;
		FormDock.PropertyChanged += (_, _) => Title = FormDock.Title;

		Docks = new()
		{
			new(),
			new(),
			new(),
			new(),
		};

		APIObserver o = APIObserver.Instance;

		o.ApiReqNyukyo_Start.RequestReceived += Updated;
		o.ApiReqNyukyo_SpeedChange.RequestReceived += Updated;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_NDock.ResponseReceived += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;
	}



	void Updated(string apiname, dynamic data)
	{
		// TableDock.SuspendLayout();
		// TableDock.RowCount = KCDatabase.Instance.Docks.Values.Count(d => d.State != -1);
		for (int i = 0; i < Docks.Count; i++)
			Docks[i].Update(i + 1);
		// TableDock.ResumeLayout();
	}


	void UpdateTimerTick()
	{
		//TableDock.SuspendLayout();
		for (int i = 0; i < Docks.Count; i++)
			Docks[i].Refresh(i + 1);
		//TableDock.ResumeLayout();
	}

	void ConfigurationChanged()
	{
		/*
		Font = Utility.Configuration.Config.UI.MainFont;

		if (ControlDock != null)
		{
			TableDock.SuspendLayout();

			foreach (var c in ControlDock)
				c.ConfigurationChanged(this);

			ControlHelper.SetTableRowStyles(TableDock, ControlHelper.GetDefaultRowStyle());

			TableDock.ResumeLayout();
		}
		*/

	}
}
