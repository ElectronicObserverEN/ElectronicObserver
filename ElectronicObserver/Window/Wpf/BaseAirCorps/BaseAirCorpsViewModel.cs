using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

public class BaseAirCorpsSquadronViewModel : BaseAirCorpsItemControlViewModel
{
	public Utility.Storage.SerializableFont Font { get; internal set; }
	public bool ShowAircraft { get; set; }
	public bool ShowAircraftLevelByNumber { get; set; }
	public LevelVisibilityFlag LevelVisibility { get; set; }

	public ObservableCollection<ShipSlotViewModel> SlotList { get; } = new()
	{
		new(),
		new(),
		new(),
		new(),
	};

	public BaseAirCorpsSquadronViewModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Font)) return;

			foreach (ShipSlotViewModel slot in SlotList)
			{
				slot.Font = Font;
			}
		};
	}

	public void SetSlotList(BaseAirCorpsData corps)
	{
		int slotLength = corps?.Squadrons?.Count() ?? 0;
		/*
		if (SlotList.Count != slotLength)
		{
			SlotList = new SlotItem[slotLength];
			for (int i = 0; i < SlotList.Length; i++)
			{
				SlotList[i] = new SlotItem();
			}
		}
		*/
		for (int i = 0; i < SlotList.Count; i++)
		{
			var squadron = corps[i + 1];
			IEquipmentData? eq = squadron.EquipmentInstance;

			switch (squadron.State)
			{
				case 0:     // 未配属
				case 2:     // 配置転換中
				default:
					// SlotList[i].EquipmentID = -1;
					// SlotList[i].AircraftCurrent =
					// SlotList[i].AircraftMax =
					// SlotList[i].Level =
					// SlotList[i].AircraftLevel = 0;
					// clear equip slot even though it exists in api in case 2 (relocation)
					SlotList[i].Equipment = null;
					SlotList[i].AircraftMax = 0;
					SlotList[i].AircraftCurrent = 0;
					break;
				case 1:     // 配属済み
					if (eq == null)
						goto case 0;
					// SlotList[i].EquipmentID = eq.EquipmentID;
					// SlotList[i].AircraftCurrent = squadron.AircraftCurrent;
					// SlotList[i].AircraftMax = squadron.AircraftMax;
					// SlotList[i].Level = eq.Level;
					// SlotList[i].AircraftLevel = eq.AircraftLevel;
					SlotList[i].Equipment = eq;
					SlotList[i].AircraftMax = squadron.AircraftMax;
					SlotList[i].AircraftCurrent = squadron.AircraftCurrent;
					break;
			}

		}

		// _slotSize = slotLength;
		// PropertyChanged();
	}
}

public class BaseAirCorpsItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public bool Visible { get; set; }
	public Enum? ImageIndex { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }

	public Visibility Visibility => Visible.ToVisibility();
	public ImageSource? Icon => ImageIndex switch
	{
		ResourceManager.EquipmentContent e => ImageSourceIcons.GetEquipmentIcon((EquipmentIconType)e),
		IconContent i => ImageSourceIcons.GetIcon(i),
		_ => null
	};


}

public class BaseAirCorpsItemViewModel : ObservableObject
{
	public FormBaseAirCorpsTranslationViewModel FormBaseAirCorps { get; }

	public BaseAirCorpsItemControlViewModel Name { get; }
	public BaseAirCorpsItemControlViewModel ActionKind { get; }
	public BaseAirCorpsItemControlViewModel AirSuperiority { get; }
	public BaseAirCorpsItemControlViewModel Distance { get; }
	public BaseAirCorpsSquadronViewModel Squadrons { get; }

	public int MapAreaId { get; set; }
	public Visibility Visibility { get; set; } = Visibility.Collapsed;
	public ICommand CopyOrganizationCommand { get; }
	public ICommand DisplayRelocatedEquipmentsCommand { get; }

