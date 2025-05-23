using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ISupportApiResponse
{
	/// <summary>
	/// 支援艦隊フラグ　0=なし, 1=空撃, 2=砲撃, 3=雷撃, 4=対潜
	/// </summary>
	SupportType ApiSupportFlag { get; set; }

	/// <summary>
	/// 支援艦隊情報　<see cref="ApiSupportFlag"/> = <see cref="SupportType.None"/> なら null
	/// </summary>
	ApiSupportInfo? ApiSupportInfo { get; set; }
}
