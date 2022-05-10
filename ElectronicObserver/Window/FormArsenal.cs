using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Support;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window;

public partial class FormArsenal : DockContent
{

	private class TableArsenalControl : IDisposable
	{

		public Label ShipName;
		public Label CompletionTime;
		private ToolTip tooltip;

		public TableArsenalControl(FormArsenal parent)
		{

			#region Initialize

			ShipName = new ImageLabel
			{
				Text = "???",
				Anchor = AnchorStyles.Left,
				ForeColor = parent.ForeColor,
				TextAlign = ContentAlignment.MiddleLeft,
				Padding = new Padding(0, 1, 0, 1),
				Margin = new Padding(2, 1, 2, 1),
				MaximumSize = new Size(60, int.MaxValue),
				//ShipName.AutoEllipsis = true;
				ImageAlign = ContentAlignment.MiddleCenter,
				AutoSize = true,
				Visible = true
			};

			CompletionTime = new Label
			{
				Text = "",
				Anchor = AnchorStyles.Left,
				ForeColor = parent.ForeColor,
				Tag = null,
				TextAlign = ContentAlignment.MiddleLeft,
				Padding = new Padding(0, 1, 0, 1),
				Margin = new Padding(2, 1, 2, 1),
				MinimumSize = new Size(60, 10),
				AutoSize = true,
				Visible = true
			};

			ConfigurationChanged(parent);

			tooltip = parent.ToolTipInfo;
			#endregion

		}


		public TableArsenalControl(FormArsenal parent, TableLayoutPanel table, int row)
			: this(parent)
		{

			AddToTable(table, row);
		}


		public void AddToTable(TableLayoutPanel table, int row)
		{

			table.Controls.Add(ShipName, 0, row);
			table.Controls.Add(CompletionTime, 1, row);

		}


		public void Update(int arsenalID)
		{

			KCDatabase db = KCDatabase.Instance;
			ArsenalData arsenal = db.Arsenals[arsenalID];
			bool showShipName = Utility.Configuration.Config.FormArsenal.ShowShipName;

			CompletionTime.BackColor = Color.Transparent;
			CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			tooltip.SetToolTip(ShipName, null);
			tooltip.SetToolTip(CompletionTime, null);

			if (arsenal == null || arsenal.State == -1)
			{
				//locked
				ShipName.Text = "";
				CompletionTime.Text = "";
				CompletionTime.Tag = null;

			}
			else if (arsenal.State == 0)
			{
				//empty
				ShipName.Text = "----";
				CompletionTime.Text = "";
				CompletionTime.Tag = null;

			}
			else if (arsenal.State == 2)
			{
				//building
				string name = showShipName ? db.MasterShips[arsenal.ShipID].NameEN : "???";
				ShipName.Text = name;
				tooltip.SetToolTip(ShipName, name);
				CompletionTime.Text = DateTimeHelper.ToTimeRemainString(arsenal.CompletionTime);
				CompletionTime.Tag = arsenal.CompletionTime;
				tooltip.SetToolTip(CompletionTime, GeneralRes.TimeToCompletion + ":" + DateTimeHelper.TimeToCSVString(arsenal.CompletionTime));

			}
			else if (arsenal.State == 3)
			{
				//complete!
				string name = showShipName ? db.MasterShips[arsenal.ShipID].NameEN : "???";
				ShipName.Text = name;
				tooltip.SetToolTip(ShipName, name);
				CompletionTime.Text = GeneralRes.Complete + "!";
				CompletionTime.Tag = null;

			}

		}


		public void Refresh(int arsenalID)
		{

			if (CompletionTime.Tag != null)
			{

				var time = (DateTime)CompletionTime.Tag;

				CompletionTime.Text = DateTimeHelper.ToTimeRemainString(time);

				if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && (time - DateTime.Now).TotalMilliseconds <= Utility.Configuration.Config.NotifierConstruction.AccelInterval)
				{
					CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteBG : Color.Transparent;
					CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteFG : Utility.Configuration.Config.UI.ForeColor;
				}

			}
			else if (Utility.Configuration.Config.FormArsenal.BlinkAtCompletion && !string.IsNullOrWhiteSpace(CompletionTime.Text))
			{
				//完成しているので
				CompletionTime.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteBG : Color.Transparent;
				CompletionTime.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Arsenal_BuildCompleteFG : Utility.Configuration.Config.UI.ForeColor;
			}
		}


