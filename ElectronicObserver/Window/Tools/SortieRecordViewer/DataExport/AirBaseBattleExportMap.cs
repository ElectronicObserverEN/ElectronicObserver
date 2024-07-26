using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseBattleExportMap : ClassMap<AirBaseBattleExportModel>
{
	public AirBaseBattleExportMap()
	{
		References<CommonDataExportMap>(s => s.CommonData);
		Map(m => m.SquadronId).Name(CsvExportResources.SquadronId);
		Map(m => m.SquadronAttackIndex).Name(CsvExportResources.SquadronAttackIndex);
		Map(m => m.AirBasePlayerContact).Name(CsvExportResources.AirBasePlayerContact);
		Map(m => m.AirBaseEnemyContact).Name(CsvExportResources.AirBaseEnemyContact);
		Map(m => m.AirBaseSquadron1EquipmentName).Name(CsvExportResources.AirBaseSquadron1EquipmentName);
		Map(m => m.AirBaseSquadron1Aircraft).Name(CsvExportResources.AirBaseSquadron1Aircraft);
		Map(m => m.AirBaseSquadron2EquipmentName).Name(CsvExportResources.AirBaseSquadron2EquipmentName);
		Map(m => m.AirBaseSquadron2Aircraft).Name(CsvExportResources.AirBaseSquadron2Aircraft);
		Map(m => m.AirBaseSquadron3EquipmentName).Name(CsvExportResources.AirBaseSquadron3EquipmentName);
		Map(m => m.AirBaseSquadron3Aircraft).Name(CsvExportResources.AirBaseSquadron3Aircraft);
		Map(m => m.AirBaseSquadron4EquipmentName).Name(CsvExportResources.AirBaseSquadron4EquipmentName);
		Map(m => m.AirBaseSquadron4Aircraft).Name(CsvExportResources.AirBaseSquadron4Aircraft);
		References<AirBattleStageExportMap>(s => s.Stage1).Prefix(CsvExportResources.PrefixStage1);
		References<AirBattleStageExportMap>(s => s.Stage2).Prefix(CsvExportResources.PrefixStage2);
		References<AirBattleShipExportMap>(s => s.Attacker1).Prefix(CsvExportResources.PrefixAttacker1);
		References<AirBattleShipExportMap>(s => s.Attacker2).Prefix(CsvExportResources.PrefixAttacker2);
		References<AirBattleShipExportMap>(s => s.Attacker3).Prefix(CsvExportResources.PrefixAttacker3);
		References<AirBattleShipExportMap>(s => s.Attacker4).Prefix(CsvExportResources.PrefixAttacker4);
		References<AirBattleShipExportMap>(s => s.Attacker5).Prefix(CsvExportResources.PrefixAttacker5);
		References<AirBattleShipExportMap>(s => s.Attacker6).Prefix(CsvExportResources.PrefixAttacker6);
		References<AirBattleShipExportMap>(s => s.Attacker7).Prefix(CsvExportResources.PrefixAttacker7);
		Map(m => m.TotalTorpedoFlags).Name(CsvExportResources.TotalTorpedoFlags);
		Map(m => m.TotalBomberFlags).Name(CsvExportResources.TotalBomberFlags);
		Map(m => m.TorpedoFlag).Name(CsvExportResources.TorpedoFlag);
		Map(m => m.BomberFlag).Name(CsvExportResources.BomberFlag);
		Map(m => m.HitType).Name(CsvExportResources.HitType);
		Map(m => m.Damage).Name(CsvExportResources.Damage);
		Map(m => m.Protected).Name(CsvExportResources.Protected);
		References<ShipExportMap>(s => s.Defender, CsvExportResources.PrefixDefender).Prefix(CsvExportResources.PrefixDefender);
		Map(m => m.ArmorBreak).Name(CsvExportResources.ArmorBreak);
	}
}
