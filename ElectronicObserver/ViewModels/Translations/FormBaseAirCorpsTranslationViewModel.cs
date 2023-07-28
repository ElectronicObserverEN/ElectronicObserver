namespace ElectronicObserver.ViewModels.Translations;

public class FormBaseAirCorpsTranslationViewModel : TranslationBaseViewModel
{
	public string Area => BaseAirCorpsResources.Area.Replace("_", "__").Replace("&", "_");
	public string UnknownArea => BaseAirCorpsResources.UnknownArea.Replace("_", "__").Replace("&", "_");
	public string Unsupplied => BaseAirCorpsResources.Unsupplied.Replace("_", "__").Replace("&", "_");
	public string AirControlSummary => BaseAirCorpsResources.AirControlSummary.Replace("_", "__").Replace("&", "_");
	public string TotalDistance => BaseAirCorpsResources.TotalDistance.Replace("_", "__").Replace("&", "_");
	public string Empty => BaseAirCorpsResources.Empty.Replace("_", "__").Replace("&", "_");
	public string Range => BaseAirCorpsResources.Range.Replace("_", "__").Replace("&", "_");
	public string CopyOrganizationFormat => BaseAirCorpsResources.CopyOrganizationFormat.Replace("_", "__").Replace("&", "_");

	public string ContextMenuBaseAirCorps_CopyOrganization => BaseAirCorpsResources.CopyOrganization.Replace("_", "__").Replace("&", "_");
	public string ContextMenuBaseAirCorps_DisplayRelocatedEquipments => BaseAirCorpsResources.DisplayRelocatedEquipments.Replace("_", "__").Replace("&", "_");

	public string Title => BaseAirCorpsResources.Title.Replace("_", "__").Replace("&", "_");
}
