using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereSubmissionService(BonodereSubmissionTranslationViewModel translations)
{
	private BonodereSubmissionTranslationViewModel BonodereSubmission { get; } = translations;

	private BonodereHttpClient BonodereClient { get; } = new();

	public string Username { get; set; } = "";

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
