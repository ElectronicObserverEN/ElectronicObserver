using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Notifier;

/// <summary>
/// 基地航空隊出撃通知を扱います。
/// </summary>
public class NotifierBaseAirCorps : NotifierBase
{
	// todo: this isn't true anymore, upgraded AB has shorter relocation time
	private static TimeSpan RelocationSpan => TimeSpan.FromMinutes(12);

	/// <summary>
	/// 未補給時に通知する
	/// </summary>
	public bool NotifiesNotSupplied { get; set; }

	/// <summary>
	/// 疲労時に通知する
	/// </summary>
	public bool NotifiesTired { get; set; }

	/// <summary>
	/// 編成されていないときに通知する
	/// </summary>
	public bool NotifiesNotOrganized { get; set; }


	/// <summary>
	/// 待機のとき通知する
	/// </summary>
	public bool NotifiesStandby { get; set; }

	/// <summary>
	/// 退避の時通知する
	/// </summary>
	public bool NotifiesRetreat { get; set; }

	/// <summary>
	/// 休息の時通知する
	/// </summary>
	public bool NotifiesRest { get; set; }


	/// <summary>
	/// 通常海域で通知する
	/// </summary>
	public bool NotifiesNormalMap { get; set; }

	/// <summary>
	/// イベント海域で通知する
	/// </summary>
	public bool NotifiesEventMap { get; set; }


	/// <summary>
	/// 基地枠の配置転換完了時に通知する
	/// </summary>
	public bool NotifiesSquadronRelocation { get; set; }

	/// <summary>
	/// 装備の配置転換完了時に通知する
	/// </summary>
	public bool NotifiesEquipmentRelocation { get; set; }


	// supress when sortieing

	private bool IsAlreadyNotified { get; set; }
	private bool IsInSortie { get; set; }
	private HashSet<int> NotifiedEquipments { get; } = new();



