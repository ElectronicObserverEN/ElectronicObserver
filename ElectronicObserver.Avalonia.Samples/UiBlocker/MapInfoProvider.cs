using System.Collections.Generic;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.UiBlocker;

namespace ElectronicObserver.Avalonia.Samples.UiBlocker;

public class MapInfoProvider : IMapInfoProvider
{
	public List<MapInfoModel> GetMaps() =>
	[
		new() { AreaId = 1, InfoId = 1 },
	];

	public List<Node> GetNodes(MapInfoModel map) =>
	[
		new("A", [1]),
		new("B", [2]),
		new("C", [3]),
	];
}
