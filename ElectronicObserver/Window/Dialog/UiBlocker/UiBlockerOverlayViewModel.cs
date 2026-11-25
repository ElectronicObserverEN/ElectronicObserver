using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.UiBlocker;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public sealed partial class UiBlockerOverlayViewModel : ObservableObject
{
	private UiElementViewModel UiElementViewModel { get; }

	[ObservableProperty] public partial double Top { get; set; }
	[ObservableProperty] public partial double Left { get; set; }
	[ObservableProperty] public partial double Height { get; set; }
	[ObservableProperty] public partial double Width { get; set; }

	public string? ImagePath => UiElementViewModel.ImagePath;
	public string? BackgroundColor => UiElementViewModel.Model.BackgroundColor;

	private double ZoomRate { get; set; }
	private double BrowserTop { get; set; }
	private double BrowserLeft { get; set; }

	public UiBlockerOverlayViewModel(UiElementViewModel uiElementViewModel)
	{
		UiElementViewModel = uiElementViewModel;

		UiElementViewModel.PropertyChanged += (_, args) =>
		{
			switch (args.PropertyName)
			{
				case nameof(UiElementViewModel.DesiredTop):
				case nameof(UiElementViewModel.DesiredLeft):
				case nameof(UiElementViewModel.DesiredHeight):
				case nameof(UiElementViewModel.DesiredWidth):
					UpdateBlockerPosition();
					break;

				case nameof(UiElementViewModel.ImagePath):
					OnPropertyChanged(nameof(ImagePath));
					break;

				case nameof(UiElementViewModel.BackgroundColor):
					OnPropertyChanged(nameof(BackgroundColor));
					break;
			}
		};
	}

	public void UpdateBrowserData(double zoomRate, double browserTop, double browserLeft)
	{
		ZoomRate = zoomRate;
		BrowserTop = browserTop;
		BrowserLeft = browserLeft;

		UpdateBlockerPosition();
	}

	private void UpdateBlockerPosition()
	{
		DpiScale dpi = VisualTreeHelper.GetDpi(Application.Current.MainWindow);

		Top = BrowserTop / dpi.DpiScaleY + UiElementViewModel.DesiredTop * ZoomRate;
		Left = BrowserLeft / dpi.DpiScaleX + UiElementViewModel.DesiredLeft * ZoomRate;
		Height = UiElementViewModel.DesiredHeight * ZoomRate;
		Width = UiElementViewModel.DesiredWidth * ZoomRate;
	}
}
