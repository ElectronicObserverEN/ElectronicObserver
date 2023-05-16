namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IBattleApiRequest
{
	/// <summary>
	/// null - no smoke devices in fleet <br />
	/// 0 - smokescreen wasn't activated <br />
	/// 1 - smokescreen was activated
	/// </summary>
	string? ApiSmokeFlag { get; set; }
}
