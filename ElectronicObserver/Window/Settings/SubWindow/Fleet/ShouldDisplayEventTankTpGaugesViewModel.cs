using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public partial class ShouldDisplayTankTpGaugeViewModel(TpGauge model) : ObservableObject
{
	public string Name => $"{TpGauge.GetEventName()} E{TpGauge.GetGaugeMapId()}-{TpGauge.GetGaugeIndex()}";

	public TpGauge TpGauge { get; } = model;

	[ObservableProperty]
	public partial bool ShouldDisplay { get; set; } = false;
}
