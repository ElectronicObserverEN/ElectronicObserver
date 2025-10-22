using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
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

			var serializer = new DataContractSerializer(this.GetType());
			var xmlsetting = new XmlWriterSettings
			{
				Encoding = new System.Text.UTF8Encoding(false),
				Indent = true,
				IndentChars = "\t",
				NewLineHandling = NewLineHandling.Replace
			};

			using (XmlWriter xw = XmlWriter.Create(path, xmlsetting))
			{
				serializer.WriteObject(xw, this);
			}


		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "Failed to write " + GetType().Name);
		}

	}

	public void Save(StringBuilder stringBuilder)
	{
		try
		{
			var serializer = new DataContractSerializer(this.GetType());
			using (XmlWriter xw = XmlWriter.Create(stringBuilder))
			{
				serializer.WriteObject(xw, this);
			}
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, GetType().Name + " の書き込みに失敗しました。");
		}
	}

	public DataStorage Load(string path)
	{
		try
		{
			var serializer = new DataContractSerializer(this.GetType());

			using (XmlReader xr = XmlReader.Create(path))
			{
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



	public void Save(Stream stream)
	{

		try
		{

			var serializer = new DataContractSerializer(this.GetType());
			var xmlsetting = new XmlWriterSettings
			{
				Encoding = new System.Text.UTF8Encoding(false),
				Indent = true,
				IndentChars = "\t",
				NewLineHandling = NewLineHandling.Replace
			};

			using (XmlWriter xw = XmlWriter.Create(stream, xmlsetting))
			{

				serializer.WriteObject(xw, this);
			}


		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "Failed to write " + GetType().Name);
		}

	}


	public DataStorage Load(Stream stream)
	{

		try
		{

			var serializer = new DataContractSerializer(this.GetType());

			using (XmlReader xr = XmlReader.Create(stream))
			{
				return (DataStorage)serializer.ReadObject(xr);
			}

		}
		catch (FileNotFoundException)
		{

			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");

		}
		catch (DirectoryNotFoundException)
		{

			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "Failed to load " + GetType().Name);

		}

		return null;
	}

	public DataStorage Load(TextReader reader)
	{
		try
		{
			var serializer = new DataContractSerializer(this.GetType());

			using (XmlReader xr = XmlReader.Create(reader))
			{
				return (DataStorage)serializer.ReadObject(xr);
			}
		}
		catch (DirectoryNotFoundException)
		{

			Utility.Logger.Add(3, GetType().Name + ": File does not exists.");

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, "Failed to load " + GetType().Name);

		}

		return null;
	}

}
