﻿namespace ElectronicObserver.Core.Types.Mocks;

public class UseItemMock : IUseItem
{
	public int ID => ItemID;
	public int ItemID { get; set; }
	public int Count { get; set; }
	public IUseItemMaster MasterUseItem { get; set; }
	public bool IsAvailable => true;

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}
}
