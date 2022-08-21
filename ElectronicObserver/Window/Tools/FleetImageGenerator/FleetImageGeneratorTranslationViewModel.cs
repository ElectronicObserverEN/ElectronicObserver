using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class FleetImageGeneratorTranslationViewModel : TranslationBaseViewModel
{
	public string Title => Properties.Window.Dialog.DialogFleetImageGenerator.Title;

	public string CopyToClipboard => Properties.Window.Dialog.DialogFleetImageGenerator.CopyToClipboard;
	public string LoadFromClipboard => Properties.Window.Dialog.DialogFleetImageGenerator.LoadFromClipboard;

	public string AirBase => Properties.Window.FormBaseAirCorps.Title;
	public string Settings => Window.Dialog.ConfigRes.Settings;

	public string HighAltitudeShort => Properties.Window.Dialog.DialogFleetImageGenerator.HighAltitudeShort;

	public string Basic => Properties.Window.Dialog.DialogFleetImageGenerator.Basic;
	public string GroupOutputPath => Properties.Window.Dialog.DialogFleetImageGenerator.GroupOutputPath;
	public string SearchOutputPath => Properties.Window.Dialog.DialogFleetImageGenerator.SearchOutputPath;
	public string SearchOutputPathToolTip => Properties.Window.Dialog.DialogFleetImageGenerator.SearchOutputPathToolTip;
	public string OutputPathToolTip => Properties.Window.Dialog.DialogFleetImageGenerator.OutputPathToolTip;
	public string CustomText => Properties.Window.Dialog.DialogFleetImageGenerator.CustomText;
	public string FleetTitle => Properties.Window.Dialog.DialogFleetImageGenerator.FleetTitle;
	public string Comment => Properties.Window.Dialog.DialogFleetImageGenerator.Comment;
	public string Mode => Properties.Window.Dialog.DialogFleetImageGenerator.Mode;
	public string ImageTypeBanner => Properties.Window.Dialog.DialogFleetImageGenerator.ImageTypeBanner;
	public string ImageTypeCutin => Properties.Window.Dialog.DialogFleetImageGenerator.ImageTypeCutin;
	public string ImageTypeCard => Properties.Window.Dialog.DialogFleetImageGenerator.ImageTypeCard;
	public string Fleet => Properties.Window.Dialog.DialogFleetImageGenerator.Fleet;

	public string Font => Properties.Window.Dialog.DialogFleetImageGenerator.Font;
	public string ButtonClearFont => Properties.Window.Dialog.DialogFleetImageGenerator.ButtonClearFont;
	public string ApplyGeneralFontToolTip => Properties.Window.Dialog.DialogFleetImageGenerator.ApplyGeneralFontToolTip;
	public string DigitSmall => Properties.Window.Dialog.DialogFleetImageGenerator.DigitSmall;
	public string DigitMedium => Properties.Window.Dialog.DialogFleetImageGenerator.DigitMedium;
	public string FontSmall => Properties.Window.Dialog.DialogFleetImageGenerator.FontSmall;
	public string FontMedium => Properties.Window.Dialog.DialogFleetImageGenerator.FontMedium;
	public string FontLarge => Properties.Window.Dialog.DialogFleetImageGenerator.FontLarge;
	public string FontTitle => Properties.Window.Dialog.DialogFleetImageGenerator.FleetTitle;
	public string ChangeAll => Properties.Window.Dialog.DialogFleetImageGenerator.ChangeAll;
	public string OpenImageDialog => Properties.Window.Dialog.DialogFleetImageGenerator.OpenImageDialog;
	public string SaveImageDialog => Properties.Window.Dialog.DialogFleetImageGenerator.SaveImageDialog;
	
	public string UseAlbumStatusName => Properties.Window.Dialog.DialogFleetImageGenerator.UseAlbumStatusName;
	public string MaxEquipmentNameWidth => Properties.Window.Dialog.DialogFleetImageGenerator.MaxEquipmentNameWidth;
}
