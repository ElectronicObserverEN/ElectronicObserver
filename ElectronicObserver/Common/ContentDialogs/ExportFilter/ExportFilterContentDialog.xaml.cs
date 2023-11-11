namespace ElectronicObserver.Common.ContentDialogs.ExportFilter;

/// <summary>
/// Interaction logic for ExportFilterContentDialog.xaml
/// </summary>
public partial class ExportFilterContentDialog
{
	private ExportFilterViewModel _exportFilter;

	public ExportFilterViewModel ExportFilter
	{
		get => _exportFilter;
		set
		{
			_exportFilter = value;
			DataContext = ExportFilter;
		}
	}

	public ExportFilterContentDialog()
	{
		InitializeComponent();
	}
}
