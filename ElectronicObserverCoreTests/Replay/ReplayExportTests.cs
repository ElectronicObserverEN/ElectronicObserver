using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;
using Xunit;

namespace ElectronicObserverCoreTests.Replay;

public class ReplayExportTests
{
	private static string DirectoryName => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
	private static string RelativePath => "SortieDetail";
	private static string BasePath => Path.Join(DirectoryName, RelativePath);

	private static async Task<List<SortieRecord>> GetSortieRecords(string fileName)
	{
		await using Stream stream = File.OpenRead(Path.Join(BasePath, fileName));
		List<SortieRecord>? data = await JsonSerializer.DeserializeAsync<List<SortieRecord>>(stream);

		Assert.NotNull(data);
		Assert.NotEmpty(data);

		return data;
	}

	[Theory]
	[InlineData("SortieDetailTest1.json")]
	[InlineData("SortieDetailTest2.json")]
	[InlineData("SortieDetailTest4.json")]
	public async Task EquipmentImprovementIsExported(string fileName)
	{
		SortieRecord sortie = (await GetSortieRecords(fileName)).First();

		ReplayData replay = sortie.ToReplayData();

		List<ReplayShip> ships = new[] { replay.Fleet1, replay.Fleet2, replay.Fleet3, replay.Fleet4 }
			.SelectMany(f => f ?? new())
			.ToList();

		Assert.NotEmpty(ships);

		foreach (ReplayShip ship in ships)
		{
			Assert.Equal(ship.Equip.Count, ship.Stars.Count);
			Assert.Equal(ship.Equip.Count, ship.Ace.Count);
		}

		Assert.Contains(ships, s => s.Stars.Any(v => v > 0));
	}

	[Fact]
	public async Task ImprovementMatchesTheSourceRecord()
	{
		SortieRecord sortie = (await GetSortieRecords("SortieDetailTest2.json")).First();

		ReplayData replay = sortie.ToReplayData();

		List<ReplayShip> replayShips = replay.Fleet1!;
		List<SortieShip> sourceShips = sortie.FleetData.Fleets[0]!.Ships;

		Assert.Equal(sourceShips.Count, replayShips.Count);

		foreach ((SortieShip source, ReplayShip exported) in sourceShips.Zip(replayShips))
		{
			List<SortieEquipmentSlot?> slots = [.. source.EquipmentSlots, source.ExpansionSlot];

			Assert.Equal(slots.Select(s => (int)(s?.Equipment?.Id ?? 0)), exported.Equip);
			Assert.Equal(slots.Select(s => s?.Equipment?.Level ?? 0), exported.Stars);
			Assert.Equal(slots.Select(s => s?.Equipment?.AircraftLevel ?? 0), exported.Ace);
		}
	}

	[Fact]
	public async Task AirBasesAreExportedWhenWorldDiffersFromMap()
	{
		// world 58, map 4 - the filter used to compare MapAreaId against the map number
		SortieRecord sortie = (await GetSortieRecords("SortieDetailTest1.json")).First();

		Assert.NotEqual(sortie.World, sortie.Map);
		Assert.NotEmpty(sortie.FleetData.AirBases);

		ReplayData replay = sortie.ToReplayData();

		Assert.NotNull(replay.AirBases);
		Assert.Equal(sortie.FleetData.AirBases.Count, replay.AirBases!.Count);
	}
}
