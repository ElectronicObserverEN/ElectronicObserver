using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Services;

/// <summary>
/// Service that triggers events on time change (day changed, ...)
/// </summary>
public class TimeChangeService : ObservableObject
{
	public DayOfWeek CurrentDayOfWeekJST { get; private set; }

	public event Action DayChanged = delegate { };

	public TimeChangeService()
	{
		PropertyChanged += TimeChangeService_PropertyChanged;

		SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;
	}

	private void SystemEvents_UpdateTimerTick()
	{
		CurrentDayOfWeekJST = DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek;
	}

	private void TimeChangeService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(CurrentDayOfWeekJST)) DayChanged();
	}
}
