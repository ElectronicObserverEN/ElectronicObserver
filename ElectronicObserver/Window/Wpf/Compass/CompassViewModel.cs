using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.Compass.ViewModels;

namespace ElectronicObserver.Window.Wpf.Compass;

public class CompassViewModel : AnchorableViewModel
{
	private KCDatabase Db { get; }
	public FormCompassTranslationViewModel FormCompass { get; }
	public string? TextMapArea { get; set; }
	public string? TextMapAreaToolTip { get; set; }

	public string? TextDestination { get; set; }
	public string? TextDestinationToolTip { get; set; }
	public EquipmentIconType? TextDestinationIcon { get; set; }

	public string? TextEventKindText { get; set; }
	public string? TextEventKindToolTip { get; set; }
	public EquipmentIconType? TextEventKindIcon { get; set; }
	public SolidColorBrush TextEventKindForeColor { get; set; } = new(Colors.White);

	public string? TextEventDetailText { get; set; }
	public string? TextEventDetailToolTip { get; set; }
	public EquipmentIconType? TextEventDetailIcon { get; set; }

	public string? TextEnemyFleetNameText { get; set; }
	public string? TextFormationText { get; set; }
	public string? TextAirSuperiorityText { get; set; }
	public string? TextAirSuperiorityToolTip { get; set; }
	public EquipmentIconType? AirIcon { get; } = EquipmentIconType.CarrierBasedFighter;

	private List<EnemyFleetRecord.EnemyFleetElement>? _enemyFleetCandidate { get; set; }

	public IEnumerable<EnemyFleetElementViewModel> EnemyFleetCandidate => GetFilteredCandidateFleetViewModels();

	public IEnumerable<MasterShipViewModel>? EnemyFleet { get; set; }

	public BaseViewModel CurrentViewModel { get; set; } = new EmptyViewModel();

	private EmptyViewModel EmptyViewModel { get; } = new();
	private TextViewModel TextViewModel { get; } = new();
	private EnemyListViewModel EnemyListViewModel { get; } = new();
	private BattleViewModel BattleViewModel { get; } = new();

