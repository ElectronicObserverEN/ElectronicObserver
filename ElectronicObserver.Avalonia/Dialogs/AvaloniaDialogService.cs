using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Avalonia.Dialogs;

public class AvaloniaDialogService(IDialogService dialogService) : DialogServiceBase(dialogService)
{
	protected override INotifyPropertyChanged Owner => Application.Current?.ApplicationLifetime switch
	{
		ClassicDesktopStyleApplicationLifetime { MainWindow.DataContext: INotifyPropertyChanged vm } => vm,
		_ => throw new NotImplementedException(),
	};
}
