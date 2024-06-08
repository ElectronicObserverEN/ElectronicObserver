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
		AssociatedObject.PointerMoved += DataGrid_PointerMoved;
		AssociatedObject.PointerReleased += DataGrid_PointerReleased;
	}

	private void DataGridUnloaded(object? sender, RoutedEventArgs e)
	{
		if (AssociatedObject is null) return;

		AssociatedObject.Loaded -= DataGridLoaded;
		AssociatedObject.Unloaded -= DataGridUnloaded;

		AssociatedObject.LoadingRow -= DataGrid_LoadingRow;
		AssociatedObject.PointerMoved -= DataGrid_PointerMoved;
		AssociatedObject.PointerReleased -= DataGrid_PointerReleased;
	}

	private void DataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
	{
		e.Row.AddHandler(InputElement.PointerPressedEvent, Row_PointerPressed, RoutingStrategies.Tunnel);
	}

	private void Row_PointerPressed(object? sender, PointerPressedEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		if (sender is not DataGridRow row) return;

		IsDragging = true;
		AssociatedObject.SelectedItems.Clear();
		AssociatedObject.SelectedItems.Add(row.DataContext);
	}

	private void DataGrid_PointerMoved(object? sender, PointerEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		if (!IsDragging) return;

		DataGridRow? row = GetDataGridRowUnderPointer(e);

		if (row is null) return;
		if (AssociatedObject.SelectedItems.Contains(row.DataContext)) return;

		AssociatedObject.SelectedItems.Add(row.DataContext);
	}

	private void DataGrid_PointerReleased(object? sender, PointerReleasedEventArgs e)
	{
		IsDragging = false;
	}

	private DataGridRow? GetDataGridRowUnderPointer(PointerEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		Point point = e.GetPosition(AssociatedObject);
		Visual? hit = AssociatedObject.InputHitTest(point) as Visual;

		return hit.FindAncestorOfType<DataGridRow>();
	}
}
