using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ElectronicObserverCoreTests.SortieCost.NagatoTouch;

#pragma warning disable xUnit1004
/// <summary>
/// Mutsu touch should be exactly the same, so it's not tested explicitly
/// </summary>
public sealed class NagatoTouchCostTests : SortieCostTestBase
{
	protected override string RelativePath => Path.Join("SortieCost", "NagatoTouch");

	[Theory(DisplayName = "no fleet after sortie data")]
	[InlineData("NagatoTouchCostTest01")]
	[InlineData("NagatoTouchCostTest02")]
	[InlineData("NagatoTouchCostTest03")]
	[InlineData("NagatoTouchCostTest04")]
	[InlineData("NagatoTouchCostTest05")]
	[InlineData("NagatoTouchCostTest06")]
	[InlineData("NagatoTouchCostTest07")]
	public override async Task SortieCostTest0(string testFilePrefix)
	{
		await base.SortieCostTest0(testFilePrefix);
	}

	[Fact(DisplayName = "combined vs combined, night S rank")]
	public async Task NagatoTouchCostTest1()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("NagatoTouchCostTest01", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 349, Ammo = 365, Bauxite = 440 };
		SortieCostModel escortResupplyCost = new() { Fuel = 62, Ammo = 69 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "combined vs combined, day S rank")]
	public async Task NagatoTouchCostTest2()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("NagatoTouchCostTest02", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 349, Ammo = 403, Bauxite = 330 };
		SortieCostModel escortResupplyCost = new() { Fuel = 62, Ammo = 69 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "combined vs single, night S rank")]
	public async Task NagatoTouchCostTest3()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("NagatoTouchCostTest03", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 395, Ammo = 497, Bauxite = 125 };
		SortieCostModel escortResupplyCost = new() { Fuel = 62, Ammo = 78 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "combined vs single, day S rank")]
	public async Task NagatoTouchCostTest4()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("NagatoTouchCostTest04", true);

		Assert.Single(sortieCosts);

		SortieCostModel mainResupplyCost = new() { Fuel = 395, Ammo = 466, Bauxite = 105 };
		SortieCostModel escortResupplyCost = new() { Fuel = 62, Ammo = 63 };

		Assert.Equal(mainResupplyCost + escortResupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "single vs single, night S rank", Skip = "todo")]
	public async Task NagatoTouchCostTest5()
	{
		// todo
		Assert.True(await Task.FromResult(true));
	}

	[Fact(DisplayName = "single vs single, day S rank")]
	public async Task NagatoTouchCostTest6()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("NagatoTouchCostTest06", true);

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 72, Ammo = 128 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "single vs single, night only", Skip = "todo")]
	public async Task NagatoTouchCostTest7()
	{
		// todo
		Assert.True(await Task.FromResult(true));
	}
}
