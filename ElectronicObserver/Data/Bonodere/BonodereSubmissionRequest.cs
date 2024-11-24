using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Data.Bonodere;

public class BonodereSubmissionRequest
{
	[JsonPropertyName("data")]
	public required List<SenkaEntryModel> Data { get; set; }
}
