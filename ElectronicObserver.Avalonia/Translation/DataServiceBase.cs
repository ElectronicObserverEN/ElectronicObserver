using System.Text.Json;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation;

public abstract class DataServiceBase(
	IConfigurationUi configurationUi,
	ISoftwareUpdaterService softwareUpdaterService,
	IEoLogger logger)
{
	protected IConfigurationUi ConfigurationUi { get; } = configurationUi;
	private ISoftwareUpdaterService SoftwareUpdaterService { get; } = softwareUpdaterService;
	private IEoLogger Logger { get; } = logger;

	private string CurrentTranslationLanguage => ConfigurationUi.Culture switch
	{
		// Japanese translations don't exist, so fall back to English
		"ja-JP" => "en-US",
		string culture => culture,
	};

	private int RetryCount { get; set; }
	private static int MaxRetries => 1;

	protected string FilePath => DataType switch
	{
		DataType.Translation => Path.Join(DataConstants.TranslationFolder(CurrentTranslationLanguage), FileName),
		DataType.Data => Path.Join(DataConstants.DataFolder, FileName),
		
		_ => FileName,
	};
	
	protected abstract string FileName { get; }
	protected abstract DataType DataType { get; }
	public abstract Task Initialize();

	protected async Task<T?> Load<T>(string path) where T : class
	{
		try
		{
			await using Stream sr = File.OpenRead(path);

			return await JsonSerializer.DeserializeAsync<T>(sr);
		}
		catch (FileNotFoundException)
		{
			string fileName = Path.GetFileName(path);

			if (RetryCount >= MaxRetries)
			{
				Logger.Add(3, $"{fileName}: File does not exists.");
				return null;
			}

			RetryCount++;

			DataType type = path.Contains(DataConstants.DataFolder) switch
			{
				true => DataType.Data,
				_ => DataType.Translation,
			};

			_ = Task.Run(async () =>
			{
				await SoftwareUpdaterService.DownloadData(fileName, type);
				await Initialize();
			});
		}
		catch (DirectoryNotFoundException)
		{
			Logger.Add(3, $"{Path.GetFileName(path)}: File does not exists.");
		}
		catch (Exception ex)
		{
			Logger.SendErrorReport(ex, $"Failed to load {Path.GetFileName(path)}");
		}

		return null;
	}
}
