using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace BrowserLibCore;

public static partial class ProcessExtensions
{
	private const int ProcessCommandLineInformation = 60;

	[LibraryImport("ntdll.dll")]
	internal static partial int NtQueryInformationProcess(
		IntPtr processHandle,
		int processInformationClass,
		IntPtr processInformation,
		int processInformationLength,
		out int returnLength);

	public static string? GetCommandLine(this Process process)
	{
		// First call gets required buffer size
		NtQueryInformationProcess(process.Handle, ProcessCommandLineInformation, IntPtr.Zero, 0, out int size);

		if (size is 0) return null;

		IntPtr ptr = Marshal.AllocHGlobal(size);

		try
		{
			int status = NtQueryInformationProcess(process.Handle, ProcessCommandLineInformation, ptr, size, out _);

			if (status is not 0) return null;

			// UNICODE_STRING structure:
			// ushort Length
			// ushort MaximumLength
			// IntPtr Buffer
			int length = Marshal.ReadInt16(ptr);
			IntPtr buffer = Marshal.ReadIntPtr(ptr, 8);

			byte[] data = new byte[length];
			Marshal.Copy(buffer, data, 0, length);

			return Encoding.Unicode.GetString(data);
		}
		finally
		{
			Marshal.FreeHGlobal(ptr);
		}
	}
}
