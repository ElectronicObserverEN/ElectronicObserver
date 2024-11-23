using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereSubmissionService
{
	private BonodereSubmissionTranslationViewModel BonodereSubmission { get; }

	private BonodereHttpClient BonodereClient { get; } = new();

	public string Username { get; set; } = "";

	public bool IsReady => BonodereClient.IsReady;

	public BonodereSubmissionService(BonodereSubmissionTranslationViewModel translations)
	{
		BonodereSubmission = translations;

		if (!string.IsNullOrEmpty(Configuration.Config.DataSubmission.BonodereUsername))
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			AfterConfigChange();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		Configuration.Instance.ConfigurationChanged += async () => await AfterConfigChange();
	}

	public async Task AfterConfigChange()
	{
		if (string.IsNullOrEmpty(Configuration.Config.DataSubmission.BonodereUsername))
		{
			if (BonodereClient.IsReady)
			{
				await BonodereClient.Logout();
			}

			return;
		}

		await Login(Configuration.Config.DataSubmission.BonodereUsername, Configuration.Config.DataSubmission.BonoderePassword);
	}

	public async Task Login(string login, string password)
	{
		try
		{
			BonodereLoginResponse? loginResponse = await BonodereClient.Login(login, password);
			Username = loginResponse?.Username ?? "";
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	public async Task Logout()
	{
		try
		{
			await BonodereClient.Logout();
			Username = "";
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	public async Task SubmitData(List<SenkaEntryModel> data)
	{
		if (!BonodereClient.IsReady) return;

		if (!IsDataValid(data))
		{
			Logger.Add(2, $"Bonodere error: {BonodereSubmission.InconsistantDataDetected}");
			return;
		}

		try
		{
			await BonodereClient.SubmitData(data);
		}
		catch (Exception ex)
		{
			LogError(ex);
		}
	}

	private bool IsDataValid(List<SenkaEntryModel> data)
	{
		if (data.Any(entry => entry.Points == 0)) return false;
		if (data.Any(entry => entry.Position > 500)) return false;
		if (data.Any(entry => entry.Position <= 0)) return false;

		return true;
	}

	private static void LogError(Exception e)
	{
		Logger.Add(2, "Bonodere error", e);
	}
}
