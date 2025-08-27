using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.ExpeditionCalculator;
using ElectronicObserver.Avalonia.Samples.AntiAirCutInUpdater;
using ElectronicObserver.Avalonia.Samples.ShipGroup;

namespace ElectronicObserver.Avalonia.Samples;

public class MainViewModel : ObservableObject
{
	public ShipGroupSampleViewModel ShipGroup { get; } = new();
	public ExpeditionCalculatorViewModel ExpeditionCalculator { get; } = new();
	public AntiAirCutInUpdaterViewModel AntiAirCutInUpdater { get; } = new();
}
