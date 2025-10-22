using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropFilterToDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			UseItemMaster i => i.NameTranslated,
			IShipDataMaster ship => ship.NameEN,
			DropRecordOption o => o.Display(),
			_ => "???"
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
