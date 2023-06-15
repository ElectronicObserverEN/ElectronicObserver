﻿using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyInfo
{
	/// <summary>
	/// 艦隊ID？　1=(英仏艦隊), 2=(第19駆逐隊, 挺身部隊), 3=(第四戦隊)
	/// </summary>
	[JsonPropertyName("api_production_type")]
	public int ApiProductionType { get; set; }

	/// <summary>
	/// 友軍の艦船ID [艦船数]
	/// </summary>
	[JsonPropertyName("api_ship_id")]
	[Required]
	public List<ShipId> ApiShipId { get; set; } = new();

	/// <summary>
	/// 友軍のLv [艦船数]
	/// </summary>
	[JsonPropertyName("api_ship_lv")]
	[Required]
	public List<int> ApiShipLv { get; set; } = new();

	/// <summary>
	/// 友軍の現在HP [艦船数]
	/// </summary>
	[JsonPropertyName("api_nowhps")]
	[Required]
	public List<int> ApiNowhps { get; set; } = new();

	/// <summary>
	/// 友軍の最大HP [艦船数]
	/// </summary>
	[JsonPropertyName("api_maxhps")]
	[Required]
	public List<int> ApiMaxhps { get; set; } = new();

	/// <summary>
	/// 友軍の装備スロット [艦船数][5] 空きスロットは -1
	/// </summary>
	[JsonPropertyName("api_Slot")]
	[Required]
	public List<List<EquipmentId>> ApiSlot { get; set; } = new();

	/// <summary>
	/// Expansion slots
	/// </summary>
	[JsonPropertyName("api_slot_ex")]
	[Required]
	public List<EquipmentId> ApiSlotEx { get; set; } = new();

	/// <summary>
	/// 友軍の基礎ステータス [艦船数][4]; [火力, 雷装, 対空, 装甲]
	/// </summary>
	[JsonPropertyName("api_Param")]
	[Required]
	public List<List<int>> ApiParam { get; set; } = new();

	/// <summary>
	/// 再生されるボイスID? [艦船数]
	/// </summary>
	[JsonPropertyName("api_voice_id")]
	[Required]
	public List<int> ApiVoiceId { get; set; } = new();

	/// <summary>
	/// 1以上=戦闘前口上あり（口上の順番？）, 0=なし [艦船数]
	/// </summary>
	[JsonPropertyName("api_voice_p_no")]
	[Required]
	public List<int> ApiVoicePNo { get; set; } = new();
}
