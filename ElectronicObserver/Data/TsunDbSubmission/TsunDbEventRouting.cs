using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbEventRouting : TsunDbRouting
{
	protected override string Url => "eventrouting";

	#region Json Properties
	[JsonProperty("currentMapHP")]
	public int CurrentMapHP { get; private set; }

	[JsonProperty("maxMapHP")]
	public int MaxMapHP { get; private set; }

	[JsonProperty("difficulty")]
	public int Difficulty { get; private set; }

	[JsonProperty("gaugeNum")]
	public int GaugeNum { get; private set; }

	[JsonProperty("gaugeType")]
	public int GaugeType { get; private set; }

	[JsonProperty("debuffSound")]
	public int DebuffSound { get; private set; }
	#endregion

	#region public methods
	/// <summary>
	/// Process next node request
	/// </summary>
	/// <param name="api_data"></param>
	public void ProcessEvent(dynamic api_data)
	{
		// Some data is initiaized by Start api
		// In some case it's missing (Enabling tsundb midsortie)
		if (!IsInitialized) return;

		KCDatabase db = KCDatabase.Instance;

		CurrentMapHP = (int)api_data.api_eventmap.api_now_maphp;
		MaxMapHP = (int)api_data.api_eventmap.api_max_maphp;
		Difficulty = db.Battle.Compass.MapInfo.EventDifficulty;
		GaugeNum = db.Battle.Compass.MapInfo.CurrentGaugeIndex;
		GaugeType = db.Battle.Compass.MapInfo.GaugeType;

		DebuffSound = 0;
	}
	#endregion
}
