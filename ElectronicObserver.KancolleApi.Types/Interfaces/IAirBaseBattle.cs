using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IAirBaseBattle
{
	// todo: api_air_base_injection

	/// <summary>
	/// 基地航空隊攻撃　[攻撃回数]
	/// </summary>
	List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }
}
