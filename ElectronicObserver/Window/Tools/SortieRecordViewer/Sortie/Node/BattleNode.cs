using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public sealed class BattleNode(IKCDatabase kcDatabase, int world, int map, int cell, BattleData battle, CellType colorNo, int eventId, int eventKind)
	: SortieNode(kcDatabase, world, map, cell, colorNo, eventId, eventKind)
{
	public IBattleApiRequest? Request { get; set; }
	public BattleData FirstBattle { get; } = battle;
	public BattleData? SecondBattle { get; set; }
	public BattleResult? BattleResult { get; private set; }
	public BattleData LastBattle => SecondBattle switch
	{
		null => FirstBattle,
		_ => SecondBattle,
	};

	public bool IsBoss => ApiEventId is 5;

	public string Result => ConstantsRes.BattleDetail_Result;

	public string? ResultRank { get; private set; }
	public string? RealWinRank => GetRealWinRank();
	public string? ResultMvpMain { get; private set; }
	public string? ResultMvpEscort { get; private set; }
	public string? AdmiralExp { get; private set; }
	public string? BaseExp { get; private set; }
	public string? DropShip { get; private set; }

	public override IEnumerable<PhaseBase> AllPhases => base.AllPhases
		.Concat(FirstBattle.Phases)
		.Concat(SecondBattle?.Phases ?? []);

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

	private string? GetRealWinRank()
	{
		IEnumerable<IShipData?> shipsBeforeBattle = FirstBattle.FleetsBeforeBattle.Fleet.MembersWithoutEscaped!;
		BattleData lastBattle = SecondBattle switch
		{
			null => FirstBattle,
			_ => SecondBattle,
		};
		IEnumerable<IShipData?> shipsAfterBattle = lastBattle.FleetsAfterBattle.Fleet.MembersWithoutEscaped!;

		if (FirstBattle.FleetsBeforeBattle.EscortFleet is IFleetData escort)
		{
			shipsBeforeBattle = shipsBeforeBattle.Concat(escort.MembersWithoutEscaped!);
			shipsAfterBattle = shipsAfterBattle.Concat(lastBattle.FleetsAfterBattle.EscortFleet!.MembersWithoutEscaped!);
		}

		int hpBeforeBattle = shipsBeforeBattle.Sum(s => s?.HPCurrent ?? 0);
		int hpAfterBattle = shipsAfterBattle.Sum(s => s?.HPCurrent ?? 0);

		bool damageTaken = hpBeforeBattle > hpAfterBattle;

		return (BattleResult?.WinRank, damageTaken) switch
		{
			("S", false) => "SS",
			_ => BattleResult?.WinRank,
		};
	}

	public override void UpdateState(ApiGetMemberShipDeckResponse deck)
	{
		LastBattle.FleetsAfterBattle.UpdateState(deck);
	}

	public void UpdateState(BattleFleets fleets)
	{
		LastBattle.FleetsAfterBattle.UpdateState(fleets);
	}

	public bool IsSubsOnly() => FirstBattle.FleetsBeforeBattle.EnemyFleet?.MembersInstance
		.All(s => s?.MasterShip.IsSubmarine ?? true) ?? false;

	public bool IsPtOnly() => FirstBattle.FleetsBeforeBattle.EnemyFleet?.MembersInstance
		.All(s => s?.MasterShip.IsPt ?? true) ?? false;
}
