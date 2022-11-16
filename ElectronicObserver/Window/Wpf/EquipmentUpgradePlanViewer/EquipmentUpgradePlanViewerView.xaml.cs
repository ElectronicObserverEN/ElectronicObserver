using System.Windows;

namespace ElectronicObserver.Window.Wpf.EquipmentUpgradePlanViewer;
/// <summary>
/// Interaction logic for EquipmentUpgradePlanViewer.xaml
/// </summary>
public partial class EquipmentUpgradePlanViewerView
{
	public EquipmentUpgradePlanViewerViewModel ViewModel
	{
		get => (EquipmentUpgradePlanViewerViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register
	(
		nameof(ViewModel),
		typeof(EquipmentUpgradePlanViewerViewModel),
		typeof(EquipmentUpgradePlanViewerView),
		new PropertyMetadata(default(EquipmentUpgradePlanViewerViewModel))
	);

	public EquipmentUpgradePlanViewerView()
	{
		InitializeComponent();
	}
}
