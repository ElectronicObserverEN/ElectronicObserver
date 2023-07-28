using System;
using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 建造完了通知を扱います。
/// </summary>
public class NotifierConstruction : NotifierBase
{
	private Dictionary<int, bool> ProcessedFlags { get; } = new();

	public NotifierConstruction(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.ConstructionTitle;
	}


	protected override void UpdateTimerTick()
	{

		foreach (var arsenal in KCDatabase.Instance.Arsenals.Values)
		{

			if (!ProcessedFlags.ContainsKey(arsenal.ArsenalID))
				ProcessedFlags.Add(arsenal.ArsenalID, false);

			if (arsenal.State > 0)
			{
				if (!ProcessedFlags[arsenal.ArsenalID] && (
					(int)(arsenal.CompletionTime - DateTime.Now).TotalMilliseconds <= AccelInterval ||
					arsenal.State == 3))
				{

					ProcessedFlags[arsenal.ArsenalID] = true;
					Notify(arsenal.ArsenalID, arsenal.ShipID);
				}

			}
			else
			{
				ProcessedFlags[arsenal.ArsenalID] = false;
			}

		}

	}

	private void Notify(int arsenalID, int shipID)
	{

		DialogData.Message = string.Format(NotifierRes.ConstructionText,
			arsenalID, Utility.Configuration.Config.FormArsenal.ShowShipName ? KCDatabase.Instance.MasterShips[shipID].NameWithClass : "艦娘");

		base.Notify();
	}
}
