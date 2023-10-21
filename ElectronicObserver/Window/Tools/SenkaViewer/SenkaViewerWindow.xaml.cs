using System.Linq;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

/// <summary>
/// Interaction logic for SenkaViewerWindow.xaml
/// </summary>
public partial class SenkaViewerWindow
{
	public SenkaViewerWindow() : base(new SenkaViewerViewModel())
	{
		InitializeComponent();
	}

	private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not DataGrid dataGrid) return;

		ViewModel.SelectedSenkaRecords = dataGrid.SelectedItems.Cast<SenkaRecord>().ToList();
	}
}
