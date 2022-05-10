using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using ElectronicObserverTypes;
using WeifenLuo.WinFormsUI.Docking;
using Translation = ElectronicObserver.Properties.Window.FormFleet;

namespace ElectronicObserver.Window;

public partial class FormFleet : DockContent
{

	private bool IsRemodeling = false;


	private class TableFleetControl : IDisposable
	{
		public Label Name;
		public FleetState State;
		public ImageLabel AirSuperiority;
		public ImageLabel SearchingAbility;
		public ImageLabel AntiAirPower;
		public ToolTip ToolTipInfo;

		public int BranchWeight { get; private set; } = 1;

		public TableFleetControl(FormFleet parent)
		{

			#region Initialize

			Name = new Label
			{
				Text = "[" + parent.FleetID.ToString() + "]",
				Anchor = AnchorStyles.Left,
				ForeColor = parent.MainFontColor,
				UseMnemonic = false,
				Padding = new Padding(0, 1, 0, 1),
				Margin = new Padding(2, 0, 2, 0),
				AutoSize = true,
				//Name.Visible = false;
				Cursor = Cursors.Help
			};

			State = new FleetState
			{
				Anchor = AnchorStyles.Left,
				ForeColor = parent.MainFontColor,
				Padding = new Padding(),
				Margin = new Padding(),
				AutoSize = true
			};

			AirSuperiority = new ImageLabel
			{
				Anchor = AnchorStyles.Left,
				ForeColor = parent.MainFontColor,
				ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedFighter,
				Padding = new Padding(2, 2, 2, 2),
				Margin = new Padding(2, 0, 2, 0),
				AutoSize = true
			};

			SearchingAbility = new ImageLabel
			{
				Anchor = AnchorStyles.Left,
				ForeColor = parent.MainFontColor,
				ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedRecon,
				Padding = new Padding(2, 2, 2, 2),
				Margin = new Padding(2, 0, 2, 0),
				AutoSize = true
			};
			SearchingAbility.Click += (sender, e) => SearchingAbility_Click(sender, e, parent.FleetID);

			AntiAirPower = new ImageLabel
			{
				Anchor = AnchorStyles.Left,
				ForeColor = parent.MainFontColor,
				ImageList = ResourceManager.Instance.Equipments,
				ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun,
				Padding = new Padding(2, 2, 2, 2),
				Margin = new Padding(2, 0, 2, 0),
				AutoSize = true
			};


			ConfigurationChanged(parent);

			ToolTipInfo = parent.ToolTipInfo;

			#endregion

		}

		public TableFleetControl(FormFleet parent, TableLayoutPanel table)
			: this(parent)
		{
			AddToTable(table);
		}

		public void AddToTable(TableLayoutPanel table)
		{

			table.SuspendLayout();
			table.Controls.Add(Name, 0, 0);
			table.Controls.Add(State, 1, 0);
			table.Controls.Add(AirSuperiority, 2, 0);
			table.Controls.Add(SearchingAbility, 3, 0);
			table.Controls.Add(AntiAirPower, 4, 0);
			table.ResumeLayout();

		}

		private void SearchingAbility_Click(object sender, EventArgs e, int fleetID)
		{
			BranchWeight--;
			if (BranchWeight <= 0)
				BranchWeight = 4;

			Update(KCDatabase.Instance.Fleet[fleetID]);
		}

		public void Update(FleetData fleet)
		{

			KCDatabase db = KCDatabase.Instance;

			if (fleet == null) return;



			Name.Text = fleet.Name;
			{
				var members = fleet.MembersInstance.Where(s => s != null);

				int levelSum = members.Sum(s => s.Level);

				int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
				int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

				int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
				int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

				int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

				string supporttype;
				switch (fleet.SupportType)
				{
					case 0:
					default:
						supporttype = Translation.SupportTypeNone; break;
					case 1:
						supporttype = Translation.SupportTypeAerial; break;
					case 2:
						supporttype = Translation.SupportTypeShelling; break;
					case 3:
						supporttype = Translation.SupportTypeTorpedo; break;
				}

				double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
				int tp = Calculator.GetTPDamage(fleet);

				// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
				var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
				var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));


				ToolTipInfo.SetToolTip(Name, string.Format(Translation.FleetNameToolTip,
					levelSum,
					(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
					Constants.GetSpeed(speed),
					supporttype,
					members.Sum(s => s.FirepowerTotal),
					members.Sum(s => s.TorpedoTotal),
					members.Sum(s => s.AATotal),
					members.Sum(s => s.ASWTotal),
					members.Sum(s => s.LOSTotal),
					transport.Sum(),
					transport.Count(i => i > 0),
					landing.Sum(),
					landing.Count(i => i > 0),
					expeditionBonus,
					tp,
					(int)(tp * 0.7),
					fueltotal,
					ammototal,
					fuelunit,
					ammounit
				));

			}


			State.UpdateFleetState(fleet, ToolTipInfo);


			//制空戦力計算
			{
				int airSuperiority = fleet.GetAirSuperiority();
				bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
				AirSuperiority.Text = fleet.GetAirSuperiorityString();
				ToolTipInfo.SetToolTip(AirSuperiority,
					string.Format(GeneralRes.ASTooltip,
						(int)(airSuperiority / 3.0),
						(int)(airSuperiority / 1.5),
						Math.Max((int)(airSuperiority * 1.5 - 1), 0),
						Math.Max((int)(airSuperiority * 3.0 - 1), 0),
						includeLevel ? Translation.WithoutProficiency : Translation.WithProficiency,
						includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet)));
			}