	public CompassViewModel() : base("Compass", "Compass", IconContent.FormCompass)
	{
		Db = KCDatabase.Instance;
		FormCompass = Ioc.Default.GetService<FormCompassTranslationViewModel>()!;

		Title = FormCompass.Title;
		FormCompass.PropertyChanged += (_, _) => Title = FormCompass.Title;

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiReqMap_AirRaid.ResponseReceived += Updated;
		o.ApiReqMember_GetPracticeEnemyInfo.ResponseReceived += Updated;

		o.ApiReqSortie_Battle.ResponseReceived += BattleStarted;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqSortie_NightToDay.ResponseReceived += BattleStarted;
		o.ApiReqSortie_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqSortie_LdShooting.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += BattleStarted;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += BattleStarted;
		o.ApiReqPractice_Battle.ResponseReceived += BattleStarted;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void Updated(string apiname, dynamic data)
	{
		System.Drawing.Color GetColorFromEventKind(int kind)
		{
			switch (kind)
			{
				case 0:
				case 1:
				default: //昼夜戦・その他
					return Utility.Configuration.Config.UI.ForeColor;
				case 2:
				case 3: //夜戦・夜昼戦
					return Utility.Configuration.Config.UI.Compass_ColorTextEventKind3;
				case 4: //航空戦
				case 6: //長距離空襲戦
					return Utility.Configuration.Config.UI.Compass_ColorTextEventKind6;
				case 5: // 敵連合
					return Utility.Configuration.Config.UI.Compass_ColorTextEventKind5;
				case 7: // 夜昼戦(対連合艦隊)
					return Utility.Configuration.Config.UI.Compass_ColorTextEventKind3;
				case 8: // レーダー射撃
					return Utility.Configuration.Config.UI.Compass_ColorTextEventKind3;
			}
		}

		if (apiname == "api_port/port")
		{
			CurrentViewModel = EmptyViewModel;
		}
		else if (apiname == "api_req_member/get_practice_enemyinfo")
		{
			TextMapArea = GeneralRes.Practice;
			TextDestination = string.Format("{0} {1}", data.api_nickname, Constants.GetAdmiralRank((int)data.api_rank));
			TextDestinationIcon = null;
			TextDestinationToolTip = null;
			TextEventKindText = data.api_cmt;
			TextEventKindForeColor = GetColorFromEventKind(0).ToBrush();
			TextEventKindIcon = null;
			TextEventKindToolTip = null;
			TextEventDetailText = string.Format("Lv. {0} / {1} exp.", data.api_level, data.api_experience[0]);
			TextEventDetailIcon = null;
			TextEventDetailToolTip = null;
			TextEnemyFleetNameText = data.api_deckname;
		}
		else
		{
			CompassData compass = Db.Battle.Compass;

			// ex: world 5-5
			TextMapArea = string.Format(GeneralRes.Map + ": {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
				compass.MapInfo.EventDifficulty > 0
					? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]"
					: "");

			// map HP, touch trigger tooltip
			{
				IMapInfoData mapinfo = compass.MapInfo;
				StringBuilder sb = new();

				if (mapinfo.RequiredDefeatedCount != -1 &&
					mapinfo.CurrentDefeatedCount < mapinfo.RequiredDefeatedCount)
				{
					sb.AppendFormat(FormCompass.MapClearCount + "\r\n",
						mapinfo.CurrentGaugeIndex > 0 ? $"#{mapinfo.CurrentGaugeIndex} " : "",
						mapinfo.CurrentDefeatedCount, mapinfo.RequiredDefeatedCount);

				}
				else if (mapinfo.MapHPMax > 0)
				{
					int current = compass.MapHPCurrent > 0 ? compass.MapHPCurrent : mapinfo.MapHPCurrent;
					int max = compass.MapHPMax > 0 ? compass.MapHPMax : mapinfo.MapHPMax;

					sb.AppendFormat("{0}{1}: {2} / {3}\r\n",
						mapinfo.CurrentGaugeIndex > 0 ? $"#{mapinfo.CurrentGaugeIndex} " : "",
						mapinfo.GaugeType == 3 ? "TP" : "HP", current, max);
				}


				foreach (var pair in KCDatabase.Instance.Battle.SpecialAttackCount)
				{
					sb.AppendLine($"{DayAttack.AttackDisplay((DayAttackKind)pair.Key)} : {FormCompass.SpecialAttackActivated}");
				}

				TextMapAreaToolTip = sb.Length switch
				{
					> 0 => sb.ToString(),
					_ => null,
				};
			}

			// ex: node 1
			TextDestination = $"{GeneralRes.NextNode}: {compass.CellDisplayWithId}";

			if (compass.IsEndPoint)
			{
				TextDestination += GeneralRes.EndNode;
			}

			if (compass.LaunchedRecon != 0)
			{
				TextDestinationIcon = EquipmentIconType.Seaplane;
				TextDestinationToolTip = compass.CommentID switch
				{
					1 => FormCompass.EnemySighted,
					2 => FormCompass.TargetSighted,
					3 => FormCompass.CoursePatrol,
					_ => FormCompass.EnemyPlaneSighted,
				};
			}
			else
			{
				TextDestinationIcon = null;
				TextDestinationToolTip = null;
			}

			TextEventDetailIcon = null;
			TextEventDetailToolTip = null;

			CurrentViewModel = TextViewModel;

			TextEventKindForeColor = GetColorFromEventKind(0).ToBrush();
			{
				string eventkind = Constants.GetMapEventID(compass.EventID);

				switch (compass.EventID)
				{
					case 0:     //初期位置
						TextEventDetailText = GeneralRes.WhyDidThisHappen;
						break;

					case 2:     //資源
						TextEventKindForeColor = Utility.Configuration.Config.UI.Color_Green.ToBrush();
						TextEventDetailText = GetMaterialInfo(compass);
						break;
					case 8:     //船団護衛成功
						TextEventDetailText = GetMaterialInfo(compass);
						break;

					case 3:     //渦潮
					{
						int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
							.Where(f => f != null && f.IsInSortie)
							.SelectMany(f => f.MembersWithoutEscaped)
							.Max(s =>
							{
								if (s == null) return 0;

								return compass.WhirlpoolItemID switch
								{
									1 => s.Fuel,
									2 => s.Ammo,
									_ => 0,
								};
							});

						TextEventDetailText = string.Format("{0} x {1} ({2:p0})",
							Constants.GetMaterialName(compass.WhirlpoolItemID),
							compass.WhirlpoolItemAmount,
							(double)compass.WhirlpoolItemAmount / Math.Max(materialmax, 1));

						Utility.Logger.Add(2, $"{compass.MapAreaID}-{compass.MapInfoID}-{compass.CellDisplay} {eventkind} {TextEventDetailText}");
					}
					break;

					case 4:     //通常戦闘
						if (compass.EventKind >= 2)
						{
							eventkind += "/" + Constants.GetMapEventKind(compass.EventKind);

							TextEventKindForeColor = GetColorFromEventKind(compass.EventKind).ToBrush();
						}
						UpdateEnemyFleet();
						break;

					case 5:     //ボス戦闘
						TextEventKindForeColor = Utility.Configuration.Config.UI.Color_Red.ToBrush();

						if (compass.EventKind >= 2)
						{
							eventkind += "/" + Constants.GetMapEventKind(compass.EventKind);
						}
						UpdateEnemyFleet();
						break;

					case 1:     //イベントなし
					case 6:     //気のせいだった
						switch (compass.EventKind)
						{

							case 0:     //気のせいだった
							default:
								break;
							case 1:
								eventkind = FormCompass.NoEnemySighted;
								break;
							case 2:
								eventkind = FormCompass.BranchChoice;
								break;
							case 3:
								eventkind = FormCompass.CalmSea;
								break;
							case 4:
								eventkind = FormCompass.CalmStrait;
								break;
							case 5:
								eventkind = FormCompass.NeedToBeCareful;
								break;
							case 6:
								eventkind = FormCompass.CalmSea2;
								break;
						}

						if (compass.RouteChoices != null)
						{
							TextEventDetailText = string.Join(FormCompass.BranchChoiceSeparator, compass.RouteChoicesDisplay);
						}
						else if (compass.FlavorTextType != -1)
						{
							TextEventDetailText = "◆";
							TextEventDetailToolTip = compass.FlavorText;
						}
						else
						{
							TextEventDetailText = "";
						}

						break;

					case 7:     //航空戦or航空偵察
						TextEventKindForeColor = GetColorFromEventKind(compass.EventKind).ToBrush();

						switch (compass.EventKind)
						{
							case 0:     //航空偵察
								eventkind = GeneralRes.AerialRecon;

								TextEventDetailText = compass.AirReconnaissanceResult switch
								{
									0 => GeneralRes.Failure,
									1 => GeneralRes.Success,
									2 => GeneralRes.GreatSuccess,
									_ => GeneralRes.Failure,
								};

								TextEventDetailIcon = compass.AirReconnaissancePlane switch
								{
									0 => null,
									1 => EquipmentIconType.FlyingBoat,
									2 => EquipmentIconType.Seaplane,
									_ => null,
								};

								if (compass.GetItems.Any())
								{
									TextEventDetailText += "　" + GetMaterialInfo(compass);
								}

								break;

							case 4:     //航空戦
							default:
								UpdateEnemyFleet();
								break;
						}
						break;

					case 9:     //揚陸地点
						TextEventDetailText = "";
						break;

					case 10:    // 泊地
						TextEventDetailText = compass.CanEmergencyAnchorageRepair ? FormCompass.RepairPossibility : "";
						break;

					default:
						TextEventDetailText = "";
						break;

				}
				TextEventKindText = eventkind;
			}

			if (compass.HasAirRaid)
			{
				TextEventKindIcon = EquipmentIconType.CarrierBasedBomber;
				TextEventKindToolTip = FormCompass.AirRaid + Constants.GetAirRaidDamage(compass.AirRaidDamageKind);
			}
			else if (Db.Battle.HeavyBaseAirRaids.Any())
			{
				int apiLostKind = (int)Db.Battle.HeavyBaseAirRaids.Last().RawData.api_lost_kind;

				TextEventKindIcon = EquipmentIconType.CarrierBasedBomber;
				TextEventKindToolTip = FormCompass.AirRaid + Constants.GetAirRaidDamage(apiLostKind);
			}
			else
			{
				TextEventKindIcon = null;
				TextEventKindToolTip = null;
			}
		}
	}

