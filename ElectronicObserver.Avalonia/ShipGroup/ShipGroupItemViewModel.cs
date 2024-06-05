using System;
using ElectronicObserverTypes;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ShipGroupItemViewModel(IShipData ship)
{
	private IShipData Ship { get; } = ship;

	public int MasterId => Ship.MasterID;
	public string ShipTypeName => Ship.MasterShip.ShipTypeName;
	public string Name => Ship.Name;
	public int Level => Ship.Level;
	public int ExpTotal => Ship.ExpTotal;
	public int ExpNext => Ship.ExpNext;
	public int ExpRemodel => Ship.ExpNextRemodel;
	public string Hp => $"{Ship.HPCurrent}/{Ship.HPMax}";
	public double HpRate => Ship.HPRate;
	public int Condition => Ship.Condition;
	public int Fuel => Ship.Fuel;
	public int Ammo => Ship.Ammo;

	public IEquipmentData? Slot1 => Ship.AllSlotInstance.Skip(0).FirstOrDefault();
	public IEquipmentData? Slot2 => Ship.AllSlotInstance.Skip(1).FirstOrDefault();
	public IEquipmentData? Slot3 => Ship.AllSlotInstance.Skip(2).FirstOrDefault();
	public IEquipmentData? Slot4 => Ship.AllSlotInstance.Skip(3).FirstOrDefault();
	public IEquipmentData? Slot5 => Ship.AllSlotInstance.Skip(4).FirstOrDefault();
	public string ExpansionSlot => Ship.ExpansionSlotInstance?.NameWithLevel ?? "";

	public int AircraftCurrent1 => Ship.Aircraft[0];
	public int AircraftCurrent2 => Ship.Aircraft[1];
	public int AircraftCurrent3 => Ship.Aircraft[2];
	public int AircraftCurrent4 => Ship.Aircraft[3];
	public int AircraftCurrent5 => Ship.Aircraft[4];
	public int AircraftCurrentTotal => Ship.AircraftTotal;

	public int AircraftMax1 => Ship.MasterShip.Aircraft[0];
	public int AircraftMax2 => Ship.MasterShip.Aircraft[1];
	public int AircraftMax3 => Ship.MasterShip.Aircraft[2];
	public int AircraftMax4 => Ship.MasterShip.Aircraft[3];
	public int AircraftMax5 => Ship.MasterShip.Aircraft[4];
	public int AircraftMaxTotal => Ship.MasterShip.AircraftTotal;

	public string Aircraft1 => GetAircraftString(0, AircraftCurrent1, AircraftMax1);
	public string Aircraft2 => GetAircraftString(1, AircraftCurrent2, AircraftMax2);
	public string Aircraft3 => GetAircraftString(2, AircraftCurrent3, AircraftMax3);
	public string Aircraft4 => GetAircraftString(3, AircraftCurrent4, AircraftMax4);
	public string Aircraft5 => GetAircraftString(4, AircraftCurrent5, AircraftMax5);
	public string AircraftTotal => $"{AircraftCurrentTotal}/{AircraftMaxTotal}";

	private string GetAircraftString(int index, int current, int max)
	{
		if (index >= ship.SlotSize)
		{
			return string.Empty;
		}

		return max switch
		{
			0 => string.Empty,
			_ => $"{current}/{max}",
		};
	}

	public string Fleet => Ship.FleetWithIndex;
	public TimeSpan RepairTime => new(Ship.RepairTime * 10_000L);
	public int RepairSteel => Ship.RepairSteel;
	public int RepairFuel => Ship.RepairFuel;

	public int Firepower => Ship.FirepowerBase;
	public int FirepowerRemain => Ship.FirepowerRemain;
	public int FirepowerTotal => Ship.FirepowerTotal;

	public int Torpedo => Ship.TorpedoBase;
	public int TorpedoRemain => Ship.TorpedoRemain;
	public int TorpedoTotal => Ship.TorpedoTotal;

	public int AA => Ship.AABase;
	public int AARemain => Ship.AARemain;
	public int AATotal => Ship.AATotal;

	public int Armor => Ship.ArmorBase;
	public int ArmorRemain => Ship.ArmorRemain;
	public int ArmorTotal => Ship.ArmorTotal;

	public int ASW => Ship.ASWBase;
	public int ASWTotal => Ship.ASWTotal;

	public int Evasion => Ship.EvasionBase;
	public int EvasionTotal => Ship.EvasionTotal;

	public int LOS => Ship.LOSBase;
	public int LOSTotal => Ship.LOSTotal;

	public int Luck => Ship.LuckBase;
	public int LuckRemain => Ship.LuckRemain;
	public int LuckTotal => Ship.LuckTotal;

	public int BomberTotal => Ship.BomberTotal;
	public int Speed => Ship.Speed;
	public int Range => Ship.Range;

	public int AirBattlePower => Ship.AirBattlePower;
	public int ShellingPower => Ship.ShellingPower;
	public int AircraftPower => Ship.AircraftPower;
	public int AntiSubmarinePower => Ship.AntiSubmarinePower;
	public int TorpedoPower => Ship.TorpedoPower;
	public int NightBattlePower => Ship.NightBattlePower;

	public bool Locked => Ship.IsLocked;
	public int SallyArea => Ship.SallyArea;

	public int SortId => Ship.MasterShip.SortID;
	public TimeSpan RepairTimeUnit => Ship.RepairTimeUnit;
}
