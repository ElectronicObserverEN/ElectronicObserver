using System.Diagnostics;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace ElectronicObserver.Avalonia.Behaviors;

public class DataGridRowTappedBehavior : Behavior<DataGrid>
{
	/// <summary>
	/// On tapped, this command gets invoked with the DataGridRow DataContext value.
	/// </summary>
	public static readonly StyledProperty<ICommand?> RowTappedCommandProperty =
		AvaloniaProperty.Register<DataGridRowTappedBehavior, ICommand?>(nameof(RowTappedCommand));

	private ICommand? RowTappedCommand
	{
		get => GetValue(RowTappedCommandProperty);
		set => SetValue(RowTappedCommandProperty, value);
	}

	/// <inheritdoc />
	protected override void OnAttached()
	{
		base.OnAttached();

		Debug.Assert(AssociatedObject is not null);

		AssociatedObject.LoadingRow += DataGrid_LoadingRow;
		AssociatedObject.UnloadingRow += DataGrid_UnloadingRow;
	}

	/// <inheritdoc />
	protected override void OnDetaching()
	{
		base.OnDetaching();

		Debug.Assert(AssociatedObject is not null);

		AssociatedObject.LoadingRow -= DataGrid_LoadingRow;
		AssociatedObject.UnloadingRow -= DataGrid_UnloadingRow;
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

		PointerPoint point = e.GetCurrentPoint(row);

		if (!point.Properties.IsLeftButtonPressed) return;

		RowTappedCommand?.Execute(row.DataContext);
	}
}
