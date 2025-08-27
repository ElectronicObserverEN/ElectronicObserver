using System;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

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