	private void UpdateEnemyFleet()
	{
		CompassData compass = Db.Battle.Compass;

		CurrentViewModel = EnemyListViewModel;

		_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values
			.Where(r =>
				r.MapAreaID == compass.MapAreaID &&
				r.MapInfoID == compass.MapInfoID &&
				r.CellID == compass.CellId &&
				r.Difficulty == compass.MapInfo.EventDifficulty)
			.ToList();

		if (_enemyFleetCandidate.Count == 0)
		{
			TextEventDetailText = GeneralRes.NoFleetCandidates;
		}
		else
		{
			_enemyFleetCandidate.Sort((a, b) =>
			{
				for (int i = 0; i < a.FleetMember.Length; i++)
				{
					int diff = a.FleetMember[i] - b.FleetMember[i];

					if (diff != 0)
					{
						return diff;
					}
				}

				return a.Formation - b.Formation;
			});

			NextEnemyFleetCandidate(0);
		}
	}

	private void NextEnemyFleetCandidate(int offset)
	{
		if (_enemyFleetCandidate == null || _enemyFleetCandidate.Count == 0) return;

		TextEventDetailText = KCDatabase.Instance.Translation.Operation.FleetName(_enemyFleetCandidate.First().FleetName);

		TextEventDetailToolTip = string.Format(FormCompass.FleetCount + "\r\n", _enemyFleetCandidate.Count);
	}

