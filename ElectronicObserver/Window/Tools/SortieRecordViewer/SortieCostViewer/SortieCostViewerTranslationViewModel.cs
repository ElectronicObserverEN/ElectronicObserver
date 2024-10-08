﻿using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewerTranslationViewModel : TranslationBaseViewModel
{
	public string Title => SortieCostViewerResources.Title;

	public string Start => DropRecordViewerResources.Start;

	public string World => SortieRecordViewerResources.World;
	public string Map => SortieRecordViewerResources.Map;

	public string Fuel => ConstantsRes.Fuel;
	public string Ammo => ConstantsRes.Ammo;
	public string Steel => ConstantsRes.Steel;
	public string Baux => ConstantsRes.Baux;
	public string Bucket => ConstantsRes.Bucket;

	public string SortieSupply => SortieCostViewerResources.SortieSupply;
	public string SortieRepair => SortieCostViewerResources.SortieRepair;
	public string NodeSupport => SortieCostViewerResources.NodeSupport;
	public string BossSupport => SortieCostViewerResources.BossSupport;
	public string AirBaseSortie => SortieCostViewerResources.AirBaseSortie;
	public string AirBaseSupply => SortieCostViewerResources.AirBaseSupply;

	public object ResourceGain => SortieCostViewerResources.ResourceNodes;
	public object SinkingResourceGain => SortieCostViewerResources.Sinking;

	public string TotalSortieCost => SortieCostViewerResources.TotalSortieCost;

	public string NormalDamage => SortieCostViewerResources.NormalDamage;
	public string LightDamage => ConstantsRes.LightDamage;
	public string MediumDamage => ConstantsRes.ModerateDamage;
	public string HeavyDamage => ConstantsRes.CriticalDamage;
}
