namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyBattle
{
	/// <summary>
	/// 照明弾投射艦インデックス　[2]; [味方, 敵]　0起点、随伴艦隊は 6-11　発動しなければ-1
	/// </summary>
	[JsonPropertyName("api_flare_pos")]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_hougeki")]
	public ApiHougeki ApiHougeki { get; set; } = new();
}
