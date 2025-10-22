using System.Collections.Generic;
using System.Windows.Controls;
using ElectronicObserver.Core.Types;
using KancolleProgress.ViewModels;

namespace KancolleProgress;

/// <summary>
/// Interaction logic for KancolleProgress.xaml
/// </summary>
public partial class KancolleProgressView : UserControl
{
	public KancolleProgressViewModel ViewModel { get; }
	internal static KancolleProgressViewModel Instance { get; private set; } = null!;

	public IEnumerable<IShipData> UserShips
	{
		set => ViewModel.UserShips = value;
	}

	public IEnumerable<IShipDataMaster> AllShips
	{
		set => ViewModel.AllShips = value;
	}

	public IEnumerable<IEquipmentData> UserEquipment
	{
		set => ViewModel.UserEquipment = value;
	}

	public KancolleProgressView()
	{
		ViewModel = new KancolleProgressViewModel();
		Instance = ViewModel;
		DataContext = ViewModel;

		InitializeComponent();
	}
}
