using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetStatusViewModel : ObservableObject
{
	public FormFleetTranslationViewModel FormFleet { get; }

	public FleetItemControlViewModel Name { get; } = new();
	public FleetStateViewModel State { get; } = new();
	public FleetItemControlViewModel AirSuperiority { get; } = new();
	public FleetItemControlViewModel SearchingAbility { get; } = new();
	public FleetItemControlViewModel AntiAirPower { get; } = new();

	private int FleetId { get; }
	public int BranchWeight { get; private set; } = 1;

	public List<TotalRate> NightRecons { get; private set; } = new();
	public List<TotalRate> Flares { get; private set; } = new();
	public List<TotalRate> SmokeGeneratorRates { get; private set; } = [];

	public FleetStatusViewModel(int fleetId)
	{
		FormFleet = Ioc.Default.GetService<FormFleetTranslationViewModel>()!;

		FleetId = fleetId;

		ConfigurationChanged();
	}

	[RelayCommand]
	private void IncreaseBranchWeight()
	{
		BranchWeight--;
		if (BranchWeight <= 0)
		{
			BranchWeight = 4;
		}

		Update(KCDatabase.Instance.Fleet[FleetId]);
	}

	public void Update(FleetData? fleet)
	{
		if (fleet is null) return;

		Name.Text = fleet.Name;
		{
			List<IShipData> members = fleet.MembersInstance!
				.Where(s => s is not null)
				.ToList();

			int levelSum = members.Sum(s => s.Level);

			int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
			int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

			int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
			int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

			int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

			string supporttype = fleet.SupportType switch
			{
				SupportType.None => FleetResources.SupportTypeNone,
				SupportType.Aerial => FleetResources.SupportTypeAerial,
				SupportType.Shelling => FleetResources.SupportTypeShelling,
				SupportType.Torpedo => FleetResources.SupportTypeTorpedo,
				SupportType.AntiSubmarine => FleetResources.SupportTypeAntiSubmarine,
				_ => FormFleet.SupportTypeNone,
			};

			double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
			int tp = Calculator.GetTpDamage(fleet);
			int tankTpE2 = TankTpGauge.Spring25E2.GetTp(fleet);
			int tankTpE5 = TankTpGauge.Spring25E5.GetTp(fleet);

			bool hasZeroSlotAircraft = fleet.MembersInstance!
				.Where(s => s is not null)
				.Any(s => s.HasZeroSlotAircraft());

			string? zeroSlotWarning = hasZeroSlotAircraft switch
			{
				true => $"\n{DataRes.ZeroSlotAircraftWarning}",
				_ => null,
			};

			// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
			IEnumerable<int> transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
			IEnumerable<int> landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType is EquipmentTypes.LandingCraft or EquipmentTypes.SpecialAmphibiousTank));
			IEnumerable<int> radar = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.IsSurfaceRadar == true));

			Name.ToolTip = string.Format(FleetResources.FleetNameToolTip,
				levelSum,
				(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
				Constants.GetSpeed(speed),
				supporttype,
				members.Sum(s => s.FirepowerTotal),
				members.Sum(s => s.TorpedoTotal),
				members.Sum(s => s.AATotal),
				members.Sum(s => s.ASWTotal),
				members.Sum(s => s.LOSTotal),
				transport.Sum(),
				transport.Count(i => i > 0),
				landing.Sum(),
				landing.Count(i => i > 0),
				expeditionBonus,
				tp,
				(int)(tp * 0.7),
				fueltotal,
				ammototal,
				fuelunit,
				ammounit,
				members.Sum(s => s.ExpeditionFirepowerTotal),
				members.Sum(s => s.ExpeditionAATotal),
				members.Sum(s => s.ExpeditionASWTotal),
				members.Sum(s => s.ExpeditionLOSTotal),
				radar.Sum(),
				radar.Count(i => i > 0),
				zeroSlotWarning,
				tankTpE2,
				(int)(tankTpE2 * 0.7),
				tankTpE5,
				(int)(tankTpE5 * 0.7)
			);

			NightRecons = fleet.NightRecons().TotalRate();
			Flares = fleet.Flares().TotalRate();
			SmokeGeneratorRates = fleet.GetSmokeTriggerRates().TotalRate();
		}

		State.UpdateFleetState(fleet);


		//制空戦力計算
		{
			int airSuperiority = fleet.GetAirSuperiority();
			bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;
			AirSuperiority.Text = fleet.GetAirSuperiorityString();
			AirSuperiority.ToolTip = string.Format(GeneralRes.ASTooltip,
				(int)(airSuperiority / 3.0),
				(int)(airSuperiority / 1.5),
				Math.Max((int)(airSuperiority * 1.5 - 1), 0),
				Math.Max((int)(airSuperiority * 3.0 - 1), 0),
				includeLevel ? FormFleet.WithoutProficiency : FormFleet.WithProficiency,
				includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet));
		}


		//索敵能力計算
		SearchingAbility.Text = fleet.GetSearchingAbilityString(BranchWeight);
		{
			StringBuilder sb = new();
			double probStart = fleet.GetContactProbability();
			Dictionary<int, double> probSelect = fleet.GetContactSelectionProbability();

			sb.AppendFormat(FormFleet.FleetLosToolTip,
				BranchWeight,
				probStart,
				probStart * 0.6);

			if (probSelect.Count > 0)
			{
				sb.AppendLine(FormFleet.ContactSelection);

				foreach ((int accuracy, double probability) in probSelect.OrderBy(p => p.Key))
				{
					sb.AppendFormat(FormFleet.ContactProbability + "\r\n", accuracy, probability);
				}
			}

			SearchingAbility.ToolTip = sb.ToString();
		}

		// 対空能力計算
		{
			StringBuilder sb = new();
			double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

			AntiAirPower.Text = lineahead.ToString("0.0");

			sb.AppendFormat(GeneralRes.AntiAirPower,
				lineahead,
				Calculator.GetAdjustedFleetAAValue(fleet, 2),
				Calculator.GetAdjustedFleetAAValue(fleet, 3));

			AntiAirPower.ToolTip = sb.ToString();
		}
	}

	public void Refresh()
	{
		State.RefreshFleetState();
	}

	public void ConfigurationChanged()
	{
		State.RefreshFleetState();
	}
}
