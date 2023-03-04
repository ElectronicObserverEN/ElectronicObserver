using System;
using System.Windows.Media;

namespace ElectronicObserver.Services;

public class ColorService
{
	private static double LuminanceCoefficient(int v)
	{
		double c = v / 255.0;

		return c switch
		{
			<= 0.03928 => c / 12.92,
			_ => Math.Pow((c + 0.055) / 1.055, 2.4),
		};
	}

	private static double Luminance(int r, int g, int b) =>
		LuminanceCoefficient(r) * 0.2126 +
		LuminanceCoefficient(g) * 0.7152 +
		LuminanceCoefficient(b) * 0.0722;

	private static double Contrast(Color rgb1, Color rgb2)
	{
		double lum1 = Luminance(rgb1.R, rgb1.G, rgb1.B);
		double lum2 = Luminance(rgb2.R, rgb2.G, rgb2.B);

		double brighter = Math.Max(lum1, lum2);
		double darker = Math.Min(lum1, lum2);

		return (brighter + 0.05) / (darker + 0.05);
	}

	/// <summary>
	/// Returns black or white depending on which one should be more visible on the given background. <br />
	/// todo: add contrasting foregrounds to themes and send those values as arguments to this function
	/// </summary>
	/// <param name="backgroundColor"></param>
	/// <param name="foreground1">Default value: <see cref="Colors.Black"/></param>
	/// <param name="foreground2">Default value: <see cref="Colors.White"/></param>
	/// <returns></returns>
	public Color GetForegroundColor(Color backgroundColor, Color? foreground1 = null, Color? foreground2 = null)
	{
		Color f1 = foreground1 ?? Colors.Black;
		Color f2 = foreground2 ?? Colors.White;

		return (Contrast(backgroundColor, f1) > Contrast(backgroundColor, f2)) switch
		{
			true => f1,
			_ => f2,
		};
	}
}
