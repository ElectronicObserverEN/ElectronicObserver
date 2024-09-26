namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IBattleApiResponse
{
	/// <summary>
	/// 味方出撃艦隊ID　数値型 <br />
	/// Fleet id
	/// </summary>
	int ApiDeckId { get; set; }

	/// <summary>
	/// 味方の現在HP [艦船数] <br />
	/// Current ship HP values [number of ships]
	/// </summary>
	List<int> ApiFNowhps { get; set; }

	/// <summary>
	/// 味方の最大HP [艦船数] <br />
	/// Max ship HP values [number of ships]
	/// </summary>
	List<int> ApiFMaxhps { get; set; }

	/// <summary>
	/// 味方基礎ステータス [艦船数][4] [0]=火力, [1]=雷装, [2]=対空, [3]=装甲 <br />
	/// Basic ship stats [number of ships][4] - [0] = firepower, [1] = torpedo, [2] = AA, [3] = armor
	/// </summary>
	List<List<int>> ApiFParam { get; set; }

	/// <summary>
	/// 敵艦船ID [艦船数]
	/// </summary>
	List<int> ApiShipKe { get; set; }

	/// <summary>
	/// 敵艦船Lv [艦船数]
	/// </summary>
	List<int> ApiShipLv { get; set; }

	/// <summary>
	/// 敵の現在HP [艦船数] <br />
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	List<object> ApiENowhps { get; set; }

	/// <summary>
	/// 敵の最大HP [艦船数] <br />
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	List<object> ApiEMaxhps { get; set; }

	/// <summary>
	/// 敵装備スロット [艦船数][5] 空きスロットは-1
	/// </summary>
	List<List<int>> ApiESlot { get; set; }

	/// <summary>
	/// 敵基礎ステータス [艦船数][4] [0]=火力, [1]=雷装, [2]=対空, [3]=装甲
	/// </summary>
	List<List<int>> ApiEParam { get; set; }

	/// <summary>
	/// 退避艦インデックス　[退避艦数]　1基点　いなければ存在しない
	/// </summary>
	List<int>? ApiEscapeIdx { get; set; }

	/// <summary>
	/// 1=装甲破壊状態 そうでなければ存在しない
	/// </summary>
	int? ApiXal01 { get; set; }

	/// <summary>
	/// 本体の戦闘糧食補給　[発動艦数]　艦船の固有ID　発動しなければ存在しない
	/// </summary>
	List<int>? ApiCombatRation { get; }

	/// <summary>
	/// 0, 1, 2, 3 - number of active smokers <br />
	/// null - old data only?
	/// </summary>
	int? ApiSmokeType { get; set; }
}
