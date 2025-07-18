using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;

public sealed partial class EquipmentSelectorViewModel(List<IEquipmentData> equipment)
	: ObservableObject, IModalDialogViewModel, ICloseable
{
	public List<EquipmentTypeGroupViewModel> EquipmentTypeGroups { get; } = equipment
		.Where(e => !e.MasterEquipment.IsAbyssalEquipment)
		.GroupBy(e => e.MasterEquipment.CategoryType.ToGroup())
		.Select(g => new EquipmentTypeGroupViewModel
		{
			EquipmentTypeGroup = g.Key,
			Equipment = [.. g],
		})
		.ToList();

	/// <inheritdoc />
	public event EventHandler? RequestClose;

	/// <inheritdoc />
	public bool? DialogResult { get; private set; }

	[MemberNotNullWhen(true, nameof(DialogResult))]
	public IEquipmentData? SelectedEquipment { get; set; }

	[RelayCommand]
	private void SelectEquipment()
	{
		DialogResult = true;

		RequestClose?.Invoke(this, EventArgs.Empty);
	}
}
