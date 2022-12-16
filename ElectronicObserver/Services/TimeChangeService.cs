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
	public DayOfWeek TodayJST { get; private set; }

	public event Action TodayChanged = delegate { };

	public TimeChangeService()
	{
		PropertyChanged += TimeChangeService_PropertyChanged;

		SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;
	}

	private void SystemEvents_UpdateTimerTick()
	{
		TodayJST = DateTimeHelper.GetJapanStandardTimeNow().DayOfWeek;
	}

	private void TimeChangeService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(TodayJST)) TodayChanged();
	}
}