	public BaseAirCorpsItemViewModel(ICommand copyOrganizationCommand, ICommand displayRelocatedEquipmentsCommand)
	{
		FormBaseAirCorps = Ioc.Default.GetService<FormBaseAirCorpsTranslationViewModel>()!;

		CopyOrganizationCommand = copyOrganizationCommand;
		DisplayRelocatedEquipmentsCommand = displayRelocatedEquipmentsCommand;

		Name = new()
		{
			// Name = "Name",
			Text = "*",
			// Anchor = AnchorStyles.Left,
			// TextAlign = ContentAlignment.MiddleLeft,
			// ImageAlign = ContentAlignment.MiddleRight,
			// ImageList = ResourceManager.Instance.Icons,
			// Padding = new Padding(2, 2, 2, 2),
			// Margin = new Padding(2, 1, 2, 1),      // ここを 2,0,2,0 にすると境界線の描画に問題が出るので
			// AutoSize = true,
			// ContextMenuStrip = parent.ContextMenuBaseAirCorps,
			Visible = false,
			// Cursor = Cursors.Help
		};

		ActionKind = new()
		{
			Text = "*",
			// Anchor = AnchorStyles.Left,
			// TextAlign = ContentAlignment.MiddleLeft,
			// ImageAlign = ContentAlignment.MiddleCenter,
			// //ActionKind.ImageList =
			// Padding = new Padding(2, 2, 2, 2),
			// Margin = new Padding(2, 0, 2, 0),
			// AutoSize = true,
			Visible = false
		};

		AirSuperiority = new()
		{
			Text = "*",
			// Anchor = AnchorStyles.Left,
			// TextAlign = ContentAlignment.MiddleLeft,
			// ImageAlign = ContentAlignment.MiddleLeft,
			// ImageList = ResourceManager.Instance.Equipments,
			ImageIndex = ResourceManager.EquipmentContent.CarrierBasedFighter,
			// Padding = new Padding(2, 2, 2, 2),
			// Margin = new Padding(2, 0, 2, 0),
			// AutoSize = true,
			Visible = false
		};

		Distance = new()
		{
			Text = "*",
			// Anchor = AnchorStyles.Left,
			// TextAlign = ContentAlignment.MiddleLeft,
			// ImageAlign = ContentAlignment.MiddleLeft,
			// ImageList = ResourceManager.Instance.Icons,
			ImageIndex = IconContent.ParameterAircraftDistance,
			// Padding = new Padding(2, 2, 2, 2),
			// Margin = new Padding(2, 0, 2, 0),
			// AutoSize = true,
			Visible = false
		};

		Squadrons = new()
		{
			// Anchor = AnchorStyles.Left,
			// Padding = new Padding(0, 1, 0, 2),
			// Margin = new Padding(2, 0, 2, 0),
			// Size = new Size(40, 20),
			// AutoSize = true,
			Visible = false
		};
		// Squadrons.ResumeLayout();

		// ConfigurationChanged(parent);

		// ToolTipInfo = parent.ToolTipInfo;

	}

