using KancolleProgress.Translations;

namespace ElectronicObserver.ViewModels.Translations;

public class FormMainTranslationViewModel : TranslationBaseViewModel
{
	// winforms uses & for keyboard navigation, wpf uses _
	// if you want to use _ in wpf you need to write it as __
	public string Title => MainResources.Title.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File => MainResources.File;

	public string StripMenu_File_Record => MainResources.File_Record.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Record_Save => MainResources.File_Record_Save.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Record_Load => MainResources.File_Record_Load.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File_Layout => MainResources.File_Layout.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Load => MainResources.File_Layout_Load.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Save => MainResources.File_Layout_Save.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Open => MainResources.File_Layout_Open.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_Change => MainResources.File_Layout_Change.Replace("_", "__").Replace("&", "_");
	public string Adjustment => MainResources.Adjustment.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_LockLayout => MainResources.File_Layout_LockLayout.Replace("_", "__").Replace("&", "_");
	public string GridSplitterSize => MainResources.GridSplitterSize.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Layout_TopMost => MainResources.File_Layout_TopMost.Replace("_", "__").Replace("&", "_");
	public string LayoutLoadFailed => MainResources.LayoutLoadFailed.Replace("_", "__").Replace("&", "_");
	public string LayoutLoadFailedTitle => MainResources.LayoutLoadFailedTitle.Replace("_", "__").Replace("&", "_");
	public string WindowCaptureLoadFailed => MainResources.WindowCaptureLoadFailed;

	public string StripMenu_File_Notification => MainResources.File_Notification.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Notification_MuteAll => MainResources.File_Notification_MuteAll.Replace("_", "__").Replace("&", "_");

	public string StripMenu_File_Configuration => MainResources.File_Configuration.Replace("_", "__").Replace("&", "_");
	public string StripMenu_File_Close => MainResources.File_Close.Replace("_", "__").Replace("&", "_");

	public string StripMenu_View => MainResources.View.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet => MainResources.View_Fleet.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_1 => MainResources.View_Fleet_1.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_2 => MainResources.View_Fleet_2.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_3 => MainResources.View_Fleet_3.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Fleet_4 => MainResources.View_Fleet_4.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_FleetOverview => MainResources.View_FleetOverview.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShipGroup => MainResources.View_ShipGroup.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Dock => MainResources.View_Dock.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Arsenal => MainResources.View_Arsenal.Replace("_", "__").Replace("&", "_");
	public string EquipmentUpgradePlanViewerTitle => EquipmentUpgradePlanViewer.Title;
	public string StripMenu_View_BaseAirCorps => MainResources.View_BaseAirCorps.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Headquarters => MainResources.View_Headquarters.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Quest => MainResources.View_Quest.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Information => MainResources.View_Information.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Compass => MainResources.View_Compass.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Battle => MainResources.View_Battle.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Browser => MainResources.View_Browser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Log => MainResources.View_Log.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_Json => MainResources.View_Json.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_FleetPreset => MainResources.View_FleetPreset.Replace("_", "__").Replace("&", "_");
	public string StripMenu_View_ShipTrainingPlanViewer => ShipTrainingPlannerResources.ViewerTitle;

