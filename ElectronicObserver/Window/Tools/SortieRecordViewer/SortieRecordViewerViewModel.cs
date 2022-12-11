using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using ElectronicObserverTypes.Serialization.FitBonus;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public partial class SortieRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private DataSerializationService DataSerializationService { get; }
	private IKCDatabase KcDatabase { get; }
	private List<FitBonusPerEquipment> FitBonusList => KCDatabase.Instance.Translation.FitBonus.FitBonusList;
	private ElectronicObserverContext Db { get; } = new();

	public SortieRecordViewerTranslationViewModel SortieRecordViewer { get; }

	private static string AllRecords { get; } = "*";

	public List<object> Worlds { get; }
	public List<object> Maps { get; }

	public object World { get; set; } = AllRecords;
	public object Map { get; set; } = AllRecords;

	public ObservableCollection<SortieRecord> Sorties { get; } = new();

	public SortieRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		DataSerializationService = Ioc.Default.GetRequiredService<DataSerializationService>();
		KcDatabase = Ioc.Default.GetRequiredService<IKCDatabase>();
		SortieRecordViewer = Ioc.Default.GetRequiredService<SortieRecordViewerTranslationViewModel>();

		Worlds = Db.Worlds
			.Select(w => w.Id)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		Maps = Db.Maps
			.Select(m => m.MapId)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();
	}

	[RelayCommand]
	private void Search()
	{
		Sorties.Clear();

		List<SortieRecord> sorties = Db.Sorties
			.Include(s => s.ApiFiles)
			.AsQueryable()
			.Where(s => World as string == AllRecords || s.World == World as int?)
			.Where(s => Map as string == AllRecords || s.Map == Map as int?)
			.OrderByDescending(f => f.Id)
			.ToList();

		foreach (SortieRecord file in sorties)
		{
			Sorties.Add(file);
		}
	}

	// normal battle - day
	// night node - battle
	// night to day - night
	// etc...
	private static bool IsFirstBattleApi(string name) => name is
		"api_req_sortie/battle" or // normal day
		"api_req_battle_midnight/sp_midnight" or // night node
		"api_req_sortie/airbattle" or // single air raid
		"api_req_sortie/ld_airbattle" or // single air raid
		"api_req_sortie/night_to_day" or // single night to day
		"api_req_sortie/ld_shooting" or // single fleet radar ambush
		"api_req_combined_battle/battle" or // combined normal
		"api_req_combined_battle/sp_midnight" or // combined night battle
		"api_req_combined_battle/airbattle" or // combined air exchange ?
		"api_req_combined_battle/battle_water" or // CTF TCF combined battle
		"api_req_combined_battle/ld_airbattle" or // air raid
		"api_req_combined_battle/ec_battle" or // CTF enemy combined battle
		"api_req_combined_battle/ec_night_to_day" or // enemy combined night to day
		"api_req_combined_battle/each_battle" or // STF combined vs combined
		"api_req_combined_battle/each_battle_water" or // STF combined
		"api_req_combined_battle/ld_shooting"; // combined radar ambush

	// normal battle - night
	// night to day - day
	// etc...
	private static bool IsSecondBattleApi(string name) => name is
		"api_req_battle_midnight/battle" or // normal night
		"api_req_combined_battle/midnight_battle" or // combined day to night
		"api_req_combined_battle/ec_midnight_battle"; // combined normal night battle

	private static bool IsBattleEndApi(string name) => name is
		"api_req_sortie/battleresult" or
		"api_req_combined_battle/battleresult" or
		"api_req_practice/battle_result";

	private static bool IsMapProgressApi(string name) => name is
		"api_req_map/start" or
		"api_req_map/next";

	[RelayCommand]
	private static void CopyReplayToClipboard(SortieRecord? sortie)
	{
		if (sortie is null) return;

		ReplayData replay = sortie.ToReplayData();

		replay.Battles = new();

		ReplayBattle battle = new();

		foreach (ApiFile apiFile in sortie.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
		{
			if (IsMapProgressApi(apiFile.Name))
			{
				IMapProgressApi? progress = apiFile.GetMapProgressApiData();

				if (progress is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.Node = progress.ApiNo;
			}

			if (IsFirstBattleApi(apiFile.Name))
			{
				battle.FirstBattle = apiFile.GetResponseApiData();
			}

			if (IsSecondBattleApi(apiFile.Name))
			{
				battle.SecondBattle = apiFile.GetResponseApiData();
			}

			if (IsBattleEndApi(apiFile.Name))
			{
				ISortieBattleResultApi? result = apiFile.GetSortieBattleResultApi();

				if (result is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.FirstBattle ??= new();
				battle.SecondBattle ??= new();
				battle.BaseExp = result.ApiGetBaseExp;
				battle.HqExp = result.ApiGetExp;
				battle.Drop = result.ApiGetShip?.ApiShipId ?? ShipId.Unknown;
				battle.Rating = result.ApiWinRank;
				battle.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				battle.Mvp = new() { result.ApiMvp, };

				if (result is ApiReqCombinedBattleBattleresultResponse combinedResult)
				{
					battle.Mvp.Add(combinedResult.ApiMvpCombined ?? -1);
				}

				replay.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				replay.Battles.Add(battle);
				battle = new();
			}
		}

		// there was battle data but no battle end
		if (battle.FirstBattle is not null || battle.SecondBattle is not null)
		{
			battle.FirstBattle ??= new();
			battle.SecondBattle ??= new();

			replay.Battles.Add(battle);
		}

		Clipboard.SetText(JsonSerializer.Serialize(replay));
	}

	[RelayCommand]
	private void OpenFleetImageGenerator(SortieRecord? sortie)
	{
		if (sortie is null) return;

		int hqLevel = KCDatabase.Instance.Admiral.Level;

		if (sortie.ApiFiles.Any())
		{
			// get the last port response right before the sortie started
			ApiFile? portFile = Db.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.Name == "api_port/port")
				.Where(f => f.TimeStamp < sortie.ApiFiles.First().TimeStamp)
				.OrderByDescending(f => f.TimeStamp)
				.FirstOrDefault();

			if (portFile is not null)
			{
				try
				{
					ApiPortPortResponse? port = JsonSerializer
						.Deserialize<ApiResponse<ApiPortPortResponse>>(portFile.Content)?.ApiData;

					if (port != null)
					{
						hqLevel = port.ApiBasic.ApiLevel;
					}
				}
				catch
				{
					// can probably ignore this
				}
			}
		}

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData
		(
			hqLevel,
			MakeFleet(sortie.FleetData.Fleets.Skip(0).FirstOrDefault()),
			MakeFleet(sortie.FleetData.Fleets.Skip(1).FirstOrDefault()),
			MakeFleet(sortie.FleetData.Fleets.Skip(2).FirstOrDefault()),
			MakeFleet(sortie.FleetData.Fleets.Skip(3).FirstOrDefault()),
			MakeAirBase(sortie.FleetData.AirBases.Skip(0).FirstOrDefault()),
			MakeAirBase(sortie.FleetData.AirBases.Skip(1).FirstOrDefault()),
			MakeAirBase(sortie.FleetData.AirBases.Skip(2).FirstOrDefault())
		);

		FleetImageGeneratorImageDataModel model = new()
		{
			Fleet1Visible = data.Fleet1 is not null,
			Fleet2Visible = data.Fleet2 is not null,
			Fleet3Visible = data.Fleet3 is not null,
			Fleet4Visible = data.Fleet4 is not null,
		};

		ToolService.FleetImageGenerator(model, data);
	}

	private IFleetData? MakeFleet(SortieFleet? fleet) => fleet switch
	{
		{ } => new FleetDataMock
		{
			Name = fleet.Name,
			MembersInstance = new ReadOnlyCollection<IShipData?>(fleet.Ships.Select(MakeShip).Select(ApplyFitBonus).ToList()),
		},
		_ => null,
	};

	private ShipDataMock? MakeShip(SortieShip? ship) => ship switch
	{
		{ } => new ShipDataMock(KcDatabase.MasterShips[(int)ship.Id])
		{
			Level = ship.Level,
			IsExpansionSlotAvailable = ship.ExpansionSlot is not null,
			SlotInstance = ship.EquipmentSlots.Select(s => MakeEquipment(s.Equipment)).ToList(),
			ExpansionSlotInstance = MakeEquipment(ship.ExpansionSlot?.Equipment),
			Fuel = ship.Fuel,
			Ammo = ship.Ammo,
			Range = ship.Range,
			Speed = ship.Speed,
			LuckModernized = ship.Kyouka.Skip(4).FirstOrDefault(),
			HPMaxModernized = ship.Kyouka.Skip(5).FirstOrDefault(),
			ASWModernized = ship.Kyouka.Skip(6).FirstOrDefault(),
		},
		_ => null,
	};

	private IShipData? ApplyFitBonus(ShipDataMock? ship)
	{
		if (ship is null) return ship;

		FitBonusValue fit = ship.GetFitBonus(FitBonusList);

		ship.FirepowerFit = fit.Firepower;
		ship.TorpedoFit = fit.Torpedo;
		ship.AaFit = fit.AntiAir;
		ship.ArmorFit = fit.Armor;
		ship.EvasionFit = fit.Evasion;
		ship.AswFit = fit.ASW;
		ship.LosFit = fit.LOS;

		return ship;
	}

	private IEquipmentData? MakeEquipment(SortieEquipment? equipment) => equipment switch
	{
		{ } => new EquipmentDataMock(KcDatabase.MasterEquipments[(int)equipment.Id])
		{
			Level = equipment.Level,
			AircraftLevel = equipment.AircraftLevel,
		},
		_ => null,
	};

	private IBaseAirCorpsData? MakeAirBase(SortieAirBase? airBase) => airBase switch
	{
		{ } => new BaseAirCorpsDataMock
		{
			Name = airBase.Name,
			ActionKind = airBase.ActionKind,
			Distance = airBase.BaseDistance + airBase.BonusDistance,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{0, MakeAirBaseSquadron(airBase.Squadrons.Skip(0).FirstOrDefault())},
				{1, MakeAirBaseSquadron(airBase.Squadrons.Skip(1).FirstOrDefault())},
				{2, MakeAirBaseSquadron(airBase.Squadrons.Skip(2).FirstOrDefault())},
				{3, MakeAirBaseSquadron(airBase.Squadrons.Skip(3).FirstOrDefault())},
			},
		},
		_ => null,
	};

	private IBaseAirCorpsSquadron MakeAirBaseSquadron(SortieAirBaseSquadron? squadron)
	{
		BaseAirCorpsSquadronMock abSlot = new()
		{
			EquipmentInstance = MakeEquipment(squadron?.EquipmentSlot.Equipment),
		};

		abSlot.AircraftMax = AirBaseAircraftCount(abSlot.EquipmentInstance?.MasterEquipment);
		abSlot.AircraftCurrent = AirBaseAircraftCount(abSlot.EquipmentInstance?.MasterEquipment);

		return abSlot;
	}

	private static int AirBaseAircraftCount(IEquipmentDataMaster? equipment) => equipment?.CategoryType switch
	{
		null => 0,

		EquipmentTypes.CarrierBasedRecon => 4,
		EquipmentTypes.FlyingBoat => 4,
		EquipmentTypes.HeavyBomber => 9,

		_ => 18,
	};
}
