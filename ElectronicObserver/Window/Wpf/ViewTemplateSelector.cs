using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Wpf.Arsenal;
using ElectronicObserver.Window.Wpf.BaseAirCorps;
using ElectronicObserver.Window.Wpf.Battle;
using ElectronicObserver.Window.Wpf.Compass;
using ElectronicObserver.Window.Wpf.Dock;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserver.Window.Wpf.FleetOverview;
using ElectronicObserver.Window.Wpf.FleetPreset;
using ElectronicObserver.Window.Wpf.Headquarters;
using ElectronicObserver.Window.Wpf.Quest;
using ElectronicObserver.Window.Wpf.WinformsHost;
using ElectronicObserver.Window.Wpf.InformationView;
using ElectronicObserver.Window.Wpf.Log;
using ElectronicObserver.Window.Wpf.ExpeditionCheck;
using ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;
using ElectronicObserver.Window.Wpf.SenkaLeaderboard;
using ElectronicObserver.Window.Wpf.ShipGroupAvalonia;
using ElectronicObserver.Window.Wpf.ShipGroupWinforms;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

namespace ElectronicObserver.Window.Wpf;

public class ViewTemplateSelector : DataTemplateSelector
{
	public DataTemplate? Fleet { get; set; }
	public DataTemplate? FleetOverview { get; set; }
	public DataTemplate? Group { get; set; }
	public DataTemplate? GroupWinforms { get; set; }
	public DataTemplate? GroupAvalonia { get; set; }
	public DataTemplate? FleetPreset { get; set; }
	public DataTemplate? ShipTrainingPlanViewer { get; set; }
	public DataTemplate? SenkaLeaderboardViewer { get; set; }

	public DataTemplate? Dock { get; set; }
	public DataTemplate? Arsenal { get; set; }
	public DataTemplate? EquipmentUpgradePlanViewer { get; set; }
	public DataTemplate? BaseAirCorps { get; set; }

	public DataTemplate? Headquarters { get; set; }
	public DataTemplate? Quest { get; set; }
	public DataTemplate? ExpeditionCheck { get; set; }
	public DataTemplate? Information { get; set; }

	public DataTemplate? Compass { get; set; }
	public DataTemplate? Battle { get; set; }

	public DataTemplate? BrowserHost { get; set; }
	public DataTemplate? Log { get; set; }
	public DataTemplate? Json { get; set; }

	public DataTemplate? WinformsHost { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		FleetViewModel => Fleet,
		FleetOverviewViewModel => FleetOverview,
		ShipGroupWinformsViewModel => GroupWinforms,
		ShipGroupAvaloniaViewModel => GroupAvalonia,
		FleetPresetViewModel => FleetPreset,
		ShipTrainingPlanViewerViewModel => ShipTrainingPlanViewer,
		LogViewModel => Log,
		DockViewModel => Dock,
		ArsenalViewModel => Arsenal,
		EquipmentUpgradePlanViewerViewModel => EquipmentUpgradePlanViewer,
		BaseAirCorpsViewModel => BaseAirCorps,
		InformationViewModel => Information,
		HeadquartersViewModel => Headquarters,
		QuestViewModel => Quest,
		ExpeditionCheckViewModel => ExpeditionCheck,
		SenkaLeaderboardViewModel => SenkaLeaderboardViewer,

		CompassViewModel => Compass,
		BattleViewModel => Battle,

		WinformsHostViewModel => WinformsHost,

		_ => base.SelectTemplate(item, container)
	};
}
