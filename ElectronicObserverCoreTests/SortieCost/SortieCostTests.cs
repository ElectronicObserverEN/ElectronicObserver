using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverCoreTests.SortieCost;

public sealed class SortieCostTests : SortieCostTestBase
{
	protected override string RelativePath => "SortieCost";

	[Theory(DisplayName = "no fleet after sortie data")]
	[InlineData("SortieCostTest01")]
	[InlineData("SortieCostTest02")]
	[InlineData("SortieCostTest03")]
	[InlineData("SortieCostTest04")]
	[InlineData("SortieCostTest05")]
	[InlineData("SortieCostTest06")]
	[InlineData("SortieCostTest07")]
	[InlineData("SortieCostTest08")]
	[InlineData("SortieCostTest09")]
	[InlineData("SortieCostTest10")]
	[InlineData("SortieCostTest11")]
	[InlineData("SortieCostTest12")]
	[InlineData("SortieCostTest13")]
	[InlineData("SortieCostTest14")]
	[InlineData("SortieCostTest15", Skip = "todo")]
	[InlineData("SortieCostTest16", Skip = "todo")]
	[InlineData("SortieCostTest17", Skip = "todo")]
	public override async Task SortieCostTest0(string testFilePrefix)
	{
		await base.SortieCostTest0(testFilePrefix);
	}

	[Fact(DisplayName = "Double 7-4 resource run without resupply with AB")]
	public async Task SortieCostTest1()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest01");

		Assert.Equal(2, sortieCosts.Count);

		SortieCostModel firstSortieFleetCost = new() { Fuel = 33, Ammo = 31 };
		SortieCostModel secondSortieFleetCost = new() { Fuel = 27, Ammo = 32 };
		SortieCostModel airBaseSortieCost = new() { Fuel = 8, Ammo = 6 };
		SortieCostModel resourceGain = new() { Fuel = 86, Bauxite = 64 };

		Assert.Equal(firstSortieFleetCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(secondSortieFleetCost, sortieCosts[1].SortieFleetSupplyCost);

		Assert.Equal(airBaseSortieCost, sortieCosts[0].TotalAirBaseSortieCost);
		Assert.Equal(airBaseSortieCost, sortieCosts[1].TotalAirBaseSortieCost);

		Assert.Equal(SortieCostModel.Zero, sortieCosts[0].TotalAirBaseSupplyCost);
		Assert.Equal(SortieCostModel.Zero, sortieCosts[1].TotalAirBaseSupplyCost);

		Assert.Equal(firstSortieFleetCost + airBaseSortieCost - resourceGain, sortieCosts[0].TotalCost);
		Assert.Equal(secondSortieFleetCost + airBaseSortieCost - resourceGain, sortieCosts[1].TotalCost);
	}

	[Fact(DisplayName = "6-5 with AB")]
	public async Task SortieCostTest2()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest02");

		Assert.Single(sortieCosts);

		SortieCostModel firstSortieFleetCost = new() { Fuel = 368, Ammo = 511, Bauxite = 150 };
		SortieCostModel airBase1SortieCost = new() { Fuel = 108, Ammo = 48 };
		SortieCostModel airBase2SortieCost = new() { Fuel = 99, Ammo = 47 };
		SortieCostModel airBase1ResupplyCost = new() { Fuel = 78, Bauxite = 130 };
		SortieCostModel airBase2ResupplyCost = new() { Fuel = 81, Bauxite = 135 };

		Assert.Equal(firstSortieFleetCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(airBase1SortieCost + airBase2SortieCost, sortieCosts[0].TotalAirBaseSortieCost);
		Assert.Equal(airBase1ResupplyCost + airBase2ResupplyCost, sortieCosts[0].TotalAirBaseSupplyCost);
	}

