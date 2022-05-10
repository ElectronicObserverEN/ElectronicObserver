using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserverTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public partial class EventLockPlannerViewModel : WindowViewModelBase
{
	public EventLockPlannerTranslationViewModel EventLockPlanner { get; }

	private List<ShipLockViewModel> AllShipLocks { get; }

	public LockGroupViewModel NoLockGroup { get; } = new(0);
	public ObservableCollection<LockGroupViewModel> LockGroups { get; } = new();

	public EventLockPlannerViewModel(IEnumerable<IShipData> allShips)
	{
		EventLockPlanner = Ioc.Default.GetService<EventLockPlannerTranslationViewModel>()!;

		AllShipLocks = allShips.Select(s => new ShipLockViewModel(s)).ToList();

		GenerateUnlockedShips();
		PopulateLocks();
	}

	public override void Loaded()
	{
		base.Loaded();

		Load();
	}

	public override void Closed()
	{
		base.Closed();

		Save();
	}

	private void GenerateUnlockedShips()
	{
		List<ShipLockViewModel> unlockedShips = AllShipLocks
			.Where(l => l.Ship.SallyArea <= 0)
			.ToList();

		foreach (ShipLockViewModel ship in unlockedShips)
		{
			NoLockGroup.Add(ship);
		}
	}

	private void PopulateLocks()
	{
		foreach (ShipLockViewModel shipLock in AllShipLocks.Where(l => l.Ship.SallyArea > 0))
		{
			while (shipLock.ActualLock > LockGroups.Count)
			{
				AddLock();
			}

			LockGroups[shipLock.ActualLock - 1].Move(shipLock, NoLockGroup);
		}
	}

	[ICommand]
	private void AddLock()
	{
		int id = LockGroups.Count + 1;
		LockGroups.Add(new(id) { Name = $"{id}" });
	}

	[ICommand]
	private void RemoveLock()
	{
		if (LockGroups.Count is 0) return;

		LockGroupViewModel group = LockGroups[^1];

		foreach (ShipLockViewModel ship in group.Ships.ToList())
		{
			NoLockGroup.Move(ship, group);
		}

		LockGroups.Remove(group);
	}

	private void Save()
	{
		using ElectronicObserverContext db = new();
		EventLockPlannerModel? model = db.EventLockPlans.FirstOrDefault();

		if (model is null)
		{
			model = new();
			db.Add(model);
		}

		model.Locks = LockGroups.Select(g => new EventLockModel
		{
			Id = g.Id,
			A = g.Color.A,
			R = g.Color.R,
			G = g.Color.G,
			B = g.Color.B,
			Name = g.Name,
		}).ToList();

		model.ShipLocks = LockGroups.SelectMany(g => g.Ships.Select(s => new ShipLockModel
		{
			Id = s.Ship.ID,
			PlannedLock = s.PlannedLock,
		})).ToList();

		db.SaveChanges();
	}

	private void Load()
	{
		using ElectronicObserverContext db = new();
		EventLockPlannerModel model = db.EventLockPlans.FirstOrDefault() ?? new();

		LoadModel(model);
	}

	private void LoadModel(EventLockPlannerModel model)
	{
		NoLockGroup.Clear();
		LockGroups.Clear();

		GenerateUnlockedShips();
		PopulateLocks();

		foreach (EventLockModel eventLock in model.Locks)
		{
			LockGroups.Add(new(eventLock.Id)
			{
				Color = Color.FromArgb(eventLock.A, eventLock.R, eventLock.G, eventLock.B),
				Name = eventLock.Name,
			});
		}

		foreach (ShipLockModel shipLockModel in model.ShipLocks)
		{
			LockGroupViewModel lockGroup = LockGroups[shipLockModel.PlannedLock - 1];
			ShipLockViewModel? shipLock = NoLockGroup.Ships.FirstOrDefault(l => l.Ship.ID == shipLockModel.Id);

			if (shipLock is null) continue;
			if (!lockGroup.CanAdd(shipLock)) continue;

			lockGroup.Move(shipLock, NoLockGroup);
		}
	}
}
