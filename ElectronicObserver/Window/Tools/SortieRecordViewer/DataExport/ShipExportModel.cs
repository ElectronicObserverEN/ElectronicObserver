namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

// all required
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record ShipExportModel
{
	public int Index { get; init; }
	public int Id { get; init; }
	public string Name { get; init; }
	public string ShipType { get; init; }
	public int? Condition { get; init; }
	public int? ConditionAfterBattle { get; init; }
	public int HpCurrent { get; init; }
	public int HpMax { get; init; }
	public string DamageState { get; init; }
	public int? FuelCurrent { get; init; }
	public int? FuelAfterBattle { get; init; }
	public int? FuelMax { get; init; }
	public int? AmmoCurrent { get; init; }
	public int? AmmoAfterBattle { get; init; }
	public int? AmmoMax { get; init; }
	public int Level { get; init; }
	public string Speed { get; init; }
	public int Firepower { get; init; }
	public int Torpedo { get; init; }
	public int AntiAir { get; init; }
	public int Armor { get; init; }
	public int Evasion { get; init; }
	public int AntiSubmarine { get; init; }
	public int Search { get; init; }
	public int Luck { get; init; }
	public string Range { get; init; }
	public EquipmentExportModel Equipment1 { get; init; }
	public EquipmentExportModel Equipment2 { get; init; }
	public EquipmentExportModel Equipment3 { get; init; }
	public EquipmentExportModel Equipment4 { get; init; }
	public EquipmentExportModel Equipment5 { get; init; }
	public EquipmentExportModel Equipment6 { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
