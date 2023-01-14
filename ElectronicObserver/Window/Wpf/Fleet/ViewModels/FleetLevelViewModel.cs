using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Tools.ExpChecker;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetLevelViewModel : ObservableObject
{
	private ToolService ToolService { get; }
	public ShipTrainingPlanViewerViewModel ShipTrainingPlanViewerViewModel { get; }

	public string? TextNext { get; internal set; }
	public int Value { get; set; }
	public int MaximumValue { get; set; }
	public int ValueNext { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }
	public SerializableFont SubFont { get; set; } = Configuration.Config.UI.SubFont;
	public System.Drawing.Color SubFontColor { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public bool NextVisible { get; set; }

	public FontFamily SubFontFamily => new(SubFont.FontData.FontFamily.Name);
	public double SubFontSize => SubFont.FontData.ToSize();
	public SolidColorBrush SubForeground => SubFontColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
	public SolidColorBrush Foreground => ForeColor.ToBrush();

	public ShipTrainingPlanViewModel? TrainingPlan { get; set; }

	public FleetLevelViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		ShipTrainingPlanViewerViewModel = Ioc.Default.GetRequiredService<ShipTrainingPlanViewerViewModel>();

		ShipTrainingPlanViewerViewModel.Plans.CollectionChanged += (_ , _) => UpdateColors();
	}

	public void UpdateColors()
	{
		TrainingPlan = ShipTrainingPlanViewerViewModel.Plans.AsEnumerable().FirstOrDefault(plan => plan.Ship.MasterID == Tag);

		if (TrainingPlan is not null && TrainingPlan.ShouldNotifyRemodelReady)
		{
			BackColor = Configuration.Config.UI.Fleet_RemodelReadyColor;
			ForeColor = Configuration.Config.UI.BackColor;
			SubFontColor = Configuration.Config.UI.SubBackColor;
		}
		else
		{
			BackColor = System.Drawing.Color.Transparent;
			ForeColor = Configuration.Config.UI.ForeColor;
			SubFontColor = Configuration.Config.UI.SubForeColor;
		}
	}

	[RelayCommand]
	private void OpenExpChecker()
	{
		ToolService.ExpChecker(new ExpCheckerViewModel(Tag));
	}

	[RelayCommand]
	private void CreateTrainingPlan()
	{
		if (TrainingPlan is not null) return;

		ShipTrainingPlanViewerViewModel.AddNewPlan(KCDatabase.Instance.Ships[Tag]);
	}
}
