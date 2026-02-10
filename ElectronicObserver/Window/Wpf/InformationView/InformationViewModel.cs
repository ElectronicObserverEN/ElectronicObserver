using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.DiscordRPC;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.InformationView;

public class InformationViewModel : AnchorableViewModel
{
	public FormInformationTranslationViewModel FormInformation { get; set; }
	private int _ignorePort;
	private List<int>? _inSortie;
	private int[] _prevResource;

	public string? Text { get; set; }
	public InformationViewModel() : base("Info", "Information", IconContent.FormInformation)
	{
		FormInformation = Ioc.Default.GetService<FormInformationTranslationViewModel>()!;
		Title = FormInformation.Title;
		FormInformation.PropertyChanged += (_, _) => Title = FormInformation.Title;
		_ignorePort = 0;
		_inSortie = null;
		_prevResource = new int[4];

		APIObserver o = APIObserver.Instance;
		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiReqMember_GetPracticeEnemyInfo.ResponseReceived += Updated;
		o.ApiGetMember_PictureBook.ResponseReceived += Updated;
		o.ApiGetMember_MapInfo.ResponseReceived += Updated;
		o.ApiGetMember_Mission.ResponseReceived += Updated;
		o.ApiReqMission_Result.ResponseReceived += Updated;
		o.ApiReqPractice_BattleResult.ResponseReceived += Updated;
		o.ApiReqSortie_BattleResult.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += Updated;
		o.ApiReqHokyu_Charge.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiReqPractice_Battle.ResponseReceived += Updated;
		o.ApiGetMember_SortieConditions.ResponseReceived += Updated;
		o.ApiGetMember_QuestList.ResponseReceived += (_, _) => QuestListUpdated();
		o.ApiReqMission_Start.RequestReceived += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
	}

	private void Updated(string apiname, dynamic data)
	{

		switch (apiname)
		{

			case "api_port/port":
				if (_ignorePort > 0)
					_ignorePort--;
				else
					Text = "";      //とりあえずクリア

				if (_inSortie != null)
				{
					Text = GetConsumptionResource(data);
				}
				_inSortie = null;

				RecordMaterials();

				// '16 summer event
				if (data.api_event_object() && data.api_event_object.api_m_flag2() && (int)data.api_event_object.api_m_flag2 > 0)
				{
					Text += $"\r\n{InformationResources.GimmickRelease}\r\n";
					Utility.Logger.Add(2, InformationResources.EnemyWasWeakened);
				}
				break;

			case "api_req_member/get_practice_enemyinfo":
				Text = GetPracticeEnemyInfo(data);
				RecordMaterials();
				break;

			case "api_get_member/picture_book":
				Text = GetAlbumInfo(data);
				break;

			case "api_get_member/mapinfo":
				Text = GetMapGauge(data);
				break;

			case "api_get_member/mission":
				Text = GetMonthlyExpeditionStatus(data);
				break;

			case "api_req_mission/result":
				Text = GetExpeditionResult(data);
				_ignorePort = 1;
				break;

			case "api_req_practice/battle_result":
			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
				Text = GetBattleResult(data);
				break;

			case "api_req_hokyu/charge":
				Text = GetSupplyInformation(data);
				break;

			case "api_req_mission/start":
				if (Utility.Configuration.Config.Control.ShowExpeditionAlertDialog)
				{
					int missionId = int.Parse(data["api_mission_id"]);
					int fleetId = int.Parse(data["api_deck_id"]);

					App.Current!.Dispatcher.BeginInvoke(() => CheckExpedition(missionId, fleetId));
				}

				break;

			case "api_get_member/sortie_conditions":
				App.Current!.Dispatcher.BeginInvoke(CheckSallyArea);
				break;

			case "api_req_map/start":
				_inSortie = KCDatabase.Instance.Fleet.Fleets.Values
					.Where(f => f.IsInSortie || f.ExpeditionState is ExpeditionState.OnExpedition)
					.Select(f => f.FleetID)
					.ToList();

				RecordMaterials();
				break;

			case "api_req_map/next":
			{
				var str = CheckGimmickUpdated(data);
				if (!string.IsNullOrWhiteSpace(str))
					Text = str;

				if (data.api_destruction_battle())
				{
					str = CheckGimmickUpdated(data.api_destruction_battle);
					if (!string.IsNullOrWhiteSpace(str))
						Text = str;
				}

			}
			break;

			case "api_req_practice/battle":
				_inSortie = new List<int>() { KCDatabase.Instance.Battle.BattleDay.Initial.FriendFleetID };
				break;

		}

	}

