using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.Bonodere;

namespace ElectronicObserver.Window.Settings.DataSubmission;

public partial class ConfigurationDataSubmissionViewModel : ConfigurationViewModelBase
{
	public ConfigurationDataSubmissionTranslationViewModel Translation { get; }

	private ConfigDataSubmission Config { get; }

	public BonodereSubmissionService BonodereSubmissionService { get; }

	[ObservableProperty]
	public partial bool SendDataToPoi { get; set; }

	[ObservableProperty]
	public partial string BonodereUsername { get; set; } = "";

	[ObservableProperty]
	public partial string BonoderePassword { get; set; } = "";

	public ConfigurationDataSubmissionViewModel(ConfigDataSubmission config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationDataSubmissionTranslationViewModel>();
		BonodereSubmissionService = Ioc.Default.GetRequiredService<BonodereSubmissionService>();

		Config = config;
		Load();
	}

	private void Load()
	{
		SendDataToPoi = Config.SendDataToPoiPreview;
		BonodereUsername = Config.BonodereUsername;
		BonoderePassword = Config.BonoderePassword;
	}

	public override void Save()
	{
		Config.SendDataToPoiPreview = SendDataToPoi;
		Config.BonodereUsername = BonodereUsername;
		Config.BonoderePassword = BonoderePassword;
	}

	[RelayCommand]
	private async Task BonodereLogin()
	{
		await BonodereSubmissionService.Login(BonodereUsername, BonoderePassword);
	}

	[RelayCommand]
	private async Task BonodereLogout()
	{
		await BonodereSubmissionService.Logout();
	}
}
