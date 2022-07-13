using System;
using System.Windows;
using System.Windows.Interop;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver;

/// <summary>
/// Interaction logic for FormMainWpf.xaml
/// </summary>
public partial class FormMainWpf : System.Windows.Window
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(FormMainViewModel), typeof(FormMainWpf), new PropertyMetadata(default(FormMainViewModel)));

	public FormMainViewModel ViewModel
	{
		get => (FormMainViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public FormMainWpf()
	{
		InitializeComponent();

		ViewModel = new(DockingManager, this);

		Loaded += (sender, _) => ViewModel.LoadLayout(sender);
		Closed += (sender, _) => ViewModel.SaveLayout(sender);
		SourceInitialized += FormMainWpf_SourceInitialized;
	}

	private void FormMainWpf_SourceInitialized(object? sender, System.EventArgs e)
	{
		WindowInteropHelper helper = new WindowInteropHelper(this);
		HwndSource source = HwndSource.FromHwnd(helper.Handle);
		source.AddHook(WndProc);
	}

	const int WM_SYSCOMMAND = 0x0112;
	const int SC_MOVE = 0xF010;

	private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	{

		switch (msg)
		{
			case WM_SYSCOMMAND:
				int command = wParam.ToInt32() & 0xfff0;
				if (command == SC_MOVE)
				{
					var c = Configuration.Config;
					if (c.Life.LockLayout)
						handled = true;
				}
				break;
			default:
				break;
		}
		return IntPtr.Zero;
	}
}
