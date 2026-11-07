using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.Expedition;

public partial class ExpeditionTaskViewModel(
	IKCDatabase kcDatabase,
	TaskDisplayerService taskDisplayerService,
	ExpeditionTaskModel model) : TaskViewModel
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override ExpeditionTaskModel Model { get; } = model;

	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	// Deferred on purpose - Mission data is only populated once the game data has actually loaded,
	// which can happen well after this view model is constructed.
	public IEnumerable<IMissionData> Expeditions => KcDatabase.Mission.Values.OrderBy(e => e.MissionID);

	public IMissionData? SelectedExpedition
	{
		get => KcDatabase.Mission.Values.FirstOrDefault(e => e.MissionID == Model.ExpeditionId);
		set
		{
			if (value is null) return;

			Model.ExpeditionId = value.MissionID;
			OnPropertyChanged();
			RaiseDisplayChanged();
		}
	}

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnCountChanged(int value)
	{
		Count = Math.Clamp(value, 1, int.MaxValue);
		Model.Count = Count;
		OnProgressChanged(Progress);
	}

	partial void OnProgressChanged(int value)
	{
		Progress = Math.Clamp(value, 0, Count);
		RaiseDisplayChanged();
	}

	public void Increment(int areaId)
	{
		if (areaId != Model.ExpeditionId) return;

		Progress++;
	}
}
