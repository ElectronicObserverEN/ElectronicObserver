using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ElectronicObserver.Utility.ElectronicObserverApi;

public class ElectronicObserverApiService
{
	private HttpClient Client { get; } = new();

	private string Url => Configuration.Config.Debug.ElectronicObserverApiUrl;

	private ElectronicObserverApiTranslationViewModel Translations { get; }

	public ElectronicObserverApiService(ElectronicObserverApiTranslationViewModel translations)
	{
		Translations = translations;
	}

	public async Task PostJson<T>(string route, T data)
	{
		if (string.IsNullOrEmpty(Url)) return;

		try
		{
			HttpResponseMessage response = await Client.PostAsJsonAsync(Path.Combine(Url, route), data);

			response.EnsureSuccessStatusCode();
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, Translations.ElectronicObserverApi);
		}
	}
}
