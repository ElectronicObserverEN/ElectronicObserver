using System;
using System.Text.Json.Serialization;
using DynaJson;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbNodeInfo : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	#region Json Properties 
	[JsonPropertyName("amountOfNodes")]
	public int AmountOfNodes { get; private set; }

	[JsonPropertyName("nodeType")]
	public int NodeType { get; private set; }

	[JsonPropertyName("eventId")]
	public int EventId { get; private set; }

	[JsonPropertyName("eventKind")]
	public int EventKind { get; private set; }

	[JsonPropertyName("nodeColor")]
	public int NodeColor { get; private set; }

	[JsonPropertyName("itemGet")]
	public object[]? ItemGet { get; private set; }
	#endregion

	public TsunDbNodeInfo(int amountOfNodes)
	{
		AmountOfNodes = amountOfNodes;
	}

	#region internal methods
	/// <summary>
	/// Process next node data
	/// </summary>
	/// <param name="apiData"></param>
	internal void ProcessNext(dynamic apiData)
	{
		JsonObject jData = (JsonObject)apiData;

		NodeType = (int)apiData["api_color_no"];
		EventId = (int)apiData["api_event_id"];
		EventKind = (int)apiData["api_event_kind"];
		NodeColor = (int)apiData["api_color_no"];

		if (jData.IsDefined("api_itemget"))
		{
			if (apiData["api_itemget"].IsArray)
			{
				ItemGet = (object[])apiData["api_itemget"];
			}
			else
			{
				// On 6-3 api_itemget is an object and not an array
				ItemGet = new[] { (object)apiData["api_itemget"] };
			}
		}
		else
		{
			ItemGet = Array.Empty<object>();
		}
	}
	#endregion
}
