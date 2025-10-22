using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class MapFirstClearTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<MapInfoModel> AllMaps { get; }

	public MapFirstClearTaskModel Model { get; set; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string? Progress => (Model.Progress >= Model.Count) switch
	{
		true => null,
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition()
	{
		StringBuilder sb = new();

		sb.Append($"{Model.Map.AreaId}-{Model.Map.InfoId}");
		sb.Append(QuestTracking.MapFirstClear);

		sb.Append(Model.Count);

		return sb.ToString();
	}

	public MapFirstClearTaskViewModel(MapFirstClearTaskModel model)
	{
		Model = model;

		AllMaps = KCDatabase.Instance.MapInfo.Values
			.Select(m => new MapInfoModel(m.MapAreaID, m.MapInfoID));

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(int compassMapAreaId, int compassMapInfoId)
	{
		if (compassMapAreaId == Model.Map.AreaId &&
			compassMapInfoId == Model.Map.InfoId)
		{
			Model.Progress++;
		}
	}
}
