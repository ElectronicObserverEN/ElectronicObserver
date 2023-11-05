using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.AirControlSimulator;

public class AirControlSimulatorTranslationViewModel : TranslationBaseViewModel
{
	public string Title => AirControlSimulatorResources.DataSelection;

	public string Fleet => AirControlSimulatorResources.Fleet;
	public string AirBase => AirControlSimulatorResources.AirBase;
	public string Data => AirControlSimulatorResources.Data;
	public string Browser => AirControlSimulatorResources.Browser;

	public string None => AirControlSimulatorResources.None;
	public string MaxProficiency => AirControlSimulatorResources.MaxProficiency;

	public string Ships => AirControlSimulatorResources.Ships;
	public string Equipment => AirControlSimulatorResources.Equipment;
	public string IncludeUnlocked => AirControlSimulatorResources.IncludeUnlocked;

	public string CopyLink => SortieRecordViewerResources.CopyLink;
	public string Ok => AirControlSimulatorResources.Ok;
	public string Cancel => AirControlSimulatorResources.Cancel;
}
