using ElectronicObserver.Data;

namespace ElectronicObserver.ViewModels.Translations;

public class FormFleetTranslationViewModel : TranslationBaseViewModel
{
	public string CopyFleetText => Properties.Window.FormFleet.CopyFleetText.Replace("_", "__").Replace("&", "_");
	public string ShipNameToolTip => Properties.Window.FormFleet.ShipNameToolTip.Replace("_", "__").Replace("&", "_");
	public string RightClickToOpenEncyclopedia => Properties.Window.FormFleet.RightClickToOpenEncyclopedia;
	public string ExpCalcHint => Properties.Window.FormFleet.ExpCalcHint.Replace("_", "__").Replace("&", "_");

	public string CvciFba => Properties.Window.FormFleet.CvciFba.Replace("_", "__").Replace("&", "_");
	public string CvciBba => Properties.Window.FormFleet.CvciBba.Replace("_", "__").Replace("&", "_");
	public string CvciBa => Properties.Window.FormFleet.CvciBa.Replace("_", "__").Replace("&", "_");

	public string CvnciFfa => Properties.Window.FormFleet.CvnciFfa.Replace("_", "__").Replace("&", "_");
	public string CvnciFa => Properties.Window.FormFleet.CvnciFa.Replace("_", "__").Replace("&", "_");
	public string CvnciPhoto => Properties.Window.FormFleet.CvnciPhoto.Replace("_", "__").Replace("&", "_");
	public string CvnciFoo => Properties.Window.FormFleet.CvnciFoo.Replace("_", "__").Replace("&", "_");

	public string LateModelTorpedoSubmarineEquipment => Properties.Window.FormFleet.LateModelTorpedoSubmarineEquipment.Replace("_", "__").Replace("&", "_");
	public string LateModelTorpedo2 => Properties.Window.FormFleet.LateModelTorpedo2.Replace("_", "__").Replace("&", "_");

	public string Power => Properties.Window.FormFleet.Power.Replace("_", "__").Replace("&", "_");
	public string Accuracy => Properties.Window.FormFleet.Accuracy.Replace("_", "__").Replace("&", "_");

	public string Asw => Properties.Window.FormFleet.Asw.Replace("_", "__").Replace("&", "_");
	public string OpeningAsw => Properties.Window.FormFleet.OpeningAsw.Replace("_", "__").Replace("&", "_");
	public string Aarb => Properties.Window.FormFleet.Aarb.Replace("_", "__").Replace("&", "_");
	public string AirstrikePower => Properties.Window.FormFleet.AirstrikePower.Replace("_", "__").Replace("&", "_");

	public string NoShips => FleetRes.NoShips.Replace("_", "__").Replace("&", "_");
	public string CriticalDamageAdvance => FleetRes.CriticalDamageAdvance.Replace("_", "__").Replace("&", "_");
	public string OnSortie => FleetRes.OnSortie.Replace("_", "__").Replace("&", "_");
	public string ExpeditionToolTip => FleetRes.ExpeditionToolTip.Replace("_", "__").Replace("&", "_");
	public string CriticallyDamagedShip => FleetRes.CriticallyDamagedShip.Replace("_", "__").Replace("&", "_");
	public string Repairing => FleetRes.Repairing.Replace("_", "__").Replace("&", "_");
	public string RepairTimeHeader => FleetRes.RepairTimeHeader.Replace("_", "__").Replace("&", "_");
	public string RepairTimeDetail => FleetRes.RepairTimeDetail.Replace("_", "__").Replace("&", "_");
	public string OnDock => FleetRes.OnDock.Replace("_", "__").Replace("&", "_");
	public string DockCompletionTime => FleetRes.DockCompletionTime.Replace("_", "__").Replace("&", "_");
	public string SupplyNeeded => FleetRes.SupplyNeeded.Replace("_", "__").Replace("&", "_");
	public string ResupplyTooltip => FleetRes.ResupplyTooltip.Replace("_", "__").Replace("&", "_");
	public string RecoveryTimeToolTip => FleetRes.RecoveryTimeToolTip.Replace("_", "__").Replace("&", "_");
	public string FightingSpiritHigh => FleetRes.FightingSpiritHigh.Replace("_", "__").Replace("&", "_");
	public string SparkledTooltip => FleetRes.SparkledTooltip.Replace("_", "__").Replace("&", "_");
	public string ReadyToSortie => FleetRes.ReadyToSortie.Replace("_", "__").Replace("&", "_");
	public string Fatigued => FleetRes.Fatigued.Replace("_", "__").Replace("&", "_");

	public string SupportTypeNone => Properties.Window.FormFleet.SupportTypeNone.Replace("_", "__").Replace("&", "_");
	public string SupportTypeAerial => Properties.Window.FormFleet.SupportTypeAerial.Replace("_", "__").Replace("&", "_");
	public string SupportTypeShelling => Properties.Window.FormFleet.SupportTypeShelling.Replace("_", "__").Replace("&", "_");
	public string SupportTypeTorpedo => Properties.Window.FormFleet.SupportTypeTorpedo.Replace("_", "__").Replace("&", "_");

	public string FleetNameToolTip => Properties.Window.FormFleet.FleetNameToolTip.Replace("_", "__").Replace("&", "_");
	public string WithoutProficiency => Properties.Window.FormFleet.WithoutProficiency.Replace("_", "__").Replace("&", "_");
	public string WithProficiency => Properties.Window.FormFleet.WithProficiency.Replace("_", "__").Replace("&", "_");
	public string FleetLosToolTip => Properties.Window.FormFleet.FleetLosToolTip.Replace("_", "__").Replace("&", "_");
	public string ContactSelection => Properties.Window.FormFleet.ContactSelection.Replace("_", "__").Replace("&", "_");
	public string ContactProbability => Properties.Window.FormFleet.ContactProbability.Replace("_", "__").Replace("&", "_");
	public string ResourceToolTip => FleetRes.ResourceToolTip.Replace("_", "__").Replace("&", "_");

	public string ContextMenuFleet_CopyFleet => Properties.Window.FormFleet.ContextMenuFleet_CopyFleet.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetDeckBuilder => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetDeckBuilder.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyKanmusuList => Properties.Window.FormFleet.ContextMenuFleet_CopyKanmusuList.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysis => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysis.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysisLockedEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisLockedEquip.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysisAllEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisAllEquip.Replace("_", "__").Replace("&", "_");

	public string CopyToFleetAnalysisSpreadsheetShips => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetShips;
	public string CopyToFleetAnalysisSpreadsheetLockedEquipment => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetLockedEquipment;
	public string CopyToFleetAnalysisSpreadsheetAllEquipment => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetAllEquipment;

	public string ContextMenuFleet_AntiAirDetails => Properties.Window.FormFleet.ContextMenuFleet_AntiAirDetails.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_Capture => Properties.Window.FormFleet.ContextMenuFleet_Capture.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_OutputFleetImage => Properties.Window.FormFleet.ContextMenuFleet_OutputFleetImage.Replace("_", "__").Replace("&", "_");
	public string AirControlSimulator => Window.Tools.AirControlSimulator.AirControlSimulator.Title;
}
