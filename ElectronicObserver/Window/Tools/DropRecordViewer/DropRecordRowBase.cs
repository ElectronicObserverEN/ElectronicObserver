namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public abstract class DropRecordRowBase(int indexOrCount)
{
	public int Index { get; init; } = indexOrCount;
	public int Count { get; init; } = indexOrCount;
}
