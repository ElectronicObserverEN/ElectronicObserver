using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentScrap;

public partial class EquipmentScrapTaskViewModel(
	IKCDatabase kcDatabase,
	TaskDisplayerService taskDisplayerService,
	DialogServiceBase dialogService,
	EquipmentSelectorFactory equipmentSelectorFactory,
	EquipmentScrapTaskModel model) : TaskViewModel
{
	private IKCDatabase KcDatabase { get; } = kcDatabase;
	private TaskDisplayerService TaskDisplayerService { get; } = taskDisplayerService;
	private DialogServiceBase DialogService { get; } = dialogService;
	private EquipmentSelectorFactory EquipmentSelectorFactory { get; } = equipmentSelectorFactory;
	private EquipmentSelectorViewModel EquipmentSelectorViewModel => EquipmentSelectorFactory.QuestTrackerManager;
	public override EquipmentScrapTaskModel Model { get; } = model;

	public IEquipmentDataMaster SelectedEquipment
	{
		get => KcDatabase.MasterEquipments[(int)Model.Id];
		set
		{
			if (value is null) return;

			Model.Id = value.EquipmentId;
			OnPropertyChanged();
		}
	}

	[ObservableProperty] public override partial int Count { get; set; } = model.Count;
	[ObservableProperty] public override partial int Progress { get; set; }

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

	public void Increment(IEnumerable<EquipmentId> ids)
	{
		Progress += ids.Count(id => id == Model.Id);
	}

	[RelayCommand]
	private async Task AddEquipment()
	{
		await DialogService.ShowDialogAsync(EquipmentSelectorViewModel);

		if (EquipmentSelectorViewModel.SelectedEquipment is null) return;

		SelectedEquipment = EquipmentSelectorViewModel.SelectedEquipment.MasterEquipment;
	}
}
