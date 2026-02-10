using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Utility;

public class SoftwareUpdater
{
	internal static string AppDataFolder =>
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElectronicObserver");

	private static bool WaitForRestart { get; set; }

	public static SoftwareUpdateData CurrentDataVersion { get; set; } = new();
	public static SoftwareUpdateData LatestDataVersion { get; set; } = new();

	public static TranslationUpdateData CurrentTranslationVersion { get; set; } = new();
	public static TranslationUpdateData LatestTranslationVersion { get; set; } = new();

	private static Uri DataUpdateURL => new($"{Configuration.Config.Control.UpdateRepoURL}/update.json");

	private static Uri TranslationUpdateURL => new($"{Configuration.Config.Control.UpdateRepoURL}/Translations/{DataAndTranslationManager.CurrentTranslationLanguage}/update.json");

	private static string DataUpdateFile => Path.Combine(DataAndTranslationManager.WorkingFolder, "update.json");

	private static string TranslationUpdateFile => Path.Combine(DataAndTranslationManager.TranslationFolder, "update.json");

	public static string DownloadProgressString { get; private set; } = "";

	public static SoftwareDownloadTranslationViewModel SoftwareDownload { get; } = new();

	/// <summary>
	/// Perform software update in background 
	/// </summary>
	public static async Task UpdateSoftware()
	{
		if (WaitForRestart) return;

		if (!Directory.Exists(AppDataFolder))
			Directory.CreateDirectory(AppDataFolder);

		var url = LatestDataVersion.AppDownloadUrl;
		if (url != string.Empty)
		{
			try
			{
				Logger.Add(1, string.Format(SoftwareInformationResources.StartedDownloadingUpdate, url));
				await DownloadUpdate(url);
				Logger.Add(1, SoftwareInformationResources.DownloadFinished);
			}
			catch
			{
				return;
			}
		}

		try
		{
			await DownloadUpdater();
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
		Logger.Add(2, SoftwareInformationResources.CloseElectronicObserverToCompleteTheUpdate);
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
	/// Check for update data, but only update translation data and game related data (Fit bonuses, equipment upgrades, ...)
	/// </summary>
	public static async Task CheckUpdateAsync()
	{
		Directory.CreateDirectory(Path.GetDirectoryName(TranslationUpdateFile)!);
		Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(DataUpdateFile)!, "Data"));

		try
		{
			await ReadRemoteAndLocalUpdateData();

			List<(string FileName, DataType Type)> downloadList = GetDownloadList();

			bool needReload = downloadList.Count > 0;

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
				Logger.Add(2, SoftwareInformationResources.TranslationFilesUpdated);
			}

			CurrentDataVersion = LatestDataVersion;
			CurrentTranslationVersion = LatestTranslationVersion;
		}
		catch (Exception e)
		{
			Logger.Add(3, SoftwareInformationResources.FailedToObtainUpdateData + e);
		}
	}

	private static List<(string FileName, DataType Type)> GetDownloadList()
	{
		List<(string FileName, DataType Type)> downloadList = [];

		if (CurrentTranslationVersion.Equipment != LatestTranslationVersion.Equipment)
			downloadList.Add(("equipment.json", DataType.Translation));

		if (CurrentTranslationVersion.Expedition != LatestTranslationVersion.Expedition)
			downloadList.Add(("expedition.json", DataType.Translation));

		if (CurrentDataVersion.Destination != LatestDataVersion.Destination)
			downloadList.Add((("destination.json", DataType.Data)));

		if (CurrentTranslationVersion.Operation != LatestTranslationVersion.Operation)
			downloadList.Add(("operation.json", DataType.Translation));

		if (CurrentTranslationVersion.Quest != LatestTranslationVersion.Quest)
			downloadList.Add(("quest.json", DataType.Translation));

		if (CurrentTranslationVersion.Ship != LatestTranslationVersion.Ship)
			downloadList.Add(("ship.json", DataType.Translation));

		if (CurrentDataVersion.QuestTrackers < LatestDataVersion.QuestTrackers)
		{
			downloadList.Add(("QuestTrackers.json", DataType.Data));
		}

		if (CurrentDataVersion.QuestsMetadata < LatestDataVersion.QuestsMetadata)
		{
			downloadList.Add(("QuestsMetadata.json", DataType.Data));
		}

		if (CurrentDataVersion.EventLocks < LatestDataVersion.EventLocks)
		{
			downloadList.Add(("Locks.json", DataType.Data));
		}

		if (CurrentTranslationVersion.LockTranslations < LatestTranslationVersion.LockTranslations)
		{
			downloadList.Add(("Locks.json", DataType.Translation));
		}

		if (CurrentDataVersion.FitBonuses < LatestDataVersion.FitBonuses)
		{
			downloadList.Add(("FitBonuses.json", DataType.Data));
		}

		if (CurrentDataVersion.EquipmentUpgrades < LatestDataVersion.EquipmentUpgrades)
		{
			downloadList.Add(("EquipmentUpgrades.json", DataType.Data));
		}

		return downloadList;
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
			(LatestDataVersion, LatestTranslationVersion) = ParseUpdate(dataUpdateData, translationUpdateData);
		}

		bool filesDoesntExist = !File.Exists(DataUpdateFile) || !File.Exists(TranslationUpdateFile);

		if (filesDoesntExist && updateDataReceived)
		{
			await File.WriteAllTextAsync(DataUpdateFile, dataUpdateData);
			await File.WriteAllTextAsync(TranslationUpdateFile, translationUpdateData);

			CurrentDataVersion = new();
			CurrentTranslationVersion = new();
		}
		else
		{
			string dataFileContent = File.ReadAllText(DataUpdateFile);
			string translationFileContent = File.ReadAllText(TranslationUpdateFile);
			(CurrentDataVersion, CurrentTranslationVersion) = ParseUpdate(dataFileContent, translationFileContent);
		}
	}

	private static async Task DownloadUpdater()
	{
		try
		{
			using HttpClient client = new();

			var url = @"https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/Data/EOUpdater.exe";
			var updaterFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\EOUpdater.exe";

			Progress<float> progress = new();
			progress.ProgressChanged += (_, progress) => DownloadProgressString = string.Format(SoftwareDownload.Update_DownloadingUpdater, progress);

			using FileStream file = new(updaterFile, FileMode.Create);
			await client.DownloadDataAsync(url, file, progress);

			Logger.Add(1, SoftwareInformationResources.UpdaterDownloadFinished);
		}
		catch (Exception e)
		{
			Logger.Add(3, SoftwareInformationResources.FailedToDownloadUpdater + e);
			throw;
		}
		finally
		{
			DownloadProgressString = "";
		}
	}

	private static async Task DownloadUpdate(string url)
	{
		try
		{
			using HttpClient client = new();
			string tempFile = AppDataFolder + @"\latest.zip"; ;

			Console.WriteLine(SoftwareInformationResources.DownloadingUpdate);

			Progress<float> progress = new();
			progress.ProgressChanged += (_, progress) => DownloadProgressString = string.Format(SoftwareDownload.Update_DownloadingUpdate, progress);

			using FileStream file = new(tempFile, FileMode.Create);
			await client.DownloadDataAsync(url, file, progress);
		}
		catch (Exception e)
		{
			Logger.Add(3, SoftwareInformationResources.FailedToDownloadElectronicObserver + e.Message);
			throw;
		}
		finally
		{
			DownloadProgressString = "";
		}
	}

	private static (SoftwareUpdateData, TranslationUpdateData) ParseUpdate(string dataJson, string translationJson)
	{
		try
		{
			SoftwareUpdateData? data = JsonSerializer.Deserialize<SoftwareUpdateData>(dataJson);
			TranslationUpdateData? translations = JsonSerializer.Deserialize<TranslationUpdateData>(translationJson);

			if (data is not null && translations is not null)
			{
				return (data, translations);
			}
		}
		catch (Exception e)
		{
			Logger.Add(3, SoftwareInformationResources.FailedToParseUpdateData + e.ToString());
		}

		return (new(), new());
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
				Logger.Add(1, string.Format(SoftwareInformationResources.FileUpdated, filename));
			}
		}
		catch (Exception e)
		{
			Logger.Add(3, string.Format(SoftwareInformationResources.FailedToUpdateFile, filename, e.Message));
			throw;
		}
	}
}
