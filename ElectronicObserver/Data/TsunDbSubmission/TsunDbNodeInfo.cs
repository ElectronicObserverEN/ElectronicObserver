using System;
using DynaJson;
using Newtonsoft.Json;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbNodeInfo : TsunDbEntity
{
	protected override string Url => throw new NotImplementedException();

	#region Json Properties 
	[JsonProperty("amountOfNodes")]
	public int AmountOfNodes { get; private set; }

	[JsonProperty("nodeType")]
	public int NodeType { get; private set; }

	[JsonProperty("eventId")]
	public int EventId { get; private set; }

	[JsonProperty("eventKind")]
	public int EventKind { get; private set; }

	[JsonProperty("nodeColor")]
	public int NodeColor { get; private set; }

	[JsonProperty("itemGet")]
	public object[] ItemGet { get; private set; }
	#endregion

	public TsunDbNodeInfo(int amountOfNodes)
	{
		AmountOfNodes = amountOfNodes;
	}

	#region internal methods
	/// <summary>
	/// Process next node data
	/// </summary>
	/// <param name="api_data"></param>
	internal void ProcessNext(dynamic api_data)
	{
		JsonObject jData = (JsonObject)api_data;

		this.NodeType = (int)api_data["api_color_no"];
		this.EventId = (int)api_data["api_event_id"]; ;
		this.EventKind = (int)api_data["api_event_kind"]; ;
		this.NodeColor = (int)api_data["api_color_no"]; ;

		if (jData.IsDefined("api_itemget"))
		{
			if (api_data["api_itemget"].IsArray)
			{
				this.ItemGet = (object[])api_data["api_itemget"];
			}
			else
			{
				// --- On 6-3 api_itemget is an object and not an array
				this.ItemGet = new object[] { (object)api_data["api_itemget"] };
			}
		}
		else
		{
			this.ItemGet = new object[0];
		}

	}
	#endregion
}
