using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Common;

public abstract class WindowViewModelBase : ObservableObject
{
	public FontFamily Font { get; set; } = default!;
	public double FontSize { get; set; }
	public SolidColorBrush FontBrush { get; set; } = default!;

	protected WindowViewModelBase()
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
}
