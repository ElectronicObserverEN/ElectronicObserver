using System;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace ElectronicObserver.Common;

public class WindowBase<TViewModel> : System.Windows.Window
{
	private Tracker Tracker { get; }
	public TViewModel ViewModel { get; }

	[Obsolete("This is only needed so WPF doesn't complain, don't use this.", true)]
#pragma warning disable CS8618
	public WindowBase()
#pragma warning restore CS8618
	{

	}

	protected WindowBase(TViewModel viewModel)
	{
		Tracker = Ioc.Default.GetRequiredService<Tracker>();

		ViewModel = viewModel;
		DataContext = ViewModel;

		if (ViewModel is WindowViewModelBase)
		{
			SetBinding(FontSizeProperty, nameof(WindowViewModelBase.FontSize));
			SetBinding(FontFamilyProperty, nameof(WindowViewModelBase.Font));
			SetBinding(ForegroundProperty, nameof(WindowViewModelBase.FontBrush));
		}

		Loaded += OnLoaded;
		Closed += OnClosed;
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		if (ViewModel is WindowViewModelBase vm)
		{
			vm.Loaded();
		}

		StartJotTracking();
	}

	private void OnClosed(object? sender, EventArgs eventArgs)
	{
		if (ViewModel is WindowViewModelBase vm)
		{
			vm.Closed();
		}

		Loaded -= OnLoaded;
		Closed -= OnClosed;
	}
}
