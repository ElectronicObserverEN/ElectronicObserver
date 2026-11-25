namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class ShipExportMap : CsvExportMap<ShipExportModel>
{
	public ShipExportMap()
	{
		Map(m => m.Index).Name(() => CsvExportResources.Index);
		Map(m => m.Id).Name(() => CsvExportResources.Id);
		Map(m => m.Name).Name(() => CsvExportResources.Name);
		Map(m => m.ShipType).Name(() => CsvExportResources.ShipType);
		Map(m => m.Condition).Name(() => CsvExportResources.Condition);
		Map(m => m.HpCurrent).Name(() => CsvExportResources.HpCurrent);
		Map(m => m.HpMax).Name(() => CsvExportResources.HpMax);
		Map(m => m.DamageState).Name(() => CsvExportResources.DamageState);
		Map(m => m.FuelCurrent).Name(() => CsvExportResources.FuelCurrent);
		Map(m => m.FuelMax).Name(() => CsvExportResources.FuelMax);
		Map(m => m.AmmoCurrent).Name(() => CsvExportResources.AmmoCurrent);
		Map(m => m.AmmoMax).Name(() => CsvExportResources.AmmoMax);
		Map(m => m.Level).Name(() => CsvExportResources.ShipLevel);
		Map(m => m.Speed).Name(() => CsvExportResources.Speed);
		Map(m => m.Firepower).Name(() => CsvExportResources.Firepower);
		Map(m => m.Torpedo).Name(() => CsvExportResources.Torpedo);
		Map(m => m.AntiAir).Name(() => CsvExportResources.AntiAir);
		Map(m => m.Armor).Name(() => CsvExportResources.Armor);
		Map(m => m.Evasion).Name(() => CsvExportResources.Evasion);
		Map(m => m.AntiSubmarine).Name(() => CsvExportResources.AntiSubmarine);
		Map(m => m.Search).Name(() => CsvExportResources.Search);
		Map(m => m.Luck).Name(() => CsvExportResources.Luck);
		Map(m => m.Range).Name(() => CsvExportResources.Range);
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment1, m => m.Prefix(CsvExportResources.PrefixEquipment1));
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment2, m => m.Prefix(CsvExportResources.PrefixEquipment2));
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment3, m => m.Prefix(CsvExportResources.PrefixEquipment3));
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment4, m => m.Prefix(CsvExportResources.PrefixEquipment4));
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment5, m => m.Prefix(CsvExportResources.PrefixEquipment5));
		References<EquipmentExportMap, EquipmentExportModel>(s => s.Equipment6, m => m.Prefix(CsvExportResources.PrefixEquipment6));
	}
}
