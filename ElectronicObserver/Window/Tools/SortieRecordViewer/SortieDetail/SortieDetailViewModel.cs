using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EcMidnightBattle;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Next;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battle;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

public class SortieDetailViewModel : WindowViewModelBase
{
	public SortieDetailTranslationViewModel SortieDetail { get; }
	private BattleFactory BattleFactory { get; }

	public int World { get; }
	public int Map { get; }
	private BattleFleets Fleets { get; }
	private List<IBaseAirCorpsData>? AirBases { get; }

	public ObservableCollection<SortieNode> Nodes { get; } = new();

	public SortieDetailViewModel(int world, int map, BattleFleets fleets, List<IBaseAirCorpsData>? airBases = null)
	{
		SortieDetail = Ioc.Default.GetRequiredService<SortieDetailTranslationViewModel>();
		BattleFactory = Ioc.Default.GetRequiredService<BattleFactory>();

		World = world;
		Map = map;
		Fleets = fleets;
		AirBases = airBases;
	}

	private List<object> ApiResponseCache { get; } = new();

	public void AddApiResponse(object response)
	{
		if (response is ApiReqMapStartResponse start)
		{
			ProcessResponseCache();

			ApiResponseCache.Add(start);

			return;
		}

		if (response is ApiReqMapNextResponse next)
		{
			ProcessResponseCache();

			ApiResponseCache.Add(next);

			return;
		}

		if (response is ApiReqSortieBattleresultResponse or ApiReqCombinedBattleBattleresultResponse)
		{
			ApiResponseCache.Add(response);

			ProcessResponseCache();

			return;
		}

		if (response is ApiGetMemberShipDeckResponse deck)
		{
			// todo: could be used to verify fleet state
			return;
		}

		ApiResponseCache.Add(response);
	}

	private void ProcessResponseCache()
	{
		if (!ApiResponseCache.Any()) return;

		SortieNode? node = null;
		int cell = 0;

		foreach (object response in ApiResponseCache)
		{
			cell = response switch
			{
				ApiReqMapStartResponse s => s.ApiNo,
				ApiReqMapNextResponse n => n.ApiNo,
				_ => cell,
			};

			BattleData? battle = GetBattle(response, node);

			if (battle is not null)
			{
				if (node is BattleNode battleNode)
				{
					battleNode.SecondBattle = battle;
				}
				else
				{
					node = new BattleNode(KCDatabase.Instance, World, Map, cell, battle);
				}
			}

			if (response is ApiReqSortieBattleresultResponse result)
			{
				if (node is not BattleNode battleNode) continue;

				battleNode.AddResult(result);
			}
		}

		Nodes.Add(node ?? new EmptyNode(KCDatabase.Instance, World, Map, cell));

		ApiResponseCache.Clear();
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
		ApiReqCombinedBattleEachBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEcBattleResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleEachBattleWaterResponse a => BattleFactory.CreateBattle(a, Fleets),
		ApiReqCombinedBattleBattleResponse a => null,

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
		ApiReqCombinedBattleEcMidnightBattleResponse a => BattleFactory.CreateBattle(a, fleets),

		_ => null,
	};
}
