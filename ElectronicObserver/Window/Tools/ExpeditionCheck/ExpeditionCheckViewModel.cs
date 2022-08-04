﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Tools.ExpeditionCheck;

public class ExpeditionCheckViewModel : AnchorableViewModel
{
	public ExpeditionCheckTranslationViewModel ExpeditionCheckTranslation { get; }

	public List<ExpeditionCheckRow> Rows { get; set; } = new();

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public ExpeditionCheckViewModel() : base(Ioc.Default.GetService<ExpeditionCheckTranslationViewModel>()!.Title, "ExpeditionCheck",
		ImageSourceIcons.GetIcon(IconContent.FormExpeditionCheck))
	{
		ExpeditionCheckTranslation = Ioc.Default.GetService<ExpeditionCheckTranslationViewModel>()!;

		LoadData();
		SubscribeToApis();
	}

	private void SubscribeToApis()
	{
		Utility.Configuration.Instance.ConfigurationChanged += LoadData;

		APIObserver o = APIObserver.Instance;

		o.ApiReqHensei_Change.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyShip.RequestReceived += Updated;
		o.ApiReqKaisou_Remodeling.RequestReceived += Updated;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_Ship2.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyShip.ResponseReceived += Updated;
		o.ApiGetMember_Ship3.ResponseReceived += Updated;
		o.ApiReqKaisou_PowerUp.ResponseReceived += Updated;
		o.ApiGetMember_SlotItem.ResponseReceived += Updated;
		o.ApiReqHensei_PresetSelect.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotExchangeIndex.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotDeprive.ResponseReceived += Updated;
		o.ApiReqKaisou_Marriage.ResponseReceived += Updated;
	}

	private void Updated(string apiname, dynamic data)
	{
		LoadData();
	}

	private void LoadData()
	{
		var db = KCDatabase.Instance;
		Rows.Clear();

		Rows = db.Mission.Values.Select(mission => new ExpeditionCheckRow()
		{
			AreaName = db.MapArea[mission.MapAreaID].NameEN,

			ExpeditionId = mission.DisplayID,
			ExpeditionName = mission.NameEN,
			ExpeditionType = mission.ExpeditionType,

			Fleet1Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[1]),
			Fleet2Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[2]),
			Fleet3Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[3]),
			Fleet4Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[4]),
			Conditions = MissionClearCondition.Check(mission.MissionID, null),
		})
			.ToList();
	}
}
