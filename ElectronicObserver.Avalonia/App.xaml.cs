using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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

	public static string Lang(string itemKey)
	{
		throw new NotImplementedException();
	}
}
