using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;

namespace ElectronicObserver.Utility.AppCenter.DataIssueLogs;

public abstract record DataIssueAnalytic(int DataVersion)
{
	public abstract string Key { get; }

	public abstract Dictionary<string, string> GetValues();

	public void ReportIssue()
	{
		Analytics.TrackEvent(Key, GetValues());
	}
}
