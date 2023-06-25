using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class AttackViewModelBase
{
	protected static string AttackDisplay(bool guardsFlagship, double damage, HitType hitType)
		=> hitType switch
		{
			HitType.Hit => $"{HitDisplay(guardsFlagship, damage)} Dmg",
			HitType.Critical => $"{HitDisplay(guardsFlagship, damage)} Critical!",
			HitType.Miss => $"{ProtectedDisplay(guardsFlagship)}Miss",
			_ => "",
		};

	private static string HitDisplay(bool guardsFlagship, double damage)
		=> $"{ProtectedDisplay(guardsFlagship)}{damage}";

	private static string ProtectedDisplay(bool guardsFlagship) => guardsFlagship switch
	{
		true => $"<{BattleRes.Protected}> ",
		_ => "",
	};
}
