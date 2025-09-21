using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class AttackViewModelBase
{
	/// <summary>
	/// null for unknown attacker index such as AB, support, air attack etc...
	/// </summary>
	public abstract BattleIndex? AttackerIndex { get; }
	public abstract string AttackerDisplay { get; }

	public abstract BattleIndex DefenderIndex { get; }
	public abstract string DefenderDisplay { get; }

	public abstract double Damage { get; }

	// \u2192 = →
	public string AttackerDefenderDisplay => $"{AttackerDisplay} \u2192 {DefenderDisplay}";
	public abstract string DamageDisplay { get; }

	protected static IEquipmentData? GetDamecon(IShipData ship) => ship.AllSlotInstance
		.FirstOrDefault(e => e?.MasterEquipment.CategoryType is EquipmentTypes.DamageControl);

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

	public override string ToString() => $"""
		{AttackerDefenderDisplay}
		{DamageDisplay}
		""";
}
