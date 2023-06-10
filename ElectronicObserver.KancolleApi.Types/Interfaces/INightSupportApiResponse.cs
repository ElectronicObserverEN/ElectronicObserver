using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface INightSupportApiResponse
{
	/// <summary>
	/// 支援艦隊フラグ　0=なし, 1=空撃, 2=砲撃, 3=雷撃, 4=対潜
	/// </summary>
	public SupportType ApiNSupportFlag { get; set; }

	/// <summary>
	/// 支援艦隊情報　<see cref="ApiNSupportFlag"/> = <see cref="SupportType.None"/> なら null
	/// </summary>
	public ApiSupportInfo? ApiNSupportInfo { get; set; }
}
