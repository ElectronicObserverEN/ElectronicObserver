using ElectronicObserver.Core;
using ElectronicObserver.Core.Types.Quests;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public sealed class QuestDataMock : IQuestData
{
	/// <inheritdoc />
	public int QuestID { get; init; }

	/// <inheritdoc />
	public int Category { get; }

	/// <inheritdoc />
	public int State { get; set; }

	/// <inheritdoc />
	public string Code { get; } = "Code";

	/// <inheritdoc />
	public string Name { get; } = "Name";

	/// <inheritdoc />
	public string Description { get; } = "Description";

	/// <inheritdoc />
	public QuestResetType GetProgressResetType() => QuestResetType.Unknown;
}
