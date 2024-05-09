using System.Diagnostics;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

// hack: no idea how to do this without reflection hacks
#pragma warning disable S3011

namespace ElectronicObserver.Avalonia.Behaviors;

public class ColumnAutoSizeBehavior : Behavior<DataGrid>
{
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

		try
		{
			foreach (DataGridColumnHeader header in GetColumnHeaders(AssociatedObject))
			{
				header.DoubleTapped += AutoSizeColumn;
			}
		}
		catch
		{
			// I don't trust reflection
		}
	}

	private void DataGridUnloaded(object? sender, RoutedEventArgs e)
	{
		if (AssociatedObject is null)
		{
			return;
		}

		AssociatedObject.Loaded -= DataGridLoaded;
		AssociatedObject.Unloaded -= DataGridUnloaded;

		try
		{
			foreach (DataGridColumnHeader header in GetColumnHeaders(AssociatedObject))
			{
				header.DoubleTapped -= AutoSizeColumn;
			}
		}
		catch
		{
			// I don't trust reflection
		}
	}

	// hack: don't do this
	private static IEnumerable<DataGridColumnHeader> GetColumnHeaders(DataGrid dataGrid) => dataGrid.Columns
		.Select(c => c
			.GetType()
			.GetProperty("HeaderCell", BindingFlags.NonPublic | BindingFlags.Instance)
			?.GetValue(c))
		.OfType<DataGridColumnHeader>();

	// hack: don't do this
	private static void AutoSizeColumn(object? sender, TappedEventArgs e)
	{
		if (sender is not DataGridColumnHeader header) return;

		// only allow auto sizing when Avalonia displays the resize cursor
		if (header.Cursor?.ToString() != StandardCursorType.SizeWestEast.ToString()) return;

		try
		{
			object? owningColumn = header
				.GetType()
				.GetProperty("OwningColumn", BindingFlags.NonPublic | BindingFlags.Instance)
				?.GetValue(header);

			if (owningColumn is not DataGridColumn column) return;

			column.Width = new(column.Width.Value, DataGridLengthUnitType.Auto);
		}
		catch
		{
			// I don't trust reflection
		}
	}
}
