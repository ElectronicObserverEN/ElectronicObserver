using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Storage;

namespace ElectronicObserver.Data;

/// <summary>
/// 艦船グループのデータを管理します。
/// </summary>
[DataContract(Name = "ShipGroupManager")]
public sealed class ShipGroupManager : DataStorage
{

	public const string DefaultFilePath = @"Settings\ShipGroups.xml";


	/// <summary>
	/// 艦船グループリスト
	/// </summary>
	[IgnoreDataMember]
	public IDDictionary<ShipGroupData> ShipGroups { get; private set; }


	[DataMember]
	private IEnumerable<ShipGroupData> ShipGroupsSerializer
	{
		get => ShipGroups.Values.OrderBy(g => g.ID).ToList();
		set => ShipGroups = new IDDictionary<ShipGroupData>(value);
	}

	public ShipGroupManager()
	{
		Initialize();
	}


	public override void Initialize()
	{
		ShipGroups = new IDDictionary<ShipGroupData>();
	}



	public ShipGroupData this[int index] => ShipGroups[index];



	public ShipGroupData Add()
	{

		int key = GetUniqueID();
		var group = new ShipGroupData(key);
		ShipGroups.Add(group);
		return group;

	}

	public int GetUniqueID()
	{
		return ShipGroups.Count > 0 ? ShipGroups.Keys.Max() + 1 : 1;
	}


	public ShipGroupManager Load()
	{

		ResourceManager.CopyFromArchive(DefaultFilePath.Replace("\\", "/"), DefaultFilePath, true, false);

		return (ShipGroupManager)Load(DefaultFilePath);
	}

	public void Save()
	{
		Save(DefaultFilePath);
	}

}
