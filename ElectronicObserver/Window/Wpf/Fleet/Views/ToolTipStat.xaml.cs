using System.Windows;
using System.Windows.Media;

namespace ElectronicObserver.Window.Wpf.Fleet.Views;

/// <summary>
/// Interaction logic for ToolTipStat.xaml
/// </summary>
public partial class ToolTipStat
{
	public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
		nameof(Icon), typeof(ImageSource), typeof(ToolTipStat), new PropertyMetadata(default(ImageSource)));

	public ImageSource Icon
	{
		get => (ImageSource)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly DependencyProperty ParameterNameProperty = DependencyProperty.Register(
		nameof(ParameterName), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string?)));

	public string? ParameterName
	{
		get => (string?)GetValue(ParameterNameProperty);
		set => SetValue(ParameterNameProperty, value);
	}

	public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
		nameof(Base), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string)));

	public string? Base
	{
		get => (string?)GetValue(BaseProperty);
		set => SetValue(BaseProperty, value);
	}

	public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
		nameof(Total), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string)));

	public string? Total
	{
		get => (string?)GetValue(TotalProperty);
		set => SetValue(TotalProperty, value);
	}

	public static readonly DependencyProperty BonusProperty = DependencyProperty.Register(
		nameof(Bonus), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string)));
	
	public string? Bonus
	{
		get => (string?)GetValue(BonusProperty);
		set => SetValue(BonusProperty, value);
	}

	public ToolTipStat()
	{
		InitializeComponent();
	}
}
