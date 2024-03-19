namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiPhaseOpeningTorpedo
{
	[JsonPropertyName("api_edam")]
	public List<double> ApiEdam { get; set; } = [];

	[JsonPropertyName("api_fdam")]
	public List<double> ApiFdam { get; set; } = [];

	[JsonPropertyName("api_frai_list_items")]
	public List<List<int>?> ApiFraiListItems { get; set; } = [];

	[JsonPropertyName("api_fcl_list_items")]
	public List<List<int>?> ApiFclListItems { get; set; } = [];

	[JsonPropertyName("api_fydam_list_items")]
	public List<List<int>?> ApiFydamListItems { get; set; } = [];

	[JsonPropertyName("api_erai_list_items")]
	public List<List<int>?> ApiEraiListItems { get; set; } = [];

	[JsonPropertyName("api_ecl_list_items")]
	public List<List<int>?> ApiEclListItems { get; set; } = [];

	[JsonPropertyName("api_eydam_list_items")]
	public List<List<int>?> ApiEydamListItems { get; set; } = [];
}