	private void QuestListUpdated()
	{
		StringBuilder info = new();
		KCDatabase db = KCDatabase.Instance;

		IEnumerable<IGrouping<DateTime, QuestData>> questListByEndDate = db.Quest.Quests.Values
			.OrderBy(q => q.ID)
			.Where(q => q.GetEndDateTime() is not null)
			.GroupBy(q => (DateTime)q.GetEndDateTime()!);

		foreach (IGrouping<DateTime, QuestData> questByEndDate in questListByEndDate)
		{
			info.AppendLine(string.Format(GeneralRes.QuestListEndsOn, questByEndDate.Key));

			foreach (QuestData quest in questByEndDate)
			{
				info.AppendLine($"{quest.Code}: {quest.Name}");
			}
		}

		Text = info.ToString();
	}

	private string GetPracticeEnemyInfo(dynamic data)
	{
		StringBuilder sb = new();
		sb.AppendLine(GeneralRes.PracticeReport);
		sb.AppendLine($"{GeneralRes.EnemyAdmiral}: {data.api_nickname}");
		sb.AppendLine($"{GeneralRes.EnemyFleetName}: {data.api_deckname}");

		FleetData fleet = KCDatabase.Instance.Fleet[1];
		int ship1lv = (int)data.api_deck.api_ships[0].api_id != -1 ? (int)data.api_deck.api_ships[0].api_level : 1;
		int ship2lv = (int)data.api_deck.api_ships[1].api_id != -1 ? (int)data.api_deck.api_ships[1].api_level : 1;

		ExerciseExp exp = Calculator.GetExerciseExp(fleet, ship1lv, ship2lv);

		sb.Append($"{GeneralRes.BaseExp}: {(int)exp.BaseA} / {InformationResources.SRank}: {(int)exp.BaseS}");

		if (exp.TrainingCruiserSurfaceA is not null && exp.TrainingCruiserSurfaceS is not null)
		{
			int trainingCruiserSurfaceA = (int)exp.TrainingCruiserSurfaceA;
			int trainingCruiserSurfaceS = (int)exp.TrainingCruiserSurfaceS;

			if (exp.TrainingCruiserSubmarineA is not null && exp.TrainingCruiserSubmarineS is not null)
			{
				int trainingCruiserSubmarineA = (int)exp.TrainingCruiserSubmarineA;
				int trainingCruiserSubmarineS = (int)exp.TrainingCruiserSubmarineS;

				sb.AppendLine();

				if (trainingCruiserSurfaceA == trainingCruiserSubmarineA)
				{
					sb.AppendFormat(InformationResources.CTBonus, trainingCruiserSurfaceA, trainingCruiserSurfaceS);
				}
				else
				{
					sb.AppendFormat(InformationResources.CtBonusSurface, trainingCruiserSurfaceA, trainingCruiserSurfaceS);
					sb.AppendLine();
					sb.AppendFormat(InformationResources.CtBonusSubmarine, trainingCruiserSubmarineA, trainingCruiserSubmarineS);
				}
			}
		}

		return sb.ToString();
	}
	private string GetAlbumInfo(dynamic data)
	{

		StringBuilder sb = new StringBuilder();

		if (data != null && data.api_list() && data.api_list != null)
		{

			if (data.api_list[0].api_yomi())
			{
				//艦娘図鑑
				const int bound = 70;       // 図鑑1ページあたりの艦船数
				int startIndex = (((int)data.api_list[0].api_index_no - 1) / bound) * bound + 1;
				bool[] flags = Enumerable.Repeat<bool>(false, bound).ToArray();

				sb.AppendLine(GeneralRes.DamagedArtUnseen);

				foreach (dynamic elem in data.api_list)
				{

					flags[(int)elem.api_index_no - startIndex] = true;

					dynamic[] state = elem.api_state;
					for (int i = 0; i < state.Length; i++)
					{
						if ((int)state[i][1] == 0)
						{

							var target = KCDatabase.Instance.MasterShips[(int)elem.api_table_id[i]];
							if (target != null)     //季節の衣替え艦娘の場合存在しないことがある
								sb.AppendLine(target.NameEN);
						}
					}

				}

				sb.AppendLine(GeneralRes.UnseenShips);
				for (int i = 0; i < bound; i++)
				{
					if (!flags[i])
					{
						IShipDataMaster? ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == startIndex + i);
						if (ship != null)
						{
							sb.AppendLine(ship.NameEN);
						}
					}
				}

			}
			else
			{
				//装備図鑑
				const int bound = 70;       // 図鑑1ページあたりの装備数
				int startIndex = (((int)data.api_list[0].api_index_no - 1) / bound) * bound + 1;
				bool[] flags = Enumerable.Repeat<bool>(false, bound).ToArray();

				foreach (dynamic elem in data.api_list)
				{

					flags[(int)elem.api_index_no - startIndex] = true;
				}

				sb.AppendLine(GeneralRes.UnseenEquips);
				for (int i = 0; i < bound; i++)
				{
					if (!flags[i])
					{
						IEquipmentDataMaster eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault(s => s.AlbumNo == startIndex + i);
						if (eq != null)
						{
							sb.AppendLine(eq.NameEN);
						}
					}
				}
			}
		}

