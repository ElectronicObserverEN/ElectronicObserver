using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace ElectronicObserver.Common;

public partial class UserControlBase<TViewModel> : System.Windows.Controls.UserControl where TViewModel : UserControlViewModelBase
{
	private Tracker Tracker { get; }
	public TViewModel ViewModel { get; set; }

	public event EventHandler Closed = delegate { };

	protected UserControlBase(TViewModel viewModel)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel = viewModel;
		DataContext = ViewModel;

		SetBinding(FontSizeProperty, nameof(UserControlViewModelBase.FontSize));
		SetBinding(FontFamilyProperty, nameof(UserControlViewModelBase.Font));
		SetBinding(ForegroundProperty, nameof(UserControlViewModelBase.FontBrush));

		Loaded += (_, _) =>
		{
			ViewModel.Loaded();
			StartJotTracking();
		};

		IsVisibleChanged += (_, e) =>
		{
			if (e.NewValue is false)
			{
				Tracker.Persist(this);
			}
		};
	}

	public void Close()
	{
		Visibility = System.Windows.Visibility.Collapsed;
		Closed?.Invoke(this, new());
		ViewModel.Closed();
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
