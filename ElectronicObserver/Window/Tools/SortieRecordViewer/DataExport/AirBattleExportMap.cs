namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBattleExportMap : CsvExportMap<AirBattleExportModel>
{
	public AirBattleExportMap()
	{
		References<CommonDataExportMap, CommonDataExportModel>(s => s.CommonData);
		References<AirBattleStageExportMap, AirBattleStageExportModel>(s => s.Stage1, m => m.Prefix(CsvExportResources.PrefixStage1));
		References<AirBattleStageExportMap, AirBattleStageExportModel>(s => s.Stage2, m => m.Prefix(CsvExportResources.PrefixStage2));
		References<AntiAirCutInExportMap, AntiAirCutInExportModel>(s => s.AntiAirCutIn, m => m.Prefix(CsvExportResources.PrefixAntiAirCutIn));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker1, m => m.Prefix(CsvExportResources.PrefixAttacker1));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker2, m => m.Prefix(CsvExportResources.PrefixAttacker2));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker3, m => m.Prefix(CsvExportResources.PrefixAttacker3));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker4, m => m.Prefix(CsvExportResources.PrefixAttacker4));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker5, m => m.Prefix(CsvExportResources.PrefixAttacker5));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker6, m => m.Prefix(CsvExportResources.PrefixAttacker6));
		References<AirBattleShipExportMap, AirBattleShipExportModel>(s => s.Attacker7, m => m.Prefix(CsvExportResources.PrefixAttacker7));
		Map(m => m.TorpedoFlag).Name(() => CsvExportResources.TorpedoFlag);
		Map(m => m.BomberFlag).Name(() => CsvExportResources.BomberFlag);
		Map(m => m.HitType).Name(() => CsvExportResources.HitType);
		Map(m => m.Damage).Name(() => CsvExportResources.Damage);
		Map(m => m.Protected).Name(() => CsvExportResources.Protected);
		References<ShipExportMap, ShipExportModel>(s => s.Defender, m => m.Prefix(CsvExportResources.PrefixDefender));
		Map(m => m.SmokeType).Name(() => CsvExportResources.SmokeType);
		References<BalloonExportMap, BalloonExportModel>(s => s.Balloon);
		Map(m => m.ArmorBreak).Name(() => CsvExportResources.ArmorBreak);
	}
}
