using Avalonia.Data.Converters;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.QuestTrackerManager2;

public static class Converters
{
	public static string ToDisplay(ShipTypes shipType) => shipType switch
	{
		ShipTypes.Escort => Core.Properties.ShipTypes.Escort,
		ShipTypes.Destroyer => Core.Properties.ShipTypes.Destroyer,
		ShipTypes.LightCruiser => Core.Properties.ShipTypes.LightCruiser,
		ShipTypes.TorpedoCruiser => Core.Properties.ShipTypes.TorpedoCruiser,
		ShipTypes.HeavyCruiser => Core.Properties.ShipTypes.HeavyCruiser,
		ShipTypes.AviationCruiser => Core.Properties.ShipTypes.AviationCruiser,
		ShipTypes.LightAircraftCarrier => Core.Properties.ShipTypes.LightAircraftCarrier,
		ShipTypes.Battlecruiser => Core.Properties.ShipTypes.Battlecruiser,
		ShipTypes.Battleship => Core.Properties.ShipTypes.Battleship,
		ShipTypes.AviationBattleship => Core.Properties.ShipTypes.AviationBattleship,
		ShipTypes.AircraftCarrier => Core.Properties.ShipTypes.AircraftCarrier,
		ShipTypes.SuperDreadnoughts => Core.Properties.ShipTypes.SuperDreadnoughts,
		ShipTypes.Submarine => Core.Properties.ShipTypes.Submarine,
		ShipTypes.SubmarineAircraftCarrier => Core.Properties.ShipTypes.SubmarineAircraftCarrier,
		ShipTypes.Transport => Core.Properties.ShipTypes.Transport,
		ShipTypes.SeaplaneTender => Core.Properties.ShipTypes.SeaplaneTender,
		ShipTypes.AmphibiousAssaultShip => Core.Properties.ShipTypes.AmphibiousAssaultShip,
		ShipTypes.ArmoredAircraftCarrier => Core.Properties.ShipTypes.ArmoredAircraftCarrier,
		ShipTypes.RepairShip => Core.Properties.ShipTypes.RepairShip,
		ShipTypes.SubmarineTender => Core.Properties.ShipTypes.SubmarineTender,
		ShipTypes.TrainingCruiser => Core.Properties.ShipTypes.TrainingCruiser,
		ShipTypes.FleetOiler => Core.Properties.ShipTypes.FleetOiler,

		_ => "???",
	};

	public static FuncValueConverter<ShipTypes, string> ShipTypeDisplayConverter { get; } = new(ToDisplay);

	public static string ToDisplay(ComparisonOperator comparisonOperator) => comparisonOperator switch
	{
		ComparisonOperator.GreaterOrEqual => ">=",
		ComparisonOperator.GreaterThan => ">",
		ComparisonOperator.Equals => "=",
		ComparisonOperator.NotEquals => "!=",
		ComparisonOperator.LessThan => "<",
		ComparisonOperator.LessOrEqual => "<=",

		_ => "???",
	};

	public static FuncValueConverter<ComparisonOperator, string> ComparisonOperatorDisplayConverter { get; } =
		new(ToDisplay);

	public static string ToDisplay(LogicalOperator logicalOperator) => logicalOperator switch
	{
		LogicalOperator.And => QuestTrackerManagerResources.Operator_And,
		LogicalOperator.Or => QuestTrackerManagerResources.Operator_Or,

		_ => "???",
	};

	public static FuncValueConverter<LogicalOperator, string> LogicalOperatorDisplayConverter { get; } = new(ToDisplay);

	public static string ToDisplay(RemodelComparison remodelComparison) => remodelComparison switch
	{
		RemodelComparison.Any => QuestTrackerManagerResources.RemodelComparisonType_Any,
		RemodelComparison.AtLeast => QuestTrackerManagerResources.RemodelComparisonType_AtLeast,
		RemodelComparison.Exact => QuestTrackerManagerResources.RemodelComparisonType_Exact,

		_ => "???",
	};

