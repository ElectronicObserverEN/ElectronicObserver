using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public class ExpeditionSummary
{
	public int Fuel { get; }
	public int Ammo { get; }
	public int Steel { get; }
	public int Bauxite { get; }
	public Dictionary<UseItemId, ExpeditionItem> ExpeditionItems { get; } = new();

	public ExpeditionSummary(ObservableCollection<ExpeditionRecordViewModel> expeditions)
	{
		foreach (ExpeditionRecordViewModel expedition in expeditions)
		{
			Fuel += expedition.Fuel;
			Ammo += expedition.Ammo;
			Steel += expedition.Steel;
			Bauxite += expedition.Bauxite;

			if (expedition.ItemOneId is not UseItemId.Unknown && expedition.ItemOneCount is int itemOneCount)
			{
				AddOrUpdateItem(expedition.ItemOneId, itemOneCount);
			}

			if (expedition.ItemTwoId is not UseItemId.Unknown && expedition.ItemTwoCount is int itemTwoCount)
			{
				AddOrUpdateItem(expedition.ItemTwoId, itemTwoCount);
			}
		}
	}

	private void AddOrUpdateItem(UseItemId id, int count)
	{
		if (ExpeditionItems.TryGetValue(id, out ExpeditionItem? value))
		{
			value.Count += count;
		}
		else
		{
			ExpeditionItems.Add(id, new ExpeditionItem(id, count));
		}
	}
}
