using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class ColumnViewModel : ObservableObject
{
	[ObservableProperty]
	private DataGridLength width = DataGridLength.Auto;

	[ObservableProperty]
	private int displayIndex;

	[ObservableProperty]
	private bool visible = true;

	[ObservableProperty]
	private string header = "";

	public bool HasSortMemberPath => !string.IsNullOrEmpty(ColumnProperties.SortMemberPath);


	public ColumnProperties ColumnProperties { get; set; }

	public ColumnViewModel(ColumnProperties properties)
	{
		ColumnProperties = properties;

		Visible = properties.Visibility is Visibility.Visible;
		Header = properties.Header;
	}

	public void SaveChanges()
	{
		ColumnProperties.Visibility = Visible ? Visibility.Visible : Visibility.Collapsed;
	}
}
