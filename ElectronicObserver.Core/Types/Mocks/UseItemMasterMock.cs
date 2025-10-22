namespace ElectronicObserver.Core.Types.Mocks;

public class UseItemMasterMock : IUseItemMaster
{
	public int ID => (int)ItemID;
	public UseItemId ItemID { get; set; }
	public int UseType { get; set; }
	public int Category { get; set; }
	public string NameTranslated { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public bool IsAvailable => true;

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}
}
