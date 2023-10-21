using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

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

	[RelayCommand]
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

	[RelayCommand]
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
