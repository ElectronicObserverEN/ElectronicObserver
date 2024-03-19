using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ScottPlot.Drawing.Colormaps;

namespace ElectronicObserver.Common.ContentDialogs.ExportFilter;

public partial class ExportFilterViewModel : ObservableObject
{
	private object World { get; }
	private object Map { get; }
	private static string All { get; } = "*";
	public List<DestinationItemViewModel> Destinations { get; private set; }

	public ExportFilterViewModel(object world, object map)
	{
		World = world;
		Map = map;

		Destinations = MakeDestinations(true);
	}

	public bool MatchesFilter(SortieNode node)
	{
		return Destinations
			.Where(d => d.IsChecked)
			.SelectMany(d => d.CellIds)
			.Contains(node.Cell);
	}

	[RelayCommand]
	private void ToggleAllDestinations()
	{
		bool newState = Destinations.Any(d => !d.IsChecked);

		Destinations = MakeDestinations(newState);
	}

	private List<DestinationItemViewModel> MakeDestinations(bool isChecked) => 
		KCDatabase.Instance.Translation.Destination.DestinationList.Values
			.Where(d => World as string == All || World as int? == d.MapAreaId)
			.Where(d => Map as string == All || Map as int? == d.MapInfoId)
			.GroupBy(d => d.Display)
			.Select(g => new DestinationItemViewModel
			{
				IsChecked = isChecked,
				Display = g.Key,
				CellIds = g.Select(c => c.CellId).ToList(),
			})
			.ToList();
}
