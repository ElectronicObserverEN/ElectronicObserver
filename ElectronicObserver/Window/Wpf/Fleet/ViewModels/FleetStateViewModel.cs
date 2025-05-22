using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class FleetStateViewModel : ObservableObject
{
	public FormFleetTranslationViewModel FormFleet { get; }
	public ObservableCollection<StateLabel> StateLabels { get; }

	public FleetStateViewModel()
	{
		FormFleet = Ioc.Default.GetService<FormFleetTranslationViewModel>()!;

		StateLabels = new();
	}

	private StateLabel AddStateLabel()
	{
		StateLabels.Add(new());
		var ret = StateLabels.Last();
		// LayoutBase.Controls.Add(ret.Label);
		return ret;
	}

	private StateLabel GetStateLabel(int index)
	{
		if (index >= StateLabels.Count)
		{
			for (int i = StateLabels.Count; i <= index; i++)
				AddStateLabel();
		}
		StateLabels[index].Enabled = true;
		return StateLabels[index];
	}

	public void UpdateFleetState(FleetData fleet)
	{
		KCDatabase db = KCDatabase.Instance;

		int index = 0;


		bool emphasizesSubFleetInPort = Utility.Configuration.Config.FormFleet.EmphasizesSubFleetInPort &&
										(db.Fleet.CombinedFlag > 0 ? fleet.FleetID >= 3 : fleet.FleetID >= 2);
		var displayMode = (FleetStateDisplayModes)Utility.Configuration.Config.FormFleet.FleetStateDisplayMode;

		Color colorDangerFG = Utility.Configuration.Config.UI.FleetOverview_ShipDamagedFG.ToWpfColor();
		Color colorDangerBG = Utility.Configuration.Config.UI.FleetOverview_ShipDamagedBG.ToWpfColor();

		Color colorInPortFG = Utility.Configuration.Config.UI.ForeColor.ToWpfColor();
		Color colorInPortBG = Colors.Transparent;

		Color colorNotExpeditionFG = Utility.Configuration.Config.UI.FleetOverview_AlertNotInExpeditionFG.ToWpfColor();
		Color colorNotExpeditionBG = Utility.Configuration.Config.UI.FleetOverview_AlertNotInExpeditionBG.ToWpfColor();

		Color colorInExpeditionFG = Utility.Configuration.Config.UI.ForeColor.ToWpfColor();
		Color colorInExpeditionBG = Colors.Transparent;

		//所属艦なし
		if (fleet == null || fleet.Members.All(id => id == -1))
		{
			var state = GetStateLabel(index);

			state.SetInformation(FleetStates.NoShip, FormFleet.NoShips, "", IconContent.FleetNoShip);
			state.Label.ToolTip = null;

			emphasizesSubFleetInPort = false;
			index++;

		}
		else
		{

			if (fleet.IsInSortie)
			{

				//大破出撃中
				if (fleet.MembersWithoutEscaped.Any(s => s is { HPRate: <= 0.25 }))
				{
					var state = GetStateLabel(index);

					state.SetInformation(FleetStates.SortieDamaged, FormFleet.CriticalDamageAdvance, FormFleet.CriticalDamageAdvance, IconContent.FleetSortieDamaged, colorDangerBG, colorDangerFG);
					state.Label.ToolTip = null;

					index++;

				}
				else
				{   //出撃中
					var state = GetStateLabel(index);

					state.SetInformation(FleetStates.Sortie, FormFleet.OnSortie, "", IconContent.FleetSortie);
					state.Label.ToolTip = null;

					index++;
				}

				emphasizesSubFleetInPort = false;
			}

			//遠征中
			if (fleet.ExpeditionState != 0)
			{
				var state = GetStateLabel(index);
				var dest = db.Mission[fleet.ExpeditionDestination];

				state.Timer = fleet.ExpeditionTime;
				state.SetInformation(FleetStates.Expedition,
					$"[{dest.DisplayID}] {DateTimeHelper.ToTimeRemainString(state.Timer)}",
					DateTimeHelper.ToTimeRemainString(state.Timer),
					IconContent.FleetExpedition, colorInExpeditionBG, colorInExpeditionFG);

				state.Label.ToolTip = string.Format(FormFleet.ExpeditionToolTip,
					dest.DisplayID, dest.NameEN, DateTimeHelper.TimeToCSVString(state.Timer));

				emphasizesSubFleetInPort = false;
				index++;
			}

			//大破艦あり
			if (!fleet.IsInSortie && fleet.MembersWithoutEscaped!.Any(s => s != null && s.CanSink(fleet)))
			{
				var state = GetStateLabel(index);

				state.SetInformation(FleetStates.Damaged, FormFleet.CriticallyDamagedShip, FormFleet.CriticallyDamagedShip, IconContent.FleetDamaged, colorDangerBG, colorDangerFG);
				state.Label.ToolTip = null;

				emphasizesSubFleetInPort = false;
				index++;
			}

			//泊地修理中
			if (fleet.CanAnchorageRepair)
			{
				var state = GetStateLabel(index);

				state.Timer = db.Fleet.AnchorageRepairingTimer;
				state.SetInformation(FleetStates.AnchorageRepairing,
					FormFleet.Repairing + DateTimeHelper.ToTimeElapsedString(state.Timer),
					DateTimeHelper.ToTimeElapsedString(state.Timer),
					IconContent.FleetAnchorageRepairing);


				StringBuilder sb = new StringBuilder();
				sb.AppendFormat(FormFleet.RepairTimeHeader,
					DateTimeHelper.TimeToCSVString(db.Fleet.AnchorageRepairingTimer));

				for (int i = 0; i < fleet.Members.Count; i++)
				{
					var ship = fleet.MembersInstance[i];
					if (ship != null && ship.HPRate < 1.0)
					{
						var totaltime = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
						var unittime = Calculator.CalculateDockingUnitTime(ship);
						sb.AppendFormat(FormFleet.RepairTimeDetail,
							i + 1,
							DateTimeHelper.ToTimeRemainString(totaltime),
							DateTimeHelper.ToTimeRemainString(unittime),
							ship.HPMax - ship.HPCurrent
						);
					}
					else
					{
						sb.Append("#").Append(i + 1).Append(" : ----\r\n");
					}
				}

				state.Label.ToolTip = sb.ToString();

				emphasizesSubFleetInPort = false;
				index++;
			}

			//入渠中
			{
				long ntime = db.Docks.Values.Where(d => d.State == 1 && fleet.Members.Contains(d.ShipID)).Select(d => d.CompletionTime.Ticks).DefaultIfEmpty().Max();

				if (ntime > 0)
				{   //入渠中
					var state = GetStateLabel(index);

					state.Timer = new DateTime(ntime);
					state.SetInformation(FleetStates.Docking,
						FormFleet.OnDock + DateTimeHelper.ToTimeRemainString(state.Timer),
						DateTimeHelper.ToTimeRemainString(state.Timer),
						IconContent.FleetDocking);

					state.Label.ToolTip = FormFleet.DockCompletionTime + DateTimeHelper.TimeToCSVString(state.Timer);

					emphasizesSubFleetInPort = false;
					index++;
				}

			}

			//未補給
			{
				var members = fleet.MembersInstance.Where(s => s != null);

				int fuel = members.Sum(ship => ship.SupplyFuel);
				int ammo = members.Sum(ship => ship.SupplyAmmo);
				int aircraft = members.SelectMany(s => s.MasterShip.Aircraft.Zip(s.Aircraft, (max, now) => max - now)).Sum();
				int bauxite = aircraft * 5;

				if (fuel > 0 || ammo > 0 || bauxite > 0)
				{
					var state = GetStateLabel(index);

					state.SetInformation(FleetStates.NotReplenished, FormFleet.SupplyNeeded, "", IconContent.FleetNotReplenished, colorInPortBG, colorInPortFG);
					state.Label.ToolTip = string.Format(FormFleet.ResupplyTooltip, fuel, ammo, bauxite, aircraft);

					index++;
				}
			}

			//疲労
			{
				int cond = fleet.MembersInstance.Min(s => s?.Condition ?? 100);

				if (cond < Utility.Configuration.Config.Control.ConditionBorder && fleet.ConditionTime != null && fleet.ExpeditionState == 0)
				{
					StateLabel state = GetStateLabel(index);

					IconContent icon = cond switch
					{
						< 20 => IconContent.ConditionVeryTired,
						< 30 => IconContent.ConditionTired,
						_ => IconContent.ConditionLittleTired,
					};

					state.Timer = (DateTime)fleet.ConditionTime;
					state.SetInformation(FleetStates.Tired,
						FormFleet.Fatigued + DateTimeHelper.ToTimeRemainString(state.Timer),
						DateTimeHelper.ToTimeRemainString(state.Timer),
						icon,
						colorInPortBG);

					state.Label.ToolTip = string.Format(FormFleet.RecoveryTimeToolTip,
						DateTimeHelper.TimeToCSVString(state.Timer), DateTimeHelper.ToTimeRemainString(TimeSpan.FromSeconds(db.Fleet.ConditionBorderAccuracy)));

					index++;

				}
				else if (cond >= 50)
				{
					//戦意高揚
					StateLabel state = GetStateLabel(index);

					state.SetInformation(FleetStates.Sparkled, FormFleet.FightingSpiritHigh, "", IconContent.ConditionSparkle, colorInPortBG);
					state.Label.ToolTip = string.Format(FormFleet.SparkledTooltip, cond, Math.Ceiling((cond - 49) / 3.0));

					index++;
				}
			}

			//出撃可能！
			if (index == 0)
			{
				var state = GetStateLabel(index);

				state.SetInformation(FleetStates.Ready, FormFleet.ReadyToSortie, "", IconContent.FleetReady, colorInPortBG);
				state.Label.ToolTip = null;

				index++;
			}

		}


		if (emphasizesSubFleetInPort)
		{
			for (int i = 0; i < index; i++)
			{
				if (StateLabels[i].Label.BackColor == Colors.Transparent)
				{
					StateLabels[i].Label.BackColor = colorNotExpeditionBG;
					StateLabels[i].Label.ForeColor = colorNotExpeditionFG;
				}
			}
		}


		for (int i = displayMode == FleetStateDisplayModes.Single ? 1 : index; i < StateLabels.Count; i++)
			StateLabels[i].Enabled = false;


		switch (displayMode)
		{

			case FleetStateDisplayModes.AllCollapsed:
				for (int i = 0; i < index; i++)
					StateLabels[i].AutoShorten = true;
				break;

			case FleetStateDisplayModes.MultiCollapsed:
				if (index == 1)
				{
					StateLabels[0].AutoShorten = false;
				}
				else
				{
					for (int i = 0; i < index; i++)
						StateLabels[i].AutoShorten = true;
				}
				break;

			case FleetStateDisplayModes.Single:
			case FleetStateDisplayModes.AllExpanded:
				for (int i = 0; i < index; i++)
					StateLabels[i].AutoShorten = false;
				break;
		}
	}


	public void RefreshFleetState()
	{
		Color foreColor = Utility.Configuration.Config.UI.ForeColor.ToWpfColor();
		Color backColor = Colors.Transparent;

		Color shipDamagedFG = Utility.Configuration.Config.UI.FleetOverview_ShipDamagedFG.ToWpfColor();
		Color shipDamagedBG = Utility.Configuration.Config.UI.FleetOverview_ShipDamagedBG.ToWpfColor();

		Color expeditionOverFG = Utility.Configuration.Config.UI.FleetOverview_ExpeditionOverFG.ToWpfColor();
		Color expeditionOverBG = Utility.Configuration.Config.UI.FleetOverview_ExpeditionOverBG.ToWpfColor();

		Color repairFinishedFG = Utility.Configuration.Config.UI.Dock_RepairFinishedFG.ToWpfColor();
		Color repairFinishedBG = Utility.Configuration.Config.UI.Dock_RepairFinishedBG.ToWpfColor();

		Color tiredRecoveredFG = Utility.Configuration.Config.UI.FleetOverview_TiredRecoveredFG.ToWpfColor();
		Color tiredRecoveredBG = Utility.Configuration.Config.UI.FleetOverview_TiredRecoveredBG.ToWpfColor();

		foreach (var state in StateLabels)
		{
			if (!state.Enabled) continue;

			switch (state.State)
			{

				case FleetStates.Damaged:
					if (Utility.Configuration.Config.FormFleet.BlinkAtDamaged)
					{
						state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? shipDamagedFG : foreColor;
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? shipDamagedBG : backColor;
					}
					break;

				case FleetStates.SortieDamaged:
					state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? shipDamagedFG : foreColor;
					state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? shipDamagedBG : backColor;
					break;

				case FleetStates.Docking:
					state.ShortenedText = DateTimeHelper.ToTimeRemainString(state.Timer);
					state.Text = FormFleet.OnDock + state.ShortenedText;
					state.UpdateText();
					if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierRepair.AccelInterval)
					{
						state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? repairFinishedFG : foreColor;
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? repairFinishedBG : backColor;
					}
					break;

				case FleetStates.Expedition:
					state.ShortenedText = DateTimeHelper.ToTimeRemainString(state.Timer);
					state.Text = state.Text.Substring(0, 5) + DateTimeHelper.ToTimeRemainString(state.Timer);
					state.UpdateText();
					if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierExpedition.AccelInterval)
					{
						state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? expeditionOverFG : foreColor;
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? expeditionOverBG : backColor;
					}
					break;

				case FleetStates.Tired:
					state.ShortenedText = DateTimeHelper.ToTimeRemainString(state.Timer);
					state.Text = FormFleet.Fatigued + state.ShortenedText;
					state.UpdateText();
					if (Utility.Configuration.Config.FormFleet.BlinkAtCompletion && (state.Timer - DateTime.Now).TotalMilliseconds <= 0)
					{
						state.Label.ForeColor = DateTime.Now.Second % 2 == 0 ? tiredRecoveredFG : foreColor;
						state.Label.BackColor = DateTime.Now.Second % 2 == 0 ? tiredRecoveredBG : backColor;
					}
					break;

				case FleetStates.AnchorageRepairing:
					state.ShortenedText = DateTimeHelper.ToTimeElapsedString(KCDatabase.Instance.Fleet.AnchorageRepairingTimer);
					state.Text = FormFleet.Repairing + state.ShortenedText;
					state.UpdateText();
					break;

			}

		}
	}
	public IconContent GetIcon()
	{
		StateLabel? first = StateLabels
			.Where(s => s.Enabled)
			.MinBy(s => s.State);

		return first?.Label.Icon ?? IconContent.FormFleet;
	}
}
