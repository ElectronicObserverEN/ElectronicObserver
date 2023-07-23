using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseFriendlySupportInfo : PhaseBase
{
	public override string Title => BattleRes.FriendlyFleet;

	private List<IShipData?> Ships { get; } = new();

	public string Display => CreateDisplay();

	public PhaseFriendlySupportInfo(ApiFriendlyInfo apiFriendlyInfo)
	{
		int count = apiFriendlyInfo.ApiShipId.Count;
		count = Math.Min(count, apiFriendlyInfo.ApiShipLv.Count);
		count = Math.Min(count, apiFriendlyInfo.ApiSlot.Count);
		count = Math.Min(count, apiFriendlyInfo.ApiSlotEx.Count);
		count = Math.Min(count, apiFriendlyInfo.ApiNowhps.Count);
		count = Math.Min(count, apiFriendlyInfo.ApiMaxhps.Count);
		count = Math.Min(count, apiFriendlyInfo.ApiParam.Count);

		for (int i = 0; i < count; i++)
		{
			// this should be impossible
			if (apiFriendlyInfo.ApiShipId[i] <= 0)
			{
				Ships.Add(null);
				continue;
			}

			ShipDataMock ship = new(KCDatabase.Instance.MasterShips[(int)apiFriendlyInfo.ApiShipId[i]])
			{
				Level = apiFriendlyInfo.ApiShipLv[i],
				HPCurrent = apiFriendlyInfo.ApiNowhps[i],
				SlotInstance = apiFriendlyInfo.ApiSlot[i]
					.Append(apiFriendlyInfo.ApiSlotEx[i])
					.Select(id => id switch
					{
						> 0 => new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)id]),
						_ => null,
					})
					.Cast<IEquipmentData?>()
					.ToList(),
			};

			ship.FirepowerModernized += apiFriendlyInfo.ApiParam[i][0] - ship.FirepowerBase;
			ship.TorpedoModernized += apiFriendlyInfo.ApiParam[i][1] - ship.TorpedoBase;
			ship.AAModernized += apiFriendlyInfo.ApiParam[i][2] - ship.AABase;
			ship.ArmorModernized += apiFriendlyInfo.ApiParam[i][3] - ship.ArmorBase;
			
			Ships.Add(ship);
		}
	}

	public override BattleFleets EmulateBattle(BattleFleets battleFleets)
	{
		FleetsBeforePhase = battleFleets.Clone();

		battleFleets.FriendFleet = new FleetDataMock
		{
			MembersInstance = new(Ships),
		};

		FleetsAfterPhase = battleFleets.Clone();

		return battleFleets;
	}

	private string CreateDisplay()
	{
		StringBuilder sb = new();

		for (int i = 0; i < Ships.Count; i++)
		{
			IShipData? ship = Ships[i];

			if (ship is null) continue;

			sb.AppendFormat($"#{{0}}: {{1}} {{2}} " +
							$"Lv. {{3}} " +
							$"HP: {{4}} / {{5}} - " +
							$"{GeneralRes.Firepower} {{6}}, " +
							$"{GeneralRes.Torpedo} {{7}}, " +
							$"{GeneralRes.AntiAir} {{8}}, " +
							$"{GeneralRes.Armor} {{9}}" +
							$"\r\n",
				i + 1,
				ship.MasterShip.ShipTypeName, ship.MasterShip.NameWithClass,
				ship.Level,
				ship.HPCurrent, ship.HPMax,
				ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase);

			sb.Append('　');
			sb.AppendLine(string.Join(", ", ship.AllSlotInstance
				.Where(eq => eq != null)
				.Select(eq => eq!.Name)));
		}

		return sb.ToString();
	}
}
