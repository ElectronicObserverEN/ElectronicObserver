using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

namespace ElectronicObserver.ViewModels.Translations;

public class FormMainTranslationViewModel : TranslationBaseViewModel
{
	// winforms uses & for keyboard navigation, wpf uses _
	// if you want to use _ in wpf you need to write it as __
	public string Title => Properties.Window.FormMain.Title.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File => Properties.Window.FormMain.File.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File_Record => Properties.Window.FormMain.File_Record.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Record_Save => Properties.Window.FormMain.File_Record_Save.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Record_Load => Properties.Window.FormMain.File_Record_Load.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File_Layout => Properties.Window.FormMain.File_Layout.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Load => Properties.Window.FormMain.File_Layout_Load.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Save => Properties.Window.FormMain.File_Layout_Save.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Open => Properties.Window.FormMain.File_Layout_Open.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Change => Properties.Window.FormMain.File_Layout_Change.Replace("_", "__").Replace("&", "_");
	public string Adjustment => Properties.Window.FormMain.Adjustment.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_LockLayout => Properties.Window.FormMain.File_Layout_LockLayout.Replace("_", "__").Replace("&", "_");
	public string GridSplitterSize => Properties.Window.FormMain.GridSplitterSize.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_TopMost => Properties.Window.FormMain.File_Layout_TopMost.Replace("_", "__").Replace("&", "_");
	public string LayoutLoadFailed => Properties.Window.FormMain.LayoutLoadFailed.Replace("_", "__").Replace("&", "_");
	public string LayoutLoadFailedTitle => Properties.Window.FormMain.LayoutLoadFailedTitle.Replace("_", "__").Replace("&", "_");
	public string WindowCaptureLoadFailed => Properties.Window.FormMain.WindowCaptureLoadFailed;

	public string StripMenu_File_Notification => Properties.Window.FormMain.File_Notification.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Notification_MuteAll => Properties.Window.FormMain.File_Notification_MuteAll.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File_Configuration => Properties.Window.FormMain.File_Configuration.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Close => Properties.Window.FormMain.File_Close.Replace("_", "__").Replace("&", "_");

	public string StripMenu_View => Properties.Window.FormMain.View.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet => Properties.Window.FormMain.View_Fleet.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_1 => Properties.Window.FormMain.View_Fleet_1.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_2 => Properties.Window.FormMain.View_Fleet_2.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_3 => Properties.Window.FormMain.View_Fleet_3.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_4 => Properties.Window.FormMain.View_Fleet_4.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_FleetOverview => Properties.Window.FormMain.View_FleetOverview.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShipGroup => Properties.Window.FormMain.View_ShipGroup.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Dock => Properties.Window.FormMain.View_Dock.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Arsenal => Properties.Window.FormMain.View_Arsenal.Replace("_", "__").Replace("&", "_");
	public string EquipmentUpgradePlanViewerTitle => EquipmentUpgradePlanViewer.Title;
	public string StripMenu_View_BaseAirCorps => Properties.Window.FormMain.View_BaseAirCorps.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Headquarters => Properties.Window.FormMain.View_Headquarters.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Quest => Properties.Window.FormMain.View_Quest.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Information => Properties.Window.FormMain.View_Information.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Compass => Properties.Window.FormMain.View_Compass.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Battle => Properties.Window.FormMain.View_Battle.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Browser => Properties.Window.FormMain.View_Browser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Log => Properties.Window.FormMain.View_Log.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Json => Properties.Window.FormMain.View_Json.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_FleetPreset => Properties.Window.FormMain.View_FleetPreset.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShipTrainingPlanViewer => ShipTrainingPlanner.ViewerTitle;

