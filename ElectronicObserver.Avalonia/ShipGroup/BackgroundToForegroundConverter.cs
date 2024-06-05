using System.Globalization;
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

		// this is fine for now, we use the same reference for transparent on custom backgrounds
		// ReSharper disable once PossibleUnintendedReferenceComparison
		if (backgroundBrush == ShipGroupColors.TransparentBrush && themeBackgroundColor == ShipGroupColors.Black)
		{
			return ShipGroupColors.WhiteBrush;
		}

		return ShipGroupColors.BlackBrush;
	}
}
