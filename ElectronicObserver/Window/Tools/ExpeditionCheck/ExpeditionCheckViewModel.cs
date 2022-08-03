using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.ExpeditionCheck;

public class ExpeditionCheckViewModel : WindowViewModelBase
{
	public ExpeditionCheckTranslationViewModel ExpeditionCheckTranslation { get; }

	public List<ExpeditionCheckRow> Rows { get; set; } = new();

	public ExpeditionCheckViewModel()
	{
		ExpeditionCheckTranslation = Ioc.Default.GetService<ExpeditionCheckTranslationViewModel>()!;

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
		}).ToList();

		/*Rows = new List<ExpeditionCheckRow>()
		{
			new ExpeditionCheckRow()
			{
				AreaName = "Area name",

				ExpeditionId = "01",
				ExpeditionName = "the expedition name",
				ExpeditionType = ExpeditionType.Normal,

				Fleet1Result = new MissionClearCondition.MissionClearConditionResult(null),
				Fleet2Result = new MissionClearCondition.MissionClearConditionResult(null),
				Fleet3Result = new MissionClearCondition.MissionClearConditionResult(null),
				Fleet4Result = new MissionClearCondition.MissionClearConditionResult(null),
				Conditions = new MissionClearCondition.MissionClearConditionResult(null),
			}
		};*/
	}
}

/*
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogExpeditionCheck;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogExpeditionCheck : Form
{
	public DialogExpeditionCheck()
	{
		InitializeComponent();

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();

		Translate();
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;

		Font = c.UI.MainFont.FontData;

		if (c.UI.IsLayoutFixed)
		{
			CheckView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			CheckView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		}
		else
		{
			CheckView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			CheckView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		}
	}

	public void Translate()
	{
		CheckView_Name.HeaderText = Translation.CheckView_Name;
		CheckView_Fleet1.HeaderText = Translation.CheckView_Fleet1;
		CheckView_Fleet2.HeaderText = Translation.CheckView_Fleet2;
		CheckView_Fleet3.HeaderText = Translation.CheckView_Fleet3;
		CheckView_Fleet4.HeaderText = Translation.CheckView_Fleet4;
		CheckView_Condition.HeaderText = Translation.CheckView_Condition;

		Text = Translation.Title;
	}

	private void DialogExpeditionCheck_Load(object sender, EventArgs e)
	{
		if (!KCDatabase.Instance.Mission.Any())
		{
			MessageBox.Show(DialogRes.ExpeditionNotLoadedMessage,
				DialogRes.ExpeditionNotLoadedTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
			Close();
			return;
		}


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
