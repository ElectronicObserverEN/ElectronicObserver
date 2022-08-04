using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Behaviors.PersistentColumns;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.ExpeditionCheck;

public class ExpeditionCheckViewModel : WindowViewModelBase
{
	public ExpeditionCheckTranslationViewModel ExpeditionCheckTranslation { get; }

	public List<ExpeditionCheckRow> Rows { get; set; } = new();

	public List<ColumnProperties> ColumnProperties { get; set; } = new();
	public List<SortDescription> SortDescriptions { get; set; } = new();

	public ExpeditionCheckViewModel()
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

/*
 


		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormExpeditionCheck]);
	}


	private void UpdateCheckView()
	{
		CheckView.SuspendLayout();

		CheckView.Rows.Clear();

		var db = KCDatabase.Instance;
		var rows = new List<DataGridViewRow>(db.Mission.Count);

		var defaultStyle = CheckView.RowsDefaultCellStyle;
		var failedStyle = defaultStyle.Clone();
		failedStyle.BackColor = Color.MistyRose;
		failedStyle.SelectionBackColor = Color.Brown;


		foreach (var mission in db.Mission.Values)
		{
			var results = new[]
			{
				MissionClearCondition.Check(mission.MissionID, db.Fleet[1]),
				MissionClearCondition.Check(mission.MissionID, db.Fleet[2]),
				MissionClearCondition.Check(mission.MissionID, db.Fleet[3]),
				MissionClearCondition.Check(mission.MissionID, db.Fleet[4]),
				MissionClearCondition.Check(mission.MissionID, null),
			};


			var row = new DataGridViewRow();
			row.CreateCells(CheckView);
			row.SetValues(
				mission.MissionID,
				mission.MissionID,
				results[0],
				results[1],
				results[2],
				results[3],
				results[4]);

			row.Cells[1].ToolTipText = $"ID: {mission.MissionID} ({mission.ExpeditionType.Display()})";

			for (int i = 0; i < 5; i++)
			{
				var result = results[i];
				var cell = row.Cells[i + 2];

				if (result.IsSuceeded || i == 4)
				{
					if (!result.FailureReason.Any())
					{
						if (mission.ExpeditionType is ExpeditionType.CombatTypeTwoExpedition)
						{
							cell.Value = result switch
							{
								{ SuccessType: BattleExpeditionSuccessType.GreatSuccess } => DialogRes.ExpeditionCheckDoubleOkSign,
								_ => DialogRes.ExpeditionCheckOkSign
							} + string.Join(", ", result.SuccessPercent);

							cell.ToolTipText = string.Join("\n", result.SuccessPercent);
						}
						else if (mission.ExpeditionType is ExpeditionType.CombatTypeOneExpedition)
						{
							cell.Value = DialogRes.ExpeditionCheckOkSign + string.Join(", ", result.SuccessPercent);
							cell.ToolTipText = string.Join("\n", result.SuccessPercent);
						}
						else
						{
							cell.Value = DialogRes.ExpeditionCheckOkSign;
						}
					}
					else
					{
						cell.Value = string.Join(", ", result.FailureReason);
						cell.ToolTipText = string.Join("\n", result.FailureReason);
					}

					cell.Style = defaultStyle;
				}
				else
				{
					cell.Value = string.Join(", ", result.FailureReason);
					cell.ToolTipText = string.Join("\n", result.FailureReason);
					cell.Style = failedStyle;
				}
			}

			rows.Add(row);
		}

		CheckView.Rows.AddRange(rows.ToArray());

		CheckView.Sort(CheckView_Name, ListSortDirection.Ascending);

		CheckView.ResumeLayout();
	}

	private void DialogExpeditionCheck_Activated(object sender, EventArgs e)
	{
		int displayedRow = CheckView.FirstDisplayedScrollingRowIndex;
		int selectedRow = CheckView.SelectedRows.OfType<DataGridViewRow>().FirstOrDefault()?.Index ?? -1;

		UpdateCheckView();

		if (0 <= displayedRow && displayedRow < CheckView.RowCount)
			CheckView.FirstDisplayedScrollingRowIndex = displayedRow;
		if (0 <= selectedRow && selectedRow < CheckView.RowCount)
		{
			CheckView.ClearSelection();
			CheckView.Rows[selectedRow].Selected = true;
		}
	}



	private void CheckView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		if (e.ColumnIndex == CheckView_Name.Index)
		{
			e.Value = KCDatabase.Instance.Mission[(int)e.Value].NameEN;
			e.FormattingApplied = true;
		}
		else if (e.ColumnIndex == CheckView_ID.Index)
		{
			var mission = KCDatabase.Instance.Mission[(int)e.Value];
			e.Value = $"{mission.DisplayID}:{KCDatabase.Instance.MapArea[mission.MapAreaID].NameEN}";
			e.FormattingApplied = true;
		}
	}

	private void CheckView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
	{
		if (e.Column.Index == CheckView_Name.Index || e.Column.Index == CheckView_ID.Index)
		{
			var m1 = KCDatabase.Instance.Mission[(int)e.CellValue1];
			var m2 = KCDatabase.Instance.Mission[(int)e.CellValue2];

			int diff = m1.MapAreaID - m2.MapAreaID;
			if (diff == 0)
				diff = m1.MissionID - m2.MissionID;

			e.SortResult = diff;
			e.Handled = true;
		}
	}

	private void DialogExpeditionCheck_FormClosed(object sender, FormClosedEventArgs e)
	{
		ResourceManager.DestroyIcon(Icon);
	}
}
*/
