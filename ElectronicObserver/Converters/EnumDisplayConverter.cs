using System;
using System.Globalization;
using System.Windows.Data;
using BrowserLibCore;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;

namespace ElectronicObserver.Converters;

public class EnumDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			EquipmentTypes e => KCDatabase.Instance.EquipmentTypes[(int)e].NameEN,
			FormationType f => Constants.GetFormation(f),
			EquipmentIconType eqIcon => eqIcon.TranslatedName(),
			EquipmentCardType eqCard => eqCard.TranslatedName(),

			TpGauge.None => BattleResources.None,
			TpGauge tankGauge => tankGauge.GetGaugeName(KCDatabase.Instance),
			
			ScreenshotMode.Automatic => Core.Properties.ScreenshotModeResources.Automatic,
			ScreenshotMode.Canvas => Core.Properties.ScreenshotModeResources.Canvas,
			ScreenshotMode.Browser => Core.Properties.ScreenshotModeResources.Browser,

			Enum e => e.Display(),
			_ => "???",
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
