using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class NodeOptionViewModel(string letter, List<int> ids, ConditionModel model) : ObservableObject
{
	private ConditionModel Model { get; } = model;

	public string Letter { get; } = letter;
	private List<int> Ids { get; } = ids;

	public bool IsChecked
	{
		get => Ids.Count > 0 && Ids.All(Model.CellIds.Contains);
		set
		{
			if (value == IsChecked)
			{
				return;
			}

			if (value)
			{
				foreach (int id in Ids)
				{
					if (!Model.CellIds.Contains(id))
					{
						Model.CellIds.Add(id);
					}
				}
			}
			else
			{
				Model.CellIds.RemoveAll(Ids.Contains);
			}

			OnPropertyChanged();
		}
	}
}
