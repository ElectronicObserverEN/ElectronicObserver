using System;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public class SenkaRecord
{
	private SenkaViewerTranslationViewModel SenkaViewer =>
		Ioc.Default.GetRequiredService<SenkaViewerTranslationViewModel>();

	public DateTime Start { get; set; }
	public DateTime End { get; set; }

	public double TotalSenkaGains => EstimatedHqExpSenkaGains + ExtraOperationSenkaGains + QuestSenkaGains;
	public double HqExpSenkaGains { get; set; }
	public double EstimatedHqExpSenkaGains { get; set; }
	public double ExtraOperationSenkaGains { get; set; }
	public double QuestSenkaGains { get; set; }

	// for example senka gained while not using EO
	private bool ContainsUnknownSenkaGains => HqExpSenkaGains.CompareTo(EstimatedHqExpSenkaGains) != 0;

	public double DisplayedHqExpSenkaGains => ContainsUnknownSenkaGains switch
	{
		true => EstimatedHqExpSenkaGains,
		_ => HqExpSenkaGains,
	};

	public string? HqExpSenkaGainsToolTip => ContainsUnknownSenkaGains switch
	{
		true => 
			$"{SenkaViewer.RecordedSenka}: {HqExpSenkaGains:+0.##;-0.##;0}\n" +
			$"{SenkaViewer.EstimatedExtraGains}: {EstimatedHqExpSenkaGains - HqExpSenkaGains:+0.##;-0.##;0}",

		_ => null,
	};
}