	public static FuncValueConverter<RemodelComparison, string> RemodelComparisonDisplayConverter { get; } =
		new(ToDisplay);

	public static string ToDisplay(PositionViewModel? position) => position?.Position switch
	{
		1 => QuestTrackerManagerResources.Flagship,
		int i => $"{QuestTrackerManagerResources.Position}：{i}",

		_ => "???",
	};

	public static FuncValueConverter<PositionViewModel, string> PositionDisplayConverter { get; } = new(ToDisplay);

	private static string ToDisplay(ConditionType conditionType) => conditionType switch
	{
		ConditionType.Group => QuestTrackerManagerResources.ConditionType_Group,
		ConditionType.Not => QuestTrackerManagerResources.ConditionType_Not,
		ConditionType.Reference => QuestTrackerManagerResources.ConditionType_Reference,
		ConditionType.ShipId => QuestTrackerManagerResources.ConditionType_Ship,
		ConditionType.ShipType => QuestTrackerManagerResources.ConditionType_ShipType,
		ConditionType.ShipClass => QuestTrackerManagerResources.ConditionType_ShipClass,
		ConditionType.ShipPosition => QuestTrackerManagerResources.ConditionType_ShipPosition,
		ConditionType.ShipCount => QuestTrackerManagerResources.ConditionType_ShipCount,
		ConditionType.ShipLevel => QuestTrackerManagerResources.ConditionType_ShipLevel,
		ConditionType.ShipNationality => QuestTrackerManagerResources.ConditionType_ShipNationality,

		_ => "???",
	};

	public static FuncValueConverter<ConditionType, string> ConditionTypeDisplayConverter { get; } = new(ToDisplay);

	public static string ToDisplay(ShipNationality shipNationality) => shipNationality switch
	{
		ShipNationality.Japanese => Core.Properties.ShipNationality.Japanese,
		ShipNationality.German => Core.Properties.ShipNationality.German,
		ShipNationality.Italian => Core.Properties.ShipNationality.Italian,
		ShipNationality.American => Core.Properties.ShipNationality.American,
		ShipNationality.British => Core.Properties.ShipNationality.British,
		ShipNationality.French => Core.Properties.ShipNationality.French,
		ShipNationality.Russian => Core.Properties.ShipNationality.Russian,
		ShipNationality.Thai => Core.Properties.ShipNationality.Thai,
		ShipNationality.Norwegian => Core.Properties.ShipNationality.Norwegian,
		ShipNationality.Swedish => Core.Properties.ShipNationality.Swedish,
		ShipNationality.Dutch => Core.Properties.ShipNationality.Dutch,
		ShipNationality.Australian => Core.Properties.ShipNationality.Australian,

		_ => "???",
	};

	public static FuncValueConverter<ShipNationality, string> ShipNationalityDisplayConverter { get; } =
		new(ToDisplay);

