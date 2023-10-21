using System.ComponentModel.DataAnnotations;
using KancolleProgress.Translations;

namespace KancolleProgress.ViewModels;

public enum FcfGroup
{
	None,

	[Display(ResourceType = typeof(KancolleProgressResources), Name = "Fcf")]
	Fcf,
}
