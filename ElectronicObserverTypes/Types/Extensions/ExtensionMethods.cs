using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace ElectronicObserver.Core.Types.Extensions;

// I have no better name right now
public static class ExtensionMethods
{
	public static string Display(this Enum enumValue) =>
		enumValue.GetType()
			.GetMember(enumValue.ToString())
			.First()?
			.GetCustomAttribute<DisplayAttribute>()?
			.GetName() ?? enumValue.ToString();

	// meant to be used for getting a specific language of a localized enum
	// for example when you want English EO but Japanese ship types
	// todo: not tested
	public static string Display(this Enum enumValue, CultureInfo cultureInfo)
	{
		DisplayAttribute? attribute = enumValue.GetType()
			.GetMember(enumValue.ToString())
			.First()?
			.GetCustomAttribute<DisplayAttribute>();

		if (attribute is null) return enumValue.ToString();

		Type? resourceType = attribute.ResourceType;
		string? resourceKey = attribute.Name;

		if (resourceType is null) return enumValue.ToString();
		if (resourceKey is null) return enumValue.ToString();

		PropertyInfo? resourceManagerMethodInfo = resourceType.GetProperty(nameof(ResourceManager), BindingFlags.Public | BindingFlags.Static);

		ResourceManager? resourceManager = (ResourceManager?)resourceManagerMethodInfo?.GetValue(null);

		return resourceManager?.GetString(resourceKey, cultureInfo) ?? enumValue.ToString();
	}
}
