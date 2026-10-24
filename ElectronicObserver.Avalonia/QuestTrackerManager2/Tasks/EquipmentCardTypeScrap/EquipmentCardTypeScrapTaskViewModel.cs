using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCardTypeScrap;

public partial class EquipmentCardTypeScrapTaskViewModel(
	TaskDisplayerService taskDisplayerService,
	EquipmentCardTypeScrapTaskModel model) : TaskViewModel
{
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	public override EquipmentCardTypeScrapTaskModel Model { get; } = model;

	public List<EquipmentCardType> CardTypes { get; } = [.. Enum.GetValues<EquipmentCardType>()];

	[ObservableProperty] public partial EquipmentCardType CardType { get; set; } = model.CardType;
	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

	public override string Display => TaskDisplayerService.ToDisplay(this);

	partial void OnCardTypeChanged(EquipmentCardType value)
	{
		Model.CardType = value;
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

	public void Increment(IEnumerable<EquipmentCardType> cardTypes)
	{
		Progress += cardTypes.Count(cardType => cardType == CardType);
	}
}
