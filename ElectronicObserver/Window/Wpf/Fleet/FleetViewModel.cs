using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;
using ElectronicObserver.Database;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using System.Drawing;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserverTypes.Attacks.Specials;

namespace ElectronicObserver.Window.Wpf.Fleet;

public partial class FleetViewModel : AnchorableViewModel
{
	public FormFleetTranslationViewModel FormFleet { get; }
	private ToolService ToolService { get; }
	private DataSerializationService DataSerializationService { get; }

	public FleetStatusViewModel ControlFleet { get; }
	public List<FleetItemViewModel> ControlMember { get; } = new();

	public int FleetId { get; }
	public int AnchorageRepairBound { get; set; }

	public List<SpecialAttack> SpecialsAttacks { get; set; } = new();

	public List<Color> ShipTagColors { get; set; } = GetShipTagColorList();

	public IEnumerable<AirBaseArea> AirBaseAreas => KCDatabase.Instance.BaseAirCorps.Values
		.Select(b => b.MapAreaID)
		.Distinct()
		.OrderBy(i => i)
		.Select(i => KCDatabase.Instance.MapArea[i])
		.Where(m => m != null)
		.Select(m => new AirBaseArea(m.MapAreaID, m.NameEN))
		.Prepend(new AirBaseArea(0, AirControlSimulatorResources.None));

	public FleetViewModel(int fleetId) : base($"#{fleetId}", $"Fleet{fleetId}", IconContent.FormFleet)
	{
		FormFleet = Ioc.Default.GetRequiredService<FormFleetTranslationViewModel>();
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		DataSerializationService = Ioc.Default.GetRequiredService<DataSerializationService>();

		Title = $"#{fleetId}";
		FormFleet.PropertyChanged += (_, _) => Title = $"#{fleetId}";

		FleetId = fleetId;

		SystemEvents.UpdateTimerTick += UpdateTimerTick;

		AnchorageRepairBound = 0;

		ControlFleet = new(FleetId);
		for (int i = 0; i < 7; i++)
		{
			ControlMember.Add(new(this));
		}

		ConfigurationChanged();

		SubscribeToApis();

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		// Update tag colours 
		EventLockPlannerViewModel.TagChanged += UpdateShipTagColorList;
	}

	private void SubscribeToApis()
	{
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

		o.ApiGetMember_MapInfo.ResponseReceived += (_, _) =>
		{
			OnPropertyChanged(nameof(AirBaseAreas));
		};
	}

	private void Updated(string apiname, dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;

		if (db.Ships.Count == 0) return;

		FleetData fleet = db.Fleet.Fleets[FleetId];
		if (fleet == null) return;

		ControlFleet.Update(fleet);

		AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

		SpecialsAttacks = fleet.GetSpecialAttacks();

		for (int i = 0; i < ControlMember.Count; i++)
		{
			ControlMember[i].SpecialAttackHitList = SpecialsAttacks
				.Where(specialAttack => specialAttack.GetHitsPerShip(i).Any())
				.ToDictionary(specialAttack => specialAttack, specialAttack => specialAttack.GetHitsPerShip(i));

			ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
		}
		
		Icon = ControlFleet.State.GetIcon();
	}

