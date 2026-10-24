using System.Globalization;
using System.Threading;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks.EquipmentCategoryScrap;
using ElectronicObserver.Avalonia.Samples.QuestTrackerManager;
using ElectronicObserver.Avalonia.Translation;
using ElectronicObserver.Core.Types.Data;
using HotAvalonia;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Avalonia.Samples;

public partial class App : Application
{
	public override void Initialize()
	{
		CultureInfo ci = new("ja-JP");
		CultureInfo.DefaultThreadCurrentCulture = ci;
		CultureInfo.DefaultThreadCurrentUICulture = ci;
		Thread.CurrentThread.CurrentCulture = ci;
		Thread.CurrentThread.CurrentUICulture = ci;

		this.UseHotReload();
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		using ServiceProvider provider = new();
		ClipboardService clipboardService = new();
		provider.ClipboardService = clipboardService;

		MainViewModel mainViewModel = provider.GetRequiredService<MainViewModel>();
		
		if (Resources["ShipClassDisplayConverter"] is ShipClassDisplayConverter shipClassConverter)
		{
			shipClassConverter.DataService = provider.GetRequiredService<DataService>();
		}

		if (Resources["EquipmentCategoryDisplayConverter"] is EquipmentCategoryDisplayConverter equipmentCategoryConverter)
		{
			equipmentCategoryConverter.KcDatabase = provider.GetRequiredService<IKCDatabase>();
		}

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			MainWindow mainWindow = new()
			{
				DataContext = mainViewModel,
			};

			clipboardService.Clipboard = mainWindow.Clipboard;

			desktop.MainWindow = mainWindow;
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
		{
			MainWindow mainWindow = new()
			{
				DataContext = mainViewModel,
			};

			clipboardService.Clipboard = mainWindow.Clipboard;

			singleViewPlatform.MainView = mainWindow;
		}

		base.OnFrameworkInitializationCompleted();
	}
}
