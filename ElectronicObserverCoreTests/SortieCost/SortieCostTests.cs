using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
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

		Assert.Equal(firstSortieFleetCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(secondSortieFleetCost, sortieCosts[1].SortieFleetSupplyCost);

		Assert.Equal(airBaseSortieCost, sortieCosts[0].TotalAirBaseSortieCost);
		Assert.Equal(airBaseSortieCost, sortieCosts[1].TotalAirBaseSortieCost);

		Assert.Equal(new(), sortieCosts[0].TotalAirBaseSupplyCost);
		Assert.Equal(new(), sortieCosts[1].TotalAirBaseSupplyCost);

		Assert.Equal(firstSortieFleetCost + airBaseSortieCost, sortieCosts[0].TotalCost);
		Assert.Equal(secondSortieFleetCost + airBaseSortieCost, sortieCosts[1].TotalCost);
	}

	[Fact(DisplayName = "6-5 with AB")]
	public async Task SortieCostTest2()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("SortieCostTest02");

		Assert.Single(sortieCosts);

		SortieCostModel firstSortieFleetCost = new() { Fuel = 368, Ammo = 511, Bauxite = 150 };
		SortieCostModel airBase1SortieCost = new() { Fuel = 108, Ammo = 48 };
		SortieCostModel airBase2SortieCost = new() { Fuel = 99, Ammo = 47 };
		SortieCostModel airBase1ResupplyCost = new() { Fuel = 78, Ammo = 130 };
		SortieCostModel airBase2ResupplyCost = new() { Fuel = 81, Ammo = 135 };

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

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
		Assert.Equal(nodeSupportResupplyCost, sortieCosts[0].NodeSupportSupplyCost);
		Assert.Equal(bossSupportResupplyCost, sortieCosts[0].BossSupportSupplyCost);
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
}
