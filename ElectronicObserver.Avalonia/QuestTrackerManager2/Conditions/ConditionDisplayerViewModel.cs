using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;

public abstract class ConditionDisplayerViewModel : ObservableObject
{
	public abstract string Display { get; }
	
	public event ConditionChangedEventHandler? ConditionChanged;

	protected void RaiseConditionChanged()
	{
		ConditionChanged?.Invoke();
	}

	protected void RaiseDisplayChanged()
	{
		OnPropertyChanged(nameof(Display));
	}
}
