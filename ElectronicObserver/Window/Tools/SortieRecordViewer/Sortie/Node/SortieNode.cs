using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public abstract class SortieNode
{
	public int World { get; }
	public int Map { get; }
	public int Cell { get; }

	public string DisplayCell => KCDatabase.Instance.Translation.Destination.DisplayID(World, Map, Cell);

	protected SortieNode(int world, int map, int cell)
	{
		World = world;
		Map = map;
		Cell = cell;
	}
}