	private void UpdateTimerTick()
	{
		FleetData? fleet = KCDatabase.Instance.Fleet.Fleets[FleetId];

		if (fleet is not null)
		{
			ControlFleet.Refresh();
		}

		foreach (FleetItemViewModel fleetItem in ControlMember)
		{
			// this is for updating the repair timer when a ship is docked
			fleetItem.HP.ResumeUpdate();
		}

		// anchorage repairing
		if (fleet is not null && Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
		{
			TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

			if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
			{
				for (int i = 0; i < AnchorageRepairBound; i++)
				{
					FleetHpViewModel hpbar = ControlMember[i].HP;

					double dockingSeconds = hpbar.Tag as double? ?? 0.0;

					if (dockingSeconds <= 0.0) continue;

					if (!hpbar.UsePrevValue)
					{
						hpbar.UsePrevValue = true;
						hpbar.ShowDifference = true;
					}

					int damage = hpbar.HPBar.MaximumValue - hpbar.PrevValue;
					int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

					hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
					hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
					hpbar.AkashiRepairBar.Value = hpbar.PrevValue + healAmount;

					// todo Akashi repair HP bar changes
					hpbar.ResumeUpdate();
				}
			}
		}
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;

		FleetData? fleet = KCDatabase.Instance.Fleet[FleetId];

		if (ControlFleet != null && fleet != null)
		{
			ControlFleet.ConfigurationChanged();
			ControlFleet.Update(fleet);
		}

		if (ControlMember != null)
		{
			bool showAircraft = c.FormFleet.ShowAircraft;
			bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
			bool shortHPBar = c.FormFleet.ShortenHPBar;
			bool colorMorphing = c.UI.BarColorMorphing;
			System.Drawing.Color[] colorScheme = c.UI.BarColorScheme.Select(col => col.ColorData).ToArray();
			bool showNext = c.FormFleet.ShowNextExp;
			bool showConditionIcon = c.FormFleet.ShowConditionIcon;
			var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
			bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
			int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
			bool isLayoutFixed = c.UI.IsLayoutFixed;

			for (int i = 0; i < ControlMember.Count; i++)
			{
				FleetItemViewModel member = ControlMember[i];

				member.Equipments.ShowAircraft = showAircraft;
				member.Name.MaxWidth = fixShipNameWidth switch
				{
					true => fixedShipNameWidth,
					_ => int.MaxValue,
				};

				member.HP.Text = shortHPBar ? "" : "HP:";
				member.HP.HPBar.ColorMorphing = colorMorphing;
				member.HP.HPBar.SetBarColorScheme(colorScheme);

				member.Level.NextVisible = showNext;
				member.Level.TextNext = showNext ? "next:" : null;

				member.Condition.ImageAlign = showConditionIcon switch
				{
					true => System.Drawing.ContentAlignment.MiddleLeft,
					_ => System.Drawing.ContentAlignment.MiddleCenter,
				};

				member.Equipments.LevelVisibility = levelVisibility;
				member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
				member.ShipResource.BarFuel.ColorMorphing = member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
				member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
				member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

				member.ConfigurationChanged();
				if (fleet != null)
					member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
		}
	}

	private void UpdateShipTagColorList()
	{
		ShipTagColors = GetShipTagColorList();

		// --- Call configuration changed to refresh the ship view 
		ConfigurationChanged();
	}

	/// <summary>
	/// Return Ship tag color list
	/// </summary>
	/// <returns></returns>
	private static List<Color> GetShipTagColorList()
	{
		using ElectronicObserverContext db = new();
		EventLockPlannerModel model = db.EventLockPlans.FirstOrDefault() ?? new();

		List<Color> defaultColors = Configuration.Config.FormFleet.SallyAreaColorScheme.Select(color => color.ColorData).ToList();

		if (model.ShipLocks.Any())
		{
			List<Color> colorsFromLockPlanner = model.Locks
				.Select(eventLock => Color.FromArgb(eventLock.A, eventLock.R, eventLock.G, eventLock.B))
				.ToList();

			List<Color> allColors = new List<Color>();

			// First color isn't used
			if (defaultColors.Any())
			{
				allColors.Add(defaultColors[0]);
				defaultColors.RemoveAt(0);
			}
			else
			{
				allColors.Add(Color.Transparent);
			}

			allColors.AddRange(colorsFromLockPlanner);
			allColors.AddRange(defaultColors);

			return allColors;
		}

		return defaultColors;
	}

	#region Commands

	[RelayCommand]
	private void Copy()
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[FleetId];
		if (fleet == null) return;

		sb.AppendFormat(FormFleet.CopyFleetText + "\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(ControlFleet.BranchWeight), Calculator.GetTpDamage(fleet));
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


		Clipboard.SetDataObject(sb.ToString());
	}

	/// <summary>
	/// 「艦隊デッキビルダー」用編成コピー
	/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
	/// </summary>
	[RelayCommand]
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	private void CopyDeckBuilder(int? areaId)
	{
		IEnumerable<BaseAirCorpsData> airbases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(a => a.MapAreaID == areaId);

		string deckBuilderData = DataSerializationService.DeckBuilder(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = KCDatabase.Instance.Fleet.Fleets.Values.Skip(0).FirstOrDefault(),
			Fleet2 = KCDatabase.Instance.Fleet.Fleets.Values.Skip(1).FirstOrDefault(),
			Fleet3 = KCDatabase.Instance.Fleet.Fleets.Values.Skip(2).FirstOrDefault(),
			Fleet4 = KCDatabase.Instance.Fleet.Fleets.Values.Skip(3).FirstOrDefault(),
			AirBase1 = airbases.Skip(0).FirstOrDefault(),
			AirBase2 = airbases.Skip(1).FirstOrDefault(),
			AirBase3 = airbases.Skip(2).FirstOrDefault(),
		});

		Clipboard.SetDataObject(deckBuilderData);
	}

	/// <summary>
	/// 「艦隊晒しページ」用編成コピー
	/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
	/// </summary>
	[RelayCommand]
	private static void CopyKanmusuList()
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// version
		sb.Append(".2");

		// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
		Dictionary<ShipId, List<ShipData>> shiplist = new();

		foreach (ShipData ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			IShipDataMaster master = ship.MasterShip;
			while (master.RemodelBeforeShip != null)
			{
				master = master.RemodelBeforeShip;
			}

			ShipId shipId = master.ShipId switch
			{
				ShipId.Souya645 or ShipId.Souya650 or ShipId.Souya699 => ShipId.Souya645,
				_ => master.ShipId,
			};

			if (!shiplist.ContainsKey(shipId))
			{
				shiplist.Add(shipId, new List<ShipData> { ship });
			}
			else
			{
				shiplist[shipId].Add(ship);
			}
		}

		// 上で作った分類の各項を文字列化
		foreach ((ShipId shipId, List<ShipData> shipList) in shiplist)
		{
			sb.Append('|').Append((int)shipId).Append(':');

			foreach (ShipData ship in shipList.OrderByDescending(s => s.Level))
			{
				sb.Append(ship.Level);

				// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
				if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
				{
					sb.Append('.');
					int count = 1;
					IShipDataMaster master = ship.MasterShip;
					while (master.RemodelBeforeShip != null)
					{
						master = master.RemodelBeforeShip;
						count++;
					}
					sb.Append(count);
				}
				sb.Append(',');
			}

			// 余った "," を削除
			sb.Remove(sb.Length - 1, 1);
		}

		Clipboard.SetDataObject(sb.ToString());
	}

