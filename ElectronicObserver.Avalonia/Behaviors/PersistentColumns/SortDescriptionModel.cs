using System.ComponentModel;

namespace ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

public class SortDescriptionModel
{
	public required string PropertyPath { get; init; }
	public required ListSortDirection Direction { get; init; }
}
