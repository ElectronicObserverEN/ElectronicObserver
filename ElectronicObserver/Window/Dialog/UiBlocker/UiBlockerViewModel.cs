using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public abstract partial class UiBlockerViewModel : ObservableObject
{
	protected UiBlockerConfiguration Configuration { get; }

	public abstract string Header { get; }

	// actual positioning values that get calculated from desired values and browser data
	[ObservableProperty] public partial double Top { get; set; }
	[ObservableProperty] public partial double Left { get; set; }
	[ObservableProperty] public partial double Height { get; set; }
	[ObservableProperty] public partial double Width { get; set; }

	// user config values
	[ObservableProperty] public partial bool IsEnabled { get; set; }
	[ObservableProperty] public partial double DesiredTop { get; set; }
	[ObservableProperty] public partial double DesiredLeft { get; set; }
	[ObservableProperty] public partial double DesiredHeight { get; set; }
	[ObservableProperty] public partial double DesiredWidth { get; set; }
	[ObservableProperty] public partial string? ImagePath { get; set; }
	[ObservableProperty] public partial string? BackgroundColor { get; set; }

	private double ZoomRate { get; set; }
	private double BrowserTop { get; set; }
	private double BrowserLeft { get; set; }

	protected UiBlockerViewModel(UiBlockerConfiguration configuration)
	{
		Configuration = configuration;

		Loaded();

		PropertyChanged += (_, e) =>
		{
			bool isDesiredPositionChanged = e.PropertyName is
				nameof(DesiredTop) or
				nameof(DesiredLeft) or
				nameof(DesiredHeight) or
				nameof(DesiredWidth);

			if (isDesiredPositionChanged)
			{
				UpdateBlockerPosition();
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
		Top = BrowserTop + DesiredTop * ZoomRate;
		Left = BrowserLeft + DesiredLeft * ZoomRate;
		Height = DesiredHeight * ZoomRate;
		Width = DesiredWidth * ZoomRate;
	}

	[RelayCommand]
	protected abstract void SetDesiredValuesToDefault();

	[RelayCommand]
	private void OpenImagePath()
	{
		string? newPath = FileService.OpenImagePath(ImagePath);

		if (newPath is null) return;

		ImagePath = newPath;
	}

	public virtual void Loaded()
	{
		Configuration.ApplyToViewModel(this);
	}

	public virtual void Closed()
	{
		this.SaveToConfiguration(Configuration);
	}
}
