using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Interfaces;

public interface IPhaseAirBattle
{
	bool IsStage1Available { get; }
	bool IsStage2Available { get; }

	int AircraftLostStage1Friend { get; }
	int AircraftTotalStage1Friend { get; }
	int AircraftLostStage1Enemy { get; }
	int AircraftTotalStage1Enemy { get; }

	int AircraftLostStage2Friend { get; }
	int AircraftTotalStage2Friend { get; }
	int AircraftLostStage2Enemy { get; }
	int AircraftTotalStage2Enemy { get; }

	string? TouchAircraftFriend { get; }
	string? TouchAircraftEnemy { get; }

	AirState AirState { get; }

	bool IsAACutinAvailable { get; }
	int AACutInIndex { get; }
	string? AACutInShipName { get; }
	int AACutInKind { get; }
}
