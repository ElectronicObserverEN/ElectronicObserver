using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace ElectronicObserver.Converters;

public class FontFamilyDisplayConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is null) return "";
		if (value is not FontFamily font) throw new NotImplementedException();

		if (font.FamilyNames.TryGetValue(XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.Name), out string name))
		{
			return name;
		}

		return font.FamilyNames.FirstOrDefault().Value ?? "Unknown font";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
