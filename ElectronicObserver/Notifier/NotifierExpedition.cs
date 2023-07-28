using System;
using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 遠征帰投通知を扱います。
/// </summary>
public class NotifierExpedition : NotifierBase
{
	private Dictionary<int, bool> ProcessedFlags { get; } = new();

	public NotifierExpedition(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.ExpeditionTitle;
	}


	protected override void UpdateTimerTick()
	{

		foreach (var fleet in KCDatabase.Instance.Fleet.Fleets.Values)
		{

			if (!ProcessedFlags.ContainsKey(fleet.FleetID))
				ProcessedFlags.Add(fleet.FleetID, false);

			if (fleet.ExpeditionState != 0)
			{
				if (!ProcessedFlags[fleet.FleetID] && (int)(fleet.ExpeditionTime - DateTime.Now).TotalMilliseconds <= AccelInterval)
				{

					ProcessedFlags[fleet.FleetID] = true;
					Notify(fleet.FleetID, fleet.ExpeditionDestination);

				}

			}
			else
			{
				ProcessedFlags[fleet.FleetID] = false;
			}

		}

	}

	private void Notify(int fleetID, int destination)
	{

		DialogData.Message = string.Format(NotifierRes.ExpeditionText,
			fleetID, KCDatabase.Instance.Fleet[fleetID].Name, KCDatabase.Instance.Mission[destination].DisplayID, KCDatabase.Instance.Mission[destination].NameEN);

		base.Notify();
	}
}
