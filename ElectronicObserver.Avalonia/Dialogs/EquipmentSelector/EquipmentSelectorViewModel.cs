using System.ComponentModel;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Controls.EquipmentFilter;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;

public sealed partial class EquipmentSelectorViewModel : ObservableObject, IModalDialogViewModel, ICloseable
{
	public EquipmentFilterViewModel EquipmentFilter { get; }
	private List<EquipmentViewModel> Equipment { get; }

	public DataGridCollectionView CollectionView { get; }

	/// <inheritdoc />
	public event EventHandler? RequestClose;

	/// <inheritdoc />
	public bool? DialogResult { get; private set; }

	public IEquipmentData? SelectedEquipment { get; set; }

	public EquipmentSelectorViewModel(TransliterationService transliterationService, 
		List<IEquipmentData> equipment)
	{
		Equipment = equipment
			.Where(e => !e.MasterEquipment.IsAbyssalEquipment)
			.Select(e => new EquipmentViewModel(e))
			.ToList();

		EquipmentFilter = new(transliterationService, equipment);

		CollectionView = new(Equipment)
		{
			Filter = o => o switch
			{
				EquipmentViewModel viewModel => EquipmentFilter.MeetsFilterCondition(viewModel.Equipment),
				_ => true,
			},
		};

		EquipmentFilter.PropertyChanged += EquipmentFilter_PropertyChanged;
	}

	private void EquipmentFilter_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		CollectionView.Refresh();
	}

	[RelayCommand]
	private void SelectEquipment(EquipmentViewModel? equipment)
	{
		SelectedEquipment = equipment?.Equipment;
		DialogResult = equipment is not null;

		Close();
	}

	private void Close()
	{
		// https://github.com/AvaloniaUI/Avalonia/issues/16199#issuecomment-2244891047
		Dispatcher.UIThread.Post(() => RequestClose?.Invoke(this, EventArgs.Empty));
	}
}
