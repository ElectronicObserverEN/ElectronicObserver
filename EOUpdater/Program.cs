using Codeplex.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;

namespace EOUpdater
{
	internal class Program
	{
		private static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver");
	    private const string UpdateUrl = @"https://raw.githubusercontent.com/silfumus/ryuukitsune.github.io/master/Translations/en-US/update.json";

	    private static void Main(string[] args)
		{
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
			            Console.WriteLine("Close Electronic Observer to start the updating process.");
			            process.WaitForExit();
		            }
		            foreach (var process in Process.GetProcessesByName("EOBrowser"))
		            {
			            Console.WriteLine("Close EOBrowser to start the updating process.");
			            process.WaitForExit();
		            }
	            }

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

			Console.WriteLine("Update complete. You can close this window.");
			Console.ReadKey();
		}

		private static void ExtractUpdate(string zipPath, string extractPath)
		{
			var localPath = new Uri(extractPath).LocalPath;
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Read))
			{
				Console.WriteLine("Start extracting...");
				foreach (var file in archive.Entries)
				{
                    var fullname = file.FullName.Replace(@"ElectronicObserver/", "");
					var completeFileName = Path.Combine(localPath, fullname);
					var directory = Path.GetDirectoryName(completeFileName);

					if (directory != null && !Directory.Exists(directory))
						Directory.CreateDirectory(directory);

					if (file.Name == "DynamicJson.dll" || file.Name == "EOUpdater.exe" || file.Name == "") continue;
					file.ExtractToFile(completeFileName, true);
				}
				Console.WriteLine("Extracting finished.");
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
				var json = DynamicJson.Parse(updateData);

				Console.WriteLine("== Update Data==");
				Console.WriteLine(json.bld_date);
				Console.WriteLine(json.ver);
				Console.WriteLine(json.note.Replace("<br>", "\r\n"));
				Console.WriteLine(json.url);
				Console.WriteLine("Hash: " + json.hash);
				Console.WriteLine("==========\r\n");

			    downloadUrl = json.url;
			    downloadHash = json.hash;
			}
		}

		private static void DownloadUpdate(string downloadUrl, string tempFile, string downloadHash)
		{
			if (!File.Exists(tempFile) || GetHash(tempFile) != downloadHash)
                DownloadUpdate(downloadUrl, tempFile);
		    Console.WriteLine("File: latest.zip SHA-256: " + GetHash(tempFile));
			Console.WriteLine("Download complete.\r\n");
		}

		private static void DownloadUpdate(string url, string tempFile)
		{
			try
			{
				using (var client = new WebClient())
				{
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
					Console.WriteLine("Downloading update...");
					client.DownloadFile(url, tempFile);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}
	}
}
