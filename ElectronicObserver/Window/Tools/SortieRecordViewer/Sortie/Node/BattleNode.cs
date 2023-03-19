using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Battleresult;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class BattleNode : SortieNode
{
	public BattleData FirstBattle { get; }
	public BattleData? SecondBattle { get; set; }
	public BattleResult? BattleResult { get; private set; }

	public string Result => ConstantsRes.BattleDetail_Result;

	public string? ResultRank => BattleResult switch
	{
		null => null,
		_ => string.Format(ConstantsRes.BattleDetail_ResultRank, BattleResult.WinRank),
	};

	public string? ResultMvpMain => BattleResult?.MvpIndex switch
	{
		int i and >= 0 => FirstBattle.FleetsBeforeBattle.Fleet.MembersInstance[i] switch
		{
			IShipData ship => string.Format(ConstantsRes.BattleDetail_ResultMVPMain, ship.NameWithLevel),
			_ => null,
		},
		_ => null,
	};

	public string? ResultMvpEscort => BattleResult?.MvpIndexCombined switch
	{
		int i and >= 0 => FirstBattle.FleetsBeforeBattle.EscortFleet?.MembersInstance[i] switch
		{
			IShipData ship => string.Format(ConstantsRes.BattleDetail_ResultMVPEscort, ship.NameWithLevel),
			_ => null,
		},
		_ => null,
	};

	public string? AdmiralExp => BattleResult?.AdmiralExp switch
	{
		int exp => string.Format(ConstantsRes.BattleDetail_AdmiralExp, exp),
		_ => null,
	};

	public string? BaseExp => BattleResult?.BaseExp switch
	{
		int exp => string.Format(ConstantsRes.BattleDetail_ShipExp, exp),
		_ => null,
	};

	public string? DropShip => BattleResult switch
	{
		{ DroppedShipId: ShipId id } => string.Format(ConstantsRes.BattleDetail_Drop, KcDatabase.MasterShips[(int)id].NameEN),
		null => null,
		_ => ConstantsRes.BattleDetail_Drop + ConstantsRes.NoNode,
	};

	public BattleNode(IKCDatabase kcDatabase, int world, int map, int cell, BattleData battle)
		: base(kcDatabase, world, map, cell)
	{
		FirstBattle = battle;
	}

	public void AddResult(ApiReqSortieBattleresultResponse result)
	{
		BattleResult = new(result);
	}
}
