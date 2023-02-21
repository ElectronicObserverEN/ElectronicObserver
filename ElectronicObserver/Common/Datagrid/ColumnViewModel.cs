using System.ComponentModel;
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
	private ListSortDirection? sortDirection;

	[ObservableProperty]
	private bool visible = true;

	[ObservableProperty]
	private string header = "";

	public bool HasSortMemberPath => !string.IsNullOrEmpty(ColumnProperties.SortMemberPath);


	public ColumnProperties ColumnProperties { get; set; }
	public SortDescription? SortDescription { get; set; }

	public ColumnViewModel(ColumnProperties properties, SortDescription? sortDescription)
	{
		ColumnProperties = properties;
		SortDescription = sortDescription;

		Visible = properties.Visibility is Visibility.Visible;
		Header = properties.Header;

		SortDirection = sortDescription switch
		{
			SortDescription direction => direction.Direction,
			_ => null
		};
	}

	public void SaveChanges()
	{
		ColumnProperties.Visibility = Visible ? Visibility.Visible : Visibility.Collapsed;

		SortDescription = SortDirection switch
		{
			ListSortDirection direction => new()
			{
				Direction = direction,
				PropertyName = ColumnProperties.SortMemberPath
			},
			_ => null
		};
	}
}
