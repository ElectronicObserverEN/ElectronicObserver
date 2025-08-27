using Avalonia;
using Avalonia.Controls;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionWeight : UserControl
{
	public static readonly StyledProperty<UseItemId> UseItemIdProperty =
		AvaloniaProperty.Register<ExpeditionWeight, UseItemId>(nameof(UseItemId));

	public UseItemId UseItemId
	{
		get => GetValue(UseItemIdProperty);
		set => SetValue(UseItemIdProperty, value);
	}

	public static readonly StyledProperty<int> WeightProperty =
		AvaloniaProperty.Register<ExpeditionWeight, int>(nameof(Weight));

	public int Weight
	{
		get => GetValue(WeightProperty);
		set => SetValue(WeightProperty, value);
	}

	public ExpeditionWeight()
	{
		InitializeComponent();
	}
}
