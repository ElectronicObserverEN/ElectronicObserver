using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class LoSTests
{
	private DatabaseFixture Db { get; }

	private int AdmiralLevel => 120;

	public LoSTests(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void LoSTest1TestData()
	{
		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]) {Level = 175},
				new ShipDataMock(Db.MasterShips[ShipId.Asakaze]) {Level = 175},
				new ShipDataMock(Db.MasterShips[ShipId.Harukaze]) {Level = 175},
				new ShipDataMock(Db.MasterShips[ShipId.Matsukaze]) {Level = 175},
				new ShipDataMock(Db.MasterShips[ShipId.Hatakaze]) {Level = 175},
			}),
		};

		double expected = -20.03;
		double actual = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);

		Assert.Equal(expected, actual.RoundDown(2));
	}

	[Fact]
	public void LoSTest2TestData()
	{
		ShipDataMock perth = new(Db.MasterShips[ShipId.PerthKai])
		{
			Level = 168,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon])
				{
					Level = 10,
				},
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				perth,
			}),
		};

		double expected1 = -21.08;
		double expected2 = -12.92;
		double expected3 = -4.77;
		double expected4 = 3.38;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	/// <summary>
	/// {"version":4,"hqlv":120,"f1":{"s1":{"id":586,"lv":151,"items":{"i1":{"id":102},"i2":{"id":102}},"hp":55,"luck":10},"s2":{"id":623,"lv":159,"items":{},"hp":47,"luck":30},"s3":{"id":568,"lv":162,"items":{},"hp":38,"luck":22},"s4":{"id":566,"lv":162,"items":{},"hp":38,"luck":20},"s5":{"id":567,"lv":161,"items":{},"hp":38,"luck":24},"s6":{"id":543,"lv":164,"items":{},"hp":38,"luck":30}},"a1":{"items":{},"mode":1},"a2":{"items":{},"mode":1},"a3":{"items":{},"mode":1}}
	/// </summary>
	[Fact]
	public void LoSTest3TestData()
	{
		ShipDataMock nisshin = new(Db.MasterShips[ShipId.NisshinA])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
			},
		};

		ShipDataMock yuubari = new(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 159,
		};

		ShipDataMock kuroshio = new(Db.MasterShips[ShipId.KuroshioKaiNi])
		{
			Level = 162,
		};

		ShipDataMock kagerou = new(Db.MasterShips[ShipId.KagerouKaiNi])
		{
			Level = 162,
		};

		ShipDataMock shiranui = new(Db.MasterShips[ShipId.ShiranuiKaiNi])
		{
			Level = 161,
		};

		ShipDataMock naganami = new(Db.MasterShips[ShipId.NaganamiKaiNi])
		{
			Level = 164,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				nisshin,
				yuubari,
				kuroshio,
				kagerou,
				shiranui,
				naganami,
			}),
		};

		double expected1 = 10.54;
		double expected2 = 17.74;
		double expected3 = 24.94;
		double expected4 = 32.14;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	/// <summary>
	/// {"version":4,"hqlv":120,"f1":{"s1":{"id":586,"lv":151,"items":{"i1":{"id":102},"i2":{"id":102},"i3":{"id":129}},"hp":55,"luck":10},"s2":{"id":623,"lv":159,"items":{"i1":{"id":129}},"hp":47,"luck":30},"s3":{"id":568,"lv":162,"items":{"i1":{"id":129}},"hp":38,"luck":22},"s4":{"id":566,"lv":162,"items":{"i1":{"id":129}},"hp":38,"luck":20},"s5":{"id":567,"lv":161,"items":{"i1":{"id":129}},"hp":38,"luck":24},"s6":{"id":543,"lv":164,"items":{"i1":{"id":129}},"hp":38,"luck":30}},"a1":{"items":{},"mode":1},"a2":{"items":{},"mode":1},"a3":{"items":{},"mode":1}}
	/// </summary>
	[Fact]
	public void LoSTest4TestData()
	{
		ShipDataMock nisshin = new(Db.MasterShips[ShipId.NisshinA])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type98ReconSeaplane_NightRecon]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ShipDataMock yuubari = new(Db.MasterShips[ShipId.YuubariKaiNiToku])
		{
			Level = 159,
			LosFit = 3,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ShipDataMock kuroshio = new(Db.MasterShips[ShipId.KuroshioKaiNi])
		{
			Level = 162,
			LosFit = 1,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ShipDataMock kagerou = new(Db.MasterShips[ShipId.KagerouKaiNi])
		{
			Level = 162,
			LosFit = 1,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ShipDataMock shiranui = new(Db.MasterShips[ShipId.ShiranuiKaiNi])
		{
			Level = 161,
			LosFit = 1,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		ShipDataMock naganami = new(Db.MasterShips[ShipId.NaganamiKaiNi])
		{
			Level = 164,
			LosFit = 1,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_SkilledLookouts]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				nisshin,
				yuubari,
				kuroshio,
				kagerou,
				shiranui,
				naganami,
			}),
		};

		double expected1 = 18.16;
		double expected2 = 32.56;
		double expected3 = 46.96;
		double expected4 = 61.36;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}

	[Fact]
	public void SgInitialFit()
	{
		IShipData fletcher = new ShipDataMock(Db.MasterShips[ShipId.FletcherMkII])
		{
			Level = 110,
			LosFit = 4,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_SGRadar_InitialModel]),
			},
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new(new List<IShipData?>
			{
				fletcher
			})
		};

		double expected1 = -24.60;
		double expected2 = -19.80;
		double expected3 = -15.00;
		double expected4 = -10.20;

		double actual1 = Calculator.GetSearchingAbility_New33(fleet, 1, AdmiralLevel);
		double actual2 = Calculator.GetSearchingAbility_New33(fleet, 2, AdmiralLevel);
		double actual3 = Calculator.GetSearchingAbility_New33(fleet, 3, AdmiralLevel);
		double actual4 = Calculator.GetSearchingAbility_New33(fleet, 4, AdmiralLevel);

		Assert.Equal(expected1, actual1.RoundDown(2));
		Assert.Equal(expected2, actual2.RoundDown(2));
		Assert.Equal(expected3, actual3.RoundDown(2));
		Assert.Equal(expected4, actual4.RoundDown(2));
	}
}
