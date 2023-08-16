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
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.BattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.LdShooting;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.MidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

public partial class SortieDetailViewModel : WindowViewModelBase
{
	public SortieDetailTranslationViewModel SortieDetail { get; }
	private BattleFactory BattleFactory { get; }
	private ToolService ToolService { get; }

	private SortieRecordViewModel Sortie { get; }

	public DateTime? StartTime { get; set; }
	public int World { get; }
	public int Map { get; }
	private BattleFleets Fleets { get; set; }

	public ObservableCollection<SortieNode> Nodes { get; } = new();

	public SortieDetailViewModel(SortieRecordViewModel sortie, BattleFleets fleets)
	{
		SortieDetail = Ioc.Default.GetRequiredService<SortieDetailTranslationViewModel>();
		BattleFactory = Ioc.Default.GetRequiredService<BattleFactory>();
		ToolService = Ioc.Default.GetRequiredService<ToolService>();

		Sortie = sortie;
		World = sortie.World;
		Map = sortie.Map;
		Fleets = fleets;
	}

	private List<(object Response, DateTime Time)> ApiResponseCache { get; } = new();

	public void AddApiFile(object response, DateTime time)
	{
		if (response is ApiReqMapStartResponse start)
		{
			ProcessResponseCache();

			ApiResponseCache.Add((start, time));

			return;
		}

		if (response is ApiReqMapNextResponse next)
		{
			ProcessResponseCache();

			ApiResponseCache.Add((next, time));

			if (next.ApiDestructionBattle is not null)
			{
				ApiResponseCache.Add((next.ApiDestructionBattle, time));
			}

			return;
		}

		if (response is ApiReqSortieBattleresultResponse or ApiReqCombinedBattleBattleresultResponse)
		{
			ApiResponseCache.Add((response, time));

			return;
		}

		if (response is ApiGetMemberShipDeckResponse deck)
		{
			ApiResponseCache.Add((deck, time));

			return;
		}

		ApiResponseCache.Add((response, time));
	}

	private void ProcessResponseCache()
	{
		if (!ApiResponseCache.Any()) return;

		SortieNode? node = null;
		BattleBaseAirRaid? abRaid = null;
		ApiGetMemberShipDeckResponse? deckResponse = null;
		int cell = 0;
		bool isBoss = false;

		foreach ((object response, DateTime time) in ApiResponseCache)
		{
			cell = response switch
			{
				ApiReqMapStartResponse s => s.ApiNo,
				ApiReqMapNextResponse n => n.ApiNo,
				_ => cell,
			};

			isBoss = response switch
			{
				ApiReqMapStartResponse s => s.ApiEventId == 5,
				ApiReqMapNextResponse n => n.ApiEventId == 5,
				_ => isBoss,
			};

			BattleData? battle = GetBattle(response, node);

			if (battle is not null)
			{
				battle.TimeStamp = time;
			}

			if (battle is BattleBaseAirRaid raid)
			{
				abRaid = raid;
				continue;
			}

			if (battle is not null)
			{
				if (node is BattleNode battleNode)
				{
					battleNode.SecondBattle = battle;
				}
				else
				{
					node = new BattleNode(KCDatabase.Instance, World, Map, cell, battle, isBoss);
				}
			}

			if (response is ISortieBattleResultApi result)
			{
				if (node is not BattleNode battleNode) continue;

				battleNode.AddResult(result);
			}

			// comes before next, so this should always be the last response
			if (response is ApiGetMemberShipDeckResponse deck)
			{
				deckResponse = deck;
			}
		}

		node ??= new EmptyNode(KCDatabase.Instance, World, Map, cell);

		if (abRaid is not null)
		{
			node.AddAirBaseRaid(abRaid);
		}

		if (node is BattleNode b)
		{
			Fleets = b.SecondBattle?.FleetsAfterBattle.Clone() ?? b.FirstBattle.FleetsAfterBattle.Clone();

			CleanFleet(Fleets.Fleet);
			CleanFleet(Fleets.EscortFleet);
			ApplyBattleResult(b.BattleResult, Fleets);
		}

		if (deckResponse is not null)
		{
			Fleets.UpdateState(deckResponse);
		}

		Nodes.Add(node);

		ApiResponseCache.Clear();
	}

