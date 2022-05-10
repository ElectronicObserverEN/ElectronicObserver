using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Support;
using WeifenLuo.WinFormsUI.Docking;
using Translation = ElectronicObserver.Properties.Window.FormQuest;

namespace ElectronicObserver.Window;

public partial class FormQuest : DockContent
{

	private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter;
	private DataGridViewCellStyle[] CSCategories;
	private bool IsLoaded = false;

	public FormQuest(FormMain parent)
	{
		InitializeComponent();

		ControlHelper.SetDoubleBuffered(QuestView);

		ConfigurationChanged();


		#region set cellstyle

		CSDefaultLeft = new DataGridViewCellStyle
		{
			Alignment = DataGridViewContentAlignment.MiddleLeft
		};
		CSDefaultLeft.BackColor =
			CSDefaultLeft.SelectionBackColor = Utility.Configuration.Config.UI.BackColor;
		CSDefaultLeft.ForeColor = Utility.Configuration.Config.UI.ForeColor;
		CSDefaultLeft.SelectionForeColor = Utility.Configuration.Config.UI.ForeColor;
		CSDefaultLeft.WrapMode = DataGridViewTriState.False;

		CSDefaultCenter = new DataGridViewCellStyle(CSDefaultLeft)
		{
			Alignment = DataGridViewContentAlignment.MiddleCenter
		};

		CSCategories = new DataGridViewCellStyle[11];
		for (int i = 0; i < CSCategories.Length; i++)
		{
			CSCategories[i] = new DataGridViewCellStyle(CSDefaultCenter);

			Color c;
			CSCategories[i].ForeColor = CSCategories[i].SelectionForeColor = Utility.Configuration.Config.UI.Quest_TypeFG;
			switch (i + 1)
			{
				case 1:     //編成
					c = Utility.Configuration.Config.UI.Quest_Type1Color;
					break;
				case 2:     //出撃
					c = Utility.Configuration.Config.UI.Quest_Type2Color;
					break;
				case 3:     //演習
					c = Utility.Configuration.Config.UI.Quest_Type3Color;
					break;
				case 4:     //遠征
					c = Utility.Configuration.Config.UI.Quest_Type4Color;
					break;
				case 5:     //補給/入渠
					c = Utility.Configuration.Config.UI.Quest_Type5Color;
					break;
				case 6:     //工廠
				case 11:
					c = Utility.Configuration.Config.UI.Quest_Type6Color;
					break;
				case 7:     //改装
					c = Utility.Configuration.Config.UI.Quest_Type7Color;
					break;
				case 8:     //出撃(2)
					c = Utility.Configuration.Config.UI.Quest_Type2Color;
					break;
				case 9:     //出撃(3)
					c = Color.FromArgb(0xFF, 0xCC, 0xCC);
					break;
				case 10:     //その他
				default:
					c = CSDefaultCenter.BackColor;
					CSCategories[i].ForeColor = CSCategories[i].SelectionForeColor = Utility.Configuration.Config.UI.ForeColor;
					break;
			}

			CSCategories[i].BackColor =
				CSCategories[i].SelectionBackColor = c;
		}

		QuestView.DefaultCellStyle = CSDefaultCenter;
		QuestView_Category.DefaultCellStyle = CSCategories[CSCategories.Length - 1];
		QuestView_Name.DefaultCellStyle = CSDefaultLeft;
		QuestView_Progress.DefaultCellStyle = CSDefaultLeft;

		#endregion

		if (Utility.Configuration.Config.UI.RemoveBarShadow)
		{ // 暂时借用这个属性
			QuestView.ColumnHeadersHeight = 21;
			QuestView.EnableHeadersVisualStyles = false;
			QuestView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			QuestView.ColumnHeadersDefaultCellStyle.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			QuestView.ColumnHeadersDefaultCellStyle.BackColor = Utility.Configuration.Config.UI.SubBackColor;
			QuestView.GridColor = Utility.Configuration.Config.UI.SubBackColor;
			QuestView.BackgroundColor = Utility.Configuration.Config.UI.BackColor;
		}

		SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;

		Translate();
	}

