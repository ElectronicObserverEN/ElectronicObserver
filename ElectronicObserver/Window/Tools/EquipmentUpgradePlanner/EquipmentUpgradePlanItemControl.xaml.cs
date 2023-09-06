using System.Windows;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
/// <summary>
/// Interaction logic for EquipmentUpgradePlanItemControl.xaml
/// </summary>
public partial class EquipmentUpgradePlanItemControl
{
	public static readonly DependencyProperty CompactModeProperty = DependencyProperty.Register(
		nameof(CompactMode), typeof(bool), typeof(EquipmentUpgradePlanItemControl), new PropertyMetadata(false));

	public bool CompactMode
	{
		get => (bool)GetValue(CompactModeProperty);
		set => SetValue(CompactModeProperty, value);
	}

	public static readonly DependencyProperty ReadOnlyroperty = DependencyProperty.Register(
		nameof(ReadOnly), typeof(bool), typeof(EquipmentUpgradePlanItemControl), new PropertyMetadata(false));

	public bool ReadOnly
	{
		get => (bool)GetValue(ReadOnlyroperty);
		set => SetValue(ReadOnlyroperty, value);
	}

	public EquipmentUpgradePlanItemControl()
	{
		InitializeComponent();
	}
}
