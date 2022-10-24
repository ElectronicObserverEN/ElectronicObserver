using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Settings.Notification;

public class FlagEnumWrapper : ObservableObject
{
	public Enum Value { get; }
	public bool IsChecked { get; set; }

	public FlagEnumWrapper(Enum value)
	{
		Value = value;
	}
}
