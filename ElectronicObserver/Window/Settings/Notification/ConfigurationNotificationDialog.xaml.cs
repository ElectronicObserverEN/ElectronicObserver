using System.ComponentModel;

namespace ElectronicObserver.Window.Settings.Notification;

/// <summary>
/// Interaction logic for ConfigurationNotificationDialog.xaml
/// </summary>
public partial class ConfigurationNotificationDialog
{
	public ConfigurationNotificationDialog(Base.ConfigurationNotificationBaseViewModel config) :
		base(new(config))
	{
		InitializeComponent();

		ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

		Closing += ConfigurationClosing;
	}

	private void ConfigurationClosing(object? sender, CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
	}

	private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.DialogResult)) return;

		Close();
	}
}
