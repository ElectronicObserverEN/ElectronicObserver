using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectronicObserver.Window.Tools.Telegram;

[XmlRoot(ElementName = "channel")]
public class Channel
{
	[XmlElement(ElementName = "title")]
	public string Title { get; set; } = "";

	[XmlElement(ElementName = "link")]
	public List<string> Link { get; set; } = new();

	[XmlElement(ElementName = "description")]
	public string Description { get; set; } = "";

	[XmlElement(ElementName = "generator")]
	public string Generator { get; set; } = "";

	[XmlElement(ElementName = "webMaster")]
	public string WebMaster { get; set; } = "";

	[XmlElement(ElementName = "language")]
	public string Language { get; set; } = "";

	[XmlElement(ElementName = "image")]
	public Image Image { get; set; } = new();

	[XmlElement(ElementName = "ttl")]
	public int Ttl { get; set; }

	[XmlElement(ElementName = "item")]
	public List<Item> Item { get; set; } = new();
}
