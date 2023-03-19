using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiReqBattleMidnight.SpMidnight;
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
	public IFleetData Fleet { get; }

	public ObservableCollection<SortieNode> Nodes { get; } = new();

	public SortieDetailViewModel(int world, int map, IFleetData fleet)
	{
		SortieDetail = Ioc.Default.GetRequiredService<SortieDetailTranslationViewModel>();
		BattleFactory = Ioc.Default.GetRequiredService<BattleFactory>();

		World = world;
		Map = map;
		Fleet = fleet;
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

		if (response is ApiReqSortieBattleresultResponse result)
		{
			ApiResponseCache.Add(result);

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

			BattleData? battle = GetBattle(response);

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

	private BattleData? GetBattle(object api) => api switch
	{
		ApiReqSortieBattleResponse a => BattleFactory.CreateBattle(Fleet, a),
		ApiReqBattleMidnightSpMidnightResponse a => BattleFactory.CreateBattle(Fleet, a),

		_ => null,
	};
}
