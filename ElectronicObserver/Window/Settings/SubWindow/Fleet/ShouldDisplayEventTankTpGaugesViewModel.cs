using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public class ShouldDisplayTankTpGaugeViewModel(TpGauge model)
{
	public string Name => $"{TpGauge.GetEventName()} E{TpGauge.GetGaugeMapId()}-{TpGauge.GetGaugeIndex()}";

	public TpGauge TpGauge { get; } = model;

	public bool ShouldDisplay { get; set; } = false;
}
