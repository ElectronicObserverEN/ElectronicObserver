using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public partial class UiElementView : UserControl
{
	public UiElementView()
	{
		InitializeComponent();
	}

	// todo: file service
	private async void OnBrowseClick(object? sender, RoutedEventArgs e)
	{
		if (DataContext is not UiElementViewModel vm) return;
		if (TopLevel.GetTopLevel(this) is not TopLevel topLevel) return;

		IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			Title = UiBlockerResources.OpenImage,
			AllowMultiple = false,
			FileTypeFilter = [FilePickerFileTypes.ImageAll],
		});

		string? path = files[0]?.TryGetLocalPath();
		if (path is not null)
		{
			vm.ImagePath = path;
		}
	}
}
