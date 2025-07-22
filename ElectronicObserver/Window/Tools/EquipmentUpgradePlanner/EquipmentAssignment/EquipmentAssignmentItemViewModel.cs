using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Avalonia.Controls.EquipmentFilter;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public partial class EquipmentAssignmentItemViewModel : ObservableValidator, IEquipmentPlanItemViewModel
{
	public EquipmentAssignmentItemModel Model { get; }

	[Required]
	[NotifyDataErrorInfo]
	[ObservableProperty]
	public partial IEquipmentData? AssignedEquipment { get; set; }

	[Required]
	[NotifyDataErrorInfo]
	[ObservableProperty]
	public partial EquipmentUpgradePlanItemViewModel? AssignedPlan { get; set; }

	[MemberNotNullWhen(false, nameof(AssignedPlan))]
	[MemberNotNullWhen(false, nameof(AssignedEquipment))]
	private bool HasValidationErrors => GetErrors().Any();

	public bool WillBeUsedForConversion { get; set; }

	public EquipmentId EquipmentMasterDataId { get; private set; }

	public EquipmentUpgradePlanCostViewModel Cost => new(new());

	private EquipmentUpgradePlanManager PlanManager { get; }
	private TransliterationService TransliterationService { get; }
	private EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public EquipmentAssignmentItemViewModel(EquipmentAssignmentItemModel model)
	{
		PlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		TransliterationService = Ioc.Default.GetRequiredService<TransliterationService>();
		Translations = Ioc.Default.GetRequiredService<EquipmentUpgradePlannerTranslationViewModel>();

		Model = model;

		LoadModel();
	}

	public void LoadModel()
	{
		AssignedPlan = PlanManager.PlannedUpgrades.FirstOrDefault(plan => plan.Plan == Model.Plan);

		AssignedEquipment = KCDatabase.Instance.Equipments.ContainsKey(Model.EquipmentId) switch
		{
			true => KCDatabase.Instance.Equipments[Model.EquipmentId]!,
			_ => null
		};

		EquipmentMasterDataId = Model.EquipmentMasterDataId;
		WillBeUsedForConversion = Model.WillBeUsedForConversion;
	}

	/// <summary>
	/// Saves changes to the model
	/// Returns false if changes couldn't have been saved
	/// </summary>
	/// <returns></returns>
	public bool TrySaveChanges()
	{
		if (HasValidationErrors)
		{
			List<ValidationResult> errors = GetErrors().ToList();

			string caption = Translations.Error;
			string errorMessage = string.Join("\n", errors);

			MessageBox.Show(App.Current!.MainWindow!, errorMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);

			return false;
		}

		Model.Plan = AssignedPlan.Plan;
		Model.EquipmentId = AssignedEquipment.ID;
		Model.EquipmentMasterDataId = EquipmentMasterDataId;
		Model.WillBeUsedForConversion = WillBeUsedForConversion;

		return true;
	}

	public void UnsubscribeFromApis()
	{
		Cost.UnsubscribeFromApis();
	}

	public List<IEquipmentPlanItemViewModel> GetPlanChildren()
	{
		return [];
	}

	public void OpenEquipmentPicker()
	{
		List<IEquipmentData> equipment = KCDatabase.Instance.Equipments.Values
			.OfType<IEquipmentData>()
			.Where(eq => eq.EquipmentId == EquipmentMasterDataId)
			.Where(eq => WillBeUsedForConversion || eq.UpgradeLevel is UpgradeLevel.Zero)
			.Where(eq => !PlanManager.GetAssignments(EquipmentMasterDataId).Select(assignment => assignment.EquipmentId).Contains(eq.ID))
			.OrderBy(s => s.MasterEquipment.CategoryType)
			.ThenBy(s => s.MasterID)
			.ToList();

		EquipmentSelectorViewModel viewModel = new(TransliterationService, equipment);

		foreach (Filter typeFilter in viewModel.EquipmentFilter.TypeFilters)
		{
			typeFilter.IsChecked = true;
		}

		viewModel.ShowDialog();

		if (viewModel.DialogResult is not true) return;

		AssignedEquipment = viewModel.SelectedEquipment;
	}
}
