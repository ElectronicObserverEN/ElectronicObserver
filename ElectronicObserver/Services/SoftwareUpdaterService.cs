using System.Threading.Tasks;
using ElectronicObserver.Avalonia.Translation;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Services;

public class SoftwareUpdaterService : ISoftwareUpdaterService
{
	public Task DownloadData(string filename, DataType type) => SoftwareUpdater.DownloadData(filename, type);
}
