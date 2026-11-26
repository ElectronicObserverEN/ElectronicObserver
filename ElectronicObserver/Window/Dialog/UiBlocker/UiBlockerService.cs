using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using ElectronicObserver.Avalonia.UiBlocker;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using GongSolutions.Wpf.DragDrop.Utilities;
using MahApps.Metro.Controls;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class UiBlockerService(
	DockingManager dockingManager,
	Configuration.ConfigurationData configuration) : IUiBlockerService
{
	private static Size KancolleSize { get; } = new(1200, 720);

	private DockingManager DockingManager { get; } = dockingManager;
	private Configuration.ConfigurationData Configuration { get; } = configuration;

	private Dictionary<UiElementViewModel, UiBlockerOverlayWindow> Overlays { get; } = [];

	public void ShowBlockers(IEnumerable<UiElementViewModel> elements)
	{
		if (GetBrowserControl(DockingManager) is not LayoutDocumentControl anchorableControl) return;
		if (GetBrowserWindow(anchorableControl) is not System.Windows.Window browserWindow) return;

		(double zoomRate, double browserTop, double browserLeft) = GetBrowserData(anchorableControl);

		foreach (UiElementViewModel elementViewModel in elements)
		{
			// Recreate the overlay when the browser moves to a different host window.
			UiBlockerOverlayWindow overlayWindow = GetOrCreateOverlay(elementViewModel, browserWindow);
			overlayWindow.ViewModel.UpdateBrowserData(zoomRate, browserTop, browserLeft);

			overlayWindow.Show();
		}
	}

	private (double ZoomRate, double BrowserTop, double BrowserLeft) GetBrowserData(
		LayoutDocumentControl anchorableControl)
	{
		DpiScale dpi = VisualTreeHelper.GetDpi(Application.Current.MainWindow);

		Point topLeft = anchorableControl.PointToScreen(new Point(0, 0));
		Size anchorableSize = new(anchorableControl.ActualWidth, anchorableControl.ActualHeight);

		int toolbarHeight = CalculateToolbarHeight();
		int toolbarWidth = CalculateToolbarWidth();
		int toolbarTopOffset = CalculateToolbarTopOffset(toolbarHeight);
		int toolbarLeftOffset = CalculateToolbarLeftOffset(toolbarWidth);
		double zoomRate = CalculateZoomRate(anchorableSize, toolbarHeight, toolbarWidth);
		Size browserSize = new(KancolleSize.Width * zoomRate, KancolleSize.Height * zoomRate);
		Point browserPosition = CalculateBrowserPosition(anchorableSize, browserSize, toolbarHeight, toolbarWidth);

		double browserTop = topLeft.Y + browserPosition.Y + toolbarTopOffset * dpi.DpiScaleY;
		double browserLeft = topLeft.X + browserPosition.X + toolbarLeftOffset * dpi.DpiScaleX;

		return (zoomRate, browserTop, browserLeft);
	}

	public void HideBlockers(IEnumerable<UiElementViewModel> elements)
	{
		foreach (UiElementViewModel elementViewModel in elements)
		{
			if (Overlays.TryGetValue(elementViewModel, out UiBlockerOverlayWindow? overlayWindow))
			{
				overlayWindow.Hide();
			}
		}
	}

	public void HideAll()
	{
		foreach (UiBlockerOverlayWindow overlay in Overlays.Values)
		{
			overlay.Hide();
		}
	}

	private static LayoutAnchorable? GetBrowserAnchorable(DockingManager dockingManager)
		=> dockingManager.Layout
			.Descendents()
			.OfType<LayoutAnchorable>()
			.FirstOrDefault(a => a.ContentId == FormBrowserHostViewModel.BrowserContentId);

	private static LayoutDocumentControl? GetBrowserControl(DockingManager dockingManager)
	{
		if (GetBrowserAnchorable(dockingManager) is not LayoutAnchorable browserAnchorable) return null;

		LayoutDocumentControl? anchorableControl = dockingManager
			.GetVisualDescendents<LayoutDocumentControl>()
			.FirstOrDefault(c => c.Model == browserAnchorable);

		// need this when browser is floating
		anchorableControl ??= dockingManager
			.GetLayoutItemFromModel(browserAnchorable)
			?.View
			.TryFindParent<LayoutDocumentControl>();

		return anchorableControl;
	}

	private static System.Windows.Window? GetBrowserWindow(LayoutDocumentControl browserControl)
		=> System.Windows.Window.GetWindow(browserControl);

	private UiBlockerOverlayWindow GetOrCreateOverlay(UiElementViewModel uiElementViewModel,
		System.Windows.Window browserWindow)
	{
		if (!Overlays.TryGetValue(uiElementViewModel, out UiBlockerOverlayWindow? overlayWindow))
		{
			overlayWindow = CreateOverlay(uiElementViewModel, browserWindow);
			Overlays[uiElementViewModel] = overlayWindow;
			return overlayWindow;
		}

		if (overlayWindow.Owner == browserWindow) return overlayWindow;

		// Keep the overlay owned by the current browser host so it stays out of Alt+Tab.
		try
		{
			overlayWindow.Close();
		}
		catch (InvalidOperationException)
		{
			// The previous owner may already have closed the old overlay.
		}

		overlayWindow = CreateOverlay(uiElementViewModel, browserWindow);
		Overlays[uiElementViewModel] = overlayWindow;

		return overlayWindow;
	}

	private UiBlockerOverlayWindow CreateOverlay(UiElementViewModel uiElementViewModel,
		System.Windows.Window browserWindow)
	{
		UiBlockerOverlayViewModel overlayViewModel = new(uiElementViewModel);
		UiBlockerOverlayWindow overlayWindow = new(overlayViewModel) { Owner = browserWindow, };

		return overlayWindow;
	}

	private double CalculateZoomRate(Size anchorableSize, int toolbarHeight, int toolbarWidth)
		=> Configuration.FormBrowser.ZoomFit switch
		{
			true => Math.Min(
				(anchorableSize.Width - toolbarWidth) / KancolleSize.Width,
				(anchorableSize.Height - toolbarHeight) / KancolleSize.Height
			),

			_ => Configuration.FormBrowser.ZoomRate,
		};

	/// <summary>
	/// The position relative to the anchorable.
	/// </summary>
	private Point CalculateBrowserPosition(Size anchorableSize, Size browserSize, int toolbarHeight, int toolbarWidth)
	{
		double x = Math.Max(0, (anchorableSize.Width - browserSize.Width - toolbarWidth) / 2);
		double y = Math.Max(0, (anchorableSize.Height - browserSize.Height - toolbarHeight) / 2);

		return new(x, y);
	}

	private int CalculateToolbarHeight()
	{
		if (!Configuration.FormBrowser.IsToolMenuVisible) return 0;
		if (Configuration.FormBrowser.ToolMenuDockStyle is System.Windows.Forms.DockStyle.Left) return 0;
		if (Configuration.FormBrowser.ToolMenuDockStyle is System.Windows.Forms.DockStyle.Right) return 0;

		// todo: logic to get both toolbar height and docking postion and adjust the blocker accordingly
		int toolbarHeight = 24;

		return toolbarHeight;
	}

	private int CalculateToolbarWidth()
	{
		if (!Configuration.FormBrowser.IsToolMenuVisible) return 0;
		if (Configuration.FormBrowser.ToolMenuDockStyle is System.Windows.Forms.DockStyle.Top) return 0;
		if (Configuration.FormBrowser.ToolMenuDockStyle is System.Windows.Forms.DockStyle.Bottom) return 0;

		// todo: logic to get both toolbar width and docking postion and adjust the blocker accordingly
		int toolbarWidth = 38;

		return toolbarWidth;
	}

	private int CalculateToolbarTopOffset(int toolbarHeight)
		=> Configuration.FormBrowser.ToolMenuDockStyle switch
		{
			System.Windows.Forms.DockStyle.Top => toolbarHeight,
			_ => 0,
		};

	private int CalculateToolbarLeftOffset(int toolbarWidth)
		=> Configuration.FormBrowser.ToolMenuDockStyle switch
		{
			System.Windows.Forms.DockStyle.Left => toolbarWidth,
			_ => 0,
		};
}
