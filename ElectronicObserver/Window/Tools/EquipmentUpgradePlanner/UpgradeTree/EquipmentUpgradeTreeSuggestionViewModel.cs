using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.UpgradeTree;

public partial class EquipmentUpgradeTreeSuggestionViewModel : ObservableObject
{
	[ObservableProperty]
	private List<EquipmentUpgradeTreeSuggestionRowViewModel> suggestions = new();

	public void CalculateSuggestions(UpgradeTreeUpgradePlanViewModel rootPlan)
	{
		List<EquipmentUpgradeTreeSuggestionRowViewModel> newSuggestions = new();

		Suggestions = newSuggestions;
	}

	private List<EquipmentUpgradeTreeSuggestionRowViewModel> GetSuggestions(UpgradeTreeUpgradePlanViewModel plan)
	{
		return new();
	}
}
