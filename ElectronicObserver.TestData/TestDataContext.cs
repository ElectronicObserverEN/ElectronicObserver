using ElectronicObserver.Core.Types;
using ElectronicObserver.TestData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicObserver.TestData;

public class TestDataContext : DbContext
{
	public DbSet<ShipDataMasterRecord> MasterShips { get; set; } = null!;
	public DbSet<EquipmentDataMasterRecord> MasterEquipment { get; set; } = null!;

	private string DbPath { get; }

	public TestDataContext()
	{
		DbPath = Path.Join("Generated", "TestData.sqlite");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DbPath}");

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<ShipDataMasterRecord>()
			.HasKey(s => s.ShipId);

		builder.Entity<ShipDataMasterRecord>()
			.Property(s => s.EquippableCategories)
			.HasConversion(EnumerableEquipmentTypesToStringConverter());

		builder.Entity<ShipDataMasterRecord>()
			.Property(s => s.Material)
			.HasConversion(ListIntToStringConverter());

		builder.Entity<ShipDataMasterRecord>()
			.Property(s => s.PowerUp)
			.HasConversion(ListIntToStringConverter());

		builder.Entity<EquipmentDataMasterRecord>()
			.HasKey(s => s.EquipmentId);
	}

	private ValueConverter<IEnumerable<EquipmentTypes>, string> EnumerableEquipmentTypesToStringConverter()
	{
		const string separator = ",";
		int i = 0;

		return new
		(
			v => string.Join(separator, v.Select(e => (int)e)),
			v => v.Split(separator, StringSplitOptions.None).Where(s => int.TryParse(s, out i)).Select(s => (EquipmentTypes)i)
		);
	}

	private ValueConverter<IList<int>, string> ListIntToStringConverter()
	{
		const string separator = ",";
		int i = 0;

		return new 
		(
			v => string.Join(separator, v),
			v => v.Split(separator, StringSplitOptions.None).Where(s => int.TryParse(s, out i)).Select(s => i).ToList()
		);
	}
}
