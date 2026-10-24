using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentIconTypeScrap;

public partial class EquipmentIconTypeScrapTaskViewModel(
	TaskDisplayerService taskDisplayerService,
	EquipmentIconTypeScrapTaskModel model) : TaskViewModel
{
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override EquipmentIconTypeScrapTaskModel Model { get; } = model;

	public List<EquipmentIconType> IconTypes { get; } = [.. Enum.GetValues<EquipmentIconType>()];

	[ObservableProperty] public partial EquipmentIconType IconType { get; set; } = model.IconType;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnIconTypeChanged(EquipmentIconType value)
	{
		Model.IconType = value;
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

	public void Increment(IEnumerable<EquipmentIconType> iconTypes)
	{
		Progress += iconTypes.Count(iconType => iconType == IconType);
	}
}
