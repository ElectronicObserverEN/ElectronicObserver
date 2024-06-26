﻿using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicObserver.Window.Tools.EquipmentList;
/// <summary>
/// Interaction logic for EquipmentListWindow.xaml
/// </summary>
public partial class EquipmentListWindow
{
	public EquipmentListWindow() : base(new EquipmentListViewModel())
	{
		InitializeComponent();
	}

	private void OpenEquipmentEncyclopedia(object sender, MouseButtonEventArgs e)
	{
		if (sender is not DataGridRow { DataContext: EquipmentListRow { Id: { } id } }) return;

		ViewModel.OpenEquipmentEncyclopedia(id);
	}
}
