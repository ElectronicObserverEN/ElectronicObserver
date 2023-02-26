﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace ElectronicObserver.Behaviors.PersistentColumns;

public class PersistentColumnsBehavior : Behavior<DataGrid>
{
	private bool UpdatingColumnInfo { get; set; }
	private bool InWidthChange { get; set; }

	private DependencyPropertyDescriptor? SortDirectionPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(DataGridColumn.SortDirectionProperty, typeof(DataGridColumn));
	private DependencyPropertyDescriptor? DisplayIndexPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(DataGridColumn.DisplayIndexProperty, typeof(DataGridColumn));
	private DependencyPropertyDescriptor? WidthPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(DataGridColumn.WidthProperty, typeof(DataGridColumn));
	private DependencyPropertyDescriptor? VisibilityPropertyDescriptor => DependencyPropertyDescriptor.FromProperty(DataGridColumn.VisibilityProperty, typeof(DataGridColumn));

	private void SortDirectionChangedHandler(object? sender, EventArgs x) => UpdateColumnInfo();
	private void DisplayIndexChangedHandler(object? sender, EventArgs x) => UpdateColumnInfo();
	private void WidthPropertyChangedHandler(object? sender, EventArgs x) => InWidthChange = true;
	private void VisibilityPropertyChangedHandler(object? sender, EventArgs x) => UpdateColumnInfo();

	#region ColumnProperties

	public static readonly DependencyProperty ColumnPropertiesProperty = DependencyProperty.Register
	(
		nameof(ColumnProperties),
		typeof(List<ColumnProperties>),
		typeof(PersistentColumnsBehavior),
		new FrameworkPropertyMetadata
		(
			default(List<ColumnProperties>),
			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			ColumnPropertiesChangedCallback
		)
	);

	public List<ColumnProperties> ColumnProperties
	{
		get => (List<ColumnProperties>)GetValue(ColumnPropertiesProperty);
		set => SetValue(ColumnPropertiesProperty, value);
	}

	private static void ColumnPropertiesChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		if (dependencyObject is not PersistentColumnsBehavior { UpdatingColumnInfo: false } behavior) return;

