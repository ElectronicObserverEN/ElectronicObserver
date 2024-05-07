﻿using System.Collections.ObjectModel;
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

	public static readonly StyledProperty<ObservableCollection<ColumnModel>> ColumnPropertiesProperty =
		AvaloniaProperty.Register<PersistentColumnsBehavior, ObservableCollection<ColumnModel>>
		(
			nameof(ColumnProperties),
			[],
			false,
			BindingMode.TwoWay
		);

	public ObservableCollection<ColumnModel> ColumnProperties
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

	private static void ColumnPropertiesChangedCallback(AvaloniaPropertyChangedEventArgs<ObservableCollection<ColumnModel>> obj)
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
			foreach ((DataGridColumn column, ColumnModel columnProperties) in AssociatedObject.Columns.Zip(ColumnProperties))
			{
				column.Width = columnProperties.Width;
				column.DisplayIndex = columnProperties.DisplayIndex;
				column.IsVisible = columnProperties.IsVisible;

				columnProperties.Header = GetColumnHeader(column);
				columnProperties.SortMemberPath = column.SortMemberPath;
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

		AddMissingColumnData(AssociatedObject, ColumnProperties);

		foreach ((DataGridColumn column, ColumnModel columnModel) in AssociatedObject.Columns.Zip(ColumnProperties))
		{
			columnModel.Width = column.Width;
			columnModel.DisplayIndex = column.DisplayIndex switch
			{
				-1 => AssociatedObject.Columns.IndexOf(column),
				int i => i,
			};
			columnModel.IsVisible = column.IsVisible;
			columnModel.Header = GetColumnHeader(column);
			columnModel.SortMemberPath = column.SortMemberPath;
		}

		UpdatingColumnInfo = false;
	}

	private void ColumnPropertiesChanged(ObservableCollection<ColumnModel> columnModels)
	{
		if (AssociatedObject is null) return;
		if (!WasGridInitialized()) return;

		UpdatingColumnInfo = true;

		AddMissingColumnData(AssociatedObject, columnModels);

		foreach ((DataGridColumn column, ColumnModel columnModel) in AssociatedObject.Columns.Zip(columnModels))
		{
			column.Width = columnModel.Width;
			column.DisplayIndex = columnModel.DisplayIndex;
			column.IsVisible = columnModel.IsVisible;

			columnModel.Header = GetColumnHeader(column);
		}

		UpdatingColumnInfo = false;
	}

	private static void AddMissingColumnData(DataGrid associatedObject, ObservableCollection<ColumnModel> columnProperties)
	{
		if (associatedObject.Columns.Count <= columnProperties.Count) return;

		foreach (DataGridColumn column in associatedObject.Columns.Skip(columnProperties.Count))
		{
			columnProperties.Add(new()
			{
				Name = GetColumnHeader(column),
				Width = column.Width,
				DisplayIndex = column.DisplayIndex,
				Header = GetColumnHeader(column),
				SortMemberPath = column.SortMemberPath,
				IsVisible = true,
			});
		}
	}

	private static string GetColumnHeader(DataGridColumn column) => column.Header switch
	{
		string stringHeader => stringHeader,
		DataGridColumnHeader header => header.Content?.ToString() ?? "",
		_ => "",
	};
}
