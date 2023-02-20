using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common.Datagrid;

public partial class ColumnViewModel : ObservableObject
{
	[ObservableProperty]
	private DataGridLength width = DataGridLength.Auto;

	[ObservableProperty]
	private int displayIndex;

	[ObservableProperty]
	private ListSortDirection? sortDirection;

	[ObservableProperty]
	private Visibility visibility = Visibility.Visible;

	[ObservableProperty]
	private string header = "";

	public ColumnViewModel()
	{
		PropertyChanged += ColumnViewModel_PropertyChanged;
	}

	private void ColumnViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{

	}
}
