using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace Browser.WebView2Browser.Extensions;

public partial class ExtensionManagerWindow
{
	public Tracker Tracker { get; }

	private WebView2 Browser { get; }
	private ExtensionManagerViewModel ViewModel { get; }

	public ExtensionManagerWindow(WebView2 browser)
	{
		Tracker = Ioc.Default.GetRequiredService<Tracker>();

		Browser = browser;
		ViewModel = new(Browser);
		DataContext = ViewModel;

		InitializeComponent();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		ViewModel.InitializeAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

		Loaded += (_, _) =>
		{
			StartJotTracking();
		};
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
