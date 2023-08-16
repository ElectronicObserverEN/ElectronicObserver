using ElectronicObserver.Common.ContentDialogs;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

/// <summary>
/// Interaction logic for SortieRecordViewerWindow.xaml
/// </summary>
public partial class SortieRecordViewerWindow
{
	public SortieRecordViewerWindow() : base(new())
	{
		InitializeComponent();

		ViewModel.ContentDialogService = new ContentDialogService
		{
			ExportProgressContentDialog = ExportProgressContentDialog,
			NotificationContentDialog = NotificationContentDialog,
		};
	}
}