	public string StripMenu_WindowCapture => Properties.Window.FormMain.WindowCapture.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_SubWindow => Properties.Window.FormMain.WindowCapture_SubWindow.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_AttachAll => Properties.Window.FormMain.WindowCapture_AttachAll.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_DetachAll => Properties.Window.FormMain.WindowCapture_DetachAll.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Tool => Properties.Window.FormMain.Tool.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_EquipmentList => Properties.Window.FormMain.Tool_EquipmentList.Replace("_", "__").Replace("&", "_");
	public string SortieRecordViewer => Window.Tools.SortieRecordViewer.SortieRecordViewer.Title;
	public string StripMenu_Tool_DropRecord => Properties.Window.FormMain.Tool_DropRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_DevelopmentRecord => Properties.Window.FormMain.Tool_DevelopmentRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ConstructionRecord => Properties.Window.FormMain.Tool_ConstructionRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ResourceChart => Properties.Window.FormMain.Tool_ResourceChart.Replace("_", "__").Replace("&", "_");
	public string SenkaViewer => Window.Tools.SenkaViewer.SenkaViewer.Title;
	public string StripMenu_Tool_AlbumMasterShip => Properties.Window.FormMain.Tool_AlbumMasterShip.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_AlbumMasterEquipment => Properties.Window.FormMain.Tool_AlbumMasterEquipment.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_AntiAirDefense => Properties.Window.FormMain.Tool_AntiAirDefense.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_FleetImageGenerator => Properties.Window.FormMain.Tool_FleetImageGenerator.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_BaseAirCorpsSimulation => Properties.Window.FormMain.Tool_BaseAirCorpsSimulation.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ExpChecker => Properties.Window.FormMain.Tool_ExpChecker.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ExpeditionCheck => Properties.Window.FormMain.Tool_ExpeditionCheck.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_KancolleProgress => Properties.Window.FormMain.Tool_KancolleProgress.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ExtraBrowser => Properties.Window.FormMain.Tool_ExtraBrowser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_QuestTrackerManager => QuestTrackerManager.Title;
	public string EventLockPlannerTitle => EventLockPlanner.Title;
	public string EquipmentUpgradePlannerTitle => EquipmentUpgradePlanner.Title; 
	public string AutoRefreshTitle => AutoRefresh.Title;

	public string StripMenu_Debug => Properties.Window.FormMain.Debug.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadAPIFromFile => Properties.Window.FormMain.Debug_LoadAPIFromFile.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadInitialAPI => Properties.Window.FormMain.Debug_LoadInitialAPI.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadRecordFromOld => Properties.Window.FormMain.Debug_LoadRecordFromOld.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_DeleteOldAPI => Properties.Window.FormMain.Debug_DeleteOldAPI.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadBaseAPI => Properties.Window.FormMain.Debug_LoadBaseAPI;
	public string StripMenu_Debug_RenameShipResource => Properties.Window.FormMain.Debug_RenameShipResource.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadDataFromOld => Properties.Window.FormMain.Debug_LoadDataFromOld.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Help => Properties.Window.FormMain.Help.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Help => Properties.Window.FormMain.Help_Help.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Issue => Properties.Window.FormMain.Help_Issue.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Discord => Properties.Window.FormMain.Help_Discord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Update => Properties.Window.FormMain.Help_Update.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Version => Properties.Window.FormMain.Help_Version.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Update_UpdateAvailable => Properties.Window.FormMain.Update_UpdateAvailable;
	public string StripMenu_Update_DownloadUpdate => Properties.Window.FormMain.Update_DownloadUpdate;
	public string StripMenu_Update_OpenReleaseNotes => Properties.Window.FormMain.Update_OpenReleaseNotes;

	public string StripStatus_Information => Properties.Window.FormMain.Information.Replace("_", "__").Replace("&", "_");
	public string StripStatus_Clock => Properties.Window.FormMain.Clock.Replace("_", "__").Replace("&", "_");

	public string EventStartsIn => Properties.Window.FormMain.EventStartsIn.Replace("_", "__").Replace("&", "_");
	public string EventHasStarted => Properties.Window.FormMain.EventHasStarted.Replace("_", "__").Replace("&", "_");

	public string EventEndsIn => Properties.Window.FormMain.EventEndsIn.Replace("_", "__").Replace("&", "_");
	public string EventHasEnded => Properties.Window.FormMain.EventHasEnded.Replace("_", "__").Replace("&", "_");

	public string MaintenanceStartsIn => Properties.Window.FormMain.MaintenanceStartsIn.Replace("_", "__").Replace("&", "_");
	public string MaintenanceHasStarted => Properties.Window.FormMain.MaintenanceHasStarted.Replace("_", "__").Replace("&", "_");

	public string NextExerciseReset => Properties.Window.FormMain.NextExerciseReset.Replace("_", "__").Replace("&", "_");
	public string NextQuestReset => Properties.Window.FormMain.NextQuestReset.Replace("_", "__").Replace("&", "_");

	public string Old => Properties.Window.FormMain.Old;
	public string New => Properties.Window.FormMain.New;
}