		public void ConfigurationChanged(FormArsenal parent)
		{

			var config = Utility.Configuration.Config.FormArsenal;

			ShipName.Font = parent.Font;
			CompletionTime.Font = parent.Font;
			CompletionTime.BackColor = Color.Transparent;
			ShipName.ForeColor = CompletionTime.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			ShipName.MaximumSize = new Size(config.MaxShipNameWidth, ShipName.MaximumSize.Height);
		}

		public void Dispose()
		{
			ShipName.Dispose();
			CompletionTime.Dispose();
		}
	}


	private TableArsenalControl[] ControlArsenal;
	private int _buildingID;

	public FormArsenal(FormMain parent)
	{
		InitializeComponent();

		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

		ControlHelper.SetDoubleBuffered(TableArsenal);

		TableArsenal.SuspendLayout();
		ControlArsenal = new TableArsenalControl[4];
		for (int i = 0; i < ControlArsenal.Length; i++)
		{
			ControlArsenal[i] = new TableArsenalControl(this, TableArsenal, i);
		}
		TableArsenal.ResumeLayout();

		_buildingID = -1;

		ConfigurationChanged();

		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormArsenal]);

		Translate();
	}

	public void Translate()
	{
		MenuMain_ShowShipName.Text = Menus.ShowShipName;

		Text = GeneralRes.Arsenal;
	}

	private void FormArsenal_Load(object sender, EventArgs e)
	{

		APIObserver o = APIObserver.Instance;

		o.ApiReqKousyou_CreateShip.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShipSpeedChange.RequestReceived += Updated;

		o.ApiGetMember_KDock.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiGetMember_RequireInfo.ResponseReceived += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

	}


	void Updated(string apiname, dynamic data)
	{

		if (_buildingID != -1 && apiname == "api_get_member/kdock")
		{

			ArsenalData arsenal = KCDatabase.Instance.Arsenals[_buildingID];
			ShipDataMaster ship = KCDatabase.Instance.MasterShips[arsenal.ShipID];
			string name;

			if (Utility.Configuration.Config.Log.ShowSpoiler && Utility.Configuration.Config.FormArsenal.ShowShipName)
			{

				name = string.Format("{0} {1}", ship.ShipTypeName, ship.NameWithClass);

			}
			else
			{

				name = GeneralRes.ShipGirl;
			}

			Utility.Logger.Add(2, string.Format(GeneralRes.ArsenalLog,
				_buildingID,
				name,
				arsenal.Fuel,
				arsenal.Ammo,
				arsenal.Steel,
				arsenal.Bauxite,
				arsenal.DevelopmentMaterial,
				KCDatabase.Instance.Fleet[1].MembersInstance[0].NameWithLevel
			));

			_buildingID = -1;
		}

		if (apiname == "api_req_kousyou/createship")
		{
			_buildingID = int.Parse(data["api_kdock_id"]);
		}

		UpdateUI();
	}

	void UpdateUI()
	{

		if (ControlArsenal == null) return;

		TableArsenal.SuspendLayout();
		TableArsenal.RowCount = KCDatabase.Instance.Arsenals.Values.Count(a => a.State != -1);
		for (int i = 0; i < ControlArsenal.Length; i++)
			ControlArsenal[i].Update(i + 1);
		TableArsenal.ResumeLayout();

	}

	void UpdateTimerTick()
	{

		TableArsenal.SuspendLayout();
		for (int i = 0; i < ControlArsenal.Length; i++)
			ControlArsenal[i].Refresh(i + 1);
		TableArsenal.ResumeLayout();

	}


	void ConfigurationChanged()
	{

		Font = Utility.Configuration.Config.UI.MainFont;
		MenuMain_ShowShipName.Checked = Utility.Configuration.Config.FormArsenal.ShowShipName;

		if (ControlArsenal != null)
		{
			TableArsenal.SuspendLayout();

			foreach (var c in ControlArsenal)
				c.ConfigurationChanged(this);

			ControlHelper.SetTableRowStyles(TableArsenal, ControlHelper.GetDefaultRowStyle());

			TableArsenal.ResumeLayout();
		}
	}


	private void MenuMain_ShowShipName_CheckedChanged(object sender, EventArgs e)
	{
		Utility.Configuration.Config.FormArsenal.ShowShipName = MenuMain_ShowShipName.Checked;

		UpdateUI();
	}


	private void TableArsenal_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
	{
		e.Graphics.DrawLine(Utility.Configuration.Config.UI.SubBackColorPen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
	}



	protected override string GetPersistString()
	{
		return "Arsenal";
	}



}
