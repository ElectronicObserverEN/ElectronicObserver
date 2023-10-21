using System.Xml.Serialization;

namespace ElectronicObserver.Window.Tools.Telegram;

[XmlRoot(ElementName = "guid")]
public class Guid
{
	[XmlAttribute(AttributeName = "isPermaLink")]
	public bool IsPermaLink { get; set; }

	[XmlText]
	public string Text { get; set; } = "";
}
