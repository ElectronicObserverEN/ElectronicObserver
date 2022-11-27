using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Common;

public partial class UserControlViewModelBase : ObservableObject
{
	public FontFamily Font { get; set; } = default!;
	public double FontSize { get; set; }
	public SolidColorBrush FontBrush { get; set; } = default!;

	public Visibility Visibility { get; set; } = Visibility.Collapsed;
	public bool CanClose { get; set; }
	public bool IsSelected { get; set; }
	public bool IsActive { get; set; }

	protected UserControlViewModelBase()
	{
		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData config = Configuration.Config;

		Font = new FontFamily(config.UI.MainFont.FontData.FontFamily.Name);
		FontSize = config.UI.MainFont.FontData.ToSize();
		FontBrush = config.UI.ForeColor.ToBrush();
	}

	public virtual void Loaded()
	{

	}

	public virtual void Closed()
	{

	}

	[RelayCommand(CanExecute = nameof(CanClose))]
	protected virtual void Close()
	{
		Visibility = Visibility.Collapsed;

		Closed();
	}
}
