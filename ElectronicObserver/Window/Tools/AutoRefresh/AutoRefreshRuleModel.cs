using System.Collections.Generic;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public record AutoRefreshRuleModel(bool IsEnabled, MapInfoModel Map, List<CellModel> AllowedCells);