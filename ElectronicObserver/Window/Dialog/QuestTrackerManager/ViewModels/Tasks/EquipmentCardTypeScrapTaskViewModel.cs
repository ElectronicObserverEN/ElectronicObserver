using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class EquipmentCardTypeScrapTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<EquipmentCardType> CardTypes { get; }

	public EquipmentCardTypeScrapTaskModel Model { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {ClearCondition}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition() => $"{QuestTracking.Illust}[{Model.CardType.TranslatedName()}]"
		+ QuestTracking.Discard + Model.Count + QuestTracking.NumberOfPieces;

	public EquipmentCardTypeScrapTaskViewModel(EquipmentCardTypeScrapTaskModel model)
	{
		CardTypes = Enum.GetValues<EquipmentCardType>();

		Model = model;

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

	public void Increment(IEnumerable<EquipmentCardType> cardTypes)
	{
		Model.Progress += cardTypes.Count(id => id == Model.CardType);
	}
}
