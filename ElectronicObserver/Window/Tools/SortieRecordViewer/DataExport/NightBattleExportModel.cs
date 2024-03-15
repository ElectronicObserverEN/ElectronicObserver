namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record NightBattleExportModel : IExportModel
{
	public required CommonDataExportModel CommonData { get; init; }
	public required string BattleType { get; init; }
	public required string? ShipName1 { get; init; }
	public required string? ShipName2 { get; init; }
	public required string? ShipName3 { get; init; }
	public required string? ShipName4 { get; init; }
	public required string? ShipName5 { get; init; }
	public required string? ShipName6 { get; init; }
	public required string PlayerFleetType { get; init; }
	public required string Start { get; init; }
	public required string AttackerSide { get; init; }
	public required int AttackType { get; init; }
	public required int AttackIndex { get; init; }
	public required string? DisplayedEquipment1 { get; init; }
	public required string? DisplayedEquipment2 { get; init; }
	public required string? DisplayedEquipment3 { get; init; }
	public required int HitType { get; init; }
	public required int Damage { get; init; }
	public required int Protected { get; init; }
	public required ShipExportModel Attacker { get; init; }
	public required ShipExportModel Defender { get; init; }
	public required string FleetType { get; init; }
	public required string EnemyFleetType { get; init; }
	public required int? PlayerSearchlightShipIndex { get; init; }
	public required int? PlayerSearchlightEquipmentId { get; init; }
	public required int? EnemySearchlightShipIndex { get; init; }
	public required int? EnemySearchlightEquipmentId { get; init; }
}
