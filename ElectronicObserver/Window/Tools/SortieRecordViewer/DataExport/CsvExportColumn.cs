using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class CsvExportColumn<T, TValue>(CsvExportColumnModel column)
{
	public CsvExportColumnModel Column { get; } = column;

	public CsvExportColumn<T, TValue> Name(Func<string> name)
	{
		Column.Name = name;
		return this;
	}
}
