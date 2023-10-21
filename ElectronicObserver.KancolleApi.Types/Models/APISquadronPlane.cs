using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSquadronPlane
{
	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_mst_id")]
	public EquipmentId ApiMstId { get; set; }
}
