using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ElectronicObserver.Resource.Record;

public class EnemyFleetElementComparer : IEqualityComparer<EnemyFleetRecord.EnemyFleetElement>
{
	public bool Equals(EnemyFleetRecord.EnemyFleetElement? x, EnemyFleetRecord.EnemyFleetElement? y)
	{
		if (x is null && y is null) return true;
		if (x is null || y is null) return false;

		foreach ((int? firstShip, int? secondShip) in x.FleetMember.Zip(y.FleetMember, (sa, sb) => (sa, sb)))
		{
			if (firstShip != secondShip) return false;
		}

		return true;
	}

	public int GetHashCode([DisallowNull] EnemyFleetRecord.EnemyFleetElement obj)
	{
		return obj.FleetMember.Aggregate((a, b) => a ^ b); 
	}
}
