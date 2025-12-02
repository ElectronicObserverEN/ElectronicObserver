using System;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Control;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class FleetHpViewModel : ObservableObject
{
	public ProgressBarProps HPBar { get; } = new();
	public ProgressBarProps AkashiRepairBar { get; } = new();

	/// <summary>
	/// バーの色(増加分)
	/// </summary>
	public SolidColorBrush BarColorIncrement { get; set; } = new(Color.FromArgb(255, 68, 255, 0));

	/// <summary>
	/// バーの色(減少分)
	/// </summary>
	public SolidColorBrush BarColorDecrement { get; set; } = new(Color.FromArgb(255, 136, 34, 34));

	public bool UsePrevValue { get; set; }
	public bool ShowDifference { get; set; }
	// when doing Akashi repair this is set to the original HP
	// value gets updated to include repair amount
	public int PrevValue { get; set; }
	public ShipStatusHPRepairTimeShowMode RepairTimeShowMode { get; set; }
	// full repair time if ship is docked
	// time to next HP point during Akashi repair
	public DateTime RepairTime { get; set; }
	public string? Text { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public string? ToolTip { get; set; }
	public SerializableFont SubFont { get; set; }
	public System.Drawing.Color MainFontColor { get; set; }
	public System.Drawing.Color SubFontColor { get; set; }
	private bool _onMouse { get; set; }

	public string? DisplayText { get; set; }

	public SolidColorBrush Foreground { get; set; }
	public SolidColorBrush Background => BackColor.ToBrush();

	public SolidColorBrush MainForeground => MainFontColor.ToBrush();
	public FontFamily SubFontFamily => new(SubFont.FontData.FontFamily.Name);
	public double SubFontSize => SubFont.FontData.ToSize();
	public SolidColorBrush SubForeground => SubFontColor.ToBrush();

	public FleetHpViewModel()
	{
		HPBar.PropertyChanged += HPBar_PropertyChanged;
		AkashiRepairBar.PropertyChanged += AkashiRepairBar_PropertyChanged;
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(_onMouse)) return;

			ResumeUpdate();
		};

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		MainFontColor = Utility.Configuration.Config.UI.ForeColor;
		SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

		Foreground = MainForeground;
	}

	[RelayCommand]
	private void MouseEnter()
	{
		_onMouse = true;
		/*
		if (RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.MouseOver)
			PropertyChanged();
		*/
	}

	[RelayCommand]
	private void MouseLeave()
	{
		_onMouse = false;
		/*
		if (RepairTimeShowMode == ShipStatusHPRepairTimeShowMode.MouseOver)
			PropertyChanged();
		*/
	}

	private void HPBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not (nameof(HPBar.Value) or nameof(HPBar.MaximumValue))) return;

		DisplayText = $"{HPBar.Value} / {HPBar.MaximumValue}";
		HPBar.Foreground = HPBar.GetColor(HPBar.Value, HPBar.MaximumValue, HPBar.ColorMorphing).ToBrush();
		AkashiRepairBar.Value = HPBar.Value;
		AkashiRepairBar.MaximumValue = HPBar.MaximumValue;

		/*
		// I don't think decrement ever happens
		AkashiRepairBar.Foreground = (HPBar.Value < AkashiRepairBar.Value) switch
		{
			true => BarColorIncrement,
			_ => BarColorDecrement
		};
		AkashiRepairBar.Visibility = (HPBar.Value < AkashiRepairBar.Value) switch
		{
			true => Visibility.Visible,
			_ => Visibility.Collapsed
		};
		*/
	}

	private void AkashiRepairBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not (nameof(AkashiRepairBar.Value) or nameof(AkashiRepairBar.MaximumValue))) return;

		// I don't think decrement ever happens
		AkashiRepairBar.Foreground = (HPBar.Value < AkashiRepairBar.Value) switch
		{
			true => BarColorIncrement,
			_ => BarColorDecrement
		};
		HPBar.Foreground = HPBar.GetColor(AkashiRepairBar.Value, HPBar.MaximumValue, HPBar.ColorMorphing).ToBrush();
		AkashiRepairBar.Visibility = (HPBar.Value < AkashiRepairBar.Value).ToVisibility();
	}

	private string GetDifferenceString()
	{
		return (AkashiRepairBar.Value - PrevValue).ToString("+0;-0;-0");
	}

	public void ResumeUpdate()
	{
		if (RepairTimeShowMode is ShipStatusHPRepairTimeShowMode.Visible ||
			(RepairTimeShowMode is ShipStatusHPRepairTimeShowMode.MouseOver && _onMouse))
		{
			DisplayText = DateTimeHelper.ToTimeRemainString(RepairTime);
			Foreground = SubForeground;
		}
		else
		{
			DisplayText = ShowDifference switch
			{
				true => $"{AkashiRepairBar.Value} / {GetDifferenceString()}",
				_ => $"{HPBar.Value} / {HPBar.MaximumValue}"
			};
			Foreground = MainForeground;
		}
	}
}
