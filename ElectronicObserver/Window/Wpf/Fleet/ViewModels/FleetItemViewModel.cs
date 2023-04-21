using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Attacks.Specials;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class FleetItemViewModel : ObservableObject
{
	private ColorService ColorService { get; }
	public FormFleetTranslationViewModel FormFleet { get; }
	private FleetViewModel Parent { get; }
	private bool Visible { get; set; }

	public Visibility Visibility => Visible.ToVisibility();

	public FleetItemControlViewModel Name { get; }
	public FleetLevelViewModel Level { get; }
	public FleetHpViewModel HP { get; }
	public FleetConditionViewModel Condition { get; }
	public ShipResourceViewModel ShipResource { get; }
	public EquipmentItemViewModel Equipments { get; }

	public Dictionary<SpecialAttack, List<SpecialAttackHit>> SpecialAttackHitList { get; set; } = new();

	public FleetItemViewModel(FleetViewModel parent)
	{
		ColorService = Ioc.Default.GetRequiredService<ColorService>();
		FormFleet = Ioc.Default.GetRequiredService<FormFleetTranslationViewModel>();

		Parent = parent;

		Name = new() { Text = "*nothing*" };
		// Name.SuspendLayout();
		// Name.Anchor = AnchorStyles.Left;
		// Name.TextAlign = ContentAlignment.MiddleLeft;
		// Name.ImageAlign = ContentAlignment.MiddleCenter;
		// Name.ForeColor = parent.MainFontColor;
		// Name.Padding = new Padding(2, 1, 2, 1);
		// Name.Margin = new Padding(2, 1, 2, 1);
		// Name.AutoSize = true;
		// //Name.AutoEllipsis = true;
		// Name.Visible = false;
		// Name.Cursor = Cursors.Help;
		// Name.MouseDown += Name_MouseDown;
		// Name.ResumeLayout();

		Level = new() { Value = 0, MaximumValue = ExpTable.ShipMaximumLevel, ValueNext = 0 };
		// Level.SuspendLayout();
		// Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
		// Level.MainFontColor = parent.MainFontColor;
		Level.SubFontColor = Utility.Configuration.Config.UI.SubForeColor;
		// //Level.TextNext = "n.";
		// Level.Padding = new Padding(0, 0, 0, 0);
		// Level.Margin = new Padding(2, 0, 2, 1);
		// Level.AutoSize = true;
		// Level.Visible = false;
		// Level.Cursor = Cursors.Help;
		// Level.MouseDown += Level_MouseDown;
		// Level.ResumeLayout();

		HP = new() { UsePrevValue = false, };
		// HP.SuspendUpdate();
		// HP.Anchor = AnchorStyles.Left;
		// HP.MaximumDigit = 999;
		// HP.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
		// HP.SubFontColor = Utility.Configuration.Config.UI.SubForeColor;
		// HP.Padding = new Padding(0, 0, 0, 0);
		// HP.Margin = new Padding(2, 1, 2, 2);
		// HP.AutoSize = true;
		// HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		// HP.Visible = false;
		// HP.ResumeUpdate();

		Condition = new() { Text = "*", };
		// Condition.SuspendLayout();
		// Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
		// Condition.ForeColor = parent.MainFontColor;
		// Condition.TextAlign = ContentAlignment.BottomRight;
		// Condition.ImageAlign = ContentAlignment.MiddleLeft;
		// Condition.ImageList = ResourceManager.Instance.Icons;
		// Condition.Padding = new Padding(2, 1, 2, 1);
		// Condition.Margin = new Padding(2, 1, 2, 1);
		// Condition.Size = new Size(40, 20);
		// Condition.AutoSize = true;
		// Condition.Visible = false;
		// Condition.ResumeLayout();

		ShipResource = new();
		// ShipResource.SuspendLayout();
		// ShipResource.Anchor = AnchorStyles.Left;
		// ShipResource.Padding = new Padding(0, 2, 0, 0);
		// ShipResource.Margin = new Padding(2, 0, 2, 1);
		// ShipResource.Size = new Size(30, 20);
		// ShipResource.AutoSize = false;
		// ShipResource.Visible = false;
		// ShipResource.ResumeLayout();

		Equipments = new();
		// Equipments.SuspendUpdate();
		// Equipments.Anchor = AnchorStyles.Left;
		// Equipments.Padding = new Padding(0, 1, 0, 1);
		// Equipments.Margin = new Padding(2, 0, 2, 1);
		// Equipments.Size = new Size(40, 20);
		// Equipments.AutoSize = true;
		// Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		// Equipments.Visible = false;
		// Equipments.ResumeUpdate();

		ConfigurationChanged();

		// ToolTipInfo = parent.ToolTipInfo;
		// Parent = parent;
	}


	public void ConfigurationChanged()
	{
		// Name.Font = parent.MainFont;
		// Level.MainFont = parent.MainFont;
		Level.SubFont = Configuration.Config.UI.SubFont;
		// HP.MainFont = parent.MainFont;
		HP.SubFont = Configuration.Config.UI.SubFont;
		// Condition.Font = parent.MainFont;
		Condition.SetDesign(Condition.Tag ?? 49);
		Equipments.Font = Configuration.Config.UI.SubFont;
	}

	public ShipData? Ship { get; set; }

	public string? ShipName => Ship switch
	{
		{ } ship => Ship.SallyArea switch
		{
			> 0 => $"[{ship.SallyArea}] {ship.MasterShip.ShipTypeName} {Ship.NameWithLevel}",
			_ => $"{ship.MasterShip.ShipTypeName} {Ship.NameWithLevel}",
		},

		null => null,
	};

	public string EquipmentAccuracy => Ship switch
	{
		null => "",
		_ => Ship.AllSlotInstance.Sum(eq => eq?.MasterEquipment.Accuracy ?? 0).ToString("+#;-#;+0"),
	};

	public string EquipmentBomber => Ship switch
	{
		null => "",
		_ => Ship.AllSlotInstance.Sum(eq => eq?.MasterEquipment.Bomber ?? 0).ToString("+#;-#;+0"),
	};

	public void Update(int shipMasterID)
	{
		KCDatabase db = KCDatabase.Instance;
		Ship = db.Ships[shipMasterID];

		UpdateShip(shipMasterID);

		Visible = shipMasterID != -1;
	}

	private void UpdateShip(int shipMasterID)
	{
		if (Ship is null)
		{
			Name.Tag = -1;
			return;
		}

		KCDatabase db = KCDatabase.Instance;

		bool isEscaped = db.Fleet[Parent.FleetId].EscapedShipList.Contains(shipMasterID);
		var equipments = Ship.AllSlotInstance.Where(eq => eq != null);

		UpdateShipName(equipments);

		UpdateLevel();

		UpdateHealth(isEscaped);

		UpdateCondition();

		ShipResource.SetResources(Ship.Fuel, Ship.FuelMax, Ship.Ammo, Ship.AmmoMax);
		ShipResource.IsEscaped = isEscaped;

		Equipments.SetSlotList(Ship);
		Equipments.ToolTip = GetEquipmentString(Ship);
	}

	private void UpdateCondition()
	{
		if (Ship is null) return;
			
		Condition.Text = Ship.Condition.ToString();
		Condition.Tag = Ship.Condition;
		Condition.SetDesign(Ship.Condition);

		if (Ship.Condition < 49)
		{
			TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - Ship.Condition) / 3.0) * 3, 0);
			Condition.ToolTip = string.Format(GeneralRes.FatigueRestoreTime, (int)ts.TotalMinutes, (int)ts.Seconds);
		}
		else
		{
			Condition.ToolTip = string.Format(GeneralRes.RemainingExpeds, (int)Math.Ceiling((Ship.Condition - 49) / 3.0));
		}
	}

	private void UpdateHealth(bool isEscaped)
	{
		KCDatabase db = KCDatabase.Instance;

		if (Ship is null) return;

		HP.HPBar.Value = HP.PrevValue = Ship.HPCurrent;
		HP.HPBar.MaximumValue = Ship.HPMax;
		HP.UsePrevValue = false;
		HP.ShowDifference = false;

		int dockID = Ship.RepairingDockID;

		if (dockID != -1)
		{
			HP.RepairTime = db.Docks[dockID].CompletionTime;
			HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
		}
		else
		{
			HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
		}

		HP.Tag = (Ship.RepairingDockID == -1 && 0.5 < Ship.HPRate && Ship.HPRate < 1.0) ? DateTimeHelper.FromAPITimeSpan(Ship.RepairTime).TotalSeconds : 0.0;
		HP.BackColor = isEscaped switch
		{
			true => Utility.Configuration.Config.UI.SubBackColor,
			_ => System.Drawing.Color.Transparent
		};


		StringBuilder sb = new StringBuilder();
		double hprate = (double)Ship.HPCurrent / Ship.HPMax;

		sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
		if (isEscaped)
		{
			sb.AppendLine(GeneralRes.Retreating);
		}
		else if (hprate > 0.50)
		{
			sb.AppendFormat(GeneralRes.ToMidAndHeavy + "\n", Ship.HPCurrent - Ship.HPMax / 2, Ship.HPCurrent - Ship.HPMax / 4);
		}
		else if (hprate > 0.25)
		{
			sb.AppendFormat(GeneralRes.ToHeavy + "\n", Ship.HPCurrent - Ship.HPMax / 4);
		}
		else
		{
			sb.AppendLine(GeneralRes.IsTaiha);
		}

		if (Ship.RepairTime > 0)
		{
			var span = DateTimeHelper.FromAPITimeSpan(Ship.RepairTime);
			sb.AppendFormat(GeneralRes.DockTime + ": {0} @ {1}",
				DateTimeHelper.ToTimeRemainString(span),
				DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(Ship)));
		}

		HP.ToolTip = sb.ToString();
	}

	private void UpdateLevel()
	{
		if (Ship is null) return;

		KCDatabase db = KCDatabase.Instance;

		Level.Value = Ship.Level;
		Level.ValueNext = Ship.ExpNext;
		Level.Tag = Ship.MasterID;

		Level.UpdateColors();

		StringBuilder tip = new StringBuilder();
		tip.AppendFormat("Total: {0:N0} exp.\r\n", Ship.ExpTotal);

		if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
			tip.AppendFormat(GeneralRes.ToNextLevel + " exp.\r\n", Ship.ExpNext.ToString("N0"));

		List<(string Name, int Level)> remodels = db.MasterShips.Values
			.Where(s => s.BaseShip() == Ship.MasterShip.BaseShip())
			.Where(s => s.RemodelTier > Ship.MasterShip.RemodelTier)
			.OrderBy(s => s.RemodelBeforeShip?.RemodelAfterLevel ?? 0)
			.Select(s => (s.NameEN, s.RemodelBeforeShip?.RemodelAfterLevel ?? 0))
			.ToList();

		foreach ((string name, int remodelLevel) in remodels)
		{
			int neededExp = Math.Max(ExpTable.GetExpToLevelShip(Ship.ExpTotal, remodelLevel), 0);
			tip.Append($"{name}({remodelLevel}): {neededExp:N0} exp.\r\n");
		}

		IEnumerable<int> remodelLevels = remodels.Select((remodel) => remodel.Level);
		List<ShipTrainingPlanViewModel> plans = Level.TrainingPlans
			.Where(p => !remodelLevels.Contains(p.TargetLevel))
			.DistinctBy(p => p.TargetLevel)
			.ToList();

		foreach(ShipTrainingPlanViewModel plan in plans.Where(p => p.TargetLevel < 99))
		{
			tip.AppendFormat(GeneralRes.ToX + " exp.\r\n", plan.TargetLevel, plan.RemainingExpToTarget.ToString("N0"));
		}

		if (Ship.Level < 99)
		{
			string lv99Exp = Math.Max(ExpTable.GetExpToLevelShip(Ship.ExpTotal, 99), 0).ToString("N0");
			tip.AppendFormat(GeneralRes.To99 + " exp.\r\n", lv99Exp);
		}

		foreach (ShipTrainingPlanViewModel plan in plans.Where(p => p.TargetLevel > 99 && p.TargetLevel != ExpTable.ShipMaximumLevel))
		{
			tip.AppendFormat(GeneralRes.ToX + " exp.\r\n", plan.TargetLevel, plan.RemainingExpToTarget.ToString("N0"));
		}

		if (Ship.Level < ExpTable.ShipMaximumLevel)
		{
			string lv175Exp = Math
				.Max(ExpTable.GetExpToLevelShip(Ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)
				.ToString("N0");
			tip.AppendFormat(GeneralRes.ToX + " exp.\r\n", ExpTable.ShipMaximumLevel, lv175Exp);
		}

		Level.ToolTip = tip.ToString();
	}

	private void UpdateShipName(IEnumerable<IEquipmentData> equipments)
	{
		if (Ship is null) return;

		Name.Text = Ship.MasterShip.NameWithClass;
		Name.Tag = Ship.ShipID;
		Name.ToolTip = string.Format(
			FormFleet.ShipNameToolTip,
			Ship.SallyArea > 0 ? $"[{Ship.SallyArea}] " : "",
			Ship.MasterShip.ShipTypeName, Ship.NameWithLevel,
			Ship.FirepowerBase, Ship.FirepowerTotal,
			Ship.TorpedoBase, Ship.TorpedoTotal,
			Ship.AABase, Ship.AATotal,
			Ship.ArmorBase, Ship.ArmorTotal,
			Ship.ASWBase, Ship.ASWTotal,
			Ship.EvasionBase, Ship.EvasionTotal,
			Ship.LOSBase, Ship.LOSTotal,
			Ship.LuckTotal,
			equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Accuracy) : 0,
			equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Bomber) : 0,
			Constants.GetRange(Ship.Range),
			Constants.GetSpeed(Ship.Speed)
		);

		Name.BackColor = GetShipBackColor();
		Name.ForeColor = GetShipForeColor(Name.BackColor);
	}

	private Color GetShipBackColor()
	{
		if (Configuration.Config.FormFleet.AppliesSallyAreaColor &&
			Parent.ShipTagColors.Count > 0 &&
			Ship?.SallyArea > 0)
		{
			return Parent.ShipTagColors[Math.Min(Ship.SallyArea, Parent.ShipTagColors.Count - 1)].ToWpfColor();
		}

		return Colors.Transparent;
	}

	private Color GetShipForeColor(Color backColor)
	{
		if (Configuration.Config.FormFleet.AppliesSallyAreaColor &&
		    Parent.ShipTagColors.Count > 0 &&
		    Ship?.SallyArea > 0)
		{
			return ColorService.GetForegroundColor(backColor);
		}

		return Configuration.Config.UI.ForeColor.ToWpfColor();
	}

	private string AttackRateDisplay(double rate) => rate switch
	{
		0 => "???",
		_ => $"{rate:P1}",
	};

	private string GetEquipmentString(IShipData ship)
	{
		StringBuilder sb = new StringBuilder();

		sb.AppendFormat(ship.NameWithLevel + "\r\n");

		for (int i = 0; i < ship.Slot.Count; i++)
		{
			var eq = ship.SlotInstance[i];
			if (eq != null)
				sb.AppendFormat("[{0}/{1}] {2}\r\n", ship.Aircraft[i], ship.MasterShip.Aircraft[i], eq.NameWithLevel);
		}

		{
			var exslot = ship.ExpansionSlotInstance;
			if (exslot != null)
				sb.AppendFormat(GeneralRes.Expansion + ": {0}\r\n", exslot.NameWithLevel);
		}

		int[] slotmaster = ship.AllSlotMaster.ToArray();

		EngagementType engagement = (EngagementType)Utility.Configuration.Config.Control.PowerEngagementForm;
		IFleetData fleet = KCDatabase.Instance.Fleet[Parent.FleetId];

		if (SpecialAttackHitList.Any())
		{
			AddDaySpecialAttacksToTooltip(ship, sb, engagement, fleet);
		}

		List<Enum> dayAttacks = ship.GetDayAttacks().ToList();

		if (dayAttacks.Any())
		{
			sb.AppendFormat($"\r\n{GeneralRes.DayBattle}:");
			List<double> asRates = dayAttacks.Select(a => ship.GetDayAttackRate(a, fleet, AirState.Superiority))
				.ToList().TotalRates();
			List<double> asPlusRates =
				dayAttacks.Select(a => ship.GetDayAttackRate(a, fleet)).ToList().TotalRates();

			foreach ((Enum attack, double asRate, double asPlusRate) in dayAttacks
				.Zip(asRates, (attack, rate) => (attack, rate))
				.Zip(asPlusRates, (ar, asPlus) => (ar.attack, ar.rate, asPlus)))
			{
				double power = ship.GetDayAttackPower(attack, fleet, engagement);
				double accuracy = ship.GetDayAttackAccuracy(attack, fleet);
				string attackDisplay = attack switch
				{
					DayAttackKind dayAttack => DayAttack.AttackDisplay(dayAttack),
					DayAirAttackCutinKind cvci => cvci switch
					{
						DayAirAttackCutinKind.FighterBomberAttacker => FormFleet.CvciFba,
						DayAirAttackCutinKind.BomberBomberAttacker => FormFleet.CvciBba,
						DayAirAttackCutinKind.BomberAttacker => FormFleet.CvciBa,
						_ => "?"
					},
					_ => $"{attack}"
				};
				sb.AppendFormat($"\r\n・[{asRate:P1} | {asPlusRate:P1}] - " +
								$"{attackDisplay} - " +
								$"{FormFleet.Power}: {power} - " +
								$"{FormFleet.Accuracy}: {accuracy:0.##}");
			}
		}

		if (SpecialAttackHitList.Any())
		{
			AddNightSpecialAttacksToTooltip(ship, sb, engagement, fleet);
		}

		List<NightAttack> nightAttacks = ship.GetNightAttacks().ToList();
		List<double> nightAttackRates = nightAttacks.Select(a => ship.GetNightAttackRate(a, fleet))
			.ToList().TotalRates();

		if (nightAttacks.Any())
		{
			sb.AppendFormat($"\r\n{GeneralRes.NightBattle}:");

			foreach ((NightAttack attack, double rate) in nightAttacks.Zip(nightAttackRates, (attack, rate) => (attack, rate)))
			{
				double power = ship.GetNightAttackPower(attack, fleet);
				double accuracy = ship.GetNightAttackAccuracy(attack, fleet);
				string attackDisplay = attack.Display;

				sb.AppendFormat($"\r\n・[{AttackRateDisplay(rate)}] - " +
								$"{attackDisplay} - " +
								$"{FormFleet.Power}: {power} - " +
								$"{FormFleet.Accuracy}: {accuracy:0.##}");
			}
		}

		sb.AppendLine();

		sb.AppendLine($"{ConstantsRes.ShellingSupport}: " +
					  $"{FormFleet.Power}: {ship.GetShellingSupportDamage(engagement)} - " +
					  $"{FormFleet.Accuracy}: {ship.GetShellingSupportAccuracy():0.##}");

		{
			int torpedo = ship.TorpedoPower;
			int asw = ship.AntiSubmarinePower;

			DayAttackKind aswAttack = Calculator.GetDayAttackKind(ship.AllSlotMaster.ToArray(), ship.ShipID, 126, false);
			int asw2 = ship.GetAswAttackPower(aswAttack, fleet);

			if (torpedo > 0)
			{
				sb.Append($"{ConstantsRes.TorpedoAttack}: " +
						  $"{FormFleet.Power}: {torpedo} - " +
						  $"{FormFleet.Accuracy}: {ship.GetTorpedoAttackAccuracy(fleet):0.##}");
			}
			if (asw > 0)
			{
				if (torpedo > 0)
					sb.AppendLine();

				sb.AppendFormat($"{FormFleet.Asw}: {FormFleet.Power}: {asw2}");

				sb.Append($" (x{ship.AswMod():0.##})");

				if (ship.CanOpeningASW)
					sb.Append(FormFleet.OpeningAsw);

				sb.Append($" - {FormFleet.Accuracy}: {ship.GetAswAttackAccuracy(fleet):0.##}");
			}
			if (torpedo > 0 || asw > 0)
				sb.AppendLine();
		}

		{
			sb.Append($"{GeneralRes.Evasion}: ");

			if (ship.MasterShip.IsSubmarine)
			{
				sb.Append($"{GeneralRes.ASW}: {ship.AswEvasion():N0}");
			}
			else
			{
				sb.Append
				(
					$"{ConstantsRes.Shelling}: {ship.ShellingEvasion():N0} - " +
					$"{ConstantsRes.TorpedoAttack}: {ship.TorpedoEvasion():N0} - " +
					$"{ConstantsRes.AirBattle}: {ship.AirstrikeEvasion():N0} - " +
					$"{ConstantsRes.NightBattle}: {ship.NightEvasion():N0}"
				);
			}

			sb.AppendLine();
		}

		{
			List<AntiAirCutIn> cutIns = AntiAirCutIn.PossibleCutIns(ship)
				.Where(a => a.Id is not 0)
				.ToList();

			if (cutIns.Any())
			{
				sb.AppendLine($"{GeneralRes.AACutIn}:");
				foreach (AntiAirCutIn cutIn in cutIns)
				{
					sb.AppendFormat("・{0} {1}\r\n", cutIn.Id, cutIn.ValueDisplay);
				}
			}
			double adjustedaa = Calculator.GetAdjustedAAValue(ship);
			sb.AppendFormat(GeneralRes.ShipAADefense + "\r\n",
				adjustedaa,
				Calculator.GetProportionalAirDefense(adjustedaa)
			);

			double aarbRate = Calculator.GetAarbRate(ship, adjustedaa);
			if (aarbRate > 0)
			{
				sb.Append($"{FormFleet.Aarb}: {aarbRate:p1}\r\n");
			}

		}

		{
			int airsup_min;
			int airsup_max;
			if (Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1)
			{
				airsup_min = Calculator.GetAirSuperiority(ship, false);
				airsup_max = Calculator.GetAirSuperiority(ship, true);
			}
			else
			{
				airsup_min = airsup_max = Calculator.GetAirSuperiorityIgnoreLevel(ship);
			}

			int airbattle = ship.AirBattlePower;
			if (airsup_min > 0)
			{

				string airsup_str;
				if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airsup_min < airsup_max)
				{
					airsup_str = string.Format("{0} ～ {1}", airsup_min, airsup_max);
				}
				else
				{
					airsup_str = airsup_min.ToString();
				}

				if (airbattle > 0)
					sb.Append($"{GeneralRes.AirPower}: {airsup_str} / {FormFleet.AirstrikePower}: {airbattle}\r\n");
				else
					sb.Append($"{GeneralRes.AirPower}: {airsup_str}\r\n");
			}
			else if (airbattle > 0)
				sb.Append($"{FormFleet.AirstrikePower}: {airbattle}\r\n");
		}

		return sb.ToString();
	}

	private void AddDaySpecialAttacksToTooltip(IShipData ship, StringBuilder sb, EngagementType engagement, IFleetData fleet)
	{
		if (!SpecialAttackHitList.Any(specialAttack => specialAttack.Key.CanTriggerOnDay)) return;

		sb.Append($"\r\n{FormFleet.SpecialAttacksDay}");

		foreach ((SpecialAttack attack, List<SpecialAttackHit> hits) in SpecialAttackHitList.Where(specialAttack => specialAttack.Key.CanTriggerOnDay))
		{
			foreach (SpecialAttackHit hit in hits.Distinct())
			{
				double power = ship.GetDayAttackPower(attack, hit, fleet, engagement);
				double accuracy = ship.GetDayAttackAccuracy(hit, fleet);

				AddSpecialAttackToTooltip(sb, attack, hit, power, accuracy);
			}
		}
	}

	private void AddNightSpecialAttacksToTooltip(IShipData ship, StringBuilder sb, EngagementType engagement, IFleetData fleet)
	{
		if (!SpecialAttackHitList.Any(specialAttack => specialAttack.Key.CanTriggerOnNight)) return;

		sb.Append($"\r\n{FormFleet.SpecialAttacksNight}");

		foreach ((SpecialAttack attack, List<SpecialAttackHit> hits) in SpecialAttackHitList.Where(specialAttack => specialAttack.Key.CanTriggerOnNight))
		{
			foreach (SpecialAttackHit hit in hits.Distinct())
			{
				double power = ship.GetNightAttackPower(attack, hit, fleet, engagement);
				double accuracy = ship.GetNightAttackAccuracy(hit, fleet);

				AddSpecialAttackToTooltip(sb, attack, hit, power, accuracy);
			}
		}
	}

	private void AddSpecialAttackToTooltip(StringBuilder sb, SpecialAttack attack, SpecialAttackHit hit, double power, double accuracy)
	{
		string attackDisplay = attack.GetDisplay();

		sb.Append("\r\n");

		if (hit.ShipIndex == 0)
		{
			sb.Append($"・[{AttackRateDisplay(attack.GetTriggerRate())}] - {attackDisplay}");
		}
		else
		{
			sb.Append($"・{attackDisplay}");
		}

		if (hit.PowerModifier > 0)
		{
			sb.Append($" - {FormFleet.Power}: {power} - {FormFleet.Accuracy}: {accuracy:0.##}");
		}
	}
}
