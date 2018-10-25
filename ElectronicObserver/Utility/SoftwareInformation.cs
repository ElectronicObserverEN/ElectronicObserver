using ElectronicObserver.Properties;
using ElectronicObserver.Utility.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codeplex.Data;

namespace ElectronicObserver.Utility
{

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
		public static string VersionJapanese => SoftwareNameJapanese + "四〇型改二";


		/// <summary>
		/// バージョン(英語)
		/// </summary>
		public static string VersionEnglish => "4.0.2";



		/// <summary>
		/// 更新日時
		/// </summary>
		public static DateTime UpdateTime => DateTimeHelper.CSVStringToTime("2018/09/17 10:00:00");




		private static System.Net.WebClient client;
		private static readonly Uri uri =
			new Uri("http://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json");

		public static void CheckUpdate()
		{

			if (!Utility.Configuration.Config.Life.CheckUpdateInformation)
				return;

			if (client == null)
			{
				client = new System.Net.WebClient
				{
					Encoding = new System.Text.UTF8Encoding(false)
				};
				client.DownloadStringCompleted += DownloadStringCompleted;
			}

			if (!client.IsBusy)
				client.DownloadStringAsync(uri);
		}

		private static void DownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
		{

			if (e.Error != null)
			{

				Utility.ErrorReporter.SendErrorReport( e.Error, "Failed to obtain update data." );
				return;

			}

			if (e.Result.StartsWith("<!DOCTYPE html>"))
			{

				Utility.Logger.Add( 3, "Invalid update URL." );
				return;

			}


			try
			{
				var json = DynamicJson.Parse(e.Result);
				DateTime date = DateTimeHelper.CSVStringToTime(json.bld_date);
				string version = json.ver;
				string description = json.note.Replace("<br>", "\r\n");

				if (UpdateTime < date)
				{

					Utility.Logger.Add(3, Resources.NewVersionFound + version);
					SoftwareUpdater.UpdateSoftware();

					var result = System.Windows.Forms.MessageBox.Show(
						string.Format(Resources.AskForUpdate,
							version, description),
						Resources.Update, System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information,
						System.Windows.Forms.MessageBoxDefaultButton.Button1);


					if (result == System.Windows.Forms.DialogResult.Yes)
					{

						System.Diagnostics.Process.Start("https://github.com/silfumus/ElectronicObserver/releases/latest");

					}
					else if (result == System.Windows.Forms.DialogResult.Cancel)
					{

						Utility.Configuration.Config.Life.CheckUpdateInformation = false;

					}

				}
				else
				{

					Utility.Logger.Add(2, "You are currently using the latest version (" + date.ToString("yyyy/MM/dd") + " release).");

				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport( ex, Resources.UpdateConnectionFailed );
			}

		}

	}

}
