using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class ShipViewModel : ObservableObject
{
	private KCDatabase Db { get; }
	private IShipData? Model { get; set; }

	public ShipId Id { get; set; }
	public string Name { get; set; } = "";
	public int Level { get; set; }

	public int Hp { get; set; }
	public int Armor { get; set; }
	public int Evasion { get; set; }
	public int AirPower { get; set; }
	public int Speed { get; set; }
	public int Range { get; set; }

	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int AntiAir { get; set; }
	public int AntiSubmarine { get; set; }
	public int Los { get; set; }
	public int Luck { get; set; }

	public ObservableCollection<EquipmentSlotViewModel> Slots { get; set; } = new();
	public EquipmentSlotViewModel? ExpansionSlot { get; set; }

	public ShipViewModel()
	{
		Db = KCDatabase.Instance;
	}

	public virtual ShipViewModel Initialize(IShipData? ship)
	{
		Model = ship;

		if (ship is null)
		{
			return this;
		}

		Id = ship.MasterShip.ShipId;
		Name = Db.Translation.Ship.Name(ship.MasterShip.Name, ship.MasterShip.ShipId);
		Level = ship.Level;

		Hp = ship.HPMax;
		Armor = ship.ArmorTotal;
		Evasion = ship.EvasionTotal;
		AirPower = Calculator.GetAirSuperiority(ship);
		Speed = ship.Speed;
		Range = ship.Range;

		Firepower = ship.FirepowerTotal;
		Torpedo = ship.TorpedoTotal;
		AntiAir = ship.AATotal;
		AntiSubmarine = ship.ASWTotal;
		Los = ship.LOSTotal;
		Luck = ship.LuckTotal;

		Slots = ship.SlotInstance
			.Take(ship.MasterShip.SlotSize)
			.Zip(ship.MasterShip.Aircraft, (eq, slot) => new EquipmentSlotViewModel(eq, slot))
			.ToObservableCollection();

		ExpansionSlot = ship.IsExpansionSlotAvailable switch
		{
			true => new EquipmentSlotViewModel(ship.ExpansionSlotInstance, 0),
			_ => null,
		};

		return this;
	}
}