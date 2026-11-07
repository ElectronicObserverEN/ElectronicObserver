using System;
using ElectronicObserver.Avalonia.Translation;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public class EoLogger : IEoLogger
{
	/// <inheritdoc />
	public void Add(int level, string msg)
	{
	}

	/// <inheritdoc />
	public void SendErrorReport(Exception ex, string message, string? connectionName = null, string? connectionData = null)
	{
	}
}
