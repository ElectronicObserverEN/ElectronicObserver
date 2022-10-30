using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public class ConfigurationFleetTranslationViewModel : TranslationBaseViewModel
{
	public string FormFleet_ShowAircraft => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAircraft;

	public string FleetStatus => Properties.Window.Dialog.DialogConfiguration.FleetStatus;
	public string AirSuperiorityMethod => ConfigRes.AirSuperiorityMethod;

	public string FormFleet_IsScrollable => Properties.Window.Dialog.DialogConfiguration.FormFleet_IsScrollable;
	public string FormFleet_IsScrollableToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_IsScrollableToolTip;

	public string EquipmentLevelDisplay => Properties.Window.Dialog.DialogConfiguration.EquipmentLevelDisplay;
	public string FormFleet_EquipmentLevelVisibilityToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_EquipmentLevelVisibilityToolTip;

	public string FormFleet_FixShipNameWidth => Properties.Window.Dialog.DialogConfiguration.FormFleet_FixShipNameWidth;
	public string FormFleet_FixedShipNameWidthToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_FixedShipNameWidthToolTip;

	public string FormFleet_ShortenHPBar => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShortenHPBar;
	public string ShortenHPHint => ConfigRes.ShortenHPHint;

	public string FormFleet_ShowAnchorageRepairingTimer => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAnchorageRepairingTimer;
	public string FormFleet_ShowAnchorageRepairingTimerToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAnchorageRepairingTimerToolTip;

	public string FormFleet_BlinkAtDamaged => Properties.Window.Dialog.DialogConfiguration.FormFleet_BlinkAtDamaged;
	public string FormFleet_BlinkAtDamagedToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_BlinkAtDamagedToolTip;

	public string FormFleet_ShowNextExp => ConfigRes.ShowNextXP;
	public string FormFleet_ShowNextExpToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowNextExpToolTip;

	public string FormFleet_ReflectAnchorageRepairHealing => Properties.Window.Dialog.DialogConfiguration.FormFleet_ReflectAnchorageRepairHealing;
	public string FormFleet_ReflectAnchorageRepairHealingToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ReflectAnchorageRepairHealingToolTip;

	public string FormFleet_EmphasizesSubFleetInPort => Properties.Window.Dialog.DialogConfiguration.FormFleet_EmphasizesSubFleetInPort;
	public string FormFleet_EmphasizesSubFleetInPortToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_EmphasizesSubFleetInPortToolTip;

	public string FormFleet_ShowConditionIcon => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowConditionIcon;
	public string FormFleet_ShowConditionIconToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowConditionIconToolTip;

	public string FormFleet_BlinkAtCompletion => ConfigRes.FleetBlinkAtCompletion;
	public string FormFleet_BlinkAtCompletionToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_BlinkAtCompletionToolTip;

	public string FormFleet_AppliesSallyAreaColor => Properties.Window.Dialog.DialogConfiguration.FormFleet_AppliesSallyAreaColor;
	public string FormFleet_AppliesSallyAreaColorToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_AppliesSallyAreaColorToolTip;

	public string FormFleet_ShowAirSuperiorityRange => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAirSuperiorityRange;
	public string FormFleet_ShowAirSuperiorityRangeToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAirSuperiorityRangeToolTip;

	public string FormFleet_ShowAircraftLevelByNumber => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAircraftLevelByNumber;
	public string FormFleet_ShowAircraftLevelByNumberToolTip => Properties.Window.Dialog.DialogConfiguration.FormFleet_ShowAircraftLevelByNumberToolTip;
}
