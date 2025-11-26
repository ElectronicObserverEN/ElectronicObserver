using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

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
			Text = "*",
			Visible = false,
		};

		ActionKind = new()
		{
			Text = "*",
			Visible = false,
		};

		AirSuperiority = new()
		{
			Text = "*",
			Visible = false,
		};

		Distance = new()
		{
			Text = "*",
			Visible = false,
		};

		Squadrons = new()
		{
			Visible = false,
		};
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
			List<IBaseAirCorpsData> allCorps = db.BaseAirCorps.Values.Where(ab => ab.MapAreaID == corps.MapAreaID).OfType<IBaseAirCorpsData>().ToList();

			MapAreaId = corps.MapAreaID;
			Name.Text = string.Format("#{0} - {1}", corps.MapAreaID, corps.Name);
			Name.Tag = corps.MapAreaID;
			var sb = new StringBuilder();


			string areaName = KCDatabase.Instance.MapArea.ContainsKey(corps.MapAreaID) ? KCDatabase.Instance.MapArea[corps.MapAreaID].NameEN : FormBaseAirCorps.UnknownArea;

			sb.AppendLine(FormBaseAirCorps.Area + areaName);

			// state

			// 疲労
			AirBaseCondition condition = corps.Squadrons.Values.Any(s => s.State is 1) switch
			{
				true => corps.Squadrons.Values
					.Max(sq => sq?.Condition ?? AirBaseCondition.Sparkled),
				_ => AirBaseCondition.Normal,
			};

			Name.ConditionIcon = condition switch
			{
				AirBaseCondition.VeryTired => IconContent.ConditionVeryTired,
				AirBaseCondition.Tired => IconContent.ConditionTired,
				AirBaseCondition.Sparkled => IconContent.ConditionSparkle,
				_ => null,
			};

			switch (condition)
			{
				case AirBaseCondition.Sparkled:
					sb.AppendLine(GeneralRes.MaximumMorale);
					break;

				case AirBaseCondition.Tired:
					sb.AppendLine(GeneralRes.Tired);
					break;

				case AirBaseCondition.VeryTired:
					sb.AppendLine(GeneralRes.VeryTired);
					break;
			}

			if (corps.Squadrons.Values.Any(sq => sq != null && sq.AircraftCurrent < sq.AircraftMax))
			{
				// 未補給
				Name.SupplyIcon = IconContent.FleetNotReplenished;
				sb.AppendLine(FormBaseAirCorps.Unsupplied);
			}
			else
			{
				Name.SupplyIcon = null;
			}

			sb.AppendLine(string.Format(FormBaseAirCorps.AirControlSummary,
				db.BaseAirCorps.Values.Where(c => c.MapAreaID == corps.MapAreaID && c.ActionKind == AirBaseActionKind.AirDefense).Select(Calculator.GetAirSuperiority).DefaultIfEmpty(0).Sum(),
				db.BaseAirCorps.Values.Where(c => c.MapAreaID == corps.MapAreaID && c.ActionKind == AirBaseActionKind.AirDefense).Select(c => Calculator.GetHighAltitudeAirSuperiority(c, allCorps, false)).DefaultIfEmpty(0).Sum()
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

				if (corps.ActionKind == AirBaseActionKind.AirDefense)
				{
					int airSuperiorityHighAltitude = Calculator.GetHighAltitudeAirSuperiority(corps, allCorps, false);
					int airSuperiorityHighAltitudeMax = Calculator.GetHighAltitudeAirSuperiority(corps, allCorps, true);

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

			IEquipmentData? eq = squadron.EquipmentInstance;

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
						case AirBaseCondition.Sparkled:
							sb.Append($"[{GeneralRes.MaximumMorale}] ");
							break;

						case AirBaseCondition.Tired:
							sb.Append($"[{GeneralRes.Tired}] ");
							break;

						case AirBaseCondition.VeryTired:
							sb.Append($"[{GeneralRes.VeryTired}] ");
							break;

						case AirBaseCondition.Normal:
						default:
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
