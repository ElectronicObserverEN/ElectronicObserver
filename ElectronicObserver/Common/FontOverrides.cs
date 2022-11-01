using System.Windows;
using System.Windows.Media;

namespace ElectronicObserver.Common;

// https://github.com/Kinnara/ModernWpf/blob/master/samples/SamplesCommon/FontOverrides.cs
public class FontOverrides : ResourceDictionary
{
	private static object[] FontFamilyKeys { get; } =
	{
		SystemFonts.MessageFontFamilyKey,
		"ContentControlThemeFontFamily",
		"PivotHeaderItemFontFamily",
		"PivotTitleFontFamily",
	};

	private static object[] FontSizeKeys { get; } =
	{
		// https://github.com/Kinnara/ModernWpf/blob/master/ModernWpf/ControlsResources.xaml#L52
		"ControlContentThemeFontSize",
	};

	private FontFamily? _fontFamily;

	public FontFamily? FontFamily
	{
		get => _fontFamily;
		set
		{
			if (_fontFamily == value) return;

			_fontFamily = value;

			if (_fontFamily != null)
			{
				foreach (object key in FontFamilyKeys)
				{
					this[key] = _fontFamily;
				}
			}
			else
			{
				foreach (object key in FontFamilyKeys)
				{
					Remove(key);
				}
			}
		}
	}

	private double _fontSize;

	public double FontSize
	{
		get => _fontSize;
		set
		{
			if (_fontSize == value) return;

			_fontSize = value;

			foreach (object key in FontSizeKeys)
			{
				this[key] = _fontSize;
			}
		}
	}
}
