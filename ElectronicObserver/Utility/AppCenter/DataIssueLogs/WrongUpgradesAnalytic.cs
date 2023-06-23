using System.Collections.Generic;
using System.Text.Json;

namespace ElectronicObserver.Utility.AppCenter.DataIssueLogs;
public record WrongUpgradesAnalytic(int DataVersion, int ShipId, byte Day, List<int> ExpectedUpgrades, List<int> ActualUpgrades) : DataIssueAnalytic(DataVersion)
{
	public override string Key => "WRONGUPGRADES";

	public override Dictionary<string, string> GetValues()
	{
		List<string> data = new() { ShipId.ToString(), Day.ToString(), string.Join(",", ExpectedUpgrades), string.Join(",", ActualUpgrades), DataVersion.ToString() };

		return new Dictionary<string, string> { { "data", JsonSerializer.Serialize(data) } };
	}
}
