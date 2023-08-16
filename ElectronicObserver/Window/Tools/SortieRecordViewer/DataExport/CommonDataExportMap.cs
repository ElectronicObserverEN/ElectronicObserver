using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class CommonDataExportMap : ClassMap<CommonDataExportModel>
{
	public CommonDataExportMap()
	{
		Map(m => m.No).Name(CsvExportResources.No);
		Map(m => m.Date).Name(CsvExportResources.Date);
		Map(m => m.World).Name(CsvExportResources.World);
		Map(m => m.Square).Name(CsvExportResources.Square);
		Map(m => m.Sortie).Name(CsvExportResources.Sortie);
		Map(m => m.Rank).Name(CsvExportResources.Rank);
		Map(m => m.EnemyFleet).Name(CsvExportResources.EnemyFleet);
		Map(m => m.AdmiralLevel).Name(CsvExportResources.AdmiralLevel);
		Map(m => m.PlayerFormation).Name(CsvExportResources.PlayerFormation);
		Map(m => m.EnemyFormation).Name(CsvExportResources.EnemyFormation);
		Map(m => m.PlayerSearch).Name(CsvExportResources.PlayerSearch);
		Map(m => m.EnemySearch).Name(CsvExportResources.EnemySearch);
		Map(m => m.AirState).Name(CsvExportResources.AirState);
		Map(m => m.Engagement).Name(CsvExportResources.Engagement);
		Map(m => m.PlayerContact).Name(CsvExportResources.PlayerContact);
		Map(m => m.EnemyContact).Name(CsvExportResources.EnemyContact);
		Map(m => m.PlayerFlare).Name(CsvExportResources.PlayerFlare);
		Map(m => m.EnemyFlare).Name(CsvExportResources.EnemyFlare);
	}
}
