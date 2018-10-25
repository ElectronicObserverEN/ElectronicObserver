using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Codeplex.Data;

namespace ElectronicObserver.Utility
{
	public class DynamicTranslator
    {
        private XDocument _shipsXml;
        private XDocument _shipTypesXml;
        private XDocument _equipmentXml;
        private XDocument _equipTypesXml;
        private XDocument _operationsXml;
        private XDocument _questsXml;
        private XDocument _expeditionsXml;
	    private readonly string _folder = SoftwareUpdater.TranslationFolder;

		internal DynamicTranslator()
		{
			CheckUpdate();
		    //LoadFile();

		}

        private void LoadFile()
        {
            try
            {
                if (File.Exists(_folder + "\\Ships.xml")) _shipsXml = XDocument.Load(_folder + "\\Ships.xml");
                if (File.Exists(_folder + "\\ShipTypes.xml")) _shipTypesXml = XDocument.Load(_folder + "\\ShipTypes.xml");
                if (File.Exists(_folder + "\\Equipment.xml")) _equipmentXml = XDocument.Load(_folder + "\\Equipment.xml");
                if (File.Exists(_folder + "\\EquipmentTypes.xml")) _equipTypesXml = XDocument.Load(_folder + "\\EquipmentTypes.xml");
                if (File.Exists(_folder + "\\Operations.xml")) _operationsXml = XDocument.Load(_folder + "\\Operations.xml");
                if (File.Exists(_folder + "\\Quests.xml")) _questsXml = XDocument.Load(_folder + "\\Quests.xml");
                if (File.Exists(_folder + "\\Expeditions.xml")) _expeditionsXml = XDocument.Load(_folder + "\\Expeditions.xml");
            }
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
            }
        }

