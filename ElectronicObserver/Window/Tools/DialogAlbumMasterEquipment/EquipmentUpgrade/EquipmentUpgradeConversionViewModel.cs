using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.Helpers;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

public class EquipmentUpgradeConversionViewModel
{
	public EquipmentUpgradeImprovementModel ImprovementModel { get; }

	public IEquipmentDataMaster EquipmentAfterConversion { get; }

    public EquipmentUpgradeItemCostViewModel ConversionCost { get; }

    public List<EquipmentUpgradeHelperViewModel> Helpers { get; }

    public EquipmentUpgradeConversionViewModel(EquipmentUpgradeImprovementModel improvementModel)
	{
		ImprovementModel = improvementModel;
		
		ConversionCost = new(improvementModel.Costs.CostMax!);

	    EquipmentAfterConversion =
		    KCDatabase.Instance.MasterEquipments[improvementModel.ConversionData!.IdEquipmentAfter];

	    Helpers = improvementModel.Helpers
		    .SelectMany(helpers => helpers.ShipIds)
		    .Distinct()
		    .Select(shipId => new EquipmentUpgradeHelperViewModel(shipId))
		    .ToList();
	}

    public void UnsubscribeFromApis()
    {
	    ConversionCost.UnsubscribeFromApis();
	    Helpers.ForEach(viewModel => viewModel.UnsubscribeFromApis());
	}
}
