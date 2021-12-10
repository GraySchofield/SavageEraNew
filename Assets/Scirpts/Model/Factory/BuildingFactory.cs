using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

public class BuildingFactory: Factory
{
	protected static Dictionary<string, Building> buildings;


	private static Dictionary<string, Building> Init(){
		TextAsset resource = (TextAsset)Resources.Load("Config/Building");
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);

		Dictionary<string, Building> buildings = new Dictionary<string, Building> ();

		XmlNodeList buildingXMLs = settings.SelectNodes ("/Buildings/Building");
		foreach (XmlNode buildingNode in buildingXMLs) {
			string clazz = buildingNode.Attributes["class"].Value;
			string type = buildingNode.Attributes["type"].Value;
			Building building = null;
			if (clazz == "House") {
				building = BuildHouse (type, buildingNode);
			} else if (clazz == "ProductionConstruction") {
				building = BuildProductionConstruction (type, buildingNode);
			} else if (clazz == "AcademicConstruction") {
				building = BuildAcademicConstruction(type, buildingNode);
			} else if (clazz == "TechnologyConstruction") {
				building = BuildTechnologyConstruction(type, buildingNode);
			}

			if (buildingNode ["SpawnIgnore"] != null) {
				bool spawnIgnore = bool.Parse(buildingNode ["SpawnIgnore"].InnerText);
				building.SpawnIgnore = spawnIgnore;
			}
			buildings[type] = building;
		}
        return buildings;
	}
	
	public static void Reload(){
		buildings = Init();
	}

	public static House BuildHouse(string type, XmlNode bldSetting){
		if (bldSetting["Capacity"] == null) {
			Debug.LogError("Type: "+type+" Capacity is null.");
		}
		int capacity = int.Parse(bldSetting["Capacity"].InnerText);
		House house = new House(type, capacity);

		return house;
	}

	public static ProductionConstruction BuildProductionConstruction(string type, XmlNode bldSetting){
		if (bldSetting ["Occupation"] == null) {
			Debug.LogError("Type: "+type+" Occupation is null.");
		}
		string occupation = bldSetting ["Occupation"].InnerText;

		XmlNodeList productNodes = bldSetting.SelectNodes ("./Produce");
		List<StackableItem> produces = BuildStackableItemList (productNodes);

		XmlNodeList requireItemNodes = bldSetting.SelectNodes ("./Require");
		List<StackableItem> requireItems = BuildStackableItemList (requireItemNodes);

		ProductionConstruction pc = new ProductionConstruction (type, occupation, requireItems, produces);

		return pc;
	}

	public static AcademicConstruction BuildAcademicConstruction(string type, XmlNode bldSetting){
		AcademicConstruction ac = new AcademicConstruction (type);

		return ac;
	}

	public static TechnologyConstruction BuildTechnologyConstruction(string type, XmlNode bldSetting){
		TechnologyConstruction tc = new TechnologyConstruction (type);

		return tc;
	}

	// build from a xml node in the Event.xml
	// <Require/Product class="theClass">theType</...>
	public static Building BuildFromXMLNode(XmlNode node){
		if (node == null) {
			Debug.LogError("node is null.");
		}

		if (node.Attributes ["type"] == null) {
			Debug.LogError("type is null.");
		}
		string type = node.Attributes ["type"].Value;

		Building building = buildings[type].Clone();

		if (node ["SpawnIgnore"] != null) {
			bool spawnIgnore = bool.Parse(node ["SpawnIgnore"].InnerText);
			building.SpawnIgnore = spawnIgnore;
		}

		return building;
	}

	public static Building Get(string type){
        if(buildings != null)
        {
            return buildings[type].Clone();
        }
        else
        {
            Debug.LogError("building factory not initialized!");
            return null;
        }
	}
}