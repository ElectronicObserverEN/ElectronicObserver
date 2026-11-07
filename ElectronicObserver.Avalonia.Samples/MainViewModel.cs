using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.ExpeditionCalculator;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.Samples.AntiAirCutInUpdater;
using ElectronicObserver.Avalonia.Samples.ShipGroup;

namespace ElectronicObserver.Avalonia.Samples;

public class MainViewModel(QuestTrackerManager2ViewModel questTrackerManager2) : ObservableObject
{
	public ShipGroupSampleViewModel ShipGroup { get; } = new();
	public ExpeditionCalculatorViewModel ExpeditionCalculator { get; } = new();
	public AntiAirCutInUpdaterViewModel AntiAirCutInUpdater { get; } = new();
	public QuestTrackerManager2ViewModel QuestTrackerManager2 { get; } = questTrackerManager2;
}
