using Avalonia;
using Avalonia.Controls;

namespace ElectronicObserver.Avalonia.Dialogs.ShipSelector;

public partial class ShipSelectorWindow : Window
{
	public ShipSelectorWindow()
	{
		InitializeComponent();

#if DEBUG
		this.AttachDevTools();
#endif
	}
}
