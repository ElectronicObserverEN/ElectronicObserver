using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

public class PhaseBaseAirAttackUnit : PhaseAirBattleBase
{
	public List<BattleBaseAirCorpsSquadron> Squadrons { get; }

	public int AirBaseId { get; }
	public string Display { get; }

	public PhaseBaseAirAttackUnit(IKCDatabase kcDatabase, ApiAirBaseAttack airBattleData, int waveIndex)
		: base(kcDatabase, airBattleData, waveIndex)
	{
		AirBaseId = airBattleData.ApiBaseId;
		Squadrons = airBattleData.ApiSquadronPlane.Select(b => new BattleBaseAirCorpsSquadron
		{
			Equipment = KcDatabase.MasterEquipments[(int)b.ApiMstId],
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

	public PhaseBaseAirAttackUnit(IKCDatabase kcDatabase, ApiAirBaseInjection airBattleData, int waveIndex)
		: base(kcDatabase, airBattleData, waveIndex)
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
