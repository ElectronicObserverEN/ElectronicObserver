using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Translation.Lock;

public sealed class LockDataService : DataServiceBase
{
	protected override string FileName => "Locks.json";
	protected override DataType DataType => DataType.Translation;

	private Dictionary<string, string> LockList { get; set; } = [];

	public LockDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		LockList = await Load<Dictionary<string, string>>(path) ?? [];
	}

	public string Lock(string name) => ConfigurationUi.DisableOtherTranslations switch
	{
		true => name,
		_ => LockList.GetValueOrDefault(name, name),
	};
}
