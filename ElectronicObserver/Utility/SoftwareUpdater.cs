using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window;
using AppSettings = ElectronicObserver.Properties.Settings;

namespace ElectronicObserver.Utility;

internal class SoftwareUpdater
{
	internal static string AppDataFolder =>
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElectronicObserver");

	private static bool WaitForRestart { get; set; }
	public static UpdateData CurrentVersion { get; set; } = new UpdateData();
	public static UpdateData LatestVersion { get; set; } = new UpdateData();

	private static Uri DataUpdateURL => new($"{Configuration.Config.Control.UpdateRepoURL}/update.json");

	private static Uri TranslationUpdateURL => new($"{Configuration.Config.Control.UpdateRepoURL}/Translations/{DataAndTranslationManager.CurrentTranslationLanguage}/update.json");

	private static string DataUpdateFile => Path.Combine(DataAndTranslationManager.WorkingFolder, "update.json");

	private static string TranslationUpdateFile => Path.Combine(DataAndTranslationManager.TranslationFolder, "update.json");

	/// <summary>
	/// Perform software update in background 
	/// </summary>
	public static void UpdateSoftware()
	{
		if (WaitForRestart) return;

		if (!Directory.Exists(AppDataFolder))
			Directory.CreateDirectory(AppDataFolder);

		var url = LatestVersion.AppDownloadUrl;
		if (url != string.Empty)
		{
			try
			{
				Logger.Add(1, string.Format(Properties.Utility.SoftwareInformation.StartedDownloadingUpdate, url));
				DownloadUpdate(url);
				Logger.Add(1, Properties.Utility.SoftwareInformation.DownloadFinished);
			}
			catch
			{
				return;
			}
		}

		try
		{
			DownloadUpdater();
		}
		catch
		{
			return;
		}

		var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";
		if (!File.Exists(updaterFile)) return;
		var updater = new Process
		{
			StartInfo =
			{
				FileName = updaterFile,
				UseShellExecute = false,
				CreateNoWindow = false,
				Arguments = "--restart"
			}
		};
		updater.Start();
		Logger.Add(2, Properties.Utility.SoftwareInformation.CloseElectronicObserverToCompleteTheUpdate);
		WaitForRestart = true;
	}

	public static async Task PeriodicUpdateCheckAsync(CancellationToken cancellationToken)
	{
		while (true)
		{
			// Check for update every 1 hour.
			await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
			await CheckUpdateAsync();
		}
	}

