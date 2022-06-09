using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Database;
using ElectronicObserver.TestData;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserverCoreTests;

public class Startup
{
	public async void ConfigureServices(IServiceCollection services)
	{
		Ioc.Default.ConfigureServices(new ServiceCollection()
			.AddSingleton<AutoRefreshTranslationViewModel>()
			.BuildServiceProvider());

		Directory.CreateDirectory("Record");

		await using ElectronicObserverContext db = new();
		await db.Database.MigrateAsync();
	}
}
