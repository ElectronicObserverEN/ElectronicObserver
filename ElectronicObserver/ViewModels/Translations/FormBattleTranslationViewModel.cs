using ElectronicObserver.Window;

namespace ElectronicObserver.ViewModels.Translations;

public class FormBattleTranslationViewModel : TranslationBaseViewModel
{
	public string ABText => Properties.Window.FormBattle.ABText.Replace("_", "__").Replace("&", "_");
	public string AerialPhaseJet => Properties.Window.FormBattle.AerialPhaseJet.Replace("_", "__").Replace("&", "_");
	public string AerialPhase1 => Properties.Window.FormBattle.AerialPhase1.Replace("_", "__").Replace("&", "_");
	public string AerialPhase2 => Properties.Window.FormBattle.AerialPhase2.Replace("_", "__").Replace("&", "_");
	public string AirDefense => Properties.Window.FormBattle.AirDefense.Replace("_", "__").Replace("&", "_");
	public string Contact => Properties.Window.FormBattle.Contact.Replace("_", "__").Replace("&", "_");
	public string None => Properties.Window.FormBattle.None.Replace("_", "__").Replace("&", "_");
	public string AACI => Properties.Window.FormBattle.AACI.Replace("_", "__").Replace("&", "_");
	public string AACIType => Properties.Window.FormBattle.AACIType.Replace("_", "__").Replace("&", "_");
	public string DidNotActivate => Properties.Window.FormBattle.DidNotActivate.Replace("_", "__").Replace("&", "_");
	public string AirBase => Properties.Window.FormBattle.AirBase.Replace("_", "__").Replace("&", "_");
	public string SupportExpedition => Properties.Window.FormBattle.SupportExpedition.Replace("_", "__").Replace("&", "_");
	public string FleetFriendShort => Properties.Window.FormBattle.FleetFriendShort.Replace("_", "__").Replace("&", "_");

	public string RightClickMenu_ShowBattleDetail => GeneralRes.RightClickMenu_ShowBattleDetail.Replace("_", "__").Replace("&", "_");
	public string RightClickMenu_ShowBattleResult => GeneralRes.RightClickMenu_ShowBattleResult.Replace("_", "__").Replace("&", "_");

	public string FormationFriend => GeneralRes.FriendlyFormation.Replace("_", "__").Replace("&", "_");
	public string Formation => GeneralRes.EncounterType.Replace("_", "__").Replace("&", "_");
	public string FormationEnemy => GeneralRes.EnemyFormation.Replace("_", "__").Replace("&", "_");

	public string AirStage2Friend => GeneralRes.ShotDown.Replace("_", "__").Replace("&", "_");
	public string AACutin => GeneralRes.AntiAir.Replace("_", "__").Replace("&", "_");
	public string AirStage2Enemy => GeneralRes.ShotDown.Replace("_", "__").Replace("&", "_");

	public string AirStage1Friend => GeneralRes.ShotDown.Replace("_", "__").Replace("&", "_");
	public string AirSuperiority => GeneralRes.AirSuperiority.Replace("_", "__").Replace("&", "_");
	public string AirStage1Enemy => GeneralRes.ShotDown.Replace("_", "__").Replace("&", "_");

	public string SearchingFriend => GeneralRes.FriendlyScout.Replace("_", "__").Replace("&", "_");
	public string Searching => GeneralRes.Scouting.Replace("_", "__").Replace("&", "_");
	public string SearchingEnemy => GeneralRes.EnemyScout.Replace("_", "__").Replace("&", "_");

	public string FleetFriend => Properties.Window.FormBattle.FleetFriend.Replace("_", "__").Replace("&", "_");
	public string FleetFriendEscort => Properties.Window.FormBattle.FleetFriendEscort.Replace("_", "__").Replace("&", "_");
	public string FleetEnemyEscort => Properties.Window.FormBattle.FleetEnemyEscort.Replace("_", "__").Replace("&", "_");
	public string FleetEnemy => Properties.Window.FormBattle.FleetEnemy.Replace("_", "__").Replace("&", "_");

	public string DamageFriend => Properties.Window.FormBattle.DamageFriend.Replace("_", "__").Replace("&", "_");
	public string WinRank => Properties.Window.FormBattle.WinRank.Replace("_", "__").Replace("&", "_");
	public string DamageEnemy => Properties.Window.FormBattle.DamageEnemy.Replace("_", "__").Replace("&", "_");

	public string CompactMode => Properties.Window.FormBattle.CompactMode;

	public string Title => GeneralRes.Battle.Replace("_", "__").Replace("&", "_");
}
