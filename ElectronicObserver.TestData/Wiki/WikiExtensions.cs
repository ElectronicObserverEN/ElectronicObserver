using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.TestData.Wiki;

public static class WikiExtensions
{
	public static ShipDataMasterMock ToMasterShip(this WikiShip wikiShip) => new()
	{
		ShipId = wikiShip._japanese_name switch
		{
			"榛名改二乙" => (ShipId)(wikiShip._api_id = (int)ShipId.HarunaKaiNiB),
			_ => (ShipId)wikiShip._api_id,
		},
		AlbumNo = wikiShip._id,
		Name = wikiShip._japanese_name,
		NameReading = wikiShip._reading.ToStringValue(),
		HPMin = wikiShip._hp,
		HPMax = wikiShip._hp_max,
		ArmorMin = wikiShip._armor,
		ArmorMax = wikiShip._armor_max,
		FirepowerMin = wikiShip._firepower,
		FirepowerMax = wikiShip._firepower_max,
		TorpedoMin = wikiShip._torpedo.ToIntValue(),
		TorpedoMax = wikiShip._torpedo_max.ToIntValue(),
		AAMin = wikiShip._aa,
		AAMax = wikiShip._aa_max.ToIntValue(),
		ASW = (ShipId)wikiShip._api_id switch
		{
			_ => new ParameterMock(wikiShip._asw.ToIntValue(), wikiShip._asw_max.ToIntValue()),
		},
		Evasion = (ShipId)wikiShip._api_id switch
		{
			ShipId.MikumaKaiNiToku => new ParameterMock(35, 75),
			_ => new ParameterMock(wikiShip._evasion ?? -1, wikiShip._evasion_max ?? -1),
		},
		LOS = (ShipId)wikiShip._api_id switch
		{
			ShipId.MikumaKaiNiToku => new ParameterMock(32, 89),
			_ => new ParameterMock(wikiShip._los ?? -1, wikiShip._los_max ?? -1),
		},
		LuckMin = wikiShip._luck,
		LuckMax = wikiShip._luck_max,
		Speed = wikiShip._speed,
		Range = wikiShip._range,
		BuildingTime = wikiShip._build_time * 60 * 1000,
		Rarity = wikiShip._rarity switch
		{
			{ ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			_ => -1,
		},
		Fuel = wikiShip._fuel,
		Ammo = wikiShip._ammo,
		Aircraft = wikiShip._equipment.Select(s => s.size).Concat(Enumerable.Repeat<int>(default, 5)).Take(5).ToArray(),
		// class is the actual name of the nameship here, while master data has in ID
		ShipClass = wikiShip._class switch
		{
			_ => 0,
		},
		RemodelAfterLevel = wikiShip._remodel_level switch
		{
			{ ValueKind: JsonValueKind.Number } n => n.GetInt32(),
			// todo: think this should be -1
			_ => 0
		},
	};

	public static ShipDataMasterMock ToMasterShip(this WikiAbyssalShip wikiShip) => new()
	{
		ShipId = (ShipId)wikiShip._api_id,
		AlbumNo = wikiShip._id,
		Name = wikiShip._japanese_name,
		NameReading = wikiShip._reading,
		HPMin = wikiShip._hp ?? 0,
		HPMax = wikiShip._hp ?? 0,
		ArmorMin = wikiShip._armor ?? 0,
		ArmorMax = wikiShip._armor ?? 0,
		FirepowerMin = wikiShip._firepower ?? 0,
		FirepowerMax = wikiShip._firepower ?? 0,
		TorpedoMin = wikiShip._torpedo ?? 0,
		TorpedoMax = wikiShip._torpedo ?? 0,
		AAMin = wikiShip._aa ?? 0,
		AAMax = wikiShip._aa ?? 0,
		ASW = new ParameterMock
		{
			MinimumEstMin = wikiShip._asw ?? 0,
			MinimumEstMax = wikiShip._asw ?? 9999,
			Maximum = wikiShip._asw ?? 9999,
		},
		Evasion = new ParameterMock
		{
			MinimumEstMin = wikiShip._evasion ?? 0,
			MinimumEstMax = wikiShip._evasion ?? 9999,
			Maximum = wikiShip._evasion ?? 9999,
		},
		LOS = new ParameterMock
		{
			MinimumEstMin = wikiShip._los ?? 0,
			MinimumEstMax = wikiShip._los ?? 9999,
			Maximum = wikiShip._los ?? 9999,
		},
		LuckMin = wikiShip._luck ?? 0,
		LuckMax = wikiShip._luck ?? 0,
		Speed = wikiShip._speed ?? 0,
		Range = wikiShip._range ?? -1,
		Aircraft = wikiShip._equipment.Select(s => s.size.ToIntValueAircraft())
			.Concat(Enumerable.Repeat<int>(default, 5)).Take(5).ToArray(),
	};

	public static EquipmentDataMasterMock ToMasterEquipment(this WikiEquipment wikiEquipment) => new()
	{
		EquipmentID = wikiEquipment._id,
		Name = wikiEquipment._japanese_name,
		NameEN = wikiEquipment._name,
		AA = wikiEquipment._aa.ToIntValue(),
		Armor = wikiEquipment._armor.ToIntValue(),
		ASW = wikiEquipment._asw.ToIntValue(),
		Bomber = wikiEquipment._bombing.ToIntValue(),
		Evasion = wikiEquipment._evasion.ToIntValue(),
		Firepower = wikiEquipment._firepower.ToIntValue(),
		LOS = wikiEquipment._los.ToIntValue(),
		Luck = wikiEquipment._luck.ToIntValue(),
		Range = wikiEquipment._range.ToIntValue(),
		Accuracy = wikiEquipment._shelling_accuracy.ToIntValue(),
		Torpedo = wikiEquipment._torpedo.ToIntValue(),
	};

	public static int ToIntValue(this JsonElement value) => value switch
	{
		{ ValueKind: JsonValueKind.Number } n => n.GetInt32(),
		// no value? (ships that have no ASW)
		{ ValueKind: JsonValueKind.False } => 0,
		// unknown value?
		_ => -1
	};

	public static int ToIntValueAircraft(this JsonElement value) => value switch
	{
		{ ValueKind: JsonValueKind.Number } n => n.GetInt32(),
		_ => 0
	};

	public static string ToStringValue(this JsonElement value) => value switch
	{
		{ ValueKind: JsonValueKind.String } s => s.GetString()!,
		_ => "",
	};
}
