using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirAttackUnit : PhaseAirBattleBase
{
	private List<BattleBaseAirCorpsSquadron> Squadrons { get; }

	public string Display { get; }

	public PhaseBaseAirAttackUnit(ApiAirBaseAttack airBattleData, int waveIndex) 
		: base(airBattleData, waveIndex)
	{
		Squadrons = airBattleData.ApiSquadronPlane.Select(b => new BattleBaseAirCorpsSquadron
		{
			Equipment = KCDatabase.Instance.MasterEquipments[(int)b.ApiMstId],
			AircraftCount = b.ApiCount,
		}).ToList();

		StringBuilder sb = new();

		sb.AppendFormat(ConstantsRes.BattleDetail_AirAttackWave + "\r\n", WaveIndex + 1);

		sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
		sb.Append('　').Append(string.Join(", ", Squadrons
			.Where(sq => sq.Equipment is not null)
			.Select(sq => sq.ToString())));


		Display = sb.ToString();
	}

	public PhaseBaseAirAttackUnit(ApiAirBaseInjection airBattleData, int waveIndex)
		: base(airBattleData, waveIndex)
	{
		Squadrons = airBattleData.ApiAirBaseData.Select(b => new BattleBaseAirCorpsSquadron
		{
			Equipment = KCDatabase.Instance.MasterEquipments[(int)b.ApiMstId],
			AircraftCount = b.ApiCount,
		}).ToList();

		StringBuilder sb = new();

		sb.AppendFormat(ConstantsRes.BattleDetail_AirAttackWave + "\r\n", WaveIndex + 1);

		sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
		sb.Append('　').Append(string.Join(", ", Squadrons
			.Where(sq => sq.Equipment is not null)
			.Select(sq => sq.ToString())));


		Display = sb.ToString();
	}
}
