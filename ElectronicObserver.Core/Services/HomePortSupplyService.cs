using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Core.Services;

/// <summary>
/// <see href="https://twitter.com/yukicacoon/status/2000948349122543813" /> <br />
/// <see href="https://twitter.com/CC_jabberwock/status/2001326121879835017" /> <br />
/// <see href="https://docs.google.com/spreadsheets/d/122yojowa_LEaNMpcxf5LNkWW_kFk7DwCEaJbIk4xbds" />
/// </summary>
public class HomePortSupplyService
{
	public static int MaxSupplyCondition => 54;

	/// <summary>
	/// 母港給糧タイマー
	/// </summary>
	public DateTime HomePortSupplyTimer { get; private set; }

	private static IEnumerable<IShipData?> HomePortSupplyShips(IFleetData fleet) => fleet.MembersInstance
		?.Take(2)
		.Where(IsHomePortSupplyShip)
		?? [];

	public static bool IsHomePortSupplyFleet(IFleetData fleet) => HomePortSupplyShips(fleet).Any();

	public static bool CanHomePortSupply(IFleetData fleet)
		=> fleet.ExpeditionState is ExpeditionState.NotDeployed
		&& fleet.MembersInstance
			.OfType<IShipData>()
			.Where(s => !IsHomePortSupplyShip(s))
			.Where(s => s.RepairingDockID is -1)
			.Any(s => s.Condition < MaxSupplyCondition)
		&& HomePortSupplyShips(fleet)
			.Any(s => s is
			{
				Condition: >= 30,
				FuelRate: 1,
				AmmoRate: 1,
				RepairingDockID: -1,
				DamageState: > DamageState.Light,
			});

	private static bool IsHomePortSupplyShip(IShipData? ship)
		=> ship?.MasterShip.ShipId is ShipId.Nosaki or ShipId.NosakiKai;

	/// <summary>
	/// Should be called before api_req_hensei/change request gets processed.
	/// </summary>
	/// <param name="fleet"></param>
	/// <param name="fleetId">api_id</param>
	/// <param name="shipIndex">api_ship_idx</param>
	/// <param name="movedShipDropId">
	/// api_ship_id <br />
	/// -1: Remove ship <br />
	/// -2: Remove all ships <br />
	/// int : Drop id of ship that was moved
	/// </param>
	/// <param name="movedShip">The ship according to api_ship_id, will be null in -1 and -2 cases.</param>
	/// <param name="replacedShip">The ship according to replaced_id.</param>
	public void FleetUpdated(IFleetData fleet, int fleetId, int shipIndex, int movedShipDropId, IShipData? movedShip, IShipData? replacedShip)
	{
		if (fleet.FleetID == fleetId)
		{
			if (movedShipDropId is -2) return;

			movedShip = movedShipDropId switch
			{
				> 0 => movedShip,
				_ => fleet.MembersInstance.Skip(shipIndex).FirstOrDefault(),
			};

			if (IsHomePortSupplyShip(movedShip))
			{
				if (shipIndex > 1) return;
				if (movedShipDropId is -1) return;
			}
			else
			{
				if (!IsHomePortSupplyFleet(fleet)) return;
			}
		}
		else
		{
			if (movedShipDropId < 1) return;
			if (replacedShip is null) return;
			if (fleet.Members is null) return;
			if (!fleet.Members.Contains(movedShipDropId)) return;

			if (IsHomePortSupplyShip(replacedShip))
			{
				if (fleet.Members.IndexOf(movedShipDropId) > 1) return;
			}
		}

		ResetTimer();
	}

	public void EnterPort()
	{
		if ((DateTime.Now - HomePortSupplyTimer).TotalMinutes < 15) return;

		TriggerHomePortSupply();
	}

	private void ResetTimer()
	{
		HomePortSupplyTimer = DateTime.Now;
	}

	public void TriggerHomePortSupply()
	{
		// notify home port supply triggered?
		ResetTimer();
	}

	/// <summary>
	/// Should only be used for testing.
	/// </summary>
	public void SetTimer(DateTime time)
	{
		HomePortSupplyTimer = time;
	}
}
