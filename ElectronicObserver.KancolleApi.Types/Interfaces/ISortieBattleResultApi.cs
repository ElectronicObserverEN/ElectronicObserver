using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface ISortieBattleResultApi
{
	public ApiGetShip? ApiGetShip { get; }

	public int ApiGetBaseExp { get; }

	public int ApiGetExp { get; }

	public string ApiWinRank { get; }

	public int ApiMvp { get; }
}
