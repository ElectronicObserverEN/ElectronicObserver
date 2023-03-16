using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class EmptyNode : SortieNode
{
	public EmptyNode(IKCDatabase kcDatabase, int world, int map, int cell) 
		: base(kcDatabase, world, map, cell)
	{
	}
}
