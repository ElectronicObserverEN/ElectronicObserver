using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public class AutoRefreshModel
{
	public int Id { get; set; }
	public bool IsSingleMapMode { get; set; }
	public List<AutoRefreshRuleModel> Rules { get; set; }
}