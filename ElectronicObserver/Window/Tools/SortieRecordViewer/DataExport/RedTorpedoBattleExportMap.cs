﻿using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class RedTorpedoBattleExportMap : ClassMap<TorpedoBattleExportModel>
{
	public RedTorpedoBattleExportMap()
	{
		References<CommonDataExportMap>(s => s.CommonData);
		Map(m => m.BattleType).Name(CsvExportResources.BattleType);
		Map(m => m.PlayerFleetType).Name(CsvExportResources.PlayerFleetType);
		Map(m => m.BattlePhase).Name(CsvExportResources.TorpedoBattlePhase);
		Map(m => m.AttackerSide).Name(CsvExportResources.AttackerSide);
		Map(m => m.AttackType).Name(CsvExportResources.TorpedoAttackType);
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
