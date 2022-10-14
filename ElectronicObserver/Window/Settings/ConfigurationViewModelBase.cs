using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Settings;

public abstract class ConfigurationViewModelBase : ObservableValidator
{
	public abstract void Save();
}
