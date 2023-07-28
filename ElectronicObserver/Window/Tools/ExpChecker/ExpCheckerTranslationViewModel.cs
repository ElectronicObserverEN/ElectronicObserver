using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.ExpChecker;

public class ExpCheckerTranslationViewModel : TranslationBaseViewModel
{
	public string Title = ExpCheckerResources.Title;

	public string DisplayCriteria => ExpCheckerResources.DisplayCriteria;
	public string Ship => ExpCheckerResources.Ship;
	public string SearchInFleet => ExpCheckerResources.SearchInFleet;
	public string SearchInFleetToolTip => ExpCheckerResources.SearchInFleetToolTip;
	public string SortieExp => ExpCheckerResources.SortieExp;
	public string ShowAllASWEquipments => ExpCheckerResources.ShowAllASWEquipments;
	public string ShowAllASWEquipmentsToolTip => ExpCheckerResources.ShowAllASWEquipmentsToolTip;

	public string ShowAllLevel => ExpCheckerResources.ShowAllLevel;
	public string ShowAllLevelToolTip => ExpCheckerResources.ShowAllLevelToolTip;

	public string AswOffset => ExpCheckerResources.AswOffset;
	public string ASWModernizationToolTip => ExpCheckerResources.ASWModernizationToolTip;

	public string ExpUnitToolTip => ExpCheckerResources.ExpUnitToolTip;
	public string GroupExp => ExpCheckerResources.GroupExp;

	public string ColumnSortieCount => ExpCheckerResources.ColumnSortieCount;
	public string ASW => ExpCheckerResources.ASW;
	public string ColumnEquipment => ExpCheckerResources.ColumnEquipment;

	public string AswUnknown => ExpCheckerResources.AswUnknown;
	public string AswApproximated => ExpCheckerResources.AswApproximated;
	public string Modernization => ExpCheckerResources.Modernization;
}
