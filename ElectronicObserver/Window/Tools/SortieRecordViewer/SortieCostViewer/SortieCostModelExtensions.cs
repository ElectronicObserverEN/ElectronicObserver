﻿using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public static class SortieCostModelExtensions
{
	public static SortieCostModel Sum(this IEnumerable<SortieCostModel> models)
		=> models.Aggregate(new SortieCostModel(), (a, b) => a + b);
}
