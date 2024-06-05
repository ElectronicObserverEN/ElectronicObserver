using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class BackgroundToForegroundConverter : IMultiValueConverter
{
	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		IBrush? backgroundBrush = values[0] switch
		{
			IBrush b => b,
			_ => null,
		};

		Color? themeBackgroundColor = values[1] switch
		{
			Color c => c,
			_ => null,
		};

		if (backgroundBrush is null && themeBackgroundColor == ShipGroupColors.Black)
		{
			return ShipGroupColors.WhiteBrush;
		}

		return ShipGroupColors.BlackBrush;
	}
}
