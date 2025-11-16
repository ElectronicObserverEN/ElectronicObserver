using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml;
using MessagePack;

namespace ElectronicObserver.Utility.Storage;

/// <summary>
/// 汎用データ保存クラスの基底です。
/// 使用時は DataContractAttribute を設定してください。
/// </summary>
[DataContract(Name = "DataStorage")]
public abstract class DataStorage : IExtensibleDataObject
{
	// ExtensionDataObject is for DataContractSerializer only
	// MessagePack should ignore it
	[IgnoreMember]
	public ExtensionDataObject ExtensionData { get; set; }

	public abstract void Initialize();

	public static JsonSerializerOptions SerializerOptions { get; } = new()
	{
		WriteIndented = true,
	};

	public DataStorage()
	{
	}

	[OnDeserializing]
	private void DefaultDeserializing(StreamingContext sc)
	{
		Initialize();
	}


	public void Save(string path)
	{

		try
		{
			using Stream stream = new FileStream($"{path}.json", FileMode.Create);

			JsonSerializer.Serialize(stream, this, GetType(), SerializerOptions);
		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "Failed to write " + GetType().Name);
		}

	}

	public DataStorage Load(string path)
	{
		try
		{
			if (File.Exists($"{path}.json"))
			{
				using Stream stream = new FileStream($"{path}.json", FileMode.Open);

				return (DataStorage)JsonSerializer.Deserialize(stream, GetType());
			}
			else
			{
				// Backward compatibility
				var serializer = new DataContractSerializer(GetType());

				using XmlReader xr = XmlReader.Create($"{path}.xml");

				return (DataStorage)serializer.ReadObject(xr);
			}
		}
		catch (FileNotFoundException)
		{

			Utility.Logger.Add(3, string.Format("{0}: {1} does not exists.", GetType().Name, path));

		}
		catch (DirectoryNotFoundException)
		{

			Utility.Logger.Add(3, string.Format("{0}: {1} does not exists.", GetType().Name, path));

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, " Failed to load " + GetType().Name);

		}

		return null;
	}
}
