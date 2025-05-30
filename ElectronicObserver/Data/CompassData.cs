﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using DynaJson;
using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

namespace ElectronicObserver.Data;

/// <summary>
/// 出撃時のマップ・進撃先を保持します。
/// </summary>
public class CompassData : ResponseWrapper
{

	/// <summary>
	/// 海域カテゴリID(2-3でいう2)
	/// </summary>
	public int MapAreaID => (int)RawData.api_maparea_id;

	/// <summary>
	/// 海域カテゴリ内番号(2-3でいう3)
	/// </summary>
	public int MapInfoID => (int)RawData.api_mapinfo_no;

	/// <summary>
	/// 次に向かうセルのID
	/// </summary>
	public int CellId => (int)RawData.api_no;

	/// <summary>
	/// Returns cell display if the display is known, otherwise just the id.
	/// </summary>
	public string CellDisplay => KCDatabase.Instance.Translation.Destination
		.CellDisplay(MapAreaID, MapInfoID, CellId);

	/// <summary>
	/// Returns cell display with id if the display is known, otherwise just the id.
	/// </summary>
	public string CellDisplayWithId => KCDatabase.Instance.Translation.Destination
		.CellDisplayWithId(MapAreaID, MapInfoID, CellId);

	/// <summary>
	/// 次のセルのグラフィック
	/// </summary>
	public int ColorID => (int)RawData.api_color_no;

	/// <summary>
	/// イベントID
	/// 0=初期位置, 2=資源, 3=渦潮, 4=通常戦闘, 5=ボス戦闘, 6=気のせいだった, 7=航空戦, 8=船団護衛成功
	/// </summary>
	public int EventID => (int)RawData.api_event_id;

	/// <summary>
	/// イベント種別
	/// 0=非戦闘, 1=通常戦闘, 2=夜戦, 3=夜昼戦, 4=航空戦
	/// </summary>
	public int EventKind => (int)RawData.api_event_kind;

	/// <summary>
	/// 次のセルでの分岐の本数
	/// </summary>
	public int NextBranchCount => (int)RawData.api_next;

	/// <summary>
	/// 行き止まりかどうか
	/// </summary>
	public bool IsEndPoint => NextBranchCount == 0;

	/// <summary>
	/// 吹き出しの内容
	/// 0=なし, 1="敵艦隊発見!", 2="攻撃目標発見!"
	/// </summary>
	public int CommentID
	{
		get
		{
			if (RawData.api_comment_kind()) //startには存在しないため
				return (int)RawData.api_comment_kind;
			else
				return 0;
		}
	}

	/// <summary>
	/// 索敵に成功したか 0=失敗, 1=成功(索敵機発艦)
	/// </summary>
	public int LaunchedRecon
	{
		get
		{
			if (RawData.api_production_kind())
				return (int)RawData.api_production_kind;
			else
				return 0;
		}
	}


	/// <summary>
	/// 「気のせいだった」セルにおける特殊メッセージ表示時の演出種別　なければ -1
	/// </summary>
	public int FlavorTextType
	{
		get
		{
			if (RawData.api_cell_flavor())
				return (int)RawData.api_cell_flavor.api_type;
			else
				return -1;
		}
	}

	/// <summary>
	/// 「気のせいだった」セルにおける特殊メッセージ　なければ null
	/// </summary>
	public string FlavorText
	{
		get
		{
			if (RawData.api_cell_flavor())
				return ((string)RawData.api_cell_flavor.api_message).Replace("<br>", "\r\n");
			else
				return null;
		}
	}


	/// <summary>
	/// 資源セルで入手できる資源のデータです。
	/// </summary>
	public class GetItemData
	{
		public int ItemID { get; set; }
		public int Metadata { get; set; }
		public int Amount { get; set; }

		public GetItemData(int itemID, int metadata, int amount)
		{
			ItemID = itemID;
			Metadata = metadata;
			Amount = amount;
		}
	}

	/// <summary>
	/// 入手するアイテムリスト
	/// </summary>
	public IEnumerable<GetItemData> GetItems
	{
		get
		{
			dynamic item;
			if (RawData.api_itemget())
				item = RawData.api_itemget;
			else if (RawData.api_itemget_eo_comment())
				item = RawData.api_itemget_eo_comment;
			else
				yield break;

			// item.IsArray だと参照できないため
			if (!(((dynamic)item).IsArray))
			{
				yield return new GetItemData((int)item.api_usemst, (int)item.api_id, (int)item.api_getcount);

			}
			else
			{
				foreach (dynamic i in item)
				{
					yield return new GetItemData((int)i.api_usemst, (int)i.api_id, (int)i.api_getcount);
				}
			}
		}

	}


