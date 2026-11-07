using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Common;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels;
using MessagePack;
using MessagePack.Resolvers;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public abstract class QuestTrackerManagerBase : WindowViewModelBase, IQuestTrackerManager
{
	public ObservableCollection<TrackerViewModel> Trackers { get; } = [];
	IEnumerable<ITrackerViewModel> IQuestTrackerManager.Trackers => Trackers;

	public DateTime LastQuestListUpdate { get; set; } = new(2000, 1, 1);
	public DateTime LastTrackerSave { get; set; } = DateTime.Now;

	// MessagePack has a bug when converting DateTime to json
	// adding these options avoids it by using a different DateTime representation
	protected MessagePackSerializerOptions DateTimeOptions => MessagePackSerializerOptions.Standard
		.WithResolver(CompositeResolver.Create(NativeDateTimeResolver.Instance, TypelessObjectResolver.Instance));	

	public abstract void Load();
	public abstract void Save();

	public TrackerViewModel? GetTrackerById(int id) => Trackers.FirstOrDefault(t => t.QuestId == id);
}
