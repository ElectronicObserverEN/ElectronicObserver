using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class FleetInfoViewModel : ObservableObject
{
	[ObservableProperty] public partial bool AllSparkled { get; set; }
	[ObservableProperty] public partial int SparkleCount { get; set; }
	[ObservableProperty] public partial int DrumCount { get; set; }
	[ObservableProperty] public partial int FlagshipLevel { get; set; } = 1;
}