	private List<EnemyFleetElementViewModel>? GetAllRecordCandidateFleetViewModels() => _enemyFleetCandidate?
		.GroupBy(f => f, new EnemyFleetElementComparer())
		.Select(f => new EnemyFleetElementViewModel
		{
			EnemyFleetCandidate = f.First(),
			Formations = f.Select(fleet => fleet.Formation).ToList()
		})
		.ToList();

	private List<EnemyFleetElementViewModel> GetFilteredCandidateFleetViewModels()
	{
		List<EnemyFleetElementViewModel> allCandidates = GetAllRecordCandidateFleetViewModels() ?? new();

		if (!allCandidates.Any(vm => vm.IsPreviewed) && MakeEnemyFleetFromPreview() is EnemyFleetElementViewModel previewFleet)
		{
			allCandidates.Add(previewFleet);
		}

		if (Utility.Configuration.Config.FormCompass.DisplayAllEnemyCompositions) return allCandidates;

		List<EnemyFleetElementViewModel> filteredCandidates = allCandidates.Where(vm => vm.IsPreviewed).ToList();

		return filteredCandidates.Count switch
		{
			0 => allCandidates,
			_ => filteredCandidates
		};
	}

	private EnemyFleetElementViewModel? MakeEnemyFleetFromPreview()
	{
		if (Db.Battle.Compass is null) return null;
		if (!Db.Battle.Compass.EnemyFleetPreview.Any()) return null;

		List<EDeckInfo> previewFleet = Db.Battle.Compass.EnemyFleetPreview;

		List<int> fleetIds = MakeEnemyFleetPartFromPreview(previewFleet.First());
		fleetIds.AddRange(MakeEnemyFleetPartFromPreview(previewFleet.Count > 1 ? previewFleet[1] : null));

		EnemyFleetElementViewModel vm = new EnemyFleetElementViewModel
		{
			EnemyFleetCandidate = new EnemyFleetRecord.EnemyFleetElement("", 0, 0, 0, 0, 0, fleetIds.ToArray(), new int[12], 0),
			Formations = new() { 0 }
		};

		return vm;
	}