	public static string ToDisplay(EquipmentCardType cardType) => cardType switch
	{
		EquipmentCardType.PrimaryArmament => Core.Properties.EquipmentCardType.PrimaryArmament,
		EquipmentCardType.SecondaryArmament => Core.Properties.EquipmentCardType.SecondaryArmament,
		EquipmentCardType.Torpedo => Core.Properties.EquipmentCardType.Torpedo,
		EquipmentCardType.MidgetSubmarine => Core.Properties.EquipmentCardType.MidgetSubmarine,
		EquipmentCardType.CarrierBasedAircraft => Core.Properties.EquipmentCardType.CarrierBasedAircraft,
		EquipmentCardType.AaGun => Core.Properties.EquipmentCardType.AaGun,
		EquipmentCardType.Reconnaissance => Core.Properties.EquipmentCardType.Reconnaissance,
		EquipmentCardType.Radar => Core.Properties.EquipmentCardType.Radar,
		EquipmentCardType.Upgrades => Core.Properties.EquipmentCardType.Upgrades,
		EquipmentCardType.Sonar => Core.Properties.EquipmentCardType.Sonar,
		EquipmentCardType.Daihatsu => Core.Properties.EquipmentCardType.Daihatsu,
		EquipmentCardType.Autogyro => Core.Properties.EquipmentCardType.Autogyro,
		EquipmentCardType.AntiSubmarinePatrol => Core.Properties.EquipmentCardType.AntiSubmarinePatrol,
		EquipmentCardType.ExtensionArmor => Core.Properties.EquipmentCardType.ExtensionArmor,
		EquipmentCardType.Searchlight => Core.Properties.EquipmentCardType.Searchlight,
		EquipmentCardType.Supply => Core.Properties.EquipmentCardType.Supply,
		EquipmentCardType.MachineTools => Core.Properties.EquipmentCardType.MachineTools,
		EquipmentCardType.Flare => Core.Properties.EquipmentCardType.Flare,
		EquipmentCardType.FleetCommand => Core.Properties.EquipmentCardType.FleetCommand,
		EquipmentCardType.MaintenanceTeam => Core.Properties.EquipmentCardType.MaintenanceTeam,
		EquipmentCardType.AaDirector => Core.Properties.EquipmentCardType.AaDirector,
		EquipmentCardType.ApShell => Core.Properties.EquipmentCardType.ApShell,
		EquipmentCardType.RocketArtillery => Core.Properties.EquipmentCardType.RocketArtillery,
		EquipmentCardType.PicketCrew => Core.Properties.EquipmentCardType.PicketCrew,
		EquipmentCardType.AaShell => Core.Properties.EquipmentCardType.AaShell,
		EquipmentCardType.AaRocket => Core.Properties.EquipmentCardType.AaRocket,
		EquipmentCardType.DamageControl => Core.Properties.EquipmentCardType.DamageControl,
		EquipmentCardType.EngineUpgrades => Core.Properties.EquipmentCardType.EngineUpgrades,
		EquipmentCardType.DepthCharge => Core.Properties.EquipmentCardType.DepthCharge,
		EquipmentCardType.FlyingBoat => Core.Properties.EquipmentCardType.FlyingBoat,
		EquipmentCardType.Ration => Core.Properties.EquipmentCardType.Ration,
		EquipmentCardType.Supply2 => Core.Properties.EquipmentCardType.Supply2,
		EquipmentCardType.MultiPurposeSeaplane => Core.Properties.EquipmentCardType.MultiPurposeSeaplane,
		EquipmentCardType.AmphibiousVehicle => Core.Properties.EquipmentCardType.AmphibiousVehicle,
		EquipmentCardType.LandAttacker => Core.Properties.EquipmentCardType.LandAttacker,
		EquipmentCardType.Interceptor => Core.Properties.EquipmentCardType.Interceptor,
		EquipmentCardType.JetFightingBomber => Core.Properties.EquipmentCardType.JetFightingBomber,
		EquipmentCardType.TransportMaterials => Core.Properties.EquipmentCardType.TransportMaterials,
		EquipmentCardType.SubmarineEquipment => Core.Properties.EquipmentCardType.SubmarineEquipment,
		EquipmentCardType.MultiPurposeSeaplane2 => Core.Properties.EquipmentCardType.MultiPurposeSeaplane2,
		EquipmentCardType.Helicopter => Core.Properties.EquipmentCardType.Helicopter,
		EquipmentCardType.DdTank => Core.Properties.EquipmentCardType.DdTank,
		EquipmentCardType.HeavyBomber => Core.Properties.EquipmentCardType.HeavyBomber,
		_ => Core.Properties.EquipmentCardType.Unknown
	};

	public static FuncValueConverter<EquipmentCardType, string> EquipmentCardTypeDisplayConverter { get; } =
		new(ToDisplay);

	public static string ToDisplay(EquipmentTypes category, IKCDatabase kcDatabase) =>
		kcDatabase.MasterEquipments.Values
			.FirstOrDefault(e => e.CategoryType == category)
			?.CategoryTypeInstance.NameEN ?? $"??? ({(int)category} - {category})";

