using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Settings.Notification.Base;

namespace ElectronicObserver.Window.Settings.Notification;

public partial class ConfigurationNotificationDialogViewModel : WindowViewModelBase
{
	public ConfigurationNotificationBaseTranslationViewModel Translation { get; }

	public Base.ConfigurationNotificationBaseViewModel Config { get; }

	public bool? DialogResult { get; set; }

	public ConfigurationNotificationDialogViewModel(Base.ConfigurationNotificationBaseViewModel config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationNotificationBaseTranslationViewModel>();

		Config = config;
		Config.Load();
	}

	[RelayCommand]
	private void Confirm()
	{
		if (!Config.TrySaveConfiguration()) return;

		Config.Dispose();

		DialogResult = true;
	}

	[RelayCommand]
	private void Cancel()
	{
		Config.Dispose();

		DialogResult = false;
	}
}
