using System;

namespace ElectronicObserver.Common;

public class WindowBase<TViewModel> : System.Windows.Window where TViewModel : WindowViewModelBase
{
	public TViewModel ViewModel { get; }

	[Obsolete("This is only needed so WPF doesn't complain, don't use this.")]
#pragma warning disable CS8618
	public WindowBase()
#pragma warning restore CS8618
	{
		
	}

	protected WindowBase(TViewModel viewModel)
	{
		ViewModel = viewModel;
		DataContext = ViewModel;

		SetBinding(FontSizeProperty, nameof(WindowViewModelBase.FontSize));
		SetBinding(FontFamilyProperty, nameof(WindowViewModelBase.Font));
		SetBinding(ForegroundProperty, nameof(WindowViewModelBase.FontBrush));

		Loaded += (_, _) => ViewModel.Loaded();
		Closed += (_, _) => ViewModel.Closed();
	}
}
