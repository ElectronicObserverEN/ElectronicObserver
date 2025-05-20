using System;
using System.Linq;
using ElectronicObserver.Avalonia.Services;
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
	public SortieRecordFleetViewModel? Fleet { get; }

	public SortieRecordViewModel(SortieRecord sortie, DateTime sortieStart, ImageLoadService imageLoadService)
	{
		Model = sortie;
		SortieStart = sortieStart.ToLocalTime();

		FleetType combinedFlag = sortie.FleetData.FleetId switch
		{
			1 => sortie.FleetData.CombinedFlag,
			_ => FleetType.Single,
		};

		IFleetData? fleet = (sortie.FleetData.Fleets.Count >= sortie.FleetData.FleetId) switch
		{
			true => sortie.FleetData.Fleets[sortie.FleetData.FleetId - 1].MakeFleet(combinedFlag),
			// in an earlier version, fleets that weren't supposed to be saved were skipped
			// instead of being saved as null
			_ => sortie.FleetData.Fleets.First().MakeFleet(sortie.FleetData.CombinedFlag),
		};

		if (fleet is null) return;

		Fleet = new(fleet, imageLoadService);
	}
}
