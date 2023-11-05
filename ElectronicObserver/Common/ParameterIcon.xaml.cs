using System.Windows;
using ElectronicObserverTypes;

namespace ElectronicObserver.Common;

/// <summary>
/// Interaction logic for ParameterIcon.xaml
/// </summary>
public partial class ParameterIcon
{
	public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
		nameof(Type), typeof(ParameterType), typeof(ParameterIcon), new PropertyMetadata(default(ParameterType)));

	public ParameterType Type
	{
		get => (ParameterType)GetValue(ParameterProperty);
		set => SetValue(ParameterProperty, value);
	}

	public ParameterIcon()
	{
		InitializeComponent();
	}
}
