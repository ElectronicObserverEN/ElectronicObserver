using System.Xml.Serialization;

namespace ElectronicObserver.Window.Tools.Telegram;

[XmlRoot(ElementName = "link")]
public class Link
{
	[XmlAttribute(AttributeName = "href")]
	public string Href { get; set; } = "";

	[XmlAttribute(AttributeName = "rel")]
	public string Rel { get; set; } = "";

	[XmlAttribute(AttributeName = "type")]
	public string Type { get; set; } = "";
}
