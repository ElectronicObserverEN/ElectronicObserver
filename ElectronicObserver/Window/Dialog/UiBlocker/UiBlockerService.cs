using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using GongSolutions.Wpf.DragDrop.Utilities;
using MahApps.Metro.Controls;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class UiBlockerService(DockingManager dockingManager, Configuration.ConfigurationData configuration)
{
	private static Size KancolleSize { get; } = new(1200, 720);

	private DockingManager DockingManager { get; } = dockingManager;
	private Configuration.ConfigurationData Configuration { get; } = configuration;

	private Dictionary<UiBlockerViewModel, UiBlockerOverlayWindow> Overlays { get; } = [];

	public void ShowBlocker(UiBlockerViewModel uiBlockerViewModel)
	{
		if (GetBrowserControl(DockingManager) is not LayoutDocumentControl anchorableControl) return;

		Point topLeft = anchorableControl.PointToScreen(new Point(0, 0));
		Size anchorableSize = new(anchorableControl.ActualWidth, anchorableControl.ActualHeight);

		int toolbarHeight = CalculateToolbarHeight();
		int toolbarWidth = CalculateToolbarWidth();
		int toolbarTopOffset = CalculateToolbarTopOffset(toolbarHeight);
		int toolbarLeftOffset = CalculateToolbarLeftOffset(toolbarWidth);
		double zoomRate = CalculateZoomRate(anchorableSize, toolbarHeight, toolbarWidth);
		Size browserSize = new(KancolleSize.Width * zoomRate, KancolleSize.Height * zoomRate);
		Point browserPosition = CalculateBrowserPosition(anchorableSize, browserSize, toolbarHeight, toolbarWidth);

		double browserTop = topLeft.Y + browserPosition.Y + toolbarTopOffset;
		double browserLeft = topLeft.X + browserPosition.X + toolbarLeftOffset;

		uiBlockerViewModel.UpdateBrowserData(zoomRate, browserTop, browserLeft);

		if (!Overlays.TryGetValue(uiBlockerViewModel, out UiBlockerOverlayWindow? overlayWindow))
		{
			overlayWindow = new(uiBlockerViewModel);
			Overlays[uiBlockerViewModel] = overlayWindow;
		}

		overlayWindow.Show();
	}

	public void HideBlocker(UiBlockerViewModel uiBlockerViewModel)
	{
		if (Overlays.TryGetValue(uiBlockerViewModel, out UiBlockerOverlayWindow? overlayWindow))
		{
			overlayWindow.Hide();
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
