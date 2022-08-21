using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Behaviors;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class SlotSizeHighlightBehavior : NumberHighlightBehavior
{
	public static readonly DependencyProperty NonAircraftBrushProperty = DependencyProperty.Register(
		nameof(NonAircraftBrush), typeof(Brush), typeof(SlotSizeHighlightBehavior), new PropertyMetadata(default(Brush)));

	public Brush? NonAircraftBrush
	{
		get => (Brush?)GetValue(NonAircraftBrushProperty);
		set => SetValue(NonAircraftBrushProperty, value);
	}

	public static readonly DependencyProperty IsAircraftProperty = DependencyProperty.Register(
		nameof(IsAircraft), typeof(bool), typeof(SlotSizeHighlightBehavior), new PropertyMetadata(default(bool)));

	public bool IsAircraft
	{
		get => (bool)GetValue(IsAircraftProperty);
		set => SetValue(IsAircraftProperty, value);
	}

	protected override Brush? GetBrush(string text) => double.TryParse(text, out double value) switch
	{
		true => value switch
		{
			> 0 => IsAircraft switch
			{
				true => PositiveNumberBrush,
				_ => NonAircraftBrush,
			},
			< 0 => NegativeNumberBrush,
			0 => ZeroBrush,
			_ => null,
		},
		_ => null,
	};
}
