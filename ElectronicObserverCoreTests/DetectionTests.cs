using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class DetectionTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "Can't get no plane detections with a carrier in fleet")]
	public void DetectionTests1()
	{
		ShipDataMock taihou = new(Db.MasterShips[ShipId.TaihouKai]);

		FleetDataMock fleet = new()
		{
			MembersInstance = new([taihou]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);
		Assert.DoesNotContain(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
	}

	[Fact(DisplayName = "Can get no plane detections with a seaplane tender in fleet")]
	public void DetectionTests2()
	{
		ShipDataMock akitsushima = new(Db.MasterShips[ShipId.AkitsushimaKai])
		{
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_FuMO25RADAR]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([akitsushima]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);
		Assert.Contains(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.Contains(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
	}

	[Fact(DisplayName = "Can't get no plane detections with a flying boat in fleet")]
	public void DetectionTests3()
	{
		ShipDataMock akitsushima = new(Db.MasterShips[ShipId.AkitsushimaKai])
		{
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.FlyingBoat_Type2LargeFlyingBoat]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([akitsushima]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);
		Assert.DoesNotContain(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
	}

	[Fact(DisplayName = "Can't get no plane detections with a seaplane recon in fleet")]
	public void DetectionTests4()
	{
		ShipDataMock akitsushima = new(Db.MasterShips[ShipId.AkitsushimaKai])
		{
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Ar196Kai]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([akitsushima]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);
		Assert.DoesNotContain(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
	}

	[Fact(DisplayName = "Can't get no plane detections with a seaplane bomber in fleet")]
	public void DetectionTests5()
	{
		ShipDataMock akitsushima = new(Db.MasterShips[ShipId.AkitsushimaKai])
		{
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneBomber_PrototypeNightZuiun_AttackEquipment]),
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([akitsushima]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);
		Assert.DoesNotContain(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
	}

	[Fact(DisplayName = "Ignore escaped ships")]
	public void DetectionTests6()
	{
		ShipDataMock tama = new(Db.MasterShips[ShipId.TamaKaiNi])
		{ 
			Level = 128,
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([tama, null, null, null, null, null, null]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);

		Assert.Contains(DetectionType.SuccessNoPlane, detectionProbabilities.Keys);
		Assert.Equal(0.85, detectionProbabilities[DetectionType.SuccessNoPlane], 3);

		Assert.Contains(DetectionType.FailureNoPlane, detectionProbabilities.Keys);
		Assert.Equal(0.15, detectionProbabilities[DetectionType.FailureNoPlane], 3);
	}

	[Fact(DisplayName = "0 slot recons don't contribute to detection")]
	public void DetectionTests7()
	{
		ShipDataMock iyo = new(Db.MasterShips[ShipId.I14Kai])
		{
			SlotInstance =
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_Prototype61cmSextuple_OxygenTorpedo]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Shiun_Skilled])
				{
					AircraftLevel = 7,
				},
			],
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([iyo]),
		};

		Dictionary<DetectionType, double> detectionProbabilities = fleet.GetDetectionProbabilities();

		Assert.NotEmpty(detectionProbabilities);

		Assert.DoesNotContain(DetectionType.Success, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.SuccessNoReturn, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.NoReturn, detectionProbabilities.Keys);
		Assert.DoesNotContain(DetectionType.Failure, detectionProbabilities.Keys);
	}
}