	[RelayCommand]
	private void CopyFleetAnalysisAllShips()
	{
		string json = DataSerializationService.FleetAnalysisShips(true);

		Clipboard.SetDataObject(json);
	}

	[RelayCommand]
	private void CopyFleetAnalysisLockedShips()
	{
		string json = DataSerializationService.FleetAnalysisShips(false);

		Clipboard.SetDataObject(json);
	}

	[RelayCommand]
	private void CopyFleetAnalysisAllEquip()
	{
		string json = DataSerializationService.FleetAnalysisEquipment(true);

		Clipboard.SetDataObject(json);
	}

	[RelayCommand]
	private void CopyFleetAnalysisLockedEquip()
	{
		string json = DataSerializationService.FleetAnalysisEquipment(false);

		Clipboard.SetDataObject(json);
	}

	[RelayCommand]
	private void CopyFleetAnalysisAllShipsShort()
	{
		GenerateShipListShort(true);
	}

	[RelayCommand]
	private void CopyFleetAnalysisLockedShipsShort()
	{
		GenerateShipListShort(false);
	}

	[RelayCommand]
	private void CopyFleetAnalysisAllEquipShort()
	{
		GenerateEquipListShort(true);
	}

	[RelayCommand]
	private void CopyFleetAnalysisLockedEquipShort()
	{
		GenerateEquipListShort(false);
	}

