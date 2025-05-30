﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Resource.Record;

/// <summary>
/// 建造のレコードです。
/// </summary>
[DebuggerDisplay("{Record.Count} Records")]
public class ConstructionRecord : RecordBase
{

	[DebuggerDisplay("[{ShipID}] : {ShipName}")]
	public sealed class ConstructionElement : RecordElementBase
	{

		/// <summary>
		/// 建造した艦のID
		/// </summary>
		public int ShipID { get; set; }

		/// <summary>
		/// 建造した艦の名前
		/// </summary>
		public string ShipName { get; set; }

		/// <summary>
		/// 建造日時
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// 投入燃料
		/// </summary>
		public int Fuel { get; set; }

		/// <summary>
		/// 投入弾薬
		/// </summary>
		public int Ammo { get; set; }

		/// <summary>
		/// 投入鋼材
		/// </summary>
		public int Steel { get; set; }

		/// <summary>
		/// 投入ボーキサイト
		/// </summary>
		public int Bauxite { get; set; }

		/// <summary>
		/// 投入開発資材
		/// </summary>
		public int DevelopmentMaterial { get; set; }

		/// <summary>
		/// 大型艦建造かのフラグ
		/// </summary>
		public bool IsLargeDock => Fuel >= 1000;

		/// <summary>
		/// 空きドック数
		/// </summary>
		public int EmptyDockAmount { get; set; }

		/// <summary>
		/// 旗艦の艦船ID
		/// </summary>
		public int FlagshipID { get; set; }

		/// <summary>
		/// 旗艦の艦名
		/// </summary>
		public string FlagshipName { get; set; }

		/// <summary>
		/// 司令部Lv.
		/// </summary>
		public int HQLevel { get; set; }



		public ConstructionElement()
		{
			ShipID = -1;
			Date = DateTime.Now;
		}

		public ConstructionElement(string line)
			: this()
		{
			LoadLine(line);
		}

		public ConstructionElement(int shipID, int fuel, int ammo, int steel, int bauxite, int developmentMaterial, int emptyDock, int flagshipID, int hqLevel)
		{
			var ship = KCDatabase.Instance.MasterShips[shipID];
			var flagship = KCDatabase.Instance.MasterShips[flagshipID];
			ShipID = shipID;
			ShipName = ship?.NameWithClass ?? "???";
			Date = DateTime.Now;
			Fuel = fuel;
			Ammo = ammo;
			Steel = steel;
			Bauxite = bauxite;
			DevelopmentMaterial = developmentMaterial;
			EmptyDockAmount = emptyDock;
			FlagshipID = flagshipID;
			FlagshipName = flagship?.NameWithClass ?? "???";
			HQLevel = hqLevel;
		}


		public override void LoadLine(string line)
		{

			string[] elem = Utility.Storage.CsvHelper.ParseCsvLine(line).ToArray();
			if (elem.Length < 13)
				throw new ArgumentException("要素数が少なすぎます。");

			ShipID = int.Parse(elem[0]);
			ShipName = elem[1];
			Date = DateTimeHelper.CSVStringToTime(elem[2]);
			Fuel = int.Parse(elem[3]);
			Ammo = int.Parse(elem[4]);
			Steel = int.Parse(elem[5]);
			Bauxite = int.Parse(elem[6]);
			DevelopmentMaterial = int.Parse(elem[7]);
			//IsLargeDock=elem[8]は読み飛ばす
			EmptyDockAmount = int.Parse(elem[9]);
			FlagshipID = int.Parse(elem[10]);
			FlagshipName = elem[11];
			HQLevel = int.Parse(elem[12]);

		}

		public override string SaveLine()
		{
			return string.Join(",",
				ShipID,
				Utility.Storage.CsvHelper.EscapeCsvCell(ShipName),
				DateTimeHelper.TimeToCSVString(Date),
				Fuel,
				Ammo,
				Steel,
				Bauxite,
				DevelopmentMaterial,
				IsLargeDock ? 1 : 0,
				EmptyDockAmount,
				FlagshipID,
				Utility.Storage.CsvHelper.EscapeCsvCell(FlagshipName),
				HQLevel);
		}
	}



	public List<ConstructionElement> Record { get; private set; }
	private int ConstructingDockID;

	private int LastSavedCount;


	public ConstructionRecord()
		: base()
	{
		Record = new List<ConstructionElement>();
		ConstructingDockID = -1;
	}

	public override void RegisterEvents()
	{
		APIObserver ao = APIObserver.Instance;

		ao.ApiReqKousyou_CreateShip.RequestReceived += ConstructionStart;
		ao.ApiGetMember_KDock.ResponseReceived += ConstructionEnd;
	}


	public ConstructionElement this[int i]
	{
		get { return Record[i]; }
		set { Record[i] = value; }
	}



	void ConstructionStart(string apiname, dynamic data)
	{

		ConstructingDockID = int.Parse(data["api_kdock_id"]);

	}

	void ConstructionEnd(string apiname, dynamic data)
	{

		if (ConstructingDockID == -1) return;

		ArsenalData a = KCDatabase.Instance.Arsenals[ConstructingDockID];
		int emptyDock = KCDatabase.Instance.Arsenals.Values.Count(c => c.State == 0);
		IShipData flagship = KCDatabase.Instance.Fleet[1].MembersInstance[0];

		Record.Add(new ConstructionElement(a.ShipID, a.Fuel, a.Ammo, a.Steel, a.Bauxite, a.DevelopmentMaterial,
			emptyDock, flagship.ShipID, KCDatabase.Instance.Admiral.Level));

		ConstructingDockID = -1;
	}



	protected override void LoadLine(string line)
	{
		Record.Add(new ConstructionElement(line));
	}

	protected override string SaveLinesAll()
	{
		var sb = new StringBuilder();
		foreach (var elem in Record.OrderBy(r => r.Date))
		{
			sb.AppendLine(elem.SaveLine());
		}
		return sb.ToString();
	}

	protected override string SaveLinesPartial()
	{
		var sb = new StringBuilder();
		foreach (var elem in Record.Skip(LastSavedCount).OrderBy(r => r.Date))
		{
			sb.AppendLine(elem.SaveLine());
		}
		return sb.ToString();
	}

	protected override void UpdateLastSavedIndex()
	{
		LastSavedCount = Record.Count;
	}

	public override bool NeedToSave => LastSavedCount < Record.Count;

	public override bool SupportsPartialSave => true;

	protected override void ClearRecord()
	{
		Record.Clear();
		LastSavedCount = 0;
	}


	public override string RecordHeader => "艦船ID,艦船名,建造日時,燃料,弾薬,鋼材,ボーキ,開発資材,大型建造,空ドック,旗艦ID,旗艦名,司令部Lv";

	public override string FileName => "ConstructionRecord.csv";
}
