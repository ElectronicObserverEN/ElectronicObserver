using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using AvalonDock.Controls;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Settings.Window;
using Application = System.Windows.Application;

namespace ElectronicObserver;

/// <summary>
/// Interaction logic for FormMainWpf.xaml
/// </summary>
public partial class FormMainWpf : System.Windows.Window
{
	private NotifyIcon? TrayIcon { get; set; }
	private bool IsTrayMinimized { get; set; }
	private WindowState RestoreWindowState { get; set; } = WindowState.Normal;
	private Dictionary<LayoutFloatingWindowControl, WindowState> FloatingWindowStates { get; } = new();

	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FormMainViewModel), typeof(FormMainWpf), new PropertyMetadata(default(FormMainViewModel)));

	public FormMainViewModel ViewModel
	{
		get => (FormMainViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public WindowState EffectiveWindowStateForPersistence =>
		WindowState == WindowState.Minimized ? RestoreWindowState : WindowState;

	public FormMainWpf()
	{
		InitializeComponent();

		ViewModel = new(DockingManager, this);

		InitializeTrayIcon();
		StateChanged += MainWindow_StateChanged;
		Loaded += (sender, _) => ViewModel.LoadLayout(sender);
		Closed += (sender, _) => ViewModel.SaveLayout(sender);
		Closed += (_, _) => CleanupTrayIcon();
	}

	private void InitializeTrayIcon()
	{
		TrayIcon = new NotifyIcon
		{
			Text = "Electronic Observer",
			Visible = false,
		};

		System.Drawing.Icon? icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ResourceAssembly.Location);
		if (icon is not null)
		{
			TrayIcon.Icon = icon;
		}

		TrayIcon.DoubleClick += (_, _) => RestoreFromTray();

		ContextMenuStrip menu = new();
		menu.Items.Add(MainResources.Tray_Restore, null, (_, _) => RestoreFromTray());
		menu.Items.Add(MainResources.Tray_Exit, null, (_, _) => ExitFromTray());
		TrayIcon.ContextMenuStrip = menu;
	}

	private void MainWindow_StateChanged(object? sender, EventArgs e)
	{
		if (WindowState != WindowState.Minimized)
		{
			RememberRestoreWindowState();
			return;
		}

		if (GetConfiguredMinimizeBehavior() != MinimizeBehavior.SystemTray) return;

		MinimizeToTray();
	}

	private MinimizeBehavior GetConfiguredMinimizeBehavior() =>
		(MinimizeBehavior)Utility.Configuration.Config.Life.MinimizeBehavior;

	private void RememberRestoreWindowState()
	{
		if (WindowState == WindowState.Minimized) return;

		RestoreWindowState = WindowState;
	}

	private void MinimizeToTray()
	{
		if (IsTrayMinimized) return;

		MinimizeFloatingWindows();

		ShowInTaskbar = false;
		Hide();

		if (TrayIcon is not null)
		{
			TrayIcon.Visible = true;
		}

		IsTrayMinimized = true;
	}

	private void RestoreFromTray()
	{
		if (!IsTrayMinimized) return;

		ShowInTaskbar = true;
		Show();
		WindowState = RestoreWindowState;
		Activate();
		RestoreFloatingWindows();

		if (TrayIcon is not null)
		{
			TrayIcon.Visible = false;
		}

		IsTrayMinimized = false;
	}

	private void ExitFromTray()
	{
		Close();
	}

	private void MinimizeFloatingWindows()
	{
		FloatingWindowStates.Clear();

		foreach (LayoutFloatingWindowControl floatingWindow in DockingManager.FloatingWindows)
		{
			FloatingWindowStates[floatingWindow] = floatingWindow.WindowState;

			if (floatingWindow.WindowState != WindowState.Minimized)
			{
				floatingWindow.WindowState = WindowState.Minimized;
			}
		}
	}

	private void RestoreFloatingWindows()
	{
		foreach ((LayoutFloatingWindowControl floatingWindow, WindowState windowState) in FloatingWindowStates)
		{
			if (!floatingWindow.IsLoaded) continue;

			floatingWindow.WindowState = windowState;
		}

		FloatingWindowStates.Clear();
	}

	private void CleanupTrayIcon()
	{
		if (TrayIcon is null) return;

		TrayIcon.Visible = false;
		TrayIcon.Dispose();
		TrayIcon = null;
	}
}