	public void Translate()
	{
		QuestView_Type.HeaderText = GeneralRes.Type;
		QuestView_Category.HeaderText = GeneralRes.Category;
		QuestView_Name.HeaderText = GeneralRes.QuestName;
		QuestView_Progress.HeaderText = GeneralRes.Progress;

		MenuProgress_Increment.Text = GeneralRes.IncrementByOne;
		MenuProgress_Decrement.Text = GeneralRes.DecrementByOne;
		MenuProgress_Reset.Text = Translation.MenuProgress_Reset;

		MenuMain_ColumnFilter.Text = GeneralRes.FilterBy;
		MenuMain_ColumnFilter_State.Text = GeneralRes.InProgressFilter;
		MenuMain_ColumnFilter_Type.Text = GeneralRes.TypeFilter;
		MenuMain_ColumnFilter_Category.Text = GeneralRes.CategoryFilter;
		MenuMain_ColumnFilter_Name.Text = GeneralRes.NameFilter;
		MenuMain_ColumnFilter_Progress.Text = GeneralRes.ProgressFilter;
		MenuMain_Initialize.Text = GeneralRes.Initialize;

		MenuMain_QuestFilter.Text = Translation.MenuMain_QuestFilter;
		MenuMain_ShowWeekly.Text = GeneralRes.ShowWeekly;
		MenuMain_ShowDaily.Text = GeneralRes.ShowDaily;
		MenuMain_ShowOnce.Text = GeneralRes.ShowOneTime;
		MenuMain_ShowRunningOnly.Text = GeneralRes.ShowInProgressOnly;
		MenuMain_ShowMonthly.Text = GeneralRes.ShowMonthly;
		MenuMain_ShowOther.Text = GeneralRes.ShowOther;
		MenuMain_GoogleQuest.Text = Translation.LookUpQuestOnDuckDuckGo;
		MenuMain_KcwikiQuest.Text = Translation.MenuMain_KcwikiQuest;

		ManuMain_QuestTitle.Text = Translation.ManuMain_QuestTitle;
		ManuMain_QuestDescription.Text = Translation.ManuMain_QuestDescription;
		ManuMain_QuestTranslate.Text = Translation.ManuMain_QuestTranslate;

		Text = GeneralRes.Quest;
	}

