namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBattleExportModel : IExportModel
{
	public required CommonDataExportModel CommonData { get; init; }
	public required AirBattleStageExportModel Stage1 { get; init; }
	public required AirBattleStageExportModel Stage2 { get; init; }
	public required AntiAirCutInExportModel AntiAirCutIn { get; init; }
	public required AirBattleShipExportModel Attacker1 { get; init; }
	public required AirBattleShipExportModel Attacker2 { get; init; }
	public required AirBattleShipExportModel Attacker3 { get; init; }
	public required AirBattleShipExportModel Attacker4 { get; init; }
	public required AirBattleShipExportModel Attacker5 { get; init; }
	public required AirBattleShipExportModel Attacker6 { get; init; }
	public required AirBattleShipExportModel Attacker7 { get; init; }
	public required int TorpedoFlag { get; init; }
	public required int BomberFlag { get; init; }
	public required int HitType { get; init; }
	public required int Damage { get; init; }
	public required int Protected { get; init; }
	public required ShipExportModel Defender { get; init; }
}
