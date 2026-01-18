using System;
using System.Runtime.InteropServices;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

[StructLayout(LayoutKind.Sequential)]
public struct MsllHookStruct
{
	public RawPoint Point;
	public uint MouseData;
	public uint Flags;
	public uint Time;
	public IntPtr DwExtraInfo;
}
