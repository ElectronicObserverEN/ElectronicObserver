namespace ElectronicObserver.ViewModels.Translations;

public class FormCompassTranslationViewModel : TranslationBaseViewModel
{
	public string MapClearCount => CompassResources.MapClearCount.Replace("_", "__").Replace("&", "_");
	public string SpecialAttackActivated => CompassResources.SpecialAttackActivated.Replace("_", "__").Replace("&", "_");

	public string EnemySighted => CompassResources.EnemySighted.Replace("_", "__").Replace("&", "_");
	public string TargetSighted => CompassResources.TargetSighted.Replace("_", "__").Replace("&", "_");
	public string CoursePatrol => CompassResources.CoursePatrol.Replace("_", "__").Replace("&", "_");
	public string EnemyPlaneSighted => CompassResources.EnemyPlaneSighted.Replace("_", "__").Replace("&", "_");

	public string NoEnemySighted => CompassResources.NoEnemySighted.Replace("_", "__").Replace("&", "_");
	public string BranchChoice => CompassResources.BranchChoice.Replace("_", "__").Replace("&", "_");
	public string CalmSea => CompassResources.CalmSea.Replace("_", "__").Replace("&", "_");
	public string CalmStrait => CompassResources.CalmStrait.Replace("_", "__").Replace("&", "_");
	public string NeedToBeCareful => CompassResources.NeedToBeCareful.Replace("_", "__").Replace("&", "_");
	public string CalmSea2 => CompassResources.CalmSea2.Replace("_", "__").Replace("&", "_");

	public string BranchChoiceSeparator => CompassResources.BranchChoiceSeparator.Replace("_", "__").Replace("&", "_");
	public string RepairPossibility => CompassResources.RepairPossibility.Replace("_", "__").Replace("&", "_");
	public string AirRaid => CompassResources.AirRaid.Replace("_", "__").Replace("&", "_");
	public string FleetCount => CompassResources.FleetCount.Replace("_", "__").Replace("&", "_");
	public string UnknownItem => CompassResources.UnknownItem.Replace("_", "__").Replace("&", "_");
	public string None => CompassResources.None.Replace("_", "__").Replace("&", "_");
	public string AirValues => CompassResources.AirValues.Replace("_", "__").Replace("&", "_");

	public string TextMapArea => CompassResources.TextMapArea.Replace("_", "__").Replace("&", "_");
	public string TextDestination => CompassResources.TextDestination.Replace("_", "__").Replace("&", "_");
	public string TextEventKind => CompassResources.TextEventKind.Replace("_", "__").Replace("&", "_");
	public string TextEventDetail => CompassResources.TextEventDetail.Replace("_", "__").Replace("&", "_");

	public string TextEnemyFleetName => CompassResources.TextEnemyFleetName.Replace("_", "__").Replace("&", "_");
	public string TextFormation => CompassResources.TextFormation.Replace("_", "__").Replace("&", "_");
	public string TextAirSuperiority => CompassResources.TextAirSuperiority.Replace("_", "__").Replace("&", "_");

	public string Title = CompassResources.Title.Replace("_", "__").Replace("&", "_");
}