		behavior.ColumnPropertiesChanged();
	}

	#endregion

	#region SortDescriptions

	public static readonly DependencyProperty SortDescriptionsProperty = DependencyProperty.Register
	(
		nameof(SortDescriptions),
		typeof(List<SortDescription>),
		typeof(PersistentColumnsBehavior),
		new FrameworkPropertyMetadata
		(
			default(List<SortDescription>),
			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			SortDescriptionsChangedCallback
		)
	);

	public List<SortDescription> SortDescriptions
	{
		get => (List<SortDescription>)GetValue(SortDescriptionsProperty);
		set => SetValue(SortDescriptionsProperty, value);
	}

	private static void SortDescriptionsChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
	{
		if (dependencyObject is not PersistentColumnsBehavior { UpdatingColumnInfo: false } behavior) return;

		behavior.SortDescriptionsChanged();
	}

	#endregion

	protected override void OnAttached()
	{
		base.OnAttached();

		AssociatedObject.Loaded += DataGridLoaded;
		AssociatedObject.Unloaded += DataGridUnloaded;
		AssociatedObject.PreviewMouseLeftButtonUp += DataGridPreviewMouseLeftButtonUp;
	}

	private void DataGridLoaded(object sender, RoutedEventArgs e)
	{
		foreach ((DataGridColumn? dataGridColumn, ColumnProperties? columnProperties) in AssociatedObject.Columns.Zip(ColumnProperties))
		{
			dataGridColumn.Width = columnProperties.Width;
			dataGridColumn.DisplayIndex = columnProperties.DisplayIndex;
			dataGridColumn.SortDirection = columnProperties.SortDirection;
			dataGridColumn.Visibility = columnProperties.Visibility;

			columnProperties.Header = dataGridColumn.Header switch
			{
				string stringHeader => stringHeader,
				DataGridColumnHeader header => header.Content?.ToString() ?? "",
				_ => "",
			};

			columnProperties.SortMemberPath = dataGridColumn.SortMemberPath?.ToString() ?? "";
		}

		foreach (DataGridColumn? column in AssociatedObject.Columns)
		{
			SortDirectionPropertyDescriptor?.AddValueChanged(column, SortDirectionChangedHandler);
			DisplayIndexPropertyDescriptor?.AddValueChanged(column, DisplayIndexChangedHandler);
			WidthPropertyDescriptor?.AddValueChanged(column, WidthPropertyChangedHandler);
			VisibilityPropertyDescriptor?.AddValueChanged(column, VisibilityPropertyChangedHandler);
		}

		UpdateColumnInfo();
	}

	private void DataGridUnloaded(object sender, RoutedEventArgs e)
	{
		foreach (DataGridColumn? column in AssociatedObject.Columns)
		{
			SortDirectionPropertyDescriptor?.RemoveValueChanged(column, SortDirectionChangedHandler);
			DisplayIndexPropertyDescriptor?.RemoveValueChanged(column, DisplayIndexChangedHandler);
			WidthPropertyDescriptor?.RemoveValueChanged(column, WidthPropertyChangedHandler);
			VisibilityPropertyDescriptor?.RemoveValueChanged(column, VisibilityPropertyChangedHandler);
		}
	}

	private void DataGridPreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		if (!InWidthChange) return;

		InWidthChange = false;
		UpdateColumnInfo();
	}

	private void UpdateColumnInfo()
	{
		UpdatingColumnInfo = true;

		ColumnProperties = AssociatedObject.Columns.Select(c => new ColumnProperties
		{
			Width = c.Width,
			DisplayIndex = c.DisplayIndex,
			SortDirection = c.SortDirection,
			Visibility = c.Visibility,
			Header = c.Header switch
			{
				string stringHeader => stringHeader,
				DataGridColumnHeader header => header.Content?.ToString() ?? "",
				_ => "",
			},
			SortMemberPath = c.SortMemberPath?.ToString() ?? ""
		}).ToList();

		SortDescriptions = AssociatedObject.Items.SortDescriptions.ToList();

		UpdatingColumnInfo = false;
	}

	private void ColumnPropertiesChanged()
	{
		if (AssociatedObject is null) return;

		foreach ((DataGridColumn? dataGridColumn, ColumnProperties? columnProperties) in AssociatedObject.Columns.Zip(ColumnProperties))
		{
			dataGridColumn.Width = columnProperties.Width;
			dataGridColumn.DisplayIndex = columnProperties.DisplayIndex;
			dataGridColumn.SortDirection = columnProperties.SortDirection;
			dataGridColumn.Visibility = columnProperties.Visibility;
		}
	}

	private void SortDescriptionsChanged()
	{
		if (AssociatedObject is ICollectionView collection)
		{
			UpdateCollectionViewSortDescriptions(collection);
		}
		else if (AssociatedObject is not null)
		{
			UpdateDataGridSortDescriptions();
		}
	}

	private void UpdateDataGridSortDescriptions()
	{
		// need to save the new value cause SortDescriptions.Clear() will wipe it
		List<SortDescription> sortDescriptions = SortDescriptions;

		AssociatedObject.Items.SortDescriptions.Clear();

		foreach (DataGridColumn column in AssociatedObject.Columns)
		{
			column.SortDirection = null;
		}

		foreach (SortDescription sortDescription in sortDescriptions)
		{
			AssociatedObject.Items.SortDescriptions.Add(sortDescription);

			// update the sort description visuals on columns
			DataGridColumn? column = AssociatedObject.Columns
				.FirstOrDefault(c => c.SortMemberPath == sortDescription.PropertyName);

			if (column is null) continue;

			column.SortDirection = sortDescription.Direction;
		}
	}

	private void UpdateCollectionViewSortDescriptions(ICollectionView collection)
	{
		List<SortDescription> sortDescriptions = collection.SortDescriptions.ToList();

		collection.SortDescriptions.Clear();

		foreach (SortDescription sortDescription in sortDescriptions)
		{
			collection.SortDescriptions.Add(sortDescription);
		}
	}
}
