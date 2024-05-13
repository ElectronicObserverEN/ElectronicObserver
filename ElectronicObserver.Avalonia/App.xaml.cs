using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using ElectronicObserver.Avalonia.ShipGroup;

namespace ElectronicObserver.Avalonia;

public class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			desktop.MainWindow = new Window
			{
				Content = new ShipGroupView
				{
					// DataContext = new ShipGroupViewModel(),
				},
			};
		}

		base.OnFrameworkInitializationCompleted();
	}

	public void UpdateTheme(ThemeVariant themeVariant)
	{
		RequestedThemeVariant = themeVariant;
	}

	public void UpdateFont(string fontName, double fontSize)
	{
		if (Resources.ContainsKey("ContentControlThemeFontFamily"))
		{
			FontFamily font = new(fontName);
			Resources["ContentControlThemeFontFamily"] = font;
		}

		if (Resources.ContainsKey("ControlContentThemeFontSize"))
		{
			Resources["ControlContentThemeFontSize"] = fontSize;
		}
	}
}
