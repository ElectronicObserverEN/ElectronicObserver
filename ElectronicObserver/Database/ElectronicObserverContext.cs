using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.MapData;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner.EquipmentAssignment;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CellModel = ElectronicObserver.Database.MapData.CellModel;

namespace ElectronicObserver.Database;

/// <summary>
/// Compiled models - didn't notice any difference for our model:
/// dotnet ef dbcontext optimize --project ElectronicObserver --output-dir Database/CompiledModels --namespace ElectronicObserver.Database.CompiledModels
///
/// Performance issues might be because of https://github.com/dotnet/efcore/issues/18884
/// </summary>
public class ElectronicObserverContext(bool inMemory = false) : DbContext
{
	public DbSet<EventLockPlannerModel> EventLockPlans => Set<EventLockPlannerModel>();
	public DbSet<AutoRefreshModel> AutoRefresh => Set<AutoRefreshModel>();
	public DbSet<WorldModel> Worlds => Set<WorldModel>();
	public DbSet<MapModel> Maps => Set<MapModel>();
	public DbSet<CellModel> Cells => Set<CellModel>();
	public DbSet<ApiFile> ApiFiles => Set<ApiFile>();
	public DbSet<SortieRecord> Sorties => Set<SortieRecord>();
	public DbSet<ExpeditionRecord> Expeditions => Set<ExpeditionRecord>();
	public DbSet<EquipmentUpgradePlanItemModel> EquipmentUpgradePlanItems => Set<EquipmentUpgradePlanItemModel>();
	public DbSet<EquipmentAssignmentItemModel> EquipmentAssignmentItems => Set<EquipmentAssignmentItemModel>();
	public DbSet<ShipTrainingPlanModel> ShipTrainingPlans => Set<ShipTrainingPlanModel>();

	private string DbPath { get; } = Path.Join("Record", "ElectronicObserver.sqlite");
	private bool InMemory { get; } = inMemory;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (InMemory)
		{
			optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
		}
		else
		{
			optionsBuilder.UseSqlite($"Data Source={DbPath}");
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<EventLockPlannerModel>()
			.HasKey(s => s.Id);

		modelBuilder.Entity<EventLockPlannerModel>()
			.Property(s => s.Locks)
			.HasConversion(JsonConverter<List<EventLockModel>>());

		modelBuilder.Entity<EventLockPlannerModel>()
			.Property(s => s.ShipLocks)
			.HasConversion(JsonConverter<List<ShipLockModel>>());

		modelBuilder.Entity<EventLockPlannerModel>()
			.Property(s => s.Phases)
			.HasConversion(JsonConverter<List<EventPhaseModel>>());

		modelBuilder.Entity<WorldModel>()
			.HasKey(w => w.Id);

		modelBuilder.Entity<MapModel>()
			.HasKey(m => m.Id);

		modelBuilder.Entity<CellModel>()
			.HasKey(n => n.Id);

		modelBuilder.Entity<AutoRefreshModel>()
			.HasKey(a => a.Id);

		modelBuilder.Entity<AutoRefreshModel>()
			.Property(a => a.SingleMapModeMap)
			.HasConversion(JsonConverter<MapModel>());

		modelBuilder.Entity<AutoRefreshModel>()
			.Property(a => a.Rules)
			.HasConversion(JsonConverter<List<AutoRefreshRuleModel>>());

		modelBuilder.Entity<ApiFile>()
			.HasKey(a => a.Id);

		modelBuilder.Entity<ApiFile>()
			.Property(a => a.Content)
			.HasConversion(new ValueConverter<string, byte[]>
			(
				s => CompressBytes(System.Text.Encoding.UTF8.GetBytes(s)),
				b => System.Text.Encoding.UTF8.GetString(DecompressBytes(b))
			));

		modelBuilder.Entity<SortieRecord>()
			.Property(s => s.FleetData)
			.HasConversion(JsonConverter<SortieFleetData>());

		modelBuilder.Entity<SortieRecord>()
			.Property(s => s.FleetAfterSortieData)
			.HasConversion(JsonConverter<SortieFleetData?>());

		modelBuilder.Entity<SortieRecord>()
			.Property(s => s.MapData)
			.HasConversion(JsonConverter<SortieMapData>());

		modelBuilder.Entity<SortieRecord>()
			.Property(s => s.CalculatedSortieCost)
			.HasConversion(JsonConverter<CalculatedSortieCost>());

		modelBuilder.Entity<ExpeditionRecord>()
			.Property(s => s.Fleet)
			.HasConversion(JsonConverter<SortieFleet>());
	}

	private static ValueConverter<T, string> JsonConverter<T>() where T : new() => new
	(
		list => JsonSerializer.Serialize(list, JsonSerializerOptions.Default),
		s => FromJson<T>(s)
	);

	private static T FromJson<T>(string s) where T : new() => s switch
	{
		null or "" => new T(),
		_ => JsonSerializer.Deserialize<T>(s)!,
	};

	private static byte[] CompressBytes(byte[] bytes)
	{
		using MemoryStream outputStream = new();
		using (BrotliStream compressStream = new(outputStream, CompressionLevel.SmallestSize))
		{
			compressStream.Write(bytes, 0, bytes.Length);
		}

		return outputStream.ToArray();
	}

	private static byte[] DecompressBytes(byte[] bytes)
	{
		using MemoryStream inputStream = new(bytes);
		using MemoryStream outputStream = new();
		using (BrotliStream decompressStream = new(inputStream, CompressionMode.Decompress))
		{
			decompressStream.CopyTo(outputStream);
		}

		return outputStream.ToArray();
	}
}
