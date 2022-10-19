using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.Log;

public class ConfigurationLogViewModel : ConfigurationViewModelBase
{
	public ConfigurationLogTranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigLog Config { get; }

	public List<EncodingOption> Encodings { get; }
	public EncodingOption SelectedEncoding { get; set; }

	public int LogLevel { get; set; }

	public bool SaveLogFlag { get; set; }

	public bool SaveErrorReport { get; set; }

	public bool ShowSpoiler { get; set; }

	public bool SaveBattleLog { get; set; }

	public bool SaveLogImmediately { get; set; }

	public ConfigurationLogViewModel(Configuration.ConfigurationData.ConfigLog config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationLogTranslationViewModel>();

		Encodings = new List<EncodingOption>
		{
			new(0),
			new(1),
			new(2),
			new(3),
			new(4),
		};
		SelectedEncoding = Encodings.First();

		Config = config;
		Load(config);
	}

	private void Load(Configuration.ConfigurationData.ConfigLog config)
	{
		LogLevel = config.LogLevel;
		SaveLogFlag = config.SaveLogFlag;
		SaveErrorReport = config.SaveErrorReport;
		ShowSpoiler = config.ShowSpoiler;
		SaveBattleLog = config.SaveBattleLog;
		SaveLogImmediately = config.SaveLogImmediately;
		SelectedEncoding = Encodings
			.FirstOrDefault(e => e.Value == config.FileEncodingID)
			?? Encodings.First();
	}

	public override void Save()
	{
		Config.LogLevel = LogLevel;
		Config.SaveLogFlag = SaveLogFlag;
		Config.SaveErrorReport = SaveErrorReport;
		Config.FileEncodingID = SelectedEncoding.Value;
		Config.ShowSpoiler = ShowSpoiler;
		Config.SaveBattleLog = SaveBattleLog;
		Config.SaveLogImmediately = SaveLogImmediately;
	}
}
