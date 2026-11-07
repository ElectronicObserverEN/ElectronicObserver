using System;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Core;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropRecordRow : DropRecordRowBase
{
	public required int Index { get; init; }
	public required string Name { get; init; }
	public required DateTime Date { get; init; }
	public required string MapDescription { get; init; }
	public required BattleRank Rank { get; init; }
	public required ShipId ShipId { get; set; }

	public string? DateDisplay =>  DateTimeHelper.TimeToCSVString(Date);
}
