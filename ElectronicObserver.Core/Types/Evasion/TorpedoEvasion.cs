using System;
using System.Linq;

namespace ElectronicObserver.Core.Types.Evasion;

internal class TorpedoEvasion : EvasionBase
{
	public TorpedoEvasion(IShipData ship, IFleetData? fleet = null, BattleDataMock? battle = null)
		: base(ship, fleet, battle)
	{
	}

	protected override double FormationModifier => Battle.FormationFriend switch
	{
		FormationType.LineAhead => 1,
		FormationType.DoubleLine => 1,

		FormationType.Diamond => 1.1,

		FormationType.Echelon => 1.3,

		FormationType.LineAbreast => 1.4,

		_ => 1
	};

	protected override double VanguardBonus => 0;

	protected override double PostcapBonus => 0;

	// boiler upgrades aren't verified yet
	protected override double EquipmentUpgradeBonus => Ship.AllSlotInstance
		.Where(e => e?.MasterEquipment.IsSonar is true)
		.Sum(e => 1.5 * Math.Sqrt(e!.Level));

	protected override double PostcapModifier => 1;

}
