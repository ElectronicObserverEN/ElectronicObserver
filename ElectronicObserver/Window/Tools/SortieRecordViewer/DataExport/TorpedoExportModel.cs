﻿namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

// all required
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record TorpedoExportModel
{
	public CommonDataExportModel CommonData { get; init; }
	public string BattleType { get; init; }
	public string PlayerFleetType { get; init; }
	public string BattlePhase { get; init; }
	public string AttackerSide { get; init; }
	public int? AttackType { get; init; }
	public string? DisplayedEquipment1 { get; init; }
	public string? DisplayedEquipment2 { get; init; }
	public string? DisplayedEquipment3 { get; init; }
	public int HitType { get; init; }
	public int Damage { get; init; }
	public int Protected { get; init; }
	public ShipExportModel Attacker { get; init; }
	public ShipExportModel Defender { get; init; }
	public string FleetType { get; init; }
	public string EnemyFleetType { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
