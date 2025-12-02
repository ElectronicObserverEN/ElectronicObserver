using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Wpf.Arsenal;

public class ArsenalItemViewModel : ObservableObject
{
	public ArsenalItemControlViewModel ShipName { get; }
	public ArsenalItemControlViewModel CompletionTime { get; }

	public ArsenalItemViewModel()
	{
		ShipName = new()
		{
			Text = "???",
			Visible = true,
		};

		CompletionTime = new()
		{
			Text = "",
			Tag = null,
			Visible = true,
		};

		ConfigurationChanged();
	}

	public void Update(int arsenalId)
	{
		KCDatabase db = KCDatabase.Instance;
		ArsenalData arsenal = db.Arsenals[arsenalId];
		bool showShipName = Configuration.Config.FormArsenal.ShowShipName;

		CompletionTime.BackColor = System.Drawing.Color.Transparent;
		CompletionTime.ForeColor = Configuration.Config.UI.ForeColor;
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


	public void Refresh(int arsenalId)
	{
		if (CompletionTime.Tag is DateTime time)
		{
			CompletionTime.Text = DateTimeHelper.ToTimeRemainString(time);

			if (Configuration.Config.FormArsenal.BlinkAtCompletion && (time - DateTime.Now).TotalMilliseconds <= Configuration.Config.NotifierConstruction.AccelInterval)
			{
				CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Configuration.Config.UI.Arsenal_BuildCompleteBG : System.Drawing.Color.Transparent;
				CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Configuration.Config.UI.Arsenal_BuildCompleteFG : Configuration.Config.UI.ForeColor;
			}
		}
		else if (Configuration.Config.FormArsenal.BlinkAtCompletion && !string.IsNullOrWhiteSpace(CompletionTime.Text))
		{
			//完成しているので
			CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Configuration.Config.UI.Arsenal_BuildCompleteBG : System.Drawing.Color.Transparent;
			CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Configuration.Config.UI.Arsenal_BuildCompleteFG : Configuration.Config.UI.ForeColor;
		}
	}

	public void ConfigurationChanged()
	{
		Configuration.ConfigurationData.ConfigFormArsenal config = Configuration.Config.FormArsenal;

		CompletionTime.BackColor = System.Drawing.Color.Transparent;
		ShipName.ForeColor = CompletionTime.ForeColor = Configuration.Config.UI.ForeColor;
		ShipName.MaximumWidth = config.MaxShipNameWidth;
	}
}
