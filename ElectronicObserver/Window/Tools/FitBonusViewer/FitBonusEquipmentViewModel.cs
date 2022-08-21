using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.FitBonusViewer;

public partial class FitBonusEquipmentViewModel : ObservableObject
{
	[ObservableProperty]
	private IEquipmentData? selectedEquipment;

	public FitBonusEquipmentViewModel()
	{

	}

	public FitBonusEquipmentViewModel(IEquipmentData? equipment)
	{
		selectedEquipment = equipment;
	}


}
