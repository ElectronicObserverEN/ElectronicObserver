using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IAirBaseBattle
{
	/// <summary>
	/// 基地航空隊　噴式強襲攻撃<br />
	/// <br />
	/// AB jet attack
	/// </summary>
	ApiAirBaseInjection? ApiAirBaseInjection { get; set; }

	/// <summary>
	/// 基地航空隊攻撃　[攻撃回数]
	/// </summary>
	List<ApiAirBaseAttack>? ApiAirBaseAttack { get; set; }
}
