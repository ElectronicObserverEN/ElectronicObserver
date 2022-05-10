using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using DynaJson;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public class DestinationData : TranslationBase
{
	private string DefaultFilePath = DataAndTranslationManager.DataFolder + @"\destination.json";

	private IDDictionary<Destinations> DestinationList;

	/// <summary>
	/// Destination Node Letter ID (e.g: 1-1-A)
	/// </summary>
	public string DisplayID(int mapAreaID, int mapInfoID, int destination)
	{
		int hash = GetHashCode(mapAreaID, mapInfoID, destination);
		return IsLoaded && DestinationList.TryGetValue(hash, out Destinations value) ? value.DestinationDisplayID : destination.ToString();
	}

	public string PreviousID(int mapAreaID, int mapInfoID, int destination)
	{
		int hash = GetHashCode(mapAreaID, mapInfoID, destination);
		return IsLoaded && DestinationList.TryGetValue(hash, out Destinations value) ? value.PreviousDisplayID : "";
	}

	private bool IsLoaded => Configuration.Config.UI.UseOriginalNodeId == false && DestinationList != null;

	private int GetHashCode(int mapAreaID, int mapInfoID, int destination)
	{
		int hash = mapAreaID;
		hash = hash * 397 ^ mapInfoID;
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

	private IDDictionary<Destinations> LoadDictionary(string path)
	{
		var dict = new IDDictionary<Destinations>();

		var json = Load(path);
		if (json == null) return dict;

		foreach (KeyValuePair<string, object> map in json)
		{
			if (map.Key == "version") continue;
			var destinations = JsonObject.Parse(map.Value.ToString());
			foreach (KeyValuePair<string, dynamic> dest in destinations)
			{
				string[] world = map.Key.Remove(0, 6).Split('-');
				int mapAreaID = int.Parse(world[0]);
				int mapInfoID = int.Parse(world[1]);
				int destination = int.Parse(dest.Key);
				string previousDisplayID = dest.Value[0];
				string destinationDisplayID = dest.Value[1];

				int hash = GetHashCode(mapAreaID, mapInfoID, destination);

				var item = new Destinations
				{
					ID = hash,
					MapAreaID = mapAreaID,
					MapInfoID = mapInfoID,
					Destination = destination,
					PreviousDisplayID = previousDisplayID,
					DestinationDisplayID = destinationDisplayID
				};

				dict.Add(item);
			}
		}
		return dict;
	}

	private class Destinations : IIdentifiable
	{
		public int ID { get; set; }
		public int MapAreaID { get; set; }
		public int MapInfoID { get; set; }
		public int Destination { get; set; }
		public string PreviousDisplayID { get; set; }
		public string DestinationDisplayID { get; set; }
	}
}
