using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;

public partial class EquipmentCategoryScrapTaskViewModel(
	TaskDisplayerService taskDisplayerService,
	EquipmentCategoryScrapTaskModel model) : TaskViewModel
{
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override EquipmentCategoryScrapTaskModel Model { get; } = model;

	public List<EquipmentTypes> Categories { get; } = [.. Enum.GetValues<EquipmentTypes>()];

	[ObservableProperty] public partial EquipmentTypes Category { get; set; } = model.Category;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnCategoryChanged(EquipmentTypes value)
	{
		Model.Category = value;
		RaiseDisplayChanged();
	}

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

	public void Increment(IEnumerable<EquipmentTypes> categories)
	{
		Progress += categories.Count(category => category == Category);
	}
}
