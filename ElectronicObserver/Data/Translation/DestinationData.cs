using System.Collections.Generic;
using DynaJson;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public sealed class DestinationData : TranslationBase
{
	private static string DefaultFilePath => DataAndTranslationManager.DataFolder + @"\destination.json";

	private static bool IsLoaded => !Configuration.Config.UI.UseOriginalNodeId;

	public IDDictionary<Destination> DestinationList { get; private set; } = new();

	private Destination? DestinationOrDefault(int mapAreaId, int mapInfoId, int destination)
	{
		if (!IsLoaded) return null;

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

	public override void Initialize()
	{
		DestinationList = LoadDictionary(DefaultFilePath);
	}

	public DestinationData()
	{
		Initialize();
	}

	private IDDictionary<Destination> LoadDictionary(string path)
	{
		IDDictionary<Destination> dict = new();

		dynamic? json = Load(path);

		if (json is null) return dict;

		foreach (KeyValuePair<string, object> map in json)
		{
			if (map.Key is "version") continue;

			dynamic? destinations = JsonObject.Parse(map.Value.ToString());

			foreach (KeyValuePair<string, dynamic> dest in destinations)
			{
				string[] world = map.Key.Remove(0, 6).Split('-');
				int mapAreaId = int.Parse(world[0]);
				int mapInfoId = int.Parse(world[1]);
				int destination = int.Parse(dest.Key);
				string previousCellDisplay = dest.Value[0];
				string cellDisplay = dest.Value[1];

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

				dict.Add(item);
			}
		}

		return dict;
	}
}

public class Destination : IIdentifiable
{
	public int ID { get; set; }
	public int MapAreaId { get; set; }
	public int MapInfoId { get; set; }
	public int CellId { get; set; }
	public string PreviousCellDisplay { get; set; }
	public string CellDisplay { get; set; }

	public string Display => $"{MapAreaId}-{MapInfoId}-{CellDisplay}";
}
