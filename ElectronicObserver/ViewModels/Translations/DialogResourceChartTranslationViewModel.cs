namespace ElectronicObserver.ViewModels.Translations;
public class DialogResourceChartTranslationViewModel : TranslationBaseViewModel
{
	public string Title => GeneralRes.ResourceChart.Replace("_", "__").Replace("&", "_");
	
	public string Start => DropRecordViewerResources.Start;
	public string End => DropRecordViewerResources.End;

	public string Menu_File => ResourceChartResources.Menu_File.Replace("_", "__").Replace("&", "_");
	public string Menu_File_SaveImage => ResourceChartResources.Menu_File_SaveImage.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Graph => ResourceChartResources.Menu_Graph.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Resource => ResourceChartResources.Menu_Graph_Resource.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_ResourceDiff => ResourceChartResources.Menu_Graph_ResourceDiff.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Material => ResourceChartResources.Menu_Graph_Material.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_MaterialDiff => ResourceChartResources.Menu_Graph_MaterialDiff.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_Experience => ResourceChartResources.Menu_Graph_Experience.Replace("_", "__").Replace("&", "_");
	public string Menu_Graph_ExperienceDiff => ResourceChartResources.Menu_Graph_ExperienceDiff.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Option_DivideByDay => ResourceChartResources.Menu_Option_DivideByDay.Replace("_", "__").Replace("&", "_");
	public string Menu_Option_ShowAllData => ResourceChartResources.Menu_Option_ShowAllData.Replace("_", "__").Replace("&", "_");
	
	public string Menu_Span => ResourceChartResources.Menu_Span.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Day => GeneralRes.Day.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Month => GeneralRes.Month.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Year => GeneralRes.Year.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Week => GeneralRes.Week.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_Season => GeneralRes.ThreeMonths.Replace("_", "__").Replace("&", "_");
	public string Menu_Span_All => GeneralRes.AllData.Replace("_", "__").Replace("&", "_");

	public string Menu_Span_WeekFirst => ResourceChartResources.WeekFirst;
	public string Menu_Span_MonthFirst => ResourceChartResources.MonthFirst;
	public string Menu_Span_SeasonFirst => ResourceChartResources.SeasonFirst;
	public string Menu_Span_YearFirst => ResourceChartResources.YearFirst;

	public string OptionsMenu => GeneralRes.Option.Replace("_", "__").Replace("&", "_");
	
	public string Ammo => GeneralRes.Ammo.Replace("_", "__").Replace("&", "_");
	public string Fuel => GeneralRes.Fuel.Replace("_", "__").Replace("&", "_");
	public string Steel => GeneralRes.Steel.Replace("_", "__").Replace("&", "_");
	public string Baux => GeneralRes.Baux.Replace("_", "__").Replace("&", "_");
	
	public string InstantRepair => GeneralRes.Bucket.Replace("_", "__").Replace("&", "_");
	public string ModdingMaterial => GeneralRes.ImpMat.Replace("_", "__").Replace("&", "_");
	public string DevelopmentMaterial => GeneralRes.DevMat.Replace("_", "__").Replace("&", "_");
	public string InstantConstruction => GeneralRes.Flamethrower.Replace("_", "__").Replace("&", "_");
	
	public string Experience => GeneralRes.Experience.Replace("_", "__").Replace("&", "_");
	
	public string Today => DropRecordViewerResources.Today;
}
