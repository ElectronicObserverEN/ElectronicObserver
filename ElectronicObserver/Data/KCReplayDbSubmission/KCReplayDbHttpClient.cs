using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ElectronicObserver.Data.KCReplayDbSubmission.KCReplayDbQuestSubmission;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.KCReplayDbSubmission;

public class KCReplayDbHttpClient
{
	private static string SofwareName =>
#if DEBUG
		$"ElectronicObserverEN-DEBUG";
#else
		$"ElectronicObserverEN";
#endif

	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		// the default encoder doesn't like Japanese characters apparently
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	private static HttpClient MakeHttpClient()
	{
		HttpClient client = new()
		{
			BaseAddress = new("https://kcrdb.hitomaru.dev/")
		};

		client.DefaultRequestHeaders.Add("x-origin", SofwareName);
		client.DefaultRequestHeaders.Add("x-version", SoftwareInformation.VersionEnglish);

		return client;
	}

	private static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
	{
#if DEBUG
		if (response.RequestMessage?.Content is HttpContent content)
		{
			string requestBody = await content.ReadAsStringAsync();
			Logger.Add(3, requestBody);
		}
#endif

		if (response.IsSuccessStatusCode) return;

		string responseMessage = await response.Content.ReadAsStringAsync();
		string errorMessage = $"{response.ReasonPhrase} {responseMessage}";

		throw new HttpRequestException(errorMessage);
	}

	public async Task Quest(KCReplayDbQuestSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("quests", submission, JsonSerializerOptions);
		await EnsureSuccessStatusCode(response);
	}

	public async Task QuestItems(KCReplayDbQuestItemsSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("quest-items", submission, JsonSerializerOptions);
		await EnsureSuccessStatusCode(response);
	}
}
