using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Avalonia.UiBlocker;

public static class UiBlockerSerializer
{
	private static JsonSerializerOptions Options { get; } = new()
	{
		WriteIndented = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	public static string Export(IEnumerable<UiBlockerModel> blockers)
	{
		List<UiBlockerModel> copies = [.. blockers.Select(b => b.Clone())];
		foreach (UiElementModel e in copies.SelectMany(b => b.UiElements))
		{
			e.ImagePath = null;
		}

		return JsonSerializer.Serialize(copies, Options);
	}

	public static void Import(string json, List<UiBlockerModel> uiBlockers)
	{
		List<UiBlockerModel>? importBlockers;
		try
		{
			importBlockers = JsonSerializer.Deserialize<List<UiBlockerModel>>(json, Options);
		}
		catch (JsonException)
		{
			return;
		}

		if (importBlockers is null)
		{
			return;
		}


		foreach (UiBlockerModel blocker in importBlockers)
		{
			if (SystemUiBlockers.Ids.Contains(blocker.Id))
			{
				continue;
			}

			UiBlockerModel? existing = uiBlockers.FirstOrDefault(b => b.Id == blocker.Id);
			if (existing is null)
			{
				uiBlockers.Add(blocker);
				continue;
			}

			existing.Name = blocker.Name;
			existing.Description = blocker.Description;
			existing.UiElements = blocker.UiElements;
		}
	}

	public static UiBlockerConfiguration LoadFromFile(string path)
	{
		if (!File.Exists(path)) return UiBlockerConfiguration.Default;

		try
		{
			string json = File.ReadAllText(path);
			UiBlockerConfiguration configuration = JsonSerializer.Deserialize<UiBlockerConfiguration>(json, Options) ?? UiBlockerConfiguration.Default;

			// add missing system blockers if we add new ones in future releases
			foreach (UiBlockerModel uiBlockerModel in SystemUiBlockers.All)
			{
				SystemUiBlockerConfiguration? systemBlockerData = configuration.SystemBlockers
					.FirstOrDefault(b => b.Id == uiBlockerModel.Id);

				if (systemBlockerData is not null) continue;
				
				systemBlockerData = new()
				{
					Id = uiBlockerModel.Id,
					IsEnabled = uiBlockerModel.Id == SystemUiBlockers.TaihaBlocker.Id,
				};
				configuration.SystemBlockers.Add(systemBlockerData);
			}
			
			return configuration;
		}
		catch
		{
			return UiBlockerConfiguration.Default;
		}
	}

	public static async Task SaveToFile(string path, UiBlockerConfiguration uiBlockerConfiguration)
	{
		try
		{
			string json = JsonSerializer.Serialize(uiBlockerConfiguration, Options);
			await File.WriteAllTextAsync(path, json);
		}
		catch
		{
			// safe to ignore?
		}
	}
}
