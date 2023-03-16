using ElectronicObserver.Data;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

public class SortieDetailTranslationViewModel : TranslationBaseViewModel
{
	public string Title => "出撃詳細";

	public string BattleDetail_FriendFleet => ConstantsRes.BattleDetail_FriendFleet;
	public string BattleDetail_FriendMainFleet => ConstantsRes.BattleDetail_FriendMainFleet;
	public string BattleDetail_FriendEscortFleet => ConstantsRes.BattleDetail_FriendEscortFleet;

	public string BattleDetail_EnemyFleet => ConstantsRes.BattleDetail_EnemyFleet;
	public string BattleDetail_EnemyMainFleet => ConstantsRes.BattleDetail_EnemyMainFleet;
	public string BattleDetail_EnemyEscortFleet => ConstantsRes.BattleDetail_EnemyEscortFleet;

	public string BattleDetail_BattleEnd => ConstantsRes.BattleDetail_BattleEnd;
}
