using System;
using System.Runtime.InteropServices;

namespace Avalonia.Win32.Interoperability;

internal static partial class UnmanagedMethods
{
	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetParent(IntPtr hWnd, IntPtr hWndNewParent);

	[LibraryImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
	public static partial uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
}
