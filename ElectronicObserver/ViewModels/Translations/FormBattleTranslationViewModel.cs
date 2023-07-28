namespace ElectronicObserver.ViewModels.Translations;

public class FormBattleTranslationViewModel : TranslationBaseViewModel
{
	public string ABText => BattleResources.ABText.Replace("_", "__").Replace("&", "_");
	public string AerialPhaseJet => BattleResources.AerialPhaseJet.Replace("_", "__").Replace("&", "_");
	public string AerialPhase1 => BattleResources.AerialPhase1.Replace("_", "__").Replace("&", "_");
	public string AerialPhase2 => BattleResources.AerialPhase2.Replace("_", "__").Replace("&", "_");
	public string AirDefense => BattleResources.AirDefense.Replace("_", "__").Replace("&", "_");
	public string Contact => BattleResources.Contact.Replace("_", "__").Replace("&", "_");
	public string None => BattleResources.None.Replace("_", "__").Replace("&", "_");
	public string AACI => BattleResources.AACI.Replace("_", "__").Replace("&", "_");
	public string AACIType => BattleResources.AACIType.Replace("_", "__").Replace("&", "_");
	public string DidNotActivate => BattleResources.DidNotActivate.Replace("_", "__").Replace("&", "_");
	public string AirBase => BattleResources.AirBase.Replace("_", "__").Replace("&", "_");
	public string SupportExpedition => BattleResources.SupportExpedition.Replace("_", "__").Replace("&", "_");
	public string FleetFriendShort => BattleResources.FleetFriendShort.Replace("_", "__").Replace("&", "_");

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

	public string FleetFriend => BattleResources.FleetFriend.Replace("_", "__").Replace("&", "_");
	public string FleetFriendEscort => BattleResources.FleetFriendEscort.Replace("_", "__").Replace("&", "_");
	public string FleetEnemyEscort => BattleResources.FleetEnemyEscort.Replace("_", "__").Replace("&", "_");
	public string FleetEnemy => BattleResources.FleetEnemy.Replace("_", "__").Replace("&", "_");

	public string DamageFriend => BattleResources.DamageFriend.Replace("_", "__").Replace("&", "_");
	public string WinRank => BattleResources.WinRank.Replace("_", "__").Replace("&", "_");
	public string DamageEnemy => BattleResources.DamageEnemy.Replace("_", "__").Replace("&", "_");

	public string CompactMode => BattleResources.CompactMode;

	public string Title => GeneralRes.Battle.Replace("_", "__").Replace("&", "_");
}
