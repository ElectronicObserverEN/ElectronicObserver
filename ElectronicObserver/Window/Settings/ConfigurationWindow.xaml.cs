// the namespace is "Settings" because "Configuration" conflicts with the static class of that name

using System.ComponentModel;

namespace ElectronicObserver.Window.Settings;
/// <summary>
/// Interaction logic for ConfigurationWindow.xaml
/// </summary>
public partial class ConfigurationWindow
{
	public ConfigurationWindow(ConfigurationViewModel viewModel) : base(viewModel)
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
