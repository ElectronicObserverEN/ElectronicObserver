using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBattleExportMap : ClassMap<AirBattleExportModel>
{
	public AirBattleExportMap()
	{
		References<CommonDataExportMap>(s => s.CommonData);
		References<AirBattleStageExportMap>(s => s.Stage1).Prefix(CsvExportResources.PrefixStage1);
		References<AirBattleStageExportMap>(s => s.Stage2).Prefix(CsvExportResources.PrefixStage2);
		References<AntiAirCutInExportMap>(s => s.AntiAirCutIn).Prefix(CsvExportResources.PrefixAntiAirCutIn);
		References<AirBattleShipExportMap>(s => s.Attacker1).Prefix(CsvExportResources.PrefixAttacker1);
		References<AirBattleShipExportMap>(s => s.Attacker2).Prefix(CsvExportResources.PrefixAttacker2);
		References<AirBattleShipExportMap>(s => s.Attacker3).Prefix(CsvExportResources.PrefixAttacker3);
		References<AirBattleShipExportMap>(s => s.Attacker4).Prefix(CsvExportResources.PrefixAttacker4);
		References<AirBattleShipExportMap>(s => s.Attacker5).Prefix(CsvExportResources.PrefixAttacker5);
		References<AirBattleShipExportMap>(s => s.Attacker6).Prefix(CsvExportResources.PrefixAttacker6);
		References<AirBattleShipExportMap>(s => s.Attacker7).Prefix(CsvExportResources.PrefixAttacker7);
		Map(m => m.TorpedoFlag).Name(CsvExportResources.TorpedoFlag);
		Map(m => m.BomberFlag).Name(CsvExportResources.BomberFlag);
		Map(m => m.HitType).Name(CsvExportResources.HitType);
		Map(m => m.Damage).Name(CsvExportResources.Damage);
		Map(m => m.Protected).Name(CsvExportResources.Protected);
		References<ShipExportMap>(s => s.Defender, CsvExportResources.PrefixDefender).Prefix(CsvExportResources.PrefixDefender);
		Map(m => m.SmokeType).Name(CsvExportResources.SmokeType);
		References<BalloonExportMap>(s => s.Balloon);
		Map(m => m.ArmorBreak).Name(CsvExportResources.ArmorBreak);
	}
}
