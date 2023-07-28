using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 疲労回復通知を扱います。
/// </summary>
public class NotifierCondition : NotifierBase
{
	private Dictionary<int, bool> ProcessedFlags { get; } = new();

	public NotifierCondition(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.ConditionTitle;

		for (int i = 1; i <= 4; i++)
		{
			ProcessedFlags.Add(i, false);
		}

		SubscribeToApis();
	}


	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += ClearFlags;
	}


	private void ClearFlags(string apiname, dynamic data)
	{

		foreach (int key in ProcessedFlags.Keys.ToArray())
		{       //列挙中の変更によるエラーを防ぐため
			ProcessedFlags[key] = false;
		}
	}


	protected override void UpdateTimerTick()
	{

		foreach (var fleet in KCDatabase.Instance.Fleet.Fleets.Values)
		{

			if (fleet.ExpeditionState > 0 || fleet.IsInSortie) continue;

			if (ProcessedFlags[fleet.FleetID])
				continue;


			if (fleet.ConditionTime != null && !fleet.IsInSortie)
			{

				if (((DateTime)fleet.ConditionTime - DateTime.Now).TotalMilliseconds <= AccelInterval)
				{

					Notify(fleet.FleetID);
					ProcessedFlags[fleet.FleetID] = true;
				}
			}
		}

	}

	private void Notify(int fleetID)
	{

		DialogData.Message = string.Format(NotifierRes.ConditionText,
			fleetID, KCDatabase.Instance.Fleet[fleetID].Name);

		base.Notify();
	}

}
