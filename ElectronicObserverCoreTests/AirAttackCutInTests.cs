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

	[Fact(DisplayName = nameof(CvnciKind.FighterFighterAttacker))]
	public void AirAttackCutInTest7()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12AKaiNi_MurataSquadronwRadar],
		];

		Assert.Equal(CvnciKind.FighterFighterAttacker, displayEquipment.GetNightAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(CvnciKind.FighterAttacker))]
	public void AirAttackCutInTest8()
	{
		List<IEquipmentDataMaster> displayEquipment =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12AKaiNi_MurataSquadronwRadar],
		];

		Assert.Equal(CvnciKind.FighterAttacker, displayEquipment.GetNightAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(CvnciKind.Phototube))]
	public void AirAttackCutInTest9()
	{
		List<IEquipmentDataMaster> displayEquipment1 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs],
		];

		Assert.Equal(CvnciKind.Phototube, displayEquipment1.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment2 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12AKaiNi_MurataSquadronwRadar],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs],
		];

		Assert.Equal(CvnciKind.Phototube, displayEquipment2.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment3 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
		];

		Assert.Equal(CvnciKind.Phototube, displayEquipment3.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment4 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12AKaiNi_MurataSquadronwRadar],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
		];

		Assert.Equal(CvnciKind.Phototube, displayEquipment4.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment5 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs],
		];

		Assert.Equal(CvnciKind.Phototube, displayEquipment5.GetNightAirAttackCutinKind());
	}

	[Fact(DisplayName = nameof(CvnciKind.FighterOtherOther))]
	public void AirAttackCutInTest10()
	{
		List<IEquipmentDataMaster> displayEquipment1 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_TenzanModel12AKaiNi_MurataSquadronwRadar],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs],
		];

		Assert.Equal(CvnciKind.FighterOtherOther, displayEquipment1.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment2 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs],
			Db.MasterEquipment[EquipmentId.CarrierBasedTorpedo_SwordfishMk_III_Skilled],
		];

		Assert.Equal(CvnciKind.FighterOtherOther, displayEquipment2.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment3 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_ZeroFighterbomberModel62_IwaiSquadron],
		];

		Assert.Equal(CvnciKind.FighterOtherOther, displayEquipment3.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment4 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
			Db.MasterEquipment[EquipmentId.CarrierBasedBomber_Type0FighterModel62Kai_SkilledNightFighterbomber],
		];

		Assert.Equal(CvnciKind.FighterOtherOther, displayEquipment4.GetNightAirAttackCutinKind());

		List<IEquipmentDataMaster> displayEquipment5 =
		[
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
			Db.MasterEquipment[EquipmentId.CarrierBasedFighter_ReppuuKaiNiModelE_CarDiv1Skilled],
		];

		Assert.Equal(CvnciKind.FighterOtherOther, displayEquipment5.GetNightAirAttackCutinKind());
	}
}
