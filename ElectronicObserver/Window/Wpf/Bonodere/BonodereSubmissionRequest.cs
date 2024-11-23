using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereSubmissionRequest
{
	[JsonPropertyName("data")]
	public required List<SenkaEntryModel> Data { get; set; }
}
