using System;
using System.Collections.Generic;
using Jot.Storage;

namespace ElectronicObserverCoreTests.Mocks;

public class TestStore : IStore
{
	public void ClearAll()
	{
		throw new NotImplementedException();
	}

	public void ClearData(string id)
	{
		throw new NotImplementedException();
	}

	public IDictionary<string, object> GetData(string id)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<string> ListIds()
	{
		throw new NotImplementedException();
	}

	public void SetData(string id, IDictionary<string, object> values)
	{
		throw new NotImplementedException();
	}
}
