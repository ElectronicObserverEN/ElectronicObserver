using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Services;

/// <summary>
/// Service that triggers events on time change (day changed, ...)
/// </summary>
public class TimeChangeService : ObservableObject
{
	public DayOfWeek CurrentDayOfWeekJST { get; private set; }
	private int CurrentHour { get; set; }

	public event Action DayChanged = delegate { };
	public event Action HourChanged = delegate { };

	public TimeChangeService()
	{
		PropertyChanged += TimeChangeService_PropertyChanged;

		SystemEvents.UpdateTimerTick += OnTick;
	}

	private void OnTick()
	{
		DateTime time = DateTimeHelper.GetJapanStandardTimeNow();
		CurrentDayOfWeekJST = time.DayOfWeek;
		CurrentHour = time.Hour;
	}

	private void TimeChangeService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(CurrentDayOfWeekJST))
		{
			DayChanged();
		}

		if (e.PropertyName == nameof(CurrentHour))
		{
			HourChanged();
		}
	}
}
