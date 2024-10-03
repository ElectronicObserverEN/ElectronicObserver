using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class FleetInfoViewModel : ObservableObject
{
	[ObservableProperty] private bool _allSparkled;
	[ObservableProperty] private int _sparkleCount;
	[ObservableProperty] private int _drumCount;
	[ObservableProperty] private int _flagshipLevel = 1;
}
