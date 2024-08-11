using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public partial class SortieCostConfigurationViewModel : ObservableObject
{
	[ObservableProperty] private bool _isShouhaBucket = true;
	[ObservableProperty] private bool _isChuuhaBucket = true;
	[ObservableProperty] private bool _isTaihaBucket = true;
}
