using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBaseAirDefenseExportModel
{
	public required DateTime Date { get; init; }
	public required string World { get; init; }
	public required string Square { get; init; }
	public required string PlayerFormation { get; init; }
	public required string EnemyFormation { get; init; }
	public required string Engagement { get; init; }
	public required string AirBaseDamage { get; init; }
	public required int PlayerAircraft { get; init; }
	public required int PlayerAircraftLost { get; init; }
	public required int EnemyAircraft { get; init; }
	public required int EnemyAircraftLost { get; init; }
	public required string? AirState { get; init; }
	public required string? PlayerContact { get; init; }
	public required string? EnemyContact { get; init; }
	public required AirBaseExportModel AirBase1 { get; init; }
	public required AirBaseExportModel AirBase2 { get; init; }
	public required AirBaseExportModel AirBase3 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip1 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip2 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip3 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip4 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip5 { get; init; }
	public required AirBaseAirDefenseShipExportModel EnemyShip6 { get; init; }

	public required int PlayerTorpedoFlags1 { get; init; }
	public required int PlayerTorpedoFlags2 { get; init; }
	public required int PlayerTorpedoFlags3 { get; init; }

	public required int EnemyTorpedoFlags1 { get; init; }
	public required int EnemyTorpedoFlags2 { get; init; }
	public required int EnemyTorpedoFlags3 { get; init; }
	public required int EnemyTorpedoFlags4 { get; init; }
	public required int EnemyTorpedoFlags5 { get; init; }
	public required int EnemyTorpedoFlags6 { get; init; }

	public required int PlayerBomberFlags1 { get; init; }
	public required int PlayerBomberFlags2 { get; init; }
	public required int PlayerBomberFlags3 { get; init; }

	public required int EnemyBomberFlags1 { get; init; }
	public required int EnemyBomberFlags2 { get; init; }
	public required int EnemyBomberFlags3 { get; init; }
	public required int EnemyBomberFlags4 { get; init; }
	public required int EnemyBomberFlags5 { get; init; }
	public required int EnemyBomberFlags6 { get; init; }

	public required int PlayerHitFlags1 { get; init; }
	public required int PlayerHitFlags2 { get; init; }
	public required int PlayerHitFlags3 { get; init; }

	public required int EnemyHitFlags1 { get; init; }
	public required int EnemyHitFlags2 { get; init; }
	public required int EnemyHitFlags3 { get; init; }
	public required int EnemyHitFlags4 { get; init; }
	public required int EnemyHitFlags5 { get; init; }
	public required int EnemyHitFlags6 { get; init; }

	public required double PlayerDamage1 { get; init; }
	public required double PlayerDamage2 { get; init; }
	public required double PlayerDamage3 { get; init; }

	public required double EnemyDamage1 { get; init; }
	public required double EnemyDamage2 { get; init; }
	public required double EnemyDamage3 { get; init; }
	public required double EnemyDamage4 { get; init; }
	public required double EnemyDamage5 { get; init; }
	public required double EnemyDamage6 { get; init; }
}
