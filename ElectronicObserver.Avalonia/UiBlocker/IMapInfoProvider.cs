using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

namespace ElectronicObserver.Avalonia.UiBlocker;

public interface IMapInfoProvider
{
	List<MapInfoModel> GetMaps();
	List<Node> GetNodes(MapInfoModel map);
}
