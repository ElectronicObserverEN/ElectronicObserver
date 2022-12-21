using System.Windows;
using ElectronicObserverTypes;

namespace ElectronicObserver.Common;

public partial class UseItemIcon
{
	public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
		nameof(Type), typeof(UseItemId), typeof(UseItemIcon), new PropertyMetadata(default(UseItemId)));

	public UseItemId Type
	{
		get => (UseItemId)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	public UseItemIcon()
	{
		InitializeComponent();
	}
}
