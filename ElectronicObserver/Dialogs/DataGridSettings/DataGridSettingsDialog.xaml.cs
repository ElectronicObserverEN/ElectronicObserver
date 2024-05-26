using System.Windows.Threading;

namespace ElectronicObserver.Dialogs.DataGridSettings;

public partial class DataGridSettingsDialog
{
	public DataGridSettingsDialog()
	{
		InitializeComponent();

		// https://github.com/Kinnara/ModernWpf/issues/378
		SourceInitialized += (_, _) =>
		{
			Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
		};
	}
}
