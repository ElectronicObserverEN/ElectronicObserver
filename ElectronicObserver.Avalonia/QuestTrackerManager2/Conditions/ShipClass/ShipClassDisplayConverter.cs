using System.Globalization;
using Avalonia.Data.Converters;
using ElectronicObserver.Avalonia.Translation;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;

public class ShipClassDisplayConverter : IValueConverter
{
	public DataService? DataService { get; set; }
	
	public string Convert(Core.Types.ShipClass shipClass) => shipClass switch
	{
		Core.Types.ShipClass.Unknown => "*",
		_ => DataService?.Ship.GetShipClass(shipClass) ?? "???",
	};

	/// <inheritdoc />
	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		=> value switch
		{
			Core.Types.ShipClass shipClass => Convert(shipClass),
			_ => "???",
		};

	/// <inheritdoc />
	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
