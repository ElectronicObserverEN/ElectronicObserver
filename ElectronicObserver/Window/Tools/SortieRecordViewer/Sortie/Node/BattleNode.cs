using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

public class BattleNode : SortieNode
{
	public BattleData FirstBattle { get; }
	public BattleData? SecondBattle { get; }

	public BattleNode(int world, int map, int cell, BattleNightOnly battle) : base(world, map, cell)
	{
		FirstBattle = battle;
	}
}
