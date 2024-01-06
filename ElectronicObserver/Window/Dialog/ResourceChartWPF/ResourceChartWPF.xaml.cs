using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using Microsoft.Win32;
using ScottPlot.Plottable;

namespace ElectronicObserver.Window.Dialog.ResourceChartWPF;

/// <summary>
/// Interaction logic for ResourceChartWPF.xaml
/// </summary>
public partial class ResourceChartWPF
{
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
			default:
				return Color.FromArgb(255 - 64, 255 - 64, 255 - 64);
		}
	}

	private Color InstantRepairColor => Color.FromArgb(32, 128, 255);
	private Color InstantRepairMatColor => Color.FromArgb(0, 128, 0);
	private Color InstantConstructionColor => Color.FromArgb(255, 128, 0);
	private Color DevelopmentMaterialColor => Color.FromArgb(0, 0, 255);
	private Color ModdingMaterialColor => GetSteelColor(); //use steel color for modding material
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
		Custom,
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
	private ScatterPlot? ExperiencePlot;
	private SignalPlotXY ExperienceSignalPlot;
	private ResourceRecord _record;

	private ChartType SelectedChartType => (ChartType)GetSelectedMenuStripIndex(ChartTypeMenu);
	private ChartSpan SelectedChartSpan => (ChartSpan)GetSelectedMenuStripIndex(ChartSpanMenu);

	private List<ScatterPlot?> ResourcePlots => new()
	{
		FuelPlot,
		AmmoPlot,
		SteelPlot,
		BauxPlot,
		InstantRepairPlot,
	};

	private List<SignalPlotXY?> ResourceDiffPlots => new()
	{
		FuelSignalPlot,
		AmmoSignalPlot,
		SteelSignalPlot,
		BauxSignalPlot,
		InstantRepairSignalPlot,
	};

	private List<ScatterPlot?> MaterialPlots => new()
	{
		InstantConstructionPlot,
		InstantRepairPlot,
		DevelopmentMaterialPlot,
		ModdingMaterialPlot,
	};

	private List<SignalPlotXY?> MaterialDiffPlots => new()
	{
		InstantConstructionSignalPlot,
		InstantRepairSignalPlot,
		DevelopmentMaterialSignalPlot,
		ModdingMaterialSignalPlot,
	};

	private List<ScatterPlot?> ExperiencePlots => new()
	{
		ExperiencePlot,
	};
	private List<SignalPlotXY?> ExperienceDiffPlots => new()
	{
		ExperienceSignalPlot,
	};

	private List<ScatterPlot?> CurrentScatterPlots => SelectedChartType switch
	{
		ChartType.Resource => ResourcePlots,
		ChartType.Material => MaterialPlots,
		ChartType.Experience => ExperiencePlots,
		_ => new()
	};

	private List<SignalPlotXY?> CurrentSignalPlots => SelectedChartType switch
	{
		ChartType.ResourceDiff => ResourceDiffPlots,
		ChartType.MaterialDiff => MaterialDiffPlots,
		ChartType.ExperienceDiff => ExperienceDiffPlots,
		_ => new()
	};

	public ResourceChartWPF() : base(new ResourceChartViewModel())
	{
		InitializeComponent();
		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		Loaded += ChartArea_Loaded;
		ConfigurationChanged();
		#region Chart Toggles
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowFuel)) return;

			if (FuelPlot is not null)
			{
				FuelPlot.IsVisible = ViewModel.ShowFuel;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (FuelSignalPlot is not null)
			{
				FuelSignalPlot.IsVisible = ViewModel.ShowFuel;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowAmmo)) return;

			if (AmmoPlot is not null)
			{
				AmmoPlot.IsVisible = ViewModel.ShowAmmo;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (AmmoSignalPlot is not null)
			{
				AmmoSignalPlot.IsVisible = ViewModel.ShowAmmo;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowSteel)) return;

			if (SteelPlot is not null)
			{
				SteelPlot.IsVisible = ViewModel.ShowSteel;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (SteelSignalPlot is not null)
			{
				SteelSignalPlot.IsVisible = ViewModel.ShowSteel;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowBaux)) return;

			if (BauxPlot is not null)
			{
				BauxPlot.IsVisible = ViewModel.ShowBaux;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (BauxSignalPlot is not null)
			{
				BauxSignalPlot.IsVisible = ViewModel.ShowBaux;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowInstantRepair)) return;

			if (InstantRepairPlot is not null)
			{
				InstantRepairPlot.IsVisible = ViewModel.ShowInstantRepair;
				if (SelectedChartType == ChartType.Resource || SelectedChartType == ChartType.ResourceDiff)
				{
					ChartArea.Plot.YAxis2.IsVisible = ViewModel.ShowInstantRepair;
				}
				SetYBounds();
				ChartArea.Refresh();
			}

			if (InstantRepairSignalPlot is not null)
			{
				InstantRepairSignalPlot.IsVisible = ViewModel.ShowInstantRepair;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowInstantConstruction)) return;

			if (InstantConstructionPlot is not null)
			{
				InstantConstructionPlot.IsVisible = ViewModel.ShowInstantConstruction;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (InstantConstructionSignalPlot is not null)
			{
				InstantConstructionSignalPlot.IsVisible = ViewModel.ShowInstantConstruction;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowDevelopmentMaterial)) return;

			if (DevelopmentMaterialPlot is not null)
			{
				DevelopmentMaterialPlot.IsVisible = ViewModel.ShowDevelopmentMaterial;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (DevelopmentMaterialSignalPlot is not null)
			{
				DevelopmentMaterialSignalPlot.IsVisible = ViewModel.ShowDevelopmentMaterial;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowModdingMaterial)) return;

			if (ModdingMaterialPlot is not null)
			{
				ModdingMaterialPlot.IsVisible = ViewModel.ShowModdingMaterial;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (ModdingMaterialSignalPlot is not null)
			{
				ModdingMaterialSignalPlot.IsVisible = ViewModel.ShowModdingMaterial;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ViewModel.ShowExperience)) return;

			if (ExperiencePlot is not null)
			{
				ExperiencePlot.IsVisible = ViewModel.ShowExperience;
				SetYBounds();
				ChartArea.Refresh();
			}

			if (ExperienceSignalPlot is not null)
			{
				ExperienceSignalPlot.IsVisible = ViewModel.ShowExperience;
				SetYBounds();
				ChartArea.Refresh();
			}
			CheckChartVisiblity();
		};
		#endregion

		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(ViewModel.DateBegin) or nameof(ViewModel.DateEnd))) return;

			UpdateChart();
		};
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
		ChartArea.Plot.XAxis.TickLabelStyle(rotation: 90, fontName: c.UI.SubFont.FontData.Name, fontSize: c.UI.SubFont.FontData.Size);
		ChartArea.Plot.YAxis.TickLabelStyle(fontName: c.UI.SubFont.FontData.Name, fontSize: c.UI.SubFont.FontData.Size);
		ChartArea.Plot.YAxis2.TickLabelStyle(fontName: c.UI.SubFont.FontData.Name, fontSize: c.UI.SubFont.FontData.Size);
	}

	private void ChartArea_Loaded(object sender, RoutedEventArgs e)
	{
		if (!RecordManager.Instance.Resource.Record.Any())
		{
			System.Windows.Forms.MessageBox.Show(ResourceChartResources.RecordDataDoesNotExist, ResourceChartResources.Error, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			Close();
			return;
		}
		_record = RecordManager.Instance.Resource;
		ViewModel.DateBegin = _record.Record.First().Date.Date;
		ViewModel.MinDate = _record.Record.First().Date.Date;

		ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
		ViewModel.MaxDate = DateTime.Now.AddDays(1).Date;
		SwitchMenuStrip(ChartSpanMenu, "2");
		SwitchMenuStrip(ChartTypeMenu, "0");

		ChartArea.Configuration.Zoom = false;
		ChartArea.Configuration.Pan = false;
		ChartArea.RightClicked -= ChartArea.DefaultRightClickEvent;
		ChartArea.Configuration.DoubleClickBenchmark = false;
		SetDateRange();
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
			double fuelpointY = FuelPlot.GetPointNearestX(mouseCoordX).y;
			double ammopointY = AmmoPlot.GetPointNearestX(mouseCoordX).y;
			double steelpointY = SteelPlot.GetPointNearestX(mouseCoordX).y;
			double bauxpointY = BauxPlot.GetPointNearestX(mouseCoordX).y;
			double instantrepairpointY = InstantRepairPlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentScatterPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;
			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX.Value));
				ViewModel.ToolTip = string.Format("{0}\n{6}: {1}\n{7}: {2}\n{8}: {3}\n{9}: {4}\n{10}: {5}", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
			}
		}
		else if (SelectedChartType == ChartType.ResourceDiff)
		{
			double fuelpointY = FuelSignalPlot.GetPointNearestX(mouseCoordX).y;
			double ammopointY = AmmoSignalPlot.GetPointNearestX(mouseCoordX).y;
			double steelpointY = SteelSignalPlot.GetPointNearestX(mouseCoordX).y;
			double bauxpointY = BauxSignalPlot.GetPointNearestX(mouseCoordX).y;
			double instantrepairpointY = InstantRepairSignalPlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentSignalPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;

			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX.Value));

				if (Menu_Option_DivideByDay.IsChecked)
				{
					ViewModel.ToolTip = string.Format("{0}\n{6}: {1:+0;-0;±0} /day \n{7}:{2:+0;-0;±0} /day \n{8}: {3:+0;-0;±0}/day \n{9}: {4:+0;-0;±0} /day\n{10}: {5:+0;-0;±0} /day", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
				}
				else
				{
					ViewModel.ToolTip = string.Format("{0}\n{6}: {1}\n{7}:{2}\n{8}: {3}\n{9}: {4}\n{10}: {5}", date, fuelpointY, ammopointY, steelpointY, bauxpointY, instantrepairpointY, fuel, ammo, steel, baux, instant_repair);
				}
			}
		}
		else if (SelectedChartType == ChartType.Material)
		{
			double instantconstructionpointY = InstantConstructionPlot.GetPointNearestX(mouseCoordX).y;
			double moddingmaterialpointY = ModdingMaterialPlot.GetPointNearestX(mouseCoordX).y;
			double developmentmaterialpointY = DevelopmentMaterialPlot.GetPointNearestX(mouseCoordX).y;
			double instantrepairpointY = InstantRepairPlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentScatterPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;

			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX.Value));
				ViewModel.ToolTip = string.Format("{0}\n{5}: {1}\n{6}: {2}\n{7}: {3}\n{8}: {4}", date, instantconstructionpointY, instantrepairpointY, developmentmaterialpointY, moddingmaterialpointY, instant_construction, instant_repair, development_material, modding_material);
			}
		}
		else if (SelectedChartType == ChartType.MaterialDiff)
		{
			double instantconstructionpointY = InstantConstructionSignalPlot.GetPointNearestX(mouseCoordX).y;
			double moddingmaterialpointY = ModdingMaterialSignalPlot.GetPointNearestX(mouseCoordX).y;
			double developmentmaterialpointY = DevelopmentMaterialSignalPlot.GetPointNearestX(mouseCoordX).y;
			double instantrepairpointY = InstantRepairSignalPlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentSignalPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;

			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX.Value));

				if (Menu_Option_DivideByDay.IsChecked)
				{
					ViewModel.ToolTip = string.Format("{0}\n{5}: {1:+0;-0;±0} /day\n{6}:{2:+0;-0;±0} /day\n{7}: {3:+0;-0;±0} /day\n{8}: {4:+0;-0;±0} /day", date, instantconstructionpointY, instantrepairpointY, developmentmaterialpointY, moddingmaterialpointY, instant_construction, instant_repair, development_material, modding_material);
				}
				else
				{
					ViewModel.ToolTip = string.Format("{0}\n{5}: {1}\n{6}:{2}\n{7}: {3}\n{8}: {4}", date, instantconstructionpointY, instantrepairpointY, developmentmaterialpointY, moddingmaterialpointY, instant_construction, instant_repair, development_material, modding_material);
				}
			}
		}
		else if (SelectedChartType == ChartType.Experience)
		{
			double experiencepointY = ExperiencePlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentScatterPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;

			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX.Value));
				ViewModel.ToolTip = string.Format("{0}\n{1}: {2}", date, experience, experiencepointY);
			}
		}
		else if (SelectedChartType == ChartType.ExperienceDiff)
		{
			double experiencepointY = ExperienceSignalPlot.GetPointNearestX(mouseCoordX).y;

			double? datePointX = CurrentSignalPlots.FirstOrDefault(c => c?.IsVisible ?? false)?.GetPointNearestX(mouseCoordX).x;

			if (datePointX is null)
			{
				ViewModel.ToolTip = null;
			}
			else
			{
				string date = DateTimeHelper.TimeToCSVString(DateTime.FromOADate(datePointX ?? 0));

				if (Menu_Option_DivideByDay.IsChecked)
				{
					ViewModel.ToolTip = string.Format("{0}\n{2}: {1:+0;-0;±0} /day", date, experiencepointY, experience);
				}
				else
				{
					ViewModel.ToolTip = string.Format("{0}\n{2}: {1:+0;-0;±0}", date, experiencepointY, experience);
				}
			}
		}
		else
		{
			ViewModel.ToolTip = null;
		}

		// update the GUI to describe the highlighted point
		ViewModel.ToolTipHorizontalOffset = e.GetPosition((IInputElement)sender).X + 16;
		ViewModel.ToolTipVerticalOffset = e.GetPosition((IInputElement)sender).Y + 16;
	}

	private void SetResourceChart()
	{
		ChartArea.Plot.Clear();

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
		ChartArea.Plot.YAxis2.IsVisible = true;
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
		SetYBounds();
		ChartArea.Refresh();
	}
	private void SetResourceDiffChart()
	{
		ChartArea.Plot.Clear();

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
		ChartArea.Plot.YAxis2.IsVisible = true;
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
		SetYBounds();
		ChartArea.Refresh();
	}
	private void SetMaterialDiffChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Visible;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ChartArea.Plot.YAxis2.IsVisible = false;

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
		InstantRepairSignalPlot.FillAboveAndBelow(InstantRepairMatColor, Color.Transparent, Color.Transparent, InstantRepairMatColor, 1);
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
		DevelopmentMaterialSignalPlot.FillAboveAndBelow(DevelopmentMaterialColor, Color.Transparent, Color.Transparent, DevelopmentMaterialColor, 1);
		DevelopmentMaterialSignalPlot.MarkerSize = 0;

		InstantConstructionSignalPlot = ChartArea.Plot.AddSignalXY(date_list.ToArray(), instant_contruction_list.ToArray());
		InstantConstructionSignalPlot.StepDisplay = true;
		InstantConstructionSignalPlot.Label = "Instant Construction";
		InstantConstructionSignalPlot.FillAboveAndBelow(InstantConstructionColor, Color.Transparent, Color.Transparent, InstantConstructionColor, 1);
		InstantConstructionSignalPlot.MarkerSize = 0;
		if (InstantConstructionSignalPlot.Xs.Length > 0)
		{
			ChartArea.Plot.SetAxisLimits(yMin: Math.Floor(ChartArea.Plot.GetDataLimits().YMin / 20) * 20, yMax: Math.Ceiling(ChartArea.Plot.GetDataLimits().YMax / 20) * 20);
		}
		SetYBounds();
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
		double unitBase = Math.Round((max - min) / powered);
		double unit = powered * unitBase switch
		{
			< 2 => 0.2,
			< 5 => 0.5,
			< 7 => 1,
			_ => 2
		};

		double axisYmin = Math.Floor(min / unit) * unit;
		double axisYmax = Math.Ceiling(max / unit) * unit;

		bool isYaxisPlotsVisible = ChartArea.Plot.GetPlottables().Any(p => p.YAxisIndex == 0 && p.IsVisible);
		ChartArea.Plot.YAxis.IsVisible = isYaxisPlotsVisible;

		if (Math.Abs(axisYmin - axisYmax) < 1)
		{
			ChartArea.Plot.AxisAuto();
			return;
		}

		if (isYaxisPlotsVisible)
		{
			ChartArea.Plot.SetAxisLimits(yMin: axisYmin, yMax: axisYmax, yAxisIndex: 0);
		}
		else
		{
			ChartArea.Plot.AxisAuto();
		}

		if (ChartArea.Plot.YAxis2.IsVisible && isYaxisPlotsVisible)
		{
			ChartArea.Plot.SetAxisLimits(yMin: axisYmin / 100, yMax: axisYmax / 100, yAxisIndex: 1);
		}
		else
		{
			ChartArea.Plot.AxisAuto();
		}
	}

	private void SetMaterialChart()
	{
		ChartArea.Plot.Clear();
		ResourcesPanel.Visibility = Visibility.Collapsed;
		MaterialPanel.Visibility = Visibility.Visible;
		ExperiencePanel.Visibility = Visibility.Collapsed;
		ChartArea.Plot.YAxis2.IsVisible = false;

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
		InstantRepairPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), instant_repair_list.ToArray(), InstantRepairMatColor, label: "Instant Repair");
		DevelopmentMaterialPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), development_material_list.ToArray(), DevelopmentMaterialColor, label: "Development Material");
		ModdingMaterialPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), modding_material_list.ToArray(), ModdingMaterialColor, label: "Modding Material");
		InstantConstructionPlot = ChartArea.Plot.AddScatterLines(date_list.ToArray(), instant_contruction_list.ToArray(), InstantConstructionColor, label: "Instant Construction");
		if (InstantConstructionPlot.Xs.Length > 0)
		{
			ChartArea.Plot.SetAxisLimits(yMin: Math.Floor(ChartArea.Plot.GetDataLimits().YMin / 200) * 200, yMax: Math.Ceiling(ChartArea.Plot.GetDataLimits().YMax / 200) * 200);
		}

		SetYBounds();
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
		if(ExperiencePlot.Xs.Length > 0)
		{
			double min = ExperiencePlot.Ys.Min();
			double max = ExperiencePlot.Ys.Min();

			if (Math.Abs(min - max) < 1)
			{
				ChartArea.Plot.AxisAuto();
			}
			else
			{
				ChartArea.Plot.SetAxisLimits(yMin: Math.Floor(min / 100000.0) * 100000, yMax: Math.Ceiling(max / 100000.0) * 100000, yAxisIndex: 0);
			}
		}
		SetYBounds();
		ChartArea.Refresh();
	}

	private void SetYBounds()
	{
		double chartMin = ChartArea.Plot.YAxis2.IsVisible ? (ChartArea.Plot.GetDataLimits(yAxisIndex: 1).YMin * 100) : ChartArea.Plot.GetDataLimits(yAxisIndex: 0).YMin;
		double chartMax = ChartArea.Plot.YAxis2.IsVisible ? (ChartArea.Plot.GetDataLimits(yAxisIndex: 1).YMax * 100) : ChartArea.Plot.GetDataLimits(yAxisIndex: 0).YMax;
		SetYBounds(
			!ChartArea.Plot.GetPlottables().Any(p => p.IsVisible) || SelectedChartType == ChartType.ExperienceDiff ? 0 : Math.Min(ChartArea.Plot.GetDataLimits(yAxisIndex: 0).YMin, chartMin),
			!ChartArea.Plot.GetPlottables().Any(p => p.IsVisible) ? 0 : Math.Max(ChartArea.Plot.GetDataLimits(yAxisIndex: 0).YMax, chartMax)
		);
	}

	private void AxisXIntervals(ChartSpan span)
	{
		Configuration.ConfigurationData c = Configuration.Config;
		var axis = ChartArea.Plot.XAxis;
		axis.DateTimeFormat(true);
		switch (span)
		{
			case ChartSpan.Day:
				axis.TickLabelFormat("MM/dd HH:mm", true);
				axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Hour);
				break;

			case ChartSpan.Week:
			case ChartSpan.WeekFirst:
				axis.TickLabelFormat("MM/dd HH:mm", true);
				axis.ManualTickSpacing(12, ScottPlot.Ticks.DateTimeUnit.Hour);
				break;

			case ChartSpan.Month:
			case ChartSpan.MonthFirst:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Day);
				break;

			case ChartSpan.Season:
			case ChartSpan.SeasonFirst:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.ManualTickSpacing(7, ScottPlot.Ticks.DateTimeUnit.Day);
				break;

			case ChartSpan.Year:
			case ChartSpan.YearFirst:
			case ChartSpan.All:
				axis.TickLabelFormat("yyyy/MM/dd", true);
				axis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Month);
				break;
			default:
				double diffofdate = Math.Floor((ViewModel.DateEnd - ViewModel.DateBegin).TotalDays);
				if (diffofdate <= 1)
				{
					axis.TickLabelFormat("MM/dd HH:mm", true);
					axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Hour);
				}
				else if (diffofdate <= 7 && diffofdate > 1)
				{
					axis.TickLabelFormat("MM/dd HH:mm", true);
					axis.ManualTickSpacing(12, ScottPlot.Ticks.DateTimeUnit.Hour);
				}
				else if (diffofdate <= 31 && diffofdate > 7)
				{
					axis.TickLabelFormat("yyyy/MM/dd", true);
					axis.ManualTickSpacing(2, ScottPlot.Ticks.DateTimeUnit.Day);
				}
				else if (diffofdate <= 93 && diffofdate > 31)
				{
					axis.TickLabelFormat("yyyy/MM/dd", true);
					axis.ManualTickSpacing(7, ScottPlot.Ticks.DateTimeUnit.Day);
				}
				else
				{
					axis.TickLabelFormat("yyyy/MM/dd", true);
					axis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Month);
				}
				break;
		}
	}
	private void CheckChartVisiblity()
	{
		ChartArea.Visibility = ChartArea.Plot.GetPlottables().Any(p => p.IsVisible) ? Visibility.Visible : Visibility.Collapsed;
	}
	private void ChartSpan_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartSpanMenu, ((MenuItem)sender).Tag);
		SetDateRange();
		UpdateChart();
	}

	private void UpdateChart()
	{
		if (ViewModel.DateEnd <= ViewModel.DateBegin)
		{
			ChartArea.Visibility = Visibility.Collapsed;
			return;
		}
		ChartArea.Visibility = Visibility.Visible;

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
		SetYBounds();
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
		foreach (var r in RecordManager.Instance.Resource.Record)
		{
			if (r.Date > ViewModel.DateBegin && r.Date < ViewModel.DateEnd)
				yield return r;
		}

		if (DateTime.Now >= ViewModel.DateEnd) yield break;

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
		CheckChartVisiblity();
	}

	private void ResourceMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "0");
		UpdateChart();
		CheckChartVisiblity();
	}

	private void MaterialDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "3");
		UpdateChart();
		CheckChartVisiblity();
	}

	private void ExperienceMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "4");
		UpdateChart();
		CheckChartVisiblity();
	}

	private void ExperienceDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "5");
		UpdateChart();
		CheckChartVisiblity();
	}

	private void ResourceDiffMenu_Click(object sender, RoutedEventArgs e)
	{
		SwitchMenuStrip(ChartTypeMenu, "1");
		UpdateChart();
		CheckChartVisiblity();
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

	private void SetDateRange()
	{
		DateTime now = DateTime.Now;

		switch (SelectedChartSpan)
		{
			case ChartSpan.Day:
				ViewModel.DateBegin = DateTime.Now - TimeSpan.FromDays(1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.Week:
				ViewModel.DateBegin = DateTime.Now - TimeSpan.FromDays(7);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.Month:
				ViewModel.DateBegin = DateTime.Now.AddMonths(-1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.Season:
				ViewModel.DateBegin = DateTime.Now.AddMonths(-3);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.Year:
				ViewModel.DateBegin = DateTime.Now.AddYears(-1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.WeekFirst:
				ViewModel.DateBegin = DateTime.Now.AddDays(now.DayOfWeek == DayOfWeek.Sunday ? -6 : (1 - (int)now.DayOfWeek));
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.MonthFirst:
				ViewModel.DateBegin = new DateTime(now.Year, now.Month, 1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;

			case ChartSpan.SeasonFirst:
			{
				int m = now.Month / 3 * 3;
				if (m == 0)
					m = 12;
				ViewModel.DateBegin = new DateTime(now.Year - (now.Month < 3 ? 1 : 0), m, 1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
			}
			break;

			case ChartSpan.YearFirst:
				ViewModel.DateBegin = new DateTime(now.Year, 1, 1);
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;
			case ChartSpan.All:
				_record = RecordManager.Instance.Resource;
				ViewModel.DateBegin = _record.Record.First().Date;
				ViewModel.DateEnd = DateTime.Now.AddDays(1).Date;
				break;
		}
	}
}
