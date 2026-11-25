namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseBattleExportMap : CsvExportMap<AirBaseBattleExportModel>
{
	public AirBaseBattleExportMap()
	{
		References<CommonDataExportMap, CommonDataExportModel>(s => s.CommonData);
		Map(m => m.SquadronId).Name(() => CsvExportResources.SquadronId);
		Map(m => m.SquadronAttackIndex).Name(() => CsvExportResources.SquadronAttackIndex);
		Map(m => m.AirBasePlayerContact).Name(() => CsvExportResources.AirBasePlayerContact);
		Map(m => m.AirBaseEnemyContact).Name(() => CsvExportResources.AirBaseEnemyContact);
		Map(m => m.AirBaseSquadron1EquipmentName).Name(() => CsvExportResources.AirBaseSquadron1EquipmentName);
		Map(m => m.AirBaseSquadron1Aircraft).Name(() => CsvExportResources.AirBaseSquadron1Aircraft);
		Map(m => m.AirBaseSquadron2EquipmentName).Name(() => CsvExportResources.AirBaseSquadron2EquipmentName);
		Map(m => m.AirBaseSquadron2Aircraft).Name(() => CsvExportResources.AirBaseSquadron2Aircraft);
		Map(m => m.AirBaseSquadron3EquipmentName).Name(() => CsvExportResources.AirBaseSquadron3EquipmentName);
		Map(m => m.AirBaseSquadron3Aircraft).Name(() => CsvExportResources.AirBaseSquadron3Aircraft);
		Map(m => m.AirBaseSquadron4EquipmentName).Name(() => CsvExportResources.AirBaseSquadron4EquipmentName);
		Map(m => m.AirBaseSquadron4Aircraft).Name(() => CsvExportResources.AirBaseSquadron4Aircraft);
		References<AirBattleStageExportMap, AirBattleStageExportModel>(s => s.Stage1, m => m.Prefix(CsvExportResources.PrefixStage1));
		References<AirBattleStageExportMap, AirBattleStageExportModel>(s => s.Stage2, m => m.Prefix(CsvExportResources.PrefixStage2));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker1, m => m.Prefix(CsvExportResources.PrefixAttacker1));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker2, m => m.Prefix(CsvExportResources.PrefixAttacker2));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker3, m => m.Prefix(CsvExportResources.PrefixAttacker3));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker4, m => m.Prefix(CsvExportResources.PrefixAttacker4));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker5, m => m.Prefix(CsvExportResources.PrefixAttacker5));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker6, m => m.Prefix(CsvExportResources.PrefixAttacker6));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker7, m => m.Prefix(CsvExportResources.PrefixAttacker7));
		Map(m => m.TotalTorpedoFlags).Name(() => CsvExportResources.TotalTorpedoFlags);
		Map(m => m.TotalBomberFlags).Name(() => CsvExportResources.TotalBomberFlags);
		Map(m => m.TorpedoFlag).Name(() => CsvExportResources.TorpedoFlag);
		Map(m => m.BomberFlag).Name(() => CsvExportResources.BomberFlag);
		Map(m => m.HitType).Name(() => CsvExportResources.HitType);
		Map(m => m.Damage).Name(() => CsvExportResources.Damage);
		Map(m => m.Protected).Name(() => CsvExportResources.Protected);
		References<ShipExportMap, ShipExportModel>(s => s.Defender, m => m.Prefix(CsvExportResources.PrefixDefender));
		Map(m => m.ArmorBreak).Name(() => CsvExportResources.ArmorBreak);
	}
}
