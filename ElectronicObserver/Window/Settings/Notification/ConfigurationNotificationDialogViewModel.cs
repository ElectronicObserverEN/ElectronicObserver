using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification;

public partial class ConfigurationNotificationDialogViewModel : WindowViewModelBase
{
	public ConfigurationNotificationBaseTranslationViewModel Translation { get; }

	public ConfigurationNotificationBaseViewModel Config { get; }

	public bool? DialogResult { get; set; }

	public ConfigurationNotificationDialogViewModel(ConfigurationNotificationBaseViewModel config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationNotificationBaseTranslationViewModel>();

		Config = config;
		Config.Load();
	}

	[RelayCommand]
	private void Confirm()
	{
		if (!Config.TrySaveConfiguration()) return;

		DialogResult = true;
	}

	[RelayCommand]
	private void Cancel()
	{
		DialogResult = false;
	}

	/// <inheritdoc />
	public override void Closed()
	{
		base.Closed();

		Config.StopSound();
	}
}
