using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public sealed partial class MouseHook : IDisposable
{
	private const int WH_MOUSE_LL = 14;

	private IntPtr HookId { get; set; } = IntPtr.Zero;
	private LowLevelMouseProc Proc { get; }

	private Func<MouseMessage, RawPoint, bool> Filter { get; }

	public MouseHook(Func<MouseMessage, RawPoint, bool> filter)
	{
		Proc = HookCallback;
		Filter = filter;

		Install();
	}

	private void Install()
	{
		if (HookId != IntPtr.Zero) return;

		HookId = SetWindowsHookExW(WH_MOUSE_LL, Proc, IntPtr.Zero, 0);
	}

	private void Uninstall()
	{
		if (HookId == IntPtr.Zero) return;

		_ = UnhookWindowsHookEx(HookId);
		HookId = IntPtr.Zero;
	}

	public void Dispose()
	{
		Uninstall();
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

	[LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExW", StringMarshalling = StringMarshalling.Utf16)]
	private static partial IntPtr SetWindowsHookExW(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

	[LibraryImport("user32.dll", EntryPoint = "UnhookWindowsHookEx")]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool UnhookWindowsHookEx(IntPtr hhk);

	[LibraryImport("user32.dll")]
	[UnmanagedCallConv(CallConvs = [typeof(System.Runtime.CompilerServices.CallConvStdcall)])]
	private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

	private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
}
