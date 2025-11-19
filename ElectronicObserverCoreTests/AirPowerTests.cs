using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AirPowerTests
{
	private DatabaseFixture Db { get; }

	public AirPowerTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact(DisplayName = "AS patrol")]
	public void AirPowerTest1()
	{
		ShipDataMock hyuuga = new(Db.MasterShips[ShipId.HyuugaKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type1FighterHayabusaModelIIKai_20thSquadron])
				{
					Level = 10,
					AircraftLevel = 7,
				},
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				hyuuga,
			}),
		};

		Assert.Equal(33, Calculator.GetAirSuperiority(fleet));

		hyuuga.SlotInstance = new List<IEquipmentData?>
		{
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraftKai])
			{
				Level = 10,
				AircraftLevel = 7,
			},
			new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
			{
				Level = 10,
				AircraftLevel = 7,
			},
		};

		Assert.Equal(0, Calculator.GetAirSuperiority(fleet));
	}

	[Fact]
	public void AirPowerTest2()
	{
		ShipDataMock taihou = new(Db.MasterShips[ShipId.TaihouKai])
		{
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron])
				{
					AircraftLevel = 6,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_MurataSquadron])
				{
					Level = 2,
					AircraftLevel = 7,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12_MurataSquadron])
				{
					AircraftLevel = 7,
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.CarrierBasedFighter_PrototypeJinpuu])
				{
					AircraftLevel = 7,
				},
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new ReadOnlyCollection<IShipData?>(new List<IShipData?>
			{
				taihou,
			}),
		};

		Assert.Equal(85, Calculator.GetAirSuperiority(fleet));
	}

	[Fact(DisplayName = "AB fighter")]
	public void AirPowerTest3()
	{
		EquipmentDataMock eq = new(Db.MasterEquipment[EquipmentId.Interceptor_Type1FighterHayabusaModelII_64thSquadron])
		{
			AircraftLevel = 7,
		};

		BaseAirCorpsSquadronMock squadron = new(eq);

		Assert.Equal(103, Calculator.GetAirSuperiority(squadron, AirBaseActionKind.Mission));
	}

	[Fact(DisplayName = "AB AS Patrol")]
	public void AirPowerTest4()
	{
		EquipmentDataMock eq = new(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
		{
			AircraftLevel = 7,
		};

		BaseAirCorpsSquadronMock squadron = new(eq);

		Assert.Equal(0, Calculator.GetAirSuperiority(squadron, AirBaseActionKind.Mission));
	}

	[Fact(DisplayName = "AB AS Patrol Hayabusa")]
	public void AirPowerTest5()
	{
		EquipmentDataMock eq = new(Db.MasterEquipment[EquipmentId.ASPatrol_Type1FighterHayabusaModelIIKai_20thSquadron])
		{
			Level = 10,
			AircraftLevel = 7,
		};

		BaseAirCorpsSquadronMock squadron = new(eq);

		Assert.Equal(50, Calculator.GetAirSuperiority(squadron, AirBaseActionKind.Mission));
	}

	[Fact(DisplayName = "AB Autogyro")]
	public void AirPowerTest6()
	{
		EquipmentDataMock eq = new(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai])
		{
			Level = 10,
			AircraftLevel = 7,
		};

		BaseAirCorpsSquadronMock squadron = new(eq);

		Assert.Equal(0, Calculator.GetAirSuperiority(squadron, AirBaseActionKind.Mission));
	}

	[Fact(DisplayName = "AB")]
	public void AirPowerTest7()
	{
		EquipmentDataMock eq1 = new(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq2 = new(Db.MasterEquipment[EquipmentId.Interceptor_Type1FighterHayabusaModelIIIA_54thSquadron])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq3 = new(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron])
		{
			Level = 10,
			AircraftLevel = 7,
		};

		BaseAirCorpsDataMock airbase = new()
		{
			ActionKind = AirBaseActionKind.Mission,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{ 1, new BaseAirCorpsSquadronMock(eq1) },
				{ 2, new BaseAirCorpsSquadronMock(eq2) },
				{ 3, new BaseAirCorpsSquadronMock(eq3) },
			},
		};

		Assert.Equal(121, Calculator.GetAirSuperiority(airbase));
	}

	[Fact(DisplayName = "High Altitude with normal planes")]
	public void HighAltitudeTest1()
	{
		EquipmentDataMock eq1 = new(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq2 = new(Db.MasterEquipment[EquipmentId.Interceptor_Type1FighterHayabusaModelIIIA_54thSquadron])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq3 = new(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron])
		{
			Level = 10,
			AircraftLevel = 7,
		};

		BaseAirCorpsDataMock airbase1 = new()
		{
			ActionKind = AirBaseActionKind.AirDefense,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{ 1, new BaseAirCorpsSquadronMock(eq1) },
				{ 2, new BaseAirCorpsSquadronMock(eq2) },
			},
		};

		BaseAirCorpsDataMock airbase2 = new()
		{
			ActionKind = AirBaseActionKind.AirDefense,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{ 1, new BaseAirCorpsSquadronMock(eq3) },
			},
		};

		Assert.Equal(40, Calculator.GetHighAltitudeAirSuperiority(airbase1, [airbase1, airbase2], true));
		Assert.Equal(21, Calculator.GetHighAltitudeAirSuperiority(airbase2, [airbase1, airbase2], true));
	}

	[Fact(DisplayName = "High Altitude with HA planes")]
	public void HighAltitudeTest2()
	{
		EquipmentDataMock eq1 = new(Db.MasterEquipment[EquipmentId.ASPatrol_Type3CommandLiaisonAircraft_ASW])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq2 = new(Db.MasterEquipment[EquipmentId.Interceptor_Type1FighterHayabusaModelIIIA_54thSquadron])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq3 = new(Db.MasterEquipment[EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron])
		{
			Level = 10,
			AircraftLevel = 7,
		};

		EquipmentDataMock eq4 = new(Db.MasterEquipment[EquipmentId.Interceptor_PrototypeShuusui])
		{
			AircraftLevel = 7,
		};

		EquipmentDataMock eq5 = new(Db.MasterEquipment[EquipmentId.Interceptor_Shuusui])
		{
			AircraftLevel = 7,
		};

		BaseAirCorpsDataMock airbase1 = new()
		{
			ActionKind = AirBaseActionKind.AirDefense,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{ 1, new BaseAirCorpsSquadronMock(eq1) },
				{ 2, new BaseAirCorpsSquadronMock(eq2) },
			},
		};

		BaseAirCorpsDataMock airbase2 = new()
		{
			ActionKind = AirBaseActionKind.AirDefense,
			Squadrons = new Dictionary<int, IBaseAirCorpsSquadron>
			{
				{ 1, new BaseAirCorpsSquadronMock(eq3) },
				{ 2, new BaseAirCorpsSquadronMock(eq4) },
				{ 3, new BaseAirCorpsSquadronMock(eq5) },
			},
		};

		Assert.Equal(88, Calculator.GetHighAltitudeAirSuperiority(airbase1, [airbase1, airbase2], true));
		Assert.Equal(283, Calculator.GetHighAltitudeAirSuperiority(airbase2, [airbase1, airbase2], true));
	}
}
