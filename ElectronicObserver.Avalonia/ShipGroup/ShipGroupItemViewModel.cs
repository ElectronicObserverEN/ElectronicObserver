using ElectronicObserver.Core.Types;

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
	public Fraction Hp => new(Ship.HPCurrent, Ship.HPMax, Ship.HPRate);
	public int Condition => Ship.Condition;
	public Fraction Fuel => new(Ship.Fuel, Ship.FuelMax, Ship.FuelRate);
	public Fraction Ammo => new(Ship.Ammo, Ship.AmmoMax, Ship.AmmoRate);

	public IEquipmentData? Slot1 => Ship.AllSlotInstance.Skip(0).FirstOrDefault();
	public IEquipmentData? Slot2 => Ship.AllSlotInstance.Skip(1).FirstOrDefault();
	public IEquipmentData? Slot3 => Ship.AllSlotInstance.Skip(2).FirstOrDefault();
	public IEquipmentData? Slot4 => Ship.AllSlotInstance.Skip(3).FirstOrDefault();
	public IEquipmentData? Slot5 => Ship.AllSlotInstance.Skip(4).FirstOrDefault();
	public string ExpansionSlot => Ship.ExpansionSlotInstance?.NameWithLevel ?? "";

	private Fraction? GetAircraftFraction(int index, int current, int max)
	{
		if (index >= Ship.SlotSize)
		{
			return null;
		}

		return max switch
		{
			0 => null,
			_ => new(current, max),
		};
	}

	private int AircraftCurrent1 => Ship.Aircraft[0];
	private int AircraftMax1 => Ship.MasterShip.Aircraft[0];
	public Fraction? Aircraft1 => GetAircraftFraction(0, AircraftCurrent1, AircraftMax1);

	private int AircraftCurrent2 => Ship.Aircraft[1];
	private int AircraftMax2 => Ship.MasterShip.Aircraft[1];
	public Fraction? Aircraft2 => GetAircraftFraction(0, AircraftCurrent2, AircraftMax2);

	private int AircraftCurrent3 => Ship.Aircraft[2];
	private int AircraftMax3 => Ship.MasterShip.Aircraft[2];
	public Fraction? Aircraft3 => GetAircraftFraction(0, AircraftCurrent3, AircraftMax3);

	private int AircraftCurrent4 => Ship.Aircraft[3];
	private int AircraftMax4 => Ship.MasterShip.Aircraft[3];
	public Fraction? Aircraft4 => GetAircraftFraction(0, AircraftCurrent4, AircraftMax4);

	private int AircraftCurrent5 => Ship.Aircraft[4];
	private int AircraftMax5 => Ship.MasterShip.Aircraft[4];
	public Fraction? Aircraft5 => GetAircraftFraction(0, AircraftCurrent5, AircraftMax5);

	private int AircraftCurrentTotal => Ship.AircraftTotal;
	private int AircraftMaxTotal => Ship.MasterShip.AircraftTotal;
	public Fraction? AircraftTotal => GetAircraftFraction(0, AircraftCurrentTotal, AircraftMaxTotal);

	public string Fleet => Ship.FleetWithIndex;
	public TimeSpan RepairTime => TimeSpan.FromMilliseconds(Ship.RepairTime);
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

	public bool IsLocked => Ship.IsLocked;
	public bool IsLockedByEquipment => Ship.IsLockedByEquipment;

	public int LockSortValue => Ship switch
	{
		{ IsLocked: true } => 2,
		{ IsLockedByEquipment: true } => 1,
		_ => 0,
	};

	public int SallyArea => Ship.SallyArea;

	public int SortId => Ship.MasterShip.SortID;
	public TimeSpan RepairTimeUnit => Ship.RepairTimeUnit;
}
