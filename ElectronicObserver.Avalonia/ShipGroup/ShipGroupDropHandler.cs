using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;

namespace ElectronicObserver.Avalonia.ShipGroup;

public class ShipGroupDropHandler : DropHandlerBase
{
	private bool Validate<T>(Visual listBox, DragEventArgs e, object? sourceContext, object? targetContext, 
		bool execute) where T : ShipGroupItem
	{
		if (sourceContext is not T sourceItem) return false;
		if (targetContext is not ShipGroupViewModel vm) return false;
		if (listBox.GetVisualAt(e.GetPosition(listBox)) is not Control targetControl) return false;
		if (targetControl.DataContext is not T targetItem) return false;

		ObservableCollection<ShipGroupItem> groups = vm.Groups;
		int sourceIndex = groups.IndexOf(sourceItem);
		int targetIndex = groups.IndexOf(targetItem);

		if (sourceIndex < 0 || targetIndex < 0)
		{
			return false;
		}

		switch (e.DragEffects)
		{
			case DragDropEffects.Move:
			{
				if (execute)
				{
					MoveItem(groups, sourceIndex, targetIndex);
				}

				return true;
			}

			case DragDropEffects.None:
			case DragDropEffects.Copy:
			case DragDropEffects.Link:
			default:
				return false;
		}
	}

	public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
	{
		if (e.Source is Control && sender is Visual visual)
		{
			return Validate<ShipGroupItem>(visual, e, sourceContext, targetContext, false);
		}

		return false;
	}

	public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
	{
		if (e.Source is Control && sender is Visual visual)
		{
			return Validate<ShipGroupItem>(visual, e, sourceContext, targetContext, true);
		}

		return false;
	}
}
