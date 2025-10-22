using System;
using System.Linq;

namespace ElectronicObserver.Core.Types.Evasion;

internal class ShellingEvasion : EvasionBase
{
	public ShellingEvasion(IShipData ship, IFleetData? fleet = null, BattleDataMock? battle = null)
		: base(ship, fleet, battle)
	{
	}

	protected override double FormationModifier => Battle.FormationFriend switch
	{
		FormationType.LineAhead => 1,
		FormationType.DoubleLine => 1,

		FormationType.Diamond => 1.1,

		FormationType.Echelon when Battle.FormationEnemy is FormationType.DoubleLine => 1.45,
		FormationType.Echelon => 1.4,

		FormationType.LineAbreast => 1.3,

		_ => 1
	};

	protected override double VanguardBonus => 0;

	protected override double PostcapBonus => 0;

	protected override double EquipmentUpgradeBonus => Ship.AllSlotInstance
		.Where(e => e?.MasterEquipment.CategoryType is EquipmentTypes.Engine)
		.Sum(e => 1.5 * Math.Sqrt(e!.Level));

	protected override double PostcapModifier => 1;

}
