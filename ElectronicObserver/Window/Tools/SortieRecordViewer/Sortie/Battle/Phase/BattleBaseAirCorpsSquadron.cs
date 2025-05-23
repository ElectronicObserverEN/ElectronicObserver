using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class BattleBaseAirCorpsSquadron
{
	public IEquipmentDataMaster? Equipment { get; init; }
	public int AircraftCount { get; init; }

	public override string ToString() => $"{Equipment?.NameEN ?? "???"} x {AircraftCount}";
}