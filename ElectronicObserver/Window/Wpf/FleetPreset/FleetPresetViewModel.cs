using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.FleetPreset;

public class FleetPresetItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
}

public class FleetPresetItemViewModel : ObservableObject
{
	public FormFleetPresetTranslationViewModel FormFleetPreset { get; }

	public FleetPresetItemControlViewModel Name { get; }
	public FleetConditionViewModel Condition { get; } = new();
	public List<FleetPresetItemControlViewModel> Ships { get; }

	public int MaxWidth { get; set; }

	public FleetPresetItemViewModel()
	{
		FormFleetPreset = Ioc.Default.GetService<FormFleetPresetTranslationViewModel>()!;

		FleetPresetItemControlViewModel CreateDefaultLabel()
		{
			return new()
			{
				Text = "",
				// Anchor = AnchorStyles.Left,
				// ForeColor = parent.ForeColor,
				// Tag = null,
				// TextAlign = ContentAlignment.MiddleLeft,
				// Padding = new Padding(0, 1, 0, 1),
				// Margin = new Padding(2, 1, 2, 1),
				// AutoEllipsis = false,
				// AutoSize = true,
				// Visible = true,
				// 
				// ImageList = ResourceManager.Instance.Icons,
				// ImageAlign = ContentAlignment.MiddleCenter,
				// ImageIndex = -1
			};
		}

		Name = CreateDefaultLabel();
		Condition.ImageAlign = ContentAlignment.MiddleRight;

		// TODO: 本体側がもし 7 隻編成に対応したら変更してください
		Ships = new();
		for (int i = 0; i < 7; i++)
		{
			Ships.Add(CreateDefaultLabel());
		}

		ConfigurationChanged();
	}

	public void Update(int presetID)
	{
		var preset = KCDatabase.Instance.FleetPreset[presetID];

		if (preset == null)
		{
			Name.Text = "----";
			Name.ToolTip = null;

			foreach (var ship in Ships)
			{
				ship.Text = string.Empty;
				ship.ToolTip = null;
			}
			return;
		}


		Name.Text = preset.Name;

		int lowestCondition = preset.MembersInstance.Where(s => s != null).Select(s => s.Condition).DefaultIfEmpty(49).Min();
		// FormFleet.SetConditionDesign(Name, lowestCondition);
		Condition.SetDesign(lowestCondition);

		Name.ToolTip = $"{FormFleetPreset.LowestCondition}: {lowestCondition}";

		for (int i = 0; i < Ships.Count; i++)
		{
			var ship = i >= preset.Members.Count ? null : preset.MembersInstance.ElementAt(i);
			var label = Ships[i];

			Ships[i].Text = ship?.Name ?? "-";

			if (ship == null)
			{
				Ships[i].ToolTip = null;
			}
			else
			{
				var sb = new StringBuilder();
				sb.AppendLine($"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}");
				sb.AppendLine($"HP: {ship.HPCurrent} / {ship.HPMax} ({ship.HPRate:p1}) [{Constants.GetDamageState(ship.HPRate)}]");
				sb.AppendLine($"cond: {ship.Condition}");
				sb.AppendLine();

				var slot = ship.AllSlotInstance;
				for (int e = 0; e < slot.Count; e++)
				{
					if (slot[e] == null)
						continue;

					if (e < ship.MasterShip.Aircraft.Count)
					{
						sb.AppendLine($"[{ship.Aircraft[e]}/{ship.MasterShip.Aircraft[e]}] {slot[e].NameWithLevel}");
					}
					else
					{
						sb.AppendLine(slot[e].NameWithLevel);
					}
				}

				Ships[i].ToolTip = sb.ToString();
			}
		}
	}

	public void ConfigurationChanged()
	{
		var config = Utility.Configuration.Config;

		MaxWidth = config.FormFleet.FixShipNameWidth switch
		{
			true => config.FormFleet.FixedShipNameWidth,
			_ => int.MaxValue
		};
	}
}

public class FleetPresetViewModel : AnchorableViewModel
{
	public FormFleetPresetTranslationViewModel FormFleetPreset { get; }
	public ObservableCollection<FleetPresetItemViewModel> TableControls { get; }

	public FleetPresetViewModel() : base("Presets", "FleetPreset",
		ImageSourceIcons.GetIcon(IconContent.FormFleetPreset))
	{
		FormFleetPreset = Ioc.Default.GetService<FormFleetPresetTranslationViewModel>()!;

		Title = FormFleetPreset.Title;
		FormFleetPreset.PropertyChanged += (_, _) => Title = FormFleetPreset.Title;

		TableControls = new();
		ConfigurationChanged();

		KCDatabase.Instance.FleetPreset.PresetChanged += Updated;

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		var config = Utility.Configuration.Config;
		// Font = Utility.Configuration.Config.UI.MainFont;
		bool fixShipNameWidth = config.FormFleet.FixShipNameWidth;

		// TablePresets.SuspendLayout();
		foreach (var item in TableControls)
			item.ConfigurationChanged();

		/*
		for (int i = 1; i < TablePresets.ColumnCount; i++)
			ControlHelper.SetTableColumnStyle(TablePresets, i, fixShipNameWidth ?
				new ColumnStyle(SizeType.Absolute, config.FormFleet.FixedShipNameWidth + 4) :
				new ColumnStyle(SizeType.AutoSize));
		ControlHelper.SetTableRowStyles(TablePresets, ControlHelper.GetDefaultRowStyle());
		*/
		// TablePresets.ResumeLayout();

	}

	private void Updated()
	{
		var presets = KCDatabase.Instance.FleetPreset;
		if (presets == null || presets.MaximumCount <= 0)
			return;

		// TablePresets.Enabled = false;
		// TablePresets.SuspendLayout();

		if (TableControls.Count < presets.MaximumCount)
		{
			for (int i = TableControls.Count; i < presets.MaximumCount; i++)
			{
				TableControls.Add(new());
			}
			// ControlHelper.SetTableRowStyles(TablePresets, ControlHelper.GetDefaultRowStyle());
		}

		for (int i = 0; i < TableControls.Count; i++)
		{
			TableControls[i].Update(i + 1);
		}

		// TablePresets.ResumeLayout();
		// TablePresets.Enabled = true;
	}

}
