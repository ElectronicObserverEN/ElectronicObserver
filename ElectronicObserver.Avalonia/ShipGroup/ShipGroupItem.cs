using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupItem : ObservableObject
{
	public IGroupItem Group { get; }

	[ObservableProperty] private string _name;
	[ObservableProperty] private bool _isSelected;

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
