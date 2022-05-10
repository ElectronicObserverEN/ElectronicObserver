using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserverCoreTests;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		Ioc.Default.ConfigureServices(new ServiceCollection().BuildServiceProvider());
	}
}
