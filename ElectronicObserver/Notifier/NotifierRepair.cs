using System;
using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 入渠完了通知を扱います。
/// </summary>
public class NotifierRepair : NotifierBase
{
	private Dictionary<int, bool> ProcessedFlags { get; } = new();

	public NotifierRepair(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		DialogData.Title = NotifierRes.RepairTitle;
	}

	protected override void UpdateTimerTick()
	{

		foreach (var dock in KCDatabase.Instance.Docks.Values)
		{

			if (!ProcessedFlags.ContainsKey(dock.DockID))
				ProcessedFlags.Add(dock.DockID, false);

			if (dock.State > 0)
			{
				if (!ProcessedFlags[dock.DockID] && (int)(dock.CompletionTime - DateTime.Now).TotalMilliseconds <= AccelInterval)
				{

					ProcessedFlags[dock.DockID] = true;
					Notify(dock.DockID, dock.ShipID);

				}

			}
			else
			{
				ProcessedFlags[dock.DockID] = false;
			}

		}

	}

	private void Notify(int dockID, int shipID)
	{

		DialogData.Message = string.Format(NotifierRes.RepairText,
			dockID, KCDatabase.Instance.Ships[shipID].NameWithLevel);

		base.Notify();
	}

}
