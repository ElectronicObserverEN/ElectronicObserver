using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public partial class NodeIdViewModel(int id) : ObservableObject
{
	[ObservableProperty] public partial int Id { get; set; } = id;
}
