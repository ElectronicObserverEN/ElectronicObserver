using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class ShipResourceViewModel : ObservableObject
{
	public FormFleetTranslationViewModel FormFleet { get; }

	public ProgressBarProps BarFuel { get; } = new();
	public ProgressBarProps BarAmmo { get; } = new();

	public ShipResourceViewModel()
	{
		FormFleet = Ioc.Default.GetService<FormFleetTranslationViewModel>()!;

		BarFuel.PropertyChanged += Bar_PropertyChanged;
		BarAmmo.PropertyChanged += Bar_PropertyChanged;
		SystemEvents.UpdateTimerTick += UpdateTimerTick;
	}

	private void UpdateTimerTick()
	{
		var c = Configuration.Config;
		if (BarFuel.Value <= 0)
		{
			BarFuel.Background = DateTime.Now.Second % 2 == 0 ? System.Windows.Media.Brushes.Maroon : c.UI.SubBackColor.ToBrush();
		}
		else
		{
			BarFuel.Background = c.UI.SubBackColor.ToBrush();
		}
		if (BarAmmo.Value <= 0)
		{
			BarAmmo.Background = DateTime.Now.Second % 2 == 0 ? System.Windows.Media.Brushes.Maroon : c.UI.SubBackColor.ToBrush();
		}
		else
		{
			BarAmmo.Background = c.UI.SubBackColor.ToBrush();
		}
	}

	private void Bar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(ProgressBarProps.Value) or nameof(ProgressBarProps.MaximumValue))
		{
			OnPropertyChanged(nameof(ToolTip));
		}
	}

	public string ToolTip => string.Format(FormFleet.ResourceToolTip,
		BarFuel.Value, BarFuel.MaximumValue,
		(int)Math.Ceiling(100.0 * BarFuel.Value / BarFuel.MaximumValue),
		BarAmmo.Value, BarAmmo.MaximumValue,
		(int)Math.Ceiling(100.0 * BarAmmo.Value / BarAmmo.MaximumValue));

	public void SetResources(int fuelCurrent, int fuelMax, int ammoCurrent, int ammoMax)
	{
		BarFuel.Value = fuelCurrent;
		BarFuel.MaximumValue = fuelMax;
		BarFuel.Foreground = BarFuel.GetColor(BarFuel.Value, BarFuel.MaximumValue, BarFuel.ColorMorphing).ToBrush();

		BarAmmo.Value = ammoCurrent;
		BarAmmo.MaximumValue = ammoMax;
		BarAmmo.Foreground = BarAmmo.GetColor(BarAmmo.Value, BarAmmo.MaximumValue, BarAmmo.ColorMorphing).ToBrush();
	}
}
