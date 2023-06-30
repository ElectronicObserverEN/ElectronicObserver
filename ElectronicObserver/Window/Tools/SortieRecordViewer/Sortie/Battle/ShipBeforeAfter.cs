using System;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public record ShipBeforeAfter(int Index, IShipData? Before, IShipData? After)
{
	public override string ToString() => (Before, After) switch
	{
		({ }, { }) => 
			$"#{Index + 1}: " +
			$"{Before.Name} " +
			$"HP: ({Math.Max(0, Before.HPCurrent)} → {Math.Max(0, After.HPCurrent)})/{Before.HPMax} " +
			$"({After.HPCurrent - Before.HPCurrent:+#;-#;0})",

		_ => "???",
	};
}
