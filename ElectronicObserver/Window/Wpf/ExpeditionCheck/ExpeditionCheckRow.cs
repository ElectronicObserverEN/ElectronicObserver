using System.Linq;
using System.Windows.Media;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using static ElectronicObserver.Data.MissionClearCondition;

namespace ElectronicObserver.Window.Wpf.ExpeditionCheck;

public class ExpeditionCheckRow
{
	public int AreaId { get; set; }
	public int ExpeditionId { get; set; }

	public string ExpeditionDisplayId { get; set; }
	public string AreaName { get; set; }
	public string ExpeditionName { get; set; }

	public MissionClearConditionResult Fleet1Result { get; set; }

	public MissionClearConditionResult Fleet2Result { get; set; }

	public MissionClearConditionResult Fleet3Result { get; set; }

	public MissionClearConditionResult Fleet4Result { get; set; }

	public MissionClearConditionResult Conditions { get; set; }

	public ExpeditionType ExpeditionType { get; set; }

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

	public string? Fleet1Tooltip => GetResultTooltip(Fleet1Result);
	public string? Fleet2Tooltip => GetResultTooltip(Fleet2Result);
	public string? Fleet3Tooltip => GetResultTooltip(Fleet3Result);
	public string? Fleet4Tooltip => GetResultTooltip(Fleet4Result);
	public string? ConditionTooltip => GetSuccessToolTip(Conditions);

	public int SortId => AreaId * 1000 + ExpeditionId;

	private System.Drawing.Color GetBackgroundColor(MissionClearConditionResult result) => result?.IsSuceeded switch
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
		if(AreaId % 2 == 1 && AreaId != 7)
		{
			return Utility.Configuration.Config.UI.BackColor;
		}
		else
		{
			return Utility.Configuration.Config.UI.SubBackColor;
		}
	}
	private string GetText(MissionClearConditionResult result) => result?.IsSuceeded switch
	{
		true => GetSuccessText(result),
		false => string.Join(", ", result.FailureReason),
		_ => ""
	};

	private string GetSuccessText(MissionClearConditionResult result) => result?.FailureReason.Any() switch
	{
		true => string.Join(", ", result.FailureReason),
		false => ExpeditionType switch
		{
			ExpeditionType.CombatTypeTwoExpedition => result switch
			{
				{ SuccessType: BattleExpeditionSuccessType.GreatSuccess } => DialogRes.ExpeditionCheckDoubleOkSign,
				_ => DialogRes.ExpeditionCheckOkSign
			} + string.Join(", ", result.SuccessPercent),

			ExpeditionType.CombatTypeOneExpedition => DialogRes.ExpeditionCheckOkSign + string.Join(", ", result.SuccessPercent),
			_ => DialogRes.ExpeditionCheckOkSign
		},
		_ => ""
	};

	private string? GetResultTooltip(MissionClearConditionResult result) => result?.IsSuceeded switch
	{
		true => GetSuccessToolTip(result),
		false => string.Join("\n", result.FailureReason),
		_ => null
	};


	private string? GetSuccessToolTip(MissionClearConditionResult result) => result?.FailureReason.Any() switch
	{
		true => string.Join(", ", result.FailureReason),
		false => ExpeditionType switch
		{
			ExpeditionType.CombatTypeTwoExpedition or ExpeditionType.CombatTypeOneExpedition => string.Join("\n", result.SuccessPercent),
			_ => null
		},
		_ => null
	};
}
