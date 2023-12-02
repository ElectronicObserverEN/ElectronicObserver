namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record ShipExportModel
{
	public required int Index { get; init; }
	public required int Id { get; init; }
	public required string Name { get; init; }
	public required string ShipType { get; init; }
	public required int? Condition { get; init; }
	public required int? ConditionAfterBattle { get; init; }
	public required int HpCurrent { get; init; }
	public required int HpMax { get; init; }
	public required string DamageState { get; init; }
	public required int? FuelCurrent { get; init; }
	public required int? FuelAfterBattle { get; init; }
	public required int? FuelMax { get; init; }
	public required int? AmmoCurrent { get; init; }
	public required int? AmmoAfterBattle { get; init; }
	public required int? AmmoMax { get; init; }
	public required int Level { get; init; }
	public required string Speed { get; init; }
	public required int Firepower { get; init; }
	public required int Torpedo { get; init; }
	public required int AntiAir { get; init; }
	public required int Armor { get; init; }
	public required int Evasion { get; init; }
	public required int AntiSubmarine { get; init; }
	public required int Search { get; init; }
	public required int Luck { get; init; }
	public required string Range { get; init; }
	public required EquipmentExportModel Equipment1 { get; init; }
	public required EquipmentExportModel Equipment2 { get; init; }
	public required EquipmentExportModel Equipment3 { get; init; }
	public required EquipmentExportModel Equipment4 { get; init; }
	public required EquipmentExportModel Equipment5 { get; init; }
	public required EquipmentExportModel Equipment6 { get; init; }
}
