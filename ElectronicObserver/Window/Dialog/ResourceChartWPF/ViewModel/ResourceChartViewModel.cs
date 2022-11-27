using System;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Dialog.ResourceChartWPF;
public partial class ResourceChartViewModel : WindowViewModelBase
{
	public ResourceChartViewModel()
	{
		DialogResourceChart = Ioc.Default.GetService<DialogResourceChartTranslationViewModel>()!;
	}

	public bool ShowFuel { get; set; } = true;
	public bool ShowAmmo { get; set; } = true;
	public bool ShowSteel { get; set; } = true;
	public bool ShowBaux { get; set; } = true;
	public bool ShowInstantRepair { get; set; } = true;
	public bool ShowInstantConstruction { get; set; } = true;
	public bool ShowDevelopmentMaterial { get; set; } = true;
	public bool ShowModdingMaterial { get; set; } = true;
	public bool ShowExperience { get; set; } = true;
	public DateTime DateBegin { get; set; }
	public DateTime DateEnd { get; set; }
	public DateTime MinDate { get; set; }
	public DateTime MaxDate { get; set; }
	public string Today => $"{DialogResourceChart.Today} : {DateTime.Now:yyyy/MM/dd}";
	public DialogResourceChartTranslationViewModel DialogResourceChart { get; }

	public string? ToolTip { get; set; }
	public double ToolTipHorizontalOffset { get; set; }
	public double ToolTipVerticalOffset { get; set; }

	[RelayCommand]
	private void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}
}
