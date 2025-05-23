using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.EquipmentList;

public class EquipmentListRow
{
	public int Id { get; }
	public EquipmentIconType IconType { get; }
	public string Name { get; }
	public int AllCount { get; }
	public int RemainCount { get; }
	public string ToolTipText { get; set; }
	public int Tag { get; set; }
	public int GameSort { get; }

	public EquipmentListRow(IEquipmentDataMaster equipment, int allCount, int remainCount)
	{
		Id = equipment.ID;
		IconType = equipment.IconTypeTyped;
		Name = equipment.NameEN;
		AllCount = allCount;
		RemainCount = remainCount;
		GameSort = (int)equipment.CategoryType * 1000 + equipment.ID;
	}
}
