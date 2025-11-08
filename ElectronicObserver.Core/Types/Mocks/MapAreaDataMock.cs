using System;
using System.Collections.Generic;

namespace ElectronicObserver.Core.Types.Mocks;

public class MapAreaDataMock : IMapAreaData
{
	public int MapAreaID { get; set; }

	public string Name { get; set; } = "";

	public string NameEN => Name;

	public int MapType { get; set; }

	public bool IsEventArea => MapAreaID > 10;

	public int ID { get; set; }

	public void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		throw new NotImplementedException();
	}

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new NotImplementedException();
	}
}
