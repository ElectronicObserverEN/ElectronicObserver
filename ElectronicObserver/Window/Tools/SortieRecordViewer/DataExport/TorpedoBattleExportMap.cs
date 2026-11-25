namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class TorpedoBattleExportMap : CsvExportMap<TorpedoBattleExportModel>
{
	public TorpedoBattleExportMap()
	{
		References<CommonDataExportMap, CommonDataExportModel>(s => s.CommonData);
		Map(m => m.BattleType).Name(() => CsvExportResources.BattleType);
		Map(m => m.PlayerFleetType).Name(() => CsvExportResources.PlayerFleetType);
		Map(m => m.BattlePhase).Name(() => CsvExportResources.TorpedoBattlePhase);
		Map(m => m.AttackerSide).Name(() => CsvExportResources.AttackerSide);
		Map(m => m.AttackType).Name(() => CsvExportResources.TorpedoAttackType);
		Map(m => m.DisplayedEquipment1).Name(() => CsvExportResources.DisplayedEquipment1);
		Map(m => m.DisplayedEquipment2).Name(() => CsvExportResources.DisplayedEquipment2);
		Map(m => m.DisplayedEquipment3).Name(() => CsvExportResources.DisplayedEquipment3);
		Map(m => m.HitType).Name(() => CsvExportResources.HitType);
		Map(m => m.Damage).Name(() => CsvExportResources.Damage);
		Map(m => m.Protected).Name(() => CsvExportResources.Protected);
		References<ShipExportMap, ShipExportModel>(s => s.Attacker, m => m.Prefix(CsvExportResources.PrefixAttacker));
		References<ShipExportMap, ShipExportModel>(s => s.Defender, m => m.Prefix(CsvExportResources.PrefixDefender));
		Map(m => m.FleetType).Name(() => CsvExportResources.FleetType);
		Map(m => m.EnemyFleetType).Name(() => CsvExportResources.EnemyFleetType);
		Map(m => m.SmokeType).Name(() => CsvExportResources.SmokeType);
		References<BalloonExportMap, BalloonExportModel>(s => s.Balloon);
		Map(m => m.ArmorBreak).Name(() => CsvExportResources.ArmorBreak);
	}
}
