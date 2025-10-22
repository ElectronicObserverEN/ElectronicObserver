using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;
/// <summary>
/// Interaction logic for ShipParameter.xaml
/// </summary>
public partial class ShipParameter : UserControl
{
	public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
		nameof(ParameterType), typeof(ParameterType), typeof(ShipParameter), new PropertyMetadata(default(ParameterType)));

	public ParameterType ParameterType
	{
		get => (ParameterType)GetValue(ParameterProperty);
		set => SetValue(ParameterProperty, value);
	}

	public static readonly DependencyProperty ParameterNameProperty = DependencyProperty.Register(
		nameof(ParameterName), typeof(string), typeof(ShipParameter), new PropertyMetadata(default(string)));

	public string? ParameterName
	{
		get => (string?)GetValue(ParameterNameProperty);
		set => SetValue(ParameterNameProperty, value);
	}

	public static readonly DependencyProperty ParameterValueProperty = DependencyProperty.Register(
		nameof(ParameterValue), typeof(string), typeof(ShipParameter), new PropertyMetadata(default(string)));

	public string? ParameterValue
	{
		get => (string?)GetValue(ParameterValueProperty);
		set => SetValue(ParameterValueProperty, value);
	}

	public ShipParameter()
	{
		InitializeComponent();
	}
}
