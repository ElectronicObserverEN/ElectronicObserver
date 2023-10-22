using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.FleetPreset;

public class FleetPresetViewModel : AnchorableViewModel
{
	public FormFleetPresetTranslationViewModel FormFleetPreset { get; }
	public ObservableCollection<FleetPresetItemViewModel> TableControls { get; }

	public FleetPresetViewModel() : base("Presets", "FleetPreset", IconContent.FormFleetPreset)
	{
		FormFleetPreset = Ioc.Default.GetService<FormFleetPresetTranslationViewModel>()!;

		Title = FormFleetPreset.Title;
		FormFleetPreset.PropertyChanged += (_, _) => Title = FormFleetPreset.Title;

		TableControls = new();
		ConfigurationChanged();

		KCDatabase.Instance.FleetPreset.PresetChanged += Updated;

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData config = Configuration.Config;

		foreach (FleetPresetItemViewModel item in TableControls)
		{
			item.ConfigurationChanged();
		}
	}

	private void Updated()
	{
		FleetPresetManager? presets = KCDatabase.Instance.FleetPreset;

		if (presets is not { MaximumCount: > 0 }) return;

		if (TableControls.Count < presets.MaximumCount)
		{
			for (int i = TableControls.Count; i < presets.MaximumCount; i++)
			{
				TableControls.Add(new());
			}
		}

		for (int i = 0; i < TableControls.Count; i++)
		{
			TableControls[i].Update(i + 1);
		}
	}
}
