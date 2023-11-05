namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

// all required
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record AirBattleExportModel
{
	public CommonDataExportModel CommonData { get; init; }
	public AirBattleStageExportModel Stage1 { get; init; }
	public AirBattleStageExportModel Stage2 { get; init; }
	public AntiAirCutInExportModel AntiAirCutIn { get; init; }
	public AirBattleShipExportModel Attacker1 { get; init; }
	public AirBattleShipExportModel Attacker2 { get; init; }
	public AirBattleShipExportModel Attacker3 { get; init; }
	public AirBattleShipExportModel Attacker4 { get; init; }
	public AirBattleShipExportModel Attacker5 { get; init; }
	public AirBattleShipExportModel Attacker6 { get; init; }
	public AirBattleShipExportModel Attacker7 { get; init; }
	public int TorpedoFlag { get; init; }
	public int BomberFlag { get; init; }
	public int HitType { get; init; }
	public int Damage { get; init; }
	public int Protected { get; init; }
	public ShipExportModel Defender { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
