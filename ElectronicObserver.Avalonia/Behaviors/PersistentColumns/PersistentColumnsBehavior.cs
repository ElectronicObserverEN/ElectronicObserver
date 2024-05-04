using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace ElectronicObserver.Avalonia.Behaviors.PersistentColumns;

public class PersistentColumnsBehavior : Behavior<DataGrid>
{
	private bool UpdatingColumnInfo { get; set; }
	private bool InWidthChange { get; set; }

	public static readonly StyledProperty<List<ColumnModel>> ColumnPropertiesProperty =
		AvaloniaProperty.Register<PersistentColumnsBehavior, List<ColumnModel>>
		(
			nameof(ColumnProperties),
			[],
			false,
			BindingMode.TwoWay
		);

	public List<ColumnModel> ColumnProperties
	{
		get => GetValue(ColumnPropertiesProperty);
		set => SetValue(ColumnPropertiesProperty, value);
	}

	public PersistentColumnsBehavior()
	{
		ColumnPropertiesProperty.Changed.Subscribe(ColumnPropertiesChangedCallback);
	}

	private void DisplayIndexChangedHandler(object? sender, EventArgs x) => UpdateColumnInfo();
	private void WidthPropertyChangedHandler(object? sender, EventArgs x) => InWidthChange = true;
	private void VisibilityPropertyChangedHandler(object? sender, EventArgs x) => UpdateColumnInfo();

	private static void ColumnPropertiesChangedCallback(AvaloniaPropertyChangedEventArgs<List<ColumnModel>> obj)
	{
		if (obj.Sender is PersistentColumnsBehavior { UpdatingColumnInfo: false } behavior)
		{
			behavior.ColumnPropertiesChanged(obj.NewValue.Value);
		}
	}

	protected override void OnAttached()
	{
		base.OnAttached();

		Debug.Assert(AssociatedObject is not null);

		AssociatedObject.Loaded += DataGridLoaded;
		AssociatedObject.Unloaded += DataGridUnloaded;
		AssociatedObject.ColumnReordered += UpdateColumnInfo;
	}

	/// <summary>
	/// If the grid was never initialized before, DisplayIndex values can be -1
	/// causing an exception.
	/// </summary>
	/// <returns></returns>
	private bool WasGridInitialized()
	{
		return ColumnProperties.All(c => c.DisplayIndex >= 0);
	}

	private void DataGridLoaded(object? sender, RoutedEventArgs e)
	{
		Debug.Assert(AssociatedObject is not null);

		if (WasGridInitialized())
		{
			foreach ((DataGridColumn? dataGridColumn, ColumnModel? columnProperties) in AssociatedObject.Columns.Zip(ColumnProperties))
			{
				dataGridColumn.Width = columnProperties.Width;
				dataGridColumn.DisplayIndex = columnProperties.DisplayIndex;
				dataGridColumn.IsVisible = columnProperties.IsVisible;

				columnProperties.Header = dataGridColumn.Header switch
				{
					string stringHeader => stringHeader,
					DataGridColumnHeader header => header.Content?.ToString() ?? "",
					_ => "",
				};

				columnProperties.SortMemberPath = dataGridColumn.SortMemberPath;
			}
		}

		foreach (DataGridColumn? column in AssociatedObject.Columns)
		{
			column.PropertyChanged += DisplayIndexChangedHandler;
			column.PropertyChanged += WidthPropertyChangedHandler;
			column.PropertyChanged += VisibilityPropertyChangedHandler;
		}

		UpdateColumnInfo();
	}

	private void DataGridUnloaded(object? sender, RoutedEventArgs e)
	{
		if (AssociatedObject is null)
		{
			return;
		}

		foreach (DataGridColumn? column in AssociatedObject.Columns)
		{
			column.PropertyChanged -= DisplayIndexChangedHandler;
			column.PropertyChanged -= WidthPropertyChangedHandler;
			column.PropertyChanged -= VisibilityPropertyChangedHandler;
		}
	}

	private void UpdateColumnInfo(object? sender, DataGridColumnEventArgs dataGridColumnEventArgs)
	{
		if (!InWidthChange) return;

		InWidthChange = false;
		UpdateColumnInfo();
	}

	private void UpdateColumnInfo()
	{
		Debug.Assert(AssociatedObject is not null);

		if (UpdatingColumnInfo)
		{
			return;
		}

		UpdatingColumnInfo = true;

		if (AssociatedObject.Columns.Count > ColumnProperties.Count)
		{
			ColumnProperties.AddRange(Enumerable
				.Range(0, AssociatedObject.Columns.Count - ColumnProperties.Count)
				.Select(i => new ColumnModel
				{
					Name = $"UnknownColumn{ColumnProperties.Count + i}",
				}));
		}

		foreach ((DataGridColumn column, ColumnModel columnModel) in AssociatedObject.Columns.Zip(ColumnProperties))
		{
			columnModel.Width = column.Width;
			columnModel.DisplayIndex = column.DisplayIndex switch
			{
				-1 => AssociatedObject.Columns.IndexOf(column),
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable Roslyn.S2589
				int i => i,
#pragma warning restore Roslyn.S2589
#pragma warning restore IDE0079 // Remove unnecessary suppression
			};
			columnModel.IsVisible = column.IsVisible;
			columnModel.Header = column.Header switch
			{
				string stringHeader => stringHeader,
				DataGridColumnHeader header => header.Content?.ToString() ?? "",
				_ => "",
			};
			columnModel.SortMemberPath = column.SortMemberPath;
		}

		UpdatingColumnInfo = false;
	}

	private void ColumnPropertiesChanged(List<ColumnModel> columnModels)
	{
		if (AssociatedObject is null) return;
		if (!WasGridInitialized()) return;

		UpdatingColumnInfo = true;

		foreach ((DataGridColumn? column, ColumnModel? columnModel) in AssociatedObject.Columns.Zip(columnModels))
		{
			column.Width = columnModel.Width;
			column.DisplayIndex = columnModel.DisplayIndex;
			column.IsVisible = columnModel.IsVisible;
		}

		UpdatingColumnInfo = false;
	}
}
