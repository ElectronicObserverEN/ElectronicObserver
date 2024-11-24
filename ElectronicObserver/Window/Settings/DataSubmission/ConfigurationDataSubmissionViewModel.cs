using System;
using System.Security;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data.Bonodere;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
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
	public partial string LoginError { get; set; } = "";

	public bool? SubmitDataToTsunDb { get; set; }

	public ConfigurationDataSubmissionViewModel(ConfigDataSubmission config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationDataSubmissionTranslationViewModel>();
		BonodereSubmissionService = Ioc.Default.GetRequiredService<BonodereSubmissionService>();

		Config = config;
		Load();


		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SubmitDataToTsunDb)) return;
			if (SubmitDataToTsunDb is false) return;

			// --- ask for confirmation
			DialogTsunDb dialogTsunDb = new();
			dialogTsunDb.FormClosed += (sender, e) =>
			{
				if (sender is not DialogTsunDb dialog) return;

				SubmitDataToTsunDb = dialog.DialogResult == System.Windows.Forms.DialogResult.Yes;
			};
			dialogTsunDb.ShowDialog(App.Current!.MainWindow!);
		};
	}

	private void Load()
	{
		SendDataToPoi = Config.SendDataToPoiPreview;

		BonodereUserId = Config.BonodereUserId;
		BonodereToken = Config.BonodereToken;
		BonodereUsername = BonodereSubmissionService.Username;

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
}
