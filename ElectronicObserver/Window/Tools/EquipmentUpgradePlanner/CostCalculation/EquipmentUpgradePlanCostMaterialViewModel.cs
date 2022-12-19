using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.CostCalculation;
public class EquipmentUpgradePlanCostMaterialViewModel : EquipmentUpgradePlanResourceDisplayViewModel
{
	public UseItemId Type { get; set; }

	public EquipmentUpgradePlanCostMaterialViewModel(int required, UseItemId type) 
	{ 
		Required = required;
		Type = type;

		SubscribeToApis();
		Update();
	}

	public void Update()
	{
		MaterialData db = KCDatabase.Instance.Material;

		Owned = Type switch
		{
			UseItemId.Fuel => db.Fuel,
			UseItemId.Ammo => db.Ammo,
			UseItemId.Steel => db.Steel,
			UseItemId.Bauxite => db.Bauxite,

			UseItemId.DevelopmentMaterial => db.DevelopmentMaterial,
			UseItemId.ImproveMaterial => db.ModdingMaterial,

			_ => 0
		};
	}

	public void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += (_,_) => Update();
	}
}
