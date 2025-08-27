namespace ElectronicObserver.Core.Types.Evasion;

internal class AirstrikeEvasion : EvasionBase
{
	public AirstrikeEvasion(IShipData ship, IFleetData? fleet = null, BattleDataMock? battle = null)
		: base(ship, fleet, battle)
	{
	}

	protected override double FormationModifier => Battle.FormationFriend switch
	{
		FormationType.LineAhead => 1,
		FormationType.Echelon => 1,
		FormationType.LineAbreast => 1,

		FormationType.DoubleLine => 1.1,

		FormationType.Diamond => 1.6,

		_ => 1
	};

	protected override double VanguardBonus => 0;

	protected override double PostcapBonus => 0;

	// boiler upgrades aren't verified yet
	protected override double EquipmentUpgradeBonus => 0;

	protected override double PostcapModifier => 1;

}