	/// <summary>
	/// Check for update data, but only update translation data.
	/// </summary>
	public static async Task CheckUpdateAsync()
	{
		Directory.CreateDirectory(Path.GetDirectoryName(TranslationUpdateFile)!);
		Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(DataUpdateFile)!, "Data"));

		try
		{
			await ReadRemoteAndLocalUpdateData();

			/*if (Configuration.Config.Life.CheckUpdateInformation == true && SoftwareInformation.UpdateTime < LatestVersion.BuildDate)
			{
				FormMain.Instance.Update_Available(LatestVersion.AppVersion);
				UpdateSoftware();
			}*/

			var downloadList = new List<string>();
			var needReload = false;

			if (CurrentVersion.Equipment != LatestVersion.Equipment)
				downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "equipment.json"));

			if (CurrentVersion.Expedition != LatestVersion.Expedition)
				downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "expedition.json"));

			if (CurrentVersion.Destination != LatestVersion.Destination)
				downloadList.Add(Path.Combine("Data", "destination.json"));

			if (CurrentVersion.Operation != LatestVersion.Operation)
				downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "operation.json"));

			if (CurrentVersion.Quest != LatestVersion.Quest)
				downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "quest.json"));

			if (CurrentVersion.Ship != LatestVersion.Ship)
				downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "ship.json"));

			if (CurrentVersion.QuestTrackers < LatestVersion.QuestTrackers)
			{
				downloadList.Add(Path.Combine("Data", "QuestTrackers.json"));
			}

			needReload = downloadList.Any();
			downloadList.Add("update.json");
			downloadList.Add(Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, "update.json"));

			var taskList = new List<Task>();
			var filenames = downloadList.ToArray();
			foreach (var filename in filenames)
			{
				taskList.Add(Task.Run(() => DownloadData(filename)));
			}

			await Task.WhenAll(taskList);
			if (needReload)
			{
				KCDatabase.Instance.Translation.Initialize();
				KCDatabase.Instance.SystemQuestTrackerManager.Load();
				Logger.Add(2, Properties.Utility.SoftwareInformation.TranslationFilesUpdated);
			}

			CurrentVersion = LatestVersion;
		}
		catch (JsonParserException e)
		{
			// file exist but isn't valid json
			// file gets corrupted somehow?
			File.Delete(TranslationUpdateFile);
			File.Delete(DataUpdateFile);
			await CheckUpdateAsync();
		}
		catch (Exception e)
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.FailedToObtainUpdateData + e);
		}
	}

	/// <summary>
	/// Read remote (EO repository) and local update data to compare them later and download the required updates
	/// </summary>
	/// <returns></returns>
	private static async Task ReadRemoteAndLocalUpdateData()
	{
		using HttpClient client = new HttpClient();
		using HttpResponseMessage dataUpdateResponse = await client.GetAsync(DataUpdateURL);
		using HttpResponseMessage translationUpdateResponse = await client.GetAsync(TranslationUpdateURL);

		string dataUpdateData = await dataUpdateResponse.Content.ReadAsStringAsync();
		string translationUpdateData = await translationUpdateResponse.Content.ReadAsStringAsync();

		bool updateDataReceived = !string.IsNullOrEmpty(dataUpdateData) && !string.IsNullOrEmpty(translationUpdateData);

		if (updateDataReceived)
		{
			var jsonData = JsonObject.Parse(dataUpdateData);
			var jsonTranslations = JsonObject.Parse(translationUpdateData);

			LatestVersion = ParseUpdate(jsonData, jsonTranslations);
		}

		bool filesDoesntExist = !File.Exists(DataUpdateFile) || !File.Exists(TranslationUpdateFile);

		if (filesDoesntExist && updateDataReceived)
		{
			await File.WriteAllTextAsync(DataUpdateFile, dataUpdateData);
			await File.WriteAllTextAsync(TranslationUpdateFile, translationUpdateData);

			CurrentVersion = new UpdateData();
		}
		else
		{
			string dataFileContent = File.ReadAllText(DataUpdateFile);
			string translationFileContent = File.ReadAllText(TranslationUpdateFile);
			CurrentVersion = ParseUpdate(JsonObject.Parse(dataFileContent), JsonObject.Parse(translationFileContent));
		}
	}

	private static void DownloadUpdater()
	{
		try
		{
			using var client = new WebClient();
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			var url = @"https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/Data/EOUpdater.exe";
			var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";

			client.DownloadFile(url, updaterFile);
			Logger.Add(1, Properties.Utility.SoftwareInformation.UpdaterDownloadFinished);
		}
		catch (Exception e)
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.FailedToDownloadUpdater + e);
			throw;
		}
	}

	private static void DownloadUpdate(string url)
	{
		try
		{
			using var client = new WebClient();
			string tempFile = AppDataFolder + @"\latest.zip"; ;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			Console.WriteLine(Properties.Utility.SoftwareInformation.DownloadingUpdate);
			client.DownloadFile(url, tempFile);
		}
		catch (Exception e)
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.FailedToDownloadElectronicObserver + e.Message);
			throw;
		}
	}

	internal static UpdateData ParseUpdate(dynamic dataJson, dynamic translationJson)
	{
		var data = new UpdateData();
		try
		{
			DateTime buildDate = DateTimeHelper.CSVStringToTime(dataJson.bld_date);
			var appVersion = (string)dataJson.ver;
			var note = (string)dataJson.note;
			var downloadUrl = (string)dataJson.url;

			var eqVersion = (string)translationJson.equipment;
			var expedVersion = (string)translationJson.expedition;
			string destVersion = dataJson.nodes.ToString();
			var opVersion = (string)translationJson.operation;
			var questVersion = (string)translationJson.quest;
			var shipVersion = (string)translationJson.ship;

			int questTrackersVersion = dataJson.QuestTrackers() switch
			{
				true => (int)dataJson.QuestTrackers,
				_ => 0
			};

			DateTime maintenanceDate = DateTimeHelper.CSVStringToTime(dataJson.kancolle_mt);
			var eventState = (MaintenanceState)(int)dataJson.event_state;

			data = new UpdateData
			{
				BuildDate = buildDate,
				AppVersion = appVersion,
				Note = note,
				AppDownloadUrl = downloadUrl,
				Equipment = eqVersion,
				Expedition = expedVersion,
				Destination = destVersion,
				Operation = opVersion,
				Quest = questVersion,
				Ship = shipVersion,
				QuestTrackers = questTrackersVersion,
				MaintenanceDate = maintenanceDate,
				EventState = eventState
			};
		}
		catch (Exception e)
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.FailedToParseUpdateData + e.ToString());
		}
		return data;
	}

	internal static async Task DownloadData(string filename)
	{
		string path = Path.Combine(DataAndTranslationManager.WorkingFolder, filename);
		string  url = Path.Combine(Configuration.Config.Control.UpdateRepoURL.AbsoluteUri, filename);
		try
		{
			using var client = new HttpClient();
			using HttpResponseMessage response = await client.GetAsync(url);

			using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
			await response.Content.CopyToAsync(fs);

			if (filename.Contains("update.json") == false)
				Logger.Add(1, string.Format(Properties.Utility.SoftwareInformation.FileUpdated, filename));
		}
		catch (Exception e)
		{
			Logger.Add(3, string.Format(Properties.Utility.SoftwareInformation.FailedToUpdateFile, filename, e.Message));
		}
	}
}

public class UpdateData
{
	public DateTime BuildDate { get; set; }
	public string AppVersion { get; set; } = "0.0.0.0";
	public string Note { get; set; } = "";
	public string AppDownloadUrl { get; set; } = "";
	public string Equipment { get; set; } = "";
	public string Expedition { get; set; } = "";
	public string Destination { get; set; } = "";
	public string Operation { get; set; } = "";
	public string Quest { get; set; } = "";
	public string Ship { get; set; } = "";
	public int QuestTrackers { get; set; }
	public DateTime MaintenanceDate { get; set; }

	/// <summary>
	/// 1=event start, 2=event end, 3=regular maintenance
	/// </summary>
	public MaintenanceState EventState { get; set; }
}
public enum MaintenanceState { None = 0, EventStart = 1, EventEnd = 2, Regular = 3 };
