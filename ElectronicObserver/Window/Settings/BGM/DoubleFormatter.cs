using ModernWpf.Controls;

namespace ElectronicObserver.Window.Settings.BGM;

public class DoubleFormatter : INumberBoxNumberFormatter
{
	public string FormatDouble(double value)
	{
		return value.ToString("0.0000");
	}

	public double? ParseDouble(string text)
	{
		if (double.TryParse(text, out double result))
		{
			return result;
		}

		return null;
	}
}
