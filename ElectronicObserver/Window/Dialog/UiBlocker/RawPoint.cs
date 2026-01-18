using System.Runtime.InteropServices;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

[StructLayout(LayoutKind.Sequential)]
public struct RawPoint
{
	public int X;
	public int Y;
}
