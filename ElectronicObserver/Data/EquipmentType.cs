using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 装備種別
/// </summary>
public class EquipmentType : ResponseWrapper, IIdentifiable, IEquipmentType
{

	/// <summary>
	/// 装備種別ID
	/// </summary>
	public int TypeID => (int)RawData.api_id;

	/// <summary>
	/// 名前
	/// </summary>
	public string Name => RawData.api_name;

	public string NameEN => KCDatabase.Instance.Translation.Equipment.TypeName(RawData.api_name);

	//show_flg


	/// <summary>
	/// 装備種別ID
	/// </summary>
	public EquipmentTypes Type => (EquipmentTypes)TypeID;


	public override string ToString() => $"[{TypeID}] {Name}";

	public int ID => TypeID;

}
