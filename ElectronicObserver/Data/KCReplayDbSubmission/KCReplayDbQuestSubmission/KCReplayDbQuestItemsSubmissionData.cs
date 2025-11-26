using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

namespace ElectronicObserver.Data.KCReplayDbSubmission.KCReplayDbQuestSubmission;

public class KCReplayDbQuestItemsSubmissionData
{
	[JsonPropertyName("api_quest_id")]
	public required int QuestId { get; init; }

	[JsonPropertyName("api_select_no")]
	public required List<int>? ItemSelection { get; init; }

	/// <summary>
	/// Should be <see cref="ApiReqQuestClearitemgetResponse" /> but we're skipping deserialization to avoid data loss.
	/// </summary>
	[JsonPropertyName("data")]
	public required JsonNode RewardData { get; init; }
}
