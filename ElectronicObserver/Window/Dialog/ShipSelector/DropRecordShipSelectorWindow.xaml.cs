using System;
using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;

namespace ElectronicObserver.Window.Dialog.ShipSelector;

public partial class DropRecordShipSelectorWindow
{
	public WpfAvaloniaHost WpfAvaloniaHost { get; }

	public DropRecordShipSelectorWindow(DropRecordShipSelectorViewModel viewModel) : base(viewModel)
	{
		WpfAvaloniaHost = new()
		{
			Content = new ShipSelectorView
			{
				DataContext = viewModel,
			},
		};

		InitializeComponent();

		ViewModel.RequestClose += ViewModel_PropertyChanged;

		Closing += ShipPicker_Closing;
	}

	private void ShipPicker_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, EventArgs e)
	{
		Close();
	}
}
