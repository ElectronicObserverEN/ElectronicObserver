using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public class GaugeConfiguration
{
	[JsonIgnore]
	public string Name => $"{TpGauge.GetEventName()} E{TpGauge.GetGaugeMapId()}-{TpGauge.GetGaugeIndex()}";

	public required TpGauge TpGauge { get; set; }

	public bool ShouldDisplay { get; set; } = true;
}
