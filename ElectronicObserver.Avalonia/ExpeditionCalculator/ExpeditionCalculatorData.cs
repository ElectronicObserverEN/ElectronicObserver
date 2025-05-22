using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public static class ExpeditionCalculatorData
{
	private static List<Expedition> World1 { get; } =
	[
		new()
		{
			Id = 1,
			DisplayId = "01",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 15, 00),
			Ammo = 30,
		},
		new()
		{
			Id = 2,
			DisplayId = "02",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 30, 00),
			Ammo = 100,
			Steel = 30,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 3,
			DisplayId = "03",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 20, 00),
			Fuel = 30,
			Ammo = 30,
			Steel = 40,
		},
		new()
		{
			Id = 4,
			DisplayId = "04",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 50, 00),
			Ammo = 70,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 5,
			DisplayId = "05",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(01, 30, 00),
			Fuel = 200,
			Ammo = 200,
			Steel = 20,
			Bauxite = 20,
		},
		new()
		{
			Id = 6,
			DisplayId = "06",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 40, 00),
			Bauxite = 80,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 7,
			DisplayId = "07",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(01, 00, 00),
			Steel = 50,
			Bauxite = 30,
			Item1 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 1,
			},
		},
		new()
		{
			Id = 8,
			DisplayId = "08",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(03, 00, 00),
			Fuel = 50,
			Ammo = 100,
			Steel = 50,
			Bauxite = 50,
			Item1 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 100,
			DisplayId = "A1",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 25, 00),
			Fuel = 45,
			Ammo = 45,
		},
		new()
		{
			Id = 101,
			DisplayId = "A2",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(00, 55, 00),
			Fuel = 70,
			Ammo = 40,
			Bauxite = 10,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 102,
			DisplayId = "A3",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(02, 15, 00),
			Fuel = 120,
			Steel = 60,
			Bauxite = 60,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 2,
			},
		},
		new()
		{
			Id = 103,
			DisplayId = "A4",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(01, 50, 00),
			Fuel = 80,
			Ammo = 120,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 2,
			},
		},
		new()
		{
			Id = 104,
			DisplayId = "A5",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(03, 00, 00),
			Ammo = 300,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 4,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 3,
			},
		},
		new()
		{
			Id = 105,
			DisplayId = "A6",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(03, 30, 00),
			Fuel = 100,
			Ammo = 500,
			Steel = 100,
			Bauxite = 200,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 5,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
	];

	private static List<Expedition> World2 { get; } =
	[
		new()
		{
			Id = 9,
			DisplayId = "09",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(04, 00, 00),
			Fuel = 350,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
		},
		new()
		{
			Id = 10,
			DisplayId = "10",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(01, 30, 00),
			Ammo = 50,
			Bauxite = 30,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 1,
			},
		},
		new()
		{
			Id = 11,
			DisplayId = "11",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(05, 00, 00),
			Bauxite = 250,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 12,
			DisplayId = "12",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(08, 00, 00),
			Fuel = 50,
			Ammo = 250,
			Steel = 200,
			Bauxite = 50,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 13,
			DisplayId = "13",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(04, 00, 00),
			Fuel = 240,
			Ammo = 300,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 14,
			DisplayId = "14",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(06, 00, 00),
			Ammo = 240,
			Steel = 200,
			Bauxite = 30,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 15,
			DisplayId = "15",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(12, 00, 00),
			Steel = 300,
			Bauxite = 400,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 16,
			DisplayId = "16",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(15, 00, 00),
			Fuel = 500,
			Ammo = 500,
			Steel = 200,
			Bauxite = 200,
			Item1 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 2,
			},
		},
		new()
		{
			Id = 110,
			DisplayId = "B1",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 35, 00),
			Steel = 10,
			Bauxite = 30,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 111,
			DisplayId = "B2",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = true,
			Duration = new(08, 40, 00),
			Fuel = 300,
			Ammo = 200,
			Steel = 100,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
		},
		new()
		{
			Id = 112,
			DisplayId = "B3",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(02, 50, 00),
			Ammo = 100,
			Steel = 100,
			Bauxite = 180,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
		},
		new()
		{
			Id = 113,
			DisplayId = "B4",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(07, 30, 00),
			Steel = 1200,
			Bauxite = 650,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 4,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 114,
			DisplayId = "B5",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(06, 30, 00),
			Fuel = 500,
			Ammo = 500,
			Steel = 1000,
			Bauxite = 750,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 4,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 115,
			DisplayId = "B6",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(05, 50, 00),
			Fuel = 600,
			Ammo = 1000,
			Steel = 600,
			Bauxite = 600,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 5,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
	];

	private static List<Expedition> World3 { get; } =
	[
		new()
		{
			Id = 17,
			DisplayId = "17",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(00, 45, 00),
			Fuel = 70,
			Ammo = 70,
			Steel = 50,
		},
		new()
		{
			Id = 18,
			DisplayId = "18",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(05, 00, 00),
			Steel = 300,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 19,
			DisplayId = "19",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(06, 00, 00),
			Fuel = 400,
			Ammo = 50,
			Steel = 50,
			Bauxite = 30,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 20,
			DisplayId = "20",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(02, 00, 00),
			Steel = 150,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 21,
			DisplayId = "21",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = false,
			Duration = new(02, 20, 00),
			Fuel = 320,
			Ammo = 270,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 22,
			DisplayId = "22",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(03, 00, 00),
			Ammo = 10,
		},
		new()
		{
			Id = 23,
			DisplayId = "23",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(04, 00, 00),
			Ammo = 20,
			Bauxite = 100,
		},
		new()
		{
			Id = 24,
			DisplayId = "24",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = false,
			Duration = new(08, 20, 00),
			Fuel = 500,
			Bauxite = 150,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
	];

	private static List<Expedition> World7 { get; } =
	[
		new()
		{
			Id = 41,
			DisplayId = "41",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(01, 00, 00),
			Fuel = 100,
			Bauxite = 20,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 42,
			DisplayId = "42",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = true,
			Duration = new(08, 00, 00),
			Fuel = 800,
			Bauxite = 200,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantConstruction,
				Amount = 3,
			},
		},
		new()
		{
			Id = 43,
			DisplayId = "43",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(12, 00, 00),
			Fuel = 2000,
			Bauxite = 400,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 4,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 44,
			DisplayId = "44",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = true,
			Duration = new(10, 00, 00),
			Ammo = 200,
			Bauxite = 800,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 4,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 2,
			},
		},
		new()
		{
			Id = 45,
			DisplayId = "45",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(03, 20, 00),
			Fuel = 40,
			Bauxite = 220,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 1,
			},
		},
		new()
		{
			Id = 46,
			DisplayId = "46",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(03, 30, 00),
			Fuel = 300,
			Steel = 150,
			Bauxite = 380,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 3,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
	];

	private static List<Expedition> World4 { get; } =
	[
		new()
		{
			Id = 25,
			DisplayId = "25",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(40, 00, 00),
			Fuel = 900,
			Steel = 500,
		},
		new()
		{
			Id = 26,
			DisplayId = "26",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(80, 00, 00),
			Bauxite = 900,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 3,
			},
		},
		new()
		{
			Id = 27,
			DisplayId = "27",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(20, 00, 00),
			Steel = 800,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 2,
			},
		},
		new()
		{
			Id = 28,
			DisplayId = "28",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(25, 00, 00),
			Steel = 900,
			Bauxite = 350,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 2,
			},
		},
		new()
		{
			Id = 29,
			DisplayId = "29",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(24, 00, 00),
			Ammo = 50,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 1,
			},
		},
		new()
		{
			Id = 30,
			DisplayId = "30",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(48, 00, 00),
			Ammo = 50,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 3,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
		},
		new()
		{
			Id = 31,
			DisplayId = "31",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(02, 00, 00),
			Ammo = 30,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 32,
			DisplayId = "32",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(24, 00, 00),
			Fuel = 50,
			Ammo = 50,
			Steel = 50,
			Bauxite = 50,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 3,
			},
		},
		new()
		{
			Id = 131,
			DisplayId = "D1",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = false,
			Duration = new(02, 00, 00),
			Ammo = 20,
			Steel = 20,
			Bauxite = 100,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 132,
			DisplayId = "D2",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(10, 00, 00),
			Steel = 400,
			Bauxite = 800,
			Item1 = new()
			{
				Type = UseItemId.MoraleFoodIrako,
				Amount = 1,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 1,
			},
		},
		new()
		{
			Id = 133,
			DisplayId = "D3",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(12, 00, 00),
			Ammo = 800,
			Steel = 500,
			Bauxite = 400,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 3,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
	];

	private static List<Expedition> World5 { get; } =
	[
		new()
		{
			Id = 35,
			DisplayId = "35",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(07, 00, 00),
			Steel = 240,
			Bauxite = 280,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.DevelopmentMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 36,
			DisplayId = "36",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(09, 00, 00),
			Fuel = 480,
			Steel = 200,
			Bauxite = 200,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 37,
			DisplayId = "37",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = false,
			Duration = new(02, 45, 00),
			Ammo = 380,
			Steel = 270,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 2,
			},
		},
		new()
		{
			Id = 38,
			DisplayId = "38",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = false,
			Duration = new(02, 55, 00),
			Fuel = 420,
			Steel = 200,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 1,
			},
		},
		new()
		{
			Id = 39,
			DisplayId = "39",
			GreatSuccessType = GreatSuccessType.Regular,
			IsMonthly = false,
			Duration = new(30, 00, 00),
			Ammo = 30,
			Steel = 300,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.FurnitureBoxMedium,
				Amount = 1,
			},
		},
		new()
		{
			Id = 40,
			DisplayId = "40",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = false,
			Duration = new(06, 50, 00),
			Fuel = 300,
			Ammo = 300,
			Steel = 100,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxSmall,
				Amount = 3,
			},
			Item2 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 1,
			},
		},
		new()
		{
			Id = 141,
			DisplayId = "E1",
			GreatSuccessType = GreatSuccessType.Level,
			IsMonthly = true,
			Duration = new(07, 30, 00),
			Ammo = 600,
			Steel = 600,
			Bauxite = 1000,
			Item1 = new()
			{
				Type = UseItemId.FurnitureBoxLarge,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
		new()
		{
			Id = 142,
			DisplayId = "E2",
			GreatSuccessType = GreatSuccessType.Drum,
			IsMonthly = true,
			Duration = new(03, 05, 00),
			Ammo = 480,
			Item1 = new()
			{
				Type = UseItemId.InstantRepair,
				Amount = 2,
			},
			Item2 = new()
			{
				Type = UseItemId.ImproveMaterial,
				Amount = 1,
			},
		},
	];

	public static List<Expedition> Expeditions { get; } =
	[
		..World1,
		..World2,
		..World3,
		..World7,
		..World4,
		..World5,
	];
}
