using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using GongSolutions.Wpf.DragDrop;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public partial class EventPhaseViewModel : ObservableObject, IDropTarget
{
	public EventLockPlannerTranslationViewModel EventLockPlanner { get; }

	private ObservableCollection<LockGroupViewModel> LockGroups { get; }
	public ObservableCollection<LockGroupViewModel> PhaseLockGroups { get; } = new();

	public IEnumerable<LockGroupViewModel> RemainingGroups => LockGroups.Except(PhaseLockGroups);

	public ObservableCollection<ShipLockViewModel> Ships { get; } = new();

	public string Name { get; set; } = "";
	public bool IsFinished { get; set; }

	public EventPhaseViewModel(ObservableCollection<LockGroupViewModel> lockGroups)
	{
		EventLockPlanner = Ioc.Default.GetService<EventLockPlannerTranslationViewModel>()!;

		LockGroups = lockGroups;

		LockGroups.CollectionChanged += (_, _) => OnPropertyChanged(nameof(RemainingGroups));
		PhaseLockGroups.CollectionChanged += (_, _) => OnPropertyChanged(nameof(RemainingGroups));

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RemainingGroups)) return;

			foreach (ShipLockViewModel shipLock in Ships)
			{
				shipLock.MatchesPhaseLock = !PhaseLockGroups.Any() || PhaseLockGroups
					.Select(g => g.Id)
					.Contains(shipLock.PlannedLock);
			}
		};

		Ships.CollectionChanged += (sender, args) =>
		{
			if (args.NewItems?.Cast<ShipLockViewModel>() is { } addedLocks)
			{
				foreach (ShipLockViewModel shipLock in addedLocks)
				{
					shipLock.PropertyChanged += PlannedLockChange;
				}
			}

			if (args.OldItems?.Cast<ShipLockViewModel>() is { } removedLocks)
			{
				foreach (ShipLockViewModel shipLock in removedLocks)
				{
					shipLock.PropertyChanged -= PlannedLockChange;
				}
			}
		};
	}

	private void PlannedLockChange(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ShipLockViewModel.PlannedLock)) return;

		OnPropertyChanged(nameof(RemainingGroups));
	}


	public bool CanAdd(ShipLockViewModel shipLock, ObservableCollection<ShipLockViewModel>? source) =>
		(source == Ships || !Ships.Contains(shipLock)) && LockCondition(shipLock);

	public void Add(ShipLockViewModel shipLock, ObservableCollection<ShipLockViewModel>? source = null) =>
		Insert(Ships.Count, shipLock, source);

	private bool LockCondition(ShipLockViewModel shipLock) => PhaseLockGroups.Any() switch
	{
		false => true,
		{ } => PhaseLockGroups.Select(g => g.Id).Contains(shipLock.PlannedLock)
	};

	private void Insert(int index, ShipLockViewModel shipLock, ObservableCollection<ShipLockViewModel>? source)
	{
		if (!CanAdd(shipLock, source)) return;

		// index needs to be adjusted for moves within a group
		if (source == Ships)
		{
			int oldIndex = Ships.IndexOf(shipLock);
			if (oldIndex < index)
			{
				index--;
			}

			Ships.Remove(shipLock);
		}

		Ships.Insert(index, shipLock);
	}

	[RelayCommand]
	private void AddLockToPhase(LockGroupViewModel? lockGroup)
	{
		if (lockGroup is null) return;

		PhaseLockGroups.Add(lockGroup);
	}

	[RelayCommand]
	private void RemoveLockFromPhase(LockGroupViewModel? lockGroup)
	{
		if (lockGroup is null) return;

		PhaseLockGroups.Remove(lockGroup);
	}

	[RelayCommand]
	private void RemoveShipLock(ShipLockViewModel? shipLock)
	{
		if (shipLock is null) return;

		Ships.Remove(shipLock);
	}

	public void DragOver(IDropInfo dropInfo)
	{
		if (dropInfo.Data is not ShipLockViewModel shipLock) return;

		ObservableCollection<ShipLockViewModel>? source = dropInfo.DragInfo.VisualSource switch
		{
			ItemsControl { DataContext: LockGroupViewModel lockGroup } => lockGroup.Ships,
			ItemsControl { DataContext: EventPhaseViewModel phase } => phase.Ships,
			_ => null
		};

		if (source is null) return;

		if (CanAdd(shipLock, source))
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

		ObservableCollection<ShipLockViewModel>? source = dropInfo.DragInfo.VisualSource switch
		{
			ItemsControl { DataContext: LockGroupViewModel lockGroup } => lockGroup.Ships,
			ItemsControl { DataContext: EventPhaseViewModel phase } => phase.Ships,
			_ => null
		};

		if (source is null) return;

		Insert(dropInfo.InsertIndex, shipLock, source);
	}
}
