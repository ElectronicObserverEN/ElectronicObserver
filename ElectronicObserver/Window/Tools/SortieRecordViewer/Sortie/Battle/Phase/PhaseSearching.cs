﻿using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Properties.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseSearching : PhaseBase
{
	public string Title => BattleRes.PhaseSearching;

	private FormationType PlayerFormationType { get; }
	private FormationType EnemyFormationType { get; }
	private EngagementType EngagementType { get; }

	private DetectionType PlayerDetectionType { get; }
	private DetectionType EnemyDetectionType { get; }

	public int? SmokeCount { get; }

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

	public PhaseSearching(IBattleApiResponse battle)
	{
		PlayerFormationType = (FormationType)battle.ApiFormation[0];
		EnemyFormationType = (FormationType)battle.ApiFormation[1];
		EngagementType = (EngagementType)battle.ApiFormation[2];

		SmokeCount = battle.ApiSmokeType;
	}

	public PhaseSearching(IDaySearch battle) : this((IBattleApiResponse)battle)
	{
		PlayerDetectionType = battle.ApiSearch[0];
		EnemyDetectionType = battle.ApiSearch[1];
	}
}
