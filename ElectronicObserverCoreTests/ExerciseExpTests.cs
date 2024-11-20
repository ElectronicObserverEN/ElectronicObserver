using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class ExerciseExpTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "https://twitter.com/yukicacoon/status/1858776234035212674")]
	public void ExerciseExpTest01()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KashimaKai]) { Level = 163 },
			new ShipDataMock(Db.MasterShips[ShipId.KongouKai]) { Level = 42 },
			new ShipDataMock(Db.MasterShips[ShipId.KitakamiKai]) { Level = 46 },
			new ShipDataMock(Db.MasterShips[ShipId.Maruyu]) { Level = 18 },
			new ShipDataMock(Db.MasterShips[ShipId.Fubuki]) { Level = 51 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 33 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 750);

		Assert.Equal(945, (int?)exp.TrainingCruiserSurfaceA);
		Assert.Equal(1134, (int?)exp.TrainingCruiserSurfaceS);
	}

	[Fact]
	public void ExerciseExpTest02()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 9 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 710);

		Assert.Equal(670, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(969, (int?)exp.TrainingCruiserSubmarineA);
	}

	[Fact]
	public void ExerciseExpTest03()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 10 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 735);

		Assert.Equal(714, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1031, (int?)exp.TrainingCruiserSubmarineA);
	}

	[Fact]
	public void ExerciseExpTest04()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 10 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 837);

		Assert.Equal(813, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1175, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(542, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest05()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]),
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 11 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 886);

		Assert.Equal(1329, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(886, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(886, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest06()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 13 },
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 688);

		Assert.Equal(1170, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1077, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(780, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest07()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 14 },
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 848);

		Assert.Equal(1442, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1327, (int?)exp.TrainingCruiserSubmarineA);
	}

	[Fact]
	public void ExerciseExpTest08()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 15 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 963);

		Assert.Equal(1638, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1508, (int?)exp.TrainingCruiserSubmarineA);
	}

	[Fact]
	public void ExerciseExpTest09()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 16 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 787);

		Assert.Equal(1338, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1232, (int?)exp.TrainingCruiserSubmarineA);
	}

	[Fact]
	public void ExerciseExpTest10()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 17 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 636);

		Assert.Equal(1202, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1106, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(801, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest11()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 716);

		Assert.Equal(1342, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(895, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(895, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest12()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]),
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 17 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 782);

		Assert.Equal(2697, (int?)(exp.TrainingCruiserSurfaceA * 1.5 * 2));
		Assert.Equal(899, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(899, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest13()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 18 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 842);

		Assert.Equal(1060, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest14()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 18 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 1046);

		Assert.Equal(1976, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1820, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(1317, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest15()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 19 },
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 634);

		Assert.Equal(1128, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1038, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(752, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest16()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 98 },
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 20 },
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 162 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 705);

		Assert.Equal(1276, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(2351, (int?)(exp.TrainingCruiserSubmarineA * 2));
		Assert.Equal(851, (int?)exp.TrainingCruiserSurfaceA);
	}

	[Fact]
	public void ExerciseExpTest17()
	{
		List<IShipData?> members =
		[
			new ShipDataMock(Db.MasterShips[ShipId.Asahi]) { Level = 32 },
			new ShipDataMock(Db.MasterShips[ShipId.Katori]) { Level = 175 },
			new ShipDataMock(Db.MasterShips[ShipId.Kashima]) { Level = 175 },
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(members),
		};
		ExerciseExp exp = Calculator.GetExerciseExp(fleet, 691);

		Assert.Equal(1262, (int?)(exp.TrainingCruiserSurfaceA * 1.5));
		Assert.Equal(1162, (int?)exp.TrainingCruiserSubmarineA);
		Assert.Equal(841, (int?)exp.TrainingCruiserSurfaceA);
	}
}
