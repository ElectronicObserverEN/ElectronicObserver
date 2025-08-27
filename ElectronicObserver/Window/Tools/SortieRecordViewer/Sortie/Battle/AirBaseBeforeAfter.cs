using System;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;

public record AirBaseBeforeAfter(int Index, IBaseAirCorpsData? Before, IBaseAirCorpsData? After)
{
	public override string ToString() => (Before, After) switch
	{
		({ }, { }) =>
			$"#{Index + 1}: " +
			$"{Before.Name} " +
			$"HP: ({Before.HPCurrent} → {Math.Max(0, After.HPCurrent)})/{Before.HPMax} " +
			$"({After.HPCurrent - Before.HPCurrent:+#;-#;0})",

		_ => "???",
	};
}
