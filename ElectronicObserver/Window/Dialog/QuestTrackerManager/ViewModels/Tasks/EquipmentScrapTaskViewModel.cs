using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public partial class EquipmentScrapTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	private IEquipmentDataMaster? _selectedEquipment;

	public IEquipmentDataMaster SelectedEquipment
	{
		// bug: Equipment doesn't get loaded till Kancolle loads
		get => _selectedEquipment ??= Equipment.FirstOrDefault(s => s.EquipmentId == Model.Id)
						 ?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref _selectedEquipment, value);
	}

	public IEnumerable<IEquipmentDataMaster> Equipment => KCDatabase.Instance.MasterEquipments.Values
		.Where(s => !s.IsAbyssalEquipment)
		.OrderBy(s => s.EquipmentID);

	public EquipmentScrapTaskModel Model { get; }

	private EquipmentSelectorViewModel? EquipmentSelectorViewModel { get; set; }
	private TransliterationService TransliterationService { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition() =>
		SelectedEquipment.NameEN + QuestTracking.Discard + Model.Count + QuestTracking.NumberOfPieces;

	public EquipmentScrapTaskViewModel(EquipmentScrapTaskModel model)
	{
		Model = model;
		TransliterationService = Ioc.Default.GetRequiredService<TransliterationService>();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedEquipment)) return;

			Model.Id = SelectedEquipment?.EquipmentId ?? EquipmentId.MainGunSmall_12_7cmSingleGun;
		};

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(IEnumerable<EquipmentId> ids)
	{
		Model.Progress += ids.Count(id => id == Model.Id);
	}

	[RelayCommand]
	private void OpenEquipmentPicker()
	{
		if (EquipmentSelectorViewModel is null)
		{
			List<IEquipmentData> equipments = Equipment
				.Select(s => new EquipmentDataMock(s))
				.OfType<IEquipmentData>()
				.ToList();

			EquipmentSelectorViewModel = new(TransliterationService, equipments);
		}

		EquipmentSelectorViewModel.ShowDialog();

		if (EquipmentSelectorViewModel.SelectedEquipment is null) return;

		Model.Id = EquipmentSelectorViewModel.SelectedEquipment.MasterEquipment.EquipmentId;
		SelectedEquipment = EquipmentSelectorViewModel.SelectedEquipment.MasterEquipment;
	}
}
