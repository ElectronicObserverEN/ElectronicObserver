using System;
using System.Text;
using DynaJson;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Utility;

/// <summary>
/// ソフトウェアの情報を保持します。
/// </summary>
public static class SoftwareInformation
{
	/// <summary>
	/// ソフトウェア名(日本語)
	/// </summary>
	public static string SoftwareNameJapanese => "七四式電子観測儀";


	/// <summary>
	/// ソフトウェア名(英語)
	/// </summary>
	public static string SoftwareNameEnglish => "Electronic Observer";


	/// <summary>
	/// バージョン(日本語, ソフトウェア名を含みます)
	/// </summary>
	public static string VersionJapanese => SoftwareNameJapanese + "四七型改";


	/// <summary>
	/// バージョン(英語)
	/// </summary>
	public static string VersionEnglish => typeof(App).Assembly.GetName().Version.ToString();

	/// <summary>
	/// 更新日時
	/// </summary>
	public static DateTime UpdateTime => new DateTime(Generated.BuildInfo.TimeStamp) + TimeSpan.FromHours(9);

	private static System.Net.WebClient? Client { get; set; }

	private static Uri Uri { get; } =
		new Uri(
			"https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/update.json");

	public static void CheckUpdate()
	{
		if (!Configuration.Config.Life.CheckUpdateInformation) return;

		if (Client == null)
		{
			Client = new System.Net.WebClient
			{
				Encoding = new UTF8Encoding(false)
			};
			Client.DownloadStringCompleted += DownloadStringCompleted;
		}

		if (!Client.IsBusy)
			Client.DownloadStringAsync(Uri);
	}

	private static void DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			ErrorReporter.SendErrorReport(e.Error, SoftwareInformationResources.FailedToObtainUpdateData);
			return;
		}

		if (e.Result.StartsWith("<!DOCTYPE html>"))
		{
			Logger.Add(3, SoftwareInformationResources.InvalidUpdateUrl);
			return;
		}

		try
		{
			var json = JsonObject.Parse(e.Result);
			DateTime date = DateTimeHelper.CSVStringToTime(json.bld_date);
			string version = json.ver;

			if (UpdateTime < date)
			{
				Logger.Add(3, Resources.NewVersionFound + version);
			}
			else
			{
				Logger.Add(3, string.Format(SoftwareInformationResources.YouAreUsingTheLatestVersion,
					date.ToString("yyyy/MM/dd")));
			}
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, Resources.UpdateConnectionFailed);
		}
	}
}
