using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public abstract class TaskViewModel : ObservableObject
{
	public abstract ITaskModel Model { get; }
	public abstract string Display { get; }
	public abstract int Count { get; set; }
	public abstract int Progress { get; set; }

	protected void RaiseDisplayChanged()
	{
		OnPropertyChanged(nameof(Display));
	}
}
