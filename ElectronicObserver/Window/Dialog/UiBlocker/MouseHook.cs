using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class MouseHook : IDisposable
{
	private const int WH_MOUSE_LL = 14;

	private IntPtr HookId { get; set; } = IntPtr.Zero;
	private LowLevelMouseProc Proc { get; }

	public Func<MouseMessage, RawPoint, bool> Filter { get; set; }

	public MouseHook(Func<MouseMessage, RawPoint, bool> filter)
	{
		Proc = HookCallback;
		Filter = filter;

		Install();
	}

	private void Install()
	{
		if (HookId != IntPtr.Zero) return;

		using Process curProcess = Process.GetCurrentProcess();
		using ProcessModule curModule = curProcess.MainModule!;

		HookId = SetWindowsHookEx(
			WH_MOUSE_LL,
			Proc,
			GetModuleHandle(curModule.ModuleName),
			0);
	}

	private void Uninstall()
	{
		if (HookId == IntPtr.Zero) return;

		UnhookWindowsHookEx(HookId);
		HookId = IntPtr.Zero;
	}

	public void Dispose()
	{
		Uninstall();
		GC.SuppressFinalize(this);
	}

	private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
	{
		if (nCode >= 0)
		{
			MouseMessage msg = (MouseMessage)wParam;
			MsllHookStruct mouseInfo = Marshal.PtrToStructure<MsllHookStruct>(lParam);

			bool swallow = Filter(msg, mouseInfo.Point);
			if (swallow)
			{
				// swallow event
				return 1;
			}
		}

		return CallNextHookEx(HookId, nCode, wParam, lParam);
	}

	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

	[DllImport("user32.dll")]
	private static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport("user32.dll")]
	private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	private static extern IntPtr GetModuleHandle(string lpModuleName);

	private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
}


