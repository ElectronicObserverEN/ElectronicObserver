namespace ElectronicObserver.Core.Types;

public enum BattleExpeditionSuccessType
{
	Failure,
	Success,
	// stats are over 220% of the requirement, resulting in no damage to your fleet
	GreatSuccess,
}
