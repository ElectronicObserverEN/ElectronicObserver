using System;
using ElectronicObserver.Core;
using ElectronicObserver.Utility.Mathematics;
using Xunit;

namespace ElectronicObserverCoreTests;

public class DateTimeHelperTests
{
	[Fact]
	public void TestingOverrideWorks()
	{
		DateTime now = DateTime.Now;

		Assert.Equal(now, DateTimeHelper.GetJapanStandardTimeNow(now));
		Assert.NotEqual(now, DateTimeHelper.GetJapanStandardTimeNow());
	}

	[Fact]
	public void IsCrossedDailyQuestReset()
	{
		DateTime lastQuestUpdateTime = new(2021, 10, 20, 11, 0, 0);

		DateTime sameDay = new(2021, 10, 20, 12, 0, 0);
		DateTime nextDay = new(2021, 10, 21, 5, 0, 0);

		Assert.False(DateTimeHelper.IsCrossedDailyQuestReset(lastQuestUpdateTime, sameDay));
		Assert.True(DateTimeHelper.IsCrossedDailyQuestReset(lastQuestUpdateTime, nextDay));
	}

	[Fact]
	public void IsCrossedWeeklyQuestReset()
	{
		DateTime lastQuestUpdateTime = new(2021, 10, 20, 11, 0, 0);

		DateTime sameWeek = new(2021, 10, 20, 12, 0, 0);
		DateTime nextWeek = new(2021, 10, 25, 5, 0, 0);

		Assert.False(DateTimeHelper.IsCrossedWeeklyQuestReset(lastQuestUpdateTime, sameWeek));
		Assert.True(DateTimeHelper.IsCrossedWeeklyQuestReset(lastQuestUpdateTime, nextWeek));
	}

	[Fact]
	public void IsCrossedMonthlyQuestReset()
	{
		DateTime lastQuestUpdateTime = new(2021, 10, 20, 11, 0, 0);

		DateTime sameMonth = new(2021, 10, 20, 12, 0, 0);
		DateTime nextMonthBeforeReset = new(2021, 11, 1, 4, 0, 0);
		DateTime nextMonth = new(2021, 11, 1, 5, 0, 0);

		Assert.False(DateTimeHelper.IsCrossedMonthlyQuestReset(lastQuestUpdateTime, sameMonth));
		Assert.False(DateTimeHelper.IsCrossedMonthlyQuestReset(lastQuestUpdateTime, nextMonthBeforeReset));
		Assert.True(DateTimeHelper.IsCrossedMonthlyQuestReset(lastQuestUpdateTime, nextMonth));
	}

	[Fact]
	public void IsCrossedQuarterlyQuestReset()
	{
		DateTime lastQuestUpdateTime = new(2021, 10, 20, 11, 0, 0);

		DateTime sameQuarter = new(2021, 10, 20, 12, 0, 0);
		DateTime nextQuarterBeforeReset = new(2021, 12, 1, 4, 0, 0);
		DateTime nextQuarter = new(2021, 12, 1, 5, 0, 0);

		Assert.False(DateTimeHelper.IsCrossedQuarterlyQuestReset(lastQuestUpdateTime, sameQuarter));
		Assert.False(DateTimeHelper.IsCrossedQuarterlyQuestReset(lastQuestUpdateTime, nextQuarterBeforeReset));
		Assert.True(DateTimeHelper.IsCrossedQuarterlyQuestReset(lastQuestUpdateTime, nextQuarter));
	}

	[Fact]
	public void IsCrossedYearlyQuestReset()
	{
		DateTime lastQuestUpdateTime = new(2021, 10, 20, 11, 0, 0);

		int yearlyResetMonth = 12;

		DateTime sameYear = new(2021, 10, 20, 12, 0, 0);
		DateTime nextYearBeforeReset = new(2021, 12, 1, 4, 0, 0);
		DateTime nextYear = new(2021, 12, 1, 5, 0, 0);

		Assert.False(DateTimeHelper.IsCrossedYearlyQuestReset(lastQuestUpdateTime, yearlyResetMonth, sameYear));
		Assert.False(DateTimeHelper.IsCrossedYearlyQuestReset(lastQuestUpdateTime, yearlyResetMonth, nextYearBeforeReset));
		Assert.True(DateTimeHelper.IsCrossedYearlyQuestReset(lastQuestUpdateTime, yearlyResetMonth, nextYear));
	}
}
