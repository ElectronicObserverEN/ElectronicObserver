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
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Support;
using WeifenLuo.WinFormsUI.Docking;
using Translation = ElectronicObserver.Properties.Window.FormHeadQuarters;

namespace ElectronicObserver.Window;

public partial class FormHeadquarters : DockContent
{

	private Form _parentForm;

	public FormHeadquarters(FormMain parent)
	{
		InitializeComponent();

		_parentForm = parent;


		ImageList icons = ResourceManager.Instance.Icons;

		ShipCount.ImageList = icons;
		ShipCount.ImageIndex = (int)IconContent.HeadQuartersShip;
		EquipmentCount.ImageList = icons;
		EquipmentCount.ImageIndex = (int)IconContent.HeadQuartersEquipment;
		InstantRepair.ImageList = icons;
		InstantRepair.ImageIndex = (int)IconContent.ItemInstantRepair;
		InstantConstruction.ImageList = icons;
		InstantConstruction.ImageIndex = (int)IconContent.ItemInstantConstruction;
		DevelopmentMaterial.ImageList = icons;
		DevelopmentMaterial.ImageIndex = (int)IconContent.ItemDevelopmentMaterial;
		ModdingMaterial.ImageList = icons;
		ModdingMaterial.ImageIndex = (int)IconContent.ItemModdingMaterial;
		FurnitureCoin.ImageList = icons;
		FurnitureCoin.ImageIndex = (int)IconContent.ItemFurnitureCoin;
		Fuel.ImageList = icons;
		Fuel.ImageIndex = (int)IconContent.ResourceFuel;
		Ammo.ImageList = icons;
		Ammo.ImageIndex = (int)IconContent.ResourceAmmo;
		Steel.ImageList = icons;
		Steel.ImageIndex = (int)IconContent.ResourceSteel;
		Bauxite.ImageList = icons;
		Bauxite.ImageIndex = (int)IconContent.ResourceBauxite;
		DisplayUseItem.ImageList = icons;
		DisplayUseItem.ImageIndex = (int)IconContent.ItemPresentBox;


		ControlHelper.SetDoubleBuffered(FlowPanelMaster);
		ControlHelper.SetDoubleBuffered(FlowPanelAdmiral);
		ControlHelper.SetDoubleBuffered(FlowPanelFleet);
		ControlHelper.SetDoubleBuffered(FlowPanelUseItem);
		ControlHelper.SetDoubleBuffered(FlowPanelResource);


		ConfigurationChanged();

		Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormHeadQuarters]);

		Translate();
	}

	public void Translate()
	{
		// these are just design time and don't actually get displayed
		AdmiralName.Text = Translation.AdmiralName;
		AdmiralComment.Text = Translation.AdmiralComment;
		HQLevel.Text = Translation.HQLevel;
		ShipCount.Text = Translation.ShipCount;
		EquipmentCount.Text = Translation.EquipmentCount;
		InstantRepair.Text = Translation.InstantRepair;
		InstantConstruction.Text = Translation.InstantConstruction;
		DevelopmentMaterial.Text = Translation.DevelopmentMaterial;
		ModdingMaterial.Text = Translation.ModdingMaterial;
		FurnitureCoin.Text = Translation.FurnitureCoin;
		DisplayUseItem.Text = Translation.DisplayUseItem;
		Fuel.Text = Translation.Fuel;
		Ammo.Text = Translation.Ammo;
		Steel.Text = Translation.Steel;
		Bauxite.Text = Translation.Bauxite;

		Text = Translation.Title;
	}

	private void FormHeadquarters_Load(object sender, EventArgs e)
	{

		APIObserver o = APIObserver.Instance;

		o.ApiReqNyukyo_Start.RequestReceived += Updated;
		o.ApiReqNyukyo_SpeedChange.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShip.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShipSpeedChange.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyShip.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyItem2.RequestReceived += Updated;
		o.ApiReqMember_UpdateComment.RequestReceived += Updated;

		o.ApiGetMember_Basic.ResponseReceived += Updated;
		o.ApiGetMember_SlotItem.ResponseReceived += Updated;
		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_Ship2.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiReqHokyu_Charge.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyShip.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyItem2.ResponseReceived += Updated;
		o.ApiReqKaisou_PowerUp.ResponseReceived += Updated;
		o.ApiReqKousyou_CreateItem.ResponseReceived += Updated;
		o.ApiReqKousyou_RemodelSlot.ResponseReceived += Updated;
		o.ApiGetMember_Material.ResponseReceived += Updated;
		o.ApiGetMember_ShipDeck.ResponseReceived += Updated;
		o.ApiReqAirCorps_SetPlane.ResponseReceived += Updated;
		o.ApiReqAirCorps_Supply.ResponseReceived += Updated;
		o.ApiGetMember_UseItem.ResponseReceived += Updated;


		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		Utility.SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;

		FlowPanelResource.SetFlowBreak(Ammo, true);

		FlowPanelMaster.Visible = false;

	}



	void ConfigurationChanged()
	{

		Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.MainFont;
		HQLevel.MainFont = Utility.Configuration.Config.UI.MainFont;
		HQLevel.SubFont = Utility.Configuration.Config.UI.SubFont;
		HQLevel.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
		HQLevel.SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

		// 点滅しない設定にしたときに消灯状態で固定されるのを防ぐ
		if (!Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
		{
			if (ShipCount.Tag as bool? ?? false)
			{
				ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}

			if (EquipmentCount.Tag as bool? ?? false)
			{
				EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
		}

		//visibility
		CheckVisibilityConfiguration();
		{
			var visibility = Utility.Configuration.Config.FormHeadquarters.Visibility.List;
			AdmiralName.Visible = visibility[0];
			AdmiralComment.Visible = visibility[1];
			HQLevel.Visible = visibility[2];
			ShipCount.Visible = visibility[3];
			EquipmentCount.Visible = visibility[4];
			InstantRepair.Visible = visibility[5];
			InstantConstruction.Visible = visibility[6];
			DevelopmentMaterial.Visible = visibility[7];
			ModdingMaterial.Visible = visibility[8];
			FurnitureCoin.Visible = visibility[9];
			Fuel.Visible = visibility[10];
			Ammo.Visible = visibility[11];
			Steel.Visible = visibility[12];
			Bauxite.Visible = visibility[13];
			DisplayUseItem.Visible = visibility[14];
		}

		UpdateDisplayUseItem();
	}


	/// <summary>
	/// VisibleFlags 設定をチェックし、不正な値だった場合は初期値に戻します。
	/// </summary>
	public static void CheckVisibilityConfiguration()
	{
		const int count = 15;
		var config = Utility.Configuration.Config.FormHeadquarters;

		if (config.Visibility == null)
			config.Visibility = new Utility.Storage.SerializableList<bool>(Enumerable.Repeat(true, count).ToList());

		for (int i = config.Visibility.List.Count; i < count; i++)
		{
			config.Visibility.List.Add(true);
		}

	}

	/// <summary>
	/// 各表示項目の名称を返します。
	/// </summary>
	public static IEnumerable<string> GetItemNames()
	{
		yield return Translation.ItemNameName;
		yield return Translation.ItemNameComment;
		yield return Translation.ItemNameHQLevel;
		yield return Translation.ItemNameShipSlots;
		yield return Translation.ItemNameEquipmentSlots;
		yield return Translation.ItemNameInstantRepair;
		yield return Translation.ItemNameInstantConstruction;
		yield return Translation.ItemNameDevelopmentMaterial;
		yield return Translation.ItemNameImproveMaterial;
		yield return Translation.ItemNameFurnitureCoin;
		yield return Translation.ItemNameFuel;
		yield return Translation.ItemNameAmmo;
		yield return Translation.ItemNameSteel;
		yield return Translation.ItemNameBauxite;
		yield return Translation.ItemNameOtherItem;
	}


	void Updated(string apiname, dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		var configUI = Utility.Configuration.Config.UI;

		if (!db.Admiral.IsAvailable)
			return;

		FlowPanelMaster.SuspendLayout();

		//Admiral
		FlowPanelAdmiral.SuspendLayout();
		AdmiralName.Text = string.Format("{0} {1}", db.Admiral.AdmiralName, Constants.GetAdmiralRank(db.Admiral.Rank));
		{
			StringBuilder tooltip = new StringBuilder();

			var sortieCount = db.Admiral.SortieWin + db.Admiral.SortieLose;
			tooltip.AppendFormat(Translation.AdmiralNameToolTipSortie + "\r\n",
				sortieCount, db.Admiral.SortieWin, db.Admiral.SortieWin / Math.Max(sortieCount, 1.0), db.Admiral.SortieLose);

			tooltip.AppendFormat(Translation.AdmiralNameToolTipSortieExp + "\r\n",
				db.Admiral.Exp / Math.Max(sortieCount, 1.0),
				db.Admiral.Exp / Math.Max(db.Admiral.SortieWin, 1.0));

			tooltip.AppendFormat(Translation.AdmiralNameToolTipExpedition + "\r\n",
				db.Admiral.MissionCount, db.Admiral.MissionSuccess, db.Admiral.MissionSuccess / Math.Max(db.Admiral.MissionCount, 1.0), db.Admiral.MissionCount - db.Admiral.MissionSuccess);

			var practiceCount = db.Admiral.PracticeWin + db.Admiral.PracticeLose;
			tooltip.AppendFormat(Translation.AdmiralNameToolTipPractice + "\r\n",
				practiceCount, db.Admiral.PracticeWin, db.Admiral.PracticeWin / Math.Max(practiceCount, 1.0), db.Admiral.PracticeLose);

			tooltip.AppendFormat(Translation.AdmiralNameToolTipFirstClassMedals + "\r\n", db.Admiral.Medals);

			ToolTipInfo.SetToolTip(AdmiralName, tooltip.ToString());
		}
		AdmiralComment.Text = db.Admiral.Comment;
		FlowPanelAdmiral.ResumeLayout();

		//HQ Level
		HQLevel.Value = db.Admiral.Level;
		{
			StringBuilder tooltip = new StringBuilder();
			if (db.Admiral.Level < ExpTable.AdmiralExp.Max(e => e.Key))
			{
				HQLevel.TextNext = "next:";
				HQLevel.ValueNext = ExpTable.GetNextExpAdmiral(db.Admiral.Exp);
				tooltip.AppendFormat("{0} / {1}\r\n", db.Admiral.Exp, ExpTable.AdmiralExp[db.Admiral.Level + 1].Total);
			}
			else
			{
				HQLevel.TextNext = "exp:";
				HQLevel.ValueNext = db.Admiral.Exp;
			}

			//戦果ツールチップ
			//fixme: もっとましな書き方はなかっただろうか
			{
				var res = RecordManager.Instance.Resource.GetRecordPrevious();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(Translation.HQLevelToolTipSenkaSession + "\r\n", diff, diff * 7 / 10000.0);
				}
			}
			{
				var res = RecordManager.Instance.Resource.GetRecordDay();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(Translation.HQLevelToolTipSenkaDay + "\r\n", diff, diff * 7 / 10000.0);
				}
			}
			{
				var res = RecordManager.Instance.Resource.GetRecordMonth();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(Translation.HQLevelToolTipSenkaMonth + "\r\n", diff, diff * 7 / 10000.0);
				}
			}

			ToolTipInfo.SetToolTip(HQLevel, tooltip.ToString());
		}

		//Fleet
		FlowPanelFleet.SuspendLayout();
		{

			ShipCount.Text = string.Format("{0}/{1}", RealShipCount, db.Admiral.MaxShipCount);
			if (RealShipCount > db.Admiral.MaxShipCount - 5)
			{
				ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
			else
			{
				ShipCount.BackColor = Color.Transparent;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			}
			ShipCount.Tag = RealShipCount >= db.Admiral.MaxShipCount;

			EquipmentCount.Text = string.Format("{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount);
			if (RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20)
			{
				EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
			else
			{
				EquipmentCount.BackColor = Color.Transparent;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			}
			EquipmentCount.Tag = RealEquipmentCount >= db.Admiral.MaxEquipmentCount;

		}
		FlowPanelFleet.ResumeLayout();



		var resday = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddHours(5));
		var resweek = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddDays(-(((int)DateTime.Now.AddHours(-5).DayOfWeek + 6) % 7)).AddHours(5)); //月曜日起点
		var resmonth = RecordManager.Instance.Resource.GetRecord(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddHours(5));


		//UseItems
		FlowPanelUseItem.SuspendLayout();

		InstantRepair.Text = db.Material.InstantRepair.ToString();
		if (db.Material.InstantRepair >= 3000)
		{
			InstantRepair.ForeColor = configUI.Headquarters_MaterialMaxFG;
			InstantRepair.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else if (db.Material.InstantRepair < (configUI.HqResLowAlertBucket == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBucket))
		{
			InstantRepair.ForeColor = configUI.Headquarters_ResourceLowFG;
			InstantRepair.BackColor = configUI.Headquarters_ResourceLowBG;
		}
		else
		{
			InstantRepair.ForeColor = configUI.ForeColor;
			InstantRepair.BackColor = Color.Transparent;
		}
		ToolTipInfo.SetToolTip(InstantRepair, string.Format(Translation.ResourceToolTip,
			resday == null ? 0 : (db.Material.InstantRepair - resday.InstantRepair),
			resweek == null ? 0 : (db.Material.InstantRepair - resweek.InstantRepair),
			resmonth == null ? 0 : (db.Material.InstantRepair - resmonth.InstantRepair)));

		InstantConstruction.Text = db.Material.InstantConstruction.ToString();
		if (db.Material.InstantConstruction >= 3000)
		{
			InstantConstruction.ForeColor = configUI.Headquarters_MaterialMaxFG;
			InstantConstruction.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			InstantConstruction.ForeColor = configUI.ForeColor;
			InstantConstruction.BackColor = Color.Transparent;
		}
		ToolTipInfo.SetToolTip(InstantConstruction, string.Format(Translation.ResourceToolTip,
			resday == null ? 0 : (db.Material.InstantConstruction - resday.InstantConstruction),
			resweek == null ? 0 : (db.Material.InstantConstruction - resweek.InstantConstruction),
			resmonth == null ? 0 : (db.Material.InstantConstruction - resmonth.InstantConstruction)));

		DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
		if (db.Material.DevelopmentMaterial >= 3000)
		{
			DevelopmentMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
			DevelopmentMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			DevelopmentMaterial.ForeColor = configUI.ForeColor;
			DevelopmentMaterial.BackColor = Color.Transparent;
		}
		ToolTipInfo.SetToolTip(DevelopmentMaterial, string.Format(Translation.ResourceToolTip,
			resday == null ? 0 : (db.Material.DevelopmentMaterial - resday.DevelopmentMaterial),
			resweek == null ? 0 : (db.Material.DevelopmentMaterial - resweek.DevelopmentMaterial),
			resmonth == null ? 0 : (db.Material.DevelopmentMaterial - resmonth.DevelopmentMaterial)));

		ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
		if (db.Material.ModdingMaterial >= 3000)
		{
			ModdingMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
			ModdingMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			ModdingMaterial.ForeColor = configUI.ForeColor;
			ModdingMaterial.BackColor = Color.Transparent;
		}
		ToolTipInfo.SetToolTip(ModdingMaterial, string.Format(Translation.ResourceToolTip,
			resday == null ? 0 : (db.Material.ModdingMaterial - resday.ModdingMaterial),
			resweek == null ? 0 : (db.Material.ModdingMaterial - resweek.ModdingMaterial),
			resmonth == null ? 0 : (db.Material.ModdingMaterial - resmonth.ModdingMaterial)));

		const int furnitureCoinCap = 350000;

		FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
		if (db.Admiral.FurnitureCoin >= furnitureCoinCap)
		{
			FurnitureCoin.ForeColor = configUI.Headquarters_CoinMaxFG;
			FurnitureCoin.BackColor = configUI.Headquarters_CoinMaxBG;
		}
		else
		{
			FurnitureCoin.ForeColor = configUI.ForeColor;
			FurnitureCoin.BackColor = Color.Transparent;
		}
		{
			int small = db.UseItems[10]?.Count ?? 0;
			int medium = db.UseItems[11]?.Count ?? 0;
			int large = db.UseItems[12]?.Count ?? 0;

			ToolTipInfo.SetToolTip(FurnitureCoin,
				string.Format(Translation.FurnitureCoinToolTip,
					small, small * 200,
					medium, medium * 400,
					large, large * 700));
		}
		UpdateDisplayUseItem();
		FlowPanelUseItem.ResumeLayout();


		//Resources
		FlowPanelResource.SuspendLayout();
		{
			const int resourceHardcap = 350000;

			Fuel.Text = db.Material.Fuel.ToString();

			if (db.Material.Fuel >= resourceHardcap)
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Fuel.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Fuel < (configUI.HqResLowAlertFuel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertFuel))
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceLowFG;
				Fuel.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Fuel > db.Admiral.MaxResourceRegenerationAmount)
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceOverFG;
				Fuel.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Fuel.ForeColor = configUI.ForeColor;
				Fuel.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip(Fuel, string.Format(Translation.ResourceToolTip,
				resday == null ? 0 : (db.Material.Fuel - resday.Fuel),
				resweek == null ? 0 : (db.Material.Fuel - resweek.Fuel),
				resmonth == null ? 0 : (db.Material.Fuel - resmonth.Fuel)));

			Ammo.Text = db.Material.Ammo.ToString();
			if (db.Material.Ammo >= resourceHardcap)
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Ammo.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Ammo < (configUI.HqResLowAlertAmmo == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertAmmo))
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceLowFG;
				Ammo.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Ammo > db.Admiral.MaxResourceRegenerationAmount)
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceOverFG;
				Ammo.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Ammo.ForeColor = configUI.ForeColor;
				Ammo.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip(Ammo, string.Format(Translation.ResourceToolTip,
				resday == null ? 0 : (db.Material.Ammo - resday.Ammo),
				resweek == null ? 0 : (db.Material.Ammo - resweek.Ammo),
				resmonth == null ? 0 : (db.Material.Ammo - resmonth.Ammo)));

			Steel.Text = db.Material.Steel.ToString();
			if (db.Material.Steel >= resourceHardcap)
			{
				Steel.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Steel.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Steel < (configUI.HqResLowAlertSteel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertSteel))
			{
				Steel.ForeColor = configUI.Headquarters_ResourceLowFG;
				Steel.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Steel > db.Admiral.MaxResourceRegenerationAmount)
			{
				Steel.ForeColor = configUI.Headquarters_ResourceOverFG;
				Steel.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Steel.ForeColor = configUI.ForeColor;
				Steel.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip(Steel, string.Format(Translation.ResourceToolTip,
				resday == null ? 0 : (db.Material.Steel - resday.Steel),
				resweek == null ? 0 : (db.Material.Steel - resweek.Steel),
				resmonth == null ? 0 : (db.Material.Steel - resmonth.Steel)));

			Bauxite.Text = db.Material.Bauxite.ToString();
			if (db.Material.Bauxite >= resourceHardcap)
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Bauxite < (configUI.HqResLowAlertBauxite == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBauxite))
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceLowFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Bauxite > db.Admiral.MaxResourceRegenerationAmount)
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceOverFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Bauxite.ForeColor = configUI.ForeColor;
				Bauxite.BackColor = Color.Transparent;
			}
			ToolTipInfo.SetToolTip(Bauxite, string.Format(Translation.ResourceToolTip,
				resday == null ? 0 : (db.Material.Bauxite - resday.Bauxite),
				resweek == null ? 0 : (db.Material.Bauxite - resweek.Bauxite),
				resmonth == null ? 0 : (db.Material.Bauxite - resmonth.Bauxite)));

		}
		FlowPanelResource.ResumeLayout();

		FlowPanelMaster.ResumeLayout();
		if (!FlowPanelMaster.Visible)
			FlowPanelMaster.Visible = true;
		AdmiralName.Refresh();
		AdmiralComment.Refresh();

	}


	void SystemEvents_UpdateTimerTick()
	{

		KCDatabase db = KCDatabase.Instance;

		if (db.Ships.Count <= 0) return;

		if (Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
		{
			if (ShipCount.Tag as bool? ?? false)
			{
				ShipCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
				ShipCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
			}

			if (EquipmentCount.Tag as bool? ?? false)
			{
				EquipmentCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : Color.Transparent;
				EquipmentCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
			}
		}
	}


	private void Resource_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == System.Windows.Forms.MouseButtons.Right)
			new Dialog.DialogResourceChart().Show(_parentForm);
	}

	private void Resource_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			try
			{
				var mat = KCDatabase.Instance.Material;
				Clipboard.SetText($"{mat.Fuel}/{mat.Ammo}/{mat.Steel}/{mat.Bauxite}/" +
								  $"{mat.InstantRepair}{Translation.CopyToClipboardBuckets}/" +
								  $"{mat.DevelopmentMaterial}{Translation.CopyToClipboardDevelopmentMaterials}/" +
								  $"{mat.InstantConstruction}{Translation.CopyToClipboardInstantConstruction}/" +
								  $"{mat.ModdingMaterial}{Translation.CopyToClipboardImproveMaterial}");
			}
			catch (Exception ex)
			{
				Utility.Logger.Add(3, Translation.FailedToCopyToClipboard + ex.Message);
			}
		}
	}


	private void UpdateDisplayUseItem()
	{
		var db = KCDatabase.Instance;
		var itemID = Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID;
		var item = db.UseItems[itemID];
		var itemMaster = db.MasterUseItems[itemID];
		string tail = "\r\n" + Translation.DisplayUseItemToolTipHint;



		switch (itemMaster?.Name)
		{
			case null:
				DisplayUseItem.Text = "???";
				ToolTipInfo.SetToolTip(DisplayUseItem, string.Format(Translation.UnknownItem, Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID) + tail);
				break;

			// '18 spring event special mode
			case "お米":
			case "梅干":
			case "海苔":
			case "お茶":
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				ToolTipInfo.SetToolTip(DisplayUseItem,
					$"{Translation.Rice}: {db.UseItems[85]?.Count ?? 0}\r\n" +
					$"{Translation.Umeboshi}: {db.UseItems[86]?.Count ?? 0}\r\n" +
					$"{Translation.Nori}: {db.UseItems[87]?.Count ?? 0}\r\n" +
					$"{Translation.Tea}: {db.UseItems[88]?.Count ?? 0}\r\n{tail}");
				break;

			// '19 autumn event special mode
			case "秋刀魚":
			case "鰯":
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				ToolTipInfo.SetToolTip(DisplayUseItem,
					$"{Translation.Sanma}: {db.UseItems[68]?.Count ?? 0}\r\n" +
					$"{Translation.Iwashi}: {db.UseItems[93]?.Count ?? 0}\r\n{tail}");
				break;

			default:
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				ToolTipInfo.SetToolTip(DisplayUseItem,
					itemMaster.Name + tail);
				break;
		}

	}

	private void DisplayUseItem_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Right)
		{
			var db = KCDatabase.Instance;
			var sb = new StringBuilder();
			foreach (var item in db.UseItems.Values)
			{
				sb.Append(item.MasterUseItem.Name).Append(" x ").Append(item.Count).AppendLine();
			}

			MessageBox.Show(sb.ToString(), Translation.ListOfOwnedItems, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}

	private int RealShipCount
	{
		get
		{
			if (KCDatabase.Instance.Battle != null)
				return KCDatabase.Instance.Ships.Count + KCDatabase.Instance.Battle.DroppedShipCount;

			return KCDatabase.Instance.Ships.Count;
		}
	}

	private int RealEquipmentCount
	{
		get
		{
			if (KCDatabase.Instance.Battle != null)
				return KCDatabase.Instance.Equipments.Count + KCDatabase.Instance.Battle.DroppedEquipmentCount;

			return KCDatabase.Instance.Equipments.Count;
		}
	}


	protected override string GetPersistString()
	{
		return "HeadQuarters";
	}


}
