using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Avalonia.UiBlocker;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class MapInfoProvider : IMapInfoProvider
{
	public List<MapInfoModel> GetMaps() =>
	[
		.. KCDatabase.Instance.MapInfo.Values
			.Select(m => new MapInfoModel { AreaId = m.MapAreaID, InfoId = m.MapInfoID, }),
	];

	public List<Node> GetNodes(MapInfoModel map) =>
		KCDatabase.Instance.Translation.Destination.DestinationList.Values
			.Where(d => d.MapAreaId == map.AreaId)
			.Where(d => d.MapInfoId == map.InfoId)
			.GroupBy(d => d.CellDisplay)
			.Select(t => new Node(t.Key, [.. t.Select(d => d.CellId)]))
			.ToList();
}
