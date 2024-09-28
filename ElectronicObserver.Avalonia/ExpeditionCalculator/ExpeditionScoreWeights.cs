using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionScoreWeights : ObservableObject
{
	[ObservableProperty] private int _fuel = 1;
	[ObservableProperty] private int _ammo = 1;
	[ObservableProperty] private int _steel = 1;
	[ObservableProperty] private int _bauxite = 1;

	[ObservableProperty] private int _instantRepair = 100;
	[ObservableProperty] private int _instantConstruction;
	[ObservableProperty] private int _developmentMaterial;
	[ObservableProperty] private int _improveMaterial;
	[ObservableProperty] private int _furnitureBoxSmall;
	[ObservableProperty] private int _furnitureBoxMedium;
	[ObservableProperty] private int _furnitureBoxLarge;
	[ObservableProperty] private int _moraleFoodIrako;
}
