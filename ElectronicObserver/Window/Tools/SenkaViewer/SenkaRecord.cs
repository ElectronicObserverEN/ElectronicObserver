using System;

namespace ElectronicObserver.Window.Tools.SenkaViewer;

public class SenkaRecord
{
	public DateTime Start { get; set; }
	public DateTime End { get; set; }

	public double TotalSenkaGains => HqExpSenkaGains + ExtraOperationSenkaGains + QuestSenkaGains;
	public double HqExpSenkaGains { get; set; }
	public double ExtraOperationSenkaGains { get; set; }
	public double QuestSenkaGains { get; set; }
}
