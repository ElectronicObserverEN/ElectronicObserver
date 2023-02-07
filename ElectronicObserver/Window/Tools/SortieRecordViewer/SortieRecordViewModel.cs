using System;
using System.Linq;
using ElectronicObserver.Database.Sortie;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordViewModel
{
	public SortieRecord Model { get; }

	public int Id => Model.Id;
	public int World => Model.World;
	public int Map => Model.Map;
	public DateTime SortieStart { get; }
	public IFleetData? Fleet { get; }

	public SortieRecordViewModel(SortieRecord sortie, DateTime sortieStart)
	{
		Model = sortie;
		SortieStart = sortieStart.ToLocalTime();
		Fleet = sortie.FleetData.Fleets[sortie.FleetData.FleetId - 1].MakeFleet();
	}
}
