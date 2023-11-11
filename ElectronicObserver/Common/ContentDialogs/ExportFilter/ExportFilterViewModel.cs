using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;

namespace ElectronicObserver.Common.ContentDialogs.ExportFilter;

public class ExportFilterViewModel : ObservableObject
{
	private static string All { get; } = "*";
	public List<DestinationItemViewModel> Destinations { get; }

	public ExportFilterViewModel(object world, object map)
	{
		Destinations = KCDatabase.Instance.Translation.Destination.DestinationList.Values
			.Where(d => world as string == All || world as int? == d.MapAreaId)
			.Where(d => map as string == All || map as int? == d.MapInfoId)
			.GroupBy(d => d.Display)
			.Select(g => new DestinationItemViewModel
			{
				Display = g.Key,
				CellIds = g.Select(c => c.CellId).ToList(),
			})
			.ToList();
	}

	public bool MatchesFilter(SortieNode node)
	{
		return Destinations
			.Where(d => d.IsChecked)
			.SelectMany(d => d.CellIds)
			.Contains(node.Cell);
	}
}
