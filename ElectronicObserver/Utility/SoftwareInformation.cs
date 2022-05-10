using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynaJson;
using ElectronicObserver.Properties;
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
			ErrorReporter.SendErrorReport(e.Error, Properties.Utility.SoftwareInformation.FailedToObtainUpdateData);
			return;
		}

		if (e.Result.StartsWith("<!DOCTYPE html>"))
		{
			Logger.Add(3, Properties.Utility.SoftwareInformation.InvalidUpdateUrl);
			return;
		}

		try
		{
			var json = JsonObject.Parse(e.Result);
			DateTime date = DateTimeHelper.CSVStringToTime(json.bld_date);
			string version = json.ver;
			string description = json.note.Replace("<br>", "\r\n");

			if (UpdateTime < date)
			{
				Logger.Add(3, Resources.NewVersionFound + version);
				Task.Run(() => SoftwareUpdater.UpdateSoftware());

				var result = System.Windows.Forms.MessageBox.Show(
					string.Format(Resources.AskForUpdate, version, description),
					Resources.Update, System.Windows.Forms.MessageBoxButtons.YesNoCancel,
					System.Windows.Forms.MessageBoxIcon.Information,
					System.Windows.Forms.MessageBoxDefaultButton.Button1);


				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					ProcessStartInfo psi = new ProcessStartInfo
					{
						FileName = "https://github.com/gre4bee/ElectronicObserver/releases/latest",
						UseShellExecute = true
					};
					Process.Start(psi);
				}
				else if (result == System.Windows.Forms.DialogResult.Cancel)
				{
					Configuration.Config.Life.CheckUpdateInformation = false;
				}
			}
			else
			{
				Logger.Add(3, string.Format(Properties.Utility.SoftwareInformation.YouAreUsingTheLatestVersion,
					date.ToString("yyyy/MM/dd")));
			}
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, Resources.UpdateConnectionFailed);
		}
	}
}
