using ElectronicObserver.Window;
using ElectronicObserver.Window.Tools.AutoRefresh;
using Xunit;

namespace ElectronicObserverCoreTests;

public class AutoRefreshTests
{
	[Fact(DisplayName = "SingleMap = false, Single rule, Rules contain current map")]
	public void AutoRefreshTest1()
	{
		AutoRefreshViewModel autoRefresh = new()
		{
			IsSingleMapMode = false,
			Rules =
			{
				new(new(1, 1))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				}
			}
		};

		Assert.False(FormBrowserHost.ShouldRefresh(1, 1, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 3, autoRefresh));
	}

	[Fact(DisplayName = "SingleMap = false, Single rule, Rules don't contain current map")]
	public void AutoRefreshTest2()
	{
		AutoRefreshViewModel autoRefresh = new()
		{
			IsSingleMapMode = false,
			Rules =
			{
				new(new(1, 1))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				}
			}
		};

		Assert.False(FormBrowserHost.ShouldRefresh(1, 2, 1, autoRefresh));
		Assert.False(FormBrowserHost.ShouldRefresh(1, 2, 2, autoRefresh));
		Assert.False(FormBrowserHost.ShouldRefresh(1, 2, 3, autoRefresh));
	}

	[Fact(DisplayName = "SingleMap = true, Single rule")]
	public void AutoRefreshTest3()
	{
		AutoRefreshViewModel autoRefresh = new()
		{
			IsSingleMapMode = true,
			Rules =
			{
				new(new(1, 1))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				}
			}
		};

		Assert.False(FormBrowserHost.ShouldRefresh(1, 1, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 3, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 2, 1, autoRefresh));
	}

	[Fact(DisplayName = "SingleMap = false, Multiple rules")]
	public void AutoRefreshTest4()
	{
		AutoRefreshViewModel autoRefresh = new()
		{
			IsSingleMapMode = false,
			Rules =
			{
				new(new(1, 1))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				},
				new(new(1, 2))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				}
			}
		};

		Assert.False(FormBrowserHost.ShouldRefresh(1, 1, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 3, autoRefresh));
		Assert.False(FormBrowserHost.ShouldRefresh(1, 2, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 2, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 2, 3, autoRefresh));
	}

	[Fact(DisplayName = "SingleMap = true, Multiple rules (one enabled)")]
	public void AutoRefreshTest5()
	{
		AutoRefreshViewModel autoRefresh = new()
		{
			IsSingleMapMode = true,
			Rules =
			{
				new(new(1, 1))
				{
					IsEnabled = false,
					AllowedCells =
					{
						new(1),
					}
				},
				new(new(1, 2))
				{
					IsEnabled = true,
					AllowedCells =
					{
						new(1),
					}
				}
			}
		};

		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 1, 3, autoRefresh));
		Assert.False(FormBrowserHost.ShouldRefresh(1, 2, 1, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 2, 2, autoRefresh));
		Assert.True(FormBrowserHost.ShouldRefresh(1, 2, 3, autoRefresh));
	}
}
