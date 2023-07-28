using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.ConstructionRecordViewer;

public enum ConstructionRecordOption
{
	[Display(ResourceType = typeof(ConstructionRecordViewerResources), Name = "NameAny")]
	All,
}
