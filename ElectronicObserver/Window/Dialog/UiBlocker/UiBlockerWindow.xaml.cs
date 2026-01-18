namespace ElectronicObserver.Window.Dialog.UiBlocker;

public partial class UiBlockerWindow
{
	public UiBlockerWindow(UiBlockerManagerViewModel viewModel) : base(viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}
