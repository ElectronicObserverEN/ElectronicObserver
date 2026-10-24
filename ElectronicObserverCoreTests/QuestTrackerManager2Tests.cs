using System.Collections.Generic;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Group;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.Reference;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipClass;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipCount;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipId;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipLevel;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipPosition;
using ElectronicObserver.Avalonia.QuestTrackerManager2.Conditions.ShipType;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class QuestTrackerManager2Tests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	private FleetEvaluationService FleetEvaluationService { get; } = new(new KCDatabaseMock()
	{
		MasterShips = [with(db.MasterShips.Values)],
	});

	[Fact(DisplayName = "3 DE at positions 0, 1, 2")]
	public void QuestTrackerManager2Test1()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [0, 1, 2], Condition = new ShipTypeCondition { ShipType = ShipTypes.Escort },
					},
				],
			},
		};

		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KunashiriKai]),
			new ShipDataMock(Db.MasterShips[ShipId.UkuruKai]),
			new ShipDataMock(Db.MasterShips[ShipId.InagiKaiNi]),
		];

		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HarukazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.MatsukazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HatakazeKai]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
	}

	// Example 2
	// Organize a fleet with two of Fubuki, Yamato, Shoukaku, Sugi, Jingei,
	// Kinugasa Kai Ni, Mogador, Gotland, Thonburi, Johnston or Fletcher Kai Mod.2/Mk.II
	// as flagship and second ship
	[Fact(DisplayName = "Two eligible ships at flagship and second ship")]
	public void QuestTrackerManager2Test2()
	{
		FleetConditionSet set = new()
		{
			Condition = new PositionCondition
			{
				Positions = [1, 2],
				Condition = new GroupCondition
				{
					Operator = LogicalOperator.Or,
					Conditions =
					[
						new ShipIdCondition { Id = ShipId.Fubuki, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Yamato, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Shoukaku, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Sugi, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Jingei, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.KinugasaKaiNi, Remodel = RemodelComparison.AtLeast },
						new ShipIdCondition { Id = ShipId.Mogador, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Gotland, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Thonburi, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.Johnston, Remodel = RemodelComparison.Any },
						new ShipIdCondition { Id = ShipId.FletcherKaiMod2, Remodel = RemodelComparison.AtLeast },
					],
				},
			},
		};

		// Fubuki Kai Ni as flagship, Yamato Kai as second — both eligible
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FubukiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.YamatoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Fubuki Kai Ni as flagship, ineligible ship as second — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FubukiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
	}

	// Example 3
	// Organize a fleet with Akagi and Kaga OR Shoukaku and Zuikaku as flagship and second ship
	[Fact(DisplayName = "Akagi+Kaga or Shoukaku+Zuikaku at flagship and second ship")]
	public void QuestTrackerManager2Test3()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.Or,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1, 2],
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipIdCondition { Id = ShipId.Akagi, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Kaga, Remodel = RemodelComparison.Any },
							],
						},
					},
					new PositionCondition
					{
						Positions = [1, 2],
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipIdCondition { Id = ShipId.Shoukaku, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Zuikaku, Remodel = RemodelComparison.Any },
							],
						},
					},
				],
			},
		};

		// Akagi Kai Ni E as flagship, Kaga Kai Ni as second
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.AkagiKaiNiE]),
			new ShipDataMock(Db.MasterShips[ShipId.KagaKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Shoukaku Kai Ni A as flagship, Zuikaku Kai Ni A as second
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.ShoukakuKaiNiA]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuikakuKaiNiA]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Akagi as flagship but wrong second ship
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.AkagiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ShoukakuKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.True(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 4
	// Organize a fleet with Fubuki Kai San/San Go (Type 6) as flagship
	// and another Fubuki-class as second ship
	[Fact(DisplayName = "Fubuki Kai San/San Go as flagship, any Fubuki-class as second")]
	public void QuestTrackerManager2Test4()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new ShipIdCondition
						{
							Id = ShipId.FubukiKaiSan, Remodel = RemodelComparison.AtLeast
						},
					},
					new PositionCondition
					{
						Positions = [2], Condition = new ShipClassCondition { ShipClass = ShipClass.Fubuki },
					},
				],
			},
		};

		// Fubuki Kai San as flagship, Shirayuki (Fubuki-class) as second
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FubukiKaiSan]),
			new ShipDataMock(Db.MasterShips[ShipId.ShirayukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Fubuki Kai (not San Ni) as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FubukiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.ShirayukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Fubuki Kai San as flagship, non-Fubuki-class as second — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FubukiKaiSan]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 5
	// Organize a fleet with two AS/CL with one as flagship and two DD/SS(V)
	[Fact(DisplayName = "One AS/CL as flagship, two AS/CL total, two DD/SSV")]
	public void QuestTrackerManager2Test5()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipTypeCondition { ShipType = ShipTypes.SubmarineTender },
								new ShipTypeCondition { ShipType = ShipTypes.LightCruiser },
							],
						},
					},
					new ShipCountCondition
					{
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipTypeCondition { ShipType = ShipTypes.SubmarineTender },
								new ShipTypeCondition { ShipType = ShipTypes.LightCruiser },
							],
						},
						Operator = ComparisonOperator.Equals,
						Count = 2,
					},
					new ShipCountCondition
					{
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipTypeCondition { ShipType = ShipTypes.Destroyer },
								new ShipTypeCondition { ShipType = ShipTypes.SubmarineAircraftCarrier },
							],
						},
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 2,
					},
				],
			},
		};

		// Jingei (AS) as flagship, Ooyodo (CL) as second, two DD
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.JingeiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.OoyodoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		// No AS/CL as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.JingeiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.OoyodoKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
	}

	// Example 6
	// Organize a fleet with Yamato as flagship, Yahagi as second ship
	// and any 4 of Isokaze, Hamakaze, Yukikaze, Fuyutsuki, Suzutsuki,
	// Kasumi, Hatsushimo and Asashimo
	[Fact(DisplayName = "Yamato flagship, Yahagi second, 4 of the specified escorts")]
	public void QuestTrackerManager2Test6()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new ShipIdCondition { Id = ShipId.Yamato, Remodel = RemodelComparison.Any },
					},
					new PositionCondition
					{
						Positions = [2],
						Condition = new ShipIdCondition { Id = ShipId.Yahagi, Remodel = RemodelComparison.Any },
					},
					new ShipCountCondition
					{
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipIdCondition { Id = ShipId.Isokaze, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Hamakaze, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Yukikaze, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Fuyutsuki, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Suzutsuki, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Kasumi, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Hatsushimo, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Asashimo, Remodel = RemodelComparison.Any },
							],
						},
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 4,
					},
				],
			},
		};

		// Yamato Kai Ni as flagship, Yahagi Kai Ni B as second, 4 eligible escorts
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.YahagiKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.IsokazeBKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HamakazeBKai]),
			new ShipDataMock(Db.MasterShips[ShipId.YukikazeKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.FuyutsukiKai]),
		];

		// Only 3 eligible escorts — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.YahagiKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.IsokazeBKai]),
			new ShipDataMock(Db.MasterShips[ShipId.HamakazeBKai]),
			new ShipDataMock(Db.MasterShips[ShipId.YukikazeKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Nagato]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
	}

	// Example 7
	// Organize a fleet with an AS/CT/DD as flagship and up to 4 other
	// DD/DE/SS(V) in your fleet (5 ship max)
	[Fact(DisplayName = "AS/CT/DD flagship, up to 4 DD/DE/SSV, 5 ships max")]
	public void QuestTrackerManager2Test7()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipTypeCondition { ShipType = ShipTypes.SubmarineTender },
								new ShipTypeCondition { ShipType = ShipTypes.TrainingCruiser },
								new ShipTypeCondition { ShipType = ShipTypes.Destroyer },
							],
						},
					},
					new ShipCountCondition { Condition = null, Operator = ComparisonOperator.LessOrEqual, Count = 5, },
				],
			},
		};

		// Jingei (AS) as flagship, 4 DD — valid
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.JingeiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.FuyutsukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.SuzutsukiKai]),
		];

		// Nagato (BB) as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.NagatoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		// 6 ships — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.JingeiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.FuyutsukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.SuzutsukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Kasumi]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 8
	// Organize a fleet with Tamanami Kai Ni as flagship and Fujinami in your fleet
	[Fact(DisplayName = "Tamanami Kai Ni flagship, Fujinami anywhere")]
	public void QuestTrackerManager2Test8()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new ShipIdCondition
						{
							Id = ShipId.TamanamiKaiNi, Remodel = RemodelComparison.AtLeast
						},
					},
					new ShipIdCondition { Id = ShipId.Fujinami, Remodel = RemodelComparison.Any },
				],
			},
		};

		// Tamanami Kai Ni as flagship, Fujinami Kai Ni anywhere
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.TamanamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.FujinamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Tamanami Kai (not Kai Ni) as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.TamanamiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fujinami]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Tamanami Kai Ni but no Fujinami — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.TamanamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 9
	// Organize a fleet with any 2 of Harusame, Harukaze, Kasuga Maru, Yayoi,
	// Kagerou, Amatsukaze, Oboro, Akebono, Haruna and Scirocco
	// with one as flagship
	[Fact(DisplayName = "2 of the eligible ships, one as flagship")]
	public void QuestTrackerManager2Test9()
	{
		FleetConditionSet set = new()
		{
			Definitions =
			[
				new ReferenceDefinition
				{
					Name = "A*",
					Condition = new GroupCondition
					{
						Operator = LogicalOperator.Or,
						Conditions =
						[
							new ShipIdCondition { Id = ShipId.Harusame, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Harukaze, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.KasugaMaru, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Yayoi, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Kagerou, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Amatsukaze, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Oboro, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Akebono, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Haruna, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Scirocco, Remodel = RemodelComparison.Any },
						],
					},
				},
			],
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition { Positions = [1], Condition = new ReferenceCondition { Reference = "A*" }, },
					new ShipCountCondition
					{
						Condition = new ReferenceCondition { Reference = "A*" },
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 2,
					},
				],
			},
		};

		// Harusame Kai Ni as flagship, Kagerou Kai Ni also in fleet
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.HarusameKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Eligible ship not as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.HarusameKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi]),
		];

		// Only 1 eligible ship — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.HarusameKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 10
	// Organize a fleet with Hiryuu Kai San as flagship and any 2 of
	// Kazagumo, Tamanami, Suzunami, Fujinami, Hayanami and Hamanami
	[Fact(DisplayName = "Hiryuu Kai San flagship, 2 of the specified escorts")]
	public void QuestTrackerManager2Test10()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition
					{
						Positions = [1],
						Condition = new ShipIdCondition
						{
							Id = ShipId.HiryuuKaiSan, Remodel = RemodelComparison.Exact
						},
					},
					new ShipCountCondition
					{
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.Or,
							Conditions =
							[
								new ShipIdCondition { Id = ShipId.Kazagumo, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Tamanami, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Suzunami, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Fujinami, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Hayanami, Remodel = RemodelComparison.Any },
								new ShipIdCondition { Id = ShipId.Hamanami, Remodel = RemodelComparison.Any },
							],
						},
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 2,
					},
				],
			},
		};

		// Hiryuu Kai San as flagship, Kazagumo Kai Ni and Tamanami Kai in fleet
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.HiryuuKaiSan]),
			new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.TamanamiKai]),
		];

		// Hiryuu Kai Ni (not Kai San) as flagship — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.HiryuuKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.TamanamiKai]),
		];

		// Only 1 eligible escort — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.HiryuuKaiSan]),
			new ShipDataMock(Db.MasterShips[ShipId.KazagumoKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 11
	// Organize a fleet with 2 Mogami-class and 1 CVL not counting Mogami-class ships
	[Fact(DisplayName = "2 Mogami-class and 1 CVL beyond the Mogami-class ships")]
	public void QuestTrackerManager2Test11()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new ShipCountCondition
					{
						Condition = new ShipClassCondition { ShipClass = ShipClass.Mogami },
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 2,
					},
					new GroupCondition
					{
						Operator = LogicalOperator.Or,
						Conditions =
						[
							new GroupCondition
							{
								Operator = LogicalOperator.And,
								Conditions =
								[
									new ShipCountCondition
									{
										Condition = new GroupCondition
										{
											Operator = LogicalOperator.And,
											Conditions =
											[
												new ShipClassCondition { ShipClass = ShipClass.Mogami },
												new ShipTypeCondition { ShipType = ShipTypes.LightAircraftCarrier },
											],
										},
										Operator = ComparisonOperator.Equals,
										Count = 0,
									},
									new ShipCountCondition
									{
										Condition = new ShipTypeCondition
										{
											ShipType = ShipTypes.LightAircraftCarrier
										},
										Operator = ComparisonOperator.GreaterOrEqual,
										Count = 1,
									},
								],
							},
							new GroupCondition
							{
								Operator = LogicalOperator.And,
								Conditions =
								[
									new ShipCountCondition
									{
										Condition = new GroupCondition
										{
											Operator = LogicalOperator.And,
											Conditions =
											[
												new ShipClassCondition { ShipClass = ShipClass.Mogami },
												new ShipTypeCondition { ShipType = ShipTypes.LightAircraftCarrier },
											],
										},
										Operator = ComparisonOperator.Equals,
										Count = 1,
									},
									new ShipCountCondition
									{
										Condition = new ShipTypeCondition
										{
											ShipType = ShipTypes.LightAircraftCarrier
										},
										Operator = ComparisonOperator.GreaterOrEqual,
										Count = 2,
									},
								],
							},
							new GroupCondition
							{
								Operator = LogicalOperator.And,
								Conditions =
								[
									new ShipCountCondition
									{
										Condition = new GroupCondition
										{
											Operator = LogicalOperator.And,
											Conditions =
											[
												new ShipClassCondition { ShipClass = ShipClass.Mogami },
												new ShipTypeCondition { ShipType = ShipTypes.LightAircraftCarrier },
											],
										},
										Operator = ComparisonOperator.GreaterOrEqual,
										Count = 2,
									},
									new ShipCountCondition
									{
										Condition = new ShipTypeCondition
										{
											ShipType = ShipTypes.LightAircraftCarrier
										},
										Operator = ComparisonOperator.GreaterOrEqual,
										Count = 3,
									},
								],
							},
						],
					},
				],
			},
		};

		// 2 non-CVL Mogami-class + 1 CVL elsewhere
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.MogamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.MikumaKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// 2 CVL Mogami-class + 1 CVL elsewhere
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.SuzuyaCVLKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.KumanoCVLKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// 2 CVL Mogami-class but no additional CVL — fails
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.SuzuyaCVLKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.KumanoCVLKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
			new ShipDataMock(Db.MasterShips[ShipId.Suzutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.True(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 12
	// Organize a fleet with two of Oboro, Ukuru, Inagi, Yuudachi, Asashio,
	// Kiyoshimo and Zuihou Kai Ni/Kai Ni B in your fleet with one as flagship
	[Fact(DisplayName = "2 of the eligible ships, one as flagship")]
	public void QuestTrackerManager2Test12()
	{
		FleetConditionSet set = new()
		{
			Definitions =
			[
				new ReferenceDefinition
				{
					Name = "A*",
					Condition = new GroupCondition
					{
						Operator = LogicalOperator.Or,
						Conditions =
						[
							new ShipIdCondition { Id = ShipId.Oboro, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Ukuru, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Inagi, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Yuudachi, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Asashio, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Kiyoshimo, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.ZuihouKaiNi, Remodel = RemodelComparison.Exact },
							new ShipIdCondition { Id = ShipId.ZuihouKaiNiB, Remodel = RemodelComparison.Exact },
						],
					},
				},
			],
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition { Positions = [1], Condition = new ReferenceCondition { Reference = "A*" }, },
					new ShipCountCondition
					{
						Condition = new ReferenceCondition { Reference = "A*" },
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 2,
					},
				],
			},
		};

		// Oboro Kai as flagship, Zuihou Kai Ni B also in fleet
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.OboroKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKaiNiB]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		// Zuihou Kai (not Kai Ni) — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.OboroKai]),
			new ShipDataMock(Db.MasterShips[ShipId.ZuihouKai]),
			new ShipDataMock(Db.MasterShips[ShipId.Fuyutsuki]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
	}

	// Example 13
	// Organize a fleet with 3 of the New Orleans-class, Heywood L. Edwards,
	// Gambier Bay, Ooyodo, Ooi, Takanami, Fujinami, Hamanami, Hayanami and
	// Fuyutsuki with one as flagship
	[Fact(DisplayName = "3 of the eligible ships, one as flagship")]
	public void QuestTrackerManager2Test13()
	{
		FleetConditionSet set = new()
		{
			Definitions =
			[
				new ReferenceDefinition
				{
					Name = "A*",
					Condition = new GroupCondition
					{
						Operator = LogicalOperator.Or,
						Conditions =
						[
							new ShipClassCondition { ShipClass = ShipClass.NewOrleans },
							new ShipIdCondition { Id = ShipId.HeywoodLE, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.GambierBay, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Ooyodo, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Ooi, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Takanami, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Fujinami, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Hamanami, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Hayanami, Remodel = RemodelComparison.Any },
							new ShipIdCondition { Id = ShipId.Fuyutsuki, Remodel = RemodelComparison.Any },
						],
					},
				},
			],
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new PositionCondition { Positions = [1], Condition = new ReferenceCondition { Reference = "A*" }, },
					new ShipCountCondition
					{
						Condition = new ReferenceCondition { Reference = "A*" },
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 3,
					},
				],
			},
		};

		// Fuyutsuki Kai as flagship, Fujinami Kai Ni and Hamanami Kai Ni also in fleet
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FuyutsukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.FujinamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.HamanamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Nagato]),
		];

		// Only 2 eligible ships — fails
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.FuyutsukiKai]),
			new ShipDataMock(Db.MasterShips[ShipId.FujinamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Nagato]),
		];

		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Minneapolis]),
			new ShipDataMock(Db.MasterShips[ShipId.FujinamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.HamanamiKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.Nagato]),
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.True(FleetEvaluationService.Evaluate(set, ships3));
	}

	// Example 14
	// Have Kagerou Kai Ni, Shiranui Kai Ni, Kuroshio Kai Ni,
	// and three level 70+ Kagerou/Yuugumo-class DD
	[Fact(DisplayName = "Kagerou Kai Ni, Shiranui Kai Ni, Kuroshio Kai Ni + 3 Lv70+ Kagerou/Yuugumo DD")]
	public void QuestTrackerManager2Test14()
	{
		FleetConditionSet set = new()
		{
			Condition = new GroupCondition
			{
				Operator = LogicalOperator.And,
				Conditions =
				[
					new ShipIdCondition { Id = ShipId.KagerouKaiNi, Remodel = RemodelComparison.AtLeast },
					new ShipIdCondition { Id = ShipId.ShiranuiKaiNi, Remodel = RemodelComparison.AtLeast },
					new ShipIdCondition { Id = ShipId.KuroshioKaiNi, Remodel = RemodelComparison.AtLeast },
					new ShipCountCondition
					{
						Condition = new GroupCondition
						{
							Operator = LogicalOperator.And,
							Conditions =
							[
								new GroupCondition
								{
									Operator = LogicalOperator.Or,
									Conditions =
									[
										new ShipClassCondition { ShipClass = ShipClass.Kagerou },
										new ShipClassCondition { ShipClass = ShipClass.Yuugumo },
									],
								},
								new ShipLevelCondition { Level = 70 },
							],
						},
						Operator = ComparisonOperator.GreaterOrEqual,
						Count = 3,
					},
				],
			},
		};

		// All three named ships at Kai Ni or beyond, all level 70+
		List<IShipData> ships1 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi]) { Level = 80 },
			new ShipDataMock(Db.MasterShips[ShipId.ShiranuiKaiNi]) { Level = 75 },
			new ShipDataMock(Db.MasterShips[ShipId.KuroshioKaiNi]) { Level = 70 },
		];

		// Kagerou Kai (not Kai Ni) — fails AtLeast check
		List<IShipData> ships2 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KagerouKai]) { Level = 80 },
			new ShipDataMock(Db.MasterShips[ShipId.ShiranuiKaiNi]) { Level = 75 },
			new ShipDataMock(Db.MasterShips[ShipId.KuroshioKaiNi]) { Level = 70 },
		];

		// All three named ships present but one is under level 70 — fails count
		List<IShipData> ships3 =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KagerouKaiNi]) { Level = 80 },
			new ShipDataMock(Db.MasterShips[ShipId.ShiranuiKaiNi]) { Level = 75 },
			new ShipDataMock(Db.MasterShips[ShipId.KuroshioKaiNi]) { Level = 69 },
		];

		Assert.True(FleetEvaluationService.Evaluate(set, ships1));
		Assert.False(FleetEvaluationService.Evaluate(set, ships2));
		Assert.False(FleetEvaluationService.Evaluate(set, ships3));
	}
}