	private static void CleanFleet(IFleetData? fleetData)
	{
		if (fleetData is not FleetDataMock fleet) return;
		if (fleet.MembersInstance is null) return;

		fleet.MembersInstance = fleet.MembersInstance
			.Where(s => s?.HPCurrent > 0)
			.ToList()
			.ToReadOnlyCollection();
	}

	private static void ApplyBattleResult(BattleResult? battleResult, BattleFleets fleets)
	{
		if (battleResult is null) return;

		ApplyExpGain(fleets.Fleet, battleResult.ExpList, battleResult.LevelUpList);

		if (fleets.EscortFleet is null) return;
		if (battleResult.LevelUpListCombined is null) return;

		ApplyExpGain(fleets.EscortFleet, battleResult.ExpList, battleResult.LevelUpListCombined);
	}

	private static void ApplyExpGain(IFleetData fleet, List<int> expList, List<List<int>> levelUpLists)
	{
		static IEnumerable<(IShipData? Ship, int Exp, List<int> LevelUpList)> GetExpData(
			IFleetData fleet, List<int> expList, List<List<int>> levelUpList)
			=> fleet.MembersInstance
				.Zip(expList.Skip(1))
				.Zip(levelUpList, (t, l) => (t.First, t.Second, l));

		foreach ((IShipData? ship, int exp, List<int> levelUpList) in GetExpData(fleet, expList, levelUpLists))
		{
			if (ship is null) continue;
			if (exp is -1) continue;

			switch (levelUpList.Count)
			{
				// level capped ship
				case 1: break;

				// ship hit level cap
				case 2 when levelUpList[0] + exp > levelUpList[1]:
					((ShipDataMock)ship).Level++;
					break;

				default:
					((ShipDataMock)ship).Level += levelUpList.Count - 2;
					break;
			}
		}
	}

	public void EnsureApiFilesProcessed()
	{
		ProcessResponseCache();
	}

	private BattleData? GetBattle(object api, SortieNode? node) => node switch
	{
		BattleNode b => GetBattle(api, b.FirstBattle.FleetsAfterBattle),
		_ => GetBattle(api),
	};

	/// <summary>
	/// Used to initialize first battles.
	/// </summary>
	/// <param name="api"></param>
	/// <returns></returns>
	private BattleData? GetBattle(object api) => api switch
	{
		ApiReqSortieBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqBattleMidnightSpMidnightResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqSortieAirbattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqSortieLdAirbattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqSortieLdShootingResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleSpMidnightResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleBattleWaterResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleLdAirbattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEcBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEachBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEachBattleWaterResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleLdShootingResponse a => BattleFactory.CreateBattle(a, Fleets),

		ApiDestructionBattle a => BattleFactory.CreateBattle(a, Fleets),

		_ => null,
	};

	/// <summary>
	/// Used to initialize second battles.
	/// </summary>
	/// <param name="api"></param>
	/// <param name="fleets"></param>
	/// <returns></returns>
	private BattleData? GetBattle(object api, BattleFleets fleets) => api switch
	{
		ApiReqBattleMidnightBattleResponse a => BattleFactory.CreateBattle(a, fleets),
		ApiReqCombinedBattleMidnightBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEcMidnightBattleResponse a => BattleFactory.CreateBattle(a, fleets),

		_ => null,
	};

	[RelayCommand]
	private void CopySortieData()
	{
		SortieRecord sortie = new()
		{
			Id = Sortie.Id,
			World = Sortie.World,
			Map = Sortie.Map,
			ApiFiles = Sortie.Model.ApiFiles
				.Where(f => f.ApiFileType is ApiFileType.Response || f.Name is "api_req_map/start")
				.ToList(),
			FleetData = Sortie.Model.FleetData,
			MapData = Sortie.Model.MapData,
		};

		Clipboard.SetText(JsonSerializer.Serialize(sortie));
	}

	[RelayCommand]
	private void LoadSortieData()
	{
		try
		{
			SortieRecord? sortie = JsonSerializer
				.Deserialize<SortieRecord>(Clipboard.GetText());

			if (sortie is null) return;

			ToolService.OpenSortieDetail(new SortieRecordViewModel(sortie, DateTime.UtcNow));
		}
		catch (Exception e)
		{
			Logger.Add(2, "Failed to load sortie details: " + e.Message + e.StackTrace);
		}
	}

	[RelayCommand]
	private void CopyAirControlSimulatorLink()
	{
		ToolService.CopyAirControlSimulatorLink(Sortie);
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		ToolService.AirControlSimulator(Sortie);
	}
}
