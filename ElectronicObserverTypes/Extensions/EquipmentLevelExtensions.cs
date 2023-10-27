namespace ElectronicObserverTypes.Extensions;

public static class EquipmentLevelExtensions
{
	public static double AircraftAaLevelCoefficient(this IEquipmentDataMaster equip) => equip.CategoryType switch
	{
		EquipmentTypes.CarrierBasedFighter or
		EquipmentTypes.SeaplaneFighter or
		EquipmentTypes.Interceptor => 0.2,

		EquipmentTypes.LandBasedAttacker or
		EquipmentTypes.HeavyBomber => 0.5,

		EquipmentTypes.CarrierBasedBomber => equip.EquipmentId switch
		{
			EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron or
			EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62 or
			EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel63 or
			EquipmentId.CarrierBasedBomber_Type0FighterModel64_TwoseatwKMX or
			EquipmentId.CarrierBasedBomber_Type0FighterModel64_SkilledFighterBomber => 0.25,

			_ => 0,
		},

		_ => 0,
	};
}
