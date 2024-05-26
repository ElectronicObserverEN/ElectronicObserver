using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;

namespace ElectronicObserver.Dialogs.TextInput;

public partial class TextInputViewModel : ObservableObject, IModalDialogViewModel, ICloseable
{
	public event EventHandler? RequestClose;
	public bool? DialogResult { get; private set; }

	public required string Title { get; init; }
	public string? Description { get; init; }
	public string Text { get; set; } = string.Empty;

	[RelayCommand]
	private void Ok()
	{
		// ModernWPF will set the value to null when you press x in the TextBox
		// ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
		Text ??= string.Empty;

		DialogResult = true;
		RequestClose?.Invoke(this, EventArgs.Empty);
	}

	[RelayCommand]
	private void Cancel()
	{
		// ModernWPF will set the value to null when you press x in the TextBox
		// ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
		Text ??= string.Empty;

		DialogResult = false;
		RequestClose?.Invoke(this, EventArgs.Empty);
	}
}
