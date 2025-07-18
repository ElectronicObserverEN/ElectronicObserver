using System.ComponentModel;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Avalonia.Dialogs;

/// <summary>
/// This is just needed to get the Owner set up.
/// </summary>
public abstract class DialogServiceBase(IDialogService dialogService)
{
	private IDialogService DialogService { get; } = dialogService;

	protected abstract INotifyPropertyChanged Owner { get; }

	public virtual Task<bool?> ShowDialogAsync(IModalDialogViewModel viewModel, INotifyPropertyChanged? owner = null)
		=> DialogService.ShowDialogAsync(owner ?? Owner, viewModel);
}
