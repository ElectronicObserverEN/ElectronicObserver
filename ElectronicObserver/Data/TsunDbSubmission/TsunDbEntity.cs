using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ElectronicObserver.Utility;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public abstract class TsunDbEntity
{
	[JsonIgnore]
	/// <summary>
	/// Is initialized ? 
	/// </summary>
	public bool IsInitialized { get; set; } = true;

	protected abstract string Url { get; }

	protected virtual bool IsBetaAPI { get; } = false;

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
				var result = await this.HttpSendData();
				string response = result.Content.ReadAsStringAsync().Result;

				if (!result.IsSuccessStatusCode)
				{
					throw new Exception("Error " + result.StatusCode + ": " + response);
				}
			}
			catch (Exception ex)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
			}
		});

		sendData.Start();
	}

	private async Task<HttpResponseMessage> HttpSendData()
	{
		using (var httpClient = new HttpClient())
		{
			string contentSerialized = MakeJson();
			StringContent content = new StringContent(contentSerialized);

			content.Headers.Add("tsun-ver", "Kasumi Kai");
			content.Headers.Add("dataorigin", "eo");
			content.Headers.Add("version", SoftwareInformation.VersionEnglish);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			if (Configuration.Config.Debug.EnableDebugMenu) WriteJson(contentSerialized);

			return await httpClient.PutAsync($"https://tsundb.kc3.moe/api/{this.Url}", content);
		}
	}

	private string MakeJson()
	{
		JsonSerializerSettings jsonSerializer = new JsonSerializerSettings()
		{
			CheckAdditionalContent = true,
			NullValueHandling = NullValueHandling.Ignore
		};

		return JsonConvert.SerializeObject(this, jsonSerializer);
	}

	private void WriteJson(string contentSerialized)
	{
		if (!Directory.Exists("TsunDb")) Directory.CreateDirectory("TsunDb");

		string path = Path.Combine("TsunDb", $"tsundb_{this.Url}.json");

		File.WriteAllText(path, contentSerialized);
	}
}
