using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IDaySupportApiResponse
{
	/// <summary>
	/// 支援艦隊フラグ　0=なし, 1=空撃, 2=砲撃, 3=雷撃, 4=対潜
	/// </summary>
	SupportType ApiSupportFlag { get; set; }

	/// <summary>
	/// 支援艦隊情報　api_support_flag = 0 なら null
	/// </summary>
	ApiSupportInfo ApiSupportInfo { get; set; }
}
