using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Interfaces;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseSearching : PhaseBase
{
	public override string Title => BattleRes.PhaseSearching;

	public FormationType PlayerFormationType { get; }
	public FormationType EnemyFormationType { get; }
	public EngagementType EngagementType { get; }

	public DetectionType PlayerDetectionType { get; }
	public DetectionType EnemyDetectionType { get; }

	public int? SmokeCount { get; }
	public int? BalloonCell { get; }

	public string Display => $"""
		{BattleRes.Formation}: {Constants.GetFormation(PlayerFormationType)} / {BattleRes.EnemyFormation}: {Constants.GetFormation(EnemyFormationType)}
		{BattleRes.Engagement}: {Constants.GetEngagementForm(EngagementType)}
		{BattleRes.Contact}: {Constants.GetSearchingResult(PlayerDetectionType)} / {BattleRes.EnemyContact}: {Constants.GetSearchingResult(EnemyDetectionType)}
		""" + SmokeScreenDisplay;

	private string SmokeScreenDisplay => SmokeCount switch
	{
		> 0 => $"\r\n{BattleRes.SmokeScreen} x{SmokeCount}",
		_ => "",
	};

	public PhaseSearching(IFirstBattleApiResponse battle)
	{
		PlayerFormationType = (FormationType)battle.ApiFormation[0];
		EnemyFormationType = (FormationType)battle.ApiFormation[1];
		EngagementType = (EngagementType)battle.ApiFormation[2];

		SmokeCount = battle.ApiSmokeType;
		BalloonCell = battle.ApiBalloonCell;
	}

	public PhaseSearching(IDaySearch battle) : this((IFirstBattleApiResponse)battle)
	{
		PlayerDetectionType = battle.ApiSearch[0];
		EnemyDetectionType = battle.ApiSearch[1];
	}
}
