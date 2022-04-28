﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using Microsoft.Win32;
using ScottPlot.Plottable;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogResourceChart;

namespace ElectronicObserver.Window.Dialog.ResourceChartWPF;

/// <summary>
/// Interaction logic for ResourceChartWPF.xaml
/// </summary>
public partial class ResourceChartWPF
{
	public ResourceChartViewModel ViewModel { get; } = new();
	private Color FuelColor => Color.FromArgb(0, 128, 0);
	private Color AmmoColor => Color.FromArgb(255, 128, 0);
	private Color BauxColor => Color.FromArgb(255, 0, 0);
	private Color SteelColor => GetSteelColor();

	private Color GetSteelColor()
	{
		Configuration.ConfigurationData c = Configuration.Config;
		switch (c.UI.ThemeMode)
		{
			case 0:
				return Color.FromArgb(64, 64, 64);
				break;
			default:
				return Color.FromArgb(255 - 64, 255 - 64, 255 - 64);
				break;
		}
	}

	private Color InstantRepairColor => Color.FromArgb(32, 128, 255);
	private Color InstantConstructionColor => Color.FromArgb(255, 128, 0);
	private Color DevelopmentMaterialColor => Color.FromArgb(0, 0, 255);
	private Color ModdingMaterialColor => Color.FromArgb(64, 64, 64);
	private Color ExperienceColor => Color.FromArgb(0, 0, 255);
	private enum ChartSpan
	{
		Day,
		Week,
		Month,
		Season,
		Year,
		All,
		WeekFirst,
		MonthFirst,
		SeasonFirst,
		YearFirst,
	}

	private enum ChartType
	{
		Resource,
		ResourceDiff,
		Material,
		MaterialDiff,
		Experience,
		ExperienceDiff,
	}

