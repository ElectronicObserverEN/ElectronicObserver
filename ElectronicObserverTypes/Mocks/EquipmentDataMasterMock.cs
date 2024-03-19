using System;
using System.Collections.Generic;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Mocks;

public class EquipmentDataMasterMock : IEquipmentDataMaster
{
	private string? _nameEn;

	public bool IsSurfaceRadar => this.IsSurfaceRadar();
	public bool IsHighAccuracyRadar => this.IsHighAccuracyRadar();
	public bool IsSonar => this.IsSonar();
	public bool IsDepthCharge => this.IsDepthCharge();
	public bool IsDepthChargeProjector => this.IsDepthChargeProjector();
	public bool IsNightAviationPersonnel => this.IsNightAviationPersonnel();
	public bool IsHightAltitudeFighter => this.IsHightAltitudeFighter();
	public bool IsAARocketLauncher => this.IsAARocketLauncher();
	public int ID => EquipmentID;
	public EquipmentId EquipmentId => (EquipmentId)EquipmentID;
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }
	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}

	public int EquipmentID { get; set; }
	public int AlbumNo { get; set; }
	public string Name { get; set; }
	public string NameEN
	{
		get => _nameEn ?? Name;
		set => _nameEn = value;
	}
	public bool IsTranslated { get; set; }
	public IList<int> EquipmentType { get; set; }
	public int Armor { get; set; }
	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int Bomber { get; set; }
	public int AA { get; set; }
	public int ASW { get; set; }
	public int Accuracy { get; set; }
	public int Evasion { get; set; }
	public int LOS { get; set; }
	public int Luck { get; set; }
	public int Range { get; set; }
	public int Rarity { get; set; }
	public IList<int> Material { get; set; }
	public string Message { get; set; }
	public int AircraftCost { get; set; }
	public int AircraftDistance { get; set; }
	public bool IsAbyssalEquipment { get; set; }
	public bool IsListedInAlbum { get; set; }
	public int CardType { get; set; }
	public EquipmentTypes CategoryType { get; set; }
	public IEquipmentType CategoryTypeInstance { get; set; }
	public int IconType { get; set; }
	public EquipmentIconType IconTypeTyped => (EquipmentIconType)IconType;
	public IEnumerable<ShipId> EquippableShipsAtExpansion { get; set; } = Array.Empty<ShipId>();
	public IEnumerable<ShipTypes> EquippableShipTypesAtExpansion { get; set; } = Array.Empty<ShipTypes>();
	public IEnumerable<ShipClass> EquippableShipClassesAtExpansion { get; set; } = Array.Empty<ShipClass>();
	public bool IsGun => this.IsGun();
	public bool IsMainGun => this.IsMainGun();
	public bool IsSecondaryGun => this.IsSecondaryGun();
	public bool IsTorpedo => this.IsTorpedo();
	public bool IsLateModelTorpedo => this.IsLateModelTorpedo();
	public bool IsHighAngleGun => this.IsHighAngleGun();
	public bool IsHighAngleGunWithAADirector => this.IsHighAngleGunWithAADirector();
	public bool IsConcentratedAAGun => this.IsConcentratedAAGun();
	public bool IsAircraft => this.IsAircraft();
	public bool IsCombatAircraft => this.IsCombatAircraft();
	public bool IsReconAircraft => this.IsReconAircraft();
	public bool IsAntiSubmarineAircraft => this.IsAntiSubmarineAircraft();
	public bool IsNightAircraft => this.IsNightAircraft();
	public bool IsNightFighter => this.IsNightFighter();
	public bool IsNightAttacker => this.IsNightAttacker();
	public bool IsSwordfish => this.IsSwordfish();
	public bool IsRadar => this.IsRadar();
	public bool IsAirRadar => this.IsAirRadar();
}
