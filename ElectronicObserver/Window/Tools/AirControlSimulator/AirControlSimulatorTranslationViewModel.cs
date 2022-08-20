using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.AirControlSimulator;

public class AirControlSimulatorTranslationViewModel : TranslationBaseViewModel
{
	public string Title => AirControlSimulator.DataSelection;

	public string Fleet => AirControlSimulator.Fleet;
	public string AirBase => AirControlSimulator.AirBase;
	public string Data => AirControlSimulator.Data;
	public string Browser => AirControlSimulator.Browser;

	public string None => AirControlSimulator.None;
	public string MaxProficiency => AirControlSimulator.MaxProficiency;

	public string Ships => AirControlSimulator.Ships;
	public string Equipment => AirControlSimulator.Equipment;
	public string AllEquipment => AirControlSimulator.AllEquipment;
	public string LockedEquipment => AirControlSimulator.LockedEquipment;

	public string Ok => AirControlSimulator.Ok;
	public string Cancel => AirControlSimulator.Cancel;
}
