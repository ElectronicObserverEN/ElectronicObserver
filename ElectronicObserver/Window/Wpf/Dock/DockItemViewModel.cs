using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Wpf.Dock;

public class DockItemViewModel : ObservableObject
{
	public DockItemControlViewModel ShipName { get; }
	public DockItemControlViewModel RepairTime { get; }

	public DockItemViewModel()
	{
		ShipName = new()
		{
			Text = "???",
		};

		RepairTime = new()
		{
			Text = "",
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

		DateTime time = (DateTime)RepairTime.Tag;

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
