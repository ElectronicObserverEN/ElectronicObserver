using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.ChartAdditionalInfo;

public class APIGetMemberChartAdditionalInfoResponse
{
	[JsonPropertyName("api_deck_param")]
	public List<ApiDeckParam> ApiDeckParam { get; set; } = [];
}
