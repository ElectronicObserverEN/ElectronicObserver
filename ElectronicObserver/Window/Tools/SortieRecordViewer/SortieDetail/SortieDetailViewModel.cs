using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
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
using ElectronicObserver.KancolleApi.Types.ApiReqMap.StartAirBase;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdAirbattle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.LdShooting;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Services;
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

	private ElectronicObserverContext Db { get; }
	private SortieRecordViewModel Sortie { get; }

	public DateTime? StartTime { get; set; }
	public int World { get; }
	public int Map { get; }
	private BattleFleets Fleets { get; set; }
	private BattleFleets? FleetsAfterSortie { get; set; }
	public List<List<int>?> StrikePoints { get; } = new();

	public ObservableCollection<SortieNode> Nodes { get; } = new();

	public SortieDetailViewModel(ElectronicObserverContext db, SortieRecordViewModel sortie,
		BattleFleets fleets, BattleFleets? fleetsAfterSortie)
	{
		SortieDetail = Ioc.Default.GetRequiredService<SortieDetailTranslationViewModel>();
		BattleFactory = Ioc.Default.GetRequiredService<BattleFactory>();
		ToolService = Ioc.Default.GetRequiredService<ToolService>();

		Db = db;
		Sortie = sortie;
		World = sortie.World;
		Map = sortie.Map;
		Fleets = fleets;
		FleetsAfterSortie = fleetsAfterSortie;
	}

	private List<(object ApiData, DateTime Time)> ApiDataCache { get; } = new();

	public void AddApiFile(object apiData, DateTime time)
	{
		if (apiData is ApiReqMapStartResponse start)
		{
			ProcessApiDataCache();

			ApiDataCache.Add((start, time));

			return;
		}

		if (apiData is ApiReqMapNextResponse next)
		{
			ProcessApiDataCache();

			ApiDataCache.Add((next, time));

			if (next.ApiDestructionBattle is not null)
			{
				ApiDataCache.Add((next.ApiDestructionBattle, time));
			}

			return;
		}

		if (apiData is ApiReqSortieBattleresultResponse or ApiReqCombinedBattleBattleresultResponse)
		{
			ApiDataCache.Add((apiData, time));

			return;
		}

		if (apiData is ApiGetMemberShipDeckResponse deck)
		{
			ApiDataCache.Add((deck, time));

			return;
		}

		ApiDataCache.Add((apiData, time));
	}

	private void ProcessApiDataCache()
	{
		if (!ApiDataCache.Any()) return;

		SortieNode? node = null;
		BattleBaseAirRaid? abRaid = null;
		ApiGetMemberShipDeckResponse? deckResponse = null;
		int cell = 0;
		int eventId = 0;
		int eventKind = 0;
		ApiOffshoreSupply? offshoreSupply = null;

		foreach ((object apiData, DateTime time) in ApiDataCache)
		{
			if (apiData is ApiReqMapStartAirBaseRequest ab)
			{
				StrikePoints.Add(ParseStrikePoints(ab.ApiStrikePoint1));
				StrikePoints.Add(ParseStrikePoints(ab.ApiStrikePoint2));
				StrikePoints.Add(ParseStrikePoints(ab.ApiStrikePoint3));

				continue;

				static List<int>? ParseStrikePoints(string? apiStrikePoint) => apiStrikePoint switch
				{
					null => null,
					_ => apiStrikePoint
						.Split(",")
						.Select(p => int.TryParse(p, out int point) switch
						{
							true => point,
							_ => -1,
						}).ToList(),
				};
			}

			cell = apiData switch
			{
				ApiReqMapStartResponse s => s.ApiNo,
				ApiReqMapNextResponse n => n.ApiNo,
				_ => cell,
			};

			eventId = apiData switch
			{
				ApiReqMapStartResponse s => s.ApiEventId,
				ApiReqMapNextResponse n => n.ApiEventId,
				_ => eventId,
			};

			eventKind = apiData switch
			{
				ApiReqMapStartResponse s => s.ApiEventKind,
				ApiReqMapNextResponse n => n.ApiEventKind,
				_ => eventKind,
			};

			offshoreSupply = apiData switch
			{
				ApiReqMapNextResponse n => n.ApiOffshoreSupply,
				_ => offshoreSupply,
			};

			BattleData? battle = GetBattle(apiData, node);

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
					node = new BattleNode(KCDatabase.Instance, World, Map, cell, battle, eventId, eventKind);
				}
			}

			if (apiData is ISortieBattleResultApi result)
			{
				if (node is not BattleNode battleNode) continue;

				battleNode.AddResult(result);
			}

			// comes before next, so this should always be the last response
			if (apiData is ApiGetMemberShipDeckResponse deck)
			{
				deckResponse = deck;
			}
		}

		node ??= new EmptyNode(KCDatabase.Instance, World, Map, cell, eventId, eventKind);

		if (abRaid is not null)
		{
			node.AddAirBaseRaid(abRaid);
		}

		node.ApiOffshoreSupply = offshoreSupply;

		if (node is BattleNode b)
		{
			if (deckResponse is not null)
			{
				b.UpdateState(deckResponse);
			}
			else if (FleetsAfterSortie is not null)
			{
				b.UpdateState(FleetsAfterSortie);
			}

			Fleets = b.SecondBattle?.FleetsAfterBattle.Clone() ?? b.FirstBattle.FleetsAfterBattle.Clone();

			CleanFleet(Fleets.Fleet);
			CleanFleet(Fleets.EscortFleet);
			ApplyBattleResult(b.BattleResult, Fleets);
		}

		Nodes.Add(node);

		ApiDataCache.Clear();
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
		ProcessApiDataCache();
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
	private async Task CopySortieData()
	{
		await ToolService.CopySortieDataToClipboard(Db, Sortie);
	}

	[RelayCommand]
	private void LoadSortieData()
	{
		ToolService.LoadSortieDataFromClipboard(Db);
	}

	[RelayCommand]
	private void CopyAirControlSimulatorLink()
	{
		ToolService.CopyAirControlSimulatorLink(Sortie, this);
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		ToolService.AirControlSimulator(Sortie);
	}

	[RelayCommand]
	private void CopyOperationRoomLink()
	{
		ToolService.CopyOperationRoomLink(Sortie);
	}

	[RelayCommand]
	private void OpenOperationRoom()
	{
		ToolService.OperationRoom(Sortie);
	}
}
