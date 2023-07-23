using System.ComponentModel.DataAnnotations;
using KancolleProgress.Translations;

namespace KancolleProgress.ViewModels;

public enum AswGroup
{
	None,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "NoSonarOpeningAsw")]
	NoSonar,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "SingleSonarOpeningAsw")]
	SingleSonar,
}
