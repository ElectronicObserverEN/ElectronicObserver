using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Control.ShipFilter;
using GongSolutions.Wpf.DragDrop;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public class LockGroupViewModel : ObservableObject, IDropTarget
{
	private ColorService ColorService { get; }

	public int Id { get; }
	public Color Color { get; set; }
	public SolidColorBrush Foreground => new(ColorService.GetForegroundColor(Color));
	public SolidColorBrush Background => new(Color);
	public string Name { get; set; } = "";

	public ShipFilterViewModel Filter { get; } = new();
	private ObservableCollection<ShipLockViewModel> ShipSource { get; } = new();
	public ObservableCollection<ShipLockViewModel> Ships { get; } = new();

	public LockGroupViewModel(int id)
	{
		ColorService = Ioc.Default.GetRequiredService<ColorService>();

		Id = id;
		Color = Color.FromRgb((byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256), (byte)Random.Shared.Next(256));

		ShipSource.CollectionChanged += (sender, args) =>
		{
			if (args.OldItems?.Cast<ShipLockViewModel>() is { } removedLocks)
			{
				foreach (ShipLockViewModel shipLock in removedLocks)
				{
					Ships.Remove(shipLock);
				}
			}

			if (args.NewItems is not null)
			{
				ReloadShips(null, null);
			}
		};

		Filter.PropertyChanged += ReloadShips;
	}

	private void ReloadShips(object? sender, PropertyChangedEventArgs? args)
	{
		Ships.Clear();

		List<ShipLockViewModel> filteredShips = ShipSource.Where(s => Filter.MeetsFilterCondition(s.Ship))
			.ToList();

		foreach (ShipLockViewModel shipLock in filteredShips)
		{
			Ships.Add(shipLock);
		}
	}

	public bool CanAdd(ShipLockViewModel shipLock) => Id switch
	{
		0 => shipLock.ActualLock <= 0,
		{ } => shipLock.ActualLock <= 0 || shipLock.ActualLock == Id
	};

	public void Add(ShipLockViewModel shipLock)
	{
		ShipSource.Add(shipLock);
	}

	public void Move(ShipLockViewModel shipLock, LockGroupViewModel source) =>
		Insert(Ships.Count, shipLock, source);

	private void Insert(int index, ShipLockViewModel shipLock)
	{
		shipLock.PlannedLock = Id;
		ShipSource.Insert(index, shipLock);
		ReloadShips(null, null);
	}

	private void Insert(int index, ShipLockViewModel shipLock, LockGroupViewModel source)
	{
		if (!CanAdd(shipLock)) return;

		// index needs to be adjusted for moves within a group
		if (source.Ships == Ships)
		{
			int oldIndex = Ships.IndexOf(shipLock);
			if (oldIndex < index)
			{
				index--;
			}
		}

		source.Remove(shipLock);
		Insert(index, shipLock);
	}

	private void Remove(ShipLockViewModel shipLock)
	{
		ShipSource.Remove(shipLock);
	}

	public void Clear()
	{
		Ships.Clear();
		ShipSource.Clear();
	}

	public void DragOver(IDropInfo dropInfo)
	{
		if (dropInfo.Data is not ShipLockViewModel shipLock) return;
		if (dropInfo.DragInfo.VisualSource is not ItemsControl { DataContext: LockGroupViewModel }) return;

		if (CanAdd(shipLock))
		{
			dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
			dropInfo.Effects = DragDropEffects.Move;
		}
		else
		{
			dropInfo.Effects = DragDropEffects.None;
		}
	}

	public void Drop(IDropInfo dropInfo)
	{
		if (dropInfo.Data is not ShipLockViewModel shipLock) return;

		bool isValidSource = dropInfo.DragInfo.SourceCollection switch
		{
			ListCollectionView source => source.SourceCollection is ObservableCollection<ShipLockViewModel>,
			ObservableCollection<ShipLockViewModel> => true,
			_ => false
		};

		if (!isValidSource) return;

		if (dropInfo.DragInfo.VisualSource is not ItemsControl { DataContext: LockGroupViewModel lockGroup }) return;

		Insert(dropInfo.InsertIndex, shipLock, lockGroup);
	}
}
