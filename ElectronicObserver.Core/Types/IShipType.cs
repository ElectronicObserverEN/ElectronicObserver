using System.Collections.Generic;

namespace ElectronicObserver.Core.Types;

public interface IShipType
{
	/// <summary>
	/// 艦種ID
	/// </summary>
	public int TypeID { get; }

	/// <summary>
	/// 並べ替え順
	/// </summary>
	public int SortID { get; }

	/// <summary>
	/// 艦種名
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// Name in romaji
	/// </summary>
	public string NameEN { get; }

	/// <summary>
	/// 入渠時間係数
	/// </summary>
	public int RepairTime { get; }

	public IList<int> EquippableCategories { get; }


	/// <summary>
	/// 艦種ID
	/// </summary>
	public ShipTypes Type { get; }


	public int ID { get; }
}
