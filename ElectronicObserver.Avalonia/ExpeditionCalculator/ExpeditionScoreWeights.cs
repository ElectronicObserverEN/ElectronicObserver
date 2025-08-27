using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionScoreWeights : ObservableObject
{
	[ObservableProperty] public partial int Fuel { get; set; } = 1;
	[ObservableProperty] public partial int Ammo { get; set; } = 1;
	[ObservableProperty] public partial int Steel { get; set; } = 1;
	[ObservableProperty] public partial int Bauxite { get; set; } = 1;

	[ObservableProperty] public partial int InstantRepair { get; set; } = 100;
	[ObservableProperty] public partial int InstantConstruction { get; set; }
	[ObservableProperty] public partial int DevelopmentMaterial { get; set; }
	[ObservableProperty] public partial int ImproveMaterial { get; set; }
	[ObservableProperty] public partial int FurnitureBoxSmall { get; set; }
	[ObservableProperty] public partial int FurnitureBoxMedium { get; set; }
	[ObservableProperty] public partial int FurnitureBoxLarge { get; set; }
	[ObservableProperty] public partial int MoraleFoodIrako { get; set; }
}