	public void Update(int baseAirCorpsID)
	{
		KCDatabase db = KCDatabase.Instance;
		var corps = db.BaseAirCorps[baseAirCorpsID];

		if (corps == null)
		{
			baseAirCorpsID = -1;
			MapAreaId = -1;
		}
		else
		{
			MapAreaId = corps.MapAreaID;
			Name.Text = string.Format("#{0} - {1}", corps.MapAreaID, corps.Name);
			Name.Tag = corps.MapAreaID;
			var sb = new StringBuilder();


			string areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].NameEN : FormBaseAirCorps.UnknownArea;

			sb.AppendLine(FormBaseAirCorps.Area + areaName);

			// state

			if (corps.Squadrons.Values.Any(sq => sq != null && sq.Condition > 1))
			{
				// 疲労
				int tired = corps.Squadrons.Values.Max(sq => sq?.Condition ?? 0);

				if (tired == 2)
				{
					// Name.ImageAlign = ContentAlignment.MiddleRight;
					Name.ImageIndex = IconContent.ConditionTired;
					sb.AppendLine(GeneralRes.Tired);

				}
				else
				{
					// Name.ImageAlign = ContentAlignment.MiddleRight;
					Name.ImageIndex = IconContent.ConditionVeryTired;
					sb.AppendLine(GeneralRes.VeryTired);

				}

			}
			else if (corps.Squadrons.Values.Any(sq => sq != null && sq.AircraftCurrent < sq.AircraftMax))
			{
				// 未補給
				// Name.ImageAlign = ContentAlignment.MiddleRight;
				Name.ImageIndex = IconContent.FleetNotReplenished;
				sb.AppendLine(FormBaseAirCorps.Unsupplied);

			}
			else
			{
				// Name.ImageAlign = ContentAlignment.MiddleCenter;
				Name.ImageIndex = null;

			}

			sb.AppendLine(string.Format(FormBaseAirCorps.AirControlSummary,
				db.BaseAirCorps.Values.Where(c => c.MapAreaID == corps.MapAreaID && c.ActionKind == 2).Select(c => Calculator.GetAirSuperiority(c)).DefaultIfEmpty(0).Sum(),
				db.BaseAirCorps.Values.Where(c => c.MapAreaID == corps.MapAreaID && c.ActionKind == 2).Select(c => Calculator.GetAirSuperiority(c, isHighAltitude: true)).DefaultIfEmpty(0).Sum()
			));

			Name.ToolTip = sb.ToString();


			ActionKind.Text = "[" + Constants.GetBaseAirCorpsActionKind(corps.ActionKind) + "]";

			{
				int airSuperiority = Calculator.GetAirSuperiority(corps);
				if (Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange)
				{
					int airSuperiority_max = Calculator.GetAirSuperiority(corps, true);
					if (airSuperiority < airSuperiority_max)
						AirSuperiority.Text = string.Format("{0} ～ {1}", airSuperiority, airSuperiority_max);
					else
						AirSuperiority.Text = airSuperiority.ToString();
				}
				else
				{
					AirSuperiority.Text = airSuperiority.ToString();
				}

				var tip = new StringBuilder();
				tip.AppendFormat(GeneralRes.BaseTooltip,
					(int)(airSuperiority / 3.0),
					(int)(airSuperiority / 1.5),
					Math.Max((int)(airSuperiority * 1.5 - 1), 0),
					Math.Max((int)(airSuperiority * 3.0 - 1), 0));

				if (corps.ActionKind == 2)
				{
					int airSuperiorityHighAltitude = Calculator.GetAirSuperiority(corps, isHighAltitude: true);
					int airSuperiorityHighAltitudeMax = Calculator.GetAirSuperiority(corps, isAircraftLevelMaximum: true, isHighAltitude: true);

					tip.AppendFormat(GeneralRes.HighAltitudeAirState,
						Utility.Configuration.Config.FormFleet.ShowAirSuperiorityRange && airSuperiorityHighAltitude != airSuperiorityHighAltitudeMax ?
							$"{airSuperiorityHighAltitude} ～ {airSuperiorityHighAltitudeMax}" :
							airSuperiorityHighAltitude.ToString(),
						(int)(airSuperiorityHighAltitude / 3.0),
						(int)(airSuperiorityHighAltitude / 1.5),
						Math.Max((int)(airSuperiorityHighAltitude * 1.5 - 1), 0),
						Math.Max((int)(airSuperiorityHighAltitude * 3.0 - 1), 0));
				}

				AirSuperiority.ToolTip = tip.ToString();
			}
			int dist_text = corps.Distance;

			Distance.Text = dist_text.ToString();

			Squadrons.SetSlotList(corps);
			Squadrons.ToolTip = GetEquipmentString(corps);
			Distance.ToolTip = string.Format(FormBaseAirCorps.TotalDistance, corps.Distance);

		}


		Name.Visible =
			ActionKind.Visible =
				AirSuperiority.Visible =
					Distance.Visible =
						Squadrons.Visible =
							baseAirCorpsID != -1;