	/// <summary>
	/// 渦潮で失うアイテムのID
	/// </summary>
	public int WhirlpoolItemID
	{
		get
		{
			if (RawData.api_happening())
			{
				return (int)RawData.api_happening.api_mst_id;
			}
			else
			{
				return -1;
			}
		}
	}

	/// <summary>
	/// 渦潮で失うアイテムの量
	/// </summary>
	public int WhirlpoolItemAmount
	{
		get
		{
			if (RawData.api_happening())
			{
				return (int)RawData.api_happening.api_count;
			}
			else
			{
				return 0;
			}
		}
	}

	/// <summary>
	/// 渦潮の被害を電探で軽減するか
	/// </summary>
	public bool WhirlpoolRadarFlag
	{
		get
		{
			if (RawData.api_happening())
			{
				return (int)RawData.api_happening.api_dentan != 0;
			}
			else
			{
				return false;
			}
		}
	}

	/// <summary>
	/// 能動分岐の選択肢
	/// </summary>
	public ReadOnlyCollection<int> RouteChoices
	{
		get
		{
			if (RawData.api_select_route())
			{
				return Array.AsReadOnly((int[])RawData.api_select_route.api_select_cells);
			}
			else
			{
				return null;
			}
		}
	}

	/// <summary>
	/// Map Node Selection Display ID
	/// </summary>
	public string[] RouteChoicesDisplay
	{
		get
		{
			var nodes = new string[RouteChoices.Count];
			for (int i = 0; i < RouteChoices.Count; i++)
			{
				nodes[i] = KCDatabase.Instance.Translation.Destination.CellDisplay(MapAreaID, MapInfoID, RouteChoices[i]);
			}
			return nodes;
		}
	}


	/// <summary>
	/// 航空偵察の航空機
	/// 0=なし, 1=大型飛行艇, 2=水上偵察機
	/// </summary>
	public int AirReconnaissancePlane
	{
		get
		{
			if (RawData.api_airsearch())
			{
				return (int)RawData.api_airsearch.api_plane_type;
			}
			else
			{
				return 0;
			}
		}
	}


	/// <summary>
	/// 航空偵察結果
	/// 0=失敗, 1=成功, 2=大成功
	/// </summary>
	public int AirReconnaissanceResult
	{
		get
		{
			if (RawData.api_airsearch())
			{
				return (int)RawData.api_airsearch.api_result;
			}
			else
			{
				return 0;
			}
		}
	}


	/// <summary>
	/// 空襲が存在したか
	/// </summary>
	public bool HasAirRaid => RawData.api_destruction_battle();


	/// <summary>
	/// 基地空襲戦データ
	/// </summary>
	public dynamic AirRaidData => HasAirRaid ? RawData.api_destruction_battle : null;



	/// <summary>
	/// 空襲被害の種別
	/// 1=資源に被害, 2=資源・航空隊に被害, 3=航空隊に被害, 4=損害なし
	/// </summary>
	public int AirRaidDamageKind => HasAirRaid && RawData.api_destruction_battle.api_lost_kind() ? (int)RawData.api_destruction_battle.api_lost_kind : 0;



	/// <summary>
	/// 海域HP の現在値
	/// 取得不能なら 0
	/// </summary>
	public int MapHPCurrent => RawData.api_eventmap() ? (int)RawData.api_eventmap.api_now_maphp : 0;


	/// <summary>
	/// 海域HP の最大値
	/// 取得不能なら 0
	/// </summary>
	public int MapHPMax => RawData.api_eventmap() ? (int)RawData.api_eventmap.api_max_maphp : 0;


	/// <summary>
	/// 緊急泊地修理が可能か
	/// </summary>
	public bool CanEmergencyAnchorageRepair => RawData.api_anchorage_flag() && (int)RawData.api_anchorage_flag != 0;


	/// <summary>
	/// 対応する海域情報
	/// </summary>
	public IMapInfoData MapInfo => KCDatabase.Instance.MapInfo[MapAreaID * 10 + MapInfoID];

	/// <summary>
	/// True if boss node
	/// </summary>
	public bool IsBossNode => EventID == 5;

	/// <summary>
	/// Map preview of the enemy fleet
	/// </summary>
	public List<EDeckInfo> EnemyFleetPreview
	{
		get
		{
			if (!RawData.api_e_deck_info()) return new();
			if (RawData.api_e_deck_info is not JsonObject fleets) return new();
			if (!fleets.IsArray) return new();

			return JsonSerializer.Deserialize<List<EDeckInfo>>(fleets.ToString()) ?? new();
		}
	}
}
