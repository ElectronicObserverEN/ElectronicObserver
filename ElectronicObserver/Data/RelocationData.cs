﻿using System;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Data;

public class RelocationData : IIdentifiable
{

	/// <summary>
	/// 装備ID
	/// </summary>
	public int EquipmentID { get; set; }

	/// <summary>
	/// 配置転換を開始した時間
	/// </summary>
	public DateTime RelocatedTime { get; set; }


	/// <summary>
	/// 装備のインスタンス
	/// </summary>
	public EquipmentData EquipmentInstance => KCDatabase.Instance.Equipments[EquipmentID];


	public RelocationData(int equipmentID, DateTime relocatedTime)
	{
		EquipmentID = equipmentID;
		RelocatedTime = relocatedTime;
	}

	public int ID => EquipmentID;
	public override string ToString() => $"[{EquipmentID}] {EquipmentInstance.NameWithLevel} @ {RelocatedTime}";
}