			//索敵能力計算
			SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
			{
				StringBuilder sb = new();
				double probStart = fleet.GetContactProbability();
				var probSelect = fleet.GetContactSelectionProbability();

				sb.AppendFormat(Translation.FleetLosToolTip,
					BranchWeight,
					probStart,
					probStart * 0.6);

				if (probSelect.Count > 0)
				{
					sb.AppendLine(Translation.ContactSelection);

					foreach ((int accuracy, double probability) in probSelect.OrderBy(p => p.Key))
					{
						sb.AppendFormat(Translation.ContactProbability + "\r\n", accuracy, probability);
					}
				}

				ToolTipInfo.SetToolTip(SearchingAbility, sb.ToString());
			}

			// 対空能力計算
			{
				var sb = new StringBuilder();
				double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

				AntiAirPower.Text = lineahead.ToString("0.0");

				sb.AppendFormat(GeneralRes.AntiAirPower,
					lineahead,
					Calculator.GetAdjustedFleetAAValue(fleet, 2),
					Calculator.GetAdjustedFleetAAValue(fleet, 3));

				ToolTipInfo.SetToolTip(AntiAirPower, sb.ToString());
			}
		}


		public void Refresh()
		{

			State.RefreshFleetState();

		}

		public void ConfigurationChanged(FormFleet parent)
		{
			Name.Font = parent.MainFont;
			State.Font = parent.MainFont;
			//State.BackColor = Color.Transparent;
			State.RefreshFleetState();
			AirSuperiority.Font = parent.MainFont;
			SearchingAbility.Font = parent.MainFont;
			AntiAirPower.Font = parent.MainFont;

			ControlHelper.SetTableRowStyles(parent.TableFleet, ControlHelper.GetDefaultRowStyle());
		}

		public void Dispose()
		{
			Name.Dispose();
			State.Dispose();
			AirSuperiority.Dispose();
			SearchingAbility.Dispose();
			AntiAirPower.Dispose();
		}
	}


	private class TableMemberControl : IDisposable
	{
		public ImageLabel Name;
		public ShipStatusLevel Level;
		public ShipStatusHP HP;
		public ImageLabel Condition { get; set; }
		public ShipStatusResource ShipResource;
		public ShipStatusEquipment Equipments;

		private ToolTip ToolTipInfo;
		private FormFleet Parent;


		public TableMemberControl(FormFleet parent)
		{

			#region Initialize

			Name = new ImageLabel();
			Name.SuspendLayout();
			Name.Text = "*nothing*";
			Name.Anchor = AnchorStyles.Left;
			Name.TextAlign = ContentAlignment.MiddleLeft;
			Name.ImageAlign = ContentAlignment.MiddleCenter;
			Name.ForeColor = parent.MainFontColor;
			Name.Padding = new Padding(2, 1, 2, 1);
			Name.Margin = new Padding(2, 1, 2, 1);
			Name.AutoSize = true;
			//Name.AutoEllipsis = true;
			Name.Visible = false;
			Name.Cursor = Cursors.Help;
			Name.MouseDown += Name_MouseDown;
			Name.ResumeLayout();

			Level = new ShipStatusLevel();
			Level.SuspendLayout();
			Level.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			Level.Value = 0;
			Level.MaximumValue = ExpTable.ShipMaximumLevel;
			Level.ValueNext = 0;
			Level.MainFontColor = parent.MainFontColor;
			Level.SubFontColor = parent.SubFontColor;
			//Level.TextNext = "n.";
			Level.Padding = new Padding(0, 0, 0, 0);
			Level.Margin = new Padding(2, 0, 2, 1);
			Level.AutoSize = true;
			Level.Visible = false;
			Level.Cursor = Cursors.Help;
			Level.MouseDown += Level_MouseDown;
			Level.ResumeLayout();

			HP = new ShipStatusHP();
			HP.SuspendUpdate();
			HP.Anchor = AnchorStyles.Left;
			HP.Value = 0;
			HP.MaximumValue = 0;
			HP.MaximumDigit = 999;
			HP.UsePrevValue = false;
			HP.MainFontColor = parent.MainFontColor;
			HP.SubFontColor = parent.SubFontColor;
			HP.Padding = new Padding(0, 0, 0, 0);
			HP.Margin = new Padding(2, 1, 2, 2);
			HP.AutoSize = true;
			HP.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			HP.Visible = false;
			HP.ResumeUpdate();

			Condition = new ImageLabel();
			Condition.SuspendLayout();
			Condition.Text = "*";
			Condition.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			Condition.ForeColor = parent.MainFontColor;
			Condition.TextAlign = ContentAlignment.BottomRight;
			Condition.ImageAlign = ContentAlignment.MiddleLeft;
			Condition.ImageList = ResourceManager.Instance.Icons;
			Condition.Padding = new Padding(2, 1, 2, 1);
			Condition.Margin = new Padding(2, 1, 2, 1);
			Condition.Size = new Size(40, 20);
			Condition.AutoSize = true;
			Condition.Visible = false;
			Condition.ResumeLayout();

			ShipResource = new ShipStatusResource(parent.ToolTipInfo);
			ShipResource.SuspendLayout();
			ShipResource.FuelCurrent = 0;
			ShipResource.FuelMax = 0;
			ShipResource.AmmoCurrent = 0;
			ShipResource.AmmoMax = 0;
			ShipResource.Anchor = AnchorStyles.Left;
			ShipResource.Padding = new Padding(0, 2, 0, 0);
			ShipResource.Margin = new Padding(2, 0, 2, 1);
			ShipResource.Size = new Size(30, 20);
			ShipResource.AutoSize = false;
			ShipResource.Visible = false;
			ShipResource.ResumeLayout();

			Equipments = new ShipStatusEquipment();
			Equipments.SuspendUpdate();
			Equipments.Anchor = AnchorStyles.Left;
			Equipments.Padding = new Padding(0, 1, 0, 1);
			Equipments.Margin = new Padding(2, 0, 2, 1);
			Equipments.Size = new Size(40, 20);
			Equipments.AutoSize = true;
			Equipments.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Equipments.Visible = false;
			Equipments.ResumeUpdate();

			ConfigurationChanged(parent);

			ToolTipInfo = parent.ToolTipInfo;
			Parent = parent;
			#endregion

		}

		public TableMemberControl(FormFleet parent, TableLayoutPanel table, int row)
			: this(parent)
		{
			AddToTable(table, row);

			Equipments.Name = string.Format("{0}_{1}", parent.FleetID, row + 1);
		}


		public void AddToTable(TableLayoutPanel table, int row)
		{

			table.SuspendLayout();

			table.Controls.Add(Name, 0, row);
			table.Controls.Add(Level, 1, row);
			table.Controls.Add(HP, 2, row);
			table.Controls.Add(Condition, 3, row);
			table.Controls.Add(ShipResource, 4, row);
			table.Controls.Add(Equipments, 5, row);

			table.ResumeLayout();

		}

		public void Update(int shipMasterID)
		{

			KCDatabase db = KCDatabase.Instance;
			ShipData ship = db.Ships[shipMasterID];

			if (ship != null)
			{

				bool isEscaped = KCDatabase.Instance.Fleet[Parent.FleetID].EscapedShipList.Contains(shipMasterID);
				var equipments = ship.AllSlotInstance.Where(eq => eq != null);

				Name.Text = ship.MasterShip.NameWithClass;
				Name.Tag = ship.ShipID;
				ToolTipInfo.SetToolTip(Name,
					string.Format(Translation.ShipNameToolTip,
						ship.SallyArea > 0 ? $"[{ship.SallyArea}] " : "",
						ship.MasterShip.ShipTypeName, ship.NameWithLevel,
						ship.FirepowerBase, ship.FirepowerTotal,
						ship.TorpedoBase, ship.TorpedoTotal,
						ship.AABase, ship.AATotal,
						ship.ArmorBase, ship.ArmorTotal,
						ship.ASWBase, ship.ASWTotal,
						ship.EvasionBase, ship.EvasionTotal,
						ship.LOSBase, ship.LOSTotal,
						ship.LuckTotal,
						equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Accuracy) : 0,
						equipments.Any() ? equipments.Sum(eq => eq.MasterEquipment.Bomber) : 0,
						Constants.GetRange(ship.Range),
						Constants.GetSpeed(ship.Speed)
					));
				{
					var colorscheme = Utility.Configuration.Config.FormFleet.SallyAreaColorScheme;

					if (Utility.Configuration.Config.FormFleet.AppliesSallyAreaColor &&
						(colorscheme?.Count ?? 0) > 0 &&
						ship.SallyArea > 0)
					{
						if (Utility.Configuration.Config.UI.ThemeMode != 0)
							Name.ForeColor = Utility.Configuration.Config.UI.BackColor;
						Name.BackColor = colorscheme[Math.Min(ship.SallyArea, colorscheme.Count - 1)];
					}
					else
					{
						Name.ForeColor = Utility.Configuration.Config.UI.ForeColor;
						Name.BackColor = Utility.Configuration.Config.UI.BackColor;
					}
				}


				Level.Value = ship.Level;
				Level.ValueNext = ship.ExpNext;
				Level.Tag = ship.MasterID;

				{
					StringBuilder tip = new();
					tip.AppendFormat("Total: {0:N0} exp.\r\n", ship.ExpTotal);

					if (!Utility.Configuration.Config.FormFleet.ShowNextExp)
						tip.AppendFormat(GeneralRes.ToNextLevel + " exp.\r\n", ship.ExpNext.ToString("N0"));

					var remodels = db.MasterShips.Values
						.Where(s => s.BaseShip() == ship.MasterShip.BaseShip())
						.Where(s => s.RemodelTier > ship.MasterShip.RemodelTier)
						.OrderBy(s => s.RemodelBeforeShip?.RemodelAfterLevel ?? 0)
						.Select(s => (s.NameEN, s.RemodelBeforeShip?.RemodelAfterLevel ?? 0));

					foreach ((var name, int remodelLevel) in remodels)
					{
						int neededExp = Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, remodelLevel), 0);
						tip.Append($"{name}({remodelLevel}): {neededExp:N0} exp.\r\n");
					}

					if (ship.Level < 99)
					{
						string lv99Exp = Math.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, 99), 0).ToString("N0");
						tip.AppendFormat(GeneralRes.To99 + " exp.\r\n", lv99Exp);
					}

					if (ship.Level < ExpTable.ShipMaximumLevel)
					{
						string lv175Exp = Math
							.Max(ExpTable.GetExpToLevelShip(ship.ExpTotal, ExpTable.ShipMaximumLevel), 0)
							.ToString("N0");
						tip.AppendFormat(GeneralRes.ToX + " exp.\r\n", ExpTable.ShipMaximumLevel, lv175Exp);
					}


					tip.AppendLine(Translation.ExpCalcHint);

					ToolTipInfo.SetToolTip(Level, tip.ToString());
				}


				HP.SuspendUpdate();
				HP.Value = HP.PrevValue = ship.HPCurrent;
				HP.MaximumValue = ship.HPMax;
				HP.UsePrevValue = false;
				HP.ShowDifference = false;
				{
					int dockID = ship.RepairingDockID;

					if (dockID != -1)
					{
						HP.RepairTime = db.Docks[dockID].CompletionTime;
						HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Visible;
					}
					else
					{
						HP.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.Invisible;
					}
				}
				HP.Tag = (ship.RepairingDockID == -1 && 0.5 < ship.HPRate && ship.HPRate < 1.0) ? DateTimeHelper.FromAPITimeSpan(ship.RepairTime).TotalSeconds : 0.0;
				if (isEscaped)
				{
					HP.BackColor = Utility.Configuration.Config.UI.SubBackColor;
				}
				else
				{
					HP.BackColor = Utility.Configuration.Config.UI.BackColor;
				}
				{
					StringBuilder sb = new StringBuilder();
					double hprate = (double)ship.HPCurrent / ship.HPMax;

					sb.AppendFormat("HP: {0:0.0}% [{1}]\n", hprate * 100, Constants.GetDamageState(hprate));
					if (isEscaped)
					{
						sb.AppendLine(GeneralRes.Retreating);
					}
					else if (hprate > 0.50)
					{
						sb.AppendFormat(GeneralRes.ToMidAndHeavy + "\n", ship.HPCurrent - ship.HPMax / 2, ship.HPCurrent - ship.HPMax / 4);
					}
					else if (hprate > 0.25)
					{
						sb.AppendFormat(GeneralRes.ToHeavy + "\n", ship.HPCurrent - ship.HPMax / 4);
					}
					else
					{
						sb.AppendLine(GeneralRes.IsTaiha);
					}

					if (ship.RepairTime > 0)
					{
						var span = DateTimeHelper.FromAPITimeSpan(ship.RepairTime);
						sb.AppendFormat(GeneralRes.DockTime + ": {0} @ {1}",
							DateTimeHelper.ToTimeRemainString(span),
							DateTimeHelper.ToTimeRemainString(Calculator.CalculateDockingUnitTime(ship)));
					}

					ToolTipInfo.SetToolTip(HP, sb.ToString());
				}
				HP.ResumeUpdate();


				Condition.Text = ship.Condition.ToString();
				Condition.Tag = ship.Condition;
				SetConditionDesign(Condition, ship.Condition);

				if (ship.Condition < 49)
				{
					TimeSpan ts = new TimeSpan(0, (int)Math.Ceiling((49 - ship.Condition) / 3.0) * 3, 0);
					ToolTipInfo.SetToolTip(Condition, string.Format(GeneralRes.FatigueRestoreTime, (int)ts.TotalMinutes, (int)ts.Seconds));
				}
				else
				{
					ToolTipInfo.SetToolTip(Condition, string.Format(GeneralRes.RemainingExpeds, (int)Math.Ceiling((ship.Condition - 49) / 3.0)));
				}

				ShipResource.SetResources(ship.Fuel, ship.FuelMax, ship.Ammo, ship.AmmoMax);


				Equipments.SetSlotList(ship);
				ToolTipInfo.SetToolTip(Equipments, GetEquipmentString(ship));

			}
			else
			{
				Name.Tag = -1;
			}


			Name.Visible =
				Level.Visible =
					HP.Visible =
						Condition.Visible =
							ShipResource.Visible =
								Equipments.Visible = shipMasterID != -1;

		}

		void Name_MouseDown(object sender, MouseEventArgs e)
		{
			if (Name.Tag is int id && id != -1)
			{
				if ((e.Button & MouseButtons.Right) != 0)
				{
					new DialogAlbumMasterShip(id).Show(Parent);
				}
			}
		}

		private void Level_MouseDown(object sender, MouseEventArgs e)
		{
			if (Level.Tag is int id && id != -1)
			{
				if ((e.Button & MouseButtons.Right) != 0)
				{
					new DialogExpChecker(id).Show(Parent);
				}
			}
		}


		private string GetEquipmentString(ShipData ship)
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
			IFleetData fleet = KCDatabase.Instance.Fleet[Parent.FleetID];

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
						DayAttackKind dayAttack => Constants.GetDayAttackKind(dayAttack),
						DayAirAttackCutinKind cvci => cvci switch
						{
							DayAirAttackCutinKind.FighterBomberAttacker => Translation.CvciFba,
							DayAirAttackCutinKind.BomberBomberAttacker => Translation.CvciBba,
							DayAirAttackCutinKind.BomberAttacker => Translation.CvciBa,
							_ => "?"
						},
						_ => $"{attack}"
					};
					sb.AppendFormat($"\r\n・[{asRate:P1} | {asPlusRate:P1}] - {attackDisplay} - " + Translation.Power + $": {power} - " + Translation.Accuracy + $": {accuracy:0.##}");
				}
			}

			List<Enum> nightAttacks = ship.GetNightAttacks().ToList();
			List<double> nightAttackRates = nightAttacks.Select(a => ship.GetNightAttackRate(a, fleet))
				.ToList().TotalRates();

			if (nightAttacks.Any())
			{
				sb.AppendFormat($"\r\n{GeneralRes.NightBattle}:");

				foreach ((Enum attack, double rate) in nightAttacks.Zip(nightAttackRates, (attack, rate) => (attack, rate)))
				{
					double power = ship.GetNightAttackPower(attack, fleet);
					double accuracy = ship.GetNightAttackAccuracy(attack, fleet);
					string attackDisplay = attack switch
					{
						NightAttackKind nightAttack => Constants.GetNightAttackKind(nightAttack),
						CvnciKind cvnci => cvnci switch
						{
							CvnciKind.FighterFighterAttacker => Translation.CvnciFfa,
							CvnciKind.FighterAttacker => Translation.CvnciFa,
							CvnciKind.Phototube => Translation.CvnciPhoto,
							CvnciKind.FighterOtherOther => Translation.CvnciFoo,
							_ => "?"
						},
						NightTorpedoCutinKind torpedoCutin => torpedoCutin switch
						{
							NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => Translation.LateModelTorpedoSubmarineEquipment,
							NightTorpedoCutinKind.LateModelTorpedo2 => Translation.LateModelTorpedo2,
							_ => "?"
						},
						_ => $"{attack}"
					};

					sb.AppendFormat($"\r\n・[{rate:P1}] - {attackDisplay} - " + Translation.Power + $": {power} - " + Translation.Accuracy + $": {accuracy:0.##}");
				}
			}

			sb.AppendLine();

			{
				int torpedo = ship.TorpedoPower;
				int asw = ship.AntiSubmarinePower;

				DayAttackKind aswAttack = Calculator.GetDayAttackKind(ship.AllSlotMaster.ToArray(), ship.ShipID, 126, false);
				int asw2 = ship.GetAswAttackPower(aswAttack, fleet);

				if (torpedo > 0)
				{
					sb.AppendFormat($"{ConstantsRes.TorpedoAttack}: {Translation.Power}: {torpedo}");
					sb.Append($" - {Translation.Accuracy}: {ship.GetTorpedoAttackAccuracy(fleet):0.##}");
				}
				if (asw > 0)
				{
					if (torpedo > 0)
						sb.AppendLine();

					sb.AppendFormat($"{Translation.Asw}: {Translation.Power}: {asw2}");

					if (ship.CanOpeningASW)
						sb.Append(Translation.OpeningAsw);

					sb.Append($" - {Translation.Accuracy}: {ship.GetAswAttackAccuracy(fleet):0.##}");
				}
				if (torpedo > 0 || asw > 0)
					sb.AppendLine();
			}

			{
				int aacutin = Calculator.GetAACutinKind(ship.ShipID, slotmaster);
				if (aacutin != 0)
				{
					sb.AppendFormat(GeneralRes.AntiAir + ": {0}\r\n", Constants.GetAACutinKind(aacutin));
				}
				double adjustedaa = Calculator.GetAdjustedAAValue(ship);
				sb.AppendFormat(GeneralRes.ShipAADefense + "\r\n",
					adjustedaa,
					Calculator.GetProportionalAirDefense(adjustedaa)
				);

				double aarbRate = Calculator.GetAarbRate(ship, adjustedaa);
				if (aarbRate > 0)
				{
					sb.Append($"{Translation.Aarb}: {aarbRate:p1}\r\n");
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
						sb.AppendFormat(GeneralRes.AirPower + ": {0} / " + Translation.AirstrikePower + ": {1}\r\n", airsup_str, airbattle);
					else
						sb.AppendFormat(GeneralRes.AirPower + ": {0}\r\n", airsup_str);
				}
				else if (airbattle > 0)
					sb.AppendFormat(Translation.AirstrikePower + ": {0}\r\n", airbattle);
			}

			return sb.ToString();
		}


		public void ConfigurationChanged(FormFleet parent)
		{
			Name.Font = parent.MainFont;
			Level.MainFont = parent.MainFont;
			Level.SubFont = parent.SubFont;
			HP.MainFont = parent.MainFont;
			HP.SubFont = parent.SubFont;
			Condition.Font = parent.MainFont;
			SetConditionDesign(Condition, (Condition.Tag as int?) ?? 49);
			Equipments.Font = parent.SubFont;
		}

		public void Dispose()
		{
			Name.Dispose();
			Level.Dispose();
			HP.Dispose();
			Condition.Dispose();
			ShipResource.Dispose();
			Equipments.Dispose();

		}
	}

	public static void SetConditionDesign(ImageLabel label, int cond)
	{

		if (label.ImageAlign == ContentAlignment.MiddleCenter)
		{
			// icon invisible
			label.ImageIndex = -1;

			(label.BackColor, label.ForeColor) = cond switch
			{
				{ } when cond < 20 => (Color.LightCoral, Color.Black),
				{ } when cond < 30 => (Color.LightSalmon, Color.Black),
				{ } when cond < 40 => (Color.Moccasin, Color.Black),
				{ } when cond < 50 => (Color.Transparent, Utility.Configuration.Config.UI.ForeColor),
				_ => (Color.LightGreen, Color.Black)
			};
		}
		else
		{
			label.BackColor = Color.Transparent;
			label.ForeColor = Utility.Configuration.Config.UI.ForeColor;

			label.ImageIndex =
				cond < 20 ? (int)IconContent.ConditionVeryTired :
				cond < 30 ? (int)IconContent.ConditionTired :
				cond < 40 ? (int)IconContent.ConditionLittleTired :
				cond < 50 ? (int)IconContent.ConditionNormal :
				(int)IconContent.ConditionSparkle;

		}
	}





	public int FleetID { get; private set; }


	public Font MainFont { get; set; }
	public Font SubFont { get; set; }
	public Color MainFontColor { get; set; }
	public Color SubFontColor { get; set; }


	private TableFleetControl ControlFleet;
	private TableMemberControl[] ControlMember;

	private int AnchorageRepairBound;
	public Action<IconContent>? SetIcon { get; }

	public FormFleet(FormMain parent, int fleetID, Action<IconContent>? setIcon = null)
	{
		InitializeComponent();

		FleetID = fleetID;
		SetIcon = setIcon;
		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

		ConfigurationChanged();

		MainFontColor = Utility.Configuration.Config.UI.ForeColor;
		SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

		AnchorageRepairBound = 0;

		//ui init

		ControlHelper.SetDoubleBuffered(TableFleet);
		ControlHelper.SetDoubleBuffered(TableMember);


		TableFleet.Visible = false;
		TableFleet.SuspendLayout();
		// TableFleet.BorderStyle = BorderStyle.FixedSingle;
		TableFleet.BackColor = Utility.Configuration.Config.UI.SubBackColor;
		ControlFleet = new TableFleetControl(this, TableFleet);
		TableFleet.ResumeLayout();


		TableMember.SuspendLayout();
		ControlMember = new TableMemberControl[7];
		for (int i = 0; i < ControlMember.Length; i++)
		{
			ControlMember[i] = new TableMemberControl(this, TableMember, i);
		}
		TableMember.ResumeLayout();


		ConfigurationChanged();     //fixme: 苦渋の決断

		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormFleet]);

		Translate();
	}

	public void Translate()
	{
		ContextMenuFleet_CopyFleet.Text = Translation.ContextMenuFleet_CopyFleet;
		ContextMenuFleet_CopyFleetDeckBuilder.Text = Translation.ContextMenuFleet_CopyFleetDeckBuilder;
		ContextMenuFleet_CopyKanmusuList.Text = Translation.ContextMenuFleet_CopyKanmusuList;
		ContextMenuFleet_CopyFleetAnalysis.Text = Translation.ContextMenuFleet_CopyFleetAnalysis;
		ContextMenuFleet_CopyFleetAnalysisLockedEquip.Text = Translation.ContextMenuFleet_CopyFleetAnalysisLockedEquip;
		ContextMenuFleet_CopyFleetAnalysisAllEquip.Text = Translation.ContextMenuFleet_CopyFleetAnalysisAllEquip;

		ContextMenuFleet_AntiAirDetails.Text = Translation.ContextMenuFleet_AntiAirDetails;
		ContextMenuFleet_Capture.Text = Translation.ContextMenuFleet_Capture;
		ContextMenuFleet_OutputFleetImage.Text = Translation.ContextMenuFleet_OutputFleetImage;

		// Text = Translation.Title;
	}

	private void FormFleet_Load(object sender, EventArgs e)
	{

		Text = string.Format("#{0}", FleetID);

		APIObserver o = APIObserver.Instance;

		o.ApiReqNyukyo_Start.RequestReceived += Updated;
		o.ApiReqNyukyo_SpeedChange.RequestReceived += Updated;
		o.ApiReqHensei_Change.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyShip.RequestReceived += Updated;
		o.ApiReqMember_UpdateDeckName.RequestReceived += Updated;
		o.ApiReqKaisou_Remodeling.RequestReceived += Updated;
		o.ApiReqMap_Start.RequestReceived += Updated;
		o.ApiReqHensei_Combined.RequestReceived += Updated;
		o.ApiReqKaisou_OpenExSlot.RequestReceived += Updated;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_Ship2.ResponseReceived += Updated;
		o.ApiGetMember_NDock.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiReqHokyu_Charge.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyShip.ResponseReceived += Updated;
		o.ApiGetMember_Ship3.ResponseReceived += Updated;
		o.ApiReqKaisou_PowerUp.ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
		o.ApiGetMember_Deck.ResponseReceived += Updated;
		o.ApiGetMember_SlotItem.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiGetMember_ShipDeck.ResponseReceived += Updated;
		o.ApiReqHensei_PresetSelect.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotExchangeIndex.ResponseReceived += Updated;
		o.ApiGetMember_RequireInfo.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotDeprive.ResponseReceived += Updated;
		o.ApiReqKaisou_Marriage.ResponseReceived += Updated;
		o.ApiReqMap_AnchorageRepair.ResponseReceived += Updated;

		//追加するときは FormFleetOverview にも同様に追加してください

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}


	void Updated(string apiname, dynamic data)
	{

		if (IsRemodeling)
		{
			if (apiname == "api_get_member/slot_item")
				IsRemodeling = false;
			else
				return;
		}
		if (apiname == "api_req_kaisou/remodeling")
		{
			IsRemodeling = true;
			return;
		}

		KCDatabase db = KCDatabase.Instance;

		if (db.Ships.Count == 0) return;

		FleetData fleet = db.Fleet.Fleets[FleetID];
		if (fleet == null) return;

		TableFleet.SuspendLayout();
		ControlFleet.Update(fleet);
		TableFleet.Visible = true;
		TableFleet.ResumeLayout();

		AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

		TableMember.SuspendLayout();
		TableMember.RowCount = fleet.Members.Count(id => id > 0);
		for (int i = 0; i < ControlMember.Length; i++)
		{
			ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
		}
		TableMember.ResumeLayout();


		if (Icon != null) ResourceManager.DestroyIcon(Icon);
		int iconIndex = ControlFleet.State.GetIconIndex();
		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[iconIndex]);
		SetIcon?.Invoke((IconContent)iconIndex);
		if (Parent != null) Parent.Refresh();       //アイコンを更新するため

	}


	void UpdateTimerTick()
	{

		FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetID];

		TableFleet.SuspendLayout();
		{
			if (fleet != null)
				ControlFleet.Refresh();

		}
		TableFleet.ResumeLayout();

		TableMember.SuspendLayout();
		for (int i = 0; i < ControlMember.Length; i++)
		{
			ControlMember[i].HP.Refresh();
		}
		TableMember.ResumeLayout();


		// anchorage repairing
		if (fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
		{
			TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

			if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
			{

				for (int i = 0; i < AnchorageRepairBound; i++)
				{
					var hpbar = ControlMember[i].HP;

					double dockingSeconds = hpbar.Tag as double? ?? 0.0;

					if (dockingSeconds <= 0.0)
						continue;

					hpbar.SuspendUpdate();

					if (!hpbar.UsePrevValue)
					{
						hpbar.UsePrevValue = true;
						hpbar.ShowDifference = true;
					}

					int damage = hpbar.MaximumValue - hpbar.PrevValue;
					int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

					hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
					hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
					hpbar.Value = hpbar.PrevValue + healAmount;

					hpbar.ResumeUpdate();
				}
			}
		}
	}


	//艦隊編成のコピー
	private void ContextMenuFleet_CopyFleet_Click(object sender, EventArgs e)
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[FleetID];
		if (fleet == null) return;

		sb.AppendFormat(Translation.CopyFleetText + "\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(ControlFleet.BranchWeight), Calculator.GetTPDamage(fleet));
		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			ShipData ship = db.Ships[fleet[i]];

			sb.AppendFormat("{0}/Lv{1}\t", ship.MasterShip.NameEN, ship.Level);

			var eq = ship.AllSlotInstance;


			if (eq != null)
			{
				for (int j = 0; j < eq.Count; j++)
				{

					if (eq[j] == null) continue;

					int count = 1;
					for (int k = j + 1; k < eq.Count; k++)
					{
						if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
						{
							count++;
						}
						else
						{
							break;
						}
					}

					if (count == 1)
					{
						sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
					}
					else
					{
						sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
					}

					j += count - 1;
				}
			}

			sb.AppendLine();
		}


		Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
	}


	private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
	{

		ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

	}



	/// <summary>
	/// 「艦隊デッキビルダー」用編成コピー
	/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
	/// </summary>
	private void ContextMenuFleet_CopyFleetDeckBuilder_Click(object sender, EventArgs e)
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ

		sb.Append(@"{""version"":4,");

		foreach (var fleet in db.Fleet.Fleets.Values)
		{
			if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

			sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

			int shipcount = 1;
			foreach (var ship in fleet.MembersInstance)
			{
				if (ship == null) break;

				sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
					shipcount,
					ship.ShipID,
					ship.Level,
					ship.LuckBase);

				int eqcount = 1;
				foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
				{
					if (eq == null) break;

					sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

					eqcount++;
				}

				if (eqcount > 1)
					sb.Remove(sb.Length - 1, 1);        // remove ","
				sb.Append(@"}},");

				shipcount++;
			}

			if (shipcount > 0)
				sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"},");

		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append(@"}");

		Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
	}


	/// <summary>
	/// 「艦隊晒しページ」用編成コピー
	/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
	/// </summary>
	private void ContextMenuFleet_CopyKanmusuList_Click(object sender, EventArgs e)
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// version
		sb.Append(".2");

		// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
		Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

		foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			var master = ship.MasterShip;
			while (master.RemodelBeforeShip != null)
				master = master.RemodelBeforeShip;

			if (!shiplist.ContainsKey(master.ShipID))
			{
				shiplist.Add(master.ShipID, new List<ShipData>() { ship });
			}
			else
			{
				shiplist[master.ShipID].Add(ship);
			}
		}

		// 上で作った分類の各項を文字列化
		foreach (var sl in shiplist)
		{
			sb.Append("|").Append(sl.Key).Append(":");

			foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
			{
				sb.Append(ship.Level);

				// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
				if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
				{
					sb.Append(".");
					int count = 1;
					var master = ship.MasterShip;
					while (master.RemodelBeforeShip != null)
					{
						master = master.RemodelBeforeShip;
						count++;
					}
					sb.Append(count);
				}
				sb.Append(",");
			}

			// 余った "," を削除
			sb.Remove(sb.Length - 1, 1);
		}

		Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
	}

	/// <summary>
	/// 「艦隊分析 -艦これ-」の艦隊情報反映用フォーマットでコピー
	/// https://kancolle-fleetanalysis.firebaseapp.com/#/
	/// </summary>
	private void ContextMenuFleet_CopyToFleetAnalysis_Click(object sender, EventArgs e)
	{
		var sb = new StringBuilder();

		sb.Append("[");
		foreach (var ship in KCDatabase.Instance.Ships.Values.Where(s => s.IsLocked))
		{
			sb.AppendFormat(@"{{""api_ship_id"":{0},""api_lv"":{1},""api_kyouka"":[{2}],""api_exp"":[{3}]}},",
				ship.ShipID, ship.Level, string.Join(",", (int[])ship.RawData.api_kyouka), string.Join(",", (int[])ship.RawData.api_exp));
		}
		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append("]");

		Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
	}


	/// <summary>
	/// 
	/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
	/// </summary>
	private void ContextMenuFleet_CopyFleetAnalysis_Click(object sender, EventArgs e)
	{
		KCDatabase db = KCDatabase.Instance;
		List<string> ships = new List<string>();

		foreach (ShipData ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			int[] apiKyouka =
			{
				ship.FirepowerModernized,
				ship.TorpedoModernized,
				ship.AAModernized,
				ship.ArmorModernized,
				ship.LuckModernized,
				ship.HPMaxModernized,
				ship.ASWModernized
			};

			int expProgress = 0;
			if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
			{
				expProgress = (ExpTable.ShipExp[ship.Level].Next - ship.ExpNext)
							  / ExpTable.ShipExp[ship.Level].Next;
			}

			int[] apiExp = { ship.ExpTotal, ship.ExpNext, expProgress };

			string shipId = $"\"api_ship_id\":{ship.ShipID}";
			string level = $"\"api_lv\":{ship.Level}";
			string kyouka = $"\"api_kyouka\":[{string.Join(",", apiKyouka)}]";
			string exp = $"\"api_exp\":[{string.Join(",", apiExp)}]";
			string sallyArea = $"\"api_sally_area\":{(ship.SallyArea)}";

			string[] analysisData = { shipId, level, kyouka, exp, sallyArea };

			ships.Add($"{{{string.Join(",", analysisData)}}}");
		}

		string json = $"[{string.Join(",", ships)}]";

		Clipboard.SetData(DataFormats.StringFormat, json);
	}

	/// <summary>
	/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
	/// </summary>
	private void ContextMenuFleet_CopyFleetAnalysisLockedEquip_Click(object sender, EventArgs e)
	{
		GenerateEquipList(false);
	}

	private void ContextMenuFleet_CopyFleetAnalysisAllEquip_Click(object sender, EventArgs e)
	{
		GenerateEquipList(true);
	}

	private void GenerateEquipList(bool allEquipment)
	{
		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ
		// pain and suffering

		sb.Append("[");

		foreach (EquipmentData equip in db.Equipments.Values.Where(eq => allEquipment || eq.IsLocked))
		{
			sb.Append($"{{\"api_slotitem_id\":{equip.EquipmentID},\"api_level\":{equip.Level}}},");
		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append("]");

		Clipboard.SetData(DataFormats.StringFormat, sb.ToString());
	}


	private void ContextMenuFleet_AntiAirDetails_Click(object sender, EventArgs e)
	{

		var dialog = new DialogAntiAirDefense();

		if (KCDatabase.Instance.Fleet.CombinedFlag != 0 && (FleetID == 1 || FleetID == 2))
			dialog.SetFleetID(5);
		else
			dialog.SetFleetID(FleetID);

		dialog.Show(this);
	}


	private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
	{

		using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
		{
			this.DrawToBitmap(bitmap, this.ClientRectangle);

			Clipboard.SetData(DataFormats.Bitmap, bitmap);
		}
	}


	private void ContextMenuFleet_OutputFleetImage_Click(object sender, EventArgs e)
	{

		using (var dialog = new DialogFleetImageGenerator(FleetID))
		{
			dialog.ShowDialog(this);
		}
	}



	void ConfigurationChanged()
	{

		var c = Utility.Configuration.Config;

		MainFont = Font = c.UI.MainFont;
		SubFont = c.UI.SubFont;

		AutoScroll = c.FormFleet.IsScrollable;

		var fleet = KCDatabase.Instance.Fleet[FleetID];

		TableFleet.SuspendLayout();
		if (ControlFleet != null && fleet != null)
		{
			ControlFleet.ConfigurationChanged(this);
			ControlFleet.Update(fleet);
		}
		TableFleet.ResumeLayout();

		TableMember.SuspendLayout();
		if (ControlMember != null)
		{
			bool showAircraft = c.FormFleet.ShowAircraft;
			bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
			bool shortHPBar = c.FormFleet.ShortenHPBar;
			bool colorMorphing = c.UI.BarColorMorphing;
			Color[] colorScheme = c.UI.BarColorScheme.Select(col => col.ColorData).ToArray();
			bool showNext = c.FormFleet.ShowNextExp;
			bool showConditionIcon = c.FormFleet.ShowConditionIcon;
			var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
			bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
			int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
			bool isLayoutFixed = c.UI.IsLayoutFixed;

			for (int i = 0; i < ControlMember.Length; i++)
			{
				var member = ControlMember[i];

				member.Equipments.ShowAircraft = showAircraft;
				if (fixShipNameWidth)
				{
					member.Name.AutoSize = false;
					member.Name.Size = new Size(fixedShipNameWidth, 20);
				}
				else
				{
					member.Name.AutoSize = true;
				}

				member.HP.SuspendUpdate();
				member.HP.Text = shortHPBar ? "" : "HP:";
				member.HP.HPBar.ColorMorphing = colorMorphing;
				member.HP.HPBar.SetBarColorScheme(colorScheme);
				member.HP.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical) : Size.Empty;
				member.HP.ResumeUpdate();
				member.Level.TextNext = showNext ? "next:" : null;
				member.Condition.ImageAlign = showConditionIcon ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
				member.Equipments.LevelVisibility = levelVisibility;
				member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
				member.Equipments.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical) : Size.Empty;
				member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
				member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
				member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

				member.ConfigurationChanged(this);
				if (fleet != null)
					member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
		}

		ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());
		TableMember.ResumeLayout();

		TableMember.Location = new Point(TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

		TableMember.PerformLayout();        //fixme:サイズ変更に親パネルが追随しない

	}



	private void TableMember_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
	{
		e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
	}


	protected override string GetPersistString()
	{
		return "Fleet #" + FleetID.ToString();
	}


	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		ControlFleet.Dispose();
		for (int i = 0; i < ControlMember.Length; i++)
			ControlMember[i].Dispose();


		// --- auto generated ---
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}


}
