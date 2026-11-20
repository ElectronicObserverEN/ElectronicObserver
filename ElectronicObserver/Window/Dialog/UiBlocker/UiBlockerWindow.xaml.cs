using System;
using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.UiBlocker;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public partial class UiBlockerWindow
{
	private UiBlockerAdapter Adapter { get; }
	public WpfAvaloniaHost WpfAvaloniaHost { get; }
	
	public UiBlockerWindow(UiBlockerAdapter adapter) : base(adapter.UiBlockerManagerViewModel)
	{
		Adapter = adapter;
		WpfAvaloniaHost = new()
		{
			Content = new UiBlockerManagerView
			{
				DataContext = adapter.UiBlockerManagerViewModel,
			},
		};
		
		InitializeComponent();
	}

	/// <inheritdoc />
	protected override void OnClosed(EventArgs e)
	{
		_ = Adapter.Save();
		Adapter.UiBlockerManagerViewModel.HideAllCommand.Execute(null);
		
		base.OnClosed(e);
	}
}
