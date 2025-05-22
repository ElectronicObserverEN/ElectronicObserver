using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Evasion;

internal class NightEvasion : EvasionBase
{
	public NightEvasion(IShipData ship, IFleetData? fleet = null, BattleDataMock? battle = null)
		: base(ship, fleet, battle)
	{
	}

	protected override double FormationModifier => Battle.FormationFriend switch
	{
		FormationType.LineAhead => 1,
		FormationType.Diamond => 1,
		FormationType.DoubleLine => 1,

		FormationType.Echelon => 1.1,

		FormationType.LineAbreast => 1.2,

		_ => 1
	};

	protected override double VanguardBonus => 0;

	protected override double PostcapBonus => HeavyCruiserBonus + SkilledLookoutsBonus;

	// boiler upgrades aren't verified yet
	protected override double EquipmentUpgradeBonus => 0;

	protected override double PostcapModifier => Ship.HasSearchlight() switch
	{
		true => 0.2,
		_ => 1
	};

	private double HeavyCruiserBonus => Ship.MasterShip.ShipType switch
	{
		ShipTypes.HeavyCruiser or ShipTypes.AviationCruiser => 5,
		_ => 0
	};

	private double SkilledLookoutsBonus => Ship.MasterShip.ShipType switch
	{
		ShipTypes.Destroyer when Ship.HasSkilledLookouts() && Ship.HasSurfaceRadar() => 10,
		_ => 0
	};
}