		Visibility = (baseAirCorpsID != -1).ToVisibility();
	}

	public void ConfigurationChanged()
	{

		var config = Utility.Configuration.Config;

		var mainfont = config.UI.MainFont;
		var subfont = config.UI.SubFont;

		// Name.Font = mainfont;
		// ActionKind.Font = mainfont;
		// AirSuperiority.Font = mainfont;
		// Distance.Font = mainfont;
		Squadrons.Font = subfont;

		Squadrons.ShowAircraft = config.FormFleet.ShowAircraft;
		Squadrons.ShowAircraftLevelByNumber = config.FormFleet.ShowAircraftLevelByNumber;
		Squadrons.LevelVisibility = config.FormFleet.EquipmentLevelVisibility;

	}


	private string GetEquipmentString(BaseAirCorpsData? corps)
	{
		StringBuilder sb = new();

		if (corps == null) return GeneralRes.BaseNotOpen;

		foreach (var squadron in corps.Squadrons.Values)
		{
			if (squadron == null) continue;

			EquipmentData? eq = squadron.EquipmentInstance;

			switch (squadron.State)
			{
				case 0:     // 未配属
				default:
					sb.AppendLine(FormBaseAirCorps.Empty);
					break;

				case 1:     // 配属済み
					if (eq == null) goto case 0;

					sb.AppendFormat("[{0}/{1}] ",
						squadron.AircraftCurrent,
						squadron.AircraftMax);

					switch (squadron.Condition)
					{
						case 1:
						default:
							break;
						case 2:
							sb.Append("[" + GeneralRes.Tired + "] ");
							break;
						case 3:
							sb.Append("[" + GeneralRes.VeryTired + "] ");
							break;
					}

					sb.AppendFormat($"{FormBaseAirCorps.Range}\n", eq.NameWithLevel, eq.MasterEquipment.AircraftDistance);
					break;

				case 2:     // 配置転換中
					sb.AppendFormat($"{GeneralRes.BaseRelocate}\n",
						DateTimeHelper.TimeToCSVString(squadron.RelocatedTime));
					break;
			}
		}

		return sb.ToString();
	}
}

public partial class BaseAirCorpsViewModel : AnchorableViewModel
{
	public FormBaseAirCorpsTranslationViewModel FormBaseAirCorps { get; }

	public List<BaseAirCorpsItemViewModel> ControlMember { get; }

	public BaseAirCorpsViewModel() : base("AB", "BaseAirCorps",
		ImageSourceIcons.GetIcon(IconContent.FormBaseAirCorps))
	{
		FormBaseAirCorps = Ioc.Default.GetService<FormBaseAirCorpsTranslationViewModel>()!;

		Title = FormBaseAirCorps.Title;
		FormBaseAirCorps.PropertyChanged += (_, _) => Title = FormBaseAirCorps.Title;

		ControlMember = new();
		// TableMember.SuspendLayout();
		for (int i = 0; i < 9; i++)
		{
			ControlMember.Add(new(CopyOrganizationCommand, DisplayRelocatedEquipmentsCommand));
		}
		// TableMember.ResumeLayout();

		var api = Observer.APIObserver.Instance;

		api.ApiPort_Port.ResponseReceived += Updated;
		api.ApiGetMember_MapInfo.ResponseReceived += Updated;
		api.ApiGetMember_BaseAirCorps.ResponseReceived += Updated;
		api.ApiReqAirCorps_ChangeDeploymentBase.ResponseReceived += Updated;
		api.ApiReqAirCorps_ChangeName.ResponseReceived += Updated;
		api.ApiReqAirCorps_SetAction.ResponseReceived += Updated;
		api.ApiReqAirCorps_SetPlane.ResponseReceived += Updated;
		api.ApiReqAirCorps_Supply.ResponseReceived += Updated;
		api.ApiReqAirCorps_ExpandBase.ResponseReceived += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		var c = Utility.Configuration.Config;

		// TableMember.SuspendLayout();

		// Font = c.UI.MainFont;

		foreach (var control in ControlMember)
			control.ConfigurationChanged();

		// ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());

		// TableMember.ResumeLayout();

		if (KCDatabase.Instance.BaseAirCorps.Any())
			Updated(null, null);
	}


