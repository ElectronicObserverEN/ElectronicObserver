namespace ElectronicObserver.Core.Types;

public enum TpGauge
{
	None = 0,
	Normal = 1,
	/// <summary>
	/// Needed for backward compatibilty
	/// </summary>
	Spring25E2 = 2,
	Spring25E2P1 = 2,
	/// <summary>
	/// Needed for backward compatibilty
	/// </summary>
	Spring25E5 = 3,
	Spring25E5P1 = 3,
	Fall25E2P2 = 4,
}