	private List<int> MakeEnemyFleetPartFromPreview(EDeckInfo? fleetPart)
	{
		if (fleetPart is null) return Enumerable.Repeat(-1, 6).ToList();

		List<int> previewShipIds = fleetPart
			.ApiShipIds
			.Cast<int>()
			.ToList();

		List<int> padIds = fleetPart.ApiKind switch
		{
			1 => new List<int> { 0 },
			2 => new List<int> { 0, 0, 0 },
			_ => new(),
		};

		previewShipIds.AddRange(padIds);
		previewShipIds.AddRange(Enumerable.Repeat(-1, 6));

		return previewShipIds.Take(6).ToList();
	}

	private string GetMaterialInfo(CompassData compass)
	{
		LinkedList<string> strs = new();

		foreach (CompassData.GetItemData item in compass.GetItems)
		{
			string itemName;

			if (item.ItemID == 4)
			{
				itemName = Constants.GetMaterialName(item.Metadata);

			}
			else
			{
				IUseItemMaster? itemMaster = KCDatabase.Instance.MasterUseItems[item.Metadata];

				itemName = itemMaster switch
				{
					{ } => itemMaster.NameTranslated,
					_ => FormCompass.UnknownItem,
				};
			}

			strs.AddLast(itemName + " x " + item.Amount);
		}

		if (!strs.Any()) return FormCompass.None;

		string materialInfo = string.Join(", ", strs);

		Utility.Logger.Add(2, $"{compass.MapAreaID}-{compass.MapInfoID}-{compass.CellDisplay} {materialInfo}");

		return materialInfo;
	}

	private void BattleStarted(string apiname, dynamic data)
	{
		UpdateEnemyFleetInstant(apiname.Contains("practice"));
	}

	private void UpdateEnemyFleetInstant(bool isPractice = false)
	{
		CurrentViewModel = BattleViewModel;

		BattleManager bm = KCDatabase.Instance.Battle;
		BattleData bd = bm.FirstBattle;

		int[] enemies = bd.Initial.EnemyMembers;
		int[][] slots = bd.Initial.EnemySlots;
		int[] levels = bd.Initial.EnemyLevels;
		int[][] parameters = bd.Initial.EnemyParameters;
		int[] hps = bd.Initial.EnemyMaxHPs;

		_enemyFleetCandidate = null;

		if (!bm.IsPractice)
		{
			EnemyFleetRecord.EnemyFleetElement efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
			EnemyFleetRecord.EnemyFleetElement efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
			if (efrecord != null)
			{
				TextEnemyFleetNameText = KCDatabase.Instance.Translation.Operation.FleetName(efrecord.FleetName);
				TextEventDetailText = "Exp: " + efrecord.ExpShip;
			}
			TextEventDetailToolTip = GeneralRes.EnemyFleetID + ": " + efcurrent.FleetID.ToString("x16");
		}

		TextFormationText = Constants.GetFormationShort(bd.Searching.FormationEnemy);

		{
			int air = Calculator.GetAirSuperiority(enemies, slots);
			TextAirSuperiorityText = isPractice ?
				air.ToString() + " ～ " + Calculator.GetAirSuperiorityAtMaxLevel(enemies, slots).ToString() :
				air.ToString();

			if (enemies.Select(id => KCDatabase.Instance.MasterShips[id])
				.Any(ship => ship != null && RecordManager.Instance.ShipParameter[ship.ShipID]?.Aircraft == null))
				TextAirSuperiorityText += "?";

			TextAirSuperiorityToolTip = GetAirSuperiorityString(isPractice ? 0 : air);
		}

		EnemyFleet = enemies
			.Select((shipId, i) => new MasterShipViewModel
			{
				Ship = shipId switch
				{
					> 0 => Db.MasterShips[shipId],
					_ => null,
				},
				Slot = slots[i],
				Level = levels[i],
				Hp = hps[i],
				Firepower = parameters[i][0],
				Torpedo = parameters[i][1],
				Aa = parameters[i][2],
				Armor = parameters[i][3],
			}).Take(6);
	}

	private string? GetAirSuperiorityString(int air) => air switch
	{
		> 0 => string.Format(FormCompass.AirValues,
			(int)(air * 3.0),
			(int)Math.Ceiling(air * 1.5),
			(int)(air / 1.5 + 1),
			(int)(air / 3.0 + 1)),

		_ => null,
	};

	private void ConfigurationChanged()
	{
		// no idea
	}
}