	public string StripMenu_WindowCapture => MainResources.WindowCapture.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_SubWindow => MainResources.WindowCapture_SubWindow.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_AttachAll => MainResources.WindowCapture_AttachAll.Replace("_", "__").Replace("&", "_");
	public string StripMenu_WindowCapture_DetachAll => MainResources.WindowCapture_DetachAll.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Tool => MainResources.Tool.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_EquipmentList => MainResources.Tool_EquipmentList.Replace("_", "__").Replace("&", "_");
	public string SortieRecordViewer => SortieRecordViewerResources.Title;
	public string StripMenu_Tool_DropRecord => MainResources.Tool_DropRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_DevelopmentRecord => MainResources.Tool_DevelopmentRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ConstructionRecord => MainResources.Tool_ConstructionRecord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ResourceChart => MainResources.Tool_ResourceChart.Replace("_", "__").Replace("&", "_");
	public string SenkaViewer => SenkaViewerResources.Title;
	public string ExpeditionRecordViewer => ExpeditionRecordViewerResources.Title;
	public string StripMenu_Tool_AlbumMasterShip => MainResources.Tool_AlbumMasterShip.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_AlbumMasterEquipment => MainResources.Tool_AlbumMasterEquipment.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_AntiAirDefense => MainResources.Tool_AntiAirDefense.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_FleetImageGenerator => MainResources.Tool_FleetImageGenerator.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_BaseAirCorpsSimulation => MainResources.Tool_BaseAirCorpsSimulation.Replace("_", "__").Replace("&", "_");
	public string OperationRoom => MainResources.OperationRoom;
	public string StripMenu_Tool_ExpChecker => MainResources.Tool_ExpChecker.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_ExpeditionCheck => MainResources.Tool_ExpeditionCheck.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_KancolleProgress => KancolleProgressResources.Title;
	public string StripMenu_Tool_ExtraBrowser => MainResources.Tool_ExtraBrowser.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Tool_QuestTrackerManager => QuestTrackerManagerResources.Title;
	public string EventLockPlannerTitle => EventLockPlannerResources.Title;
	public string EquipmentUpgradePlannerTitle => EquipmentUpgradePlannerResources.Title; 
	public string AutoRefreshTitle => AutoRefreshResources.Title;
	public string Telegram => TelegramResources.Title;

	public string StripMenu_Debug => MainResources.Debug.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadAPIFromFile => MainResources.Debug_LoadAPIFromFile.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadInitialAPI => MainResources.Debug_LoadInitialAPI.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadRecordFromOld => MainResources.Debug_LoadRecordFromOld.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_DeleteOldAPI => MainResources.Debug_DeleteOldAPI.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadBaseAPI => MainResources.Debug_LoadBaseAPI;
	public string StripMenu_Debug_RenameShipResource => MainResources.Debug_RenameShipResource.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Debug_LoadDataFromOld => MainResources.Debug_LoadDataFromOld.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Help => MainResources.Help.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Help => MainResources.Help_Help.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Issue => MainResources.Help_Issue.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Discord => MainResources.Help_Discord.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Update => MainResources.Help_Update.Replace("_", "__").Replace("&", "_");
	public string StripMenu_Help_Version => MainResources.Help_Version.Replace("_", "__").Replace("&", "_");

	public string StripMenu_Update_UpdateAvailable => MainResources.Update_UpdateAvailable;
	public string StripMenu_Update_DownloadUpdate => MainResources.Update_DownloadUpdate;
	public string StripMenu_Update_OpenReleaseNotes => MainResources.Update_OpenReleaseNotes;

	public string StripStatus_Information => MainResources.Information.Replace("_", "__").Replace("&", "_");
	public string StripStatus_Clock => MainResources.Clock.Replace("_", "__").Replace("&", "_");

	public string EventStartsIn => MainResources.EventStartsIn.Replace("_", "__").Replace("&", "_");
	public string EventHasStarted => MainResources.EventHasStarted.Replace("_", "__").Replace("&", "_");

	public string EventEndsIn => MainResources.EventEndsIn.Replace("_", "__").Replace("&", "_");
	public string EventHasEnded => MainResources.EventHasEnded.Replace("_", "__").Replace("&", "_");

	public string MaintenanceStartsIn => MainResources.MaintenanceStartsIn.Replace("_", "__").Replace("&", "_");
	public string MaintenanceHasStarted => MainResources.MaintenanceHasStarted.Replace("_", "__").Replace("&", "_");

	public string NextExerciseReset => MainResources.NextExerciseReset.Replace("_", "__").Replace("&", "_");
	public string NextQuestReset => MainResources.NextQuestReset.Replace("_", "__").Replace("&", "_");

	public string Old => MainResources.Old;
	public string New => MainResources.New;
}
