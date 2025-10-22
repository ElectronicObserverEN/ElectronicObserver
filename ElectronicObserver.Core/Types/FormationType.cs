namespace ElectronicObserver.Core.Types;

public enum FormationType
{
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "LineAhead")]
	LineAhead = 1,
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "DoubleLine")]
	DoubleLine = 2,
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "Diamond")]
	Diamond = 3,
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "Echelon")]
	Echelon = 4,
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "LineAbreast")]
	LineAbreast = 5,
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "Vanguard")]
	Vanguard = 6,
	/// <summary>Anchor</summary>
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "FirstPatrolFormation")]
	FirstPatrolFormation = 11,
	/// <summary>Torpedo</summary>
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "SecondPatrolFormation")]
	SecondPatrolFormation = 12,
	/// <summary>Turtle</summary>
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "ThirdPatrolFormation")]
	ThirdPatrolFormation = 13,
	/// <summary>Chicken Foot</summary>
	//[Display(ResourceType = typeof(Properties.FormationType), Name = "FourthPatrolFormation")]
	FourthPatrolFormation = 14
}
