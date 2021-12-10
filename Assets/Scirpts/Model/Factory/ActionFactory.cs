using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

public class ActionFactory: Factory
{
	protected static Dictionary<string, Action> actions ;
	
	private static Dictionary<string, Action> Init(){
		string configFile = Config.IsDebugMode ? "Config/ActionDebug" : "Config/Action";
		TextAsset resource = (TextAsset)Resources.Load(configFile);
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, Action> actions = new Dictionary<string, Action> ();
		
		XmlNodeList actionXMLs = settings.SelectNodes ("/Actions/Action");
		foreach (XmlNode actionNode in actionXMLs) {
			string clazz = actionNode.Attributes["class"].Value;
			string type = actionNode.Attributes["type"].Value;
			Action action = null;
			if (clazz == "MakeWeaponAction") {
				action = BuildMakeWeaponAction (type, actionNode);
			} else if (clazz == "MakeToolAction") {
				action = BuildMakeToolAction (type, actionNode);
			} else if (clazz == "MakeAccessoryAction") {
				action = BuildMakeAccessoryAction(type, actionNode);
			} else if (clazz == "MakeArmorAction") {
				action = BuildMakeArmorAction(type, actionNode);
			} else if (clazz == "LearnSkillAction") {
				action = BuildLearnSkillAction(type, actionNode);
			} else if (clazz == "BuildConstructionAction"){
				action = BuildBuildConstructionAction(type, actionNode);
			}

			actions[type] = action;
		}
		
		return actions;
	}
	
	public static void Reload(){
		actions = Init();
	}

	public static Action BuildMakeToolAction(string type, XmlNode actionSetting){
		XmlNodeList requireNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode ("./Produce");
		Tool tool = ItemFactory.BuildFromXMLNode (produceNode) as Tool;
		
		return new MakeToolAction (type, requireItems, requireBuildings, tool);
	}
	
	public static Action BuildMakeWeaponAction(string type, XmlNode actionSetting){
		XmlNodeList requireNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode ("./Produce");
		Weapon weapon = ItemFactory.BuildFromXMLNode (produceNode) as Weapon;
		
		return new MakeWeaponAction (type, requireItems, requireBuildings, weapon);
	}
	
	public static Action BuildMakeAccessoryAction(string type, XmlNode actionSetting){
		XmlNodeList requireNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode ("./Produce");
		Accessory accessory = ItemFactory.BuildFromXMLNode (produceNode) as Accessory;
		
		return new MakeAccessoryAction (type, requireItems, requireBuildings, accessory);
	}
	
	public static Action BuildMakeArmorAction(string type, XmlNode actionSetting){
		XmlNodeList requireNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode ("./Produce");
		Armor armor = ItemFactory.BuildFromXMLNode (produceNode) as Armor;
		
		return new MakeArmorAction (type, requireItems, requireBuildings, armor);
	}
	
	public static Action BuildLearnSkillAction(string type, XmlNode actionSetting){
		XmlNodeList requireNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode("./Produce");
		Skill skill = SkillBuilder.BuildSkill(produceNode.Attributes ["type"].Value);
		
		return new LearnSkillAction (type, requireItems, requireBuildings, skill);
	}
	
	public static Action BuildBuildConstructionAction(string type, XmlNode actionSetting){
		XmlNodeList requireItemNodes = actionSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireItemNodes);
		
		XmlNodeList requireBuildingNodes = actionSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);
		
		XmlNode produceNode = actionSetting.SelectSingleNode ("./Produce");
		Building building = BuildingFactory.BuildFromXMLNode (produceNode) as Building;
		
		return new BuildConstructionAction (type, requireItems, requireBuildings, building);
	}
	
	public static Action Get(string type){
		return actions [type].Clone ();
	}
}
