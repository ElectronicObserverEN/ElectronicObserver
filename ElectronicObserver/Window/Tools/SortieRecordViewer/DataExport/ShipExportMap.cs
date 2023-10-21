using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class ShipExportMap : ClassMap<ShipExportModel>
{
	// prefix here is needed because using the Prefix function wipes the parent prefix
	// so here we want Attacker.EquipmentN.
	// parent declares the Attacker. prefix, this class declares the EquipmentN. prefix
	// since the Prefix function wipes the parent prefix, we need to add the whole Attacker.EquipmentN. in this class
	public ShipExportMap(string prefix)
	{
		Map(m => m.Index).Name(CsvExportResources.Index);
		Map(m => m.Id).Name(CsvExportResources.Id);
		Map(m => m.Name).Name(CsvExportResources.Name);
		Map(m => m.ShipType).Name(CsvExportResources.ShipType);
		Map(m => m.Condition).Name(CsvExportResources.Condition);
		Map(m => m.HpCurrent).Name(CsvExportResources.HpCurrent);
		Map(m => m.HpMax).Name(CsvExportResources.HpMax);
		Map(m => m.DamageState).Name(CsvExportResources.DamageState);
		Map(m => m.FuelCurrent).Name(CsvExportResources.FuelCurrent);
		Map(m => m.FuelMax).Name(CsvExportResources.FuelMax);
		Map(m => m.AmmoCurrent).Name(CsvExportResources.AmmoCurrent);
		Map(m => m.AmmoMax).Name(CsvExportResources.AmmoMax);
		Map(m => m.Level).Name(CsvExportResources.ShipLevel);
		Map(m => m.Speed).Name(CsvExportResources.Speed);
		Map(m => m.Firepower).Name(CsvExportResources.Firepower);
		Map(m => m.Torpedo).Name(CsvExportResources.Torpedo);
		Map(m => m.AntiAir).Name(CsvExportResources.AntiAir);
		Map(m => m.Armor).Name(CsvExportResources.Armor);
		Map(m => m.Evasion).Name(CsvExportResources.Evasion);
		Map(m => m.AntiSubmarine).Name(CsvExportResources.AntiSubmarine);
		Map(m => m.Search).Name(CsvExportResources.Search);
		Map(m => m.Luck).Name(CsvExportResources.Luck);
		Map(m => m.Range).Name(CsvExportResources.Range);
		References<EquipmentExportMap>(s => s.Equipment1).Prefix($"{prefix}{CsvExportResources.PrefixEquipment1}");
		References<EquipmentExportMap>(s => s.Equipment2).Prefix($"{prefix}{CsvExportResources.PrefixEquipment2}");
		References<EquipmentExportMap>(s => s.Equipment3).Prefix($"{prefix}{CsvExportResources.PrefixEquipment3}");
		References<EquipmentExportMap>(s => s.Equipment4).Prefix($"{prefix}{CsvExportResources.PrefixEquipment4}");
		References<EquipmentExportMap>(s => s.Equipment5).Prefix($"{prefix}{CsvExportResources.PrefixEquipment5}");
		References<EquipmentExportMap>(s => s.Equipment6).Prefix($"{prefix}{CsvExportResources.PrefixEquipment6}");
	}
}
