using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Services.Data;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Control;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public partial class ConfigurationFleetViewModel : ConfigurationViewModelBase
{
	public ConfigurationFleetTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormFleet Config { get; }

	public List<FleetStateDisplayMode> FleetStateDisplayModes { get; }
	public List<AirSuperiorityMethod> AirSuperiorityMethods { get; }
	public List<LevelVisibilityFlag> LevelVisibilityFlags { get; }
	public List<ShouldDisplayTankTpGaugeViewModel> GaugeList { get; }

	public bool ShowAircraft { get; set; }

	public int SearchingAbilityMethod { get; set; }

	public bool IsScrollable { get; set; }

	public bool FixShipNameWidth { get; set; }

	public bool ShortenHPBar { get; set; }

	public bool ShowNextExp { get; set; }

	public LevelVisibilityFlag EquipmentLevelVisibility { get; set; }

	public bool ShowAircraftLevelByNumber { get; set; }

	public AirSuperiorityMethod AirSuperiorityMethod { get; set; }

	public bool ShowAnchorageRepairingTimer { get; set; }

	public bool BlinkAtCompletion { get; set; }

	public bool ShowConditionIcon { get; set; }

	public int FixedShipNameWidth { get; set; }

	public bool ShowAirSuperiorityRange { get; set; }

	public bool ReflectAnchorageRepairHealing { get; set; }

	public bool EmphasizesSubFleetInPort { get; set; }

	public bool BlinkAtDamaged { get; set; }

	public FleetStateDisplayMode FleetStateDisplayMode { get; set; }

	public bool AppliesSallyAreaColor { get; set; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(CanSelectTankTpGauges))]
	public partial bool DisplayOnlyCurrentEventTankTp { get; set; }

	public bool CanSelectTankTpGauges => !DisplayOnlyCurrentEventTankTp;

	public ConfigurationFleetViewModel(Configuration.ConfigurationData.ConfigFormFleet config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationFleetTranslationViewModel>();
		ITransportGaugeService gauges = Ioc.Default.GetRequiredService<ITransportGaugeService>();

		FleetStateDisplayModes = Enum.GetValues<FleetStateDisplayMode>().ToList();
		AirSuperiorityMethods = Enum.GetValues<AirSuperiorityMethod>().ToList();
		LevelVisibilityFlags = Enum.GetValues<LevelVisibilityFlag>().ToList();

		GaugeList = gauges.GetEventLandingGauges(false)
			.Select(gauge => new ShouldDisplayTankTpGaugeViewModel(gauge))
			.ToList();

		PropertyChanged += OnDisplayOnlyCurrentEventTankTpChanged;

		Config = config;
		Load();
	}

	private void OnDisplayOnlyCurrentEventTankTpChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(DisplayOnlyCurrentEventTankTp)) return;

		if (DisplayOnlyCurrentEventTankTp)
		{
			foreach (ShouldDisplayTankTpGaugeViewModel vm in GaugeList)
			{
				vm.ShouldDisplay = false;
			}
		}
	}

	private void Load()
	{
		ShowAircraft = Config.ShowAircraft;
		SearchingAbilityMethod = Config.SearchingAbilityMethod;
		IsScrollable = Config.IsScrollable;
		FixShipNameWidth = Config.FixShipNameWidth;
		ShortenHPBar = Config.ShortenHPBar;
		ShowNextExp = Config.ShowNextExp;
		EquipmentLevelVisibility = Config.EquipmentLevelVisibility;
		ShowAircraftLevelByNumber = Config.ShowAircraftLevelByNumber;
		AirSuperiorityMethod = (AirSuperiorityMethod)Config.AirSuperiorityMethod;
		ShowAnchorageRepairingTimer = Config.ShowAnchorageRepairingTimer;
		BlinkAtCompletion = Config.BlinkAtCompletion;
		ShowConditionIcon = Config.ShowConditionIcon;
		FixedShipNameWidth = Config.FixedShipNameWidth;
		ShowAirSuperiorityRange = Config.ShowAirSuperiorityRange;
		ReflectAnchorageRepairHealing = Config.ReflectAnchorageRepairHealing;
		EmphasizesSubFleetInPort = Config.EmphasizesSubFleetInPort;
		BlinkAtDamaged = Config.BlinkAtDamaged;
		FleetStateDisplayMode = (FleetStateDisplayMode)Config.FleetStateDisplayMode;
		AppliesSallyAreaColor = Config.AppliesSallyAreaColor;
		DisplayOnlyCurrentEventTankTp = Config.DisplayOnlyCurrentEventTankTp;

		foreach (ShouldDisplayTankTpGaugeViewModel vm in GaugeList)
		{
			vm.ShouldDisplay = Config.AreaIdsForTankTpGaugeDisplay.Contains(vm.TpGauge);
		}
	}

	public override void Save()
	{
		Config.ShowAircraft = ShowAircraft;
		Config.SearchingAbilityMethod = SearchingAbilityMethod;
		Config.IsScrollable = IsScrollable;
		Config.FixShipNameWidth = FixShipNameWidth;
		Config.ShortenHPBar = ShortenHPBar;
		Config.ShowNextExp = ShowNextExp;
		Config.EquipmentLevelVisibility = EquipmentLevelVisibility;
		Config.ShowAircraftLevelByNumber = ShowAircraftLevelByNumber;
		Config.AirSuperiorityMethod = (int)AirSuperiorityMethod;
		Config.ShowAnchorageRepairingTimer = ShowAnchorageRepairingTimer;
		Config.BlinkAtCompletion = BlinkAtCompletion;
		Config.ShowConditionIcon = ShowConditionIcon;
		Config.FixedShipNameWidth = FixedShipNameWidth;
		Config.ShowAirSuperiorityRange = ShowAirSuperiorityRange;
		Config.ReflectAnchorageRepairHealing = ReflectAnchorageRepairHealing;
		Config.EmphasizesSubFleetInPort = EmphasizesSubFleetInPort;
		Config.BlinkAtDamaged = BlinkAtDamaged;
		Config.FleetStateDisplayMode = (int)FleetStateDisplayMode;
		Config.AppliesSallyAreaColor = AppliesSallyAreaColor;
		Config.AreaIdsForTankTpGaugeDisplay = GaugeList.Where(vm => vm.ShouldDisplay).Select(vm => vm.TpGauge).ToList();
		Config.DisplayOnlyCurrentEventTankTp = DisplayOnlyCurrentEventTankTp;
	}
}
