using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IFirstBattleApiResponse : IBattleApiResponse
{
	/// <summary>
	/// 陣形/交戦形態　[0]=味方, [1]=敵, [2]=交戦形態 <br />
	/// [0,1]：1=単縦陣, 2=複縦陣, 3=輪形陣, 4=梯形陣, 5=単横陣, 6=警戒陣 <br />
	/// [2]：1=同航戦, 2=反航戦, 3=T字有利, 4=T字不利 <br />
	/// <br />
	/// Formations/engagement form - [0] = friendly formation, [1] = enemy formation, [2] = engagement form <br />
	/// [0,1] = <see cref="FormationType"/> <br />
	/// [2] = <see cref="EngagementType"/> <br />
	///  <br />
	/// </summary>
	List<int> ApiFormation { get; set; }
}
