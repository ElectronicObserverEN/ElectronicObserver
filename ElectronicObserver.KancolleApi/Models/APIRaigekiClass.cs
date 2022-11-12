namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiRaigekiClass
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ecl")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEcl { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_edam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEdam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_erai")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiErai { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_eydam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEydam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fcl")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFcl { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fdam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFdam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_frai")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFrai { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fydam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFydam { get; set; } = new();
}
