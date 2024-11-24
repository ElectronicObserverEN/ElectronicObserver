using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Navigation;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data.Bonodere;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;

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

	private string BonodereUserId { get; set; } = "";

	private string BonodereToken { get; set; } = "";

	public bool IsBonodereReady => !string.IsNullOrEmpty(BonodereToken);

	[ObservableProperty]
	public partial bool EnableBonodereIntegration { get; set; }

	[ObservableProperty]
	public partial string LoginError { get; set; } = "";

	public bool SubmitDataToTsunDb { get; set; }

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

		BonodereUserId = Config.BonodereUserId;
		BonodereToken = Config.BonodereToken;
		BonodereUsername = BonodereSubmissionService.Username;
		EnableBonodereIntegration = !string.IsNullOrEmpty(Config.BonodereToken);

		SubmitDataToTsunDb = Config.SubmitDataToTsunDb;
	}

	public override void Save()
	{
		Config.SendDataToPoiPreview = SendDataToPoi;
		Config.BonodereUserId = BonodereUserId;
		Config.BonodereToken = BonodereToken;
		Config.SubmitDataToTsunDb = SubmitDataToTsunDb;
	}

	[RelayCommand]
	private async Task BonodereLogin(SecureString password)
	{
		LoginError = "";

		await BonodereSubmissionService.Logout();

		try
		{

			BonodereLoginResponse? response = await BonodereSubmissionService.Login(BonodereUsername, password);

			if (response is null) return;

			BonodereUserId = response.User.Id;
			BonodereToken = response.Token;
			BonodereUsername = response.User.UserInfo.Username;

			OnPropertyChanged(nameof(IsBonodereReady));
		}
		catch (Exception ex)
		{
			LoginError = ex.Message;

			Logger.Add(2, "Bonodere error", ex);
		}
	}

	[RelayCommand]
	private void BonodereLogout()
	{
		BonodereToken = "";
		BonodereUserId = "";
		BonodereUsername = "";

		OnPropertyChanged(nameof(IsBonodereReady));
	}

	[RelayCommand]
	private void OpenHyperlink(string uri)
	{
		ProcessStartInfo psi = new()
		{
			FileName = uri,
			UseShellExecute = true,
		};

		Process.Start(psi);
	}
}
