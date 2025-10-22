namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiDeckParam
{
	/// <summary>
	/// Fleet air power
	/// </summary>
	[JsonPropertyName("api_seiku_value")]
	public int ApiSeikuValue { get; set; }

	/// <summary>
	/// Fleet transport point value on S rank
	/// </summary>
	[JsonPropertyName("api_tp_value")]
	public int ApiTpValue { get; set; }
}
