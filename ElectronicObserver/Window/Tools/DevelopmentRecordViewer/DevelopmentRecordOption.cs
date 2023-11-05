using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer;

public enum DevelopmentRecordOption
{
	[Display(ResourceType = typeof(DevelopmentRecordViewerResources), Name = "NameAny")]
	All,
	[Display(ResourceType = typeof(DevelopmentRecordViewerResources), Name = "NameExist")]
	Success,
	[Display(ResourceType = typeof(DevelopmentRecordViewerResources), Name = "NameNotExist")]
	Failure
}
