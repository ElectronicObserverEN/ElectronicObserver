using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common.Datagrid;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;

public partial class ShipTrainingPlannerViewerDataGrid : DataGridViewModelBase
{
	public ShipTrainingPlannerTranslationViewModel ShipTrainingPlanner { get; }

	public ColumnViewModel? NameColumn => GetColumn(nameof(NameColumn));

	public ShipTrainingPlannerViewerDataGrid()
	{
		ShipTrainingPlanner = Ioc.Default.GetRequiredService<ShipTrainingPlannerTranslationViewModel>();

		if (NameColumn is null)
			Columns.Add(nameof(NameColumn), new()
			{
				Header = ShipTrainingPlanner.ShipName
			});

		// TODO : reload TL on config change
	}

	[RelayCommand]
	private void OpenColumnSelector()
	{
		OpenColumnSelectorBase();
	}
}
