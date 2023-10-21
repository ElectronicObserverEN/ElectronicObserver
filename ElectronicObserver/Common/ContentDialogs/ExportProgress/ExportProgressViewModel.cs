using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common.ContentDialogs.ExportProgress;

public class ExportProgressViewModel : ObservableObject
{
	public int Progress { get; set; }
	public int Total { get; set; }
}
