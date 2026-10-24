using System.Diagnostics.CodeAnalysis;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Mocks;

namespace ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;

public class EquipmentSelectorFactory(
	IKCDatabase db,
	TransliterationService transliterationService)
{
	private IKCDatabase Db { get; } = db;
	private TransliterationService TransliterationService { get; } = transliterationService;

	[field: MaybeNull]
	public EquipmentSelectorViewModel QuestTrackerManager =>
		field ??= CreateQuestTrackerManagerEquipmentSelectorViewModel();

	private EquipmentSelectorViewModel CreateQuestTrackerManagerEquipmentSelectorViewModel()
	{
		List<IEquipmentData> equipment =
		[
			.. Db.MasterEquipments.Values
				.Where(e => !e.IsAbyssalEquipment)
				.Select(e => new EquipmentDataMock(e)),
		];

		return new(TransliterationService, equipment);
	}
}
