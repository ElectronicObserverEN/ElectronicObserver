using System;
using System.ComponentModel;
using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.Dialogs.EquipmentSelector;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;

namespace ElectronicObserver.Window.Dialog.EquipmentSelector;

public partial class EquipmentSelectorWindow
{
	public WpfAvaloniaHost WpfAvaloniaHost { get; }

	public EquipmentSelectorWindow(EquipmentSelectorViewModel viewModel) : base(viewModel)
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

		Closing += EquipmentSelector_Closing;
	}

	private void EquipmentSelector_Closing(object? sender, CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
	}

	private void ViewModel_PropertyChanged(object? sender, EventArgs e)
	{
		Close();
	}
}
