using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using ElectronicObserver.Behaviors.PersistentColumns;

namespace ElectronicObserver.Common.Datagrid;

public partial class ColumnViewModel : ObservableObject
{
	public ColumnProperties? ColumnProperties { get; }
	public ColumnModel? ColumnModel { get; }

	[ObservableProperty] private DataGridLength _width = DataGridLength.Auto;
	[ObservableProperty] private int _displayIndex;
	[ObservableProperty] private bool _isVisible = true;
	[ObservableProperty] private string _header = "";

	public ColumnViewModel(ColumnProperties properties)
	{
		ColumnProperties = properties;

		IsVisible = properties.Visibility is Visibility.Visible;
		Header = properties.Header;
	}

	public ColumnViewModel(ColumnModel columnModel)
	{
		ColumnModel = columnModel;

		IsVisible = columnModel.IsVisible;
		Header = columnModel.Header;
	}

	public void SaveChanges()
	{
		if (ColumnProperties is not null)
		{
			ColumnProperties.Visibility = IsVisible switch
			{
				true => Visibility.Visible,
				_ => Visibility.Collapsed,
			};
		}

		if (ColumnModel is not null)
		{
			ColumnModel.IsVisible = IsVisible;
		}
	}
}
