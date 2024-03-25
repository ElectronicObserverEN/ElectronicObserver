using System;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropRecordRow(int indexOrCount) : DropRecordRowBase(indexOrCount)
{
	public required string Name { get; init; }
	public required DateTime Date { get; init; }
	public required string MapDescription { get; init; }
	public BattleRank? Rank { get; init; }
	public ShipId ShipId { get; set; }

	public string? DateDisplay =>  DateTimeHelper.TimeToCSVString(Date);
}
