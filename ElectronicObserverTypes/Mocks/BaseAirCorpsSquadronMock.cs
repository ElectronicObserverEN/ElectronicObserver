using System;

namespace ElectronicObserverTypes.Mocks;

public class BaseAirCorpsSquadronMock : IBaseAirCorpsSquadron
{
	public int SquadronID { get; set; }
	public int State { get; set; } = 1;
	public int EquipmentMasterID { get; set; }
	public IEquipmentData? EquipmentInstance { get; set; }
	public int EquipmentID { get; set; }
	public IEquipmentDataMaster? EquipmentInstanceMaster { get; set; }
	public int AircraftCurrent { get; set; }
	public int AircraftMax { get; set; }
	public int Condition { get; set; } = 49;
	public DateTime RelocatedTime { get; set; }
	public int ID { get; set; }
	public bool IsAvailable { get; set; }

	public BaseAirCorpsSquadronMock()
	{
		
	}

	public BaseAirCorpsSquadronMock(IEquipmentData equipment) : this()
	{
		EquipmentInstance = equipment;
		EquipmentInstanceMaster = equipment.MasterEquipment;
		AircraftCurrent = AirBaseAircraftCount(equipment.MasterEquipment);
		AircraftMax = AirBaseAircraftCount(equipment.MasterEquipment);
	}

	private static int AirBaseAircraftCount(IEquipmentDataMaster? equipment) => equipment?.CategoryType switch
	{
		null => 0,

		EquipmentTypes.CarrierBasedRecon => 4,
		EquipmentTypes.FlyingBoat => 4,
		EquipmentTypes.HeavyBomber => 9,

		_ => 18,
	};

	public void LoadFromResponse(string apiname, object elem)
	{
		throw new NotImplementedException();
	}
}
