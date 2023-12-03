using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;

public partial class EquipmentAssignmentItemViewModel : ObservableValidator, IEquipmentPlanItemViewModel
{
	public EquipmentAssignmentItemModel Model { get; }

	[Required] [NotifyDataErrorInfo] [ObservableProperty] private IEquipmentData? _assignedEquipment;

	[Required] [NotifyDataErrorInfo][ObservableProperty] private EquipmentUpgradePlanItemViewModel? _assignedPlan;

	[MemberNotNullWhen(false, nameof(AssignedPlan))]
	[MemberNotNullWhen(false, nameof(AssignedEquipment))]
	private bool HasValidationErrors => GetErrors().Any();

	public EquipmentId EquipmentMasterDataId { get; private set; }

	public EquipmentUpgradePlanCostViewModel Cost => new(new());

	private EquipmentUpgradePlanManager PlanManager { get; }
	private EquipmentPickerService EquipmentPicker { get; }
	private EquipmentUpgradePlannerTranslationViewModel Translations { get; }

	public EquipmentAssignmentItemViewModel(EquipmentAssignmentItemModel model)
	{
		PlanManager = Ioc.Default.GetRequiredService<EquipmentUpgradePlanManager>();
		EquipmentPicker = Ioc.Default.GetRequiredService<EquipmentPickerService>();
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

		return true;
	}

	public void UnsubscribeFromApis()
	{
		Cost.UnsubscribeFromApis();
	}

	public List<IEquipmentPlanItemViewModel> GetPlanChildren()
	{
		return new();
	}

	public void OpenEquipmentPicker()
	{
		EquipmentAssignmentPickerViewModel viewModel = new(PlanManager, EquipmentMasterDataId);

		IEquipmentData? equipment = EquipmentPicker.OpenEquipmentPicker(viewModel);

		if (equipment is null) return;

		AssignedEquipment = equipment;
	}
}
