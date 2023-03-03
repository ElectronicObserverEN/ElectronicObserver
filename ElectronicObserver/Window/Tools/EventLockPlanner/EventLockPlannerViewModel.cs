using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data.Translation;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EventLockPlanner;

namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public partial class EventLockPlannerViewModel : WindowViewModelBase
{
	private string EventLocksPath => Path.Join(DataAndTranslationManager.DataFolder, "Locks.json");

	public EventLockPlannerTranslationViewModel EventLockPlanner { get; }
	private DataSerializationService DataSerializationService { get; }
	private LockTranslationData LockTranslator { get; }

	private List<ShipLockViewModel> AllShipLocks { get; }

	public LockGroupViewModel NoLockGroup { get; } = new(0) { Name = "0", Color = Colors.Transparent };
	public ObservableCollection<LockGroupViewModel> LockGroups { get; } = new();
	public ObservableCollection<EventPhaseViewModel> EventPhases { get; } = new();

	public bool ShowFinishedPhases { get; set; } = true;

	public delegate void TagChangedEventHandler();
	public static event TagChangedEventHandler TagChanged = delegate { };

	public DataGridViewModel<ShipLockViewModel> DataGridViewModel { get; set; }

	public EventLockPlannerViewModel(IEnumerable<IShipData> allShips, LockTranslationData lockTranslator)
	{
		EventLockPlanner = Ioc.Default.GetService<EventLockPlannerTranslationViewModel>()!;
		DataSerializationService = Ioc.Default.GetService<DataSerializationService>()!;
		LockTranslator = lockTranslator;

		DataGridViewModel = new(NoLockGroup.Ships);

		AllShipLocks = allShips.Select(s => new ShipLockViewModel(s)).ToList();
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

	[RelayCommand]
	private void AddLock()
	{
		int id = LockGroups.Count + 1;
		LockGroups.Add(new(id) { Name = $"{id}" });
	}

	[RelayCommand]
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

	[RelayCommand]
	private void AddPhase()
	{
		EventPhases.Add(new(LockGroups));
	}

	[RelayCommand]
	private void RemovePhase()
	{
		if (EventPhases.Count is 0) return;

		EventPhases.Remove(EventPhases[^1]);
	}

	[RelayCommand]
	private void DeletePhase(EventPhaseViewModel? phase)
	{
		if(phase is null) return;

		EventPhases.Remove(phase);
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

		model.Phases = EventPhases.Select(p => new EventPhaseModel
		{
			Name = p.Name,
			IsFinished = p.IsFinished,
			PhaseLockGroups = p.PhaseLockGroups.Select(g => g.Id).ToList(),
			PhaseShips = p.Ships.Select(s => s.Ship.MasterID).ToList(),
		}).ToList();

		db.SaveChanges();
		TagChanged();
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

		foreach (EventLockModel eventLock in model.Locks)
		{
			LockGroups.Add(new(eventLock.Id)
			{
				Color = Color.FromArgb(eventLock.A, eventLock.R, eventLock.G, eventLock.B),
				Name = eventLock.Name,
			});
		}

		GenerateUnlockedShips();
		PopulateLocks();

		foreach (ShipLockModel shipLockModel in model.ShipLocks)
		{
			int lockIndex = shipLockModel.PlannedLock - 1;

			if (lockIndex < 0 || lockIndex > LockGroups.Count)
			{
				Logger.Add(2, EventLockPlanner.InvalidModelState);
				return;
			}

			LockGroupViewModel lockGroup = LockGroups[lockIndex];
			ShipLockViewModel? shipLock = NoLockGroup.Ships.FirstOrDefault(l => l.Ship.ID == shipLockModel.Id);

			if (shipLock is null) continue;
			if (!lockGroup.CanAdd(shipLock)) continue;

			lockGroup.Move(shipLock, NoLockGroup);
		}

		foreach (EventPhaseModel eventPhaseModel in model.Phases)
		{
			EventPhaseViewModel phase = new(LockGroups)
			{
				Name = eventPhaseModel.Name,
				IsFinished = eventPhaseModel.IsFinished,
			};

			EventPhases.Add(phase);

			foreach (int lockGroupId in eventPhaseModel.PhaseLockGroups)
			{
				LockGroupViewModel? group = LockGroups.Skip(lockGroupId - 1).FirstOrDefault();

				if (group is null) continue;

				phase.PhaseLockGroups.Add(group);
			}

			foreach (int phaseShipId in eventPhaseModel.PhaseShips)
			{
				ShipLockViewModel? shipLock = NoLockGroup.Ships
					.Concat(LockGroups.SelectMany(g => g.Ships))
					.FirstOrDefault(l => l.Ship.ID == phaseShipId);

				if (shipLock is null) continue;

				phase.Add(shipLock);
			}
		}
	}

	[RelayCommand]
	private void CopyLocksToClipboard()
	{
		Clipboard.SetDataObject(DataSerializationService.EventLockPlanner(this));
	}

	[RelayCommand]
	private void LoadLocksFromClipboard()
	{
		if (MessageBox.Show
		(
			EventLockPlanner.LockLoadWarningText,
			EventLockPlanner.Warning,
			MessageBoxButton.OKCancel,
			MessageBoxImage.Warning
		) is not MessageBoxResult.OK)
		{
			return;
		}

		string json = Clipboard.GetText();

		EventLockPlannerData? data = JsonSerializer.Deserialize<EventLockPlannerData>(json);

		if (data is null)
		{
			// error
			return;
		}

		ImportLocks(data);
	}

	[RelayCommand]
	private void LoadEventLocks()
	{
		if (MessageBox.Show
		(
			EventLockPlanner.LockLoadWarningText,
			EventLockPlanner.Warning,
			MessageBoxButton.OKCancel,
			MessageBoxImage.Warning
		) is not MessageBoxResult.OK)
		{
			return;
		}

		string? json;

		try
		{
			json = File.ReadAllText(EventLocksPath);
		}
		catch
		{
			Logger.Add(2, EventLockPlanner.FailedToLoadLockData);
			return;
		}

		EventLockPlannerData? data = JsonSerializer.Deserialize<EventLockPlannerData>(json);

		if (data is null)
		{
			// error
			return;
		}

		ImportLocks(data);
	}

	private void ImportLocks(EventLockPlannerData data)
	{
		List<List<ShipLockViewModel>> lockGroups = LockGroups
			.Select(g => g.Ships.ToList())
			.Prepend(NoLockGroup.Ships.ToList())
			.ToList();

		List<EventPhaseViewModel> eventPhases = EventPhases
			.ToList();

		NoLockGroup.Clear();
		LockGroups.Clear();
		EventPhases.Clear();

		foreach (EventLockPlannerLock eventLock in data.Locks.OrderBy(l => l.Id))
		{
			LockGroups.Add(new(eventLock.Id)
			{
				Color = Color.FromArgb(eventLock.A, eventLock.R, eventLock.G, eventLock.B),
				Name = LockTranslator.Lock(eventLock.Name),
			});
		}

		foreach ((List<ShipLockViewModel> shipLocks, int id) in lockGroups.Select((l, i) => (l, i)))
		{
			if (id is 0 || id > LockGroups.Count)
			{
				foreach (ShipLockViewModel shipLock in shipLocks)
				{
					NoLockGroup.Add(shipLock);
				}
			}
			else
			{
				foreach (ShipLockViewModel shipLock in shipLocks)
				{
					LockGroups[id - 1].Add(shipLock);
				}
			}
		}

		foreach (EventLockPlannerPhase phase in data.Phases)
		{
			EventPhaseViewModel eventPhase = new(LockGroups)
			{
				Name = phase.Name,
			};

			EventPhases.Add(eventPhase);

			foreach (int id in phase.LockGroups)
			{
				LockGroupViewModel? lockGroup = LockGroups.FirstOrDefault(g => g.Id == id);

				if (lockGroup is null) continue;

				eventPhase.AddLockToPhaseCommand.Execute(lockGroup);
			}
		}

		foreach ((EventPhaseViewModel eventPhase, int id) in eventPhases.Select((p, id) => (p, id)))
		{
			if (id >= EventPhases.Count) continue;

			foreach (ShipLockViewModel shipLock in eventPhase.Ships.ToList())
			{
				EventPhases[id].Add(shipLock);
			}

			EventPhases[id].IsFinished = eventPhase.IsFinished;
		}
	}
}
