using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbEventRouting : TsunDbRouting
{
	protected override string Url => "eventrouting";

	#region Json Properties
	[JsonPropertyName("currentMapHP")]
	public int CurrentMapHP { get; private set; }

	[JsonPropertyName("maxMapHP")]
	public int MaxMapHP { get; private set; }

	[JsonPropertyName("difficulty")]
	public int Difficulty { get; private set; }

	[JsonPropertyName("gaugeNum")]
	public int GaugeNum { get; private set; }

	[JsonPropertyName("gaugeType")]
	public int GaugeType { get; private set; }

	[JsonPropertyName("debuffSound")]
	public int DebuffSound { get; private set; }
	#endregion

	#region public methods
	/// <summary>
	/// Process next node request
	/// </summary>
	/// <param name="apiData"></param>
	public void ProcessEvent(dynamic apiData)
	{
		// Some data is initialized by Start api
		// In some case it's missing (Enabling tsundb midsortie)
		if (!IsInitialized) return;

		KCDatabase db = KCDatabase.Instance;

		CurrentMapHP = (int)apiData.api_eventmap.api_now_maphp;
		MaxMapHP = (int)apiData.api_eventmap.api_max_maphp;
		Difficulty = db.Battle.Compass.MapInfo.EventDifficulty;
		GaugeNum = db.Battle.Compass.MapInfo.CurrentGaugeIndex;
		GaugeType = db.Battle.Compass.MapInfo.GaugeType;

		DebuffSound = 0;
	}
	#endregion
}
