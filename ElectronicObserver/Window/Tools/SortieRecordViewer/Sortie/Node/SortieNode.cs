using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public abstract class SortieNode
{
	protected IKCDatabase KcDatabase { get; }

	public int World { get; }
	public int Map { get; }
	public int Cell { get; }
	public BattleBaseAirRaid? AirBaseRaid { get; private set; }

	public string DisplayCell => KCDatabase.Instance.Translation.Destination.DisplayID(World, Map, Cell);

	protected SortieNode(IKCDatabase kcDatabase, int world, int map, int cell)
	{
		KcDatabase = kcDatabase;

		World = world;
		Map = map;
		Cell = cell;
	}

	public void AddAirBaseRaid(BattleBaseAirRaid abRaid)
	{
		AirBaseRaid = abRaid;
	}
}