		return sb.ToString();
	}


	private string GetCreateItemInfo(dynamic data)
	{

		if ((int)data.api_create_flag == 0)
		{

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(GeneralRes.DevelopmentFailed);
			sb.AppendLine(data.api_fdata);

			IEquipmentDataMaster eqm = KCDatabase.Instance.MasterEquipments[int.Parse(((string)data.api_fdata).Split(",".ToCharArray())[1])];
			if (eqm != null)
				sb.AppendLine(eqm.NameEN);


			return sb.ToString();

		}
		else
			return "";
	}


	private string GetMapGauge(dynamic data)
	{

		StringBuilder sb = new StringBuilder();
		sb.AppendLine(GeneralRes.MapGauges);

		string rpcMapInfo = "";


		foreach (var map in KCDatabase.Instance.MapInfo.Values)
		{
			int gaugeType = -1;
			int current = 0;
			int max = 0;

			if (map.RequiredDefeatedCount != -1 && map.CurrentDefeatedCount < map.RequiredDefeatedCount)
			{
				gaugeType = 1;
				current = map.CurrentDefeatedCount;
				max = map.RequiredDefeatedCount;
			}
			else if (map.MapHPMax > 0)
			{
				gaugeType = map.GaugeType;
				current = map.MapHPCurrent;
				max = map.MapHPMax;
			}

			if (gaugeType > 0)
			{
				sb.AppendLine(string.Format("{0}-{1} {2}: {3}{4} {5}/{6}{7}",
					map.MapAreaID, map.MapInfoID,
					map.EventDifficulty > 0 ? $" [{Constants.GetDifficulty(map.EventDifficulty)}]" : "",
					map.CurrentGaugeIndex > 0 ? $"#{map.CurrentGaugeIndex} " : "",
					gaugeType == 1 ? InformationResources.Defeated : gaugeType == 2 ? "HP" : "TP",
					current, max,
					gaugeType == 1 ? InformationResources.Times : ""));

				if (map.MapAreaID > 10)
				{
					rpcMapInfo = string.Format("E{0} {1}: {2}{3} {4}/{5}{6}",
						map.MapInfoID,
						map.EventDifficulty > 0 ? $"{Constants.GetDifficulty(map.EventDifficulty)}" : "",
						map.CurrentGaugeIndex > 0 ? $"#{map.CurrentGaugeIndex} " : "",
						gaugeType == 1 ? InformationResources.Defeated : gaugeType == 2 ? "HP" : "TP",
						current, max,
						gaugeType == 1 ? InformationResources.Times : "");
				}
			}
		}

		DiscordRpcManager.Instance.GetRPCData().MapInfo = rpcMapInfo;

		return sb.ToString();
	}

	private enum MissionState
	{
		New,
		NotCompleted,
		Completed
	}

	private string GetMonthlyExpeditionStatus(dynamic data)
	{
		List<(int ID, MissionState State)> missionStates = new List<(int, MissionState)>();
		foreach (dynamic item in data.api_list_items)
		{
			missionStates.Add(((int)item.api_mission_id, (MissionState)item.api_state));
		}

		IEnumerable<string> unfinishedMonthlyIds =
			from mission in KCDatabase.Instance.Mission.Values
			where mission.ResetType == ResetType.Monthly
			join missionState in missionStates on mission.ID equals missionState.ID into missionsWithState
			from missionState in missionsWithState.DefaultIfEmpty()
			where missionState.State != MissionState.Completed
			select mission.DisplayID;

		return $"{InformationResources.UnfinishedMonthlyExpeditions}: {string.Join(", ", unfinishedMonthlyIds)}";
	}

	private string GetExpeditionResult(dynamic data)
	{
		StringBuilder sb = new StringBuilder();

		sb.AppendLine(GeneralRes.ExpeditionReturned);
		sb.AppendLine(KCDatabase.Instance.Translation.Mission.Name(data.api_quest_name));
		sb.AppendFormat(GeneralRes.Result + ": {0}\r\n", Constants.GetExpeditionResult((int)data.api_clear_result));
		sb.AppendFormat(GeneralRes.AdmiralXP + ": +{0}\r\n", (int)data.api_get_exp);
		sb.AppendFormat(GeneralRes.ShipXP + ": +{0}\r\n", ((int[])data.api_get_ship_exp).Min());

		return sb.ToString();
	}


	private string GetBattleResult(dynamic data)
	{
		StringBuilder sb = new StringBuilder();

		sb.AppendLine(GeneralRes.BattleComplete);
		sb.AppendFormat(GeneralRes.EnemyName + "\r\n", KCDatabase.Instance.Translation.Operation.FleetName(data.api_enemy_info.api_deck_name));
		sb.AppendFormat(InformationResources.BattleResultRank + "\r\n", KCDatabase.Instance.Battle.PredictedBattleRank);
		sb.AppendFormat(GeneralRes.AdmiralXP + ": +{0}\r\n", (int)data.api_get_exp);

		sb.Append(CheckGimmickUpdated(data));

		return sb.ToString();
	}

	private string CheckGimmickUpdated(dynamic data)
	{
		if (data.api_m1() && data.api_m1 != 0)
		{
			Utility.Logger.Add(2, InformationResources.ChangesInEventMapDetected);
			return "\r\n" + InformationResources.GimmickRelease + "\r\n";
		}

		return "";
	}


	private string GetSupplyInformation(dynamic data)
	{

		StringBuilder sb = new StringBuilder();

		sb.AppendLine(GeneralRes.ResupplyComplete);
		sb.AppendFormat(GeneralRes.BauxiteUsage + "\r\n", (int)data.api_use_bou, (int)data.api_use_bou / 5);

		return sb.ToString();
	}


	private string GetConsumptionResource(dynamic data)
	{

		StringBuilder sb = new StringBuilder();
		var material = KCDatabase.Instance.Material;


		int fuel_diff = material.Fuel - _prevResource[0],
			ammo_diff = material.Ammo - _prevResource[1],
			steel_diff = material.Steel - _prevResource[2],
			bauxite_diff = material.Bauxite - _prevResource[3];


		var ships = KCDatabase.Instance.Fleet.Fleets.Values
			.Where(f => _inSortie.Contains(f.FleetID))
			.SelectMany(f => f.MembersInstance)
			.Where(s => s != null);

		int fuel_supply = ships.Sum(s => s.SupplyFuel);
		int ammo = ships.Sum(s => s.SupplyAmmo);
		int bauxite = ships.Sum(s => s.Aircraft.Zip(s.MasterShip.Aircraft, (current, max) => new { Current = current, Max = max }).Sum(a => (a.Max - a.Current) * 5));

		int fuel_repair = ships.Sum(s => s.RepairFuel);
		int steel = ships.Sum(s => s.RepairSteel);


		sb.AppendFormat(GeneralRes.ResupplyString,
			fuel_diff - fuel_supply - fuel_repair, fuel_diff, fuel_supply, fuel_repair,
			ammo_diff - ammo, ammo_diff, ammo,
			steel_diff - steel, steel_diff, steel,
			bauxite_diff - bauxite, bauxite_diff, bauxite, bauxite / 5);

		return sb.ToString();
	}


	private void CheckSallyArea()
	{
		// only check if there's any event maps
		// this function shouldn't get called outside of events so the check should be pointless
		// if (KCDatabase.Instance.MapArea.Values.Any(m => m.ID > 20)) return;

		IEnumerable<IEnumerable<IShipData>> group;

		if (KCDatabase.Instance.Fleet.CombinedFlag != 0)
			group = new[] { KCDatabase.Instance.Fleet[1].MembersInstance.Concat(KCDatabase.Instance.Fleet[2].MembersInstance).Where(s => s != null) };
		else
			group = KCDatabase.Instance.Fleet.Fleets.Values
				.Where(f => f?.ExpeditionState == 0)
				.Select(f => f.MembersInstance.Where(s => s != null));


		group = group.Where(ss =>
			ss.All(s => s.RepairingDockID == -1) &&
			ss.Any(s => s.SallyArea == 0));


		if (group.Any())
		{
			var freeShips = group.SelectMany(f => f).Where(s => s.SallyArea == 0);

			Text = InformationResources.FleetTagWarning + string.Join("\r\n", freeShips.Select(s => s.NameWithLevel));

			if (Utility.Configuration.Config.Control.ShowSallyAreaAlertDialog)
				MessageBox.Show(InformationResources.FleetTagAlertDialogText, InformationResources.FleetTagAlertDialogCaption,
					MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}

	private void CheckExpedition(int missionID, int fleetID)
	{
		var fleet = KCDatabase.Instance.Fleet[fleetID];
		var result = MissionClearCondition.Check(missionID, fleet);
		var mission = KCDatabase.Instance.Mission[missionID];

		if (!result.IsSuceeded)
		{
			MessageBox.Show(
				string.Format(GeneralRes.ExpeditionFailureWarningMessage, fleet.FleetID, fleet.Name, mission.DisplayID, mission.NameEN, string.Join("\r\n", result.FailureReason))
				, GeneralRes.ExpeditionFailureWarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		if (fleet.MembersInstance.Any(s => s?.FuelRate < 1 || s?.AmmoRate < 1))
		{
			MessageBox.Show(
				string.Format(InformationResources.ExpeditionFailureFuelAmmo, fleet.FleetID, fleet.Name, mission.DisplayID, mission.Name),
				GeneralRes.ExpeditionFailureWarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}


	private void RecordMaterials()
	{
		var material = KCDatabase.Instance.Material;
		_prevResource[0] = material.Fuel;
		_prevResource[1] = material.Ammo;
		_prevResource[2] = material.Steel;
		_prevResource[3] = material.Bauxite;
	}

	protected string GetPersistString()
	{
		return "Information";
	}
}
