﻿using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class SupportTypeTests(DatabaseFixture db)
{
	private DatabaseFixture Db { get; } = db;

	[Fact(DisplayName = "Support needs at least 2 destroyers")]
	public void SupportTypeTest1()
	{
		List<IShipData?> ships =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(ships),
		};

		Assert.Equal(SupportType.None, fleet.SupportType);
	}

	[Fact(DisplayName = "Anti submarine support")]
	public void SupportTypeTest2()
	{
		List<IShipData?> destroyers =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
		];

		List<IShipData?> escorts =
		[
			new ShipDataMock(Db.MasterShips[ShipId.UkuruKai]),
			new ShipDataMock(Db.MasterShips[ShipId.InagiKai]),
		];

		ShipDataMock chitose = new(Db.MasterShips[ShipId.ChitoseCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>(
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			]),
		};
		ShipDataMock chiyoda = new(Db.MasterShips[ShipId.ChiyodaCVLKaiNi])
		{
			SlotInstance = new List<IEquipmentData?>(
			[
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Autogyro_S51JKai]),
			]),
		};

		FleetDataMock fleet = new()
		{
			MembersInstance = new([.. destroyers, .. escorts]),
		};

		Assert.Equal(SupportType.Torpedo, fleet.SupportType);

		fleet.MembersInstance = new([.. destroyers, .. escorts, chitose]);

		Assert.Equal(SupportType.AntiSubmarine, fleet.SupportType);

		fleet.MembersInstance = new([.. destroyers, chitose, chiyoda]);

		Assert.Equal(SupportType.AntiSubmarine, fleet.SupportType);
	}

	/// <summary>
	/// https://twitter.com/Matsu_class_DD/status/1807257792811819063
	/// </summary>
	[Fact(DisplayName = "Shelling support 2CA 2CAV")]
	public void SupportTypeTest3()
	{
		List<IShipData?> ships =
		[
			new ShipDataMock(Db.MasterShips[ShipId.KamikazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.AsakazeKai]),
			new ShipDataMock(Db.MasterShips[ShipId.MyoukouKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.HaguroKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.ToneKaiNi]),
			new ShipDataMock(Db.MasterShips[ShipId.ChikumaKaiNi]),
		];

		FleetDataMock fleet = new()
		{
			MembersInstance = new(ships),
		};

		Assert.Equal(SupportType.Shelling, fleet.SupportType);
	}
}
