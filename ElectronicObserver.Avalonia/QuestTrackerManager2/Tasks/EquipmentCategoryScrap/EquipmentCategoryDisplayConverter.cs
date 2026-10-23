using System.Globalization;
using Avalonia.Data.Converters;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;

public class EquipmentCategoryDisplayConverter : IValueConverter
{
	public IKCDatabase? KcDatabase { get; set; }

	public string Convert(EquipmentTypes category) => KcDatabase switch
	{
		{ } kcDatabase => Converters.ToDisplay(category, kcDatabase),
		_ => "???",
	};

	/// <inheritdoc />
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value switch
		{
			EquipmentTypes category => Convert(category),
			_ => "???",
		};

	/// <inheritdoc />
	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