	private ScatterPlot? FuelPlot;
	private ScatterPlot? AmmoPlot;
	private ScatterPlot? SteelPlot;
	private ScatterPlot? BauxPlot;
	private SignalPlotXY? FuelSignalPlot;
	private SignalPlotXY? AmmoSignalPlot;
	private SignalPlotXY? SteelSignalPlot;
	private SignalPlotXY? BauxSignalPlot;
	private ScatterPlot? InstantRepairPlot;
	private ScatterPlot? InstantConstructionPlot;
	private ScatterPlot? ModdingMaterialPlot;
	private ScatterPlot? DevelopmentMaterialPlot;
	private SignalPlotXY? InstantRepairSignalPlot;
	private SignalPlotXY? InstantConstructionSignalPlot;
	private SignalPlotXY? ModdingMaterialSignalPlot;
	private SignalPlotXY? DevelopmentMaterialSignalPlot;
	private ToolTip? toolTip;
	private ScatterPlot? ExperiencePlot;
	private SignalPlotXY ExperienceSignalPlot;
	private ChartType SelectedChartType => (ChartType)GetSelectedMenuStripIndex(ChartTypeMenu);
	private ChartSpan SelectedChartSpan => (ChartSpan)GetSelectedMenuStripIndex(ChartSpanMenu);
	public ResourceChartWPF()
	{
		InitializeComponent();
		DataContext = ViewModel;
		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
		Loaded += ChartArea_Loaded;
		#region Chart Toggles
		ViewModel.PropertyChanged += (sender, args) =>
{
	if (args.PropertyName is not nameof(ViewModel.ShowFuel)) return;

	if (FuelPlot is not null)
	{
		FuelPlot.IsVisible = ViewModel.ShowFuel;
		ChartArea.Refresh();
	}

	if (FuelSignalPlot is not null)
	{
		FuelSignalPlot.IsVisible = ViewModel.ShowFuel;
		ChartArea.Refresh();
	}
};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowAmmo)) return;

			if (AmmoPlot is not null)
			{
				AmmoPlot.IsVisible = ViewModel.ShowAmmo;
				ChartArea.Refresh();
			}

			if (AmmoSignalPlot is not null)
			{
				AmmoSignalPlot.IsVisible = ViewModel.ShowAmmo;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowSteel)) return;

			if (SteelPlot is not null)
			{
				SteelPlot.IsVisible = ViewModel.ShowSteel;
				ChartArea.Refresh();
			}

			if (SteelSignalPlot is not null)
			{
				SteelSignalPlot.IsVisible = ViewModel.ShowSteel;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowBaux)) return;

			if (BauxPlot is not null)
			{
				BauxPlot.IsVisible = ViewModel.ShowBaux;
				ChartArea.Refresh();
			}

			if (BauxSignalPlot is not null)
			{
				BauxSignalPlot.IsVisible = ViewModel.ShowBaux;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowInstantRepair)) return;

			if (InstantRepairPlot is not null)
			{
				InstantRepairPlot.IsVisible = ViewModel.ShowInstantRepair;
				ChartArea.Refresh();
			}

			if (InstantRepairSignalPlot is not null)
			{
				InstantRepairSignalPlot.IsVisible = ViewModel.ShowInstantRepair;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowInstantConstruction)) return;

			if (InstantConstructionPlot is not null)
			{
				InstantConstructionPlot.IsVisible = ViewModel.ShowInstantConstruction;
				ChartArea.Refresh();
			}

			if (InstantConstructionSignalPlot is not null)
			{
				InstantConstructionSignalPlot.IsVisible = ViewModel.ShowInstantConstruction;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowDevelopmentMaterial)) return;

			if (DevelopmentMaterialPlot is not null)
			{
				DevelopmentMaterialPlot.IsVisible = ViewModel.ShowDevelopmentMaterial;
				ChartArea.Refresh();
			}

			if (DevelopmentMaterialSignalPlot is not null)
			{
				DevelopmentMaterialSignalPlot.IsVisible = ViewModel.ShowDevelopmentMaterial;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowModdingMaterial)) return;

			if (ModdingMaterialPlot is not null)
			{
				ModdingMaterialPlot.IsVisible = ViewModel.ShowModdingMaterial;
				ChartArea.Refresh();
			}

			if (ModdingMaterialSignalPlot is not null)
			{
				ModdingMaterialSignalPlot.IsVisible = ViewModel.ShowModdingMaterial;
				ChartArea.Refresh();
			}
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowExperience)) return;

			if (ExperiencePlot is not null)
			{
				ExperiencePlot.IsVisible = ViewModel.ShowExperience;
				ChartArea.Refresh();
			}

			if (ExperienceSignalPlot is not null)
			{
				ExperienceSignalPlot.IsVisible = ViewModel.ShowExperience;
				ChartArea.Refresh();
			}
		};
		#endregion
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;
		switch (c.UI.ThemeMode)
		{
			case 0:
				ChartArea.Plot.Style(ScottPlot.Style.Default);
				break;
			default:
				ChartArea.Plot.Style(ScottPlot.Style.Black);
				break;
		}
	}

	private void ChartArea_Loaded(object sender, RoutedEventArgs e)
	{
		if (!RecordManager.Instance.Resource.Record.Any())
		{
			System.Windows.Forms.MessageBox.Show(Translation.RecordDataDoesNotExist, Translation.Error, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			Close();
			return;
		}

		toolTip = new ToolTip
		{
			Content = "ToolTip"
		};
		ChartArea.ToolTip = toolTip;
		SwitchMenuStrip(ChartSpanMenu, "2");
		SwitchMenuStrip(ChartTypeMenu, "0");

		ChartArea.Configuration.Zoom = false;
		ChartArea.Configuration.Pan = false;
		ChartArea.RightClicked -= ChartArea.DefaultRightClickEvent;
		ChartArea.Configuration.DoubleClickBenchmark = false;
		UpdateChart();
	}

	/// <summary>
	/// Chart onhover handler
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void ChartArea_MouseMove(object sender, MouseEventArgs e)
	{
		// determine point nearest the cursor
		(double mouseCoordX, double mouseCoordY) = ChartArea.GetMouseCoordinates();
		double xyRatio = ChartArea.Plot.XAxis.Dims.PxPerUnit / ChartArea.Plot.YAxis.Dims.PxPerUnit;
		string fuel = GeneralRes.Fuel;
		string ammo = GeneralRes.Ammo;
		string steel = GeneralRes.Steel;
		string baux = GeneralRes.Baux;
		string instant_repair = GeneralRes.Bucket;
		string instant_construction = GeneralRes.Flamethrower;
		string modding_material = GeneralRes.ImpMat;
		string development_material = GeneralRes.DevMat;
		string experience = GeneralRes.Experience;
		if (SelectedChartType == ChartType.Resource)
		{
			(double fuelpointX, double fuelpointY, int fuelpointIndex) = FuelPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double ammopointX, double ammopointY, int ammopointIndex) = AmmoPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double steelpointX, double steelpointY, int steelpointIndex) = SteelPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double bauxpointX, double bauxpointY, int bauxpointIndex) = BauxPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double instantrepairpointX, double instantrepairpointY, int instantrepairpointIndex) = InstantRepairPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			DateTime date = DateTime.FromOADate(fuelpointX);
			toolTip.Content = string.Format("{0}\n{6}: {1}\n{7}:{2}\n{8}: {3}\n{9}: {4}\n{10}: {5}", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
		}
		else if (SelectedChartType == ChartType.ResourceDiff)
		{
			(double fuelpointX, double fuelpointY, int fuelpointIndex) = FuelSignalPlot.GetPointNearestX(mouseCoordX);
			(double ammopointX, double ammopointY, int ammopointIndex) = AmmoSignalPlot.GetPointNearestX(mouseCoordX);
			(double steelpointX, double steelpointY, int steelpointIndex) = SteelSignalPlot.GetPointNearestX(mouseCoordX);
			(double bauxpointX, double bauxpointY, int bauxpointIndex) = BauxSignalPlot.GetPointNearestX(mouseCoordX);
			(double instantrepairpointX, double instantrepairpointY, int instantrepairpointIndex) = InstantRepairSignalPlot.GetPointNearestX(mouseCoordX);
			DateTime date = DateTime.FromOADate(fuelpointX);
			if (Menu_Option_DivideByDay.IsChecked)
			{
				toolTip.Content = string.Format("{0}\n{6}: {1:+0;-0;±0} /day \n{7}:{2:+0;-0;±0} /day \n{8}: {3:+0;-0;±0}/day \n{9}: {4:+0;-0;±0} /day\n{10}: {5:+0;-0;±0} /day", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
			}
			else
			{
				toolTip.Content = string.Format("{0}\n{6}: {1}\n{7}:{2}\n{8}: {3}\n{9}: {4}\n{10}: {5}", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
			}
		}
		else if (SelectedChartType == ChartType.Material)
		{
			(double instantconstructionpointX, double instantconstructionpointY, int instantconstructionpointpointIndex) = InstantConstructionPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double moddingmaterialpointX, double moddingmaterialpointY, int moddingmaterialpointIndex) = ModdingMaterialPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double developmentmaterialpointX, double developmentmaterialpointY, int developmentmaterialpointIndex) = DevelopmentMaterialPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			(double instantrepairpointX, double instantrepairpointY, int instantrepairpointIndex) = InstantRepairPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			DateTime date = DateTime.FromOADate(instantconstructionpointX);
			toolTip.Content = string.Format("{0}\n{5}: {1}\n{6}:{2}\n{7}: {3}\n{8}: {4}", date, developmentmaterialpointY, moddingmaterialpointY, instantconstructionpointY, instantrepairpointY, development_material, modding_material, instant_construction, instant_repair);
		}
		else if (SelectedChartType == ChartType.MaterialDiff)
		{
			(double instantconstructionpointX, double instantconstructionpointY, int instantconstructionpointpointIndex) = InstantConstructionSignalPlot.GetPointNearestX(mouseCoordX);
			(double moddingmaterialpointX, double moddingmaterialpointY, int moddingmaterialpointIndex) = ModdingMaterialSignalPlot.GetPointNearestX(mouseCoordX);
			(double developmentmaterialpointX, double developmentmaterialpointY, int developmentmaterialpointIndex) = DevelopmentMaterialSignalPlot.GetPointNearestX(mouseCoordX);
			(double instantrepairpointX, double instantrepairpointY, int instantrepairpointIndex) = InstantRepairSignalPlot.GetPointNearestX(mouseCoordX);
			DateTime date = DateTime.FromOADate(instantconstructionpointX);
			if (Menu_Option_DivideByDay.IsChecked)
			{
				toolTip.Content = string.Format("{0}\n{5}: {1:+0;-0;±0} /day\n{6}:{2:+0;-0;±0} /day\n{7}: {3:+0;-0;±0} /day\n{8}: {4:+0;-0;±0} /day", date, developmentmaterialpointY, moddingmaterialpointY, instantconstructionpointY, instantrepairpointY, development_material, modding_material, instant_construction, instant_repair); ;
			}
			else
			{
				toolTip.Content = string.Format("{0}\n{5}: {1}\n{6}:{2}\n{7}: {3}\n{8}: {4}", date, developmentmaterialpointY, moddingmaterialpointY, instantconstructionpointY, instantrepairpointY, development_material, modding_material, instant_construction, instant_repair); ;
			}
		}
		else if (SelectedChartType == ChartType.Experience)
		{
			(double experiencepointX, double experiencepointY, int experiencepointIndex) = ExperiencePlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);
			DateTime date = DateTime.FromOADate(experiencepointX);
			toolTip.Content = string.Format("{0}\n{1}: {2}", date, experience, experiencepointY);
		}
		else if (SelectedChartType == ChartType.ExperienceDiff)
		{
			(double experiencepointX, double experiencepointY, int experiencepointIndex) = ExperienceSignalPlot.GetPointNearestX(mouseCoordX);
			DateTime date = DateTime.FromOADate(experiencepointX);
			if (Menu_Option_DivideByDay.IsChecked)
			{
				toolTip.Content = string.Format("{0}\n{2}: {1:+0;-0;±0} /day", date, experiencepointY, experience);
			}
			else
			{
				toolTip.Content = string.Format("{0}\n{2}: {1:+0;-0;±0}", date, experiencepointY, experience);
			}
		}
		else
		{
			toolTip = null;
		}
		// update the GUI to describe the highlighted point
		double mouseX = e.GetPosition(this).X;
		double mouseY = e.GetPosition(this).Y;

		toolTip.HorizontalOffset = mouseX;
		toolTip.VerticalOffset = mouseY - 20;
	}

	private void SetResourceChart()
	{
		ChartArea.Plot.Clear();

		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Resource");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		AxisXIntervals(SelectedChartSpan);
		ViewModel.ShowFuel = true;
		ViewModel.ShowAmmo = true;
		ViewModel.ShowBaux = true;
		ViewModel.ShowSteel = true;
		ResourcesPanel.Visibility = Visibility.Visible;
		MaterialPanel.Visibility = Visibility.Collapsed;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ViewModel.ShowInstantRepair = true;
		List<double>? fuel_list = Array.Empty<double>().ToList();

		List<double>? ammo_list = Array.Empty<double>().ToList();

		List<double>? baux_list = Array.Empty<double>().ToList();

		List<double>? steel_list = Array.Empty<double>().ToList();

		List<double>? instant_repair_list = Array.Empty<double>().ToList();
		ChartArea.Plot.YAxis2.Ticks(true);
		ChartArea.Plot.YAxis2.MajorGrid(true);
		List<double>? date_list = Array.Empty<double>().ToList();

		{
			var record = GetRecords();

			ResourceRecord.ResourceElement prev = null;
			if (record.Any())
			{
				prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;

					date_list.Add(r.Date.ToOADate());
					fuel_list.Add(r.Fuel);
					ammo_list.Add(r.Ammo);
					baux_list.Add(r.Bauxite);
					steel_list.Add(r.Steel);
					instant_repair_list.Add(r.InstantRepair);

					prev = r;
				}
			}
		}
		FuelPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), fuel_list.ToArray(), FuelColor, label: "Fuel");
		AmmoPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), ammo_list.ToArray(), AmmoColor, label: "Ammo");
		BauxPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), baux_list.ToArray(), BauxColor, label: "Bauxite");
		SteelPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), steel_list.ToArray(), SteelColor, label: "Steel");
		InstantRepairPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), instant_repair_list.ToArray(), InstantRepairColor, label: "Instant Repair");
		InstantRepairPlot.YAxisIndex = 1;
		ChartArea.Refresh();
	}
	private void SetResourceDiffChart()
	{
		ChartArea.Plot.Clear();

		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Resource");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		AxisXIntervals(SelectedChartSpan);
		ViewModel.ShowFuel = true;
		ViewModel.ShowAmmo = true;
		ViewModel.ShowBaux = true;
		ViewModel.ShowSteel = true;
		ResourcesPanel.Visibility = Visibility.Visible;
		MaterialPanel.Visibility = Visibility.Collapsed;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ViewModel.ShowInstantRepair = true;
		List<double>? fuel_list = Array.Empty<double>().ToList();

		List<double>? ammo_list = Array.Empty<double>().ToList();

		List<double>? baux_list = Array.Empty<double>().ToList();

		List<double>? steel_list = Array.Empty<double>().ToList();

		List<double>? instant_repair_list = Array.Empty<double>().ToList();
		ChartArea.Plot.YAxis2.Ticks(true);
		ChartArea.Plot.YAxis2.MajorGrid(true);
		List<double>? date_list = Array.Empty<double>().ToList();

		{
			var record = GetRecords();

			ResourceRecord.ResourceElement prev = null;
			if (record.Any())
			{
				prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;

					double[] ys = new double[] {
						r.Fuel - prev.Fuel,
						r.Ammo - prev.Ammo,
						r.Steel - prev.Steel,
						r.Bauxite - prev.Bauxite,
						r.InstantRepair - prev.InstantRepair };
					if (Menu_Option_DivideByDay.IsChecked)
					{
						for (int i = 0; i < 4; i++)
							ys[i] /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);
					}

					date_list.Add(r.Date.ToOADate());

					fuel_list.Add(ys[0]);
					ammo_list.Add(ys[1]);
					steel_list.Add(ys[2]);
					baux_list.Add(ys[3]);
					instant_repair_list.Add(ys[4]);

					prev = r;
				}
			}
		}
		InstantRepairSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), instant_repair_list.ToArray());
		InstantRepairSignalPlot.StepDisplay = true;
		InstantRepairSignalPlot.FillAboveAndBelow(InstantRepairColor, Color.Transparent, Color.Transparent, InstantRepairColor, 1);
		InstantRepairSignalPlot.Label = "Instant Repair";
		InstantRepairSignalPlot.MarkerSize = 0;
		InstantRepairSignalPlot.YAxisIndex = 1;

		FuelSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), fuel_list.ToArray());
		FuelSignalPlot.StepDisplay = true;
		FuelSignalPlot.FillAboveAndBelow(FuelColor, Color.Transparent, Color.Transparent, FuelColor, 1);
		FuelSignalPlot.Label = "Fuel";
		FuelSignalPlot.MarkerSize = 0;

		AmmoSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), ammo_list.ToArray());
		AmmoSignalPlot.StepDisplay = true;
		AmmoSignalPlot.FillAboveAndBelow(AmmoColor, Color.Transparent, Color.Transparent, AmmoColor, 1);
		AmmoSignalPlot.Label = "Ammo";
		AmmoSignalPlot.MarkerSize = 0;

		SteelSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), steel_list.ToArray());
		SteelSignalPlot.StepDisplay = true;
		SteelSignalPlot.FillAboveAndBelow(SteelColor, Color.Transparent, Color.Transparent, SteelColor, 1);
		SteelSignalPlot.Label = "Steel";
		SteelSignalPlot.MarkerSize = 0;

		BauxSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), baux_list.ToArray());
		BauxSignalPlot.StepDisplay = true;
		BauxSignalPlot.FillAboveAndBelow(BauxColor, Color.Transparent, Color.Transparent, BauxColor, 1);
		BauxSignalPlot.Label = "Bauxite";
		BauxSignalPlot.MarkerSize = 0;

		ChartArea.Refresh();
	}
	private void SetMaterialDiffChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Visible;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ChartArea.Plot.YAxis2.IsVisible = false;
		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Material");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		AxisXIntervals(SelectedChartSpan);
		ViewModel.ShowDevelopmentMaterial = true;
		ViewModel.ShowModdingMaterial = true;
		ViewModel.ShowInstantRepair = true;
		ViewModel.ShowInstantConstruction = true;
		List<double>? instant_repair_list = Array.Empty<double>().ToList();
		List<double>? development_material_list = Array.Empty<double>().ToList();
		List<double>? modding_material_list = Array.Empty<double>().ToList();
		List<double>? instant_contruction_list = Array.Empty<double>().ToList();

		List<double>? date_list = Array.Empty<double>().ToList();
		{
			var record = GetRecords();

			ResourceRecord.ResourceElement prev = null;
			if (record.Any())
			{
				prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;
					double[] ys = new double[] {
						r.InstantConstruction - prev.InstantConstruction ,
						r.InstantRepair - prev.InstantRepair,
						r.DevelopmentMaterial - prev.DevelopmentMaterial ,
						r.ModdingMaterial - prev.ModdingMaterial };

					if (Menu_Option_DivideByDay.IsChecked)
					{
						for (int i = 0; i < 4; i++)
							ys[i] /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);
					}
					date_list.Add(r.Date.ToOADate());
					instant_contruction_list.Add(ys[0]);
					instant_repair_list.Add(ys[1]);
					development_material_list.Add(ys[2]);
					modding_material_list.Add(ys[3]);

					prev = r;
				}
			}
		}
		InstantRepairSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), instant_repair_list.ToArray());
		InstantRepairSignalPlot.StepDisplay = true;
		InstantRepairSignalPlot.FillAboveAndBelow(InstantRepairColor, Color.Transparent, Color.Transparent, InstantRepairColor, 1);
		InstantRepairSignalPlot.Label = "Instant Repair";
		InstantRepairSignalPlot.MarkerSize = 0;

		ModdingMaterialSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), modding_material_list.ToArray());
		ModdingMaterialSignalPlot.StepDisplay = true;
		ModdingMaterialSignalPlot.Label = "Modding Material";
		ModdingMaterialSignalPlot.FillAboveAndBelow(ModdingMaterialColor, Color.Transparent, Color.Transparent, ModdingMaterialColor, 1);
		ModdingMaterialSignalPlot.MarkerSize = 0;

		DevelopmentMaterialSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), development_material_list.ToArray());
		DevelopmentMaterialSignalPlot.StepDisplay = true;
		DevelopmentMaterialSignalPlot.Label = "Development Material";
		DevelopmentMaterialSignalPlot.FillAboveAndBelow(DevelopmentMaterialColor, Color.Transparent, Color.Transparent,DevelopmentMaterialColor, 1);
		DevelopmentMaterialSignalPlot.MarkerSize = 0;

		InstantConstructionSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), instant_contruction_list.ToArray());
		InstantConstructionSignalPlot.StepDisplay = true;
		InstantConstructionSignalPlot.Label = "Instant Construction";
		InstantConstructionSignalPlot.FillAboveAndBelow(InstantConstructionColor, Color.Transparent, Color.Transparent, InstantConstructionColor, 1);
		InstantConstructionSignalPlot.MarkerSize = 0;

		ChartArea.Refresh();
	}
	private bool ShouldSkipRecord(TimeSpan span)
	{
		if (Menu_Option_ShowAllData.IsChecked)
			return false;

		if (span.Ticks == 0)        //初回のデータ( prev == First )は無視しない
			return false;

		switch (SelectedChartSpan)
		{
			case ChartSpan.Day:
			case ChartSpan.Week:
			case ChartSpan.WeekFirst:
			default:
				return false;
			case ChartSpan.Month:
			case ChartSpan.MonthFirst:
				return span.TotalHours < 12.0;
			case ChartSpan.Season:
			case ChartSpan.SeasonFirst:
			case ChartSpan.Year:
			case ChartSpan.YearFirst:
			case ChartSpan.All:
				return span.TotalDays < 1.0;
		}
	}

	private void SetYBounds(double min, double max)
	{
		int order = (int)Math.Log10(Math.Max(max - min, 1));
		double powered = Math.Pow(10, order);
		double unitbase = Math.Round((max - min) / powered);
		double unit = powered * (
			unitbase < 2 ? 0.2 :
			unitbase < 5 ? 0.5 :
			unitbase < 7 ? 1.0 : 2.0);

		//ResourceChart.ChartAreas[0].AxisY.Minimum = Math.Floor(min / unit) * unit;
		//ResourceChart.ChartAreas[0].AxisY.Maximum = Math.Ceiling(max / unit) * unit;

		//ResourceChart.ChartAreas[0].AxisY.Interval = unit;
		//ResourceChart.ChartAreas[0].AxisY.MinorGrid.Interval = unit / 2;

		//if (ResourceChart.Series.Where(s => s.Enabled).Any(s => s.YAxisType == AxisType.Secondary))
		//{
		//	ResourceChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
		//	if (ResourceChart.Series.Count(s => s.Enabled) == 1)
		//	{
		//		ResourceChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = true;
		//		ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Enabled = true;
		//	}
		//	else
		//	{
		//		ResourceChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
		//		ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Enabled = false;
		//	}
		//	ResourceChart.ChartAreas[0].AxisY2.Minimum = ResourceChart.ChartAreas[0].AxisY.Minimum / 100;
		//	ResourceChart.ChartAreas[0].AxisY2.Maximum = ResourceChart.ChartAreas[0].AxisY.Maximum / 100;
		//	ResourceChart.ChartAreas[0].AxisY2.Interval = unit / 100;
		//	ResourceChart.ChartAreas[0].AxisY2.MinorGrid.Interval = unit / 200;
		//}
		//else
		//{
		//	//ResourceChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;
		//}
	}

	private void SetMaterialChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Visible;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ChartArea.Plot.YAxis2.IsVisible = false;
		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Material");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		AxisXIntervals(SelectedChartSpan);
		ViewModel.ShowModdingMaterial = true;
		ViewModel.ShowDevelopmentMaterial = true;
		ViewModel.ShowInstantRepair = true;
		ViewModel.ShowInstantConstruction = true;
		List<double>? instant_repair_list = Array.Empty<double>().ToList();
		List<double>? development_material_list = Array.Empty<double>().ToList();
		List<double>? modding_material_list = Array.Empty<double>().ToList();
		List<double>? instant_contruction_list = Array.Empty<double>().ToList();

		List<double>? date_list = Array.Empty<double>().ToList();
		{
			var record = GetRecords();

			ResourceRecord.ResourceElement prev = null;
			if (record.Any())
			{
				prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;

					date_list.Add(r.Date.ToOADate());
					instant_repair_list.Add(r.InstantRepair);
					development_material_list.Add(r.DevelopmentMaterial);
					modding_material_list.Add(r.ModdingMaterial);
					instant_contruction_list.Add(r.InstantConstruction);
					prev = r;
				}
			}
		}
		InstantRepairPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), instant_repair_list.ToArray(), InstantRepairColor, label: "Instant Repair");
		DevelopmentMaterialPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), development_material_list.ToArray(), DevelopmentMaterialColor, label: "Development Material");
		ModdingMaterialPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), modding_material_list.ToArray(), ModdingMaterialColor, label: "Modding Material");
		InstantConstructionPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), instant_contruction_list.ToArray(), InstantConstructionColor, label: "Instant Construction");
		ChartArea.Refresh();
	}

	private void SetExperienceChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Collapsed;
		ExperiencePanel.Visibility = Visibility.Visible;
		AxisXIntervals(SelectedChartSpan);
		ChartArea.Plot.YAxis2.IsVisible = false;
		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Experience");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		ViewModel.ShowExperience = true;
		List<double>? experience_list = Array.Empty<double>().ToList();

		List<double>? date_list = Array.Empty<double>().ToList();

		{
			var record = GetRecords();

			if (record.Any())
			{
				var prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;

					experience_list.Add(r.HQExp);
					date_list.Add(r.Date.ToOADate());
					prev = r;
				}
			}
		}
		ExperiencePlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), experience_list.ToArray(), ExperienceColor, label: "HQ Experience");
		ChartArea.Refresh();
	}

	private void SetYBounds()
	{
		//SetYBounds(
		//	!ResourceChart.Series.Any(s => s.Enabled) || SelectedChartType == ChartType.ExperienceDiff ? 0 : ResourceChart.Series.Where(s => s.Enabled).Select(s => s.YAxisType == AxisType.Secondary ? s.Points.Min(p => p.YValues[0] * 100) : s.Points.Min(p => p.YValues[0])).Min(),
		//	!ResourceChart.Series.Any(s => s.Enabled) ? 0 : ResourceChart.Series.Where(s => s.Enabled).Select(s => s.YAxisType == AxisType.Secondary ? s.Points.Max(p => p.YValues[0] * 100) : s.Points.Max(p => p.YValues[0])).Max()
		//);
	}

	private void AxisXIntervals(ChartSpan span)
	{
		var axis = ChartArea.Plot.XAxis;
		switch (span)
		{
			case ChartSpan.Day:
				axis.TickLabelFormat("MM/dd HH:mm", true);
				axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Hour);
				axis.TickLabelStyle(rotation: 90);
				break;

			case ChartSpan.Week:
			case ChartSpan.WeekFirst:
				axis.TickLabelFormat("MM/dd HH:mm", true);
				axis.ManualTickSpacing(12, ScottPlot.Ticks.DateTimeUnit.Hour);
				axis.TickLabelStyle(rotation: 90);
				break;

			case ChartSpan.Month:
			case ChartSpan.MonthFirst:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Day);
				axis.TickLabelStyle(rotation: 90);
				break;

			case ChartSpan.Season:
			case ChartSpan.SeasonFirst:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.ManualTickSpacing(7, ScottPlot.Ticks.DateTimeUnit.Day);
				axis.TickLabelStyle(rotation: 90);
				break;

			case ChartSpan.Year:
			case ChartSpan.YearFirst:
			case ChartSpan.All:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.TickLabelStyle(rotation: 90);
				axis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Month);
				break;
		}
	}

	private void ChartSpan_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartSpanMenu, ((MenuItem)sender).Tag);
		UpdateChart();
	}

	private void UpdateChart()
	{
		switch (SelectedChartType)
		{
			case ChartType.Resource:
				SetResourceChart();
				break;
			case ChartType.ResourceDiff:
				SetResourceDiffChart();
				break;
			case ChartType.Material:
				SetMaterialChart();
				break;
			case ChartType.MaterialDiff:
				SetMaterialDiffChart();
				break;
			case ChartType.Experience:
				SetExperienceChart();
				break;
			case ChartType.ExperienceDiff:
				SetExperienceDiffChart();
				break;
		}
	}

	private void SetExperienceDiffChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Collapsed;
		ExperiencePanel.Visibility = Visibility.Visible;
		AxisXIntervals(SelectedChartSpan);
		ChartArea.Plot.YAxis2.IsVisible = false;
		ChartArea.Plot.XAxis.Label("Date");
		ChartArea.Plot.YAxis.Label("Experience");
		ChartArea.Plot.XAxis.DateTimeFormat(true);
		ViewModel.ShowExperience = true;
		List<double>? experience_list = Array.Empty<double>().ToList();

		List<double>? date_list = Array.Empty<double>().ToList();

		{
			var record = GetRecords();

			if (record.Any())
			{
				var prev = record.First();
				foreach (var r in record)
				{
					if (ShouldSkipRecord(r.Date - prev.Date))
						continue;
					double ys = r.HQExp - prev.HQExp;
					if (Menu_Option_DivideByDay.IsChecked)
						ys /= Math.Max((r.Date - prev.Date).TotalDays, 1.0 / 1440.0);

					experience_list.Add(ys);
					date_list.Add(r.Date.ToOADate());
					prev = r;
				}
			}
		}
		ExperienceSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), experience_list.ToArray());
		ExperienceSignalPlot.StepDisplay = true;
		ExperienceSignalPlot.FillAboveAndBelow(ExperienceColor, Color.Transparent, Color.Transparent, ExperienceColor, 1);
		ExperienceSignalPlot.Label = "HQ Experience";
		ExperienceSignalPlot.MarkerSize = 0;
		ChartArea.Refresh();
	}

	private void SwitchMenuStrip(MenuItem parent, object index)
	{
		int intindex = int.Parse((string)index);
		var items = parent.Items.OfType<MenuItem>();
		int c = 0;

		foreach (var item in items)
		{
			item.IsChecked = intindex == c;
			c++;
		}
		parent.Tag = intindex;
	}

	private int GetSelectedMenuStripIndex(MenuItem parent)
	{
		return parent.Tag as int? ?? -1;
	}

	private IEnumerable<ResourceRecord.ResourceElement> GetRecords()
	{
		var border = DateTime.MinValue;
		var now = DateTime.Now;

		switch (SelectedChartSpan)
		{
			case ChartSpan.Day:
				border = now.AddDays(-1);
				break;

			case ChartSpan.Week:
				border = now.AddDays(-7);
				break;

			case ChartSpan.Month:
				border = now.AddMonths(-1);
				break;

			case ChartSpan.Season:
				border = now.AddMonths(-3);
				break;

			case ChartSpan.Year:
				border = now.AddYears(-1);
				break;

			case ChartSpan.WeekFirst:
				border = now.AddDays(now.DayOfWeek == DayOfWeek.Sunday ? -6 : (1 - (int)now.DayOfWeek));
				break;

			case ChartSpan.MonthFirst:
				border = new DateTime(now.Year, now.Month, 1);
				break;

			case ChartSpan.SeasonFirst:
			{
				int m = now.Month / 3 * 3;
				if (m == 0)
					m = 12;
				border = new DateTime(now.Year - (now.Month < 3 ? 1 : 0), m, 1);
			}
			break;

			case ChartSpan.YearFirst:
				border = new DateTime(now.Year, 1, 1);
				break;
		}

		foreach (var r in RecordManager.Instance.Resource.Record)
		{
			if (r.Date >= border)
				yield return r;
		}

		var material = KCDatabase.Instance.Material;
		var admiral = KCDatabase.Instance.Admiral;
		if (material.IsAvailable && admiral.IsAvailable)
		{
			yield return new ResourceRecord.ResourceElement(
				material.Fuel, material.Ammo, material.Steel, material.Bauxite,
				material.InstantConstruction, material.InstantRepair, material.DevelopmentMaterial, material.ModdingMaterial,
				admiral.Level, admiral.Exp);
		}
	}

	private void MaterialMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "2");
		UpdateChart();
	}

	private void ResourceMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "0");
		UpdateChart();
	}

	private void MaterialDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "3");
		UpdateChart();
	}

	private void ExperienceMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "4");
		UpdateChart();
	}

	private void ExperienceDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "5");
		UpdateChart();
	}

	private void ResourceDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "1");
		UpdateChart();
	}

	private void Menu_Option_ShowAllData_Click(object sender, RoutedEventArgs e)
	{
		UpdateChart();
	}

	private void Menu_Option_DivideByDay_Click(object sender, RoutedEventArgs e)
	{
		UpdateChart();
	}

	private void FileSaveImage_Click(object sender, RoutedEventArgs e)
	{
		var sfd = new SaveFileDialog
		{
			FileName = "ResourceChart.png",
			Filter = "PNG Files (*.png)|*.png;*.png" +
			 "|JPG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
			 "|BMP Files (*.bmp)|*.bmp;*.bmp" +
			 "|All files (*.*)|*.*"
		};

		if (sfd.ShowDialog() is true)
			ChartArea.Plot.SaveFig(sfd.FileName);
	}
}