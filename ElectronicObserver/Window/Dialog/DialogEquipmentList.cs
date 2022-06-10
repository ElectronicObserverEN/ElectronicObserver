using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Support;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using DialogAlbumMasterShipTranslation = ElectronicObserver.Properties.Window.Dialog.DialogAlbumMasterShip;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogEquipmentList;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogEquipmentList : Form
{


	private DataGridViewCellStyle CSDefaultLeft, CSDefaultRight,
		CSUnselectableLeft, CSUnselectableRight;


	public DialogEquipmentList()
	{

		InitializeComponent();

		ControlHelper.SetDoubleBuffered(EquipmentView);

		Font = Utility.Configuration.Config.UI.MainFont;

		foreach (DataGridViewColumn column in EquipmentView.Columns)
		{
			column.MinimumWidth = 2;
		}



		#region CellStyle

		CSDefaultLeft = new DataGridViewCellStyle
		{
			Alignment = DataGridViewContentAlignment.MiddleLeft,
			BackColor = SystemColors.Control,
			Font = Font,
			ForeColor = SystemColors.ControlText,
			SelectionBackColor = Color.FromArgb(0xFF, 0xFF, 0xCC),
			SelectionForeColor = SystemColors.ControlText,
			WrapMode = DataGridViewTriState.False
		};

		CSDefaultRight = new DataGridViewCellStyle(CSDefaultLeft)
		{
			Alignment = DataGridViewContentAlignment.MiddleRight
		};

		CSUnselectableLeft = new DataGridViewCellStyle(CSDefaultLeft);
		CSUnselectableLeft.SelectionForeColor = CSUnselectableLeft.ForeColor;
		CSUnselectableLeft.SelectionBackColor = CSUnselectableLeft.BackColor;

		CSUnselectableRight = new DataGridViewCellStyle(CSDefaultRight);
		CSUnselectableRight.SelectionForeColor = CSUnselectableRight.ForeColor;
		CSUnselectableRight.SelectionBackColor = CSUnselectableRight.BackColor;


		EquipmentView.DefaultCellStyle = CSDefaultRight;
		EquipmentView_Name.DefaultCellStyle = CSDefaultLeft;
		EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

		DetailView.DefaultCellStyle = CSUnselectableRight;
		DetailView_EquippedShip.DefaultCellStyle = CSUnselectableLeft;
		DetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

		#endregion

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();

		Translate();
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;

		Font = c.UI.MainFont.FontData;
		TopMenu.Font = Font;

		if (c.UI.IsLayoutFixed)
		{
			EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			EquipmentView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

			DetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			DetailView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		}
		else
		{
			EquipmentView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			EquipmentView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

			DetailView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			DetailView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		}
	}

	public void Translate()
	{
		EquipmentView_Name.HeaderText = EncycloRes.EquipName;
		EquipmentView_CountAll.HeaderText = EncycloRes.CountAll;
		EquipmentView_CountRemain.HeaderText = EncycloRes.CountExtra;

		TopMenu_File.Text = Translation.TopMenu_File;
		TopMenu_File_CSVOutput.Text = Translation.TopMenu_File_CSVOutput;
		TopMenu_File_CopyToFleetAnalysis.Text = Translation.TopMenu_File_CopyToFleetAnalysis;

		SaveCSVDialog.Title = Translation.SaveCSVDialog;

		DetailView_Level.HeaderText = EncycloRes.StarLevel;
		DetailView_AircraftLevel.HeaderText = EncycloRes.SkillLevel;
		DetailView_CountAll.HeaderText = EncycloRes.CountAll;
		DetailView_CountRemain.HeaderText = EncycloRes.CountExtra;

		Text = EncycloRes.EquipmentList;
	}

	private void DialogEquipmentList_Load(object sender, EventArgs e)
	{

		UpdateView();

		this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormEquipmentList]);

	}


	private void EquipmentView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
	{

		if (e.Column.Index == EquipmentView_Name.Index)
		{

			int id1 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex1].Value;
			int id2 = (int)EquipmentView[EquipmentView_ID.Index, e.RowIndex2].Value;

			e.SortResult =
				KCDatabase.Instance.MasterEquipments[id1].CategoryType -
				KCDatabase.Instance.MasterEquipments[id2].CategoryType;

			if (e.SortResult == 0)
			{
				e.SortResult = id1 - id2;
			}

		}
		else
		{

			e.SortResult = ((IComparable)e.CellValue1).CompareTo(e.CellValue2);

		}


		if (e.SortResult == 0)
		{
			e.SortResult = (EquipmentView.Rows[e.RowIndex1].Tag as int? ?? 0) - (EquipmentView.Rows[e.RowIndex2].Tag as int? ?? 0);
		}

		e.Handled = true;

	}

	private void EquipmentView_Sorted(object sender, EventArgs e)
	{

		for (int i = 0; i < EquipmentView.Rows.Count; i++)
			EquipmentView.Rows[i].Tag = i;

	}

	private void EquipmentView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{

		if (e.ColumnIndex == EquipmentView_Icon.Index)
		{
			e.Value = ResourceManager.GetEquipmentImage((int)e.Value);
			e.FormattingApplied = true;
		}

	}

	private void EquipmentView_SelectionChanged(object sender, EventArgs e)
	{

		if (EquipmentView.Enabled && EquipmentView.SelectedRows != null && EquipmentView.SelectedRows.Count > 0)
			UpdateDetailView((int)EquipmentView[EquipmentView_ID.Index, EquipmentView.SelectedRows[0].Index].Value);

	}


	private void DetailView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{

		if (e.ColumnIndex != DetailView_EquippedShip.Index)
		{
			if (AreEqual(e.RowIndex, e.RowIndex - 1))
			{

				e.Value = "";
				e.FormattingApplied = true;
				return;
			}
		}

		if (e.ColumnIndex == DetailView_Level.Index)
		{

			e.Value = "+" + e.Value;
			e.FormattingApplied = true;

		}

	}

	private void DetailView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
	{

		e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

		if (AreEqual(e.RowIndex, e.RowIndex + 1))
		{
			e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
		}
		else
		{
			e.AdvancedBorderStyle.Bottom = DetailView.AdvancedCellBorderStyle.Bottom;
		}

	}

	/// <summary>
	/// DetailView 内の行が同じレベルであるかを判定します。
	/// </summary>
	private bool AreEqual(int rowIndex1, int rowIndex2)
	{

		if (rowIndex1 < 0 ||
			rowIndex1 >= DetailView.Rows.Count ||
			rowIndex2 < 0 ||
			rowIndex2 >= DetailView.Rows.Count)
			return false;

		return ((IComparable)DetailView[DetailView_Level.Index, rowIndex1].Value)
			   .CompareTo(DetailView[DetailView_Level.Index, rowIndex2].Value) == 0 &&
			   ((IComparable)DetailView[DetailView_AircraftLevel.Index, rowIndex1].Value)
			   .CompareTo(DetailView[DetailView_AircraftLevel.Index, rowIndex2].Value) == 0;
	}


	/// <summary>
	/// 一覧ビューを更新します。
	/// </summary>
	private void UpdateView()
	{

		var ships = KCDatabase.Instance.Ships.Values;
		var equipments = KCDatabase.Instance.Equipments.Values;
		var masterEquipments = KCDatabase.Instance.MasterEquipments;
		int masterCount = masterEquipments.Values.Count(eq => !eq.IsAbyssalEquipment);

		var allCount = equipments.GroupBy(eq => eq.EquipmentID).ToDictionary(group => group.Key, group => group.Count());

		var remainCount = new Dictionary<int, int>(allCount);


		//剰余数計算
		foreach (var eq in ships
			.SelectMany(s => s.AllSlotInstanceMaster)
			.Where(eq => eq != null))
		{

			remainCount[eq.EquipmentID]--;
		}

		foreach (var eq in KCDatabase.Instance.BaseAirCorps.Values
			.SelectMany(corps => corps.Squadrons.Values.Select(sq => sq.EquipmentInstance))
			.Where(eq => eq != null))
		{

			remainCount[eq.EquipmentID]--;
		}

		foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
			.Where(eq => eq.EquipmentInstance != null))
		{

			remainCount[eq.EquipmentInstance.EquipmentID]--;
		}



		//表示処理
		EquipmentView.SuspendLayout();

		EquipmentView.Enabled = false;
		EquipmentView.Rows.Clear();


		var rows = new List<DataGridViewRow>(allCount.Count);
		var ids = allCount.Keys;

		foreach (int id in ids)
		{

			var row = new DataGridViewRow();
			row.CreateCells(EquipmentView);
			row.SetValues(
				id,
				masterEquipments[id].IconType,
				masterEquipments[id].NameEN,
				allCount[id],
				remainCount[id]
			);

			{
				StringBuilder sb = new StringBuilder();
				var eq = masterEquipments[id];

				sb.AppendFormat("{0} {1} (ID: {2})\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN, eq.EquipmentID);
				if (eq.Firepower != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.Firepower + " {0:+0;-0}\r\n", eq.Firepower);
				if (eq.Torpedo != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.Torpedo + " {0:+0;-0}\r\n", eq.Torpedo);
				if (eq.AA != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.AA + " {0:+0;-0}\r\n", eq.AA);
				if (eq.Armor != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.Armor + " {0:+0;-0}\r\n", eq.Armor);
				if (eq.ASW != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.ASW + " {0:+0;-0}\r\n", eq.ASW);
				if (eq.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? DialogAlbumMasterShipTranslation.Interception : DialogAlbumMasterShipTranslation.Evasion, eq.Evasion);
				if (eq.LOS != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.LOS + " {0:+0;-0}\r\n", eq.LOS);
				if (eq.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? DialogAlbumMasterShipTranslation.AntiBomb : DialogAlbumMasterShipTranslation.Accuracy, eq.Accuracy);
				if (eq.Bomber != 0) sb.AppendFormat(DialogAlbumMasterShipTranslation.Bombing + " {0:+0;-0}\r\n", eq.Bomber);
				sb.AppendLine(DialogAlbumMasterShipTranslation.RightClickToOpenInNewWindow);

				row.Cells[2].ToolTipText = sb.ToString();
			}
			rows.Add(row);
		}

		for (int i = 0; i < rows.Count; i++)
			rows[i].Tag = i;

		EquipmentView.Rows.AddRange(rows.ToArray());

		EquipmentView.Sort(EquipmentView_Name, ListSortDirection.Ascending);


		EquipmentView.Enabled = true;
		EquipmentView.ResumeLayout();

		if (EquipmentView.Rows.Count > 0)
			EquipmentView.CurrentCell = EquipmentView[0, 0];

	}

	private void EquipmentView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		if (0 <= e.RowIndex && e.RowIndex < EquipmentView.RowCount)
		{
			int equipmentID = (int)EquipmentView.Rows[e.RowIndex].Cells[0].Value;

			if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
			{
				Cursor = Cursors.AppStarting;
				new DialogAlbumMasterEquipmentWpf(equipmentID).Show(App.Current.MainWindow);
				Cursor = Cursors.Default;
			}
		}
	}


	private class DetailCounter : IIdentifiable
	{

		public int level;
		public int aircraftLevel;
		public int countAll;
		public int countRemain;
		public int countRemainPrev;

		public List<string> equippedShips;

		public DetailCounter(int lv, int aircraftLv)
		{
			level = lv;
			aircraftLevel = aircraftLv;
			countAll = 0;
			countRemainPrev = 0;
			countRemain = 0;
			equippedShips = new List<string>();
		}

		public static int CalculateID(int level, int aircraftLevel)
		{
			return level + aircraftLevel * 100;
		}

		public static int CalculateID(IEquipmentData eq)
		{
			return CalculateID(eq.Level, eq.AircraftLevel);
		}

		public int ID => CalculateID(level, aircraftLevel);
	}


	/// <summary>
	/// 詳細ビューを更新します。
	/// </summary>
	private void UpdateDetailView(int equipmentID)
	{

		DetailView.SuspendLayout();

		DetailView.Rows.Clear();

		//装備数カウント
		var eqs = KCDatabase.Instance.Equipments.Values.Where(eq => eq.EquipmentID == equipmentID);
		var countlist = new IDDictionary<DetailCounter>();

		foreach (var eq in eqs)
		{
			var c = countlist[DetailCounter.CalculateID(eq)];
			if (c == null)
			{
				countlist.Add(new DetailCounter(eq.Level, eq.AircraftLevel));
				c = countlist[DetailCounter.CalculateID(eq)];
			}
			c.countAll++;
			c.countRemain++;
			c.countRemainPrev++;
		}

		//装備艦集計
		foreach (var ship in KCDatabase.Instance.Ships.Values)
		{

			foreach (var eq in ship.AllSlotInstance.Where(s => s != null && s.EquipmentID == equipmentID))
			{

				countlist[DetailCounter.CalculateID(eq)].countRemain--;
			}

			foreach (var c in countlist.Values)
			{
				if (c.countRemain != c.countRemainPrev)
				{

					int diff = c.countRemainPrev - c.countRemain;

					c.equippedShips.Add(ship.NameWithLevel + (diff > 1 ? (" x" + diff) : ""));

					c.countRemainPrev = c.countRemain;
				}
			}

		}

		// 基地航空隊 - 配備中の装備を集計
		foreach (var corps in KCDatabase.Instance.BaseAirCorps.Values)
		{

			foreach (var sq in corps.Squadrons.Values.Where(sq => sq != null && sq.EquipmentID == equipmentID))
			{

				countlist[DetailCounter.CalculateID(sq.EquipmentInstance)].countRemain--;
			}

			foreach (var c in countlist.Values)
			{
				if (c.countRemain != c.countRemainPrev)
				{
					int diff = c.countRemainPrev - c.countRemain;

					c.equippedShips.Add(string.Format("#{0} {1}{2}", corps.MapAreaID, corps.Name, diff > 1 ? (" x" + diff) : ""));

					c.countRemainPrev = c.countRemain;
				}
			}

		}

		// 基地航空隊 - 配置転換中の装備を集計
		foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
			.Select(v => v.EquipmentInstance)
			.Where(eq => eq != null && eq.EquipmentID == equipmentID))
		{

			countlist[DetailCounter.CalculateID(eq)].countRemain--;
		}

		foreach (var c in countlist.Values)
		{
			if (c.countRemain != c.countRemainPrev)
			{

				int diff = c.countRemainPrev - c.countRemain;

				c.equippedShips.Add("配置転換中" + (diff > 1 ? (" x" + diff) : ""));

				c.countRemainPrev = c.countRemain;
			}
		}


		//行に反映
		var rows = new List<DataGridViewRow>(eqs.Count());

		foreach (var c in countlist.Values)
		{

			if (c.equippedShips.Count() == 0)
			{
				c.equippedShips.Add("");
			}

			foreach (var s in c.equippedShips)
			{

				var row = new DataGridViewRow();
				row.CreateCells(DetailView);
				row.SetValues(c.level, c.aircraftLevel, c.countAll, c.countRemain, s);
				rows.Add(row);
			}

		}

		DetailView.Rows.AddRange(rows.ToArray());
		DetailView.Sort(DetailView_AircraftLevel, ListSortDirection.Ascending);
		DetailView.Sort(DetailView_Level, ListSortDirection.Ascending);

		DetailView.ResumeLayout();

		Text = EncycloRes.EquipmentList + " - " + KCDatabase.Instance.MasterEquipments[equipmentID].NameEN;
	}


	private void DetailView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
	{

		e.SortResult = ((IComparable)e.CellValue1).CompareTo(e.CellValue2);

		if (e.SortResult == 0)
		{
			e.SortResult = (DetailView.Rows[e.RowIndex1].Tag as int? ?? 0) - (DetailView.Rows[e.RowIndex2].Tag as int? ?? 0);

			if (DetailView.SortOrder == SortOrder.Descending)
				e.SortResult = -e.SortResult;
		}

		e.Handled = true;
	}

	private void DetailView_Sorted(object sender, EventArgs e)
	{

		for (int i = 0; i < DetailView.Rows.Count; i++)
			DetailView.Rows[i].Tag = i;

	}



	private void Menu_File_CSVOutput_Click(object sender, EventArgs e)
	{

		if (SaveCSVDialog.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{

				using (StreamWriter sw = new StreamWriter(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding))
				{
					sw.WriteLine(EncycloRes.EquipListCSVFormat);
					string arg = string.Format("{{{0}}}", string.Join("},{", Enumerable.Range(0, 8)));

					foreach (var eq in KCDatabase.Instance.Equipments.Values)
					{

						if (eq.Name == "なし") continue;

						ShipData equippedShip = KCDatabase.Instance.Ships.Values.FirstOrDefault(s => s.AllSlot.Contains(eq.MasterID));


						sw.WriteLine(arg,
							eq.MasterID,
							eq.EquipmentID,
							eq.Name,
							eq.Level,
							eq.AircraftLevel,
							eq.IsLocked ? 1 : 0,
							equippedShip?.MasterID ?? -1,
							equippedShip?.NameWithLevel ?? ""
						);

					}

				}

			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, EncycloRes.FailedOutputEqListCSV);
				MessageBox.Show(EncycloRes.FailedOutputEqListCSV + "\r\n" + ex.Message, Translation.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

			}

		}


	}


	/// <summary>
	/// 「艦隊分析」装備情報反映用
	/// https://kancolle-fleetanalysis.firebaseapp.com/
	/// </summary>
	private void TopMenu_File_CopyToFleetAnalysis_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(
			"[" + string.Join(",", KCDatabase.Instance.Equipments.Values.Where(eq => eq?.IsLocked ?? false)
				.Select(eq => $"{{\"api_slotitem_id\":{eq.EquipmentID},\"api_level\":{eq.Level}}}")) + "]");
	}


	private void DialogEquipmentList_FormClosed(object sender, FormClosedEventArgs e)
	{

		ResourceManager.DestroyIcon(Icon);

	}

	private void TopMenu_File_Update_Click(object sender, EventArgs e)
	{

		UpdateView();
	}


}