        private void LoadFile(TranslationFile filename)
        {
            try
            {
                switch (filename)
                {
                    case TranslationFile.Equipment:
                        _equipmentXml = XDocument.Load(_folder + "\\Equipment.xml");
                        break;
                    case TranslationFile.EquipmentTypes:
                        _equipTypesXml = XDocument.Load(_folder + "\\EquipmentTypes.xml");
                        break;
                    case TranslationFile.Expeditions:
                        _expeditionsXml = XDocument.Load(_folder + "\\Expeditions.xml");
                        break;
                    case TranslationFile.Operations:
                        _operationsXml = XDocument.Load(_folder + "\\Operations.xml");
                        break;
                    case TranslationFile.Quests:
                        _questsXml = XDocument.Load(_folder + "\\Quests.xml");
                        break;
                    case TranslationFile.Ships:
                        _shipsXml = XDocument.Load(_folder + "\\Ships.xml");
                        break;
                    case TranslationFile.ShipTypes:
                        _shipTypesXml = XDocument.Load(_folder + "\\ShipTypes.xml");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Add(3, "Could not load translation file: " + ex.Message);
            }
        }

        private void CheckUpdate()
	    {
			if (!Directory.Exists(_folder))
				Directory.CreateDirectory(_folder);

			SoftwareUpdater.CheckVersion();

            Parallel.ForEach((TranslationFile[])Enum.GetValues(typeof(TranslationFile)), filename =>
            {
                var current = SoftwareUpdater.CheckDataVersion(filename);
                var latest = current;

                if (filename == TranslationFile.Equipment) latest = SoftwareUpdater.EqVer;
                if (filename == TranslationFile.EquipmentTypes) latest = SoftwareUpdater.EqTypeVer;
                if (filename == TranslationFile.Expeditions) latest = SoftwareUpdater.ExpVer;
                if (filename == TranslationFile.Operations) latest = SoftwareUpdater.OpVer;
                if (filename == TranslationFile.Quests) latest = SoftwareUpdater.QuestVer;
                if (filename == TranslationFile.Ships) latest = SoftwareUpdater.ShipVer;
                if (filename == TranslationFile.ShipTypes) latest = SoftwareUpdater.ShipTypeVer;

                if (current != latest)
                    SoftwareUpdater.DownloadData(filename);
                LoadFile(filename);
            });
        }

        private IEnumerable<XElement> GetTranslationList(TranslationType type)
        {
            switch (type)
            {
                case TranslationType.Ships:
                    if (_shipsXml != null)
                        return _shipsXml.Descendants("Ship");
                    break;
                case TranslationType.ShipTypes:
                    if (_shipTypesXml != null)
                        return _shipTypesXml.Descendants("Type");
                    break;
                case TranslationType.Equipment:
                    if (_equipmentXml != null)
                        return _equipmentXml.Descendants("Item");
                    break;
				case TranslationType.EquipmentDesc:
					if (_equipmentXml != null)
						return _equipmentXml.Descendants("ItemDesc");
					break;
				case TranslationType.EquipmentType:
                    if (_equipTypesXml != null)
                        return _equipTypesXml.Descendants("Type");
                    break;
                case TranslationType.OperationMaps:
                    if (_operationsXml != null)
                        return _operationsXml.Descendants("Map");
                    break;
                case TranslationType.OperationSortie:
                    if (_operationsXml != null)
                        return _operationsXml.Descendants("Sortie");
                    break;
				case TranslationType.OperationMapNodes:
					if (_operationsXml != null)
						return _operationsXml.Descendants("MapNode");
					break;
				case TranslationType.Quests:
                case TranslationType.QuestTitle:
                case TranslationType.QuestDetail:
                    if (_questsXml != null)
                        return _questsXml.Descendants("Quest");
                    break;
                case TranslationType.Expeditions:
                case TranslationType.ExpeditionTitle:
                case TranslationType.ExpeditionDetail:
                    if (_expeditionsXml != null)
                        return _expeditionsXml.Descendants("Expedition");
                    break;
                default:
                    return null;
            }
            return null;
        }

        public string GetTranslation(string jpString, TranslationType type, int id = -1)
        {
			var translate = true;
			switch (type)
			{
				case TranslationType.Ships: translate = !Configuration.Config.UI.JapaneseShipName; break;
				case TranslationType.ShipTypes: translate = !Configuration.Config.UI.JapaneseShipType; break;
				case TranslationType.Equipment: translate = !Configuration.Config.UI.JapaneseEquipmentName; break;
				case TranslationType.EquipmentDesc: translate = !Configuration.Config.UI.JapaneseEquipmentName; break;
				case TranslationType.EquipmentType: translate = !Configuration.Config.UI.JapaneseEquipmentType; break;
			}
	        if (!translate) return jpString;
	        try
	        {
		        var translationList = GetTranslationList(type);
		        if (translationList == null) return jpString;
		        var jpChildElement = "JP-Name";
		        var trChildElement = "TR-Name";
		        if (type == TranslationType.QuestDetail || type == TranslationType.ExpeditionDetail)
		        {
			        jpChildElement = "JP-Detail";
			        trChildElement = "TR-Detail";
		        }
		        var translated = jpString;
		        if (GetTranslation(jpString, translationList, jpChildElement, trChildElement, id, ref translated))
			        return translated;
	        }
	        catch (Exception e)
	        {
		        Logger.Add(3, "Can't output translation: " + e.Message);
	        }
	        return jpString;
        }

	    public string GetMapNodes(int worldId, int areaId, int nodeId)
		{
			var filepath = _folder + @"\nodes.json";
		    var id = nodeId.ToString();
			if (Configuration.Config.UI.UseOriginalNodeId)
				return id;
			using (var sr = new StreamReader(filepath))
		    {
		        var json = DynamicJson.Parse(sr.ReadToEnd());
		        var worldKey = string.Concat(worldId.ToString("D2"), areaId.ToString());
		        var nodeKey = nodeId.ToString("D2");
		        foreach (KeyValuePair<string, object> world in json)
		        {
		            if (world.Key.Remove(0, 1).PadLeft(3, '0') != worldKey) continue;
		            var nodes = DynamicJson.Parse(world.Value.ToString());
		            foreach (KeyValuePair<string, object> node in nodes)
		            {
		                if (node.Key.Remove(0, 1).PadLeft(2, '0') != nodeKey) continue;
		                id = DynamicJson.Parse(node.Value.ToString())[1];
		            }
		        }
		    }
		    return id;
        }

		private bool GetTranslation(string jpString, IEnumerable<XElement> translationList, string jpChildElement, string trChildElement, int id, ref string translate)
        {
            var foundTranslation = translationList.Where(el =>
            {
                try
                {
                    if (el.Element(jpChildElement).Value.Equals(jpString)) return true;
                    if (el.Attribute("mode") != null)
                    {
                        if (el.Attribute("mode").Value.Equals("suffix"))
                        {
                            var sl = el.Element(jpChildElement).Value.Length;
                            if (jpString.Length > sl)
                            {
                                if (el.Element(jpChildElement).Value.Equals(jpString.Substring(jpString.Length - sl))) return true;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }
                return false;
            }
            );

            var foundWrongId = false;
	        foreach (var el in foundTranslation)
            {
                if (el.Attribute("mode") != null && !el.Attribute("mode").Value.Equals("normal"))
                {
                    if (el.Attribute("mode").Value.Equals("suffix")) {
                        try
                        {
                            var t = jpString.Substring(0, jpString.Length - el.Element(jpChildElement).Value.Length);
                            if (GetTranslation(t, translationList, jpChildElement, trChildElement, -1, ref t))
                            {
                                if (el.Attribute("suffixType") != null && el.Attribute("suffixType").Value.Equals("pre")) translate = el.Element(trChildElement).Value + t;
                                else translate = t + el.Element(trChildElement).Value;
                                        return true;
                            }
                        }
                        catch (NullReferenceException)
                        {

                        }
                    }
                    continue;
                }

                try
                {
                    if(id >= 0)
                    {
                        if(!int.TryParse(el.Element("ID").Value, out _))
                        {
                            foundWrongId = true;
                            translate = el.Element(trChildElement).Value;
                        }
                        else
                        {
	                        if (id < 0 || el.Element("ID") == null || Convert.ToInt32(el.Element("ID").Value) != id)
		                        continue;
	                        translate = el.Element(trChildElement).Value;
	                        return true;
                        }
                    }
                    else
                    {
                        translate = el.Element(trChildElement).Value;
                        return true;
                    }
                }
                catch (NullReferenceException)
                {

                }
            }

            return foundWrongId;
        }

	}

	public enum TranslationType
    {
        App,
        Equipment,
		EquipmentDesc,
		EquipmentType,
        Operations,
        Quests,
        Ships,
        ShipTypes,
        OperationMaps,
        OperationSortie,
		OperationMapNodes,
		QuestDetail,
        QuestTitle,
        Expeditions,
        ExpeditionDetail,
        ExpeditionTitle
    }

	internal enum TranslationFile
	{
		Ships,
		ShipTypes,
		Equipment,
		EquipmentTypes,
		Operations,
		Quests,
		Expeditions
	}
}
