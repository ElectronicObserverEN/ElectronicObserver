using System.Collections.Generic;
using System.Drawing;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Mocks;

public class ShipDataMasterMock : IShipDataMaster
{
	public int ShipID => (int)ShipId;
	public ShipId ShipId { get; set; }
	public int AlbumNo { get; set; }
	public int SortID { get; set; }
	public string Name { get; set; }
	public string NameEN => Name;
	public string NameReading { get; set; }
	public string NameReadingEN { get; set; }
	public ShipTypes ShipType { get; set; }
	public int ShipClass { get; set; }
	public ShipClass ShipClassTyped => (ShipClass)ShipClass;
	public int RemodelAfterLevel { get; set; }
	public int RemodelAfterShipID { get; set; }
	public IShipDataMaster? RemodelAfterShip { get; set; }
	public int RemodelBeforeShipID { get; set; }
	public IShipDataMaster? RemodelBeforeShip { get; set; }
	public int RemodelAmmo { get; set; }
	public int RemodelSteel { get; set; }
	public int NeedBlueprint { get; set; }
	public int NeedCatapult { get; set; }
	public int NeedActionReport { get; set; }
	public int NeedAviationMaterial { get; set; }
	public int NeedArmamentMaterial { get; set; }
	public int HPMin { get; set; }
	public int HPMax { get; set; }
	public int FirepowerMin { get; set; }
	public int FirepowerMax { get; set; }
	public int TorpedoMin { get; set; }
	public int TorpedoMax { get; set; }
	public int AAMin { get; set; }
	public int AAMax { get; set; }
	public int ArmorMin { get; set; }
	public int ArmorMax { get; set; }
	public IParameter ASW { get; set; }
	public IParameter Evasion { get; set; }
	public IParameter LOS { get; set; }
	public int LuckMin { get; set; }
	public int LuckMax { get; set; }
	public int Speed { get; set; }
	public int Range { get; set; }
	public int SlotSize { get; set; }
	public IList<int> Aircraft { get; set; }
	public int AircraftTotal { get; set; }
	public IList<int>? DefaultSlot { get; set; }
	public IEnumerable<int>? SpecialEquippableCategories { get; set; }
	public IEnumerable<int> EquippableCategories { get; }
	public IEnumerable<EquipmentTypes> EquippableCategoriesTyped { get; set; }
	public int BuildingTime { get; set; }
	public IList<int> Material { get; set; }
	public IList<int> PowerUp { get; set; }
	public int Rarity { get; set; }
	public string MessageGet { get; set; }
	public string MessageAlbum { get; set; }
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int VoiceFlag { get; set; }
	public IShipGraphicData GraphicData { get; set; }
	public string ResourceName { get; set; }
	public string? ResourceGraphicVersion { get; set; }
	public string? ResourceVoiceVersion { get; set; }
	public string? ResourcePortVoiceVersion { get; set; }
	public int OriginalCostumeShipID { get; set; }
	public int HPMaxMarried => this.HpMaxMarried();
	public int HPMaxModernizable => this.HpMaxModernizable();
	public int HPMaxMarriedModernizable => this.HpMaxMarriedModernizable();
	public int HPMaxModernized => this.HpMaxModernized();
	public int HPMaxMarriedModernized => this.HpMaxMarriedModernized();
	public int ASWModernizable => this.AswModernizable();

	public bool IsAbyssalShip => this.IsAbyssalShip();
	public string NameWithClass => (!IsAbyssalShip || NameReading == "" || NameReading == "-") switch
	{
		true => NameEN,
		_ => $"{NameEN} {NameReading}",
	};
	public IShipType ShipTypeInstance { get; set; }
	public bool IsLandBase { get; set; }
	public bool IsListedInAlbum { get; set; }
	public int RemodelTier { get; set; }
	public RemodelTier RemodelTierTyped { get; set; }
	public string ShipTypeName { get; set; }
	public bool IsSubmarine => ShipType is ShipTypes.Submarine or ShipTypes.SubmarineAircraftCarrier;
	public bool IsAircraftCarrier => this.IsAircraftCarrier();
	public bool IsRegularCarrier { get; set; }
	public bool IsEscortAircraftCarrier { get; set; }
	public bool IsPt => this.IsPt();
	public int ID => ShipID;

	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }

	public Color GetShipNameColor()
	{
		throw new System.NotImplementedException();
	}

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}

	public IShipDataMaster BaseShip() => ShipDataExtensions.BaseShip(this);
}
