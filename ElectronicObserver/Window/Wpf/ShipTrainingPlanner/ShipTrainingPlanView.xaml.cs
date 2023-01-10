using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
/// <summary>
/// Interaction logic for ShipTrainingPlanView.xaml
/// </summary>
public partial class ShipTrainingPlanView : WindowBase<ShipTrainingPlanViewModel>
{
	public ShipTrainingPlanView(ShipTrainingPlanViewModel vm) : base(vm)
	{
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
