namespace ElectronicObserver.Avalonia.Translation;

public interface IEoLogger
{
	void Add(int level, string msg);
	void SendErrorReport(Exception ex, string message, string? connectionName = null, string? connectionData = null);
}
