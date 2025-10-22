using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.TestData.Wiki;

public static class WikiDataParser
{
	public static Dictionary<ShipId, IShipDataMaster> Ships(List<IEquipmentDataMaster> equipment)
	{
		const string link = "https://raw.githubusercontent.com/kcwiki/kancolle-data/master/wiki/ship.json";

		HttpClient client = new();
		string data = client.GetStringAsync(link).Result;

		Dictionary<string, WikiShip> wikiShips = JsonSerializer.Deserialize<Dictionary<string, WikiShip>>(data) ?? new();

		List<ShipDataMasterMock> ships = wikiShips.Values.Select(x => x.ToMasterShip()).ToList();

		void SetRemodels(ShipDataMasterMock ship, WikiShip wikiShip)
		{
			SetRemodelBefore(ship, wikiShip);
			SetRemodelAfter(ship, wikiShip);
		}

		void SetRemodelBefore(ShipDataMasterMock ship, WikiShip wikiShip)
		{
			string? remodelBeforeShipName = wikiShip._remodel_from switch
			{
				{ ValueKind: JsonValueKind.String } s => s.GetString(),
				_ => null,
			};

			if (remodelBeforeShipName is null) return;

			ship.RemodelBeforeShip = FindShip(remodelBeforeShipName);
			ship.RemodelBeforeShipID = ship.RemodelBeforeShip?.ShipID ?? -1;
		}

		void SetRemodelAfter(ShipDataMasterMock ship, WikiShip wikiShip)
		{
			string? remodelAfterShipName = wikiShip._remodel_to switch
			{
				{ ValueKind: JsonValueKind.String } s => s.GetString(),
				_ => null,
			};

			if (remodelAfterShipName is null) return;

			ship.RemodelAfterShip = FindShip(remodelAfterShipName);
			ship.RemodelAfterShipID = ship.RemodelAfterShip?.ShipID ?? -1;
		}

		IShipDataMaster? FindShip(string remodelName)
		{
			string[] nameAndSuffix = remodelName.Split("/");
			string name = nameAndSuffix[0];
			string suffix = nameAndSuffix.Skip(1).FirstOrDefault("");

			string key = suffix switch
			{
				"" => name,
				_ => $"{name} {suffix}"
			};

			wikiShips.TryGetValue(key, out WikiShip? nextRemodelShip);

			if (nextRemodelShip is null) throw new Exception();

			return ships.FirstOrDefault(s => s.ShipID == nextRemodelShip._api_id);
		}

		foreach (ShipDataMasterMock ship in ships)
		{
			SetRemodels(ship, wikiShips.Values.First(s => s._api_id == ship.ShipID));
		}

		foreach (WikiShip ship in wikiShips.Values)
		{
			// if any equip is null, set the whole default equip as unknown
			if (ship._equipment.Any(s => s.equipment is { ValueKind: JsonValueKind.Null }))
			{
				ships.First(s => s.ShipID == ship._api_id).DefaultSlot = null;
				continue;
			}

			try
			{
				ships.First(s => s.ShipID == ship._api_id).DefaultSlot = ship._equipment
					.Select(s => s.equipment switch
					{
						{ ValueKind: JsonValueKind.String } n => n.GetString().Replace("_", " "),
						_ => null
					})
					.Select(n => n switch
					{
						// Kinu kai
						"12.7cm Twin High-angle Gun Mount" when ship._api_id is 289 => 229,
						string s => equipment.First(e => e.NameEN == s).EquipmentID,
						_ => -1
					})
					.Concat(Enumerable.Repeat(-1, 5))
					.Take(5)
					.ToList();
			}
			catch (Exception ex)
			{

			}
		}

		return ships
			.Cast<IShipDataMaster>()
			.OrderBy(s => s.ShipID)
			.ToDictionary(s => (ShipId)s.ShipID);
	}

	public static List<IEquipmentDataMaster> Equipment()
	{
		const string link = "https://raw.githubusercontent.com/kcwiki/kancolle-data/master/wiki/equipment.json";

		HttpClient client = new();
		string data = client.GetStringAsync(link).Result;

		Dictionary<string, WikiEquipment> wikiEquipment = JsonSerializer.Deserialize<Dictionary<string, WikiEquipment>>(data) ?? new();

		List<EquipmentDataMasterMock> masterEquipment = wikiEquipment.Values.Select(x => x.ToMasterEquipment()).ToList();

		return masterEquipment
			.Cast<IEquipmentDataMaster>()
			.ToList();
	}

	public static Dictionary<ShipId, IShipDataMaster> AbyssalShips(List<IEquipmentDataMaster> equipment)
	{
		const string link = "https://raw.githubusercontent.com/kcwiki/kancolle-data/master/wiki/enemy.json";

		HttpClient client = new();
		string data = client.GetStringAsync(link).Result;

		Dictionary<string, WikiAbyssalShip> wikiShips = JsonSerializer.Deserialize<Dictionary<string, WikiAbyssalShip>>(data) ?? new();

		List<ShipDataMasterMock> masterShips = wikiShips.Values.Select(x => x.ToMasterShip()).ToList();

		foreach (WikiAbyssalShip ship in wikiShips.Values)
		{
			// if any equip is null, set the whole default equip as unknown
			if (ship._equipment.Any(s => s.equipment is { ValueKind: JsonValueKind.Null }))
			{
				masterShips.First(s => s.ShipID == ship._api_id).DefaultSlot = null;
				continue;
			}

			masterShips.First(s => s.ShipID == ship._api_id).DefaultSlot = ship._equipment
				.Select(s => s.equipment switch
				{
					{ ValueKind: JsonValueKind.String } n => n.GetString(),
					_ => null
				})
				.Select(n => n switch
				{
					string s => equipment.First(e => e.NameEN == s).EquipmentID,
					_ => -1
				})
				.Concat(Enumerable.Repeat(-1, 5))
				.Take(5)
				// wiki made a fucking mess only updating some equipment ids
				.Select(i => i switch
				{
					-1 => -1,
					< 1500 => i + 1000,
					_ => i,
				})
				.ToList();
		}

		return masterShips
			.Cast<IShipDataMaster>()
			.OrderBy(s => s.ShipID)
			.ToDictionary(s => (ShipId)s.ShipID);
	}

	public static List<IEquipmentDataMaster> AbyssalEquipment()
	{
		const string link = "https://raw.githubusercontent.com/kcwiki/kancolle-data/master/wiki/enemyEquipment.json";

		HttpClient client = new();
		string data = client.GetStringAsync(link).Result;

		Dictionary<string, WikiEquipment> wikiEquipment = JsonSerializer.Deserialize<Dictionary<string, WikiEquipment>>(data) ?? new();

		List<EquipmentDataMasterMock> masterEquipment = wikiEquipment.Values.Select(x => x.ToMasterEquipment()).ToList();

		return masterEquipment
			.Cast<IEquipmentDataMaster>()
			.ToList();
	}
}
