using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;

namespace ElectronicObserver.Services.ApiFileService;

// this is suboptimal but I'm not sure how else to migrate old files to the new format
// migrating all at once is very expensive
public class ApiFileService : ObservableObject
{
	/// <summary>
	/// Version 1 details: <br />
	/// - removes the "svdata=" prefix from response body <br />
	/// - uses UTC time instead of local time <br />
	/// - "api_port/port" is trimmed <br />
	/// </summary>
	private static int CurrentApiFileVersion => 1;

	private ElectronicObserverContext Db { get; } = new();

	private BlockingCollection<ApiFileData> ApiFileQueue { get; } = new();

	private SortieRecord? SortieRecord { get; set; }

	private static List<string> IgnoredApis { get; } = new()
	{
		"api_start2/getData",
		"api_get_member/questlist",
		"api_get_member/slot_item",
		"api_get_member/unsetslot",
		"api_get_member/ship3",
		"api_get_member/furniture",
		"api_get_member/require_info",
	};

	public ApiFileService()
	{
		// https://michaelscodingspot.com/c-job-queues/
		Thread thread = new(ProcessQueue)
		{
			IsBackground = true,
		};
		thread.Start();
	}

	private async void ProcessQueue()
	{
		foreach (ApiFileData apiFile in ApiFileQueue.GetConsumingEnumerable())
		{
			await SaveApiData(apiFile.ApiName, apiFile.RequestBody, apiFile.ResponseBody);
		}
	}

	private async Task SaveApiData(string apiName, string requestBody, string responseBody)
	{
		if (IgnoredApis.Contains(apiName)) return;

		responseBody = TrimSvdata(responseBody);
		responseBody = TrimPort(apiName, responseBody);

		ApiFile requestFile = new()
		{
			ApiFileType = ApiFileType.Request,
			Name = apiName,
			Content = requestBody,
			TimeStamp = DateTime.UtcNow,
			Version = CurrentApiFileVersion,
		};

		ApiFile responseFile = new()
		{
			ApiFileType = ApiFileType.Response,
			Name = apiName,
			Content = responseBody,
			TimeStamp = DateTime.UtcNow,
			Version = CurrentApiFileVersion,
		};

#pragma warning disable CS0618
		await Db.ApiFiles.AddAsync(requestFile);
		await Db.ApiFiles.AddAsync(responseFile);
#pragma warning restore CS0618

		await ProcessSortieData(requestFile, responseFile);

		await Db.SaveChangesAsync();
	}

	public Task Add(string apiName, string requestBody, string responseBody)
	{
		ApiFileQueue.Add(new(apiName, requestBody, responseBody));

		return Task.CompletedTask;
	}

	public void SaveChanges()
	{
		Db.SaveChanges();
	}

	public List<ApiFile> Query(Func<IQueryable<ApiFile>, IQueryable<ApiFile>> query)
	{
#pragma warning disable CS0618
		var apiFiles = query(Db.ApiFiles).ToList();
#pragma warning restore CS0618

		foreach (ApiFile file in apiFiles)
		{
			if (file.Version is 0)
			{
				Version0To1(file);
			}
		}

		return apiFiles;
	}

	private static void Version0To1(ApiFile file)
	{
		if (file.ApiFileType == ApiFileType.Response)
		{
			file.Content = TrimSvdata(file.Content);
		}

		file.TimeStamp = file.TimeStamp.ToUniversalTime();
	}

	private static string TrimSvdata(string responseBody)
	{
		if (responseBody.StartsWith("s"))
		{
			// trim "svdata="
			responseBody = responseBody[7..];
		}

		return responseBody;
	}

	private static string TrimPort(string apiName, string responseBody)
	{
		if (apiName is not "api_port/port") return responseBody;

		try
		{
			ApiResponse<ApiPortPortResponse>? response = JsonSerializer
				.Deserialize<ApiResponse<ApiPortPortResponse>>(responseBody);

			if (response?.ApiData is not null)
			{
				response.ApiData.ApiShip = new();
				response.ApiData.ApiDeckPort = new();
				response.ApiData.ApiLog = new();
				response.ApiData.ApiNdock = new();

				responseBody = JsonSerializer.Serialize(response);
			}
			else
			{
				// todo: report failed parsing
			}
		}
		catch
		{
			// todo: report failed parsing
		}

		return responseBody;
	}

	private async Task ProcessSortieData(ApiFile requestFile, ApiFile responseFile)
	{
		if (requestFile.Name is "api_port/port")
		{
			SortieRecord = null;
			return;
		}

		if (requestFile.Name is "api_req_map/start")
		{
			if (SortieRecord is not null)
			{
				// todo: log bug - SortieRecord should always be null before a sortie starts
			}

			ApiResponse<ApiReqMapStartResponse>? response = null;

			try
			{
				response = JsonSerializer.Deserialize<ApiResponse<ApiReqMapStartResponse>>(responseFile.Content);
			}
			catch
			{
				// todo: report failed parsing
			}

			SortieRecord = new()
			{
				World = response?.ApiData?.ApiMapareaId ?? 0,
				Map = response?.ApiData?.ApiMapinfoNo ?? 0,
			};

			await Db.Sorties.AddAsync(SortieRecord);
		}

		if (SortieRecord is null)
		{
			// this should be all apis that are not related to a sortie
			// apis related to sortie are all api calls that happen between
			// "api_req_map/start" and "api_port/port"
			return;
		}

		SortieRecord.ApiFiles.Add(requestFile);
		SortieRecord.ApiFiles.Add(responseFile);
	}
}
