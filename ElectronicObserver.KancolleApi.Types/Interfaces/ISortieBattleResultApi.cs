using ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ISortieBattleResultApi
{
	int ApiDests { get; set; }
	int ApiDestsf { get; set; }
	ApiEnemyInfo ApiEnemyInfo { get; set; }
	ApiEscape? ApiEscape { get; set; }
	int ApiEscapeFlag { get; set; }
	int ApiFirstClear { get; set; }
	int ApiGetBaseExp { get; set; }
	int? ApiGetEventflag { get; set; }
	List<ApiGetEventitem>? ApiGetEventitem { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	object ApiGetExmapRate { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	object ApiGetExmapUseitemId { get; set; }

	int ApiGetExp { get; set; }
	List<List<int>> ApiGetExpLvup { get; set; }
	List<int> ApiGetFlag { get; set; }
	ApiGetShip? ApiGetShip { get; set; }
	List<int> ApiGetShipExp { get; set; }
	ApiGetUseitem? ApiGetUseitem { get; set; }
	ApiLandingHp? ApiLandingHp { get; set; }
	int? ApiM1 { get; set; }
	string? ApiMSuffix { get; set; }
	int ApiMapcellIncentive { get; set; }
	int ApiMemberExp { get; set; }
	int ApiMemberLv { get; set; }
	int ApiMvp { get; set; }

	/// <summary>
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	List<object>? ApiNextMapIds { get; set; }

	int ApiQuestLevel { get; set; }
	string ApiQuestName { get; set; }
	List<int> ApiShipId { get; set; }

	// todo: this should be an enum
	string ApiWinRank { get; set; }
}
