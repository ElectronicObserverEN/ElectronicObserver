﻿using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Support;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Translation = ElectronicObserver.Properties.Window.FormShipGroup;

namespace ElectronicObserver.Window
{
	public partial class FormShipGroup : DockContent
	{


		/// <summary>タブ背景色(アクティブ)</summary>
		private readonly Color TabActiveColor = Color.FromArgb(0xFF, 0xFF, 0xCC);

		/// <summary>タブ背景色(非アクティブ)</summary>
		private readonly Color TabInactiveColor = SystemColors.Control;



		// セル背景色
		private readonly Color CellColorRed = Color.FromArgb(0xFF, 0xBB, 0xBB);
		private readonly Color CellColorOrange = Color.FromArgb(0xFF, 0xDD, 0xBB);
		private readonly Color CellColorYellow = Color.FromArgb(0xFF, 0xFF, 0xBB);
		private readonly Color CellColorGreen = Color.FromArgb(0xBB, 0xFF, 0xBB);
		private readonly Color CellColorGray = Color.FromArgb(0xBB, 0xBB, 0xBB);
		private readonly Color CellColorCherry = Color.FromArgb(0xFF, 0xDD, 0xDD);
		private readonly Color CellColorBlue = Color.FromArgb(173, 216, 230);
		private readonly Color CellColorPurple = Color.FromArgb(156, 143, 238);
		private readonly Color CellColorCyan = Color.FromArgb(224, 255, 255);

		//セルスタイル
		private DataGridViewCellStyle CSDefaultLeft, CSDefaultCenter, CSDefaultRight,
			CSRedRight, CSOrangeRight, CSYellowRight, CSGreenRight, CSGrayRight, CSCherryRight,
			CSBlueRight, CSPurpleRight, CSCyanRight, CSIsLocked;

		/// <summary>選択中のタブ</summary>
		private ImageLabel SelectedTab = null;

		/// <summary>選択中のグループ</summary>
		private ShipGroupData CurrentGroup => SelectedTab == null ? null : KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];


		private bool IsRowsUpdating;
		private int _splitterDistance;
		private int _shipNameSortMethod;


		public FormShipGroup(FormMain parent)
		{
			InitializeComponent();

			ControlHelper.SetDoubleBuffered(ShipView);

			IsRowsUpdating = true;
			_splitterDistance = -1;

			foreach (DataGridViewColumn column in ShipView.Columns)
			{
				column.MinimumWidth = 2;
			}


			#region set CellStyle

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

			CSDefaultCenter = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleCenter
			};

			CSDefaultRight = new DataGridViewCellStyle(CSDefaultLeft)
			{
				Alignment = DataGridViewContentAlignment.MiddleRight
			};

			CSRedRight = new DataGridViewCellStyle(CSDefaultRight);
			CSRedRight.BackColor =
			CSRedRight.SelectionBackColor = CellColorRed;

			CSOrangeRight = new DataGridViewCellStyle(CSDefaultRight);
			CSOrangeRight.BackColor =
			CSOrangeRight.SelectionBackColor = CellColorOrange;

			CSYellowRight = new DataGridViewCellStyle(CSDefaultRight);
			CSYellowRight.BackColor =
			CSYellowRight.SelectionBackColor = CellColorYellow;

			CSGreenRight = new DataGridViewCellStyle(CSDefaultRight);
			CSGreenRight.BackColor =
			CSGreenRight.SelectionBackColor = CellColorGreen;

			CSGrayRight = new DataGridViewCellStyle(CSDefaultRight);
			CSGrayRight.ForeColor =
			CSGrayRight.SelectionForeColor = CellColorGray;

			CSCherryRight = new DataGridViewCellStyle(CSDefaultRight);
			CSCherryRight.BackColor =
			CSCherryRight.SelectionBackColor = CellColorCherry;

			CSBlueRight = new DataGridViewCellStyle(CSDefaultRight);
			CSBlueRight.BackColor =
			CSBlueRight.SelectionBackColor = CellColorBlue;

			CSPurpleRight = new DataGridViewCellStyle(CSDefaultRight);
			CSPurpleRight.BackColor =
				CSPurpleRight.SelectionBackColor = CellColorPurple;

			CSCyanRight = new DataGridViewCellStyle(CSDefaultRight);
			CSCyanRight.BackColor =
				CSCyanRight.SelectionBackColor = CellColorCyan;

			CSIsLocked = new DataGridViewCellStyle(CSDefaultCenter);
			CSIsLocked.ForeColor =
			CSIsLocked.SelectionForeColor = Color.FromArgb(0xFF, 0x88, 0x88);


			ShipView.DefaultCellStyle = CSDefaultRight;
			ShipView_Name.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot1.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot2.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot3.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot4.DefaultCellStyle = CSDefaultLeft;
			ShipView_Slot5.DefaultCellStyle = CSDefaultLeft;
			ShipView_ExpansionSlot.DefaultCellStyle = CSDefaultLeft;

			#endregion


			SystemEvents.SystemShuttingDown += SystemShuttingDown;

