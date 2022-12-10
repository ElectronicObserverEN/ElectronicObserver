using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.DatabaseExplorer.ApiFile;
using ElectronicObserver.Window.Tools.DatabaseExplorer.ApiTypeTester;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer;

public class DatabaseExplorerViewModel : WindowViewModelBase
{
	public ApiFileExplorerViewModel ApiFileExplorer { get; } = new();
	public ApiTypeTesterViewModel ApiTypeTester { get; } = new();
}
