using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.TestData.Models;

public class EquipmentDataMasterRecord
{
	public int EquipmentId { get; set; }
	public string Name { get; set; } = string.Empty;
	public int Armor { get; set; }
	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int Bomber { get; set; }
	public int Aa { get; set; }
	public int Asw { get; set; }
	public int Accuracy { get; set; }
	public int Evasion { get; set; }
	public int LoS { get; set; }
	public int Luck { get; set; }
	public int Range { get; set; }
	public int CategoryType { get; set; }
	public int IconType { get; set; }

	public EquipmentDataMasterRecord()
	{
			
	}

	public EquipmentDataMasterRecord(IEquipmentDataMaster equipment)
	{
		EquipmentId = equipment.EquipmentID;
		Name = equipment.Name;
		Armor = equipment.Armor;
		Firepower = equipment.Firepower;
		Torpedo = equipment.Torpedo;
		Bomber = equipment.Bomber;
		Aa = equipment.AA;
		Asw = equipment.ASW;
		Accuracy = equipment.Accuracy;
		Evasion = equipment.Evasion;
		LoS = equipment.LOS;
		Luck = equipment.Luck;
		Range = equipment.Range;
		CategoryType = (int)equipment.CategoryType;
		IconType = equipment.IconType;
	}

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
		CategoryType = (EquipmentTypes)CategoryType,
		IconType = IconType,
	};
}
