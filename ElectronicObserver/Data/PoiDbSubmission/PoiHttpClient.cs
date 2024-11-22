using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

namespace ElectronicObserver.Data.PoiDbSubmission;

public class PoiHttpClient
{
	private static HttpClient MakeHttpClient() => new()
	{
		BaseAddress = new("http://report2.kcwiki.org:17027/api/report/"),
	};

	public async Task Quest(PoiDbQuestSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("/quest", submission);
		response.EnsureSuccessStatusCode();
	}

	public async Task Battle(PoiDbBattleSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("/battle", submission);
		response.EnsureSuccessStatusCode();
	}

	public async Task FriendFleet(Dictionary<string, Dictionary<string, JsonNode?>> submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("/friendly_info", submission);
		response.EnsureSuccessStatusCode();
	}

	public async Task AirDefense(Dictionary<string, JsonNode?> submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("/air_base_attack", submission);
		response.EnsureSuccessStatusCode();
	}

	public async Task Route(PoiDbRouteSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("/next_way_v2", submission);
		response.EnsureSuccessStatusCode();
	}
}
