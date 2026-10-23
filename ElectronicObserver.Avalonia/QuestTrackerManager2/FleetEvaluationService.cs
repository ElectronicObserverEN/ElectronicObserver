using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Not;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipNationality;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public class FleetEvaluationService(IKCDatabase kcDatabase)
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;

	public bool Evaluate(FleetConditionSet set, List<IShipData>? ships)
		=> Evaluate(set.Condition, ships ?? [], set.Definitions);

	private static FleetCondition? TryGetReference(List<ReferenceDefinition> definitions, string? reference)
		=> definitions.FirstOrDefault(d => d.Name == reference)?.Condition;

	private bool Evaluate(FleetCondition? condition, List<IShipData> ships, List<ReferenceDefinition> definitions)
		=> condition switch
		{
			ReferenceCondition r => Evaluate(TryGetReference(definitions, r.Reference), ships, definitions),

			GroupCondition { Operator: LogicalOperator.And } and
				=> and.Conditions.All(c => Evaluate(c, ships, definitions)),

			GroupCondition { Operator: LogicalOperator.Or } or
				=> or.Conditions.Any(c => Evaluate(c, ships, definitions)),

			NotCondition not => !Evaluate(not.Condition, ships, definitions),

			ShipIdCondition shipId => ships.Any(s => MatchesShipId(s, shipId)),
			ShipTypeCondition shipType => ships.Any(s => shipType.ShipType == s.MasterShip.ShipType),
			ShipClassCondition shipClass => ships.Any(s => shipClass.ShipClass == s.MasterShip.ShipClassTyped),

			PositionCondition { Positions.Count: 0 } => true,
			PositionCondition pos => ships
				.Where((_, i) => pos.Positions.Contains(i + 1))
				.All(s => Evaluate(pos.Condition, [s], definitions)),

			ShipCountCondition shipCount => Compare(
				ships.Count(s => Evaluate(shipCount.Condition, [s], definitions)),
				shipCount.Operator,
				shipCount.Count),

			ShipLevelCondition l => ships.Any(s => s.Level >= l.Level),
			ShipNationalityCondition n => ships.Any(s => s.MasterShip.Nationality() == n.Nationality),

			// ignore unknown conditions
			_ => true,
		};

	private bool MatchesShipId(IShipData ship, ShipIdCondition condition) =>
		condition.Remodel switch
		{
			RemodelComparison.Exact => ship.MasterShip.ShipId == condition.Id,
			RemodelComparison.Any => AnyRemodelCheck(ship, condition),
			RemodelComparison.AtLeast => HigherRemodelCheck(ship, condition),
			_ => true,
		};

	private bool AnyRemodelCheck(IShipData ship, ShipIdCondition condition)
	{
		IShipDataMaster conditionShip = KcDatabase.MasterShips.Values
			.First(s => s.ShipId == condition.Id);

		return ship.MasterShip.BaseShip().ShipId == conditionShip.BaseShip().ShipId;
	}

	private bool HigherRemodelCheck(IShipData ship, ShipIdCondition condition)
	{
		IShipDataMaster conditionShip = KcDatabase.MasterShips.Values
			.First(s => s.ShipId == condition.Id);

		IShipDataMaster? masterShip = ship.MasterShip;

		while (masterShip is not null)
		{
			if (masterShip.ShipId == conditionShip.ShipId)
			{
				return true;
			}

			masterShip = masterShip.RemodelBeforeShip;
		}

		return false;
	}

	private static bool Compare(int actual, ComparisonOperator comparisonOperator, int expected)
		=> comparisonOperator switch
		{
			ComparisonOperator.Equals => actual == expected,
			ComparisonOperator.NotEquals => actual != expected,
			ComparisonOperator.GreaterThan => actual > expected,
			ComparisonOperator.LessThan => actual < expected,
			ComparisonOperator.GreaterOrEqual => actual >= expected,
			ComparisonOperator.LessOrEqual => actual <= expected,

			_ => true,
		};
}
