using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DynaJson;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public abstract class TranslationBase
{
	private int RetryCount { get; set; }
	private int MaxRetries => 1;


	public abstract void Initialize();

	// todo: use the generic version instead of this one for all translation files
	public dynamic? Load(string path)
	{
		try
		{
			using StreamReader sr = new StreamReader(path);
			{
				var json = JsonObject.Parse(sr.ReadToEnd());
				return json;
			}
		}
		catch (FileNotFoundException)
		{
			Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (DirectoryNotFoundException)
		{
			Logger.Add(3, GetType().Name + ": File does not exists.");
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, "Failed to load " + GetType().Name);
		}

		return null;
	}

	protected T? Load<T>(string path) where T : class
	{
		try
		{
			using StreamReader sr = new StreamReader(path);

			return JsonSerializer.Deserialize<T>(sr.ReadToEnd());
		}
		catch (FileNotFoundException)
		{
			string fileName = Path.GetFileName(path);

			if (RetryCount >= MaxRetries)
			{
				Logger.Add(3, fileName + ": File does not exists.");
				return null;
			}

			RetryCount++;

			DataType type = path.Contains(DataAndTranslationManager.DataFolder) switch
			{
				true => DataType.Data,
				_ => DataType.Translation,
			};

			Task.Run(async () =>
			{
				await SoftwareUpdater.DownloadData(fileName, type);
				Initialize();
			});
		}
		catch (DirectoryNotFoundException)
		{
			Logger.Add(3, Path.GetFileName(path) + ": File does not exists.");
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, "Failed to load " + Path.GetFileName(path));
		}

		return null;
	}
}
