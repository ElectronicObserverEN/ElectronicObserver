using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed partial class UiTriggerOptionViewModel(UiBlockerTrigger trigger) : ObservableObject
{
	public UiBlockerTrigger Trigger { get; } = trigger;

	[ObservableProperty] public partial bool IsChecked { get; set; }
}
