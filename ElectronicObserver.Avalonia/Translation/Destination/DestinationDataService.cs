using System.Text.Json;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.Translation.Destination;

public sealed class DestinationDataService : DataServiceBase
{
	protected override string FileName => "destination.json";
	protected override DataType DataType => DataType.Data;

	public IDDictionary<Destination> DestinationList { get; private set; } = [];

	public DestinationDataService(
		IConfigurationUi configurationUi,
		ISoftwareUpdaterService softwareUpdaterService,
		IEoLogger logger) : base(configurationUi, softwareUpdaterService, logger)
	{
		_ = Initialize();
	}

	/// <inheritdoc />
	protected override async Task Initialize()
	{
		DestinationList = [];
		await LoadDictionary(FilePath);
	}

	private async Task LoadDictionary(string path)
	{
		JsonDocument? json = await Load<JsonDocument>(path);

		if (json is null) return;

		foreach (JsonProperty property in json.RootElement.EnumerateObject())
		{
			if (property.NameEquals("version")) continue;

			foreach (JsonProperty stepProp in property.Value.EnumerateObject())
			{
				string[] world = property.Name[6..].Split('-');
				int mapAreaId = int.Parse(world[0]);
				int mapInfoId = int.Parse(world[1]);
				int destination = int.Parse(stepProp.Name);
				string previousCellDisplay = stepProp.Value[0].GetString()!;
				string cellDisplay = stepProp.Value[1].GetString()!;

				int hash = GetHashCode(mapAreaId, mapInfoId, destination);

				Destination item = new()
				{
					ID = hash,
					MapAreaId = mapAreaId,
					MapInfoId = mapInfoId,
					CellId = destination,
					PreviousCellDisplay = previousCellDisplay,
					CellDisplay = cellDisplay,
				};

				DestinationList.Add(item);
			}
		}
	}

	private Destination? DestinationOrDefault(int mapAreaId, int mapInfoId, int destination)
	{
		if (ConfigurationUi.UseOriginalNodeId) return null;

		int hash = GetHashCode(mapAreaId, mapInfoId, destination);
		DestinationList.TryGetValue(hash, out Destination? value);

		return value;
	}

	/// <summary>
	/// Returns cell display if the display is known, otherwise just the id.
	/// </summary>
	public string CellDisplay(int mapAreaId, int mapInfoId, int cellId) => 
		DestinationOrDefault(mapAreaId, mapInfoId, cellId)?.CellDisplay ?? cellId.ToString();

	/// <summary>
	/// Returns cell display with id if the display is known, otherwise just the id.
	/// </summary>
	public string CellDisplayWithId(int mapAreaId, int mapInfoId, int cellId)
	{
		Destination? destination = DestinationOrDefault(mapAreaId, mapInfoId, cellId);

		return destination switch
		{
			not null => $"{destination.CellDisplay} ({cellId})",
			_ => cellId.ToString(),
		};
	}

	public string PreviousCellDisplay(int mapAreaId, int mapInfoId, int cellId) =>
		DestinationOrDefault(mapAreaId, mapInfoId, cellId)?.PreviousCellDisplay ?? cellId.ToString();

	private static int GetHashCode(int mapAreaId, int mapInfoId, int cellId)
	{
		int hash = mapAreaId;
		hash = hash * 397 ^ mapInfoId;
		hash = hash * 397 ^ cellId;
		return hash;
	}
}
