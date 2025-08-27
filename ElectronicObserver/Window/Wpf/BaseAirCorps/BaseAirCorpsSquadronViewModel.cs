using System.Collections.ObjectModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.BaseAirCorps;

public class BaseAirCorpsSquadronViewModel : BaseAirCorpsItemControlViewModel
{
	public Utility.Storage.SerializableFont Font { get; internal set; }
	public bool ShowAircraft { get; set; }
	public bool ShowAircraftLevelByNumber { get; set; }
	public LevelVisibilityFlag LevelVisibility { get; set; }

	public ObservableCollection<ShipSlotViewModel> SlotList { get; } = new()
	{
		new(),
		new(),
		new(),
		new(),
	};

	public BaseAirCorpsSquadronViewModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Font)) return;

			foreach (ShipSlotViewModel slot in SlotList)
			{
				slot.Font = Font;
			}
		};
	}

	public void SetSlotList(BaseAirCorpsData corps)
	{
		for (int i = 0; i < SlotList.Count; i++)
		{
			IBaseAirCorpsSquadron squadron = corps[i + 1];
			IEquipmentData? eq = squadron.EquipmentInstance;

			switch (squadron.State)
			{
				case 0:	// 未配属
				case 2:	// 配置転換中
				default:
					SlotList[i].Equipment = null;
					SlotList[i].AircraftMax = 0;
					SlotList[i].AircraftCurrent = 0;
					break;
				case 1:	// 配属済み
					if (eq == null)
						goto case 0;
					SlotList[i].Equipment = eq;
					SlotList[i].AircraftMax = squadron.AircraftMax;
					SlotList[i].AircraftCurrent = squadron.AircraftCurrent;
					break;
			}

		}
	}
}
