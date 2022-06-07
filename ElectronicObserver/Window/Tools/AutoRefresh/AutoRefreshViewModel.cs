using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public partial class AutoRefreshViewModel : WindowViewModelBase
{
	public AutoRefreshTranslationViewModel AutoRefresh { get; }

	public List<MapAreaData> Areas { get; set; } = new();
	public List<MapInfoData> Infos { get; set; } = new();

	public List<MapInfoModel> Maps { get; set; } = new();
	public MapInfoModel? SelectedMap { get; set; }
	public ObservableCollection<AutoRefreshRuleViewModel> Rules { get; } = new();

	private List<AutoRefreshRuleViewModel> EnabledRules => Rules
		.Where(r => r.IsEnabled)
		.ToList();

	public bool IsSingleMapMode { get; set; }
	public string? SingleMapModeRuleDisplay => EnabledRules switch
	{
		{ Count: > 0 } => RuleDisplay(EnabledRules[0]),
		_ => null
	};

	private static string RuleDisplay(AutoRefreshRuleViewModel rule) =>
		$"{rule.Map.Display} ({string.Join(", ", rule.AllowedCells.Select(c => c.Id))})";

	public AutoRefreshViewModel()
	{
		AutoRefresh = Ioc.Default.GetRequiredService<AutoRefreshTranslationViewModel>();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Areas) or nameof(Infos))) return;

			IEnumerable<MapInfoModel> maps =
				from area in Areas
				join info in Infos on area.ID equals info.MapAreaID
				select new MapInfoModel(area.ID, info.MapInfoID);

			Maps = maps.ToList();
			SelectedMap = Maps.FirstOrDefault();
		};

		Rules.CollectionChanged += UpdateSingleMapModeRuleDisplay;
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

	private void Save()
	{
		using ElectronicObserverContext db = new();
		AutoRefreshModel? model = db.AutoRefresh.FirstOrDefault();

		if (model is null)
		{
			model = new()
			{
				Id = 1,
			};
			db.Add(model);
		}

		model.IsSingleMapMode = IsSingleMapMode;

		model.Rules = Rules.Select(r => new AutoRefreshRuleModel
		(
			r.IsEnabled,
			new(r.Map.AreaId, r.Map.InfoId),
			r.AllowedCells.Select(c => new CellModel
			(
				c.Id
			)).ToList()

		)).ToList();

		db.SaveChanges();
	}

	private void Load()
	{
		using ElectronicObserverContext db = new();
		AutoRefreshModel? model = db.AutoRefresh.FirstOrDefault();

		if (model is null) return;

		LoadModel(model);
	}

	private void LoadModel(AutoRefreshModel model)
	{
		Rules.Clear();

		IsSingleMapMode = model.IsSingleMapMode;

		foreach (AutoRefreshRuleModel rule in model.Rules)
		{
			AutoRefreshRuleViewModel ruleViewModel = new(rule.Map)
			{
				IsEnabled = rule.IsEnabled,
			};

			foreach (CellModel cell in rule.AllowedCells)
			{
				ruleViewModel.AllowedCells.Add(new(cell.CellId));
			}

			AddRule(ruleViewModel);
		}
	}

	[ICommand]
	private void AddRule()
	{
		if (SelectedMap is null) return;

		AddRule(new(SelectedMap));
	}

	private void AddRule(AutoRefreshRuleViewModel rule)
	{
		rule.AllowedCells.CollectionChanged += UpdateSingleMapModeRuleDisplay;
		rule.PropertyChanged += RuleOnPropertyChanged;

		Rules.Add(rule);
	}

	[ICommand]
	private void RemoveRule(AutoRefreshRuleViewModel? rule)
	{
		if (rule is null) return;

		Rules.Remove(rule);
	}

	private void RuleOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(AutoRefreshRuleViewModel.IsEnabled)) return;

		OnPropertyChanged(nameof(SingleMapModeRuleDisplay));
	}

	private void UpdateSingleMapModeRuleDisplay(object? sender, NotifyCollectionChangedEventArgs args)
	{
		OnPropertyChanged(nameof(SingleMapModeRuleDisplay));
	}
}
