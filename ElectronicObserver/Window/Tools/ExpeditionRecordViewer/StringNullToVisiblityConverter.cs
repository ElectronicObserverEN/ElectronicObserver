using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;
public class StringNullVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return string.IsNullOrEmpty(value as string)
		? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
