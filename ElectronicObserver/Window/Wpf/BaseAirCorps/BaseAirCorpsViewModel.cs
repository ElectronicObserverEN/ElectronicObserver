using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

public partial class BaseAirCorpsViewModel : AnchorableViewModel
{
	public FormBaseAirCorpsTranslationViewModel FormBaseAirCorps { get; }

	public List<BaseAirCorpsItemViewModel> ControlMember { get; }

	public BaseAirCorpsViewModel() : base("AB", "BaseAirCorps", IconContent.FormBaseAirCorps)
	{
		FormBaseAirCorps = Ioc.Default.GetService<FormBaseAirCorpsTranslationViewModel>()!;

		Title = FormBaseAirCorps.Title;
		FormBaseAirCorps.PropertyChanged += (_, _) => Title = FormBaseAirCorps.Title;

		ControlMember = new();

		for (int i = 0; i < 9; i++)
		{
			ControlMember.Add(new(CopyOrganizationCommand, DisplayRelocatedEquipmentsCommand));
		}

		APIObserver api = APIObserver.Instance;

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
		foreach (BaseAirCorpsItemViewModel control in ControlMember)
		{
			control.ConfigurationChanged();
		}

		if (KCDatabase.Instance.BaseAirCorps.Any())
		{
			Updated(null, null);
		}
	}

	private void Updated(string? apiname, dynamic? data)
	{
		List<int> keys = KCDatabase.Instance.BaseAirCorps.Keys.ToList();

		if (Utility.Configuration.Config.FormBaseAirCorps.ShowEventMapOnly)
		{
			List<int> eventAreaCorps = KCDatabase.Instance.BaseAirCorps.Values
				.Where(b => KCDatabase.Instance.MapArea[b.MapAreaID] is { MapType: 1 })
				.Select(b => b.ID)
				.ToList();

			if (eventAreaCorps.Any())
			{
				keys = eventAreaCorps;
			}
		}

		for (int i = 0; i < ControlMember.Count; i++)
		{
			ControlMember[i].Update(i < keys.Count ? keys.ElementAt(i) : -1);
		}

		List<IBaseAirCorpsSquadron> squadrons = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b != null)
			.SelectMany(b => b.Squadrons.Values)
			.Where(s => s != null)
			.ToList();

		bool isNotReplenished = squadrons.Any(s => s.State == 1 && s.AircraftCurrent < s.AircraftMax);
		bool isTired = squadrons.Any(s => s is { State: 1, Condition: AirBaseCondition.Tired });
		bool isVeryTired = squadrons.Any(s => s is { State: 1, Condition: AirBaseCondition.VeryTired });
		bool isAllSparkled = squadrons.Any() && squadrons
			.Where(s => s.State is 1)
			.All(s => s.Condition is AirBaseCondition.Sparkled);

		Icon = true switch
		{
			_ when isNotReplenished => IconContent.FleetNotReplenished,
			_ when isVeryTired => IconContent.ConditionVeryTired,
			_ when isTired => IconContent.ConditionTired,
			_ when isAllSparkled => IconContent.ConditionSparkle,
			_ => IconContent.FormBaseAirCorps,
		};
	}

	[RelayCommand]
	private void CopyOrganization(int? areaid)
	{
		areaid ??= -1;

		StringBuilder sb = new();

		IEnumerable<BaseAirCorpsData> baseaircorps = KCDatabase.Instance.BaseAirCorps.Values;
		if (areaid != -1)
		{
			baseaircorps = baseaircorps.Where(c => c.MapAreaID == areaid);
		}

		foreach (BaseAirCorpsData corps in baseaircorps)
		{
			string areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].NameEN : "Unknown Area";

			sb.AppendFormat($"{FormBaseAirCorps.CopyOrganizationFormat}\n",
				(areaid == -1 ? (areaName + "：") : "") + corps.Name,
				Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
				Calculator.GetAirSuperiority(corps),
				corps.Distance);

			IBaseAirCorpsSquadron[] sq = corps.Squadrons.Values.ToArray();

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
						IEquipmentData? eq = sq[i].EquipmentInstance;

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
		{
			message = GeneralRes.ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Detail;
		}

		MessageBox.Show(message, GeneralRes.ContextMenuBaseAirCorps_DisplayRelocatedEquipments_Title, MessageBoxButton.OK, MessageBoxImage.Information);
	}
}
