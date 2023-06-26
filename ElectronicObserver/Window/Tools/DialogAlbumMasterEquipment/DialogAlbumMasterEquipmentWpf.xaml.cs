using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;

/// <summary>
/// Interaction logic for DialogAlbumMasterEquipmentWpf.xaml
/// </summary>
public partial class DialogAlbumMasterEquipmentWpf
{
	public DialogAlbumMasterEquipmentWpf() : base(new DialogAlbumMasterEquipmentViewModel())
	{
		InitializeComponent();

		ViewModel.PropertyChanged += SelectedEquipmentChanged;
	}

	public DialogAlbumMasterEquipmentWpf(int equipId) : this()
	{
		ViewModel.SelectedEquipmentModel = ViewModel.DataGridViewModel.ItemsSource
			.FirstOrDefault(e => e.ID == equipId);
	}

	private void SelectedEquipmentChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.SelectedEquipmentViewModel)) return;

		ScrollIntoView();
	}

	private void DataGrid_OnTargetUpdated(object? sender, DataTransferEventArgs e)
	{
		ScrollIntoView();
	}

	private void ScrollIntoView()
	{
		if (ViewModel.SelectedEquipmentViewModel is null) return;

		DataGrid.ScrollIntoView(ViewModel.SelectedEquipmentViewModel);
	}

	private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
	{
		if (sender is not DataGridRow { DataContext: EquipmentDataViewModel { Equipment: { } equip } }) return;

		ViewModel.OpenEquipmentEncyclopedia(equip);
	}
}
