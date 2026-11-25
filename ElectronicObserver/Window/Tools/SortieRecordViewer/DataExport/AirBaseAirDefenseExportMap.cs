namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed partial class AirBaseAirDefenseExportMap : CsvExportMap<AirBaseAirDefenseExportModel>
{
	public AirBaseAirDefenseExportMap()
	{
		Map(m => m.Date).Name(() => CsvExportResources.Date);
		Map(m => m.World).Name(() => CsvExportResources.World);
		Map(m => m.Square).Name(() => CsvExportResources.Square);
		Map(m => m.PlayerFormation).Name(() => CsvExportResources.PlayerFormation);
		Map(m => m.EnemyFormation).Name(() => CsvExportResources.EnemyFormation);
		Map(m => m.Engagement).Name(() => CsvExportResources.Engagement);
		Map(m => m.AirBaseDamage).Name(() => CsvExportResources.AirBaseDamage);
		Map(m => m.PlayerAircraft).Name(() => CsvExportResources.PlayerAircraft);
		Map(m => m.PlayerAircraftLost).Name(() => CsvExportResources.PlayerAircraftLost2);
		Map(m => m.EnemyAircraft).Name(() => CsvExportResources.EnemyAircraft);
		Map(m => m.EnemyAircraftLost).Name(() => CsvExportResources.EnemyAircraftLost2);
		Map(m => m.AirState).Name(() => CsvExportResources.AirState);
		Map(m => m.PlayerContact).Name(() => CsvExportResources.PlayerContact);
		Map(m => m.EnemyContact).Name(() => CsvExportResources.EnemyContact);
		References<AirBaseExportMap, AirBaseExportModel>(s => s.AirBase1, m => m.Prefix(CsvExportResources.PrefixAirBase1));
		References<AirBaseExportMap, AirBaseExportModel>(s => s.AirBase2, m => m.Prefix(CsvExportResources.PrefixAirBase2));
		References<AirBaseExportMap, AirBaseExportModel>(s => s.AirBase3, m => m.Prefix(CsvExportResources.PrefixAirBase3));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip1, m => m.Prefix(CsvExportResources.PrefixEnemyShip1));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip2, m => m.Prefix(CsvExportResources.PrefixEnemyShip2));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip3, m => m.Prefix(CsvExportResources.PrefixEnemyShip3));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip4, m => m.Prefix(CsvExportResources.PrefixEnemyShip4));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip5, m => m.Prefix(CsvExportResources.PrefixEnemyShip5));
		References<AirBaseAirDefenseShipExportMap, AirBaseAirDefenseShipExportModel>(s => s.EnemyShip6, m => m.Prefix(CsvExportResources.PrefixEnemyShip6));
		Map(m => m.PlayerTorpedoFlags1).Name(() => $"{CsvExportResources.PrefixAirBase1}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.PlayerTorpedoFlags2).Name(() => $"{CsvExportResources.PrefixAirBase2}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.PlayerTorpedoFlags3).Name(() => $"{CsvExportResources.PrefixAirBase3}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags1).Name(() => $"{CsvExportResources.PrefixEnemyShip1}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags2).Name(() => $"{CsvExportResources.PrefixEnemyShip2}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags3).Name(() => $"{CsvExportResources.PrefixEnemyShip3}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags4).Name(() => $"{CsvExportResources.PrefixEnemyShip4}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags5).Name(() => $"{CsvExportResources.PrefixEnemyShip5}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.EnemyTorpedoFlags6).Name(() => $"{CsvExportResources.PrefixEnemyShip6}{CsvExportResources.TorpedoFlag2}");
		Map(m => m.PlayerBomberFlags1).Name(() => $"{CsvExportResources.PrefixAirBase1}{CsvExportResources.BomberFlag2}");
		Map(m => m.PlayerBomberFlags2).Name(() => $"{CsvExportResources.PrefixAirBase2}{CsvExportResources.BomberFlag2}");
		Map(m => m.PlayerBomberFlags3).Name(() => $"{CsvExportResources.PrefixAirBase3}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags1).Name(() => $"{CsvExportResources.PrefixEnemyShip1}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags2).Name(() => $"{CsvExportResources.PrefixEnemyShip2}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags3).Name(() => $"{CsvExportResources.PrefixEnemyShip3}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags4).Name(() => $"{CsvExportResources.PrefixEnemyShip4}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags5).Name(() => $"{CsvExportResources.PrefixEnemyShip5}{CsvExportResources.BomberFlag2}");
		Map(m => m.EnemyBomberFlags6).Name(() => $"{CsvExportResources.PrefixEnemyShip6}{CsvExportResources.BomberFlag2}");
		Map(m => m.PlayerHitFlags1).Name(() => $"{CsvExportResources.PrefixAirBase1}{CsvExportResources.ClFlag}");
		Map(m => m.PlayerHitFlags2).Name(() => $"{CsvExportResources.PrefixAirBase2}{CsvExportResources.ClFlag}");
		Map(m => m.PlayerHitFlags3).Name(() => $"{CsvExportResources.PrefixAirBase3}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags1).Name(() => $"{CsvExportResources.PrefixEnemyShip1}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags2).Name(() => $"{CsvExportResources.PrefixEnemyShip2}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags3).Name(() => $"{CsvExportResources.PrefixEnemyShip3}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags4).Name(() => $"{CsvExportResources.PrefixEnemyShip4}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags5).Name(() => $"{CsvExportResources.PrefixEnemyShip5}{CsvExportResources.ClFlag}");
		Map(m => m.EnemyHitFlags6).Name(() => $"{CsvExportResources.PrefixEnemyShip6}{CsvExportResources.ClFlag}");
		Map(m => m.PlayerDamage1).Name(() => $"{CsvExportResources.PrefixAirBase1}{CsvExportResources.Damage2}");
		Map(m => m.PlayerDamage2).Name(() => $"{CsvExportResources.PrefixAirBase2}{CsvExportResources.Damage2}");
		Map(m => m.PlayerDamage3).Name(() => $"{CsvExportResources.PrefixAirBase3}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage1).Name(() => $"{CsvExportResources.PrefixEnemyShip1}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage2).Name(() => $"{CsvExportResources.PrefixEnemyShip2}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage3).Name(() => $"{CsvExportResources.PrefixEnemyShip3}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage4).Name(() => $"{CsvExportResources.PrefixEnemyShip4}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage5).Name(() => $"{CsvExportResources.PrefixEnemyShip5}{CsvExportResources.Damage2}");
		Map(m => m.EnemyDamage6).Name(() => $"{CsvExportResources.PrefixEnemyShip6}{CsvExportResources.Damage2}");
	}
}
