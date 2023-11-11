using System.Collections.Generic;
using DynaJson;
using ElectronicObserver.Utility;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Data.Translation;

public sealed class DestinationData : TranslationBase
{
	private static string DefaultFilePath => DataAndTranslationManager.DataFolder + @"\destination.json";

	public IDDictionary<Destination> DestinationList { get; private set; } = new();

	/// <summary>
	/// Destination Node Letter ID (e.g: 1-1-A)
	/// </summary>
	public string DisplayId(int mapAreaId, int mapInfoId, int destination)
	{
		int hash = GetHashCode(mapAreaId, mapInfoId, destination);
		return IsLoaded && DestinationList.TryGetValue(hash, out Destination value) ? value.DestinationDisplayId : destination.ToString();
	}

	public string PreviousId(int mapAreaId, int mapInfoId, int destination)
	{
		int hash = GetHashCode(mapAreaId, mapInfoId, destination);
		return IsLoaded && DestinationList.TryGetValue(hash, out Destination value) ? value.PreviousDisplayId : "";
	}

	private static bool IsLoaded => !Configuration.Config.UI.UseOriginalNodeId;

	private static int GetHashCode(int mapAreaId, int mapInfoId, int destination)
	{
		int hash = mapAreaId;
		hash = hash * 397 ^ mapInfoId;
		hash = hash * 397 ^ destination;
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
				string previousDisplayId = dest.Value[0];
				string destinationDisplayId = dest.Value[1];

				int hash = GetHashCode(mapAreaId, mapInfoId, destination);

				Destination item = new()
				{
					ID = hash,
					MapAreaId = mapAreaId,
					MapInfoId = mapInfoId,
					CellId = destination,
					PreviousDisplayId = previousDisplayId,
					DestinationDisplayId = destinationDisplayId
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
	public string PreviousDisplayId { get; set; }
	public string DestinationDisplayId { get; set; }

	public string Display => $"{MapAreaId}-{MapInfoId}-{DestinationDisplayId}";
}
