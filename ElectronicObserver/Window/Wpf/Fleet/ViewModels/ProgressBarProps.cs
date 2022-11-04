using System;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class ProgressBarProps : ObservableObject
{
	private System.Drawing.Color[]? Colors { get; set; }

	public bool ColorMorphing { get; set; }
	public int Value { get; set; }
	public int MaximumValue { get; set; }

	public Visibility Visibility { get; set; } = Visibility.Visible;

	public SolidColorBrush Foreground { get; set; }
	public SolidColorBrush Background { get; set; }

	private static double GetPercentage(int value, int max)
	{
		if (max <= 0 || value <= 0)
			return 0.0;
		else if (value > max)
			return 1.0;
		else
			return (double)value / max;
	}

	public System.Drawing.Color GetColor(int value, int maximumValue, bool colorMorphing)
	{
		if (Colors is null) return System.Drawing.Color.Transparent;

		double p = GetPercentage(value, maximumValue);

		System.Drawing.Color barColor;
		if (!colorMorphing)
		{
			barColor = p switch
			{
				<= 0.25 => Colors[0],
				<= 0.50 => Colors[2],
				<= 0.75 => Colors[4],
				< 1.00 => Colors[6],
				_ => Colors[8]
			};
		}
		else
		{
			barColor = p switch
			{
				<= 0.25 => BlendColor(Colors[0], Colors[1], p * 4.0),
				<= 0.50 => BlendColor(Colors[2], Colors[3], (p - 0.25) * 4.0),
				<= 0.75 => BlendColor(Colors[4], Colors[5], (p - 0.50) * 4.0),
				< 1.00 => BlendColor(Colors[6], Colors[7], (p - 0.75) * 4.0),
				_ => Colors[8]
			};
		}

		return barColor;
	}

	private static System.Drawing.Color BlendColor(System.Drawing.Color a, System.Drawing.Color b, double weight)
	{
		if (weight < 0.0 || 1.0 < weight)
			throw new ArgumentOutOfRangeException("weight は 0.0 - 1.0 の範囲内でなければなりません。指定値: " + weight);

		return System.Drawing.Color.FromArgb(
			(int)(a.A * (1 - weight) + b.A * weight),
			(int)(a.R * (1 - weight) + b.R * weight),
			(int)(a.G * (1 - weight) + b.G * weight),
			(int)(a.B * (1 - weight) + b.B * weight));
	}

	public void SetBarColorScheme(System.Drawing.Color[] colors)
	{
		if (colors.Length < 12)
			throw new ArgumentOutOfRangeException("colors の配列長が足りません。");

		Colors = colors;
	}
}
