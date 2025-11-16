using System;
using System.Runtime.Serialization;

namespace ElectronicObserver.Utility.Storage;

[DataContract(Name = "SerializableEnum")]
public class SerializableEnum<T> where T : Enum
{

	[IgnoreDataMember]
	public T? Value { get; set; }

	public SerializableEnum()
	{
		Value = default;
	}

	public SerializableEnum(T value)
	{
		Value = value;
	}

	[DataMember]
	public int? SerializedValue
	{
		get { return EnumToInt(Value); }
		set { Value = IntToEnum(value); }
	}


	public static implicit operator T?(SerializableEnum<T> value)
	{
		if (value == null) return default;
		return value.Value;
	}

	public static implicit operator SerializableEnum<T>(T value)
	{
		return new SerializableEnum<T>(value);
	}


	public static T? IntToEnum(int? serial, bool suppressError = false)
	{
		if (serial is null) return default;

		try
		{
			return (T)Enum.Parse(typeof(T), serial.ToString() ?? "");
		}
		catch (Exception ex)
		{

			if (!suppressError)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "SerializableEnum: IntToEnum " + LoggerRes.Failed);
			}
		}

		return default;
	}

	public static int? EnumToInt(T? value, bool suppressError = false)
	{
		if (value == null) return null;

		try
		{
			return Convert.ToInt32(value);
		}
		catch (Exception ex)
		{
			if (!suppressError)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "SerializableEnum: EnumToInt " + LoggerRes.Failed);
			}
		}

		return null;
	}
}
