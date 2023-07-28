using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class BattleNode : SortieNode
{
	public BattleData FirstBattle { get; }
	public BattleData? SecondBattle { get; set; }
	public BattleResult? BattleResult { get; set; }

	public string Result => ConstantsRes.BattleDetail_Result;

	public string? ResultRank { get; private set; }
	public string? ResultMvpMain { get; private set; }
	public string? ResultMvpEscort { get; private set; }
	public string? AdmiralExp { get; private set; }
	public string? BaseExp { get; private set; }
	public string? DropShip { get; private set; }

	public BattleNode(IKCDatabase kcDatabase, int world, int map, int cell, BattleData battle)
		: base(kcDatabase, world, map, cell)
	{
		FirstBattle = battle;
	}

	public void AddResult(ISortieBattleResultApi result)
	{
		bool isPlayerCombinedFleet = FirstBattle.FleetsBeforeBattle.EscortFleet is not null;

		BattleResult = result switch
		{
			ApiReqCombinedBattleBattleresultResponse r => new(r),
			_ => new(result),
		};

		ResultRank = string.Format(ConstantsRes.BattleDetail_ResultRank, BattleResult.WinRank);

		ResultMvpMain = BattleResult.MvpIndex switch
		{
			int i and >= 0 => FirstBattle.FleetsBeforeBattle.Fleet.MembersInstance[i] switch
			{
				IShipData ship when isPlayerCombinedFleet => string.Format(ConstantsRes.BattleDetail_ResultMVPMain, ship.NameWithLevel),
				IShipData ship => string.Format("MVP: {0}", ship.NameWithLevel),
				_ => null,
			},
			_ => null,
		};

		ResultMvpEscort = BattleResult.MvpIndexCombined switch
		{
			int i and >= 0 => FirstBattle.FleetsBeforeBattle.EscortFleet?.MembersInstance[i] switch
			{
				IShipData ship => string.Format(ConstantsRes.BattleDetail_ResultMVPEscort, ship.NameWithLevel),
				_ => null,
			},
			_ => null,
		};

		AdmiralExp = string.Format(ConstantsRes.BattleDetail_AdmiralExp, BattleResult.AdmiralExp);
		BaseExp = string.Format(ConstantsRes.BattleDetail_ShipExp, BattleResult.BaseExp);

		DropShip = BattleResult.DroppedShipId switch
		{
			ShipId id => KcDatabase.MasterShips[(int)id] switch
			{
				{ } ship => $"{ConstantsRes.BattleDetail_Drop} {ship.ShipTypeName} {ship.NameWithClass}",
				_ => ConstantsRes.BattleDetail_Drop + ConstantsRes.NoNode,
			},
			_ => null,
		};
	}
}
