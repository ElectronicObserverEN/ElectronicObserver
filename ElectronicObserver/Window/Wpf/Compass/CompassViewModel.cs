using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
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
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Wpf.Compass;

public class CompassViewModel : AnchorableViewModel
{
	private KCDatabase Db { get; }
	public FormCompassTranslationViewModel FormCompass { get; }
	public string? TextMapArea { get; set; }
	public string? TextMapAreaToolTip { get; set; }

	public string? TextDestination { get; set; }
	public string? TextDestinationToolTip { get; set; }
	public ImageSource? TextDestinationIcon { get; set; }

	public string? TextEventKindText { get; set; }
	public string? TextEventKindToolTip { get; set; }
	public ImageSource? TextEventKindIcon { get; set; }
	public SolidColorBrush TextEventKindForeColor { get; set; } = new(Colors.White);

	public string? TextEventDetailText { get; set; }
	public string? TextEventDetailToolTip { get; set; }
	public ImageSource? TextEventDetailIcon { get; set; }

	public string? TextEnemyFleetNameText { get; set; }
	public string? TextFormationText { get; set; }
	public string? TextAirSuperiorityText { get; set; }
	public string? TextAirSuperiorityToolTip { get; set; }
	public ImageSource? AirIcon { get; } = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedFighter);

	private List<EnemyFleetRecord.EnemyFleetElement>? _enemyFleetCandidate { get; set; }

	public IEnumerable<EnemyFleetElementViewModel> EnemyFleetCandidate => GetFilteredCandidateFleetViewModels();

	public IEnumerable<MasterShipViewModel>? EnemyFleet { get; set; }

	public BaseViewModel CurrentViewModel { get; set; } = new EmptyViewModel();

	private EmptyViewModel EmptyViewModel { get; } = new();
	private TextViewModel TextViewModel { get; } = new();
	private EnemyListViewModel EnemyListViewModel { get; } = new();
	private BattleViewModel BattleViewModel { get; } = new();

	public CompassViewModel() : base("Compass", "Compass",
		ImageSourceIcons.GetIcon(IconContent.FormCompass))
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
		System.Drawing.Color getColorFromEventKind(int kind)
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
			// BasePanel.Visible = false;
		}
		else if (apiname == "api_req_member/get_practice_enemyinfo")
		{
			TextMapArea = GeneralRes.Practice;
			TextDestination = string.Format("{0} {1}", data.api_nickname, Constants.GetAdmiralRank((int)data.api_rank));
			// TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
			// TextDestination.ImageIndex = -1;
			TextDestinationIcon = null;
			TextDestinationToolTip = null;
			TextEventKindText = data.api_cmt;
			TextEventKindForeColor = getColorFromEventKind(0).ToBrush();
			// TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
			// TextEventKind.ImageIndex = -1;
			TextEventKindIcon = null;
			TextEventKindToolTip = null;
			TextEventDetailText = string.Format("Lv. {0} / {1} exp.", data.api_level, data.api_experience[0]);
			// TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
			// TextEventDetail.ImageIndex = -1;
			TextEventDetailIcon = null;
			TextEventDetailToolTip = null;
			TextEnemyFleetNameText = data.api_deckname;
		}
		else
		{
			CompassData compass = Db.Battle.Compass;
			/*


			BasePanel.SuspendLayout();
			PanelEnemyFleet.Visible = false;
			PanelEnemyCandidate.Visible = false;

			_enemyFleetCandidate = null;
			_enemyFleetCandidateIndex = -1;
			*/
			/*
			TextMapArea.Text = string.Format(GeneralRes.Map + ": {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
				compass.MapInfo.EventDifficulty > 0 ? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]" : "");
			*/

			// ex: world 5-5
			TextMapArea = string.Format(GeneralRes.Map + ": {0}-{1}{2}", compass.MapAreaID, compass.MapInfoID,
				compass.MapInfo.EventDifficulty > 0
					? " [" + Constants.GetDifficulty(compass.MapInfo.EventDifficulty) + "]"
					: "");

			// map HP, touch trigger tooltip
			{
				var mapinfo = compass.MapInfo;
				var sb = new StringBuilder();

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
				/*
				ToolTipInfo.SetToolTip(TextMapArea, sb.Length > 0 ? sb.ToString() : null);
				*/

				TextMapAreaToolTip = sb.Length switch
				{
					> 0 => sb.ToString(),
					_ => null
				};
			}

			// ex: node 1
			TextDestination = string.Format(GeneralRes.NextNode + ": {0}{1}", compass.DestinationID, (compass.IsEndPoint ? GeneralRes.EndNode : ""));

			if (compass.LaunchedRecon != 0)
			{
				/*
				TextDestination.ImageAlign = ContentAlignment.MiddleRight;
				TextDestination.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;
				*/

				TextDestinationIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane);
				TextDestinationToolTip = compass.CommentID switch
				{
					1 => FormCompass.EnemySighted,
					2 => FormCompass.TargetSighted,
					3 => FormCompass.CoursePatrol,
					_ => FormCompass.EnemyPlaneSighted
				};


			}
			else
			{
				/*
				TextDestination.ImageAlign = ContentAlignment.MiddleCenter;
				TextDestination.ImageIndex = -1;
				*/
				TextDestinationIcon = null;
				TextDestinationToolTip = null;
				// ToolTipInfo.SetToolTip(TextDestination, null);
			}
			/*
			//とりあえずリセット
			TextEventDetail.ImageAlign = ContentAlignment.MiddleCenter;
			TextEventDetail.ImageIndex = -1;
			ToolTipInfo.SetToolTip(TextEventDetail, null);

			*/

			TextEventDetailIcon = null;
			TextEventDetailToolTip = null;

			CurrentViewModel = TextViewModel;

			TextEventKindForeColor = getColorFromEventKind(0).ToBrush();
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
								switch (compass.WhirlpoolItemID)
								{
									case 1:
										return s.Fuel;
									case 2:
										return s.Ammo;
									default:
										return 0;
								}
							});

						TextEventDetailText = string.Format("{0} x {1} ({2:p0})",
							Constants.GetMaterialName(compass.WhirlpoolItemID),
							compass.WhirlpoolItemAmount,
							(double)compass.WhirlpoolItemAmount / Math.Max(materialmax, 1));

						Utility.Logger.Add(2, $"{compass.MapAreaID}-{compass.MapInfoID}-{compass.DestinationID} {eventkind} {TextEventDetailText}");
					}
					break;

					case 4:     //通常戦闘
						if (compass.EventKind >= 2)
						{
							eventkind += "/" + Constants.GetMapEventKind(compass.EventKind);

							TextEventKindForeColor = getColorFromEventKind(compass.EventKind).ToBrush();
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
						TextEventKindForeColor = getColorFromEventKind(compass.EventKind).ToBrush();

						switch (compass.EventKind)
						{
							case 0:     //航空偵察
								eventkind = GeneralRes.AerialRecon;

								TextEventDetailText = compass.AirReconnaissanceResult switch
								{
									0 => GeneralRes.Failure,
									1 => GeneralRes.Success,
									2 => GeneralRes.GreatSuccess,
									_ => GeneralRes.Failure
								};

								TextEventDetailIcon = compass.AirReconnaissancePlane switch
								{
									0 => null,
									1 => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.FlyingBoat),
									2 => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane),
									_ => null
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
				// TextEventKind.ImageAlign = ContentAlignment.MiddleRight;
				// TextEventKind.ImageIndex = (int)ResourceManager.EquipmentContent.CarrierBasedBomber;
				// ToolTipInfo.SetToolTip(TextEventKind, "Air raid - " + Constants.GetAirRaidDamage(compass.AirRaidDamageKind));
				TextEventKindIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedBomber);
				TextEventKindToolTip = FormCompass.AirRaid + Constants.GetAirRaidDamage(compass.AirRaidDamageKind);
			}
			else if (Db.Battle.HeavyBaseAirRaids.Any())
			{
				int apiLostKind = (int)Db.Battle.HeavyBaseAirRaids.Last().RawData.api_lost_kind;

				TextEventKindIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.CarrierBasedBomber);
				TextEventKindToolTip = FormCompass.AirRaid + Constants.GetAirRaidDamage(apiLostKind);
			}
			else
			{
				// TextEventKind.ImageAlign = ContentAlignment.MiddleCenter;
				// TextEventKind.ImageIndex = -1;
				// ToolTipInfo.SetToolTip(TextEventKind, null);
				TextEventKindIcon = null;
				TextEventKindToolTip = null;
			}

			/*
			BasePanel.ResumeLayout();

			BasePanel.Visible = true;
			*/
		}
	}

	private void UpdateEnemyFleet()
	{
		CompassData compass = Db.Battle.Compass;

		CurrentViewModel = EnemyListViewModel;

		_enemyFleetCandidate = RecordManager.Instance.EnemyFleet.Record.Values.Where(
			r =>
				r.MapAreaID == compass.MapAreaID &&
				r.MapInfoID == compass.MapInfoID &&
				r.CellID == compass.Destination &&
				r.Difficulty == compass.MapInfo.EventDifficulty
		).ToList();
		// _enemyFleetCandidateIndex = 0;


		if (_enemyFleetCandidate.Count == 0)
		{
			TextEventDetailText = GeneralRes.NoFleetCandidates;
			// TextEnemyFleetName.Text = GeneralRes.EnemyUnknown;


			// TableEnemyCandidate.Visible = false;

		}
		else
		{
			_enemyFleetCandidate.Sort((a, b) =>
			{
				for (int i = 0; i < a.FleetMember.Length; i++)
				{
					int diff = a.FleetMember[i] - b.FleetMember[i];
					if (diff != 0)
						return diff;
				}
				return a.Formation - b.Formation;
			});

			NextEnemyFleetCandidate(0);
		}


		// PanelEnemyFleet.Visible = false;

	}

	private void NextEnemyFleetCandidate(int offset)
	{
		if (_enemyFleetCandidate == null || _enemyFleetCandidate.Count == 0) return;

		/*
			_enemyFleetCandidateIndex += offset;
		if (_enemyFleetCandidateIndex < 0)
			_enemyFleetCandidateIndex = (_enemyFleetCandidate.Count - 1) - (_enemyFleetCandidate.Count - 1) % _candidatesDisplayCount;
		else if (_enemyFleetCandidateIndex >= _enemyFleetCandidate.Count)
			_enemyFleetCandidateIndex = 0;
		*/

		/*

		var candidate = _enemyFleetCandidate[_enemyFleetCandidateIndex];

		*/
		TextEventDetailText = /*TextEnemyFleetName.Text =*/ KCDatabase.Instance.Translation.Operation.FleetName(_enemyFleetCandidate.First().FleetName);
		/*
		if (_enemyFleetCandidate.Count > _candidatesDisplayCount)
		{
			TextEventDetail.Text += " ▼";
			ToolTipInfo.SetToolTip(TextEventDetail, string.Format("Fleets: {0} / {1}\r\n(left or right click to change pages)\r\n", _enemyFleetCandidateIndex + 1, _enemyFleetCandidate.Count));
		}
		else
		{
			ToolTipInfo.SetToolTip(TextEventDetail, string.Format("Fleets: {0}\r\n", _enemyFleetCandidate.Count));
		}
		*/
		TextEventDetailToolTip = string.Format(FormCompass.FleetCount + "\r\n", _enemyFleetCandidate.Count);
		/*
		TableEnemyCandidate.SuspendLayout();
		for (int i = 0; i < ControlCandidates.Length; i++)
		{
			if (i + _enemyFleetCandidateIndex >= _enemyFleetCandidate.Count || i >= _candidatesDisplayCount)
			{
				ControlCandidates[i].Update(null);
				continue;
			}

			ControlCandidates[i].Update(_enemyFleetCandidate[i + _enemyFleetCandidateIndex]);
		}
		TableEnemyCandidate.ResumeLayout();
		TableEnemyCandidate.Visible = true;

		PanelEnemyCandidate.Visible = true;
		*/
	}

	private List<EnemyFleetElementViewModel>? GetAllRecordCandidateFleetViewModels()
		=> _enemyFleetCandidate?
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
			1 => new List<int>() { 0 },
			2 => new List<int>() { 0, 0, 0 },
			_ => new()
		};

		previewShipIds.AddRange(padIds);
		previewShipIds.AddRange(Enumerable.Repeat(-1, 6));

		return previewShipIds.Take(6).ToList();
	}

	private string GetMaterialInfo(CompassData compass)
	{

		var strs = new LinkedList<string>();

		foreach (var item in compass.GetItems)
		{

			string itemName;

			if (item.ItemID == 4)
			{
				itemName = Constants.GetMaterialName(item.Metadata);

			}
			else
			{
				var itemMaster = KCDatabase.Instance.MasterUseItems[item.Metadata];
				if (itemMaster != null)
					itemName = itemMaster.NameTranslated;
				else
					itemName = FormCompass.UnknownItem;
			}

			strs.AddLast(itemName + " x " + item.Amount);
		}

		if (!strs.Any()) return FormCompass.None;

		string materialInfo = string.Join(", ", strs);

		Utility.Logger.Add(2, $"{compass.MapAreaID}-{compass.MapInfoID}-{compass.DestinationID} {materialInfo}");

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
		/*
		_enemyFleetCandidateIndex = -1;
		*/


		if (!bm.IsPractice)
		{
			var efcurrent = EnemyFleetRecord.EnemyFleetElement.CreateFromCurrentState();
			var efrecord = RecordManager.Instance.EnemyFleet[efcurrent.FleetID];
			if (efrecord != null)
			{
				TextEnemyFleetNameText = KCDatabase.Instance.Translation.Operation.FleetName(efrecord.FleetName);
				TextEventDetailText = "Exp: " + efrecord.ExpShip;
			}
			TextEventDetailToolTip = GeneralRes.EnemyFleetID + ": " + efcurrent.FleetID.ToString("x16");
		}
		TextFormationText = Constants.GetFormationShort(bd.Searching.FormationEnemy);
		//TextFormation.ImageIndex = (int)ResourceManager.IconContent.BattleFormationEnemyLineAhead + bd.Searching.FormationEnemy - 1;
		/*
		TextFormation.Visible = true;
		*/
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
		/*
		TableEnemyMember.SuspendLayout();
		
		for (int i = 0; i < ControlMembers.Length; i++)
		{
			int shipID = enemies[i];
			ControlMembers[i].Update(shipID, shipID != -1 ? slots[i] : null);

			if (shipID != -1)
				ControlMembers[i].UpdateEquipmentToolTip(shipID, slots[i], levels[i], hps[i], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3]);
		}
		
		TableEnemyMember.ResumeLayout();
		TableEnemyMember.Visible = true;

		PanelEnemyFleet.Visible = true;

		PanelEnemyCandidate.Visible = false;

		BasePanel.Visible = true;           //checkme
		*/

		// int hp, int firepower, int torpedo, int aa, int armor
		// hps[i], parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3]
		EnemyFleet = enemies.Select((shipId, i) => new MasterShipViewModel
		{
			Ship = shipId switch
			{
				> 0 => Db.MasterShips[shipId],
				_ => null
			},
			Slot = slots[i],
			Level = levels[i],
			Hp = hps[i],
			Firepower = parameters[i][0],
			Torpedo = parameters[i][1],
			Aa = parameters[i][2],
			Armor = parameters[i][3],
		})
			.Take(6);
	}

	private string? GetAirSuperiorityString(int air)
	{
		if (air > 0)
		{
			return string.Format(FormCompass.AirValues,
				(int)(air * 3.0),
				(int)Math.Ceiling(air * 1.5),
				(int)(air / 1.5 + 1),
				(int)(air / 3.0 + 1));
		}
		return null;
	}

	void ConfigurationChanged()
	{

		/*
		BasePanel.AutoScroll = Utility.Configuration.Config.FormCompass.IsScrollable;

		_candidatesDisplayCount = Utility.Configuration.Config.FormCompass.CandidateDisplayCount;
		_enemyFleetCandidateIndex = 0;
		if (PanelEnemyCandidate.Visible)
			NextEnemyFleetCandidate(0);

		if (ControlMembers != null)
		{
			TableEnemyMember.SuspendLayout();

			TableEnemyMember.Location = new Point(TableEnemyMember.Location.X, TableEnemyFleet.Bottom + 6);

			bool flag = Utility.Configuration.Config.FormFleet.ShowAircraft;
			for (int i = 0; i < ControlMembers.Length; i++)
			{
				ControlMembers[i].Equipments.ShowAircraft = flag;
				ControlMembers[i].ConfigurationChanged();
			}

			ControlHelper.SetTableRowStyles(TableEnemyMember, ControlHelper.GetDefaultRowStyle());
			TableEnemyMember.ResumeLayout();
		}


		if (ControlCandidates != null)
		{
			TableEnemyCandidate.SuspendLayout();

			for (int i = 0; i < ControlCandidates.Length; i++)
				ControlCandidates[i].ConfigurationChanged();

			ControlHelper.SetTableRowStyles(TableEnemyCandidate, new RowStyle(SizeType.AutoSize));
			ControlHelper.SetTableColumnStyles(TableEnemyCandidate, ControlHelper.GetDefaultColumnStyle());
			TableEnemyCandidate.ResumeLayout();
		}
		*/
	}
}
