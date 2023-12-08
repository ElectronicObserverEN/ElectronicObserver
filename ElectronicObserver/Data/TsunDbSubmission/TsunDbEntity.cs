using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.TsunDbSubmission;

public abstract class TsunDbEntity
{
	/// <summary>
	/// Is initialized ? 
	/// </summary>
	[JsonIgnore]
	public bool IsInitialized { get; set; } = true;

	protected abstract string Url { get; }

	protected virtual bool IsBetaAPI => false;

	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		
	};

	public void SendData()
	{
		if (!IsInitialized) return;

		if (IsBetaAPI)
		{
			string jsonContent = MakeJson();
			WriteJson(jsonContent);
			return;
		}

		Task sendData = new Task(async () =>
		{
			try
			{
				HttpResponseMessage result = await HttpSendData();
				string response = await result.Content.ReadAsStringAsync();

				if (!result.IsSuccessStatusCode)
				{
					throw new Exception("Error " + result.StatusCode + ": " + response);
				}
			}
			catch (Exception ex)
			{
				ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
			}
		});

		sendData.Start();
	}

	private async Task<HttpResponseMessage> HttpSendData()
	{
		using var httpClient = new HttpClient();
		string contentSerialized = MakeJson();
		StringContent content = new StringContent(contentSerialized);

		content.Headers.Add("tsun-ver", "Kasumi Kai");
		content.Headers.Add("dataorigin", "eo");
		content.Headers.Add("version", SoftwareInformation.VersionEnglish);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		if (Configuration.Config.Debug.EnableDebugMenu)
		{
			WriteJson(contentSerialized);
		}

		return await httpClient.PutAsync($"https://tsundb.kc3.moe/api/{this.Url}", content);
	}

	private string MakeJson()
	{
		// the cast to object is needed to serialize an abstract class
		return JsonSerializer.Serialize((object)this, JsonSerializerOptions);
	}

	private void WriteJson(string contentSerialized)
	{
		Directory.CreateDirectory("TsunDb");

		string path = Path.Combine("TsunDb", $"tsundb_{Url}.json");

		File.WriteAllText(path, contentSerialized);
	}
}
