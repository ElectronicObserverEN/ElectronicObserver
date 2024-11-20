using System;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Services;

/// <summary>
/// Service that triggers events on time change (day changed, ...)
/// </summary>
public class TimeChangeService : ObservableObject, IDisposable
{
	public DayOfWeek CurrentDayOfWeekJST { get; private set; }
	public RankingCutoffKind RankingLeaderboardCutoffKind { get; private set; }

	public event Action DayChanged = delegate { };
	public event Action RankingLeaderboardUpdate = delegate { };

	private Timer Timer { get; }

	public TimeChangeService()
	{
		PropertyChanged += TimeChangeService_PropertyChanged;

		OnTimer(null);

		TimeSpan now = DateTime.Now.TimeOfDay;
		TimeSpan nextHour = new TimeSpan(now.Hours + 1, 0, 0);

		TimeSpan timeBeforeNextHour = nextHour - now;
		Timer = new(OnTimer, null, (int)timeBeforeNextHour.TotalMilliseconds, 60 * 60 * 1000);
	}

	private void OnTimer(object? sender)
	{
		DateTime time = DateTimeHelper.GetJapanStandardTimeNow();
		CurrentDayOfWeekJST = time.DayOfWeek;

		if ((time.Day == DateTime.DaysInMonth(time.Year, time.Month) && time.Hour > 22) || (time.Day is 1 && time.Hour < 3))
		{
			RankingLeaderboardCutoffKind = RankingCutoffKind.NewMonth;
		}
		else
		{
			RankingLeaderboardCutoffKind = time.TimeOfDay switch
			{
				{ Hours: >= 15 or < 3 } => RankingCutoffKind.MidDay,
				_ => RankingCutoffKind.NewDay,
			};
		}
	}

	private void TimeChangeService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(CurrentDayOfWeekJST))
		{
			DayChanged();
		}

		if (e.PropertyName == nameof(RankingLeaderboardCutoffKind))
		{
			RankingLeaderboardUpdate();
		}
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Timer.Dispose();
	}
}
