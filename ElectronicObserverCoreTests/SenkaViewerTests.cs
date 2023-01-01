using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Window.Tools.SenkaViewer;
using Xunit;

namespace ElectronicObserverCoreTests;

public class SenkaViewerTests
{
	[Fact(DisplayName = "Starts with end of month")]
	public void SenkaViewerTest1()
	{
		// midnight JST
		DateTime start = new(2022, 12, 31, 15, 0, 0, DateTimeKind.Utc);
		DateTime end = new(2023, 1, 1, 15, 0, 0, DateTimeKind.Utc);

		List<SenkaRecord> senkaRecords = SenkaViewerViewModel.GenerateSenkaRecords(start, end);

		Assert.Equal(3, senkaRecords.Count);

		// 2023/01/02 02:00
		DateTime time1 = new(2023, 1, 1, 17, 0, 0, DateTimeKind.Utc);
		// 2023/01/01 14:00
		DateTime time2 = new(2023, 1, 1, 5, 0, 0, DateTimeKind.Utc);
		// 2023/01/01 02:00
		DateTime time3 = new(2022, 12, 31, 17, 0, 0, DateTimeKind.Utc);
		// 2022/12/31 22:00
		DateTime time4 = new(2022, 12, 31, 13, 0, 0, DateTimeKind.Utc);

		// 2023/01/01 14:00 - 2023/01/02 02:00
		Assert.Equal(time2, senkaRecords[0].Start);
		Assert.Equal(time1, senkaRecords[0].End);

		// 2023/01/01 02:00 - 2023/01/01 14:00
		Assert.Equal(time3, senkaRecords[1].Start);
		Assert.Equal(time2, senkaRecords[1].End);

		// 2022/12/31 22:00 - 2023/01/01 02:00
		Assert.Equal(time4, senkaRecords[2].Start);
		Assert.Equal(time3, senkaRecords[2].End);
	}

	[Fact(DisplayName = "Normal case")]
	public void SenkaViewerTest2()
	{
		// midnight JST
		DateTime start = new(2022, 12, 30, 15, 0, 0, DateTimeKind.Utc);
		DateTime end = new(2023, 1, 1, 15, 0, 0, DateTimeKind.Utc);

		List<SenkaRecord> senkaRecords = SenkaViewerViewModel.GenerateSenkaRecords(start, end);

		Assert.Equal(6, senkaRecords.Count);

		// 2023/01/02 02:00
		DateTime time1 = new(2023, 1, 1, 17, 0, 0, DateTimeKind.Utc);
		// 2023/01/01 14:00
		DateTime time2 = new(2023, 1, 1, 5, 0, 0, DateTimeKind.Utc);
		// 2023/01/01 02:00
		DateTime time3 = new(2022, 12, 31, 17, 0, 0, DateTimeKind.Utc);
		// 2022/12/31 22:00
		DateTime time4 = new(2022, 12, 31, 13, 0, 0, DateTimeKind.Utc);
		// 2022/12/31 14:00
		DateTime time5 = new(2022, 12, 31, 5, 0, 0, DateTimeKind.Utc);
		// 2022/12/31 02:00
		DateTime time6 = new(2022, 12, 30, 17, 0, 0, DateTimeKind.Utc);
		// 2022/12/30 14:00
		DateTime time7 = new(2022, 12, 30, 5, 0, 0, DateTimeKind.Utc);

		// 2023/01/01 14:00 - 2023/01/02 02:00
		Assert.Equal(time2, senkaRecords[0].Start);
		Assert.Equal(time1, senkaRecords[0].End);

		// 2023/01/01 02:00 - 2023/01/01 14:00
		Assert.Equal(time3, senkaRecords[1].Start);
		Assert.Equal(time2, senkaRecords[1].End);

		// 2022/12/31 22:00 - 2023/01/01 02:00
		Assert.Equal(time4, senkaRecords[2].Start);
		Assert.Equal(time3, senkaRecords[2].End);

		// 2022/12/31 14:00 - 2022/12/31 22:00
		Assert.Equal(time5, senkaRecords[3].Start);
		Assert.Equal(time4, senkaRecords[3].End);

		// 2022/12/31 02:00 - 2022/12/31 14:00
		Assert.Equal(time6, senkaRecords[4].Start);
		Assert.Equal(time5, senkaRecords[4].End);

		// 2022/12/30 14:00 - 2023/01/31 02:00
		Assert.Equal(time7, senkaRecords[5].Start);
		Assert.Equal(time6, senkaRecords[5].End);
	}
}
