using System.Xml.Serialization;

namespace ElectronicObserver.Window.Tools.Telegram;

[XmlRoot(ElementName = "rss")]
public class Rss
{
	[XmlElement(ElementName = "channel")]
	public Channel Channel { get; set; } = new();

	[XmlAttribute(AttributeName = "atom")]
	public string Atom { get; set; } = "";

	[XmlAttribute(AttributeName = "version")]
	public double Version { get; set; }

	[XmlText]
	public string Text { get; set; } = "";
}
