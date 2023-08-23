using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Mocks;

public class ShipDataMock : IShipData
{
	public int ExpNextRemodel { get; set; }
	public string Name => MasterShip.IsAbyssalShip switch
	{
		false => MasterShip.NameEN,
		_ => MasterShip.NameWithClass,
	};
	public string NameWithLevel => MasterShip.IsAbyssalShip switch
	{
		false => $"{MasterShip.NameEN} Lv. {Level}",
		_ => $"{MasterShip.NameWithClass} Lv. {Level}",
	};
	public double HPRate => (double)HPCurrent / HPMax;
	public int FuelMax { get; set; }
	public int AmmoMax { get; set; }
	public double FuelRate => (double)Fuel / FuelMax;
	public double AmmoRate => (double)Ammo / AmmoMax;
	public int SupplyFuel { get; set; }
	public int SupplyAmmo { get; set; }
	public IList<double> AircraftRate { get; set; }
	public double AircraftTotalRate { get; set; }
	public bool IsExpansionSlotAvailable { get; set; }
	public int AirBattlePower { get; set; }
	public IList<int> AirBattlePowers { get; set; }
	public int ShellingPower { get; set; }
	public int AircraftPower { get; set; }
	public int AntiSubmarinePower { get; set; }
	public int TorpedoPower { get; set; }
	public int NightBattlePower { get; set; }
	public bool CanAttackSubmarine { get; set; }
	public bool CanOpeningASW { get; set; }
	public int Level { get; set; }
	public int ExpTotal { get; set; }
	public int ExpNext { get; set; }
	public double ExpNextPercentage { get; set; }
	public int HPCurrent { get; set; }
	public int HPMax => IsMarried switch
	{
		true => MasterShip.HPMaxMarried + HPMaxModernized,
		_ => MasterShip.HPMin + HPMaxModernized,
	};
	public int Speed { get; set; }
	public int Range { get; set; }
	public IList<int> Slot { get; set; }
	public IList<int> SlotMaster { get; set; }
	public IList<IEquipmentData?> SlotInstance { get; set; } = new List<IEquipmentData?>();
	public IList<IEquipmentDataMaster> SlotInstanceMaster { get; set; }
	public int ExpansionSlot { get; set; }
	public int ExpansionSlotMaster { get; set; }
	public IEquipmentData? ExpansionSlotInstance { get; set; }
	public IEquipmentDataMaster ExpansionSlotInstanceMaster { get; set; }
	public IList<int> AllSlot { get; set; }
	public IList<int> AllSlotMaster { get; set; }
	public IList<int> AllSlotMasterReplay { get; set; }
	public IList<IEquipmentDataMaster?> AllSlotInstanceMaster => AllSlotInstance.Select(e => e?.MasterEquipment).ToList();
	public IList<int> Aircraft { get; set; }
	public int AircraftTotal { get; set; }
	public int Fuel { get; set; }
	public int Ammo { get; set; }
	public int SlotSize { get; set; }
	public int RepairTime { get; set; }
	public int RepairSteel { get; set; }
	public int RepairFuel { get; set; }
	public int Condition { get; set; }
	public int[] Kyouka { get; set; }
	public int FirepowerModernized { get; set; }
	public int TorpedoModernized { get; set; }
	public int AAModernized { get; set; }
	public int ArmorModernized { get; set; }
	public int LuckModernized { get; set; }
	public int HPMaxModernized { get; set; }
	public int ASWModernized { get; set; }
	public int FirepowerRemain { get; set; }
	public int TorpedoRemain { get; set; }
	public int AARemain { get; set; }
	public int ArmorRemain { get; set; }
	public int LuckRemain { get; set; }
	public int HPMaxRemain { get; set; }
	public int ASWRemain { get; set; }
	public int FirepowerFit { get; set; }
	public int TorpedoFit { get; set; }
	public int AaFit { get; set; }
	public int ArmorFit { get; set; }
	public int EvasionFit { get; set; }
	public int AswFit { get; set; }
	public int LosFit { get; set; }
	// todo: fits should be included in <stat>Total extension methods
	public int FirepowerTotal => this.FirepowerTotal() + FirepowerFit;
	public int TorpedoTotal => this.TorpedoTotal() + TorpedoFit;
	public int AATotal => this.AaTotal() + AaFit;
	public int ArmorTotal => this.ArmorTotal() + ArmorFit;
	public int EvasionTotal => this.EvasionTotal() + EvasionFit;
	public int ASWTotal => this.AswTotal() + AswFit;
	public int LOSTotal => this.LosTotal() + LosFit;
	public int LuckTotal => this.LuckTotal();
	public int BomberTotal => this.BomberTotal();
	public int AccuracyTotal => this.AccuracyTotal();
	public int ExpeditionFirepowerTotal { get; }
	public int ExpeditionASWTotal { get; }
	public int ExpeditionLOSTotal { get; }
	public int ExpeditionAATotal { get; }
	public int FirepowerBase => Math.Min(MasterShip.FirepowerMin + FirepowerModernized, MasterShip.FirepowerMax);
	public int TorpedoBase => Math.Min(MasterShip.TorpedoMin + TorpedoModernized, MasterShip.TorpedoMax);
	public int AABase => Math.Min(MasterShip.AAMin + AAModernized, MasterShip.AAMax);
	public int ArmorBase => Math.Min(MasterShip.ArmorMin + ArmorModernized, MasterShip.ArmorMax);
	public int EvasionBase => MasterShip.Evasion.IsDetermined switch
	{
		true => MasterShip.Evasion.GetParameter(Level),
		_ => 0,
	};
	public int ShipID => (int)MasterShip.ShipId;
	public int MasterID { get; set; }
	public int SortID { get; set; }
	public int SallyArea { get; set; }
	public IShipDataMaster MasterShip { get; set; }
	public int RepairingDockID { get; set; } = -1;
	public int Fleet { get; set; }
	public string FleetWithIndex { get; set; }
	public bool IsMarried => Level > 99;
	public IList<IEquipmentData?> AllSlotInstance => SlotInstance.Append(ExpansionSlotInstance).ToList();
	public bool CanNoSonarOpeningAsw { get; set; }
	public bool CanAttackAtNight { get; set; }
	public int DamageControlID { get; set; }
	public int ID { get; set; }
	public Dictionary<string, string> RequestData { get; set; }
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }
	public int ASWBase => MasterShip.ASW.IsDetermined switch
	{
		true => MasterShip.ASW.GetParameter(Level),
		_ => 0,
	} + ASWModernized;
	public int LOSBase => MasterShip.LOS.IsDetermined switch
	{
		true => MasterShip.LOS.GetParameter(Level),
		_ => 0,
	};
	public int LuckBase
	{
		get => Math.Min(MasterShip.LuckMin + LuckModernized, MasterShip.LuckMax);
		set => LuckModernized = value - MasterShip.LuckMin;
	}

	public int EvasionMax { get; set; }
	public int ASWMax { get; set; }
	public int LOSMax { get; set; }
	public List<SpecialEffectItem> SpecialEffectItems { get; set; } = new();
	public int SpecialEffectItemFirepower { get; set; }
	public int SpecialEffectItemTorpedo { get; set; }
	public int SpecialEffectItemArmor { get; set; }
	public int SpecialEffectItemEvasion { get; set; }
	public bool IsLocked { get; set; }
	public bool IsLockedByEquipment { get; set; }

	public ShipDataMock(IShipDataMaster ship)
	{
		MasterShip = ship;

		HPCurrent = MasterShip.HPMin;

		SlotSize = MasterShip.SlotSize;
		Aircraft = MasterShip.Aircraft;

		Fuel = ship.Fuel;
		FuelMax = ship.Fuel;
		Ammo = ship.Ammo;
		AmmoMax = ship.Ammo;

		FirepowerModernized = MasterShip.FirepowerMax - MasterShip.FirepowerMin;
		TorpedoModernized = MasterShip.TorpedoMax - MasterShip.TorpedoMin;
		AAModernized = MasterShip.AAMax - MasterShip.AAMin;
		ArmorModernized = MasterShip.ArmorMax - MasterShip.ArmorMin;
		LuckModernized = 0;
		HPMaxModernized = 0;
		ASWModernized = 0;

		Speed = ship.Speed;
	}

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}

	public void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		throw new System.NotImplementedException();
	}

	public ShipDataMock Clone() => new(MasterShip)
	{
		// todo: copy all values
		ID = ID,
	};
}