			Translate();
		}

		public void Translate()
		{
			ShipView_ShipType.HeaderText = Translation.ShipView_ShipType;
			ShipView_Name.HeaderText = Translation.ShipView_Name;
			ShipView_NextRemodel.HeaderText = Translation.ShipView_NextRemodel;

			ShipView_Fuel.HeaderText = GeneralRes.Fuel;
			ShipView_Ammo.HeaderText = GeneralRes.Ammo;
			ShipView_Slot1.HeaderText = Translation.ShipView_Slot1;
			ShipView_Slot2.HeaderText = Translation.ShipView_Slot2;
			ShipView_Slot3.HeaderText = Translation.ShipView_Slot3;
			ShipView_Slot4.HeaderText = Translation.ShipView_Slot4;
			ShipView_Slot5.HeaderText = Translation.ShipView_Slot5;
			ShipView_ExpansionSlot.HeaderText = GeneralRes.Expansion;

			ShipView_Aircraft1.HeaderText = GeneralRes.Planes + " 1";
			ShipView_Aircraft2.HeaderText = GeneralRes.Planes + " 2";
			ShipView_Aircraft3.HeaderText = GeneralRes.Planes + " 3";
			ShipView_Aircraft4.HeaderText = GeneralRes.Planes + " 4";
			ShipView_Aircraft5.HeaderText = GeneralRes.Planes + " 5";
			ShipView_AircraftTotal.HeaderText = GeneralRes.Planes + GeneralRes.Total;

			ShipView_Fleet.HeaderText = Translation.ShipView_Fleet;
			ShipView_RepairTime.HeaderText = Translation.ShipView_RepairTime;
			ShipView_RepairSteel.HeaderText = Translation.ShipView_RepairSteel;
			ShipView_RepairFuel.HeaderText = Translation.ShipView_RepairFuel;

			ShipView_Firepower.HeaderText = GeneralRes.Firepower;
			ShipView_FirepowerRemain.HeaderText = GeneralRes.Firepower + GeneralRes.ModRemaining;
			ShipView_FirepowerTotal.HeaderText = GeneralRes.Firepower + GeneralRes.Total;

			ShipView_Torpedo.HeaderText = GeneralRes.Torpedo;
			ShipView_TorpedoRemain.HeaderText = GeneralRes.Torpedo + GeneralRes.ModRemaining;
			ShipView_TorpedoTotal.HeaderText = GeneralRes.Torpedo + GeneralRes.Total;

			ShipView_AA.HeaderText = GeneralRes.AntiAir;
			ShipView_AARemain.HeaderText = GeneralRes.AntiAir + GeneralRes.ModRemaining;
			ShipView_AATotal.HeaderText = GeneralRes.AntiAir + GeneralRes.Total;

			ShipView_Armor.HeaderText = GeneralRes.Armor;
			ShipView_ArmorRemain.HeaderText = GeneralRes.Armor + GeneralRes.ModRemaining;
			ShipView_ArmorTotal.HeaderText = GeneralRes.Armor + GeneralRes.Total;

			ShipView_ASW.HeaderText = GeneralRes.ASW;
			ShipView_ASWTotal.HeaderText = GeneralRes.ASW + GeneralRes.Total;

			ShipView_Evasion.HeaderText = GeneralRes.Evasion;
			ShipView_EvasionTotal.HeaderText = GeneralRes.Evasion + GeneralRes.Total;

			ShipView_LOS.HeaderText = GeneralRes.LoS;
			ShipView_LOSTotal.HeaderText = GeneralRes.LoS + GeneralRes.Total;

			ShipView_Luck.HeaderText = GeneralRes.Luck;
			ShipView_LuckRemain.HeaderText = GeneralRes.Luck + GeneralRes.ModRemaining;
			ShipView_LuckTotal.HeaderText = GeneralRes.Luck + GeneralRes.Total;

			ShipView_BomberTotal.HeaderText = GeneralRes.Bombers + GeneralRes.Total;
			ShipView_Speed.HeaderText = GeneralRes.Speed;
			ShipView_Range.HeaderText = GeneralRes.Range;

			ShipView_AirBattlePower.HeaderText = GeneralRes.Air + GeneralRes.Power;
			ShipView_ShellingPower.HeaderText = GeneralRes.Shelling + GeneralRes.Power;
			ShipView_AircraftPower.HeaderText = GeneralRes.Bombing + GeneralRes.Power;
			ShipView_AntiSubmarinePower.HeaderText = GeneralRes.ASW + GeneralRes.Power;
			ShipView_TorpedoPower.HeaderText = GeneralRes.Torpedo + GeneralRes.Power;
			ShipView_NightBattlePower.HeaderText = Translation.ShipView_NightBattlePower;

			ShipView_Locked.HeaderText = GeneralRes.Lock;
			ShipView_SallyArea.HeaderText = Translation.ShipView_SallyArea;

			MenuMember_AddToGroup.Text = Translation.MenuMember_AddToGroup;
			MenuMember_CreateGroup.Text = Translation.MenuMember_CreateGroup;
			MenuMember_Exclude.Text = Translation.MenuMember_Exclude;
			MenuMember_Filter.Text = Translation.MenuMember_Filter;
			MenuMember_ColumnFilter.Text = Translation.MenuMember_ColumnFilter;
			MenuMember_SortOrder.Text = Translation.MenuMember_SortOrder;
			MenuMember_CSVOutput.Text = Translation.MenuMember_CSVOutput;

			MenuGroup_Add.Text = Translation.MenuGroup_Add;
			MenuGroup_Copy.Text = Translation.MenuGroup_Copy;
			MenuGroup_Rename.Text = Translation.MenuGroup_Rename;
			MenuGroup_Delete.Text = Translation.MenuGroup_Delete;
			MenuGroup_AutoUpdate.Text = Translation.MenuGroup_AutoUpdate;
			MenuGroup_ShowStatusBar.Text = Translation.MenuGroup_ShowStatusBar;

			Text = Translation.Title;
		}

        private void FormShipGroup_Load(object sender, EventArgs e)
        {

            ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


            // 空(≒初期状態)の時、おなじみ全所属艦を追加
            if (groups.ShipGroups.Count == 0)
            {

                Utility.Logger.Add(3, string.Format(Translation.GroupNotFound, ShipGroupManager.DefaultFilePath));

				var group = KCDatabase.Instance.ShipGroup.Add();
				group.Name = GeneralRes.AllAssignedShips;

                for (int i = 0; i < ShipView.Columns.Count; i++)
                {
                    var newdata = new ShipGroupData.ViewColumnData(ShipView.Columns[i]);
                    if (SelectedTab == null)
                        newdata.Visible = true;     //初期状態では全行が非表示のため
                    group.ViewColumns.Add(ShipView.Columns[i].Name, newdata);
                }
            }


            foreach (var g in groups.ShipGroups.Values)
            {
                TabPanel.Controls.Add(CreateTabLabel(g.GroupID));
            }


            //*/
            {
                int columnCount = ShipView.Columns.Count;
                for (int i = 0; i < columnCount; i++)
                {
                    ShipView.Columns[i].Visible = false;
                }
            }
            //*/


            ConfigurationChanged();


            APIObserver o = APIObserver.Instance;

            o.APIList["api_port/port"].ResponseReceived += APIUpdated;
            o.APIList["api_get_member/ship2"].ResponseReceived += APIUpdated;
            o.APIList["api_get_member/ship_deck"].ResponseReceived += APIUpdated;


            Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

            IsRowsUpdating = false;
            Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormShipGroup]);

        }


        void ConfigurationChanged()
        {

            var config = Utility.Configuration.Config;

            ShipView.Font = StatusBar.Font = Font = config.UI.MainFont;

            CSDefaultLeft.Font =
            CSDefaultCenter.Font =
            CSDefaultRight.Font =
            CSRedRight.Font =
            CSOrangeRight.Font =
            CSYellowRight.Font =
            CSGreenRight.Font =
            CSGrayRight.Font =
            CSCherryRight.Font =
            CSIsLocked.Font =
                config.UI.MainFont;

            foreach (System.Windows.Forms.Control c in TabPanel.Controls)
                c.Font = Font;

            MenuGroup_AutoUpdate.Checked = config.FormShipGroup.AutoUpdate;
            MenuGroup_ShowStatusBar.Checked = config.FormShipGroup.ShowStatusBar;
            _shipNameSortMethod = config.FormShipGroup.ShipNameSortMethod;


            int rowHeight;
            if (config.UI.IsLayoutFixed)
            {
                ShipView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                ShipView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                rowHeight = 21;
            }
            else
            {
                ShipView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                ShipView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

                if (ShipView.Rows.Count > 0)
                    rowHeight = ShipView.Rows[0].GetPreferredHeight(0, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, false);
                else
                    rowHeight = 21;
            }

            foreach (DataGridViewRow row in ShipView.Rows)
            {
                row.Height = rowHeight;
            }

        }


        // レイアウトロード時に呼ばれる
        public void ConfigureFromPersistString(string persistString)
        {

            string[] args = persistString.Split("?=&".ToCharArray());

            for (int i = 1; i < args.Length - 1; i += 2)
            {
                switch (args[i])
                {
                    case "SplitterDistance":
                        // 直接変えるとサイズが足りないか何かで変更が適用されないことがあるため、 Resize イベント中に変更する(ために値を記録する)
                        // しかし Resize イベントだけだと呼ばれないことがあるため、直接変えてもおく
                        // つらい
                        splitContainer1.SplitterDistance = _splitterDistance = int.Parse(args[i + 1]);
                        break;
                }
            }
        }


        protected override string GetPersistString() => "ShipGroup?SplitterDistance=" + splitContainer1.SplitterDistance;



        /// <summary>
        /// 指定したグループIDに基づいてタブ ラベルを生成します。
        /// </summary>
        private ImageLabel CreateTabLabel(int id)
        {

            ImageLabel label = new ImageLabel
            {
                Text = KCDatabase.Instance.ShipGroup[id].Name,
                Anchor = AnchorStyles.Left,
                Font = ShipView.Font,
                BackColor = TabInactiveColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(4, 4, 4, 4),
                Margin = new Padding(0, 0, 0, 0),
                ImageAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Cursor = Cursors.Hand
            };

            //イベントと固有IDの追加(内部データとの紐付)
            label.Click += TabLabel_Click;
            label.MouseDown += TabLabel_MouseDown;
            label.MouseMove += TabLabel_MouseMove;
            label.MouseUp += TabLabel_MouseUp;
            label.ContextMenuStrip = MenuGroup;
            label.Tag = id;

            return label;
        }




        void TabLabel_Click(object sender, EventArgs e)
        {
            ChangeShipView(sender as ImageLabel);
        }

        private void APIUpdated(string apiname, dynamic data)
        {
            if (MenuGroup_AutoUpdate.Checked)
                ChangeShipView(SelectedTab);
        }






        /// <summary>
        /// ShipView用の新しい行のインスタンスを作成します。
        /// </summary>
        /// <param name="ship">追加する艦娘データ。</param>
        private DataGridViewRow CreateShipViewRow(ShipData ship)
        {

            if (ship == null) return null;

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(ShipView);
            row.Height = 21;

            row.SetValues(
                ship.MasterID,
                ship.MasterShip.ShipType,
                ship.MasterShip.NameEN,
                ship.Level,
                ship.ExpTotal,
                ship.ExpNext,
                ship.ExpNextRemodel,
                new Fraction(ship.HPCurrent, ship.HPMax),
                ship.Condition,
                new Fraction(ship.Fuel, ship.FuelMax),
                new Fraction(ship.Ammo, ship.AmmoMax),
                GetEquipmentString(ship, 0),
                GetEquipmentString(ship, 1),
                GetEquipmentString(ship, 2),
                GetEquipmentString(ship, 3),
                GetEquipmentString(ship, 4),
                GetEquipmentString(ship, 5),        //補強スロット
                new Fraction(ship.Aircraft[0], ship.MasterShip.Aircraft[0]),
                new Fraction(ship.Aircraft[1], ship.MasterShip.Aircraft[1]),
                new Fraction(ship.Aircraft[2], ship.MasterShip.Aircraft[2]),
                new Fraction(ship.Aircraft[3], ship.MasterShip.Aircraft[3]),
                new Fraction(ship.Aircraft[4], ship.MasterShip.Aircraft[4]),
                new Fraction(ship.AircraftTotal, ship.MasterShip.AircraftTotal),
                ship.FleetWithIndex,
                ship.RepairingDockID == -1 ? ship.RepairTime : -1000 + ship.RepairingDockID,
                ship.RepairSteel,
                ship.RepairFuel,
                ship.FirepowerBase,
                ship.FirepowerRemain,
                ship.FirepowerTotal,
                ship.TorpedoBase,
                ship.TorpedoRemain,
                ship.TorpedoTotal,
                ship.AABase,
                ship.AARemain,
                ship.AATotal,
                ship.ArmorBase,
                ship.ArmorRemain,
                ship.ArmorTotal,
                ship.ASWBase,
                ship.ASWTotal,
                ship.EvasionBase,
                ship.EvasionTotal,
                ship.LOSBase,
                ship.LOSTotal,
                ship.LuckBase,
                ship.LuckRemain,
                ship.LuckTotal,
                ship.BomberTotal,
                ship.Speed,
                ship.Range,
                ship.AirBattlePower,
                ship.ShellingPower,
                ship.AircraftPower,
                ship.AntiSubmarinePower,
                ship.TorpedoPower,
                ship.NightBattlePower,
                ship.IsLocked ? 1 : ship.IsLockedByEquipment ? 2 : 0,
                ship.SallyArea
                );


            row.Cells[ShipView_Name.Index].Tag = ship.ShipID;
            //row.Cells[ShipView_Level.Index].Tag = ship.ExpTotal;


            {
                DataGridViewCellStyle cs;
                double hprate = ship.HPRate;
                if (hprate <= 0.25)
                    cs = CSRedRight;
                else if (hprate <= 0.50)
                    cs = CSOrangeRight;
                else if (hprate <= 0.75)
                    cs = CSYellowRight;
                else if (hprate < 1.00)
                    cs = CSGreenRight;
                else
                    cs = CSDefaultRight;

                row.Cells[ShipView_HP.Index].Style = cs;
            }
            {
                DataGridViewCellStyle cs;
                if (ship.Condition < 20)
                    cs = CSRedRight;
                else if (ship.Condition < 30)
                    cs = CSOrangeRight;
                else if (ship.Condition < Utility.Configuration.Config.Control.ConditionBorder)
                    cs = CSYellowRight;
                else if (ship.Condition < 50)
                    cs = CSDefaultRight;
                else
                    cs = CSGreenRight;

                row.Cells[ShipView_Condition.Index].Style = cs;
            }
            row.Cells[ShipView_Fuel.Index].Style = ship.Fuel < ship.FuelMax ? CSYellowRight : CSDefaultRight;
            row.Cells[ShipView_Ammo.Index].Style = ship.Ammo < ship.AmmoMax ? CSYellowRight : CSDefaultRight;
            {
                var current = ship.Aircraft;
                var max = ship.MasterShip.Aircraft;
                row.Cells[ShipView_Aircraft1.Index].Style = (max[0] > 0 && current[0] == 0) ? CSRedRight : (current[0] < max[0]) ? CSYellowRight : CSDefaultRight;
                row.Cells[ShipView_Aircraft2.Index].Style = (max[1] > 0 && current[1] == 0) ? CSRedRight : (current[1] < max[1]) ? CSYellowRight : CSDefaultRight;
                row.Cells[ShipView_Aircraft3.Index].Style = (max[2] > 0 && current[2] == 0) ? CSRedRight : (current[2] < max[2]) ? CSYellowRight : CSDefaultRight;
                row.Cells[ShipView_Aircraft4.Index].Style = (max[3] > 0 && current[3] == 0) ? CSRedRight : (current[3] < max[3]) ? CSYellowRight : CSDefaultRight;
                row.Cells[ShipView_Aircraft5.Index].Style = (max[4] > 0 && current[4] == 0) ? CSRedRight : (current[4] < max[4]) ? CSYellowRight : CSDefaultRight;
                row.Cells[ShipView_AircraftTotal.Index].Style = (ship.MasterShip.AircraftTotal > 0 && ship.AircraftTotal == 0) ? CSRedRight : (ship.AircraftTotal < ship.MasterShip.AircraftTotal) ? CSYellowRight : CSDefaultRight;
            }
            {
                DataGridViewCellStyle cs;
                if (ship.RepairTime == 0)
                    cs = CSDefaultRight;
                else if (ship.RepairTime < 1000 * 60 * 60)
                    cs = CSYellowRight;
                else if (ship.RepairTime < 1000 * 60 * 60 * 6)
                    cs = CSOrangeRight;
                else
                    cs = CSRedRight;

				row.Cells[ShipView_RepairTime.Index].Style = cs;
			}
            row.Cells[ShipView_Firepower.Index].Style = row.Cells[ShipView_FirepowerTotal.Index].Style = CSRedRight;
            row.Cells[ShipView_FirepowerRemain.Index].Style = ship.FirepowerRemain == 0 ? CSRedRight : CSDefaultRight;

            row.Cells[ShipView_Torpedo.Index].Style = row.Cells[ShipView_TorpedoTotal.Index].Style = ship.TorpedoTotal > 0 ? CSBlueRight : CSDefaultRight;
            row.Cells[ShipView_TorpedoRemain.Index].Style = ship.TorpedoTotal > 0 && ship.TorpedoRemain == 0 ? CSBlueRight : CSDefaultRight;

            row.Cells[ShipView_AA.Index].Style = row.Cells[ShipView_AATotal.Index].Style = ship.AATotal > 0 ? CSOrangeRight : CSDefaultRight;
            row.Cells[ShipView_AARemain.Index].Style = ship.AATotal > 0 && ship.AARemain == 0 ? CSOrangeRight : CSDefaultRight;

            row.Cells[ShipView_Armor.Index].Style = row.Cells[ShipView_ArmorTotal.Index].Style = CSYellowRight;
            row.Cells[ShipView_ArmorRemain.Index].Style = ship.ArmorRemain == 0 ? CSYellowRight : CSDefaultRight;

            row.Cells[ShipView_Luck.Index].Style = row.Cells[ShipView_LuckTotal.Index].Style = CSGreenRight;
            row.Cells[ShipView_LuckRemain.Index].Style = ship.LuckRemain == 0 ? CSGreenRight : CSDefaultRight;

		    row.Cells[ShipView_ASW.Index].Style = row.Cells[ShipView_ASWTotal.Index].Style = ship.ASWTotal > 0 ? CSPurpleRight : CSDefaultRight;
		    row.Cells[ShipView_LOS.Index].Style = row.Cells[ShipView_LOSTotal.Index].Style = CSCyanRight;
		    row.Cells[ShipView_Evasion.Index].Style = row.Cells[ShipView_EvasionTotal.Index].Style = CSRedRight;


            row.Cells[ShipView_Locked.Index].Style = ship.IsLocked ? CSIsLocked : CSDefaultCenter;

            return row;
        }


        /// <summary>
        /// 指定したタブのグループのShipViewを作成します。
        /// </summary>
        /// <param name="target">作成するビューのグループデータ</param>
        private void BuildShipView(ImageLabel target)
        {
            if (target == null)
                return;

            ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)target.Tag];

            IsRowsUpdating = true;
            ShipView.SuspendLayout();

            UpdateMembers(group);

            ShipView.Rows.Clear();

            var ships = group.MembersInstance;
            var rows = new List<DataGridViewRow>(ships.Count());

            foreach (var ship in ships)
            {
                if (ship == null) continue;

                DataGridViewRow row = CreateShipViewRow(ship);
                rows.Add(row);
            }

            for (int i = 0; i < rows.Count; i++)
                rows[i].Tag = i;

            ShipView.Rows.AddRange(rows.ToArray());



            // 設定に抜けがあった場合補充
            if (group.ViewColumns == null)
            {
                group.ViewColumns = new Dictionary<string, ShipGroupData.ViewColumnData>();
            }
            if (ShipView.Columns.Count != group.ViewColumns.Count)
            {
                foreach (DataGridViewColumn column in ShipView.Columns)
                {

                    if (!group.ViewColumns.ContainsKey(column.Name))
                    {
                        var newdata = new ShipGroupData.ViewColumnData(column)
                        {
                            Visible = true     //初期状態でインビジだと不都合なので
                        };

                        group.ViewColumns.Add(newdata.Name, newdata);
                    }
                }
            }


            ApplyViewData(group);
            ApplyAutoSort(group);


            // 高さ設定(追加直後に実行すると高さが0になることがあるのでここで実行)
            int rowHeight = 21;
            if (!Utility.Configuration.Config.UI.IsLayoutFixed)
            {
                if (ShipView.Rows.Count > 0)
                    rowHeight = ShipView.Rows[0].GetPreferredHeight(0, DataGridViewAutoSizeRowMode.AllCellsExceptHeader, false);
            }

            foreach (DataGridViewRow row in ShipView.Rows)
                row.Height = rowHeight;


            ShipView.ResumeLayout();
            IsRowsUpdating = false;

			//status bar
			if (KCDatabase.Instance.Ships.Count > 0)
			{
				int membersCount = group.MembersInstance.Count(s => s != null);
				int levelsum = group.MembersInstance.Sum(s => s?.Level ?? 0);
				double levelAverage = levelsum / Math.Max(membersCount, 1.0);
				int expsum = group.MembersInstance.Sum(s => s?.ExpTotal ?? 0);
				double expAverage = expsum / Math.Max(membersCount, 1.0);

				Status_ShipCount.Text = string.Format(Translation.ShipCount, group.Members.Count);
				Status_LevelTotal.Text = string.Format(Translation.TotalAndAverageLevel, levelsum, levelAverage);
				Status_LevelAverage.Text = string.Format(Translation.TotalAndAverageExp, expsum, expAverage);
            }
		}


        /// <summary>
        /// ShipViewを指定したタブに切り替えます。
        /// </summary>
        private void ChangeShipView(ImageLabel target)
        {

            if (target == null) return;


            var group = KCDatabase.Instance.ShipGroup[(int)target.Tag];
            var currentGroup = CurrentGroup;

            int headIndex = 0;
            List<int> selectedIDList = new List<int>();

			if (group == null)
			{
				Utility.Logger.Add(3, Translation.GroupDoesNotExist);
				return;
			}

            if (currentGroup != null)
            {

                UpdateMembers(currentGroup);

                if (CurrentGroup.GroupID != group.GroupID)
                {
                    ShipView.Rows.Clear();      //別グループの行の並び順を引き継がせないようにする

                }
                else
                {
                    headIndex = ShipView.FirstDisplayedScrollingRowIndex;
                    selectedIDList = ShipView.SelectedRows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[ShipView_ID.Index].Value).ToList();
                }
            }


            if (SelectedTab != null)
                SelectedTab.BackColor = TabInactiveColor;

            SelectedTab = target;


            BuildShipView(SelectedTab);
            SelectedTab.BackColor = TabActiveColor;


            if (0 <= headIndex && headIndex < ShipView.Rows.Count)
            {
                try
                {
                    ShipView.FirstDisplayedScrollingRowIndex = headIndex;

                }
                catch (InvalidOperationException)
                {
                    // 1行も表示できないサイズのときに例外が出るので握りつぶす
                }
            }

            if (selectedIDList.Count > 0)
            {
                ShipView.ClearSelection();
                for (int i = 0; i < ShipView.Rows.Count; i++)
                {
                    var row = ShipView.Rows[i];
                    if (selectedIDList.Contains((int)row.Cells[ShipView_ID.Index].Value))
                    {
                        row.Selected = true;
                    }
                }
            }

        }


        private string GetEquipmentString(ShipData ship, int index)
        {

			if (index < 5)
			{
				return (index >= ship.SlotSize && ship.Slot[index] == -1) ? "" :
					ship.SlotInstance[index]?.NameWithLevel ?? Translation.None;
			}
			else
			{
				return ship.ExpansionSlot == 0 ? "" :
					ship.ExpansionSlotInstance?.NameWithLevel ?? Translation.None;
			}

        }


        /// <summary>
        /// 現在選択している艦船のIDリストを求めます。
        /// </summary>
        private IEnumerable<int> GetSelectedShipID()
        {
            return ShipView.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => (int)r.Cells[ShipView_ID.Index].Value);
        }


        /// <summary>
        /// 現在の表を基に、グループメンバーを更新します。
        /// </summary>
        private void UpdateMembers(ShipGroupData group)
        {
            group.UpdateMembers(ShipView.Rows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[ShipView_ID.Index].Value));
        }


        private void ShipView_SelectionChanged(object sender, EventArgs e)
        {

			var group = CurrentGroup;
			if (KCDatabase.Instance.Ships.Count > 0 && group != null)
			{
				if (ShipView.Rows.GetRowCount(DataGridViewElementStates.Selected) >= 2)
				{
					int selectedShipCount = ShipView.Rows.GetRowCount(DataGridViewElementStates.Selected);
					int totalShipCount = group.Members.Count;
					var levels = ShipView.SelectedRows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[ShipView_Level.Index].Value);
                    var exp = ShipView.SelectedRows.Cast<DataGridViewRow>().Select(r => (int)r.Cells[ShipView_Exp.Index].Value);
                    
					Status_ShipCount.Text = string.Format(Translation.SelectedShips, selectedShipCount, totalShipCount);
					Status_LevelTotal.Text = string.Format(Translation.TotalAndAverageLevel, levels.Sum(), levels.Average());
					Status_LevelAverage.Text = string.Format(Translation.TotalAndAverageExp, exp.Sum(), exp.Average());

                }
				else
				{
					int membersCount = group.MembersInstance.Count(s => s != null);
					int levelsum = group.MembersInstance.Sum(s => s?.Level ?? 0);
					double levelAverage = levelsum / Math.Max(membersCount, 1.0);
					int expsum = group.MembersInstance.Sum(s => s?.ExpTotal ?? 0);
					double expAverage = expsum / Math.Max(membersCount, 1.0);

					Status_ShipCount.Text = string.Format(Translation.ShipCount, group.Members.Count);
					Status_LevelTotal.Text = string.Format(Translation.TotalAndAverageLevel, levelsum, levelAverage);
					Status_LevelAverage.Text = string.Format(Translation.TotalAndAverageExp, expsum, expAverage);
				}

            }
            else
            {
                Status_ShipCount.Text =
                Status_LevelTotal.Text =
                Status_LevelAverage.Text = "";
            }
        }


		private void ShipView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{

			if (e.ColumnIndex == ShipView_ShipType.Index)
			{
				e.Value = KCDatabase.Instance.ShipTypes[(int)e.Value].NameEN;
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_Fleet.Index)
			{
				if (e.Value == null)
					e.Value = "";
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_RepairTime.Index)
			{

				if ((int)e.Value < 0)
				{
					e.Value = $"{Translation.Dock} #" + ((int)e.Value + 1000);
				}
				else
				{
					e.Value = DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan((int)e.Value));
				}
				e.FormattingApplied = true;

			}
			else if ((
			  e.ColumnIndex == ShipView_FirepowerRemain.Index ||
			  e.ColumnIndex == ShipView_TorpedoRemain.Index ||
			  e.ColumnIndex == ShipView_AARemain.Index ||
			  e.ColumnIndex == ShipView_ArmorRemain.Index ||
			  e.ColumnIndex == ShipView_LuckRemain.Index
			  ) && (int)e.Value == 0)
			{
				e.Value = "MAX";
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_Aircraft1.Index ||
			   e.ColumnIndex == ShipView_Aircraft2.Index ||
			   e.ColumnIndex == ShipView_Aircraft3.Index ||
			   e.ColumnIndex == ShipView_Aircraft4.Index ||
			   e.ColumnIndex == ShipView_Aircraft5.Index)
			{   // AircraftTotal は 0 でも表示する
				if (((Fraction)e.Value).Max == 0)
				{
					e.Value = "";
					e.FormattingApplied = true;
				}

			}
			else if (e.ColumnIndex == ShipView_Locked.Index)
			{
				e.Value = (int)e.Value == 1 ? "❤" : (int)e.Value == 2 ? "■" : "";
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_SallyArea.Index && (int)e.Value == -1)
			{
				e.Value = "";
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_Range.Index)
			{
				e.Value = Constants.GetRange((int)e.Value);
				e.FormattingApplied = true;

			}
			else if (e.ColumnIndex == ShipView_Speed.Index)
			{
				e.Value = Constants.GetSpeed((int)e.Value);
				e.FormattingApplied = true;

			}

		}


        private void ShipView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {

            if (e.Column.Index == ShipView_Name.Index)
            {

                ShipDataMaster ship1 = KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex1].Cells[e.Column.Index].Tag],
                                ship2 = KCDatabase.Instance.MasterShips[(int)ShipView.Rows[e.RowIndex2].Cells[e.Column.Index].Tag];

                switch (_shipNameSortMethod)
                {
                    case 0:     // 図鑑番号順
                    default:
                        e.SortResult = ship1.AlbumNo - ship2.AlbumNo;
                        break;

                    case 1:     // あいうえお順
                        e.SortResult = ship1.NameReadingEN.CompareTo(ship2.NameReadingEN);

                        if (e.SortResult == 0)
                            e.SortResult = ship1.NameEN.CompareTo(ship2.NameEN);
                        break;

					case 2:     // ソートキー順
						e.SortResult = ship1.SortID - ship2.SortID;
						break;
                }

            }
            else if (e.Column.Index == ShipView_Exp.Index)
            {
                e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
                if (e.SortResult == 0)  //for Lv.99-100
                    e.SortResult = (int)ShipView[ShipView_Level.Index, e.RowIndex1].Value - (int)ShipView[ShipView_Level.Index, e.RowIndex2].Value;

            }
            else if (
              e.Column.Index == ShipView_HP.Index ||
              e.Column.Index == ShipView_Fuel.Index ||
              e.Column.Index == ShipView_Ammo.Index ||
              e.Column.Index == ShipView_Aircraft1.Index ||
              e.Column.Index == ShipView_Aircraft2.Index ||
              e.Column.Index == ShipView_Aircraft3.Index ||
              e.Column.Index == ShipView_Aircraft4.Index ||
              e.Column.Index == ShipView_Aircraft5.Index ||
              e.Column.Index == ShipView_AircraftTotal.Index
              )
            {
                Fraction frac1 = (Fraction)e.CellValue1, frac2 = (Fraction)e.CellValue2;

                double rate = frac1.Rate - frac2.Rate;

                if (rate > 0.0)
                    e.SortResult = 1;
                else if (rate < 0.0)
                    e.SortResult = -1;
                else
                    e.SortResult = frac1.Current - frac2.Current;

            }
            else if (e.Column.Index == ShipView_Fleet.Index)
            {
                if ((string)e.CellValue1 == "")
                {
                    if ((string)e.CellValue2 == "")
                        e.SortResult = 0;
                    else
                        e.SortResult = 1;
                }
                else
                {
                    if ((string)e.CellValue2 == "")
                        e.SortResult = -1;
                    else
                        e.SortResult = ((string)e.CellValue1).CompareTo(e.CellValue2);
                }

            }
            else
            {
                e.SortResult = (int)e.CellValue1 - (int)e.CellValue2;
            }



            if (e.SortResult == 0)
            {
                e.SortResult = (int)ShipView.Rows[e.RowIndex1].Tag - (int)ShipView.Rows[e.RowIndex2].Tag;

                if (ShipView.SortOrder == SortOrder.Descending)
                    e.SortResult = -e.SortResult;
            }

            e.Handled = true;
        }



        private void ShipView_Sorted(object sender, EventArgs e)
        {

            int count = ShipView.Rows.Count;
            var direction = ShipView.SortOrder;

            for (int i = 0; i < count; i++)
                ShipView.Rows[i].Tag = i;

        }





        // 列のサイズ変更関連
        private void ShipView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {

            if (IsRowsUpdating)
                return;

            var group = CurrentGroup;
            if (group != null)
            {

                if (!group.ViewColumns[e.Column.Name].AutoSize)
                {
                    group.ViewColumns[e.Column.Name].Width = e.Column.Width;
                }
            }

        }

        private void ShipView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {

            if (IsRowsUpdating)
                return;

            var group = CurrentGroup;
            if (group != null)
            {

                foreach (DataGridViewColumn column in ShipView.Columns)
                {
                    group.ViewColumns[column.Name].DisplayIndex = column.DisplayIndex;
                }
            }

        }




        #region メニュー:グループ操作

        private void MenuGroup_Add_Click(object sender, EventArgs e)
        {

			using (var dialog = new DialogTextInput(Translation.DialogGroupAddTitle, Translation.DialogGroupAddDescription))
			{

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    var group = KCDatabase.Instance.ShipGroup.Add();


                    group.Name = dialog.InputtedText.Trim();

                    for (int i = 0; i < ShipView.Columns.Count; i++)
                    {
                        var newdata = new ShipGroupData.ViewColumnData(ShipView.Columns[i]);
                        if (SelectedTab == null)
                            newdata.Visible = true;     //初期状態では全行が非表示のため
                        group.ViewColumns.Add(ShipView.Columns[i].Name, newdata);
                    }

                    TabPanel.Controls.Add(CreateTabLabel(group.GroupID));

                }

            }

        }

        private void MenuGroup_Copy_Click(object sender, EventArgs e)
        {
			//想定外
			if (MenuGroup.SourceControl is not ImageLabel senderLabel) return;

			using (var dialog = new DialogTextInput(Translation.DialogGroupCopyTitle, Translation.DialogGroupCopyDescription))
			{

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    var group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag].Clone();

                    group.GroupID = KCDatabase.Instance.ShipGroup.GetUniqueID();
                    group.Name = dialog.InputtedText.Trim();

                    KCDatabase.Instance.ShipGroup.ShipGroups.Add(group);

                    TabPanel.Controls.Add(CreateTabLabel(group.GroupID));

                }
            }

        }

        private void MenuGroup_Delete_Click(object sender, EventArgs e)
        {
			//想定外
			if (MenuGroup.SourceControl is not ImageLabel senderLabel) return;

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];

			if (group != null)
			{
				if (MessageBox.Show(string.Format(Translation.DialogGroupDeleteDescription, group.Name), Translation.DialogGroupDeleteTitle,
					MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
					== System.Windows.Forms.DialogResult.Yes)
				{

                    if (SelectedTab == senderLabel)
                    {
                        ShipView.Rows.Clear();
                        SelectedTab = null;
                    }
                    KCDatabase.Instance.ShipGroup.ShipGroups.Remove(group);
                    TabPanel.Controls.Remove(senderLabel);
                    senderLabel.Dispose();
                }

			}
			else
			{
				MessageBox.Show(Translation.DialogCouldNotDeleteGroupDescription, Translation.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

        private void MenuGroup_Rename_Click(object sender, EventArgs e)
        {
			if (MenuGroup.SourceControl is not ImageLabel senderLabel) return;

			ShipGroupData group = KCDatabase.Instance.ShipGroup[(int)senderLabel.Tag];

            if (group != null)
            {

				using (var dialog = new DialogTextInput(Translation.DialogGroupRenameTitle, Translation.DialogGroupRenameDescription))
				{

                    dialog.InputtedText = group.Name;

                    if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {

                        group.Name = senderLabel.Text = dialog.InputtedText.Trim();

                    }
                }

			}
			else
			{
				MessageBox.Show(Translation.DialogCouldNotRenameGroupDescription, Translation.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

        }


        private void TabPanel_DoubleClick(object sender, EventArgs e)
        {

            MenuGroup_Add.PerformClick();

        }



        #endregion


        #region メニューON/OFF操作
        private void MenuGroup_Opening(object sender, CancelEventArgs e)
        {

            if (MenuGroup.SourceControl == TabPanel || SelectedTab == null)
            {
                MenuGroup_Add.Enabled = true;
                MenuGroup_Copy.Enabled = false;
                MenuGroup_Rename.Enabled = false;
                MenuGroup_Delete.Enabled = false;
            }
            else
            {
                MenuGroup_Add.Enabled = true;
                MenuGroup_Copy.Enabled = true;
                MenuGroup_Rename.Enabled = true;
                MenuGroup_Delete.Enabled = true;
            }

        }

        private void MenuMember_Opening(object sender, CancelEventArgs e)
        {

            if (SelectedTab == null)
            {

                e.Cancel = true;
                return;
            }

            if (KCDatabase.Instance.Ships.Count == 0)
            {
                MenuMember_Filter.Enabled = false;
                MenuMember_CSVOutput.Enabled = false;

            }
            else
            {
                MenuMember_Filter.Enabled = true;
                MenuMember_CSVOutput.Enabled = true;
            }

            if (ShipView.Rows.GetRowCount(DataGridViewElementStates.Selected) == 0)
            {
                MenuMember_AddToGroup.Enabled = false;
                MenuMember_CreateGroup.Enabled = false;
                MenuMember_Exclude.Enabled = false;

            }
            else
            {
                MenuMember_AddToGroup.Enabled = true;
                MenuMember_CreateGroup.Enabled = true;
                MenuMember_Exclude.Enabled = true;

            }

        }
        #endregion


        #region メニュー:メンバー操作

        private void MenuMember_ColumnFilter_Click(object sender, EventArgs e)
        {

            ShipGroupData group = CurrentGroup;

			if (group == null)
			{
				MessageBox.Show(Translation.DialogGroupCanNotBeModifiedDescription, Translation.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}


            try
            {
                using (var dialog = new DialogShipGroupColumnFilter(ShipView, group))
                {

                    if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {

                        group.ViewColumns = dialog.Result.ToDictionary(r => r.Name);
                        group.ScrollLockColumnCount = dialog.ScrollLockColumnCount;

                        ApplyViewData(group);
                    }



                }
            }
            catch (Exception ex)
            {

				Utility.ErrorReporter.SendErrorReport(ex, Translation.ColumnSettingError);
			}
		}





        private void MenuMember_Filter_Click(object sender, EventArgs e)
        {

            var group = CurrentGroup;
            if (group != null)
            {

                try
                {
                    if (group.Expressions == null)
                        group.Expressions = new ExpressionManager();

                    using (var dialog = new DialogShipGroupFilter(group))
                    {

                        if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {

                            // replace
                            int id = group.GroupID;
                            group = dialog.ExportGroupData();
                            group.GroupID = id;
                            group.Expressions.Compile();

                            KCDatabase.Instance.ShipGroup.ShipGroups.Remove(id);
                            KCDatabase.Instance.ShipGroup.ShipGroups.Add(group);

                            ChangeShipView(SelectedTab);
                        }
                    }
                }
                catch (Exception ex)
                {

					Utility.ErrorReporter.SendErrorReport(ex, Translation.FilterDialogError);
				}

            }
        }



        /// <summary>
        /// 表示設定を反映します。
        /// </summary>
        private void ApplyViewData(ShipGroupData group)
        {

            IsRowsUpdating = true;

            // いったん解除しないと列入れ替え時にエラーが起きる
            foreach (DataGridViewColumn column in ShipView.Columns)
            {
                column.Frozen = false;
            }

            foreach (var data in group.ViewColumns.Values.OrderBy(g => g.DisplayIndex))
            {
                data.ToColumn(ShipView.Columns[data.Name]);
            }

            int count = 0;
            foreach (var column in ShipView.Columns.Cast<DataGridViewColumn>().OrderBy(c => c.DisplayIndex))
            {
                column.Frozen = count < group.ScrollLockColumnCount;
                count++;
            }

            IsRowsUpdating = false;
        }


        private void MenuMember_SortOrder_Click(object sender, EventArgs e)
        {

            var group = CurrentGroup;

            if (group != null)
            {

                try
                {
                    using (var dialog = new DialogShipGroupSortOrder(ShipView, group))
                    {

                        if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {

                            group.AutoSortEnabled = dialog.AutoSortEnabled;
                            group.SortOrder = dialog.Result;

                            ApplyAutoSort(group);
                        }

                    }
                }
                catch (Exception ex)
                {

					Utility.ErrorReporter.SendErrorReport(ex, Translation.AutoSortDialogError);
				}
			}

        }


        private void ApplyAutoSort(ShipGroupData group)
        {

            if (!group.AutoSortEnabled || group.SortOrder == null)
                return;

            // 一番上/最後に実行したほうが優先度が高くなるので逆順で
            for (int i = group.SortOrder.Count - 1; i >= 0; i--)
            {

                var order = group.SortOrder[i];
                ListSortDirection dir = order.Value;

                if (ShipView.Columns[order.Key].SortMode != DataGridViewColumnSortMode.NotSortable)
                    ShipView.Sort(ShipView.Columns[order.Key], dir);
            }


        }





        private void MenuMember_AddToGroup_Click(object sender, EventArgs e)
        {

			using (var dialog = new DialogTextSelect(Translation.DialogGroupAddToGroupTitle, Translation.DialogGroupAddToGroupDescription,
				KCDatabase.Instance.ShipGroup.ShipGroups.Values.ToArray()))
			{

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    var group = (ShipGroupData)dialog.SelectedItem;
                    if (group != null)
                    {
                        group.AddInclusionFilter(GetSelectedShipID());

                        if (group.ID == CurrentGroup.ID)
                            ChangeShipView(SelectedTab);
                    }

                }
            }

        }

        private void MenuMember_CreateGroup_Click(object sender, EventArgs e)
        {

            var ships = GetSelectedShipID();
            if (ships.Count() == 0)
                return;

			using (var dialog = new DialogTextInput(Translation.DialogGroupAddTitle, Translation.DialogGroupAddDescription))
			{

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    var group = KCDatabase.Instance.ShipGroup.Add();

                    group.Name = dialog.InputtedText.Trim();

                    for (int i = 0; i < ShipView.Columns.Count; i++)
                    {
                        var newdata = new ShipGroupData.ViewColumnData(ShipView.Columns[i]);
                        if (SelectedTab == null)
                            newdata.Visible = true;     //初期状態では全行が非表示のため
                        group.ViewColumns.Add(ShipView.Columns[i].Name, newdata);
                    }

                    // 艦船ID == 0 を作成(空リストを作る)
                    group.Expressions.Expressions.Add(new ExpressionList(false, true, false));
                    group.Expressions.Expressions[0].Expressions.Add(new ExpressionData(".MasterID", ExpressionData.ExpressionOperator.Equal, 0));
                    group.Expressions.Compile();

                    group.AddInclusionFilter(ships);

                    TabPanel.Controls.Add(CreateTabLabel(group.GroupID));

                }

            }
        }

        private void MenuMember_Exclude_Click(object sender, EventArgs e)
        {

            var group = CurrentGroup;
            if (group != null)
            {

                group.AddExclusionFilter(GetSelectedShipID());

                ChangeShipView(SelectedTab);
            }

        }





		// todo: does this need translating?
        private static readonly string ShipCSVHeaderUser = "固有ID,艦種,艦名,Lv,Exp,next,改装まで,耐久現在,耐久最大,Cond,燃料,弾薬,装備1,装備2,装備3,装備4,装備5,補強装備,入渠,火力,火力改修,火力合計,雷装,雷装改修,雷装合計,対空,対空改修,対空合計,装甲,装甲改修,装甲合計,対潜,対潜合計,回避,回避合計,索敵,索敵合計,運,運改修,運合計,射程,速力,ロック,出撃先,母港ソートID,航空威力,砲撃威力,空撃威力,対潜威力,雷撃威力,夜戦威力";

        private static readonly string ShipCSVHeaderData = "固有ID,艦種,艦名,艦船ID,Lv,Exp,next,改装まで,耐久現在,耐久最大,Cond,燃料,弾薬,装備1,装備2,装備3,装備4,装備5,補強装備,装備ID1,装備ID2,装備ID3,装備ID4,装備ID5,補強装備ID,艦載機1,艦載機2,艦載機3,艦載機4,艦載機5,入渠,入渠燃料,入渠鋼材,火力,火力改修,火力合計,雷装,雷装改修,雷装合計,対空,対空改修,対空合計,装甲,装甲改修,装甲合計,対潜,対潜合計,回避,回避合計,索敵,索敵合計,運,運改修,運合計,射程,速力,ロック,出撃先,母港ソートID,航空威力,砲撃威力,空撃威力,対潜威力,雷撃威力,夜戦威力";


        private void MenuMember_CSVOutput_Click(object sender, EventArgs e)
        {

            IEnumerable<ShipData> ships;

            if (SelectedTab == null)
            {
                ships = KCDatabase.Instance.Ships.Values;
            }
            else
            {
                //*/
                ships = ShipView.Rows.Cast<DataGridViewRow>().Select(r => KCDatabase.Instance.Ships[(int)r.Cells[ShipView_ID.Index].Value]);
                /*/
				var group = KCDatabase.Instance.ShipGroup[(int)SelectedTab.Tag];
				if ( group == null )
					ships = KCDatabase.Instance.Ships.Values;
				else
					ships = group.MembersInstance;
				//*/
            }


            using (var dialog = new DialogShipGroupCSVOutput())
            {

                if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {

                    try
                    {

                        using (StreamWriter sw = new StreamWriter(dialog.OutputPath, false, Utility.Configuration.Config.Log.FileEncoding))
                        {

                            string header = dialog.OutputFormat == DialogShipGroupCSVOutput.OutputFormatConstants.User ? ShipCSVHeaderUser : ShipCSVHeaderData;
                            sw.WriteLine(header);


                            foreach (ShipData ship in ships.Where(s => s != null))
                            {

                                if (dialog.OutputFormat == DialogShipGroupCSVOutput.OutputFormatConstants.User)
                                {

                                    sw.WriteLine(string.Join(",",
                                        ship.MasterID,
                                        ship.MasterShip.ShipTypeName,
                                        ship.MasterShip.NameWithClass,
                                        ship.Level,
                                        ship.ExpTotal,
                                        ship.ExpNext,
                                        ship.ExpNextRemodel,
                                        ship.HPCurrent,
                                        ship.HPMax,
                                        ship.Condition,
                                        ship.Fuel,
                                        ship.Ammo,
                                        GetEquipmentString(ship, 0),
                                        GetEquipmentString(ship, 1),
                                        GetEquipmentString(ship, 2),
                                        GetEquipmentString(ship, 3),
                                        GetEquipmentString(ship, 4),
                                        GetEquipmentString(ship, 5),
                                        DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan(ship.RepairTime)),
                                        ship.FirepowerBase,
                                        ship.FirepowerRemain,
                                        ship.FirepowerTotal,
                                        ship.TorpedoBase,
                                        ship.TorpedoRemain,
                                        ship.TorpedoTotal,
                                        ship.AABase,
                                        ship.AARemain,
                                        ship.AATotal,
                                        ship.ArmorBase,
                                        ship.ArmorRemain,
                                        ship.ArmorTotal,
                                        ship.ASWBase,
                                        ship.ASWTotal,
                                        ship.EvasionBase,
                                        ship.EvasionTotal,
                                        ship.LOSBase,
                                        ship.LOSTotal,
                                        ship.LuckBase,
                                        ship.LuckRemain,
                                        ship.LuckTotal,
                                        Constants.GetRange(ship.Range),
                                        Constants.GetSpeed(ship.Speed),
                                        ship.IsLocked ? "●" : ship.IsLockedByEquipment ? "■" : "-",
                                        ship.SallyArea,
										ship.MasterShip.SortID,
                                        ship.AirBattlePower,
                                        ship.ShellingPower,
                                        ship.AircraftPower,
                                        ship.AntiSubmarinePower,
                                        ship.TorpedoPower,
                                        ship.NightBattlePower
                                        ));

                                }
                                else
                                {       //data

                                    sw.WriteLine(string.Join(",",
                                        ship.MasterID,
                                        (int)ship.MasterShip.ShipType,
                                        ship.MasterShip.NameWithClass,
                                        ship.ShipID,
                                        ship.Level,
                                        ship.ExpTotal,
                                        ship.ExpNext,
                                        ship.ExpNextRemodel,
                                        ship.HPCurrent,
                                        ship.HPMax,
                                        ship.Condition,
                                        ship.Fuel,
                                        ship.Ammo,
                                        GetEquipmentString(ship, 0),
                                        GetEquipmentString(ship, 1),
                                        GetEquipmentString(ship, 2),
                                        GetEquipmentString(ship, 3),
                                        GetEquipmentString(ship, 4),
                                        GetEquipmentString(ship, 5),
                                        ship.Slot[0],
                                        ship.Slot[1],
                                        ship.Slot[2],
                                        ship.Slot[3],
                                        ship.Slot[4],
                                        ship.ExpansionSlot,
                                        ship.Aircraft[0],
                                        ship.Aircraft[1],
                                        ship.Aircraft[2],
                                        ship.Aircraft[3],
                                        ship.Aircraft[4],
                                        ship.RepairTime,
                                        ship.RepairFuel,
                                        ship.RepairSteel,
                                        ship.FirepowerBase,
                                        ship.FirepowerRemain,
                                        ship.FirepowerTotal,
                                        ship.TorpedoBase,
                                        ship.TorpedoRemain,
                                        ship.TorpedoTotal,
                                        ship.AABase,
                                        ship.AARemain,
                                        ship.AATotal,
                                        ship.ArmorBase,
                                        ship.ArmorRemain,
                                        ship.ArmorTotal,
                                        ship.ASWBase,
                                        ship.ASWTotal,
                                        ship.EvasionBase,
                                        ship.EvasionTotal,
                                        ship.LOSBase,
                                        ship.LOSTotal,
                                        ship.LuckBase,
                                        ship.LuckRemain,
                                        ship.LuckTotal,
                                        ship.Range,
                                        ship.Speed,
                                        ship.IsLocked ? 1 : ship.IsLockedByEquipment ? 2 : 0,
                                        ship.SallyArea,
										ship.MasterShip.SortID,
										ship.AirBattlePower,
                                        ship.ShellingPower,
                                        ship.AircraftPower,
                                        ship.AntiSubmarinePower,
                                        ship.TorpedoPower,
                                        ship.NightBattlePower
                                        ));

                                }

                            }

                        }

                        Utility.Logger.Add(2, string.Format(Translation.ExportToCsvSuccess, dialog.OutputPath));

					}
					catch (Exception ex)
					{
						Utility.ErrorReporter.SendErrorReport(ex, Translation.ExportToCsvFail);
						MessageBox.Show(Translation.ExportToCsvFail + "\r\n" + ex.Message, Translation.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

                }
            }

        }




        #endregion


        #region タブ操作系

        private Point? _tempMouse = null;
        void TabLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _tempMouse = TabPanel.PointToClient(e.Location);
            }
            else
            {
                _tempMouse = null;
            }
        }

        void TabLabel_MouseMove(object sender, MouseEventArgs e)
        {

            if (_tempMouse != null)
            {

                Rectangle move = new Rectangle(
                    _tempMouse.Value.X - SystemInformation.DragSize.Width / 2,
                    _tempMouse.Value.Y - SystemInformation.DragSize.Height / 2,
                    SystemInformation.DragSize.Width,
                    SystemInformation.DragSize.Height
                    );

                if (!move.Contains(TabPanel.PointToClient(e.Location)))
                {
                    TabPanel.DoDragDrop(sender, DragDropEffects.All);
                    _tempMouse = null;
                }
            }

        }

        void TabLabel_MouseUp(object sender, MouseEventArgs e)
        {
            _tempMouse = null;
        }


        private void TabPanel_DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(typeof(ImageLabel)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void TabPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

            //右クリックでキャンセル
            if ((e.KeyState & 2) != 0)
            {
                e.Action = DragAction.Cancel;
            }

        }


        private void TabPanel_DragDrop(object sender, DragEventArgs e)
        {

            //fixme:カッコカリ　範囲外にドロップすると端に行く

            Point mp = TabPanel.PointToClient(new Point(e.X, e.Y));

            var item = TabPanel.GetChildAtPoint(mp);

            int index = TabPanel.Controls.GetChildIndex(item, false);

            TabPanel.Controls.SetChildIndex((System.Windows.Forms.Control)e.Data.GetData(typeof(ImageLabel)), index);

            TabPanel.Invalidate();

        }

        #endregion


        private void MenuGroup_ShowStatusBar_CheckedChanged(object sender, EventArgs e)
        {

            StatusBar.Visible = MenuGroup_ShowStatusBar.Checked;

        }



        void SystemShuttingDown()
        {

            Utility.Configuration.Config.FormShipGroup.AutoUpdate = MenuGroup_AutoUpdate.Checked;
            Utility.Configuration.Config.FormShipGroup.ShowStatusBar = MenuGroup_ShowStatusBar.Checked;



            ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


            List<ImageLabel> list = TabPanel.Controls.OfType<ImageLabel>().OrderBy(c => TabPanel.Controls.GetChildIndex(c)).ToList();

            for (int i = 0; i < list.Count; i++)
            {

                ShipGroupData group = groups[(int)list[i].Tag];
                if (group != null)
                    group.GroupID = i + 1;
            }

        }


        private void FormShipGroup_Resize(object sender, EventArgs e)
        {
            if (_splitterDistance != -1 && splitContainer1.Height > 0)
            {
                try
                {
                    splitContainer1.SplitterDistance = _splitterDistance;
                    _splitterDistance = -1;

                }
                catch (Exception)
                {
                    // *ぷちっ*
                }
            }
        }


    }
}
