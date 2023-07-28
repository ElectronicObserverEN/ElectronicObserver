using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IDayFriendFleetApiResponse : IFriendFleetApiResponse
{
	/// <summary>
	/// 友軍艦隊航空攻撃　発動時のみ存在　概ね api_kouku に準じる
	/// </summary>
	ApiKouku? ApiFriendlyKouku { get; }
}
