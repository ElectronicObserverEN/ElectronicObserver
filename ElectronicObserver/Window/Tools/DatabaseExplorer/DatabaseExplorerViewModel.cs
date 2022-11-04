using ElectronicObserver.Common;
using ElectronicObserver.Window.Tools.DatabaseExplorer.ApiFile;
using ElectronicObserver.Window.Tools.DatabaseExplorer.Sortie;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer;

public partial class DatabaseExplorerViewModel : WindowViewModelBase
{
	public ApiFileExplorerViewModel ApiFileExplorer { get; } = new();
	public SortieExplorerViewModel SortieExplorer { get; } = new();
}
