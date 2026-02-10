using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types;

public class CsvNullableDateConverter : JsonConverter<DateTime?>
{
	public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string? raw = reader.GetString();

		if (raw is null) return null;

		return DateTimeHelper.CSVStringToTime(raw);
	}

	public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
	}
}
