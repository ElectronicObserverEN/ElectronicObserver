namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBaseBattleExportModel
{
	public required CommonDataExportModel CommonData { get; init; }
	public required int SquadronId { get; init; }
	public required int SquadronAttackIndex { get; init; }
	public required string? AirBasePlayerContact { get; init; }
	public required string? AirBaseEnemyContact { get; init; }
	public required string? AirBaseSquadron1EquipmentName { get; init; }
	public required int? AirBaseSquadron1Aircraft { get; init; }
	public required string? AirBaseSquadron2EquipmentName { get; init; }
	public required int? AirBaseSquadron2Aircraft { get; init; }
	public required string? AirBaseSquadron3EquipmentName { get; init; }
	public required int? AirBaseSquadron3Aircraft { get; init; }
	public required string? AirBaseSquadron4EquipmentName { get; init; }
	public required int? AirBaseSquadron4Aircraft { get; init; }
	public required AirBattleStageExportModel Stage1 { get; init; }
	public required AirBattleStageExportModel Stage2 { get; init; }
	public required AirBattleShipExportModel Attacker1 { get; init; }
	public required AirBattleShipExportModel Attacker2 { get; init; }
	public required AirBattleShipExportModel Attacker3 { get; init; }
	public required AirBattleShipExportModel Attacker4 { get; init; }
	public required AirBattleShipExportModel Attacker5 { get; init; }
	public required AirBattleShipExportModel Attacker6 { get; init; }
	public required AirBattleShipExportModel Attacker7 { get; init; }
	public required int TotalTorpedoFlags { get; init; }
	public required int TotalBomberFlags { get; init; }
	public required int TorpedoFlag { get; init; }
	public required int BomberFlag { get; init; }
	public required int HitType { get; init; }
	public required int Damage { get; init; }
	public required int Protected { get; init; }
	public required ShipExportModel Defender { get; init; }
}
