using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

namespace EOUpdater
{
	enum Language
	{
		Japanese,
		English
	}

	internal class Program
	{
		private static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver");
		private const string UpdateUrl = @"https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/update.json";

		private static Language Language { get; set; }

		private static void Main(string[] args)
		{
			Language = CultureInfo.CurrentCulture.Name switch
			{
				"ja-JP" => Language.Japanese,
				_ => Language.English
			};

			Console.Title = Language switch
			{
				Language.Japanese => "七四式アップデータ",
				_ => "EO updater"
			};

			var wait = true;
			var restart = false;

			foreach (var argument in args)
			{
				switch (argument)
				{
					case "--nowait":
						wait = false;
						break;
					case "--restart":
						restart = true;
						break;
					default:
						Console.WriteLine(argument + "is not a valid argument.");
						break;
				}
			}

			if (!Directory.Exists(AppDataFolder))
				Directory.CreateDirectory(AppDataFolder);

			try
			{
				var tempFile = AppDataFolder + @"\latest.zip";
				var destPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

				CheckUpdate(UpdateUrl, out var downloadUrl, out var downloadHash);
				DownloadUpdate(downloadUrl, tempFile, downloadHash);

				if (wait)
				{
					foreach (var process in Process.GetProcessesByName("ElectronicObserver"))
					{
						string s = Language switch
						{
							Language.Japanese => "七四式を閉じると、アップデート処理が開始されます。",
							_ => "Close Electronic Observer to start the updating process."
						};
						Console.WriteLine(s);
						process.WaitForExit();
					}
					foreach (var process in Process.GetProcessesByName("EOBrowser"))
					{
						string s = Language switch
						{
							Language.Japanese => "EOBrowserを閉じて、アップデート作業を開始します。",
							_ => "Close EOBrowser to start the updating process."
						};
						Console.WriteLine(s);
						process.WaitForExit();
					}
				}

				DeleteOldFiles();
				ExtractUpdate(tempFile, destPath);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			if (restart)
			{
				var appPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"ElectronicObserver.exe");
				Process.Start(appPath);
				return;
			}

			string st = Language switch
			{
				Language.Japanese => "アップデートが完了しました。このウィンドウを閉じることができます。",
				_ => "Update complete. You can close this window."
			};
			Console.WriteLine(st);
			Console.ReadKey();
		}

		private static void DeleteOldFiles()
		{
			string eoFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			IEnumerable<string> dllFiles = Directory.EnumerateFiles(eoFolderPath, "*.dll", SearchOption.TopDirectoryOnly);
			IEnumerable<string> configFiles = Directory.EnumerateFiles(eoFolderPath, "*.runtimeconfig.json", SearchOption.TopDirectoryOnly);

			foreach (string fileName in dllFiles.Concat(configFiles))
			{
				try
				{
					File.Delete(Path.Combine(eoFolderPath, fileName));
					// Console.WriteLine($"Deleted {fileName}");
				}
				catch
				{
					// Console.WriteLine($"Failed to delete {fileName}");
				}
			}
		}

		private static void ExtractUpdate(string zipPath, string extractPath)
		{
			var localPath = new Uri(extractPath).LocalPath;
			using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
			{
				string s = Language switch
				{
					Language.Japanese => "抽出開始...",
					_ => "Start extracting..."
				};
				Console.WriteLine(s);
				foreach (var file in archive.Entries)
				{
					var fullname = file.FullName.Replace(@"ElectronicObserver/", "");
					var completeFileName = Path.Combine(localPath, fullname);
					var directory = Path.GetDirectoryName(completeFileName);

					if (directory != null && !Directory.Exists(directory))
						Directory.CreateDirectory(directory);

					if (file.Name == "DynamicJson.dll" || file.Name == "EOUpdater.exe" || file.Name == "") continue;

					try
					{
						file.ExtractToFile(completeFileName, true);
					}
					catch (Exception)
					{
						Console.WriteLine($"Couldn't update {fullname}");
					}
				}
				s = Language switch
				{
					Language.Japanese => "抽出終了。",
					_ => "Extracting finished."
				};
				Console.WriteLine(s);
			}
		}

		private static string GetHash(string filename)
		{
			using (var sha256 = SHA256.Create())
			{
				using (var stream = File.OpenRead(filename))
				{
					var hash = sha256.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "");
				}
			}
		}

		private static void CheckUpdate(string url, out string downloadUrl, out string downloadHash)
		{
			using (var client = new WebClient())
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				var updateData = client.OpenRead(url);

				if (updateData == null) throw new Exception("Failed to download update data.");

				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Rootobject));
				Rootobject json = (Rootobject)serializer.ReadObject(updateData);

				string s = Language switch
				{
					Language.Japanese => "== 更新のデータ ==",
					_ => "== Update Data =="
				};
				Console.WriteLine(s);
				Console.WriteLine(json.bld_date);
				Console.WriteLine(json.ver);
				Console.WriteLine(json.note.Replace("<br>", "\r\n"));
				Console.WriteLine(json.url);
				s = Language switch
				{
					Language.Japanese => "ハッシュ",
					_ => "Hash"
				};
				Console.WriteLine($"{s}: {json.hash}");
				Console.WriteLine("==========\r\n");

				downloadUrl = json.url;
				downloadHash = json.hash;


			}
		}

		private static void DownloadUpdate(string downloadUrl, string tempFile, string downloadHash)
		{
			if (!File.Exists(tempFile) || GetHash(tempFile) != downloadHash)
				DownloadUpdate(downloadUrl, tempFile);

			string s = Language switch
			{
				Language.Japanese => "ファイル",
				_ => "File"
			};
			Console.WriteLine($"{s}: latest.zip SHA-256: {GetHash(tempFile)}");

			s = Language switch
			{
				Language.Japanese => "ダウンロード完了。",
				_ => "Download complete."
			};
			Console.WriteLine($"{s}\r\n");
		}

		private static void DownloadUpdate(string url, string tempFile)
		{
			try
			{
				using (var client = new WebClient())
				{
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
					string s = Language switch
					{
						Language.Japanese => "アップデートをダウンロード中...",
						_ => "Downloading update..."
					};
					Console.WriteLine(s);
					client.DownloadFile(url, tempFile);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}

		[DataContract]
		public class Rootobject
		{
			[DataMember]
			public string bld_date { get; set; }
			[DataMember]
			public string ver { get; set; }
			[DataMember]
			public string note { get; set; }
			[DataMember]
			public string url { get; set; }
			[DataMember]
			public string hash { get; set; }
			[DataMember]
			public string kancolle_mt { get; set; }
			[DataMember]
			public int event_state { get; set; }
		}
	}
}
