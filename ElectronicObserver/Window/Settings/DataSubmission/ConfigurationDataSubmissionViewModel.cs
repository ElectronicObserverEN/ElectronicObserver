using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.DataSubmission;

public partial class ConfigurationDataSubmissionViewModel : ConfigurationViewModelBase
{
	public ConfigurationDataSubmissionTranslationViewModel Translation { get; }

	private ConfigDataSubmission Config { get; }

	[ObservableProperty]
	public partial bool SendDataToPoi { get; set; }

	public ConfigurationDataSubmissionViewModel(ConfigDataSubmission config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationDataSubmissionTranslationViewModel>();
		
		Config = config;
		Load();
	}

	private void Load()
	{
		SendDataToPoi = Config.SendDataToPoiPreview;
	}

	public override void Save()
	{
		Config.SendDataToPoiPreview = SendDataToPoi;
	}
}
