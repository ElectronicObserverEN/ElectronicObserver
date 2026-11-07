using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;

public sealed partial class PositionViewModel : ObservableObject
{
	[ObservableProperty] public partial bool IsChecked { get; set; }
	public int Position { get; init; }
}
