using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

/// <summary>
/// Interaction logic for DialogAlbumMasterShipWpf.xaml
/// </summary>
public partial class DialogAlbumMasterShipWpf
{
	public DialogAlbumMasterShipWpf() : base(new DialogAlbumMasterShipViewModel())
	{
		InitializeComponent();

		ViewModel.PropertyChanged += SelectedShipChanged;
	}

	public DialogAlbumMasterShipWpf(int shipId) : this()
	{
		IShipDataMaster ship = KCDatabase.Instance.MasterShips[shipId];
		ViewModel.ChangeShipCommand.Execute(ship);
	}

	public DialogAlbumMasterShipWpf(IShipDataMaster ship) : this()
	{
		ViewModel.ChangeShipCommand.Execute(ship);
	}

	private void SelectedShipChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.SelectedShip)) return;

		ScrollIntoView();
	}

	private void DataGrid_OnTargetUpdated(object? sender, DataTransferEventArgs e)
	{
		ScrollIntoView();
	}

	private void ScrollIntoView()
	{
		if (ViewModel.SelectedShip is null) return;

		DataGrid.ScrollIntoView(ViewModel.SelectedShip);
	}

	private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
	{
		if (sender is not DataGridRow { DataContext: ShipDataRecord { Ship: { } ship } }) return;

		ViewModel.OpenShipEncyclopediaCommand.Execute(ship);
	}
}
