using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicObserver.Database;

public class ElectronicObserverContext : DbContext
{
	public DbSet<EventLockPlannerModel> EventLockPlans { get; set; } = null!;

	private string DbPath { get; }

	public ElectronicObserverContext()
	{
		DbPath = Path.Join("Record", "ElectronicObserver.sqlite");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DbPath}");

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<EventLockPlannerModel>()
			.HasKey(s => s.Id);

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.Locks)
			.HasConversion(EventLockListToStringConverter());

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.ShipLocks)
			.HasConversion(ShipLockListToStringConverter());
	}

	private static ValueConverter<List<EventLockModel>, string> EventLockListToStringConverter() => new
	(
		list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
		s => JsonSerializer.Deserialize<List<EventLockModel>>(s, (JsonSerializerOptions?)null)!
	);

	private static ValueConverter<List<ShipLockModel>, string> ShipLockListToStringConverter() => new
	(
		list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
		s => JsonSerializer.Deserialize<List<ShipLockModel>>(s, (JsonSerializerOptions?)null)!
	);

}
