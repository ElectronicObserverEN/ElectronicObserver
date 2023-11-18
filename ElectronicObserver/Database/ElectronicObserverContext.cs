using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

// Add-Migration <name> -Context ElectronicObserverContext -OutputDir Database/Migrations
public class ElectronicObserverContext : DbContext
{
	public DbSet<EventLockPlannerModel> EventLockPlans { get; set; } = null!;
	public DbSet<AutoRefreshModel> AutoRefresh { get; set; } = null!;
	public DbSet<WorldModel> Worlds { get; set; } = null!;
	public DbSet<MapModel> Maps { get; set; } = null!;
	public DbSet<CellModel> Cells { get; set; } = null!;
	public DbSet<ApiFile> ApiFiles { get; set; } = null!;
	public DbSet<SortieRecord> Sorties { get; set; } = null!;
	public DbSet<ExpeditionRecord> Expeditions { get; set; } = null!;
	public DbSet<EquipmentUpgradePlanItemModel> EquipmentUpgradePlanItems { get; set; } = null!;
	public DbSet<EquipmentAssignmentItemModel> EquipmentAssignmentItems { get; set; } = null!;
	public DbSet<ShipTrainingPlanModel> ShipTrainingPlans { get; set; } = null!;

	private string DbPath { get; }
	private bool InMemory { get; }

	public ElectronicObserverContext(bool inMemory = false)
	{
		DbPath = Path.Join("Record", "ElectronicObserver.sqlite");
		InMemory = inMemory;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (InMemory)
		{
			options.UseInMemoryDatabase("ElectronicObserver");
		}
		else
		{
			options.UseSqlite($"Data Source={DbPath}");
		}
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<EventLockPlannerModel>()
			.HasKey(s => s.Id);

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.Locks)
			.HasConversion(JsonConverter<List<EventLockModel>>());

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.ShipLocks)
			.HasConversion(JsonConverter<List<ShipLockModel>>());

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.Phases)
			.HasConversion(JsonConverter<List<EventPhaseModel>>());

		builder.Entity<WorldModel>()
			.HasKey(w => w.Id);

		builder.Entity<MapModel>()
			.HasKey(m => m.Id);

		builder.Entity<CellModel>()
			.HasKey(n => n.Id);

		builder.Entity<AutoRefreshModel>()
			.HasKey(a => a.Id);

		builder.Entity<AutoRefreshModel>()
			.Property(a => a.SingleMapModeMap)
			.HasConversion(JsonConverter<MapModel>());

		builder.Entity<AutoRefreshModel>()
			.Property(a => a.Rules)
			.HasConversion(JsonConverter<List<AutoRefreshRuleModel>>());

		builder.Entity<ApiFile>()
			.HasKey(a => a.Id);

		builder.Entity<ApiFile>()
			.Property(a => a.Content)
			.HasConversion(new ValueConverter<string, byte[]>
			(
				s => CompressBytes(System.Text.Encoding.UTF8.GetBytes(s)),
				b => System.Text.Encoding.UTF8.GetString(DecompressBytes(b))
			));

		builder.Entity<SortieRecord>()
			.Property(s => s.FleetData)
			.HasConversion(JsonConverter<SortieFleetData>());

		builder.Entity<SortieRecord>()
			.Property(s => s.FleetAfterSortieData)
			.HasConversion(JsonConverter<SortieFleetData?>());

		builder.Entity<SortieRecord>()
			.Property(s => s.MapData)
			.HasConversion(JsonConverter<SortieMapData>());

		builder.Entity<ExpeditionRecord>()
			.Property(s => s.Fleet)
			.HasConversion(JsonConverter<SortieFleet>());
	}

	private static ValueConverter<T, string> JsonConverter<T>() where T : new() => new
	(
		list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
		s => FromJson<T>(s)
	);

	private static T FromJson<T>(string s) where T : new() => s switch
	{
		null or "" => new T(),
		_ => JsonSerializer.Deserialize<T>(s)!
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

	private static async Task<byte[]> CompressBytesAsync(byte[] bytes, CancellationToken cancel = default)
	{
		using var outputStream = new MemoryStream();
		await using (var compressStream = new BrotliStream(outputStream, CompressionLevel.SmallestSize))
		{
			await compressStream.WriteAsync(bytes, 0, bytes.Length, cancel);
		}

		return outputStream.ToArray();
	}

	private static async Task<byte[]> DecompressBytesAsync(byte[] bytes, CancellationToken cancel = default)
	{
		using var inputStream = new MemoryStream(bytes);
		using var outputStream = new MemoryStream();
		await using (var decompressStream = new BrotliStream(inputStream, CompressionMode.Decompress))
		{
			await decompressStream.CopyToAsync(outputStream, cancel);
		}

		return outputStream.ToArray();
	}
}
