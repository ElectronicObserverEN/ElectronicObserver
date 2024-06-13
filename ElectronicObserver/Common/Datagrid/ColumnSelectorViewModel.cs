using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common.Datagrid;

public partial class ColumnSelectorViewModel : WindowViewModelBase
{
	public ColumnSelectorTranslationViewModel ColumnSelector { get; } = new();
	public required List<ColumnViewModel> Columns { get; init; }
	[ObservableProperty] private int? _frozenColumns;
}
