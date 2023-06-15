using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IRadarBattleApiResponse : IFirstBattleApiResponse, IAirBaseBattle
{
	ApiHougeki1? ApiHougeki1 { get; set; }
}