	public NotifierBaseAirCorps(Configuration.ConfigurationData.ConfigNotifierBaseAirCorps config)
		: base(config)
	{
		DialogData.Title = NotifierBaseAirCorpsResources.Title;

		SubscribeToApis();

		NotifiesNotSupplied = config.NotifiesNotSupplied;
		NotifiesTired = config.NotifiesTired;
		NotifiesNotOrganized = config.NotifiesNotOrganized;

		NotifiesStandby = config.NotifiesStandby;
		NotifiesRetreat = config.NotifiesRetreat;
		NotifiesRest = config.NotifiesRest;

		NotifiesNormalMap = config.NotifiesNormalMap;
		NotifiesEventMap = config.NotifiesEventMap;

		NotifiesSquadronRelocation = config.NotifiesSquadronRelocation;
		NotifiesEquipmentRelocation = config.NotifiesEquipmentRelocation;
	}

	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Port;
		o.ApiGetMember_MapInfo.ResponseReceived += BeforeSortie;
		o.ApiGetMember_SortieConditions.ResponseReceived += BeforeSortieEventMap;
		o.ApiReqMap_Start.RequestReceived += Sally;
	}


	private void Port(string apiname, dynamic data)
	{
		IsAlreadyNotified = false;
		IsInSortie = false;
		NotifiedEquipments.Clear();
	}

	private void BeforeSortieEventMap(string apiname, dynamic data)
	{
		if (IsAlreadyNotified)
			return;

		if (!NotifiesEventMap)
			return;

		KCDatabase db = KCDatabase.Instance;
		CheckBaseAirCorps(db.BaseAirCorps.Values.Where(corps => db.MapArea[corps.MapAreaID].MapType == 1));
	}

	private void BeforeSortie(string apiname, dynamic data)
	{
		if (IsAlreadyNotified)
			return;

		if (!NotifiesNormalMap)
			return;

		KCDatabase db = KCDatabase.Instance;
		CheckBaseAirCorps(db.BaseAirCorps.Values.Where(corps => db.MapArea[corps.MapAreaID].MapType == 0));
	}


	private bool CheckBaseAirCorps(IEnumerable<BaseAirCorpsData> corpslist)
	{
		KCDatabase db = KCDatabase.Instance;
		StringBuilder sb = new StringBuilder();
		var messages = new LinkedList<string>();

		foreach (BaseAirCorpsData corps in corpslist)
		{
			if (NotifiesNotSupplied && corps.Squadrons.Values.Any(sq => sq.State == 1 && sq.AircraftCurrent < sq.AircraftMax))
				messages.AddLast(FleetRes.SupplyNeeded);
			if (NotifiesTired && corps.Squadrons.Values.Any(sq => sq is { State: 1, Condition: > AirBaseCondition.Normal }))
				messages.AddLast(FleetRes.Fatigued);
			if (NotifiesNotOrganized)
			{
				if (corps.Squadrons.Values.Any(sq => sq.State == 0))
					messages.AddLast(NotifierBaseAirCorpsResources.Unorganized);
				if (corps.Squadrons.Values.Any(sq => sq.State == 2))
					messages.AddLast(GeneralRes.BaseRedeployment);
			}

			if (NotifiesStandby && corps.ActionKind == AirBaseActionKind.Standby)
				messages.AddLast(NotifierBaseAirCorpsResources.Standby);
			if (NotifiesRetreat && corps.ActionKind == AirBaseActionKind.TakeCover)
				messages.AddLast(GeneralRes.Retreating);
			if (NotifiesRest && corps.ActionKind == AirBaseActionKind.Rest)
				messages.AddLast(NotifierBaseAirCorpsResources.Resting);

			if (messages.Any())
			{
				if (sb.Length == 0)
				{
					sb.Append(NotifierBaseAirCorpsResources.NotReadyToSortie);
				}
				sb.Append($"#{corps.MapAreaID} {corps.Name} ({string.Join(", ", messages)})");
			}
			messages.Clear();
		}

		if (sb.Length > 0)
		{
			Notify(sb.ToString());
			IsAlreadyNotified = true;
			return true;
		}
		return false;
	}

	private void Sally(string apiname, dynamic data)
	{
		IsInSortie = true;
	}


	protected override void UpdateTimerTick()
	{
		KCDatabase db = KCDatabase.Instance;

		if (!db.RelocatedEquipments.Any())
			return;

		if (IsInSortie)
			return;

		if (NotifiesSquadronRelocation)
		{
			StringBuilder sb = null;
			foreach (var corps in db.BaseAirCorps.Values.Where(corps =>
				(NotifiesNormalMap && db.MapArea[corps.MapAreaID].MapType == 0) ||
				(NotifiesEventMap && db.MapArea[corps.MapAreaID].MapType == 1)))
			{
				var targetSquadrons = corps.Squadrons.Values
					.Where(sq => sq.State == 2 &&
								 !NotifiedEquipments.Contains(sq.EquipmentID) &&
								 (DateTime.Now - sq.RelocatedTime) >= RelocationSpan)
					.ToList();

				if (targetSquadrons.Any())
				{
					sb = sb?.Append(", ") ?? new StringBuilder();

					sb.Append(string.Join(", ", targetSquadrons.Select(sq =>
						$"#{corps.MapAreaID} {corps.Name} squad {sq.SquadronID} ({sq.EquipmentInstance.NameWithLevel})")));

					foreach (var sq in targetSquadrons)
						NotifiedEquipments.Add(sq.EquipmentID);
				}
			}

			if (sb != null)
			{
				Notify($"{sb} {NotifierBaseAirCorpsResources.RelocationCompleted}");
			}
		}

		if (NotifiesEquipmentRelocation)
		{
			var targets = db.RelocatedEquipments
				.Where(kv => !NotifiedEquipments.Contains(kv.Key) &&
							 (DateTime.Now - kv.Value.RelocatedTime) >= RelocationSpan)
				.ToList();

			if (targets.Any())
			{
				Notify($"{string.Join(", ", targets.Select(kv => kv.Value.EquipmentInstance.NameWithLevel))} の配置転換が完了しました。母港に入り直すと更新されます。");

				foreach (var t in targets)
					NotifiedEquipments.Add(t.Key);
			}
		}


	}

	private void Notify(string message)
	{
		DialogData.Title = NotifierBaseAirCorpsResources.Title;
		DialogData.Message = message;

		base.Notify();
	}

	public override void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{
		base.ApplyToConfiguration(config);

		if (config is Configuration.ConfigurationData.ConfigNotifierBaseAirCorps c)
		{
			c.NotifiesNotSupplied = NotifiesNotSupplied;
			c.NotifiesTired = NotifiesTired;
			c.NotifiesNotOrganized = NotifiesNotOrganized;

			c.NotifiesStandby = NotifiesStandby;
			c.NotifiesRetreat = NotifiesRetreat;
			c.NotifiesRest = NotifiesRest;

			c.NotifiesNormalMap = NotifiesNormalMap;
			c.NotifiesEventMap = NotifiesEventMap;

			c.NotifiesSquadronRelocation = NotifiesSquadronRelocation;
			c.NotifiesEquipmentRelocation = NotifiesEquipmentRelocation;
		}
	}
}
