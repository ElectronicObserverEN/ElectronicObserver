using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.TestData.Models;

public class EquipmentDataMasterRecord
{
	public required int EquipmentId { get; set; }
	public required string Name { get; set; }
	public required int Armor { get; set; }
	public required int Firepower { get; set; }
	public required int Torpedo { get; set; }
	public required int Bomber { get; set; }
	public required int Aa { get; set; }
	public required int Asw { get; set; }
	public required int Accuracy { get; set; }
	public required int Evasion { get; set; }
	public required int LoS { get; set; }
	public required int Luck { get; set; }
	public required int Range { get; set; }
	public required int AircraftCost { get; set; }
	public required EquipmentCardType CardType { get; set; }
	public required EquipmentTypes CategoryType { get; set; }
	public required EquipmentIconType IconType { get; set; }

	public static EquipmentDataMasterRecord FromMasterEquipment(IEquipmentDataMaster equipment) => new()
	{
		EquipmentId = equipment.EquipmentID,
		Name = equipment.Name,
		Armor = equipment.Armor,
		Firepower = equipment.Firepower,
		Torpedo = equipment.Torpedo,
		Bomber = equipment.Bomber,
		Aa = equipment.AA,
		Asw = equipment.ASW,
		Accuracy = equipment.Accuracy,
		Evasion = equipment.Evasion,
		LoS = equipment.LOS,
		Luck = equipment.Luck,
		Range = equipment.Range,
		AircraftCost = equipment.AircraftCost,
		CardType = equipment.CardType,
		CategoryType = equipment.CategoryType,
		IconType = equipment.IconTypeTyped,
	};

	public IEquipmentDataMaster ToMasterEquipment() => new EquipmentDataMasterMock
	{
		EquipmentID = EquipmentId,
		Name = Name,
		Armor = Armor,
		Firepower = Firepower,
		Torpedo = Torpedo,
		Bomber = Bomber,
		AA = Aa,
		ASW = Asw,
		Accuracy = Accuracy,
		Evasion = Evasion,
		LOS = LoS,
		Luck = Luck,
		Range = Range,
		AircraftCost = AircraftCost,
		CardType = CardType,
		CategoryType = CategoryType,
		IconTypeTyped = IconType,
	};
}
