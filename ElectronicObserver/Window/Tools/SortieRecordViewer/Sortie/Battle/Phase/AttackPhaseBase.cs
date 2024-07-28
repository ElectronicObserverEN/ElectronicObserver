using System.Collections.Generic;
using ElectronicObserver.Window.Dialog.BattleDetail;
using System.Linq;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public abstract class AttackPhaseBase : PhaseBase
{
	public abstract IEnumerable<AttackViewModelBase> AttackDisplays { get; }

	private IEnumerable<string> SearchBattleDetails(int index) => AttackDisplays
		.Where(a => a.AttackerIndex?.ToFlatIndex() == index || a.DefenderIndex.ToFlatIndex() == index)
		.Select(a => a.ToString());

	public string? GetBattleDetail(int index = -1)
	{
		IEnumerable<string> list = index switch
		{
			-1 => AttackDisplays.Select(d => d.ToString()),
			_ => SearchBattleDetails(index),
		};

		if (list.Any())
		{
			return string.Join("\r\n\r\n", list) + "\r\n";
		}

		return null;
	}
}
