using System.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public interface IConditionViewModel : INotifyPropertyChanged
{
	string Display { get; }
	bool ConditionMet(IFleetData fleet);
}
