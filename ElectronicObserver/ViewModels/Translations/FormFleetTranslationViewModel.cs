using ElectronicObserver.Data;
using ElectronicObserver.Properties.Window;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations;

public class FormFleetTranslationViewModel : TranslationBaseViewModel
{
	public string CopyFleetText => Properties.Window.FormFleet.CopyFleetText;
	public string ShipNameToolTip => Properties.Window.FormFleet.ShipNameToolTip;
	public string RightClickToOpenEncyclopedia => Properties.Window.FormFleet.RightClickToOpenEncyclopedia;
	public string ExpCalcHint => Properties.Window.FormFleet.ExpCalcHint;

	public string CvciFba => Properties.Window.FormFleet.CvciFba;
	public string CvciBba => Properties.Window.FormFleet.CvciBba;
	public string CvciBa => Properties.Window.FormFleet.CvciBa;

	public string Power => Properties.Window.FormFleet.Power;
	public string Accuracy => Properties.Window.FormFleet.Accuracy;

	public string Asw => Properties.Window.FormFleet.Asw;
	public string OpeningAsw => Properties.Window.FormFleet.OpeningAsw;
	public string Aarb => Properties.Window.FormFleet.Aarb;
	public string AirstrikePower => Properties.Window.FormFleet.AirstrikePower;

	public string NoShips => FleetRes.NoShips;
	public string CriticalDamageAdvance => FleetRes.CriticalDamageAdvance;
	public string OnSortie => FleetRes.OnSortie;
	public string ExpeditionToolTip => FleetRes.ExpeditionToolTip;
	public string CriticallyDamagedShip => FleetRes.CriticallyDamagedShip;
	public string Repairing => FleetRes.Repairing;
	public string RepairTimeHeader => FleetRes.RepairTimeHeader;
	public string RepairTimeDetail => FleetRes.RepairTimeDetail;
	public string OnDock => FleetRes.OnDock;
	public string DockCompletionTime => FleetRes.DockCompletionTime;
	public string SupplyNeeded => FleetRes.SupplyNeeded;
	public string ResupplyTooltip => FleetRes.ResupplyTooltip;
	public string RecoveryTimeToolTip => FleetRes.RecoveryTimeToolTip;
	public string FightingSpiritHigh => FleetRes.FightingSpiritHigh;
	public string SparkledTooltip => FleetRes.SparkledTooltip;
	public string ReadyToSortie => FleetRes.ReadyToSortie;
	public string Fatigued => FleetRes.Fatigued;

	public string SupportTypeNone => Properties.Window.FormFleet.SupportTypeNone;
	public string SupportTypeAerial => Properties.Window.FormFleet.SupportTypeAerial;
	public string SupportTypeShelling => Properties.Window.FormFleet.SupportTypeShelling;
	public string SupportTypeTorpedo => Properties.Window.FormFleet.SupportTypeTorpedo;

	public string FleetNameToolTip => Properties.Window.FormFleet.FleetNameToolTip;
	public string WithoutProficiency => Properties.Window.FormFleet.WithoutProficiency;
	public string WithProficiency => Properties.Window.FormFleet.WithProficiency;
	public string FleetLosToolTip => Properties.Window.FormFleet.FleetLosToolTip;
	public string ContactSelection => Properties.Window.FormFleet.ContactSelection;
	public string ContactProbability => Properties.Window.FormFleet.ContactProbability;
	public string ResourceToolTip => FleetRes.ResourceToolTip;

	public string ContextMenuFleet_CopyFleet => Properties.Window.FormFleet.ContextMenuFleet_CopyFleet.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetDeckBuilder => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetDeckBuilder.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyKanmusuList => Properties.Window.FormFleet.ContextMenuFleet_CopyKanmusuList.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysis => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysis.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysisLockedEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisLockedEquip.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_CopyFleetAnalysisAllEquip => Properties.Window.FormFleet.ContextMenuFleet_CopyFleetAnalysisAllEquip.Replace("_", "__").Replace("&", "_");
	
	public string ContextMenuFleetShipLevel_OpenExpChecker => Properties.Window.FormFleet.ContextMenuFleetShipLevel_OpenExpChecker;
	public string ContextMenuFleetShipLevel_CreateTrainingPlan => Properties.Window.FormFleet.ContextMenuFleetShipLevel_CreateTrainingPlan;
	public string ContextMenuFleetShipLevel_EditTrainingPlan => Properties.Window.FormFleet.ContextMenuFleetShipLevel_EditTrainingPlan;
	public string ContextMenuFleetShipLevel_RemoveTrainingPlan => Properties.Window.FormFleet.ContextMenuFleetShipLevel_RemoveTrainingPlan;

	public string CopyToFleetAnalysisSpreadsheetShips => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetShips;
	public string CopyToFleetAnalysisSpreadsheetLockedEquipment => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetLockedEquipment;
	public string CopyToFleetAnalysisSpreadsheetAllEquipment => Properties.Window.FormFleet.CopyToFleetAnalysisSpreadsheetAllEquipment;

	public string ContextMenuFleet_AntiAirDetails => Properties.Window.FormFleet.ContextMenuFleet_AntiAirDetails.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_Capture => Properties.Window.FormFleet.ContextMenuFleet_Capture.Replace("_", "__").Replace("&", "_");
	public string ContextMenuFleet_OutputFleetImage => Properties.Window.FormFleet.ContextMenuFleet_OutputFleetImage.Replace("_", "__").Replace("&", "_");
	public string AirControlSimulator => Window.Tools.AirControlSimulator.AirControlSimulator.Title;

	public string HP => DialogAlbumMasterShip.TitleHP;
	public string Firepower => DialogAlbumMasterShip.Firepower;
	public string Torpedo => DialogAlbumMasterShip.Torpedo;
	public string AA => DialogAlbumMasterShip.AA;
	public string Armor => DialogAlbumMasterShip.Armor;
	public string ASW => DialogAlbumMasterShip.ASW;
	public string Evasion => DialogAlbumMasterShip.Evasion;
	public string Interception => DialogAlbumMasterShip.Interception;
	public string LOS => DialogAlbumMasterShip.LOS;
	public string AntiBomb => DialogAlbumMasterShip.AntiBomb;
	public string Luck => DialogAlbumMasterShip.Luck;
	public string Bombing => DialogAlbumMasterShip.Bombing;
	public string Range => GeneralRes.Range;
	public string Speed => GeneralRes.Speed;

	public string SpecialAttacksDay => FormFleet.SpecialAttacksDay;
	public string SpecialAttacksNight => FormFleet.SpecialAttacksNight;
}
