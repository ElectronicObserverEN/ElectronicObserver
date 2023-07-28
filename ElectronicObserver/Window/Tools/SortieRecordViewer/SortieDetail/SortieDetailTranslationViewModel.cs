using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

public class SortieDetailTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SortieRecordViewerResources.SortieDetail;

	public string File => MainResources.File;
	public string CopySortieData => SortieRecordViewerResources.CopySortieData;
	public string LoadSortieData => SortieRecordViewerResources.LoadSortieData;
	public string AirControlSimulator => AirControlSimulatorResources.Title;
	public string CopyLink => SortieRecordViewerResources.CopyLink;
	public string Open => SortieRecordViewerResources.Open;

	public string BattleDetail_FriendFleet => ConstantsRes.BattleDetail_FriendFleet;
	public string BattleDetail_FriendMainFleet => ConstantsRes.BattleDetail_FriendMainFleet;
	public string BattleDetail_FriendEscortFleet => ConstantsRes.BattleDetail_FriendEscortFleet;

	public string BattleDetail_EnemyFleet => ConstantsRes.BattleDetail_EnemyFleet;
	public string BattleDetail_EnemyMainFleet => ConstantsRes.BattleDetail_EnemyMainFleet;
	public string BattleDetail_EnemyEscortFleet => ConstantsRes.BattleDetail_EnemyEscortFleet;

	public string BattleDetail_BattleEnd => ConstantsRes.BattleDetail_BattleEnd;
}
