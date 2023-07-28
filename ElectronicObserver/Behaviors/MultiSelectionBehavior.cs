using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace ElectronicObserver.Behaviors;

/// <summary>
/// Behavior enables binding <see cref="MultiSelector.SelectedItems"/> to an existing instance of <see cref="IList"/>
/// </summary>
/// <remarks>
/// <see href="https://github.com/Insire/MvvmScarletToolkit/blob/master/src/MvvmScarletToolkit.Wpf/Behaviors/MultiSelectionBehavior.cs"/>
/// </remarks>
public sealed class MultiSelectionBehavior : Behavior<MultiSelector>
{
	public IList? SelectedItems
	{
		get => (IList?)GetValue(SelectedItemsProperty);
		set => SetValue(SelectedItemsProperty, value);
	}

	/// <summary>Identifies the <see cref="SelectedItems"/> dependency property.</summary>
	public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
	(
		nameof(SelectedItems),
		typeof(IList),
		typeof(MultiSelectionBehavior),
		new UIPropertyMetadata(default(IList), OnSelectedItemsChanged)
	);

	protected override void OnAttached()
	{
		base.OnAttached();

		AssociatedObject.SelectionChanged += OnSelectionChanged;

		if (SelectedItems is null) return;

		if (AssociatedObject.SelectedItems.Count > 0)
		{
			AssociatedObject.SelectedItems.Clear();
		}

		foreach (object? item in SelectedItems)
		{
			AssociatedObject.SelectedItems.Add(item);
		}
	}

	protected override void OnDetaching()
	{
		AssociatedObject.SelectionChanged -= OnSelectionChanged;
		base.OnDetaching();
	}

	private static void OnSelectedItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
	{
		if (o is not MultiSelectionBehavior behavior) return;
		if (behavior.AssociatedObject is null) return;

		if (e.OldValue is INotifyCollectionChanged oldValue)
		{
			oldValue.CollectionChanged -= behavior.SourceCollectionChanged;
		}

		if (e.NewValue is not INotifyCollectionChanged newValue) return;

		// skip setting the initial value from the UI (since that's an empty collection), as that will overwrite anything that has been set in the bound object
		if (e.OldValue is not null && behavior.AssociatedObject.SelectedItems.Count > 0)
		{
			behavior.AssociatedObject.SelectedItems.Clear();
		}

		foreach (object? item in (IEnumerable)newValue)
		{
			behavior.AssociatedObject.SelectedItems.Add(item);
		}

		newValue.CollectionChanged += behavior.SourceCollectionChanged;
	}

	private bool IsUpdatingTarget { get; set; }
	private bool IsUpdatingSource { get; set; }

	private void SourceCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		if (IsUpdatingSource) return;

		try
		{
			IsUpdatingTarget = true;

			if (e.OldItems is not null)
			{
				foreach (object? item in e.OldItems)
				{
					AssociatedObject.SelectedItems.Remove(item);
				}
			}

			if (e.NewItems is not null)
			{
				foreach (object? item in e.NewItems)
				{
					AssociatedObject.SelectedItems.Add(item);
				}
			}

			if (e.Action is NotifyCollectionChangedAction.Reset)
			{
				AssociatedObject.SelectedItems.Clear();
			}
		}
		finally
		{
			IsUpdatingTarget = false;
		}
	}

	private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (IsUpdatingTarget) return;
		if (!ReferenceEquals(e.OriginalSource, sender)) return;

		IList? selectedItems = SelectedItems;

		if (selectedItems is null) return;

		try
		{
			IsUpdatingSource = true;

			foreach (object? item in e.RemovedItems)
			{
				selectedItems.Remove(item);
			}

			foreach (object? item in e.AddedItems)
			{
				selectedItems.Add(item);
			}
		}
		finally
		{
			IsUpdatingSource = false;
		}
	}
}
