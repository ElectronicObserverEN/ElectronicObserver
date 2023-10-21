namespace ElectronicObserver.Window.Tools.Telegram;
/// <summary>
/// Interaction logic for TelegramWindow.xaml
/// </summary>
public partial class TelegramWindow
{
	public TelegramWindow()
	{
		InitializeComponent();

		DataContext = new TelegramViewModel();
	}
}
