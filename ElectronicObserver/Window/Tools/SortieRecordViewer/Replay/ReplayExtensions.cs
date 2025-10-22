using System;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Database.Sortie;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public static class ReplayExtensions
{
	public static ReplayData ToReplayData(this SortieRecord sortie) => new()
	{
		Id = 1,
		Support1 = sortie.FleetData.NodeSupportFleetId,
		Support2 = sortie.FleetData.BossSupportFleetId,
		Combined = sortie.FleetData.CombinedFlag,
		DefeatCount = sortie.MapData.RequiredDefeatedCount switch
		{
			-1 => 0,
			_ => sortie.MapData.RequiredDefeatedCount,
		},
		NowMaphp = sortie.MapData.MapHPMax switch
		{
			> 0 => sortie.MapData.MapHPCurrent,
			_ => 0,
		},
		MaxMaphp = sortie.MapData.MapHPMax switch
		{
			> 0 => sortie.MapData.MapHPMax,
			_ => 0,
		},
		Fleetnum = sortie.FleetData.FleetId,
		World = sortie.World,
		Mapnum = sortie.Map,
		Fleet1 = sortie.FleetData.Fleets.Skip(0).FirstOrDefault()?.Ships
			.Select(s => new ReplayShip
			{
				ShipId = s.Id,
				Level = s.Level,
				Morale = s.Condition,
				Kyouka = s.Kyouka,
				Equip = s.EquipmentSlots.Append(s.ExpansionSlot).Select(e => ((int?)e?.Equipment?.Id) ?? 0).ToList(),
			}).ToList() ?? new(),
		Fleet2 = sortie.FleetData.Fleets.Skip(1).FirstOrDefault()?.Ships
			.Select(s => new ReplayShip
			{
				ShipId = s.Id,
				Level = s.Level,
				Morale = s.Condition,
				Kyouka = s.Kyouka,
				Equip = s.EquipmentSlots.Append(s.ExpansionSlot).Select(e => ((int?)e?.Equipment?.Id) ?? 0).ToList(),
			}).ToList() ?? new(),
		Fleet3 = sortie.FleetData.Fleets.Skip(2).FirstOrDefault()?.Ships
			.Select(s => new ReplayShip
			{
				ShipId = s.Id,
				Level = s.Level,
				Morale = s.Condition,
				Kyouka = s.Kyouka,
				Equip = s.EquipmentSlots.Append(s.ExpansionSlot).Select(e => ((int?)e?.Equipment?.Id) ?? 0).ToList(),
			}).ToList() ?? new(),
		Fleet4 = sortie.FleetData.Fleets.Skip(3).FirstOrDefault()?.Ships
			.Select(s => new ReplayShip
			{
				ShipId = s.Id,
				Level = s.Level,
				Morale = s.Condition,
				Kyouka = s.Kyouka,
				Equip = s.EquipmentSlots.Append(s.ExpansionSlot).Select(e => ((int?)e?.Equipment?.Id) ?? 0).ToList(),
			}).ToList() ?? new(),
		AirBases = sortie.FleetData.AirBases
			.Where(b => b.MapAreaId == sortie.Map)
			.Select(b => new ReplayAirBase
			{
				Rid = b.AirCorpsId,
				Action = b.ActionKind,
				Range = new()
				{
					ApiBase = b.BaseDistance,
					ApiBonus = b.BonusDistance,
				},
				Planes = b.Squadrons
					.Select(s => s switch
					{
						{ } => new ReplayAirBaseSquadron
						{
							EquipmentId = s.EquipmentSlot.Equipment?.Id ?? EquipmentId.Unknown,
							State = s.State,
							Count = s.EquipmentSlot.AircraftCurrent,
							Morale = s.Condition,
							Stars = s.EquipmentSlot.Equipment?.Level ?? 0,
							Ace = s.EquipmentSlot.Equipment?.AircraftLevel ?? 0,
						},
						_ => throw new NotFiniteNumberException(),
					}).ToList(),
			}).ToList(),
		Battles = new(),
	};
}
