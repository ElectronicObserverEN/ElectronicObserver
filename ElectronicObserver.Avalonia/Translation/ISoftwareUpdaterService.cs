namespace ElectronicObserver.Avalonia.Translation;

public interface ISoftwareUpdaterService
{
	Task DownloadData(string filename, DataType type);
}
