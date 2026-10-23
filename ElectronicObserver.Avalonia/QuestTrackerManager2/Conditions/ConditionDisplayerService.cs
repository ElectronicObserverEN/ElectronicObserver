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
using ElectronicObserver.Avalonia.Translation;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public sealed class ConditionDisplayerService(DataService dataService)
{
	private ShipClassDisplayConverter ShipClassDisplayConverter { get; } = new() { DataService = dataService };

	public string ToDisplay(ConditionDisplayerViewModel? conditionDisplayer) => conditionDisplayer switch
	{
		GroupConditionViewModel g => string.Join(
			$" {Converters.ToDisplay(g.Operator)} ",
			g.Conditions.Select(ToSubConditionDisplay)),

		NotConditionViewModel n => $"!({ToDisplay(n.Condition)})",
		ShipIdConditionViewModel i => $"{i.Ship?.NameEN} ({Converters.ToDisplay(i.RemodelComparisonType)})",
		ShipTypeConditionViewModel t => Converters.ToDisplay(t.ShipType),
		
		ShipClassConditionViewModel c => ShipClassDisplayConverter.Convert(c.ShipClass),

		PositionConditionViewModel p =>
			$"{ToSubConditionDisplay(p.Condition)} ({string.Join(
				$" {Converters.ToDisplay(LogicalOperator.And)} ",
				p.Positions.Where(pos => pos.IsChecked).Select(Converters.ToDisplay))})",

		CountConditionViewModel c =>
			$"{ToSubConditionDisplay(c.Condition)} {Converters.ToDisplay(c.ComparisonOperator)} {c.Count}",

		ReferenceConditionViewModel r => r.Reference?.Name ?? string.Empty,
		ShipLevelConditionViewModel l => $"lv. >= {l.Level}",
		ShipNationalityConditionViewModel l => Converters.ToDisplay(l.Nationality),

		_ => string.Empty,
	};

	private string ToSubConditionDisplay(ConditionDisplayerViewModel? conditionDisplayer)
		=> conditionDisplayer switch
		{
			GroupConditionViewModel g => $"({ToDisplay(g)})",
			_ => ToDisplay(conditionDisplayer),
		};
}
