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

	private string Url => "https://localhost:44344/"; // Configuration.Config.Debug.ElectronicObserverApiUrl;

	private ElectronicObserverApiTranslationViewModel Translations { get; } = translations;

	public bool IsEnabled => !string.IsNullOrEmpty(Url);

	public async Task PostJson<T>(string route, T data)
	{
		if (!IsEnabled) return;

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
