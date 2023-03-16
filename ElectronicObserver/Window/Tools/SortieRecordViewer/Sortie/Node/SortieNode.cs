using ElectronicObserver.Data;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public abstract class SortieNode
{
	protected IKCDatabase KcDatabase { get; }

	public int World { get; }
	public int Map { get; }
	public int Cell { get; }

	public string DisplayCell => KCDatabase.Instance.Translation.Destination.DisplayID(World, Map, Cell);

	protected SortieNode(IKCDatabase kcDatabase, int world, int map, int cell)
	{
		KcDatabase = kcDatabase;

		World = world;
		Map = map;
		Cell = cell;
	}
}
