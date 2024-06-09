using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace ElectronicObserver.Avalonia.Behaviors;

public class MultiSelectBehavior : Behavior<DataGrid>
{
	private bool IsDragging { get; set; }

	protected override void OnAttached()
	{
		base.OnAttached();

		Debug.Assert(AssociatedObject is not null);

		AssociatedObject.Loaded += DataGridLoaded;
		AssociatedObject.Unloaded += DataGridUnloaded;
	}

	private void DataGridLoaded(object? sender, RoutedEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		AssociatedObject.LoadingRow += DataGrid_LoadingRow;
		AssociatedObject.UnloadingRow += DataGrid_UnloadingRow;
		AssociatedObject.PointerMoved += DataGrid_PointerMoved;
		AssociatedObject.PointerReleased += DataGrid_PointerReleased;
	}

	private void DataGridUnloaded(object? sender, RoutedEventArgs e)
	{
		if (AssociatedObject is null) return;

		AssociatedObject.Loaded -= DataGridLoaded;
		AssociatedObject.Unloaded -= DataGridUnloaded;

		AssociatedObject.LoadingRow -= DataGrid_LoadingRow;
		AssociatedObject.UnloadingRow -= DataGrid_UnloadingRow;
		AssociatedObject.PointerMoved -= DataGrid_PointerMoved;
		AssociatedObject.PointerReleased -= DataGrid_PointerReleased;
	}

	private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
	{
		e.Row.AddHandler(InputElement.PointerPressedEvent, Row_PointerPressed, RoutingStrategies.Tunnel);
	}

	private void DataGrid_UnloadingRow(object? sender, DataGridRowEventArgs e)
	{
		e.Row.RemoveHandler(InputElement.PointerPressedEvent, Row_PointerPressed);
	}

	private void Row_PointerPressed(object? sender, PointerPressedEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		if (sender is not DataGridRow row) return;

		IsDragging = true;

		AssociatedObject.SelectedItems.Clear();
		AssociatedObject.SelectedItems.Add(row.DataContext);
	}

	private void DataGrid_PointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		IsDragging = false;
	}

	private void DataGrid_PointerMoved(object? sender, PointerEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		if (!IsDragging) return;

		DataGridRow? row = GetDataGridRowUnderPointer(e);

		if (row is null) return;

		ScrollIfNeeded(AssociatedObject, e, row);

		if (AssociatedObject.SelectedItems.Contains(row.DataContext)) return;

		AssociatedObject.SelectedItems.Add(row.DataContext);
	}

	private static void ScrollIfNeeded(DataGrid dataGrid, PointerEventArgs e, DataGridRow row)
	{
		Point mousePosition = e.GetPosition(dataGrid);

		double scrollMargin = dataGrid.RowHeight switch
		{
			double.NaN => 32,
			_ => dataGrid.RowHeight,
		};

		double columnHeaderHeight = dataGrid.ColumnHeaderHeight switch
		{
			double.NaN => 32,
			_ => dataGrid.ColumnHeaderHeight,
		};

		if (mousePosition.Y < scrollMargin + columnHeaderHeight)
		{
			// scroll up
			ScrollIntoView(dataGrid, row.DataContext, -1);
		}
		else if (mousePosition.Y > dataGrid.Bounds.Height - scrollMargin)
		{
			// scroll down
			ScrollIntoView(dataGrid, row.DataContext, 1);
		}
	}

	private static void ScrollIntoView(DataGrid dataGrid, object? item, int scrollAmount)
	{
		int index = dataGrid.ItemsSource
			.OfType<object>()
			.TakeWhile(i => i != item)
			.Count();

		object? nextItem = dataGrid.ItemsSource
			.OfType<object>()
			.Skip(index + scrollAmount)
			.FirstOrDefault();

		dataGrid.ScrollIntoView(nextItem, null);
	}

	private DataGridRow? GetDataGridRowUnderPointer(PointerEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		Point point = e.GetPosition(AssociatedObject);
		Visual? hit = AssociatedObject.InputHitTest(point) as Visual;

		return hit.FindAncestorOfType<DataGridRow>();
	}
}
