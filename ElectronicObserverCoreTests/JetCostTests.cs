using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using Xunit;

namespace ElectronicObserverCoreTests;

/// <summary>
/// <see href="https://wikiwiki.jp/kancolle/%E5%99%B4%E5%BC%8F%E6%99%AF%E9%9B%B2%E6%94%B9#eaec51fd" />
/// </summary>
[Collection(DatabaseCollection.Name)]
public class JetCostTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Theory(DisplayName = "Keiun")]
	[InlineData(2, 6)]
	[InlineData(4, 11)]
	[InlineData(6, 17)]
	[InlineData(8, 22)]
	[InlineData(9, 25)]
	[InlineData(12, 34)]
	[InlineData(16, 45)]
	[InlineData(18, 50)]
	[InlineData(19, 53)]
	[InlineData(21, 59)]
	[InlineData(24, 67)]
	[InlineData(34, 95)]
	public void JetCostTest1(int aircraft, int expectedCost)
	{
		IEquipmentDataMaster eq = Db.MasterEquipment[EquipmentId.JetBomber_JetKeiunKai];

		Assert.Equal(expectedCost, eq.JetSteelCost(aircraft));
	}

	[Theory(DisplayName = "Kikka")]
	[InlineData(2, 5)]
	[InlineData(4, 10)]
	[InlineData(6, 16)]
	[InlineData(8, 21)]
	[InlineData(9, 23)]
	[InlineData(12, 31)]
	[InlineData(16, 42)]
	[InlineData(18, 47)]
	[InlineData(19, 49)]
	[InlineData(21, 55)]
	[InlineData(24, 62)]
	[InlineData(34, 88)]
	public void JetCostTest2(int aircraft, int expectedCost)
	{
		IEquipmentDataMaster eq = Db.MasterEquipment[EquipmentId.JetBomber_KikkaKai];

		Assert.Equal(expectedCost, eq.JetSteelCost(aircraft));
	}

	[Theory(DisplayName = "Ho229")]
	[InlineData(2, 7)]
	[InlineData(4, 13)]
	[InlineData(6, 20)]
	[InlineData(8, 27)]
	[InlineData(9, 30)]
	[InlineData(12, 40)]
	[InlineData(16, 54)]
	[InlineData(18, 60)]
	public void JetCostTest3(int aircraft, int expectedCost)
	{
		IEquipmentDataMaster eq = Db.MasterEquipment[EquipmentId.JetBomber_Ho229];

		Assert.Equal(expectedCost, eq.JetSteelCost(aircraft));
	}

	[Theory(DisplayName = "Shinden")]
	[InlineData(2, 4)]
	[InlineData(4, 9)]
	[InlineData(6, 13)]
	[InlineData(8, 18)]
	[InlineData(9, 20)]
	[InlineData(12, 26)]
	[InlineData(16, 35)]
	[InlineData(18, 40)]
	[InlineData(19, 42)]
	[InlineData(21, 46)]
	[InlineData(24, 53)]
	[InlineData(34, 75)]
	public void JetCostTest4(int aircraft, int expectedCost)
	{
		IEquipmentDataMaster eq = Db.MasterEquipment[EquipmentId.JetFighter_ShindenKai3_PrototypeJetShinden];

		Assert.Equal(expectedCost, eq.JetSteelCost(aircraft));
	}
}
