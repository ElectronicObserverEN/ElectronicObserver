using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface INightFriendFleetApiResponse : IFriendFleetApiResponse
{
	/// <summary>
	/// 友軍艦隊攻撃　発動時のみ存在？
	/// </summary>
	ApiFriendlyBattle? ApiFriendlyBattle { get; set; }
}
