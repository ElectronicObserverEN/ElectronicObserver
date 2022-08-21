using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.FitBonusViewer;

public class FitBonusViewerViewModel : WindowViewModelBase
{

	public FitBonusViewerTranslationViewModel FitBonusViewer { get; } = new();

	public ObservableCollection<FitBonusShipViewModel> FitBonusResults { get; set; } = new();

	public FitBonusViewerViewModel()
	{
		
		FitBonusResults.Add(new FitBonusShipViewModel(new ShipDataMock(KCDatabase.Instance.MasterShips[(int)ShipId.HachijouKai])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE]),
				new EquipmentDataMock(KCDatabase.Instance.MasterEquipments[(int)EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE])
			}
		}));
	}

	public FitBonusViewerViewModel(List<IShipData> ships)
	{
		foreach (IShipData ship in ships)
		{
			FitBonusResults.Add(new FitBonusShipViewModel(ship));
		}
	}
}
