using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.Dialogs.ShipSelector;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Tools.DropRecordViewer;

namespace ElectronicObserver.Window.Dialog.ShipSelector;

public partial class DropRecordShipSelectorViewModel(ImageLoadService imageLoadService, List<IShipData> ships)
	: ShipSelectorViewModel(imageLoadService, ships)
{
	public List<DropRecordOption> DropRecordOptions { get; } = [.. Enum.GetValues<DropRecordOption>()];

	public DropRecordOption? SelectedOption { get; set; }

	[RelayCommand]
	private void SelectDropRecordOption(DropRecordOption? pickedOption)
	{
		SelectedOption = pickedOption;
		DialogResult = pickedOption is not null;

		Close();
	}
}
