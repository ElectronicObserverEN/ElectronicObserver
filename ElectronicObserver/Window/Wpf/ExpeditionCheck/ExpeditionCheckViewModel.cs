using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.ExpeditionCheck;

public class ExpeditionCheckViewModel : AnchorableViewModel
{
	public ExpeditionCheckTranslationViewModel ExpeditionCheckTranslation { get; }

	public ObservableCollection<ExpeditionCheckRow> Rows { get; } = new();

	public DataGridViewModel<ExpeditionCheckRow> DataGridViewModel { get; }

	public ExpeditionCheckViewModel() : base("ExpeditionCheck", "ExpeditionCheck", IconContent.FormExpeditionCheck)
	{
		ExpeditionCheckTranslation = Ioc.Default.GetService<ExpeditionCheckTranslationViewModel>()!;

		DataGridViewModel = new(Rows);

		Title = ExpeditionCheckTranslation.Title;
		ExpeditionCheckTranslation.PropertyChanged += (_, _) => Title = ExpeditionCheckTranslation.Title;

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
		KCDatabase db = KCDatabase.Instance;
		Rows.Clear();

		List<ExpeditionCheckRow> rows = db.Mission.Values
			.Select(mission => new ExpeditionCheckRow
			{
				AreaName = db.MapArea[mission.MapAreaID].NameEN,
				AreaId = mission.MapAreaID,

				ExpeditionId = mission.ID,
				ExpeditionDisplayId = mission.DisplayID,
				ExpeditionName = mission.NameEN,
				ExpeditionType = mission.ExpeditionType,

				Fleet1Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[1]),
				Fleet2Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[2]),
				Fleet3Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[3]),
				Fleet4Result = MissionClearCondition.Check(mission.MissionID, db.Fleet[4]),
				Conditions = MissionClearCondition.Check(mission.MissionID, null),
			})
			.OrderBy(m => m.AreaId)
			.ThenBy(m => m.ExpeditionId)
			.ToList();

		foreach (ExpeditionCheckRow row in rows)
		{
			Rows.Add(row);
		}
	}
}
