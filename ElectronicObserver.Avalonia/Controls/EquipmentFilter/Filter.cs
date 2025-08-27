using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Controls.EquipmentFilter;

public partial class Filter(EquipmentTypeGroup value) : ObservableObject
{
	public EquipmentTypeGroup Value { get; } = value;
	public string Display => Value.Display();
	[ObservableProperty] public partial bool IsChecked { get; set; }
}
