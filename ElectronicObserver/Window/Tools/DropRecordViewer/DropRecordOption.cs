using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public enum DropRecordOption
{
	[Display(ResourceType = typeof(DropRecordViewerResources), Name = "NameAny")]
	All,
	[Display(ResourceType = typeof(DropRecordViewerResources), Name = "NameExist")]
	Drop,
	[Display(ResourceType = typeof(DropRecordViewerResources), Name = "NameNotExist")]
	NoDrop,
	[Display(ResourceType = typeof(DropRecordViewerResources), Name = "NameFullPort")]
	FullPort,
}
