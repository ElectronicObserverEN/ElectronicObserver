using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Extensions;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class AirAttackCutInTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.JetFighterJetBomberJetBomber))]
	public void AirAttackCutInTest1()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.JetFighter_ShindenKai3_PrototypeJetShinden],
			Db.MasterEquipment[EquipmentId.JetBomber_KikkaKai],
			Db.MasterEquipment[EquipmentId.JetBomber_JetKeiunKai],
		];

		Assert.Equal(DayAirAttackCutinKind.JetFighterJetBomberJetBomber, displayEquipment.GetDayAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.JetFighterJetBomber))]
	public void AirAttackCutInTest2()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.JetFighter_ShindenKai3_PrototypeJetShinden],
			Db.MasterEquipment[EquipmentId.JetBomber_KikkaKai],
		];

		Assert.Equal(DayAirAttackCutinKind.JetFighterJetBomber, displayEquipment.GetDayAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.JetFighterBomberAttacker))]
	public void AirAttackCutInTest3()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.JetFighter_ShindenKai3_PrototypeJetShinden],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_RyuuseiKai_TomonagaSquadron],
		];

		Assert.Equal(DayAirAttackCutinKind.JetFighterBomberAttacker, displayEquipment.GetDayAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.FighterBomberAttacker))]
	public void AirAttackCutInTest4()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ShindenKaiNi_CarrierFighterKaiNi],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_RyuuseiKai_TomonagaSquadron],
		];

		Assert.Equal(DayAirAttackCutinKind.FighterBomberAttacker, displayEquipment.GetDayAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.BomberBomberAttacker))]
	public void AirAttackCutInTest5()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_RyuuseiKai_TomonagaSquadron],
		];

		Assert.Equal(DayAirAttackCutinKind.BomberBomberAttacker, displayEquipment.GetDayAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(DayAirAttackCutinKind.BomberAttacker))]
	public void AirAttackCutInTest6()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Suisei_EgusaSquadron],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_RyuuseiKai_TomonagaSquadron],
		];

		Assert.Equal(DayAirAttackCutinKind.BomberAttacker, displayEquipment.GetDayAirAttackCutinKind());
	}
}
