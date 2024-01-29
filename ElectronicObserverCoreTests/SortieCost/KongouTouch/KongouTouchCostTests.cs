using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ElectronicObserverCoreTests.SortieCost.KongouTouch;

#pragma warning disable xUnit1004
/// <summary>
/// Hiei, Haruna touch should be exactly the same, so it's not tested explicitly
/// </summary>
public sealed class KongouTouchCostTests : SortieCostTestBase
{
	protected override string RelativePath => Path.Join("SortieCost", "KongouTouch");

	[Theory(DisplayName = "no fleet after sortie data")]
	[InlineData("KongouTouchCostTest01")]
	[InlineData("KongouTouchCostTest02")]
	[InlineData("KongouTouchCostTest03")]
	[InlineData("KongouTouchCostTest04")]
	[InlineData("KongouTouchCostTest05")]
	public override async Task SortieCostTest0(string testFilePrefix)
	{
		await base.SortieCostTest0(testFilePrefix);
	}

	[Fact(DisplayName = "combined vs combined", Skip = "todo")]
	public async Task KongouTouchCostTest1()
	{
		// todo
		Assert.True(await Task.FromResult(true));
	}

	[Fact(DisplayName = "combined vs single", Skip = "todo")]
	public async Task KongouTouchCostTest2()
	{
		// todo
		Assert.True(await Task.FromResult(true));
	}

	[Fact(DisplayName = "single vs combined", Skip = "todo")]
	public async Task KongouTouchCostTest3()
	{
		// todo
		Assert.True(await Task.FromResult(true));
	}

	[Fact(DisplayName = "single vs single")]
	public async Task KongouTouchCostTest4()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("KongouTouchCostTest04", true);

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 40, Ammo = 111 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}

	[Fact(DisplayName = "single vs single, night only")]
	public async Task KongouTouchCostTest5()
	{
		List<SortieCostViewModel> sortieCosts = await MakeSortieCosts("KongouTouchCostTest05", true);

		Assert.Single(sortieCosts);

		SortieCostModel resupplyCost = new() { Fuel = 19, Ammo = 35 };

		Assert.Equal(resupplyCost, sortieCosts[0].SortieFleetSupplyCost);
	}
}
