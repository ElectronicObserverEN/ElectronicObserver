using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Utility.Mathematics;

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

			List<(string FileName, DataType Type)> downloadList = new();
			bool needReload = false;

			if (CurrentVersion.Equipment != LatestVersion.Equipment)
				downloadList.Add(("equipment.json", DataType.Translation));

			if (CurrentVersion.Expedition != LatestVersion.Expedition)
				downloadList.Add(("expedition.json", DataType.Translation));

			if (CurrentVersion.Destination != LatestVersion.Destination)
				downloadList.Add((("destination.json", DataType.Data)));

			if (CurrentVersion.Operation != LatestVersion.Operation)
				downloadList.Add(("operation.json", DataType.Translation));

			if (CurrentVersion.Quest != LatestVersion.Quest)
				downloadList.Add(("quest.json", DataType.Translation));

			if (CurrentVersion.Ship != LatestVersion.Ship)
				downloadList.Add(("ship.json", DataType.Translation));

			if (CurrentVersion.QuestTrackers < LatestVersion.QuestTrackers)
			{
				downloadList.Add(("QuestTrackers.json", DataType.Data));
			}

			if (CurrentVersion.EventLocks < LatestVersion.EventLocks)
			{
				downloadList.Add(("Locks.json", DataType.Data));
			}

			if (CurrentVersion.LockTranslations < LatestVersion.LockTranslations)
			{
				downloadList.Add(("Locks.json", DataType.Translation));
			}

			if (CurrentVersion.FitBonuses < LatestVersion.FitBonuses)
			{
				downloadList.Add(("FitBonuses.json", DataType.Data));
			}

			if (CurrentVersion.EquipmentUpgrades < LatestVersion.EquipmentUpgrades)
			{
				downloadList.Add(("EquipmentUpgrades.json", DataType.Data));
			}

			needReload = downloadList.Any();

			List<Task> taskList = new();

			foreach ((string fileName, DataType type) in downloadList)
			{
				taskList.Add(Task.Run(() => DownloadData(fileName, type)));
			}

			await Task.WhenAll(taskList);

			// it's possible that one of the other files fails to download (exception gets thrown)
			// only update the update files after all files were downloaded successfully
			await DownloadData("update.json", DataType.None);
			await DownloadData("update.json", DataType.Translation);

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
			int lockTranslationsVersion = (int)translationJson.Locks;

			int fitBonusesVersion = dataJson.FitBonuses() switch
			{
				true => (int)dataJson.FitBonuses,
				_ => 0
			};

			int questTrackersVersion = dataJson.QuestTrackers() switch
			{
				true => (int)dataJson.QuestTrackers,
				_ => 0
			};
			int eventLocksVersion = (int)dataJson.Locks;

			int equipmentUpgradesVersion = dataJson.EquipmentUpgrades() switch
			{
				true => (int)dataJson.EquipmentUpgrades,
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
				EventLocks = eventLocksVersion,
				LockTranslations = lockTranslationsVersion,
				MaintenanceDate = maintenanceDate,
				EventState = eventState,
				FitBonuses = fitBonusesVersion,
				EquipmentUpgrades = equipmentUpgradesVersion,
			};
		}
		catch (Exception e)
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.FailedToParseUpdateData + e.ToString());
		}
		return data;
	}

	private static string GetFullPath(string fileName, DataType type) => type switch
	{
		DataType.Translation => Path.Combine("Translations", DataAndTranslationManager.CurrentTranslationLanguage, fileName),
		DataType.Data => Path.Combine("Data", fileName),
		DataType.None => fileName,
	};

	public static async Task DownloadData(string filename, DataType type)
	{
		filename = GetFullPath(filename, type);

		string path = Path.Combine(DataAndTranslationManager.WorkingFolder, filename);
		string url = Path.Combine(Configuration.Config.Control.UpdateRepoURL.AbsoluteUri, filename);

		try
		{
			using HttpClient client = new();
			using HttpResponseMessage response = await client.GetAsync(url);

			response.EnsureSuccessStatusCode();

			await File.WriteAllTextAsync(path, await response.Content.ReadAsStringAsync());

			if (filename.Contains("update.json") == false)
			{
				Logger.Add(1, string.Format(Properties.Utility.SoftwareInformation.FileUpdated, filename));
			}
		}
		catch (Exception e)
		{
			Logger.Add(3, string.Format(Properties.Utility.SoftwareInformation.FailedToUpdateFile, filename, e.Message));
			throw;
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
	public int EventLocks { get; set; }
	public int LockTranslations { get; set; }
	public int FitBonuses { get; set; }
	public int EquipmentUpgrades { get; set; }
	public DateTime MaintenanceDate { get; set; }

	/// <summary>
	/// 1=event start, 2=event end, 3=regular maintenance
	/// </summary>
	public MaintenanceState EventState { get; set; }
}
public enum MaintenanceState { None = 0, EventStart = 1, EventEnd = 2, Regular = 3 };
