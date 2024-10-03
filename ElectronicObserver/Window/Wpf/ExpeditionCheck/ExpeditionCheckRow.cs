using System.Linq;
using System.Windows.Media;
using ElectronicObserver.Avalonia.ExpeditionCalculator;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.ExpeditionCheck;

public class ExpeditionCheckRow
{
	public required int AreaId { get; init; }
	public required int ExpeditionId { get; init; }

	public required string ExpeditionDisplayId { get; init; }
	public required string AreaName { get; init; }
	public required string ExpeditionName { get; set; }

	public required MissionClearCondition.MissionClearConditionResult Fleet1Result { get; init; }
	public required MissionClearCondition.MissionClearConditionResult Fleet2Result { get; init; }
	public required MissionClearCondition.MissionClearConditionResult Fleet3Result { get; init; }
	public required MissionClearCondition.MissionClearConditionResult Fleet4Result { get; init; }
	public required MissionClearCondition.MissionClearConditionResult Conditions { get; init; }

	public required ExpeditionType ExpeditionType { get; init; }

	public string IdDisplay => $"{ExpeditionDisplayId}: {AreaName}";

	public SolidColorBrush Fleet1BackgroundColor => GetBackgroundColor(Fleet1Result).ToBrush();
	public SolidColorBrush Fleet2BackgroundColor => GetBackgroundColor(Fleet2Result).ToBrush();
	public SolidColorBrush Fleet3BackgroundColor => GetBackgroundColor(Fleet3Result).ToBrush();
	public SolidColorBrush Fleet4BackgroundColor => GetBackgroundColor(Fleet4Result).ToBrush();
	public SolidColorBrush WorldBackgroundColor => GetBackground().ToBrush();

	public string Fleet1Text => GetText(Fleet1Result);
	public string Fleet2Text => GetText(Fleet2Result);
	public string Fleet3Text => GetText(Fleet3Result);
	public string Fleet4Text => GetText(Fleet4Result);
	public string ConditionText => GetSuccessText(Conditions);

	public string Fleet1Tooltip => GetResultTooltip(Fleet1Result);
	public string Fleet2Tooltip => GetResultTooltip(Fleet2Result);
	public string Fleet3Tooltip => GetResultTooltip(Fleet3Result);
	public string Fleet4Tooltip => GetResultTooltip(Fleet4Result);
	public string ConditionTooltip => GetSuccessToolTip(Conditions);

	public int SortId => AreaId * 1000 + ExpeditionId;

	private System.Drawing.Color GetBackgroundColor(MissionClearCondition.MissionClearConditionResult result) => result.IsSuceeded switch
	{
		false => Utility.Configuration.Config.UI.ThemeMode switch
		{
			0 => System.Drawing.Color.MistyRose,
			_ => System.Drawing.Color.FromArgb(255, 255, 63, 63)
		},
		_ => GetBackground()
	};

	private System.Drawing.Color GetBackground()
	{
		if (AreaId % 2 == 1 && AreaId != 7)
		{
			return Utility.Configuration.Config.UI.BackColor;
		}
		else
		{
			return Utility.Configuration.Config.UI.SubBackColor;
		}
	}

	private string GetText(MissionClearCondition.MissionClearConditionResult result) => result.IsSuceeded switch
	{
		true => GetSuccessText(result),
		false => string.Join(", ", result.FailureReason),
	};

	private string GetSuccessText(MissionClearCondition.MissionClearConditionResult result) => result.FailureReason.Any() switch
	{
		true => string.Join(", ", result.FailureReason),
		false => ExpeditionType switch
		{
			ExpeditionType.CombatTypeTwoExpedition => result switch
			{
				{ SuccessType: BattleExpeditionSuccessType.GreatSuccess } => DialogRes.ExpeditionCheckDoubleOkSign + GreatSuccessRate(result),
				_ => GreatSuccessRate(result),
			} + string.Join(", ", result.SuccessPercent),

			ExpeditionType.CombatTypeOneExpedition => GreatSuccessRate(result) + string.Join(", ", result.SuccessPercent),
			_ => GreatSuccessRate(result),
		},
	};

	private string GetResultTooltip(MissionClearCondition.MissionClearConditionResult result) => result.IsSuceeded switch
	{
		true => GetSuccessToolTip(result),
		false => string.Join("\n", result.FailureReason),
	};

	private string GetSuccessToolTip(MissionClearCondition.MissionClearConditionResult result) => result.FailureReason.Any() switch
	{
		true => string.Join(", ", result.FailureReason),
		false => ExpeditionType switch
		{
			ExpeditionType.CombatTypeTwoExpedition or
			ExpeditionType.CombatTypeOneExpedition
				=> string.Join("\n", [GreatSuccessRate(result), .. result.SuccessPercent]),

			_ => GreatSuccessRate(result),
		},
	};

	private string GreatSuccessRate(MissionClearCondition.MissionClearConditionResult result)
	{
		string unknownGreatSuccess = $"{ExpeditionCalculatorResources.GreatSuccess} ???";

		if (result.TargetFleet is not IFleetData fleet) return unknownGreatSuccess;
		if (fleet.MembersInstance.FirstOrDefault() is not IShipData flagship) return unknownGreatSuccess;

		Expedition? expedition = ExpeditionCalculatorData.Expeditions
			.Find(e => e.Id == ExpeditionId);

		if (expedition is null) return unknownGreatSuccess;

		FleetInfoViewModel fleetInfo = new()
		{
			AllSparkled = fleet.MembersInstance.OfType<IShipData>().All(s => s.Condition > 49),
			SparkleCount = fleet.MembersInstance.OfType<IShipData>().Count(s => s.Condition > 49),
			DrumCount = fleet.MembersInstance
				.OfType<IShipData>()
				.SelectMany(s => s.AllSlotInstance)
				.OfType<IEquipmentData>()
				.Count(e => e.MasterEquipment.CategoryType is EquipmentTypes.TransportContainer),
			FlagshipLevel = flagship.Level,
		};

		double greatSuccessRate = fleetInfo.GreatSuccessRate(expedition);

		return $"{ExpeditionCalculatorResources.GreatSuccess} {greatSuccessRate:P2}";
	}
}
