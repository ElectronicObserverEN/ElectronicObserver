using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 艦種
/// </summary>
public class ShipType : ResponseWrapper, IIdentifiable, IShipType
{

	/// <summary>
	/// 艦種ID
	/// </summary>
	public int TypeID => (int)RawData.api_id;

	/// <summary>
	/// 並べ替え順
	/// </summary>
	public int SortID => (int)RawData.api_sortno;

	/// <summary>
	/// 艦種名
	/// </summary>
	public string Name => RawData.api_name;

	/// <summary>
	/// Name in romaji
	/// </summary>
	public string NameEN => KCDatabase.Instance.Translation.Ship.TypeName(RawData.api_name);

	/// <summary>
	/// 入渠時間係数
	/// </summary>
	public int RepairTime => (int)RawData.api_scnt;


	//TODO: api_kcnt


	/// <summary>
	/// 装備可能なカテゴリ一覧
	/// </summary>
	private int[] _equippableCategories;
	public IList<int> EquippableCategories => Array.AsReadOnly(_equippableCategories);


	/// <summary>
	/// 艦種ID
	/// </summary>
	public ShipTypes Type => (ShipTypes)TypeID;


	public int ID => TypeID;
	public override string ToString() => $"[{TypeID}] {Name}";



	public ShipType()
		: base()
	{
		_equippableCategories = new int[0];
	}

	public override void LoadFromResponse(string apiname, dynamic data)
	{

		base.LoadFromResponse(apiname, (object)data);


		if (IsAvailable)
		{
			IEnumerable<int> getType()
			{
				foreach (KeyValuePair<string, object> type in RawData.api_equip_type)
				{
					if ((double)type.Value != 0)
						yield return Convert.ToInt32(type.Key);
				}
			}

			_equippableCategories = getType().ToArray();
		}
	}

}
