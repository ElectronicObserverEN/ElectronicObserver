using System.Threading.Tasks;
using ElectronicObserver.Avalonia.Translation;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public class SoftwareUpdaterService : ISoftwareUpdaterService
{
	/// <inheritdoc />
	public Task DownloadData(string filename, DataType type)
	{
		return Task.CompletedTask;
	}
}
