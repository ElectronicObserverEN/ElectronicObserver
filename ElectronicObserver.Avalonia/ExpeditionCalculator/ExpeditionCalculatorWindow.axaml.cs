using Avalonia.Controls;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionCalculatorWindow : Window
{
	public ExpeditionCalculatorWindow(ExpeditionCalculatorViewModel viewModel)
	{
		InitializeComponent();

		DataContext = viewModel;
	}
}
