using System.Collections.Generic;

namespace ElectronicObserver.Core.Types;

public interface IApiWrapper : IResponseWrapper
{
	void LoadFromRequest(string apiname, Dictionary<string, string> data);
}