	public static string ToDisplay(EquipmentIconType iconType) => iconType switch
	{
		EquipmentIconType.Nothing => Core.Properties.EquipmentIconType.Nothing,
		EquipmentIconType.MainGunSmall => Core.Properties.EquipmentIconType.MainGunSmall,
		EquipmentIconType.MainGunMedium => Core.Properties.EquipmentIconType.MainGunMedium,
		EquipmentIconType.MainGunLarge => Core.Properties.EquipmentIconType.MainGunLarge,
		EquipmentIconType.SecondaryGun => Core.Properties.EquipmentIconType.SecondaryGun,
		EquipmentIconType.Torpedo => Core.Properties.EquipmentIconType.Torpedo,
		EquipmentIconType.CarrierBasedFighter => Core.Properties.EquipmentIconType.CarrierBasedFighter,
		EquipmentIconType.CarrierBasedBomber => Core.Properties.EquipmentIconType.CarrierBasedBomber,
		EquipmentIconType.CarrierBasedTorpedo => Core.Properties.EquipmentIconType.CarrierBasedTorpedo,
		EquipmentIconType.CarrierBasedRecon => Core.Properties.EquipmentIconType.CarrierBasedRecon,
		EquipmentIconType.Seaplane => Core.Properties.EquipmentIconType.Seaplane,
		EquipmentIconType.Radar => Core.Properties.EquipmentIconType.Radar,
		EquipmentIconType.AAShell => Core.Properties.EquipmentIconType.AAShell,
		EquipmentIconType.APShell => Core.Properties.EquipmentIconType.APShell,
		EquipmentIconType.DamageControl => Core.Properties.EquipmentIconType.DamageControl,
		EquipmentIconType.AAGun => Core.Properties.EquipmentIconType.AAGun,
		EquipmentIconType.HighAngleGun => Core.Properties.EquipmentIconType.HighAngleGun,
		EquipmentIconType.DepthCharge => Core.Properties.EquipmentIconType.DepthCharge,
		EquipmentIconType.Sonar => Core.Properties.EquipmentIconType.Sonar,
		EquipmentIconType.Engine => Core.Properties.EquipmentIconType.Engine,
		EquipmentIconType.LandingCraft => Core.Properties.EquipmentIconType.LandingCraft,
		EquipmentIconType.Autogyro => Core.Properties.EquipmentIconType.Autogyro,
		EquipmentIconType.ASPatrol => Core.Properties.EquipmentIconType.ASPatrol,
		EquipmentIconType.ExtraArmor => Core.Properties.EquipmentIconType.ExtraArmor,
		EquipmentIconType.Searchlight => Core.Properties.EquipmentIconType.Searchlight,
		EquipmentIconType.TransportContainer => Core.Properties.EquipmentIconType.TransportContainer,
		EquipmentIconType.RepairFacility => Core.Properties.EquipmentIconType.RepairFacility,
		EquipmentIconType.StarShell => Core.Properties.EquipmentIconType.StarShell,
		EquipmentIconType.CommandFacility => Core.Properties.EquipmentIconType.CommandFacility,
		EquipmentIconType.AviationPersonnel => Core.Properties.EquipmentIconType.AviationPersonnel,
		EquipmentIconType.AADirector => Core.Properties.EquipmentIconType.AADirector,
		EquipmentIconType.Rocket => Core.Properties.EquipmentIconType.Rocket,
		EquipmentIconType.SurfaceShipPersonnel => Core.Properties.EquipmentIconType.SurfaceShipPersonnel,
		EquipmentIconType.FlyingBoat => Core.Properties.EquipmentIconType.FlyingBoat,
		EquipmentIconType.Ration => Core.Properties.EquipmentIconType.Ration,
		EquipmentIconType.Supplies => Core.Properties.EquipmentIconType.Supplies,
		EquipmentIconType.SpecialAmphibiousTank => Core.Properties.EquipmentIconType.SpecialAmphibiousTank,
		EquipmentIconType.LandBasedAttacker => Core.Properties.EquipmentIconType.LandBasedAttacker,
		EquipmentIconType.Interceptor => Core.Properties.EquipmentIconType.Interceptor,
		EquipmentIconType.JetBomberKeiun => Core.Properties.EquipmentIconType.JetBomberKeiun,
		EquipmentIconType.JetBomberKikka => Core.Properties.EquipmentIconType.JetBomberKikka,
		EquipmentIconType.TransportMaterial => Core.Properties.EquipmentIconType.TransportMaterial,
		EquipmentIconType.SubmarineEquipment => Core.Properties.EquipmentIconType.SubmarineEquipment,
		EquipmentIconType.SeaplaneFighter => Core.Properties.EquipmentIconType.SeaplaneFighter,
		EquipmentIconType.LandBasedFighter => Core.Properties.EquipmentIconType.LandBasedFighter,
		EquipmentIconType.NightFighter => Core.Properties.EquipmentIconType.NightFighter,
		EquipmentIconType.NightAttacker => Core.Properties.EquipmentIconType.NightAttacker,
		EquipmentIconType.LandBasedASPatrol => Core.Properties.EquipmentIconType.LandBasedASPatrol,
		EquipmentIconType.LandAssaulter => Core.Properties.EquipmentIconType.LandAssaulter,
		EquipmentIconType.HeavyBomber => Core.Properties.EquipmentIconType.HeavyBomber,
		EquipmentIconType.NightSeaplane => Core.Properties.EquipmentIconType.NightSeaplane,
		EquipmentIconType.NightSeaplaneBomber => Core.Properties.EquipmentIconType.NightSeaplaneBomber,
		EquipmentIconType.ArmyInfantry => Core.Properties.EquipmentIconType.ArmyInfantry,
		EquipmentIconType.SmokeGenerator => Core.Properties.EquipmentIconType.SmokeGenerator,
		EquipmentIconType.BarrageBalloon => Core.Properties.EquipmentIconType.BarrageBalloon,
		EquipmentIconType.LandBasedFighterJet => Core.Properties.EquipmentIconType.LandBasedFighterJet,
		EquipmentIconType.LandBasedFighterShinden => Core.Properties.EquipmentIconType.LandBasedFighter,
		EquipmentIconType.JetBomberHo229 => Core.Properties.EquipmentIconType.JetBomberHo229,
		EquipmentIconType.JetFighterShinden => Core.Properties.EquipmentIconType.JetFighterShinden,
		EquipmentIconType.Locked => Core.Properties.EquipmentIconType.Locked,
		_ => Core.Properties.EquipmentIconType.Unknown,
	};