	/// <summary>
	/// <see cref="https://docs.google.com/spreadsheets/d/1ppbOl9MR_8g_CPDpgMVDdRnhEMRTjg4x78bCg8uLzdg"/>
	/// </summary>
	/// <param name="allShips"></param>
	private void GenerateShipListShort(bool allShips)
	{
		KCDatabase db = KCDatabase.Instance;
		List<string> ships = new List<string>();

		foreach (ShipData ship in db.Ships.Values.Where(s => allShips || s.IsLocked))
		{
			int[] apiKyouka =
			{
				ship.FirepowerModernized,
				ship.TorpedoModernized,
				ship.AAModernized,
				ship.ArmorModernized,
				ship.LuckModernized,
				ship.HPMaxModernized,
				ship.ASWModernized,
			};

			int expProgress = 0;
			if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
			{
				expProgress = (ExpTable.ShipExp[ship.Level].Next - ship.ExpNext) / ExpTable.ShipExp[ship.Level].Next;
			}

			int[] apiExp = { ship.ExpTotal, ship.ExpNext, expProgress };

			string shipId = $"\"id\":{ship.ShipID}";
			string level = $"\"lv\":{ship.Level}";
			string kyouka = $"\"st\":[{string.Join(",", apiKyouka)}]";
			string exp = $"\"exp\":[{string.Join(",", apiExp)}]";

			string[] analysisData = { shipId, level, kyouka, exp };

			ships.Add($"{{{string.Join(",", analysisData)}}}");
		}

		string json = $"[{string.Join(",", ships)}]";

		Clipboard.SetDataObject(json);
	}

	/// <summary>
	/// <see cref="https://docs.google.com/spreadsheets/d/1ppbOl9MR_8g_CPDpgMVDdRnhEMRTjg4x78bCg8uLzdg"/>
	/// </summary>
	/// <param name="allEquipment"></param>
	private void GenerateEquipListShort(bool allEquipment)
	{
		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ
		// pain and suffering

		sb.Append("[");

		foreach (EquipmentData equip in db.Equipments.Values.Where(eq => allEquipment || eq.IsLocked))
		{
			sb.Append($"{{\"id\":{equip.EquipmentID},\"lv\":{equip.Level}}},");
		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append("]");

		Clipboard.SetDataObject(sb.ToString());
	}

	[RelayCommand]
	private void AntiAirDetails()
	{
		AirDefenseViewModel viewModel = new()
		{
			SelectedFleet = (KCDatabase.Instance.Fleet.CombinedFlag != 0 && FleetId is 1 or 2) switch
			{
				true => Tools.AirDefense.FleetId.CombinedFleet,
				_ => (FleetId)FleetId
			}
		};

		new AirDefenseWindow(viewModel).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	private void OutputFleetImage()
	{
		ToolService.FleetImageGenerator(new()
		{
			Fleet1Visible = FleetId is 1,
			Fleet2Visible = FleetId is 2 || (FleetId is 1 && KCDatabase.Instance.Fleet.CombinedFlag > 0),
			Fleet3Visible = FleetId is 3,
			Fleet4Visible = FleetId is 4,
		});
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		ToolService.AirControlSimulator(new AirControlSimulatorViewModel
		{
			Fleet1 = FleetId is 1,
			Fleet2 = FleetId is 2 || (FleetId is 1 && KCDatabase.Instance.Fleet.CombinedFlag > 0),
			Fleet3 = FleetId is 3,
			Fleet4 = FleetId is 4,
		});
	}

	[RelayCommand]
	private void OpenOperationRoom()
	{
		ToolService.OperationRoom(new AirControlSimulatorViewModel(DataSerializationService.OperationRoomLink)
		{
			Fleet1 = FleetId is 1,
			Fleet2 = FleetId is 2 || (FleetId is 1 && KCDatabase.Instance.Fleet.CombinedFlag > 0),
			Fleet3 = FleetId is 3,
			Fleet4 = FleetId is 4,
		});
	}
	#endregion
}
