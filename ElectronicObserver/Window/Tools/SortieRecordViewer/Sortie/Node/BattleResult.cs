using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqPractice.BattleResult;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class BattleResult
{
	public string Rank { get; }
	public int AdmiralExp { get; }
	public int MvpIndex { get; }
	public int? MvpIndexCombined { get; }
	public int BaseExp { get; }
	public List<int> ExpList { get; }
	public List<int>? ExpListCombined { get; }
	public List<List<int>> LevelUpList { get; }
	public List<List<int>>? LevelUpListCombined { get; }
	public string EnemyFleetName { get; }
	public ShipId? DroppedShipId { get; }
	public int? DroppedItemId { get; }
	public bool CanEscape { get; }
	public List<int>? EscapingShipIndex { get; }

	/// <summary>
	/// api_first_clear is 1 when clearing a map for the first time
	/// for maps with gauges it's 1 every time the last gauge is cleared
	/// </summary>
	public bool IsFirstClear { get; }

	/// <summary>
	/// Seems to be unused.
	/// </summary>
	public int? DroppedEquipmentId { get; }

	public BattleResult(ISortieBattleResultApi result)
	{
		Rank = result.ApiWinRank;
		AdmiralExp = result.ApiGetExp;
		MvpIndex = result.ApiMvp - 1;
		BaseExp = result.ApiGetBaseExp;
		ExpList = result.ApiGetShipExp;
		LevelUpList = result.ApiGetExpLvup;
		EnemyFleetName = result.ApiEnemyInfo.ApiDeckName;
		DroppedShipId = result.ApiGetShip?.ApiShipId;
		DroppedItemId = result.ApiGetUseitem?.ApiUseitemId;
		CanEscape = result.ApiEscape is not null;
		IsFirstClear = result.ApiFirstClear > 0;
		EscapingShipIndex = result.ApiEscape switch
		{
			ApiEscape e => e.ApiEscapeIdx.Concat(e.ApiTowIdx).ToList(),
			_ => null,
		};
	}

	public BattleResult(ApiReqCombinedBattleBattleresultResponse result)
		: this((ISortieBattleResultApi)result)
	{
		MvpIndexCombined = result.ApiMvpCombined - 1;
		ExpListCombined = result.ApiGetShipExpCombined;
		LevelUpListCombined = result.ApiGetExpLvupCombined;
	}

	public BattleResult(ApiReqPracticeBattleResultResponse result)
	{
		Rank = result.ApiWinRank;
		AdmiralExp = result.ApiGetExp;
		MvpIndex = result.ApiMvp - 1;
		BaseExp = result.ApiGetBaseExp;
		ExpList = result.ApiGetShipExp;
		LevelUpList = result.ApiGetExpLvup;
		EnemyFleetName = result.ApiEnemyInfo.ApiDeckName;
	}
}
