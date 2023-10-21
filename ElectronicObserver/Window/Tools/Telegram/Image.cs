using System.Xml.Serialization;

namespace ElectronicObserver.Window.Tools.Telegram;

[XmlRoot(ElementName = "image")]
public class Image
{
	[XmlElement(ElementName = "url")]
	public string Url { get; set; } = "";

	[XmlElement(ElementName = "title")]
	public string Title { get; set; } = "";

	[XmlElement(ElementName = "link")]
	public string Link { get; set; } = "";
}
