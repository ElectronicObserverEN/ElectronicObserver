using System.ComponentModel;
using System.Threading.Tasks;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.ViewModels;
using HanumanInstitute.MvvmDialogs;
using DialogServiceBase = ElectronicObserver.Avalonia.Dialogs.DialogServiceBase;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

/// <summary>
/// Needed so we can open dialogs from the Avalonia project.
/// </summary>
public sealed class WpfDialogService() : DialogServiceBase(null!)
{
	protected override INotifyPropertyChanged Owner => App.MainViewModel;

	public override Task<bool?> ShowDialogAsync(IModalDialogViewModel viewModel, INotifyPropertyChanged? owner = null)
	{
		bool? result = viewModel switch
		{
			ShipSelectorViewModel ship => ship.ShowDialog(),
			EquipmentSelectorViewModel equipment => equipment.ShowDialog(),

			_ => null,
		};

		return Task.FromResult(result);
	}
}
