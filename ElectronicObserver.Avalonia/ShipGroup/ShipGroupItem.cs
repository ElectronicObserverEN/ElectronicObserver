using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

namespace ElectronicObserver.Avalonia.ShipGroup;

public partial class ShipGroupItem : ObservableObject
{
	public IGroupItem Group { get; }

	[ObservableProperty] public partial int Id { get; set; }
	[ObservableProperty] public partial string Name { get; set; }
	[ObservableProperty] public partial bool IsSelected { get; set; }
	[ObservableProperty] public partial int FrozenColumns { get; set; }
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
		FrozenColumns = group.ScrollLockColumnCount;

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Name)) return;

			Group.Name = Name;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(FrozenColumns)) return;

			Group.ScrollLockColumnCount = FrozenColumns;
		};
	}
}
