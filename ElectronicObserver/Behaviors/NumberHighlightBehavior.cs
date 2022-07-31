using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace ElectronicObserver.Behaviors;

public class NumberHighlightBehavior : Behavior<TextBlock>
{
	public static readonly DependencyProperty PositiveNumberBrushProperty = DependencyProperty.Register(
		nameof(PositiveNumberBrush), typeof(Brush), typeof(NumberHighlightBehavior), new PropertyMetadata(default(Brush)));

	public Brush? PositiveNumberBrush
	{
		get => (Brush?)GetValue(PositiveNumberBrushProperty);
		set => SetValue(PositiveNumberBrushProperty, value);
	}

	public static readonly DependencyProperty NegativeNumberBrushProperty = DependencyProperty.Register(
		nameof(NegativeNumberBrush), typeof(Brush), typeof(NumberHighlightBehavior), new PropertyMetadata(default(Brush)));

	public Brush? NegativeNumberBrush
	{
		get => (Brush?)GetValue(NegativeNumberBrushProperty);
		set => SetValue(NegativeNumberBrushProperty, value);
	}

	public static readonly DependencyProperty ZeroBrushProperty = DependencyProperty.Register(
		nameof(ZeroBrush), typeof(Brush), typeof(NumberHighlightBehavior), new PropertyMetadata(default(Brush)));

	public Brush? ZeroBrush
	{
		get => (Brush?)GetValue(ZeroBrushProperty);
		set => SetValue(ZeroBrushProperty, value);
	}

	private static DependencyPropertyDescriptor TextPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));
	private static DependencyPropertyDescriptor ForegroundPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(TextBlock.ForegroundProperty, typeof(TextBlock));

	private bool ChangingForeground { get; set; }

	protected override void OnAttached()
	{
		base.OnAttached();

		TextPropertyDescriptor.AddValueChanged(AssociatedObject, TextChangedHandler);
		ForegroundPropertyDescriptor.AddValueChanged(AssociatedObject, ForegroundChangedHandler);

		UpdateForeground();
	}

	private void TextChangedHandler(object? sender, EventArgs x) => UpdateForeground();
	private void ForegroundChangedHandler(object? sender, EventArgs x)
	{
		if (ChangingForeground) return;

		UpdateForeground();
	}

	private void UpdateForeground()
	{
		ChangingForeground = true;

		Brush? brush = GetBrush(AssociatedObject.Text);

		if (brush is null)
		{
			ForegroundPropertyDescriptor.SetValue(AssociatedObject, DependencyProperty.UnsetValue);
		}
		else
		{
			AssociatedObject.Foreground = brush;
		}

		ChangingForeground = false;
	}

	protected virtual Brush? GetBrush(string text) => double.TryParse(text, out double value) switch
	{
		true => value switch
		{
			> 0 => PositiveNumberBrush,
			< 0 => NegativeNumberBrush,
			0 => ZeroBrush,
			_ => null,
		},
		_ => null,
	};
}
