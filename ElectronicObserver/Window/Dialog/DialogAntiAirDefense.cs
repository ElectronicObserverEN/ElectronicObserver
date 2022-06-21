using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogAntiAirDefense;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogAntiAirDefense : Form
{

	private class AACutinComboBoxData
	{
		public readonly int Kind;
		public AACutinComboBoxData(int kind)
		{
			Kind = kind;
		}

		public override string ToString() => $"{Kind}: {Constants.GetAACutinKind(Kind)}";


		public static implicit operator int(AACutinComboBoxData data)
		{
			if (data == null)
				return -1;
			return data.Kind;
		}
	}

	private class FormationComboBoxData
	{
		public readonly int Formation;
		public FormationComboBoxData(int formation)
		{
			Formation = formation;
		}

		public override string ToString() => Constants.GetFormation(Formation);


		public static implicit operator int(FormationComboBoxData data)
		{
			if (data == null)
				return -1;
			return data.Formation;
		}
	}


	/// <summary>
	/// NumericUpDown から Value を正しく取得できないことがあるため、一旦これにキャッシュする
	/// </summary>
	/// <remarks>https://github.com/andanteyk/ElectronicObserver/pull/197</remarks>
	private int enemySlotCountValue;


	public DialogAntiAirDefense()
	{
		InitializeComponent();
		enemySlotCountValue = (int)EnemySlotCount.Value;

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
			ResultView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
			ResultView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		}
		else
		{
			ResultView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
			ResultView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		}
	}

	public void Translate()
	{
		label6.Text = Translation.WipeRate;
		ToolTipInfo.SetToolTip(ShowAll, Translation.ShowAllToolTip);
		label5.Text = Translation.FleetAA;
		label4.Text = Translation.AACI;
		label3.Text = Translation.Formation;
		Formation.Items.Clear();
		Formation.Items.AddRange(new object[]
		{
			Translation.Formation_LineAhead,
			Translation.Formation_DoubleLine,
			Translation.Formation_Ring
		});
		label2.Text = Translation.PlaneSlot;
		label1.Text = Translation.Fleet;
		FleetID.Items.Clear();
		FleetID.Items.AddRange(new object[]
		{
			Translation.FleetID_First,
			Translation.FleetID_Second,
			Translation.FleetID_Third,
			Translation.FleetID_Fourth,
			Translation.FleetID_CombinedFleet
		});

		ResultView_ShipName.HeaderText = Translation.ResultView_ShipName;
		ResultView_AntiAir.HeaderText = Translation.ResultView_AntiAir;
		ResultView_AdjustedAntiAir.HeaderText = Translation.ResultView_AdjustedAntiAir;
		ResultView_ProportionalAirDefense.HeaderText = Translation.ResultView_ProportionalAirDefense;
		ResultView_FixedAirDefense.HeaderText = Translation.ResultView_FixedAirDefense;
		ResultView_ShootDownBoth.HeaderText = Translation.ResultView_ShootDownBoth;
		ResultView_ShootDownBoth.ToolTipText = Translation.ResultView_ShootDownBothToolTip;
		ResultView_ShootDownProportional.HeaderText = Translation.ResultView_ShootDownProportional;
		ResultView_ShootDownProportional.ToolTipText = Translation.ResultView_ShootDownProportionalToolTip;
		ResultView_ShootDownFixed.HeaderText = Translation.ResultView_ShootDownFixed;
		ResultView_ShootDownFixed.ToolTipText = Translation.ResultView_ShootDownFixedToolTip;
		ResultView_ShootDownFailed.HeaderText = Translation.ResultView_ShootDownFailed;
		ResultView_ShootDownFailed.ToolTipText = Translation.ResultView_ShootDownFailedToolTip;
		ResultView_AARocketBarrageProbability.HeaderText = Translation.ResultView_AARocketBarrageProbability;
		ResultView_AARocketBarrageProbability.ToolTipText = Translation.ResultView_AARocketBarrageProbabilityToolTip;

		Text = Translation.Title;
	}

	private void DialogAntiAirDefense_Load(object sender, EventArgs e)
	{

		if (!KCDatabase.Instance.Fleet.IsAvailable)
		{
			MessageBox.Show(Translation.DataNotLoaded, Translation.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			Close();
			return;
		}

		if (FleetID.SelectedIndex == -1)
			FleetID.SelectedIndex = 0;
		Formation.SelectedIndex = 0;

		UpdateAACutinKind(ShowAll.Checked);
		UpdateFormation();

		this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormAntiAirDefense]);
	}

	private void DialogAntiAirDefense_FormClosed(object sender, FormClosedEventArgs e)
	{
		ResourceManager.DestroyIcon(Icon);
	}


	public void SetFleetID(int id)
	{
		FleetID.SelectedIndex = id - 1;
	}

	private void Updated()
	{

		IShipData[] ships = GetShips().ToArray();
		int formation = Formation.SelectedItem as FormationComboBoxData;
		int aaCutinKind = AACutinKind.SelectedItem as AACutinComboBoxData;
		AntiAirCutIn cutIn = AntiAirCutIn.AllCutIns
			.FirstOrDefault(a => a.Id == aaCutinKind)
			?? AntiAirCutIn.AllCutIns.First(a => a.Id == 0);
		int enemyAircraftCount = enemySlotCountValue;


		// 加重対空値
		double[] adjustedAAs = ships.Select(s => s == null ? 0.0 : Calculator.GetAdjustedAAValue(s)).ToArray();

		// 艦隊防空値
		double adjustedFleetAA = Calculator.GetAdjustedFleetAAValue(ships, formation);

		// 割合撃墜
		double[] proportionalAAs = adjustedAAs.Select((val, i) => Calculator.GetProportionalAirDefense(val, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();

		// 固定撃墜
		int[] fixedAAs = adjustedAAs.Select((val, i) => Calculator.GetFixedAirDefense(val, adjustedFleetAA, cutIn, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();



		int[] shootDownBoth = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], fixedAAs[i], cutIn)).ToArray();

		int[] shootDownProportional = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], 0, cutIn)).ToArray();

		int[] shootDownFixed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, 0, fixedAAs[i], cutIn)).ToArray();

		int[] shootDownFailed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, 0, 0, cutIn)).ToArray();

		double[] aaRocketBarrageProbability = ships.Select(ship => Calculator.GetAARocketBarrageProbability(ship)).ToArray();


		ResultView.Rows.Clear();
		var rows = new DataGridViewRow[ships.Length];
		for (int i = 0; i < ships.Length; i++)
		{
			if (ships[i] == null)
				continue;

			rows[i] = new DataGridViewRow();
			rows[i].CreateCells(ResultView);

			rows[i].SetValues(
				ships[i].Name,
				ships[i].AABase,
				adjustedAAs[i],
				proportionalAAs[i],
				fixedAAs[i],
				shootDownBoth[i],
				shootDownProportional[i],
				shootDownFixed[i],
				shootDownFailed[i],
				aaRocketBarrageProbability[i]);

		}
		ResultView.Rows.AddRange(rows.Where(r => r != null).ToArray());

		AdjustedFleetAA.Text = adjustedFleetAA.ToString("0.0");
		{
			var allShootDown = shootDownBoth.Concat(shootDownProportional).Concat(shootDownFixed).Concat(shootDownFailed);
			AnnihilationProbability.Text = (allShootDown.Count(i => i >= enemyAircraftCount) / Math.Max(ships.Count(s => s != null) * 4, 1.0)).ToString("p1");
		}
	}


	private IEnumerable<IShipData> GetShips()
	{
		if (FleetID.SelectedIndex < 4)
			return KCDatabase.Instance.Fleet[FleetID.SelectedIndex + 1].MembersWithoutEscaped;
		else
			return KCDatabase.Instance.Fleet[1].MembersWithoutEscaped.Concat(KCDatabase.Instance.Fleet[2].MembersWithoutEscaped);
	}

	private bool IsCombined => FleetID.SelectedIndex == 4;


	private void UpdateAACutinKind(bool showAll)
	{

		AACutinComboBoxData[] list;

		if (showAll)
		{

			int max = AntiAirCutIn.AllCutIns.Max(a => a.Id);
			list = Enumerable.Range(0, max + 1).Select(kind => new AACutinComboBoxData(kind)).ToArray();

		}
		else
		{

			list = GetShips()
				.Where(s => s != null)
				.Select(s => Calculator.GetAACutinKind(s.ShipID, s.AllSlotMaster.ToArray()))
				.Concat(Enumerable.Repeat(0, 1))
				.Distinct()
				.OrderBy(i => i)
				.Select(kind => new AACutinComboBoxData(kind)).ToArray();

		}

		AACutinKind.Items.Clear();
		AACutinKind.Items.AddRange(list);
		AACutinKind.SelectedIndex = 0;
	}

	private void UpdateFormation()
	{
		var items = (IsCombined ? Enumerable.Range(11, 4) : Enumerable.Range(1, 6))
			.Select(i => new FormationComboBoxData(i)).ToArray();

		int selected = Formation.SelectedItem as FormationComboBoxData;
		int index = Array.FindIndex(items, item => item == selected);

		Formation.Items.Clear();
		Formation.Items.AddRange(items);
		Formation.SelectedIndex = Math.Max(index, 0);
	}


	private void ResultView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{

		if (e.ColumnIndex == ResultView_ShootDownBoth.Index ||
			e.ColumnIndex == ResultView_ShootDownProportional.Index ||
			e.ColumnIndex == ResultView_ShootDownFixed.Index ||
			e.ColumnIndex == ResultView_ShootDownFailed.Index)
		{

			int value = e.Value as int? ?? 0;
			int enemySlot = enemySlotCountValue;

			e.Value = string.Format("{0} ({1:p0})", value, (double)value / enemySlot);
			e.FormattingApplied = true;
			e.CellStyle.BackColor = e.CellStyle.SelectionBackColor =
				value >= enemySlot ? Color.MistyRose : SystemColors.Window;
		}

	}



	private void FleetID_SelectedIndexChanged(object sender, EventArgs e)
	{
		Updated();
		UpdateAACutinKind(ShowAll.Checked);
		UpdateFormation();
	}

	private void Formation_SelectedIndexChanged(object sender, EventArgs e)
	{
		Updated();
	}

	private void AACutinKind_SelectedIndexChanged(object sender, EventArgs e)
	{
		Updated();
	}

	private void EnemySlotCount_ValueChanged(object sender, EventArgs e)
	{
		enemySlotCountValue = (int)EnemySlotCount.Value;
		Updated();
	}

	private void ShowAll_CheckedChanged(object sender, EventArgs e)
	{
		UpdateAACutinKind(ShowAll.Checked);
	}


}
