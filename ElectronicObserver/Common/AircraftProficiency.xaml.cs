using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Common;
/// <summary>
/// Interaction logic for AircraftProficiency.xaml
/// </summary>
public partial class AircraftProficiency : UserControl
{
	public static readonly DependencyProperty AircraftLevelProperty = DependencyProperty.Register(
		nameof(AircraftLevel), typeof(int), typeof(AircraftProficiency), new PropertyMetadata(default(int)));

	public int AircraftLevel
	{
		get => (int)GetValue(AircraftLevelProperty);
		set => SetValue(AircraftLevelProperty, value);
	}

	public AircraftProficiency()
	{
		InitializeComponent();
	}
}
