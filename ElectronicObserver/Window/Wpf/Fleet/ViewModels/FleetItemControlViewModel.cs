using System;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetItemControlViewModel : ObservableObject
{
	public int MaxWidth { get; set; }
	public string? Text { get; set; }
	public int Tag { get; set; }
	public string? ToolTip { get; set; }
	public Color ForeColor { get; set; }
	public Color BackColor { get; set; }
	public bool Visible { get; set; }
	public Enum? ImageIndex { get; set; }

	public SolidColorBrush Foreground => new(ForeColor);
	public SolidColorBrush Background => new(BackColor);
	public ImageSource? Icon => ImageIndex switch
	{
		IconContent i => ImageSourceIcons.GetIcon(i),
		ResourceManager.EquipmentContent e => ImageSourceIcons.GetEquipmentIcon((EquipmentIconType)e),
		_ => null
	};

	public FleetItemControlViewModel()
	{
		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		ConfigurationChanged();
	}

	[RelayCommand]
	private void OpenShipEncyclopedia()
	{
		new DialogAlbumMasterShipWpf(Tag).Show(App.Current.MainWindow);
	}

	private void ConfigurationChanged()
	{
		ForeColor = Utility.Configuration.Config.UI.ForeColor.ToWpfColor();
		BackColor = Colors.Transparent;
	}
}
