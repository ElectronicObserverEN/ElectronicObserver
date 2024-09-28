using System.Globalization;
using Avalonia.Data.Converters;
using ElectronicObserverTypes;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public class UseItemIdConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return value switch
		{
			UseItemId.Fuel => ExpeditionCalculatorResources.Fuel,
			UseItemId.Ammo => ExpeditionCalculatorResources.Ammo,
			UseItemId.Steel => ExpeditionCalculatorResources.Steel,
			UseItemId.Bauxite => ExpeditionCalculatorResources.Bauxite,
			UseItemId.InstantRepair => ExpeditionCalculatorResources.InstantRepair,
			UseItemId.InstantConstruction => ExpeditionCalculatorResources.InstantConstruction,
			UseItemId.DevelopmentMaterial => ExpeditionCalculatorResources.DevelopmentMaterial,
			UseItemId.ImproveMaterial => ExpeditionCalculatorResources.ImproveMaterial,
			UseItemId.FurnitureBoxSmall => ExpeditionCalculatorResources.FurnitureBoxSmall,
			UseItemId.FurnitureBoxMedium => ExpeditionCalculatorResources.FurnitureBoxMedium,
			UseItemId.FurnitureBoxLarge => ExpeditionCalculatorResources.FurnitureBoxLarge,
			UseItemId.MoraleFoodIrako => ExpeditionCalculatorResources.MoraleFoodIrako,
			_ => "???",
		};
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