	[Fact(DisplayName = "Sortie record version 0→1 test")]
	public async Task SortieCostTest3()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest03");

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 525, Ammo = 532, Bauxite = 200 };
		SortieCostModel souyaRepairCost = new() { Fuel = 16, Steel = 30 };
		SortieCostModel musashiRepairCost = new() { Fuel = 52, Steel = 99 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(souyaRepairCost + musashiRepairCost, sortieCosts[0].SortieFleetRepairCost);
	}

	[Fact(DisplayName = "Refreshing a battle before battle result ignores fuel/ammo cost")]
	public async Task SortieCostTest4()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest04", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 131, Ammo = 74, Bauxite = 880 };
		SortieCostModel escortResupplyCost = new() { Fuel = 39, Ammo = 25 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "combined vs combined, friend fleet, night S rank")]
	public async Task SortieCostTest5()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest05", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 197, Ammo = 140, Bauxite = 780 };
		SortieCostModel escortResupplyCost = new() { Fuel = 57, Ammo = 43 };
		SortieCostModel nodeSupportResupplyCost = new() { Fuel = 99, Ammo = 81, Bauxite = 20 };
		SortieCostModel bossSupportResupplyCost = new() { Fuel = 176, Ammo = 406 };
		SortieCostModel airBaseSortieCost = new() { Fuel = 219, Ammo = 113 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(nodeSupportResupplyCost, sortieCosts[0].NodeSupportSupplyCost);
		Assert.Equal(bossSupportResupplyCost, sortieCosts[0].BossSupportSupplyCost);
		Assert.Equal(airBaseSortieCost, sortieCosts[0].TotalAirBaseSortieCost);
	}

	[Fact(DisplayName = "single vs combined, night S rank")]
	public async Task SortieCostTest6()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest06", true);

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 81, Ammo = 97 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "single vs single, night S rank")]
	public async Task SortieCostTest7()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest07", true);

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 197, Ammo = 299, Bauxite = 190 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "Sortie record version 0→1 test, missing fleet after sortie data and initial HP value")]
	public async Task SortieCostTest8()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest08");

		Assert.Single(sortieCosts);

		Assert.Equal(SortieCostModel.Zero, sortieCosts[0].TotalRepairCost);
	}

	[Fact(DisplayName = "All subs boss consumption is 0.2, 0.2")]
	public async Task SortieCostTest9()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest09", true);

		Assert.Single(sortieCosts);

		SortieCostModel supplyCost = new() { Fuel = 18, Ammo = 6 };

		Assert.Equal(supplyCost, sortieCosts[0].TotalSupplyCost);
	}

	[Fact(DisplayName = "6-1 - no air base sortie cost even if bases are set to sortie")]
	public async Task SortieCostTest10()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest10", true);

		Assert.Single(sortieCosts);

		Assert.Equal(SortieCostModel.Zero, sortieCosts[0].TotalAirBaseSortieCost);
	}

	[Fact(DisplayName = "planes lost in air base raid")]
	public async Task SortieCostTest11()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest11", true);

		Assert.Single(sortieCosts);

		SortieCostModel airBaseSupplyCost = new() { Fuel = 72, Bauxite = 120 };

		Assert.Equal(airBaseSupplyCost, sortieCosts[0].TotalAirBaseSupplyCost);
	}

	[Fact(DisplayName = "resource gain")]
	public async Task SortieCostTest12()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest12", true);

		Assert.Single(sortieCosts);

		SortieCostModel resourceGain = new() { Fuel = 60, Ammo = 15 };

		Assert.Equal(resourceGain, sortieCosts[0].ResourceGain);
	}

	[Fact(DisplayName = "sink resource gain")]
	public async Task SortieCostTest13()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest13", true);

		Assert.Single(sortieCosts);

		SortieCostModel resourceGain = new() { Fuel = 128, Bauxite = 120 };
		SortieCostModel sinkResourceGain = new() { Fuel = 37, Ammo = 65 };

		Assert.Equal(SortieCostModel.Zero, sortieCosts[0].TotalRepairCost);
		Assert.Equal(resourceGain, sortieCosts[0].ResourceGain);
		Assert.Equal(sinkResourceGain, sortieCosts[0].SinkingResourceGain);
	}

	[Fact(DisplayName = "consumed items test 1")]
	public async Task SortieCostTest14()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest14");
		List<SortieCostViewModel> calculatedSortieCosts = await MakeSortieCosts("SortieCostTest14", true);

		AssertSortieCosts(sortieCosts);
		// todo AssertSortieCosts(calculatedSortieCosts);
		return;

		static void AssertSortieCosts(List<SortieCostViewModel> sortieCosts)
		{
			Assert.Single(sortieCosts);

			Assert.Single(sortieCosts[0].ConsumedItems);
			Assert.Equal(EquipmentId.DamageControl_EmergencyRepairPersonnel, sortieCosts[0].ConsumedItems[0].Id);
			Assert.Equal(1, sortieCosts[0].ConsumedItems[0].Count);
		}
	}

	[Fact(DisplayName = "consumed items test 2")]
	public async Task SortieCostTest15()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest15");
		List<SortieCostViewModel> calculatedSortieCosts = await MakeSortieCosts("SortieCostTest15", true);

		AssertSortieCosts(sortieCosts);
		// todo AssertSortieCosts(calculatedSortieCosts);
		return;

		static void AssertSortieCosts(List<SortieCostViewModel> sortieCosts)
		{
			Assert.Single(sortieCosts);

			Assert.Single(sortieCosts[0].ConsumedItems);
			Assert.Equal(EquipmentId.DamageControl_EmergencyRepairGoddess, sortieCosts[0].ConsumedItems[0].Id);
			Assert.Equal(1, sortieCosts[0].ConsumedItems[0].Count);
		}
	}

	[Fact(DisplayName = "bucket cost test - sunk ships")]
	public async Task SortieCostTest16()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest16");
		List<SortieCostViewModel> calculatedSortieCosts = await MakeSortieCosts("SortieCostTest16", true);

		AssertSortieCosts(sortieCosts);
		AssertSortieCosts(calculatedSortieCosts);
		return;

		static void AssertSortieCosts(List<SortieCostViewModel> sortieCosts)
		{
			Assert.Single(sortieCosts);
			Assert.Equal(3, sortieCosts[0].Buckets);
		}
	}

	[Fact(DisplayName = "consumed items test 3")]
	public async Task SortieCostTest17()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest17");
		List<SortieCostViewModel> calculatedSortieCosts = await MakeSortieCosts("SortieCostTest17", true);

		AssertSortieCosts(sortieCosts);
		// todo AssertSortieCosts(calculatedSortieCosts);
		return;

		static void AssertSortieCosts(List<SortieCostViewModel> sortieCosts)
		{
			Assert.Single(sortieCosts); 
			
			Assert.Single(sortieCosts[0].ConsumedItems);
			Assert.Equal(EquipmentId.DamageControl_EmergencyRepairGoddess, sortieCosts[0].ConsumedItems[0].Id);
			Assert.Equal(8, sortieCosts[0].Buckets);
		}
	}
}