	private void FormQuest_Load(object sender, EventArgs e)
	{

		/*/
		APIObserver o = APIObserver.Instance;

		APIReceivedEventHandler rec = ( string apiname, dynamic data ) => Invoke( new APIReceivedEventHandler( APIUpdated ), apiname, data );

		o.ApiReqQuest_ClearItemGet.RequestReceived += rec;

		o.ApiGetMember_QuestList.ResponseReceived += rec;
		//*/

		KCDatabase.Instance.Quest.QuestUpdated += Updated;


		ClearQuestView();

		try
		{
			int sort = Utility.Configuration.Config.FormQuest.SortParameter;

			QuestView.Sort(QuestView.Columns[sort >> 1], (sort & 1) == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending);

		}
		catch (Exception)
		{

			QuestView.Sort(QuestView_Name, ListSortDirection.Ascending);
		}


		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormQuest]);

		IsLoaded = true;
	}


	void ConfigurationChanged()
	{

		var c = Utility.Configuration.Config;

		QuestView.Font = Font = c.UI.MainFont;

		MenuMain_ShowRunningOnly.Checked = c.FormQuest.ShowRunningOnly;
		MenuMain_ShowOnce.Checked = c.FormQuest.ShowOnce;
		MenuMain_ShowDaily.Checked = c.FormQuest.ShowDaily;
		MenuMain_ShowWeekly.Checked = c.FormQuest.ShowWeekly;
		MenuMain_ShowMonthly.Checked = c.FormQuest.ShowMonthly;
		MenuMain_ShowOther.Checked = c.FormQuest.ShowOther;

		if (c.FormQuest.ColumnFilter == null || ((List<bool>)c.FormQuest.ColumnFilter).Count != QuestView.Columns.Count)
		{
			c.FormQuest.ColumnFilter = Enumerable.Repeat(true, QuestView.Columns.Count).ToList();
		}
		if (c.FormQuest.ColumnWidth == null || ((List<int>)c.FormQuest.ColumnWidth).Count != QuestView.Columns.Count)
		{
			c.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(column => column.Width).ToList();
		}
		{
			List<bool> list = c.FormQuest.ColumnFilter;
			List<int> width = c.FormQuest.ColumnWidth;

			for (int i = 0; i < QuestView.Columns.Count; i++)
			{
				QuestView.Columns[i].Visible =
					((ToolStripMenuItem)MenuMain_ColumnFilter.DropDownItems[i]).Checked = list[i];
				QuestView.Columns[i].Width = width[i];
			}
		}

		foreach (DataGridViewColumn column in QuestView.Columns)
		{
			column.SortMode = c.FormQuest.AllowUserToSortRows ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
		}

		if (c.UI.IsLayoutFixed)
		{
			QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		}
		else
		{
			QuestView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			QuestView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		}

		foreach (DataGridViewRow row in QuestView.Rows)
		{
			row.Height = 21;
		}

		Updated();

	}


	void SystemEvents_SystemShuttingDown()
	{

		try
		{

			if (QuestView.SortedColumn != null)
				Utility.Configuration.Config.FormQuest.SortParameter = QuestView.SortedColumn.Index << 1 | (QuestView.SortOrder == SortOrder.Ascending ? 0 : 1);

			Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

		}
		catch (Exception)
		{
			// *ぷちっ*
		}
	}



	void Updated()
	{

		if (!KCDatabase.Instance.Quest.IsLoaded) return;

		QuestView.SuspendLayout();
		int scrollPos = QuestView.FirstDisplayedScrollingRowIndex;

		QuestView.Rows.Clear();

		var indexedQuests = KCDatabase.Instance.Quest.Quests.Values
			.OrderBy(q => q.ID)
			.Select((q, i) => (q, i));

		foreach ((QuestData q, int questIndex) in indexedQuests)
		{

			if (MenuMain_ShowRunningOnly.Checked && !(q.State == 2 || q.State == 3))
				continue;

			switch (q.Type)
			{
				case 1:
					if (!MenuMain_ShowDaily.Checked) continue;
					break;
				case 2:
					if (!MenuMain_ShowWeekly.Checked) continue;
					break;
				case 3:
					if (!MenuMain_ShowMonthly.Checked) continue;
					break;
				case 4:
				default:
					if (!MenuMain_ShowOnce.Checked) continue;
					break;
				case 5:
					if (q.QuestID == 211 || q.QuestID == 212)
					{
						// 空母3か輸送5
						if (!MenuMain_ShowDaily.Checked) continue;
					}
					else
					{
						if (!MenuMain_ShowOther.Checked) continue;
					}

					break;
			}


			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells(QuestView);
			row.Height = 21;

			// Add support for tooltip-based page numbering
			Color color = (questIndex / 5 % 2) switch
			{
				0 => Configuration.Config.UI.BackColor,
				_ => Configuration.Config.UI.SubBackColor
			};

			row.Cells[QuestView_State.Index].Value = (q.State == 3) ? ((bool?)null) : (q.State == 2);
			row.Cells[QuestView_State.Index].ToolTipText = $"{Translation.Page} #{questIndex / 5 + 1}";
			row.Cells[QuestView_State.Index].Style.BackColor = color;
			row.Cells[QuestView_State.Index].Style.SelectionBackColor = color;

			row.Cells[QuestView_Type.Index].Value = q.LabelType >= 100 ? q.LabelType : q.Type;
			row.Cells[QuestView_Type.Index].ToolTipText = Constants.GetQuestLabelType(q.LabelType);
			row.Cells[QuestView_Type.Index].Style.BackColor = color;
			row.Cells[QuestView_Type.Index].Style.SelectionBackColor = color;

			row.Cells[QuestView_Category.Index].Value = q.Category;
			row.Cells[QuestView_Category.Index].ToolTipText = Constants.GetQuestCategory(q.Category);
			row.Cells[QuestView_Category.Index].Style =
				CSCategories[Math.Min(q.Category - 1, CSCategories.Length - 1)];

			row.Cells[QuestView_Name.Index].Value = q.QuestID;
			row.Cells[QuestView_Name.Index].Style.BackColor = color;
			row.Cells[QuestView_Name.Index].Style.SelectionBackColor = color;

			row.Cells[QuestView_Progress.Index].Style.BackColor = color;
			row.Cells[QuestView_Progress.Index].Style.SelectionBackColor = color;

			{
				var progress = KCDatabase.Instance.QuestProgress[q.QuestID];
				var code = q.Code != "" ? $"{q.Code}: " : "";
				row.Cells[QuestView_Name.Index].ToolTipText =
					$"{code}{q.Name} (ID: {q.QuestID})\r\n{q.Description}\r\n{progress?.GetClearCondition() ?? ""}";
			}
			{
				string value;
				double tag;

				if (q.State == 3)
				{
					value = Translation.Complete;
					tag = 1.0;

				}
				else
				{
					var tracker = KCDatabase.Instance.QuestTrackerManagers.Trackers
						.FirstOrDefault(t => t.QuestId == q.QuestID);
					if (tracker is not null)
					{
						value = tracker.ProgressDisplay ?? "100%";
						tag = tracker.Progress;
					}
					else if (KCDatabase.Instance.QuestProgress.Progresses.ContainsKey(q.QuestID))
					{
						var p = KCDatabase.Instance.QuestProgress[q.QuestID];

						value = p.ToString();
						tag = p.ProgressPercentage;

					}
					else
					{

						switch (q.Progress)
						{
							case 0:
								value = "-";
								tag = 0.0;
								break;
							case 1:
								value = GeneralRes.Over50;
								tag = 0.5;
								break;
							case 2:
								value = GeneralRes.Over80;
								tag = 0.8;
								break;
							default:
								value = "???";
								tag = 0.0;
								break;
						}
					}
				}

				row.Cells[QuestView_Progress.Index].Value = value;
				row.Cells[QuestView_Progress.Index].Tag = tag;
			}

			QuestView.Rows.Add(row);
		}


		if (KCDatabase.Instance.Quest.Quests.Count < KCDatabase.Instance.Quest.Count)
		{
			int index = QuestView.Rows.Add();
			QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
			QuestView.Rows[index].Cells[QuestView_Name.Index].Value = string.Format(Translation.OtherQuests, (KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count));
		}

		if (KCDatabase.Instance.Quest.Quests.Count == 0)
		{
			int index = QuestView.Rows.Add();
			QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
			QuestView.Rows[index].Cells[QuestView_Name.Index].Value = GeneralRes.AllComplete;
		}

		//更新時にソートする
		if (QuestView.SortedColumn != null)
			QuestView.Sort(QuestView.SortedColumn, QuestView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

		// Retain scroll position
		if (QuestView.FirstDisplayedScrollingRowIndex > 0 && QuestView.Rows.Count > scrollPos)
			QuestView.FirstDisplayedScrollingRowIndex = scrollPos;

		QuestView.ResumeLayout();
	}


	private void QuestView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		if (e.Value is not int questId) return;

		if (e.ColumnIndex == QuestView_Type.Index)
		{
			e.Value = Constants.GetQuestType(questId);
			e.FormattingApplied = true;

		}
		else if (e.ColumnIndex == QuestView_Category.Index)
		{
			e.Value = Constants.GetQuestCategory(questId);
			e.FormattingApplied = true;

		}
		else if (e.ColumnIndex == QuestView_Name.Index)
		{
			var quest = KCDatabase.Instance.Quest[questId];
			string code = quest.Code switch
			{
				"" => "", // missing translation
				_ => $"{quest.Code}: "
			};
			e.Value = quest switch
			{
				{ } => $"{code}{quest.Name}",
				_ => "???"
			};
			e.FormattingApplied = true;

		}

	}



	private void QuestView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
	{

		if (e.Column.Index == QuestView_State.Index)
		{
			e.SortResult = (e.CellValue1 == null ? 2 : ((bool)e.CellValue1 ? 1 : 0)) -
						   (e.CellValue2 == null ? 2 : ((bool)e.CellValue2 ? 1 : 0));
		}
		else
		{
			e.SortResult = (e.CellValue1 as int? ?? 99999999) - (e.CellValue2 as int? ?? 99999999);
		}

		if (e.SortResult == 0)
		{
			e.SortResult = (QuestView.Rows[e.RowIndex1].Tag as int? ?? 0) - (QuestView.Rows[e.RowIndex2].Tag as int? ?? 0);
		}

		e.Handled = true;
	}

	private void QuestView_Sorted(object sender, EventArgs e)
	{

		for (int i = 0; i < QuestView.Rows.Count; i++)
		{
			QuestView.Rows[i].Tag = i;
		}

	}


	private void QuestView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{

		if (e.ColumnIndex != QuestView_Progress.Index ||
			e.RowIndex < 0 ||
			(e.PaintParts & DataGridViewPaintParts.Background) == 0)
			return;


		using (var bback = new SolidBrush(e.CellStyle.BackColor))
		{

			Color col;
			double rate;
			bool drawgaugeback = false;
			if (QuestView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null)
			{
				rate = 0.0;
			}
			else
			{
				rate = (double)QuestView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
				drawgaugeback = true;
			}

			if (rate < 0.5)
				col = Utility.Configuration.Config.UI.Quest_ColorProcessLT50;
			else if (rate < 0.8)
				col = Utility.Configuration.Config.UI.Quest_ColorProcessLT80;
			else if (rate < 1.0)
				col = Utility.Configuration.Config.UI.Quest_ColorProcessLT100;
			else
				col = Utility.Configuration.Config.UI.Quest_ColorProcessDefault;

			using (var bgauge = new SolidBrush(col))
			using (var bgaugeback = new SolidBrush(Utility.Configuration.Config.UI.SubBackColor))
			{
				const int thickness = 4;

				e.Graphics.FillRectangle(bback, e.CellBounds);
				if (drawgaugeback && rate < 1.0) e.Graphics.FillRectangle(bgaugeback, new Rectangle(e.CellBounds.X, e.CellBounds.Bottom - thickness, e.CellBounds.Width, thickness));
				e.Graphics.FillRectangle(bgauge, new Rectangle(e.CellBounds.X, e.CellBounds.Bottom - thickness, (int)(e.CellBounds.Width * rate), thickness));
			}
		}

		e.Paint(e.ClipBounds, e.PaintParts & ~DataGridViewPaintParts.Background);
		e.Handled = true;

	}



	private void MenuMain_ShowRunningOnly_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowRunningOnly = MenuMain_ShowRunningOnly.Checked;
		Updated();
	}


	private void MenuMain_ShowOnce_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowOnce = MenuMain_ShowOnce.Checked;
		Updated();
	}

	private void MenuMain_ShowDaily_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowDaily = MenuMain_ShowDaily.Checked;
		Updated();
	}

	private void MenuMain_ShowWeekly_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowWeekly = MenuMain_ShowWeekly.Checked;
		Updated();
	}

	private void MenuMain_ShowMonthly_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowMonthly = MenuMain_ShowMonthly.Checked;
		Updated();
	}

	private void MenuMain_ShowOther_Click(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormQuest.ShowOther = MenuMain_ShowOther.Checked;
		Updated();
	}



	private void MenuMain_Initialize_Click(object sender, EventArgs e)
	{

		if (MessageBox.Show(GeneralRes.InitializeQuestData, GeneralRes.InitQuestTitle,
			MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
		{

			KCDatabase.Instance.Quest.Clear();
			KCDatabase.Instance.QuestProgress.Clear();
			ClearQuestView();
		}

	}


	private void ClearQuestView()
	{

		QuestView.Rows.Clear();

		{
			DataGridViewRow row = new DataGridViewRow();
			row.CreateCells(QuestView);
			row.SetValues(null, null, null, Translation.Unknown, null);
			QuestView.Rows.Add(row);
		}

	}


	private void MenuMain_ColumnFilter_Click(object sender, EventArgs e)
	{

		var menu = sender as ToolStripMenuItem;
		if (menu == null) return;

		int index = -1;
		for (int i = 0; i < MenuMain_ColumnFilter.DropDownItems.Count; i++)
		{
			if (sender == MenuMain_ColumnFilter.DropDownItems[i])
			{
				index = i;
				break;
			}
		}

		if (index == -1) return;

		QuestView.Columns[index].Visible =
			Utility.Configuration.Config.FormQuest.ColumnFilter.List[index] = menu.Checked;
	}


	private void QuestView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
	{

		if (IsLoaded)
			Utility.Configuration.Config.FormQuest.ColumnWidth = QuestView.Columns.Cast<DataGridViewColumn>().Select(c => c.Width).ToList();

	}




	private void QuestView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
	{

		if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex >= 0)
		{
			QuestView.ClearSelection();
			QuestView.Rows[e.RowIndex].Selected = true;
		}

	}

	private void MenuProgress_Increment_Click(object sender, EventArgs e)
	{

		int id = GetSelectedRowQuestID();

		var quest = KCDatabase.Instance.Quest[id];
		var progress = KCDatabase.Instance.QuestProgress[id];

		if (id != -1 && quest != null && progress != null)
		{

			try
			{
				progress.Increment();
				Updated();

			}
			catch (Exception)
			{
				Utility.Logger.Add(3, string.Format(Translation.FailedToChangeQuestProgress, quest.Name));
				System.Media.SystemSounds.Hand.Play();
			}
		}
	}

	private void MenuProgress_Decrement_Click(object sender, EventArgs e)
	{

		int id = GetSelectedRowQuestID();
		var quest = KCDatabase.Instance.Quest[id];
		var progress = KCDatabase.Instance.QuestProgress[id];

		if (id != -1 && quest != null && progress != null)
		{

			try
			{
				progress.Decrement();
				Updated();

			}
			catch (Exception)
			{
				Utility.Logger.Add(3, string.Format(Translation.FailedToChangeQuestProgress, quest.Name));
				System.Media.SystemSounds.Hand.Play();
			}
		}
	}

	private void MenuProgress_Reset_Click(object sender, EventArgs e)
	{

		int id = GetSelectedRowQuestID();

		var quest = KCDatabase.Instance.Quest[id];
		var progress = KCDatabase.Instance.QuestProgress[id];

		if (id != -1 && (quest != null || progress != null))
		{

			if (MessageBox.Show(string.Format(Translation.QuestResetConfirmationText, (quest != null ? ($"『{quest.Name}』") : ($"ID: {id} "))),
				Translation.QuestResetConfirmationCaption,
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
			{

				if (quest != null)
					KCDatabase.Instance.Quest.Quests.Remove(quest);

				if (progress != null)
					KCDatabase.Instance.QuestProgress.Progresses.Remove(progress);

				Updated();
			}
		}

	}


	// デフォルトのツールチップは消える時間が速すぎるので、自分で制御する
	private void QuestView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
	{

		if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= QuestView.RowCount || e.ColumnIndex >= QuestView.ColumnCount)
		{
			ToolTipInfo.SetToolTip(QuestView, null);
			return;
		}

		if (!string.IsNullOrWhiteSpace(QuestView[e.ColumnIndex, e.RowIndex].ToolTipText))
		{
			ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].ToolTipText);

		}
		else if (e.ColumnIndex == QuestView_Progress.Index && QuestView[e.ColumnIndex, e.RowIndex].Value != null)
		{
			ToolTipInfo.SetToolTip(QuestView, QuestView[e.ColumnIndex, e.RowIndex].Value.ToString());

		}
		else
		{
			ToolTipInfo.SetToolTip(QuestView, null);
		}

	}

	private void QuestView_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
	{
		ToolTipInfo.SetToolTip(QuestView, null);
	}


	private void MenuMain_Opening(object sender, CancelEventArgs e)
	{

		var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

		if (quest != null)
		{
			var searchKey = quest.Name;
			if (quest.Name.Length > 25)
				searchKey = quest.Name.Substring(0, 22) + "...";

			MenuMain_GoogleQuest.Enabled = true;
			MenuMain_GoogleQuest.Text = string.Format(Translation.LookUpSpecificQuestOnDuckDuckGo, searchKey);
			MenuMain_KcwikiQuest.Enabled = true;
			MenuMain_KcwikiQuest.Text = string.Format(Translation.LookUpSpecificQuestOnWiki, searchKey);
		}
		else
		{
			MenuMain_GoogleQuest.Enabled = false;
			MenuMain_GoogleQuest.Text = Translation.LookUpQuestOnDuckDuckGo;
			MenuMain_KcwikiQuest.Enabled = false;
			MenuMain_KcwikiQuest.Text = Translation.MenuMain_KcwikiQuest;
		}
	}

	private void MenuMain_GoogleQuest_Click(object sender, EventArgs e)
	{
		var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

		if (quest != null)
		{
			try
			{
				ProcessStartInfo psi = new ProcessStartInfo
				{
					FileName = @"https://www.duckduckgo.com/?q=" + Uri.EscapeDataString(quest.Code) + "+" + Uri.EscapeDataString(quest.Name) + Translation.SearchEngineKancolleSpecifier,
					UseShellExecute = true
				};
				// google <任務名> 艦これ
				Process.Start(psi);
			}
			catch (Exception ex)
			{
				Utility.ErrorReporter.SendErrorReport(ex, Translation.FailedToSearchOnDuckDuckGo);
			}
		}

	}

	private void MenuMain_KcwikiQuest_Click(object sender, EventArgs e)
	{
		var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

		if (quest == null) return;

		try
		{
			string url = quest.Code switch
			{
				"" => @"https://www.duckduckgo.com/?q=" + Uri.EscapeDataString(quest.Name) + "+" + Uri.EscapeDataString(Translation.SearchOnWikiQuery),
				_ => string.Format(Translation.SearchLinkWiki, Uri.EscapeDataString(quest.Code)),
			};

			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, Translation.FailedToOpenKancolleWiki);
		}

	}

	private int GetSelectedRowQuestID()
	{
		var rows = QuestView.SelectedRows;

		if (rows != null && rows.Count > 0 && rows[0].Index != -1)
		{

			return rows[0].Cells[QuestView_Name.Index].Value as int? ?? -1;
		}

		return -1;
	}

	private void ManuMain_QuestDescription_Click(object sender, EventArgs e)
	{
		var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

		if (quest != null)
		{
			Clipboard.SetText(quest.Description.Replace(Environment.NewLine, ""));
		}
	}

	private void ManuMain_QuestTitle_Click(object sender, EventArgs e)
	{
		var quest = KCDatabase.Instance.Quest[GetSelectedRowQuestID()];

		if (quest != null)
		{
			Clipboard.SetText(quest.Name);
		}
	}

	private void ManuMain_QuestTranslate_Click(object sender, EventArgs e)
	{
		bool needTranslation = false;
		dynamic json = new JsonObject();
		foreach (QuestData quest in KCDatabase.Instance.Quest.Quests.Values)
		{
			if (quest.Translated) continue;

			json[quest.ID.ToString()] = new
			{
				code = "",
				name_jp = quest.NameJP,
				name = "",
				desc_jp = quest.DescriptionJP.Replace("\r\n", "<br>"),
				desc = ""
			};
			needTranslation = true;
		}
		string serializedOutput = json.ToString();

		if (needTranslation == false)
		{
			MessageBox.Show(Translation.AllQuestsAreTranslated, Translation.Information, MessageBoxButtons.OK,
				MessageBoxIcon.Information);
			return;
		}

		Clipboard.SetText(serializedOutput);
	}

	protected override string GetPersistString()
	{
		return "Quest";
	}


}
