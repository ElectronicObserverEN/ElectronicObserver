using System.Windows;
using System.Windows.Controls;
using ElectronicObserverTypes;

namespace ElectronicObserver.Common;
/// <summary>
/// Interaction logic for ParameterIcon.xaml
/// </summary>
public partial class ParameterIcon : UserControl
{
	public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
		nameof(ParameterType), typeof(ParameterType), typeof(ParameterIcon), new PropertyMetadata(default(ParameterType)));

	public ParameterType ParameterType
	{
		get => (ParameterType)GetValue(ParameterProperty);
		set => SetValue(ParameterProperty, value);
	}

	public ParameterIcon()
	{
		InitializeComponent();
	}
}
