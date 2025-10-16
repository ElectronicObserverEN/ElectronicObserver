using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ElectronicObserver");
const string updateUrl = "https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/update.json";

Language language = CultureInfo.CurrentCulture.Name switch
{
	"ja-JP" => Language.Japanese,
	_ => Language.English
};

Console.Title = language switch
{
	Language.Japanese => "七四式アップデータ",
	_ => "EO updater"
};

bool wait = true;
bool restart = false;

foreach (string argument in args)
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

Directory.CreateDirectory(appDataFolder);

try
{
	string tempFile = appDataFolder + @"\latest.zip";
	string destPath = Path.GetDirectoryName(AppContext.BaseDirectory)!;

	(string downloadUrl, string downloadHash) = await CheckUpdate(updateUrl);
	await DownloadUpdate(downloadUrl, tempFile, downloadHash);

	if (wait)
	{
		foreach (Process process in Process.GetProcessesByName("ElectronicObserver"))
		{
			Console.WriteLine(language switch
			{
				Language.Japanese => "七四式を閉じると、アップデート処理が開始されます。",
				_ => "Close Electronic Observer to start the updating process."
			});
			process.WaitForExit();
		}
		foreach (Process process in Process.GetProcessesByName("EOBrowser"))
		{
			Console.WriteLine(language switch
			{
				Language.Japanese => "EOBrowserを閉じて、アップデート作業を開始します。",
				_ => "Close EOBrowser to start the updating process."
			});
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
	string appPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase!, "ElectronicObserver.exe");
	Process.Start(appPath);
	return;
}

string st = language switch
{
	Language.Japanese => "アップデートが完了しました。このウィンドウを閉じることができます。",
	_ => "Update complete. You can close this window."
};

Console.WriteLine(st);
Console.ReadKey();

return;

static void DeleteOldFiles()
{
	string eoFolderPath = Path.GetDirectoryName(AppContext.BaseDirectory)!;

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

void ExtractUpdate(string zipPath, string extractPath)
{
	string localPath = new Uri(extractPath).LocalPath;
	using ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Read);

	Console.WriteLine(language switch
	{
		Language.Japanese => "抽出開始...",
		_ => "Start extracting..."
	});

	foreach (ZipArchiveEntry file in archive.Entries)
	{
		string fullname = file.FullName.Replace("ElectronicObserver/", "");
		string completeFileName = Path.Combine(localPath, fullname);
		string? directory = Path.GetDirectoryName(completeFileName);

		if (directory != null && !Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}

		if (file.Name is "DynamicJson.dll" or "EOUpdater.exe" or "") continue;

		try
		{
			file.ExtractToFile(completeFileName, true);
		}
		catch (Exception)
		{
			Console.WriteLine($"Couldn't update {fullname}");
		}
	}

	Console.WriteLine(language switch
	{
		Language.Japanese => "抽出終了。",
		_ => "Extracting finished."
	});
}

static string GetHash(string filename)
{
	using SHA256 sha256 = SHA256.Create();
	using FileStream stream = File.OpenRead(filename);

	byte[] hash = sha256.ComputeHash(stream);
	return BitConverter.ToString(hash).Replace("-", "");
}

async Task<(string downloadUrl, string downloadHash)> CheckUpdate(string url)
{
	using HttpClient client = new();

	UpdateData json = await client.GetFromJsonAsync(url, SourceGenerationContext.Default.UpdateData)
		?? throw new Exception("Failed to download update data.");

	Console.WriteLine(language switch
	{
		Language.Japanese => "== 更新のデータ ==",
		_ => "== Update Data =="
	});
	Console.WriteLine(json.BuildDate);
	Console.WriteLine(json.Version);
	Console.WriteLine(json.Note.Replace("<br>", "\r\n"));
	Console.WriteLine(json.Url);
	string hash = language switch
	{
		Language.Japanese => "ハッシュ",
		_ => "Hash"
	};
	Console.WriteLine($"{hash}: {json.Hash}");
	Console.WriteLine("==========\r\n");

	return (json.Url, json.Hash);
}

async Task DownloadUpdate(string downloadUrl, string tempFile, string downloadHash)
{
	if (!File.Exists(tempFile) || GetHash(tempFile) != downloadHash)
	{
		await DownloadUpdate2(downloadUrl, tempFile);
	}

	string s = language switch
	{
		Language.Japanese => "ファイル",
		_ => "File"
	};
	Console.WriteLine($"{s}: latest.zip SHA-256: {GetHash(tempFile)}");

	s = language switch
	{
		Language.Japanese => "ダウンロード完了。",
		_ => "Download complete."
	};
	Console.WriteLine($"{s}\r\n");
}

async Task DownloadUpdate2(string url, string tempFile)
{
	try
	{
		using HttpClient client = new();

		Console.WriteLine(language switch
		{
			Language.Japanese => "アップデートをダウンロード中...",
			_ => "Downloading update..."
		});

		await using Stream stream = await client.GetStreamAsync(url);
		await using FileStream fs = new(tempFile, FileMode.Create);

		await stream.CopyToAsync(fs);
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
	}

}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(UpdateData))]
internal partial class SourceGenerationContext : JsonSerializerContext;

public class UpdateData
{
	[JsonPropertyName("bld_date")]
	public required string BuildDate { get; init; }

	[JsonPropertyName("ver")]
	public required string Version { get; init; }

	[JsonPropertyName("note")]
	public required string Note { get; init; }

	[JsonPropertyName("url")]
	public required string Url { get; init; }

	[JsonPropertyName("hash")]
	public required string Hash { get; init; }
}

enum Language
{
	Japanese,
	English
}
