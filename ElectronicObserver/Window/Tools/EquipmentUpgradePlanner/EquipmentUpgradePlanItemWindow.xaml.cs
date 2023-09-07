using System.Windows.Threading;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
/// <summary>
/// Interaction logic for EquipmentUpgradePlanItemWindow.xaml
/// </summary>
public partial class EquipmentUpgradePlanItemWindow
{
	public EquipmentUpgradePlanItemWindow(EquipmentUpgradePlanItemViewModel vm) : base(vm)
	{
		// https://github.com/Kinnara/ModernWpf/issues/378
		SourceInitialized += (s, a) =>
		{
			Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
		};

		InitializeComponent();
	}

	private void OnConfirmClick(object sender, System.Windows.RoutedEventArgs e)
	{
		DialogResult = true;
	}

	private void OnCancelClick(object sender, System.Windows.RoutedEventArgs e)
	{
		DialogResult = false;
	}
}
