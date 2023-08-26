using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;
public static class EnumExtension
{
	public static string? GetDescription(this Enum value)
	{
		FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
		if (fieldInfo == null) return null;
		DescriptionAttribute? attribute = (DescriptionAttribute?)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
		return attribute switch
		{
			null => null,
			_ => attribute.Description
		};
	}
}
