using ElectronicObserver.Properties.Window.Dialog;

namespace ElectronicObserver.ViewModels.Translations;

public class DialogDropRecordViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => DialogDropRecordViewer.Title;

	public string LabelShipName => DialogDropRecordViewer.LabelShipName;
	public string ShipNameToolTip => DialogDropRecordViewer.ShipNameToolTip;
	public string LabelItemName => DialogDropRecordViewer.LabelItemName;
	public string ItemNameToolTip => DialogDropRecordViewer.ItemNameToolTip;
	public string LabelEquipmentName => DialogDropRecordViewer.LabelEquipmentName;
	public string EquipmentNameToolTip => DialogDropRecordViewer.EquipmentNameToolTip;

	public string Start => DialogDropRecordViewer.Start;
	public string End => DialogDropRecordViewer.End;
	public string DateBeginToolTip => DialogDropRecordViewer.DateBeginToolTip;
	public string DateEndToolTip => DialogDropRecordViewer.DateEndToolTip;

	public string RankSToolTip => DialogDropRecordViewer.RankSToolTip;
	public string RankAToolTip => DialogDropRecordViewer.RankAToolTip;
	public string RankBToolTip => DialogDropRecordViewer.RankBToolTip;
	public string RankXToolTip => DialogDropRecordViewer.RankXToolTip;

	public string NodeSelection => DialogDropRecordViewer.NodeSelection;
	public string MapDifficultyToolTip => DialogDropRecordViewer.MapDifficultyToolTip;
	public string IsBossOnly => DialogDropRecordViewer.IsBossOnly;

	public string MapAreaIDToolTip => DialogDropRecordViewer.MapAreaIDToolTip;
	public string MapInfoIDToolTip => DialogDropRecordViewer.MapInfoIDToolTip;
	public string MapCellIDToolTip => DialogDropRecordViewer.MapCellIDToolTip;

	public string MergeRows => DialogDropRecordViewer.MergeRows;
	public string MergeRowsToolTip => DialogDropRecordViewer.MergeRowsToolTip;
	public string ButtonRun => DialogDropRecordViewer.ButtonRun;

	public string RecordView_Name => DialogDropRecordViewer.RecordView_Name;
	public string RecordView_Date => DialogDropRecordViewer.RecordView_Date;
	public string RecordView_Map => DialogDropRecordViewer.RecordView_Map;
	public string RecordView_Rank => DialogDropRecordViewer.RecordView_Rank;
	public string Tries => Window.Dialog.EncycloRes.Tries;
	public string RecordView_RankS => DialogDropRecordViewer.RecordView_RankS;
	public string RecordView_RankA => DialogDropRecordViewer.RecordView_RankA;
	public string RecordView_RankB => DialogDropRecordViewer.RecordView_RankB;

	public string NameAny => DialogDropRecordViewer.NameAny;
	public string NameNotExist => DialogDropRecordViewer.NameNotExist;
	public string NameFullPort => DialogDropRecordViewer.NameFullPort;
	public string NameExist => DialogDropRecordViewer.NameExist;

	public string Boss => DialogDropRecordViewer.Boss;

	public string SelectedItems => DialogDropRecordViewer.SelectedItems;

	public string BattleHistoryNotFound => DialogDropRecordViewer.BattleHistoryNotFound;
	public string ReplayFileNotFound =>"Replay File not found";
	public string SearchingBattleHistory => DialogDropRecordViewer.SearchingBattleHistory;
	public string OpenBattleHistory => DialogDropRecordViewer.OpenBattleHistory;
	public string OpenBattleLog => "Open Battle Log";
	public string OpenReplayFile => "Open Replay File";
	public string CopyReplayURL => "Copy Replay URL";
	public string CopiedReplayURL => "Replay URL Copied: {0}";
	public string CouldNotOpenBattleHistory => DialogDropRecordViewer.CouldNotOpenBattleHistory;
	public string CouldNotOpenReplayFile => "Could not open Replay File";
	public string Today => DialogDropRecordViewer.Today;
}
