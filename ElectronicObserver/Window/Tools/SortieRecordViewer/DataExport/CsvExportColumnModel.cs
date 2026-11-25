using System;
using CsvHelper;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class CsvExportColumnModel
{
	public required Func<string> Name { get; set; }
	public required Action<CsvWriter, object?> Write { get; set; }
}
