using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupItem : ObservableObject
{
	public IGroupItem Group { get; }

	[ObservableProperty] private int _id;
	[ObservableProperty] private string _name;
	[ObservableProperty] private bool _isSelected;
	public required ObservableCollection<ColumnModel> Columns { get; set; }
	public DataGridSortDescriptionCollection SortDescriptions { get; set; } = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public ShipGroupItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
		// this should only be used for debug purposes
	}

	public ShipGroupItem(IGroupItem group)
	{
		Group = group;
		Id = group.GroupID;
		Name = group.Name;

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Name)) return;

			Group.Name = Name;
		};
	}
}
