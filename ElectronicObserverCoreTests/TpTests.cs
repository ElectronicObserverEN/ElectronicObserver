using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class TpTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	/// <summary>
	/// <see href="https://tinyurl.com/2cgrrkst" />
	/// </summary>
	[Fact(DisplayName = "Kinu k2 dupe")]
	public void TpTest1()
	{
		FleetDataMock main = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KinuKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.PerthKai]),
			])
		};

		FleetDataMock escort = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KinuKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
			])
		};

		Assert.Equal(24, TpGauge.Normal.GetTp([main, escort]));
		Assert.Equal(17, TpGauge.Spring25E2.GetTp([main, escort]));
		Assert.Equal(20, TpGauge.Spring25E5.GetTp([main, escort]));
	}

	/// <summary>
	/// <see href="https://tinyurl.com/2dok47bg" />
	/// </summary>
	[Fact(DisplayName = "daihatsu fiesta")]
	public void TpTest2()
	{
		FleetDataMock main = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KinuKaiNi])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_DaihatsuLC]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_DaihatsuLC_Type89Tank_LandingForce]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsuLC]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.PerthKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.TransportContainer_Drum_Transport]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.No101LandingShipKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ArmyInfantry_ArmyInfantryUnit]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ArmyInfantry_Type97MediumTank_ChiHa]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ArmyInfantry_Type97MediumTankNewTurret_ChiHaKai]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.ShinshuuMaruKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsuLC_11thTankRegiment]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_M4A1DD]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_DaihatsuLandingCraft_PanzerIINorthAfricanSpecification]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_ArmedDaihatsu]),
					],
					ExpansionSlotInstance = new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_Soukoutei_ABClass]),
				},
				new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNiJuu])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_Type1GunTank]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIII_NorthAfricanCorps]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsu_ChiHa]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsu_ChiHaKai]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.LandingCraft_TokuDaihatsuLandingCraft_PanzerIIITypeJ]),
					],
				},
				new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SpecialAmphibiousTank_SpecialType2AmphibiousTank]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTank]),
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SpecialAmphibiousTank_SpecialType4AmphibiousTankKai]),
					],
				},
			])
		};

		FleetDataMock escort = new()
		{
			MembersInstance = new(
			[
				new ShipDataMock(Db.MasterShips[ShipId.KinuKaiNi]),
				new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
				new ShipDataMock(Db.MasterShips[ShipId.No101LandingShipKai])
				{
					SlotInstance =
					[
						new EquipmentDataMock(Db.MasterEquipment[EquipmentId.ArmyInfantry_ArmyInfantryCorps_ChiHaKai]),
					],
				},
			])
		};

		Assert.Equal(182, TpGauge.Normal.GetTp([main, escort]));
		Assert.Equal(442, TpGauge.Spring25E2.GetTp([main, escort]));
		Assert.Equal(345, TpGauge.Spring25E5.GetTp([main, escort]));
	}
}
