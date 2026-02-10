using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.ElectronicObserverApi;

public class ElectronicObserverApiService(ElectronicObserverApiTranslationViewModel translations)
{
	private HttpClient Client { get; } = new()
	{
		DefaultRequestHeaders =
		{
			UserAgent = { new("ElectronicObserverEN", SoftwareInformation.VersionEnglish) },
		},
	};

	private string Url => Configuration.Config.Debug.ElectronicObserverApiUrl switch
	{
		{ Length: >0 } => Configuration.Config.Debug.ElectronicObserverApiUrl,
		_ => SoftwareUpdater.CurrentDataVersion.AppApiServerUrl,
	};

	private ElectronicObserverApiTranslationViewModel Translations { get; } = translations;

	public bool IsServerAvailable => !string.IsNullOrEmpty(Url);
	
	public async Task PostJson<T>(string route, T data)
	{
		if (!IsServerAvailable) return;

		try
		{
			HttpResponseMessage response = await Client.PostAsJsonAsync(Path.Combine(Url, route), data);

			response.EnsureSuccessStatusCode();
		}
		catch (Exception ex)
		{
			Utility.Logger.Add(1, string.Format($"{Translations.ElectronicObserverApi} {ex.Message}"));
		}
	}
}
