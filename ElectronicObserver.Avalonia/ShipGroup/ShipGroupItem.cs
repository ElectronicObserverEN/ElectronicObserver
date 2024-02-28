using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ShipGroupItem : ObservableObject
{
	public IGroupItem Group { get; }

	public string Name { get; set; }
	public bool IsSelected { get; set; }

	public ShipGroupItem(IGroupItem group)
	{
		Group = group;
		Name = group.Name;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Name)) return;

			Group.Name = Name;
		};
	}
}
