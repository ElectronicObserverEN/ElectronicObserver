using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.UiBlocker;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks.Specials;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class GameStateProvider : IGameStateProvider
{
	/// <inheritdoc />
	public bool CanShipSink
	{
		get
		{
			BattleManager bm = KCDatabase.Instance.Battle;

			if (bm.Compass is null) return false;
			if (bm.Compass.IsEndPoint) return false;

			BattleData battle = bm.SecondBattle ?? bm.FirstBattle;

			List<int> usedDamecons = bm.SecondBattle switch
			{
				null => bm.FirstBattle.UsedDamecons,
				_ => [.. bm.FirstBattle.UsedDamecons, .. bm.SecondBattle.UsedDamecons],
			};

			if (CanAnyShipSink(battle.Initial.FriendFleet, battle.ResultHPs, usedDamecons))
			{
				return true;
			}

			if (!bm.IsCombinedBattle) return false;

			return CanAnyShipSink(battle.Initial.FriendFleetEscort, battle.ResultHPs.Skip(6), usedDamecons);

			static bool CanAnyShipSink(FleetData fleet, IEnumerable<int> hps, List<int> usedDamecons)
			{
				if (fleet.MembersInstance is null) return false;

				return fleet.MembersInstance
					.Zip(hps, (s, hp) => (Ship: s, Hp: hp))
					.Any(t => t.Ship.CanSink(fleet, t.Hp, usedDamecons.Contains(t.Ship?.MasterID ?? 0)));
			}
		}
	}

	/// <inheritdoc />
	public bool CanDoDoubleLineTouch => KCDatabase.Instance.Fleet.Fleets.Values
		.FirstOrDefault(f => f.IsInSortie)
		?.GetSpecialAttacks()
		.Any(a => a is NelsonSpecialAttack or RichelieuSpecialAttack) ?? false;

	/// <inheritdoc />
	public bool CanDoEchelonTouch => KCDatabase.Instance.Fleet.Fleets.Values
		.FirstOrDefault(f => f.IsInSortie)
		?.GetSpecialAttacks()
		.Any(a => a is
			NagatoSpecialAttack or
			ColoradoSpecialAttack or
			QueenElizabethSpecialAttack or
			Yamato12SpecialAttack or
			Yamato123SpecialAttack) ?? false;

	/// <inheritdoc />
	public bool CanDoSecondPatrolFormationTouch => KCDatabase.Instance.Fleet.Fleets.Values
		.FirstOrDefault(f => f.IsInSortie)
		?.GetSpecialAttacks()
		.Any(a => a is
			NelsonSpecialAttack or
			NagatoSpecialAttack or
			ColoradoSpecialAttack or
			RichelieuSpecialAttack or
			QueenElizabethSpecialAttack) ?? false;

	/// <inheritdoc />
	public bool CanDoFourthPatrolFormationTouch => KCDatabase.Instance.Fleet.Fleets.Values
		.FirstOrDefault(f => f.IsInSortie)
		?.GetSpecialAttacks()
		.Any(a => a is Yamato12SpecialAttack or Yamato123SpecialAttack) ?? false;

	/// <inheritdoc />
	public bool IsBattleNode => KCDatabase.Instance.Battle.Compass?.EventID is 4;

	/// <inheritdoc />
	public bool IsBoss => KCDatabase.Instance.Battle.Compass?.IsBossNode ?? false;

	/// <inheritdoc />
	public bool IsEvent => KCDatabase.Instance.MapArea.Values.Any(m => m.IsEventArea);

	/// <inheritdoc />
	public bool IsCombinedFleet => KCDatabase.Instance.Fleet.Fleets.Values
		.FirstOrDefault(f => f.IsInSortie) is { FleetID: 1, FleetType: not FleetType.Single };

	/// <inheritdoc />
	public BattleRank CurrentRank => KCDatabase.Instance.Battle.PredictedBattleResultRank;

	/// <inheritdoc />
	public IMapInfoData? Map => KCDatabase.Instance.Battle.Compass?.MapInfo;

	/// <inheritdoc />
	public int? CellId => KCDatabase.Instance.Battle.Compass?.CellId;
}
