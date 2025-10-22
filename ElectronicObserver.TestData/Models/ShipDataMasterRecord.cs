using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.TestData.Models;

public class ShipDataMasterRecord
{
	public ShipId ShipId { get; set; }
	public int AlbumNo { get; set; }
	public int SortId { get; set; }
	public string Name { get; set; }
	public string NameReading { get; set; }
	public ShipTypes ShipType { get; set; }
	public int ShipClass { get; set; }
	public int RemodelAfterLevel { get; set; }
	public int RemodelAfterShipId { get; set; }
	public int RemodelBeforeShipId { get; set; }
	public int RemodelAmmo { get; set; }
	public int RemodelSteel { get; set; }
	public int NeedBlueprint { get; set; }
	public int NeedCatapult { get; set; }
	public int NeedActionReport { get; set; }
	public int NeedAviationMaterial { get; set; }
	public int NeedArmamentMaterial { get; set; }
	public int HpMin { get; set; }
	public int HpMax { get; set; }
	public int FirepowerMin { get; set; }
	public int FirepowerMax { get; set; }
	public int TorpedoMin { get; set; }
	public int TorpedoMax { get; set; }
	public int AaMin { get; set; }
	public int AaMax { get; set; }
	public int ArmorMin { get; set; }
	public int ArmorMax { get; set; }
	public int AswMin { get; set; }
	public int AswMax { get; set; }
	public int EvasionMin { get; set; }
	public int EvasionMax { get; set; }
	public int LosMin { get; set; }
	public int LosMax { get; set; }
	public int LuckMin { get; set; }
	public int LuckMax { get; set; }
	public int Speed { get; set; }
	public int Range { get; set; }
	public int SlotSize { get; set; }
	public int? Aircraft1 { get; set; }
	public int? Aircraft2 { get; set; }
	public int? Aircraft3 { get; set; }
	public int? Aircraft4 { get; set; }
	public int? Aircraft5 { get; set; }
	public int? DefaultSlot1 { get; set; }
	public int? DefaultSlot2 { get; set; }
	public int? DefaultSlot3 { get; set; }
	public int? DefaultSlot4 { get; set; }
	public int? DefaultSlot5 { get; set; }
	public IEnumerable<EquipmentTypes> EquippableCategories { get; set; }
	public int BuildingTime { get; set; }
	public IList<int> Material { get; set; }
	public IList<int> PowerUp { get; set; }
	public int Rarity { get; set; }
	public string MessageGet { get; set; }
	public string MessageAlbum { get; set; }
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int VoiceFlag { get; set; }
	[JsonIgnore] public string ResourceName { get; set; }
	[JsonIgnore] public string? ResourceGraphicVersion { get; set; }
	[JsonIgnore] public string? ResourceVoiceVersion { get; set; }
	[JsonIgnore] public string? ResourcePortVoiceVersion { get; set; }
	public int OriginalCostumeShipID { get; set; }

	public ShipDataMasterRecord()
	{

	}

