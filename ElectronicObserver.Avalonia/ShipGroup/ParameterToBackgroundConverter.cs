using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ParameterToBackgroundConverter : IMultiValueConverter
{
	public required IBrush ParameterBrush { get; set; }

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		int total = (int)values[0]!;

		int? remaining = values switch
		{
			{ Count: > 1 } => (int)values[1]!,
			_ => null,
		};

		IBrush brush = (remaining, total) switch
		{
			(_, 0) => ShipGroupColors.TransparentBrush,
			(> 0, _) => ShipGroupColors.TransparentBrush,
			_ => ParameterBrush,
		};

		return brush;
	}
}
