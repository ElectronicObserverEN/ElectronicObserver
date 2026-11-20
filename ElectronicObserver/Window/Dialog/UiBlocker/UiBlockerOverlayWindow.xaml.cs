using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using ElectronicObserver.Avalonia.UiBlocker;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public partial class UiBlockerOverlayWindow
{
	private MouseHook? MouseHook { get; set; }

	public UiBlockerOverlayViewModel ViewModel { get; }

	public UiBlockerOverlayWindow(UiBlockerOverlayViewModel viewModel)
	{
		DataContext = viewModel;
		ViewModel = viewModel;

		Top = viewModel.Top;
		Left = viewModel.Left;
		Height = viewModel.Height;
		Width = viewModel.Width;

		viewModel.PropertyChanged += UpdatePosition;
		IsVisibleChanged += OverlayIsVisibleChanged;

		InitializeComponent();
	}

	private void OverlayIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		MouseHook?.Dispose();

		MouseHook = e.NewValue switch
		{
			true => new(MouseFilter),
			_ => null,
		};
	}

	/// <summary>
	/// Binding doesn't seem to work for these properties so have to use this handler.
	/// </summary>
	private void UpdatePosition(object? sender, PropertyChangedEventArgs args)
	{
		if (sender is not UiBlockerOverlayViewModel viewModel) return;

		switch (args.PropertyName)
		{
			case nameof(viewModel.Top):
				Top = viewModel.Top;
				break;

			case nameof(viewModel.Left):
				Left = viewModel.Left;
				break;

			case nameof(viewModel.Height):
				Height = viewModel.Height;
				break;

			case nameof(viewModel.Width):
				Width = viewModel.Width;
				break;
		}
	}

	private bool MouseFilter(MouseMessage message, RawPoint point)
	{
		if (!IsVisible) return false;

		bool isBlockedMessage = message is
			MouseMessage.WM_LBUTTONUP or
			MouseMessage.WM_LBUTTONDOWN or
			MouseMessage.WM_RBUTTONUP or
			MouseMessage.WM_RBUTTONDOWN or
			MouseMessage.WM_MBUTTONUP or
			MouseMessage.WM_MBUTTONDOWN or
			MouseMessage.WM_XBUTTONUP or
			MouseMessage.WM_XBUTTONDOWN;

		if (!isBlockedMessage) return false;

		Point screenPoint = new(point.X, point.Y);
		Point windowPoint = PointFromScreen(screenPoint);

		bool isClickWithinBlocker =
			windowPoint.X >= 0 &&
			windowPoint.X <= ActualWidth &&
			windowPoint.Y >= 0 &&
			windowPoint.Y <= ActualHeight;

		// hide overlay with ctrl + click
		if (isClickWithinBlocker && IsCtrlDown())
		{
			Dispatcher.BeginInvoke(Hide);
			return true;
		}

		return isClickWithinBlocker;
	}

	private static bool IsCtrlDown()
	{
		const int VK_CONTROL = 0x11;
		return (GetKeyState(VK_CONTROL) & 0x8000) != 0;
	}

	protected override void OnClosed(EventArgs e)
	{
		// Detach the view model handler before the overlay is recreated for another host window.
		if (DataContext is UiBlockerViewModel viewModel)
		{
			viewModel.PropertyChanged -= UpdatePosition;
		}

		MouseHook?.Dispose();
		IsVisibleChanged -= OverlayIsVisibleChanged;
		base.OnClosed(e);
	}

	[LibraryImport("user32.dll")]
	private static partial short GetKeyState(int nVirtKey);
}