	public ShipDataMasterRecord(IShipDataMaster ship)
	{
		ShipId = (ShipId)ship.ShipID;
		AlbumNo = ship.AlbumNo;
		SortId = ship.SortID;
		Name = ship.Name;
		NameReading = ship.NameReading;
		ShipType = ship.ShipType;
		ShipClass = ship.ShipClass;
		RemodelAfterLevel = ship.RemodelAfterLevel;
		RemodelAfterShipId = ship.RemodelAfterShipID;
		RemodelBeforeShipId = ship.RemodelBeforeShipID;
		RemodelAmmo = ship.RemodelAmmo;
		RemodelSteel = ship.RemodelSteel;
		NeedBlueprint = ship.NeedBlueprint;
		NeedCatapult = ship.NeedCatapult;
		NeedActionReport = ship.NeedActionReport;
		NeedAviationMaterial = ship.NeedAviationMaterial;
		NeedArmamentMaterial = ship.NeedArmamentMaterial;
		HpMin = ship.HPMin;
		HpMax = ship.HPMax;
		FirepowerMin = ship.FirepowerMin;
		FirepowerMax = ship.FirepowerMax;
		TorpedoMin = ship.TorpedoMin;
		TorpedoMax = ship.TorpedoMax;
		AaMin = ship.AAMin;
		AaMax = ship.AAMax;
		ArmorMin = ship.ArmorMin;
		ArmorMax = ship.ArmorMax;
		AswMin = ship.ASW.Minimum;
		AswMax = ship.ASW.Maximum;
		EvasionMin = ship.Evasion.Minimum;
		EvasionMax = ship.Evasion.Maximum;
		LosMin = ship.LOS.Minimum;
		LosMax = ship.LOS.Maximum;
		LuckMin = ship.LuckMin;
		LuckMax = ship.LuckMax;
		Speed = ship.Speed;
		Range = ship.Range;
		SlotSize = ship.SlotSize;
		if (ship.Aircraft is not null)
		{
			Aircraft1 = ship.Aircraft[0];
			Aircraft2 = ship.Aircraft[1];
			Aircraft3 = ship.Aircraft[2];
			Aircraft4 = ship.Aircraft[3];
			Aircraft5 = ship.Aircraft[4];
		}
		if (ship.DefaultSlot is not null)
		{
			DefaultSlot1 = ship.DefaultSlot[0];
			DefaultSlot2 = ship.DefaultSlot[1];
			DefaultSlot3 = ship.DefaultSlot[2];
			DefaultSlot4 = ship.DefaultSlot[3];
			DefaultSlot5 = ship.DefaultSlot[4];
		}
		EquippableCategories = ship.EquippableCategoriesTyped;
		BuildingTime = ship.BuildingTime;
		Material = ship.Material;
		PowerUp = ship.PowerUp;
		Rarity = ship.Rarity;
		MessageGet = ship.MessageGet;
		MessageAlbum = ship.MessageAlbum;
		Fuel = ship.Fuel;
		Ammo = ship.Ammo;
		VoiceFlag = ship.VoiceFlag;
		ResourceName = ship.ResourceName;
		ResourceGraphicVersion = ship.ResourceGraphicVersion;
		ResourceVoiceVersion = ship.ResourceVoiceVersion;
		ResourcePortVoiceVersion = ship.ResourcePortVoiceVersion;
		OriginalCostumeShipID = ship.OriginalCostumeShipID;
	}

	public IShipDataMaster ToMasterShip() => new ShipDataMasterMock
	{
		ShipId = ShipId,
		AlbumNo = AlbumNo,
		SortID = SortId,
		Name = Name,
		NameReading = NameReading,
		ShipType = ShipType,
		ShipClass = ShipClass,
		RemodelAfterLevel = RemodelAfterLevel,
		RemodelAfterShipID = RemodelAfterShipId,
		RemodelBeforeShipID = RemodelBeforeShipId,
		RemodelAmmo = RemodelAmmo,
		RemodelSteel = RemodelSteel,
		NeedBlueprint = NeedBlueprint,
		NeedCatapult = NeedCatapult,
		NeedActionReport = NeedActionReport,
		NeedAviationMaterial = NeedAviationMaterial,
		NeedArmamentMaterial = NeedArmamentMaterial,
		HPMin = HpMin,
		HPMax = HpMax,
		FirepowerMin = FirepowerMin,
		FirepowerMax = FirepowerMax,
		TorpedoMin = TorpedoMin,
		TorpedoMax = TorpedoMax,
		AAMin = AaMin,
		AAMax = AaMax,
		ArmorMin = ArmorMin,
		ArmorMax = ArmorMax,
		ASW = new ParameterMock(AswMin, AswMax),
		Evasion = new ParameterMock(EvasionMin, EvasionMax),
		LOS = new ParameterMock(LosMin, LosMax),
		LuckMin = LuckMin,
		LuckMax = LuckMax,
		Speed = Speed,
		Range = Range,
		SlotSize = SlotSize,
		Aircraft = new List<int>
		{
			Aircraft1 ?? 0,
			Aircraft2 ?? 0,
			Aircraft3 ?? 0,
			Aircraft4 ?? 0,
			Aircraft5 ?? 0,
		},
		DefaultSlot = new List<int>
		{
			DefaultSlot1 ?? -1,
			DefaultSlot2 ?? -1,
			DefaultSlot3 ?? -1,
			DefaultSlot4 ?? -1,
			DefaultSlot5 ?? -1,
		},
		EquippableCategoriesTyped = EquippableCategories,
		BuildingTime = BuildingTime,
		Material = Material,
		PowerUp = PowerUp,
		Rarity = Rarity,
		MessageGet = MessageGet,
		MessageAlbum = MessageAlbum,
		Fuel = Fuel,
		Ammo = Ammo,
		VoiceFlag = VoiceFlag,
		ResourceName = ResourceName,
		ResourceGraphicVersion = ResourceGraphicVersion,
		ResourceVoiceVersion = ResourceVoiceVersion,
		ResourcePortVoiceVersion = ResourcePortVoiceVersion,
		OriginalCostumeShipID = OriginalCostumeShipID,
	};
}
