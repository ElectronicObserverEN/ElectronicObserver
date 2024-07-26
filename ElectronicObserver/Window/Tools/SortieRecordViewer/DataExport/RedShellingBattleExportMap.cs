using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class RedShellingBattleExportMap : ClassMap<ShellingBattleExportModel>
{
	public RedShellingBattleExportMap()
	{
		References<CommonDataExportMap>(s => s.CommonData);
		Map(m => m.BattleType).Name(CsvExportResources.BattleType);
		Map(m => m.ShipName1).Name(CsvExportResources.ShipName1);
		Map(m => m.ShipName2).Name(CsvExportResources.ShipName2);
		Map(m => m.ShipName3).Name(CsvExportResources.ShipName3);
		Map(m => m.ShipName4).Name(CsvExportResources.ShipName4);
		Map(m => m.ShipName5).Name(CsvExportResources.ShipName5);
		Map(m => m.ShipName6).Name(CsvExportResources.ShipName6);
		Map(m => m.PlayerFleetType).Name(CsvExportResources.PlayerFleetType);
		Map(m => m.BattlePhase).Name(CsvExportResources.BattlePhase);
		Map(m => m.AttackerSide).Name(CsvExportResources.AttackerSide);
		Map(m => m.AttackType).Name(CsvExportResources.AttackType);
		Map(m => m.AttackIndex).Name(CsvExportResources.AttackIndex);
		Map(m => m.DisplayedEquipment1).Name(CsvExportResources.DisplayedEquipment1);
		Map(m => m.DisplayedEquipment2).Name(CsvExportResources.DisplayedEquipment2);
		Map(m => m.DisplayedEquipment3).Name(CsvExportResources.DisplayedEquipment3);
		Map(m => m.HitType).Name(CsvExportResources.HitType);
		Map(m => m.Damage).Name(CsvExportResources.Damage);
		Map(m => m.Protected).Name(CsvExportResources.Protected);
		References<RedShipExportMap>(s => s.Attacker, CsvExportResources.PrefixAttacker).Prefix(CsvExportResources.PrefixAttacker);
		References<RedShipExportMap>(s => s.Defender, CsvExportResources.PrefixDefender).Prefix(CsvExportResources.PrefixDefender);
		Map(m => m.FleetType).Name(CsvExportResources.FleetType);
		Map(m => m.EnemyFleetType).Name(CsvExportResources.EnemyFleetType);
		Map(m => m.SmokeType).Name(CsvExportResources.SmokeType);
		References<BalloonExportMap>(s => s.Balloon);
		Map(m => m.ArmorBreak).Name(CsvExportResources.ArmorBreak);
	}
}
