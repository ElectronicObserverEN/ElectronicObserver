using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public record BattleIndex(int Index, FleetFlag FleetFlag)
{
	public string Display => $"#{Index + 1}";
}
