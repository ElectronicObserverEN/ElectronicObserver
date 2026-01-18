using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public partial class UiBlockerOverlayWindow
{
	private MouseHook MouseHook { get; set; } = null!;

	public UiBlockerOverlayWindow(UiBlockerViewModel viewModel)
	{
		DataContext = viewModel;

		Top = viewModel.Top;
		Left = viewModel.Left;
		Height = viewModel.Height;
		Width = viewModel.Width;

		viewModel.PropertyChanged += UpdatePosition;

		InitializeComponent();
	}

	/// <summary>
	/// Binding doesn't seem to work for these properties so have to use this handler.
	/// </summary>
	private void UpdatePosition(object? sender, PropertyChangedEventArgs args)
	{
		if (sender is not UiBlockerViewModel viewModel) return;

		if (args.PropertyName is nameof(viewModel.Top))
		{
			Top = viewModel.Top;
		}
		else if (args.PropertyName is nameof(viewModel.Left))
		{
			Left = viewModel.Left;
		}
		else if (args.PropertyName is nameof(viewModel.Height))
		{
			Height = viewModel.Height;
		}
		else if (args.PropertyName is nameof(viewModel.Width))
		{
			Width = viewModel.Width;
		}
	}

	protected override void OnSourceInitialized(EventArgs e)
	{
		base.OnSourceInitialized(e);

		MouseHook = new(MouseFilter);
	}

	private bool MouseFilter(MouseMessage message, RawPoint point)
	{
		if (!IsVisible) return false;

		bool isBlockedMessage = message is
			MouseMessage.WM_LBUTTONUP or
			MouseMessage.WM_LBUTTONDOWN or
			MouseMessage.WM_RBUTTONUP or
			MouseMessage.WM_RBUTTONDOWN;

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
			Dispatcher.Invoke(Hide);
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
		MouseHook?.Dispose();
		base.OnClosed(e);
	}

	[LibraryImport("user32.dll")]
	private static partial short GetKeyState(int nVirtKey);
}
