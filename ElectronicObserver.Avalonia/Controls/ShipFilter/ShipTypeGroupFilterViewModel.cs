using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Controls.ShipFilter;

public partial class ShipTypeGroupFilterViewModel(ShipTypeGroup value) : ObservableObject
{
	public ShipTypeGroup Value { get; } = value;
	public string Display => Value.Display();
	[ObservableProperty] public partial bool IsChecked { get; set; }
}
