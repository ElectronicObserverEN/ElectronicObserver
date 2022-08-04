using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace ElectronicObserver.Common;

public class UserControlBase<TViewModel> : System.Windows.Controls.UserControl where TViewModel : WindowViewModelBase
{
	private Tracker Tracker { get; }
	public TViewModel ViewModel { get; }

	protected UserControlBase(TViewModel viewModel)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel = viewModel;
		DataContext = ViewModel;

		SetBinding(FontSizeProperty, nameof(WindowViewModelBase.FontSize));
		SetBinding(FontFamilyProperty, nameof(WindowViewModelBase.Font));
		SetBinding(ForegroundProperty, nameof(WindowViewModelBase.FontBrush));

		Loaded += (_, _) =>
		{
			ViewModel.Loaded();
			StartJotTracking();
		};
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