	public static FuncValueConverter<EquipmentIconType, string> EquipmentIconTypeDisplayConverter { get; } =
		new(ToDisplay);

	public static string ToDisplay(TaskType taskType) => taskType switch
	{
		TaskType.BossKill => QuestTrackerManagerResources.QuestTaskType_BossKill,
		TaskType.Expedition => QuestTrackerManagerResources.QuestTaskType_Expedition,
		TaskType.BattleNodeId => QuestTrackerManagerResources.QuestTaskType_BattleNodeId,
		TaskType.NodeReach => QuestTrackerManagerResources.QuestTaskType_NodeReach,
		TaskType.MapFirstClear => QuestTrackerManagerResources.QuestTaskType_MapFirstClear,
		TaskType.Exercise => QuestTrackerManagerResources.QuestTaskType_Exercise,
		TaskType.EquipmentScrap => QuestTrackerManagerResources.QuestTaskType_EquipmentScrap,
		TaskType.EquipmentCategoryScrap => QuestTrackerManagerResources.QuestTaskType_EquipmentCategoryScrap,
		TaskType.EquipmentCardTypeScrap => QuestTrackerManagerResources.QuestTaskType_EquipmentCardTypeScrap,
		TaskType.EquipmentIconTypeScrap => QuestTrackerManagerResources.QuestTaskType_EquipmentIconTypeScrap,

		_ => "???",
	};

	public static FuncValueConverter<TaskType, string> TaskTypeDisplayConverter { get; } = new(ToDisplay);
}
