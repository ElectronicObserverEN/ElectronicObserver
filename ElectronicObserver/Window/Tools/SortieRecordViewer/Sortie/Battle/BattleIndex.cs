using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public record BattleIndex(int Index, FleetFlag FleetFlag)
{
	public string Display => $"#{Index + 1}";
	public bool IsFriend => FleetFlag is FleetFlag.Player;

	public int ToFlatIndex() => FleetFlag switch
	{
		FleetFlag.Player => Index,
		_ => Index + 12,
	};
}
