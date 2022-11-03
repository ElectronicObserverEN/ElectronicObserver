namespace ElectronicObserver.Window.Tools.DatabaseExplorer;

/// <summary>
/// Interaction logic for DatabaseExplorerWindow.xaml
/// </summary>
public partial class DatabaseExplorerWindow
{
	public DatabaseExplorerWindow() : base(new DatabaseExplorerViewModel())
	{
		InitializeComponent();
	}
}