	void Updated(string apiname, dynamic data)
	{
		var keys = KCDatabase.Instance.BaseAirCorps.Keys;

		if (Utility.Configuration.Config.FormBaseAirCorps.ShowEventMapOnly)
		{
			var eventAreaCorps = KCDatabase.Instance.BaseAirCorps.Values.Where(b =>
			{
				var maparea = KCDatabase.Instance.MapArea[b.MapAreaID];
				return maparea != null && maparea.MapType == 1;
			}).Select(b => b.ID);

			if (eventAreaCorps.Any())
				keys = eventAreaCorps;
		}


		// TableMember.SuspendLayout();
		// TableMember.RowCount = keys.Count();
		for (int i = 0; i < ControlMember.Count; i++)
		{
			ControlMember[i].Update(i < keys.Count() ? keys.ElementAt(i) : -1);
		}
		// TableMember.ResumeLayout();

		// set icon
		{
			var squadrons = KCDatabase.Instance.BaseAirCorps.Values.Where(b => b != null)
				.SelectMany(b => b.Squadrons.Values)
				.Where(s => s != null);
			bool isNotReplenished = squadrons.Any(s => s.State == 1 && s.AircraftCurrent < s.AircraftMax);
			bool isTired = squadrons.Any(s => s.State == 1 && s.Condition == 2);
			bool isVeryTired = squadrons.Any(s => s.State == 1 && s.Condition == 3);

			IconContent imageIndex;

			if (isNotReplenished)
				imageIndex = IconContent.FleetNotReplenished;
			else if (isVeryTired)
				imageIndex = IconContent.ConditionVeryTired;
			else if (isTired)
				imageIndex = IconContent.ConditionTired;
			else
				imageIndex = IconContent.FormBaseAirCorps;

			IconSource = ImageSourceIcons.GetIcon(imageIndex);

			// if (Icon != null) ResourceManager.DestroyIcon(Icon);
			// Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[imageIndex]);
			// if (Parent != null) Parent.Refresh();       //アイコンを更新するため
		}
	}

	private void ContextMenuBaseAirCorps_Opening()
	{
		/*
		if (KCDatabase.Instance.BaseAirCorps.Count == 0)
		{
			e.Cancel = true;
			return;
		}

		if (ContextMenuBaseAirCorps.SourceControl.Name == "Name")
			ContextMenuBaseAirCorps_CopyOrganization.Tag = ContextMenuBaseAirCorps.SourceControl.Tag as int? ?? -1;
		else
			ContextMenuBaseAirCorps_CopyOrganization.Tag = -1;
		*/
	}

	[ICommand]
	private void CopyOrganization(int? areaid)
	{
		areaid ??= -1;

		var sb = new StringBuilder();
		// int areaid = ContextMenuBaseAirCorps_CopyOrganization.Tag as int? ?? -1;

		var baseaircorps = KCDatabase.Instance.BaseAirCorps.Values;
		if (areaid != -1)
			baseaircorps = baseaircorps.Where(c => c.MapAreaID == areaid);

		foreach (var corps in baseaircorps)
		{

			string areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].NameEN : "Unknown Area";

			sb.AppendFormat($"{FormBaseAirCorps.CopyOrganizationFormat}\n",
				(areaid == -1 ? (areaName + "：") : "") + corps.Name,
				Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
				Calculator.GetAirSuperiority(corps),
				corps.Distance);

			var sq = corps.Squadrons.Values.ToArray();

			for (int i = 0; i < sq.Length; i++)
			{
				if (i > 0)
					sb.Append(", ");

				if (sq[i] == null)
				{
					sb.Append(GeneralRes.BaseUnknown);
					continue;
				}

				switch (sq[i].State)
				{
					case 0:
						sb.Append(GeneralRes.BaseUnassigned);
						break;
					case 1:
					{
						var eq = sq[i].EquipmentInstance;

						sb.Append(eq?.NameWithLevel ?? FormBaseAirCorps.Empty);

						if (sq[i].AircraftCurrent < sq[i].AircraftMax)
							sb.AppendFormat("[{0}/{1}]", sq[i].AircraftCurrent, sq[i].AircraftMax);
					}
					break;
					case 2:
						sb.Append("(" + GeneralRes.BaseRedeployment + ")");
						break;
				}
			}

			sb.AppendLine();
		}

		Clipboard.SetDataObject(sb.ToString());
	}

	[ICommand]
	private void DisplayRelocatedEquipments()
	{
		string message = string.Join("\r\n", KCDatabase.Instance.RelocatedEquipments.Values
			.Where(eq => eq.EquipmentInstance != null)
			.Select(eq => string.Format("{0} ({1}～)", eq.EquipmentInstance.NameWithLevel, DateTimeHelper.TimeToCSVString(eq.RelocatedTime))));

		if (message.Length == 0)
			message = GeneralRes.ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Detail;

		MessageBox.Show(message, GeneralRes.ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Title, MessageBoxButton.OK, MessageBoxImage.Information);
	}
}
