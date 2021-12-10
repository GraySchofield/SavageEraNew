using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Reflection;
using System;

public class EventFactory: Factory
{
	protected static Dictionary<string, IEvent> events;
	
	private static Dictionary<string, IEvent> Init(){
		string configFile = Config.IsDebugMode ? "Config/EventDebug" : "Config/Event";
		TextAsset resource = (TextAsset)Resources.Load(configFile);
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, IEvent> events = new Dictionary<string, IEvent> ();
		
		XmlNodeList eventXMLs = settings.SelectNodes ("/Events/Event");
		foreach (XmlNode eventNode in eventXMLs) {
			string clazz = eventNode.Attributes["class"].Value;
			string type = eventNode.Attributes["type"].Value;
			IEvent e = null;
			if (clazz == "spawn_resource_event") {
				e = BuildSpawnResourceEvent (type, eventNode);
			} else if (clazz == "spawn_story_event") {
				e = BuildSpawnStoryEvent (type, eventNode);
			}
			
			events[type] = e;
		}
		
		return events;
	}
	
	public static void Reload(){
		events = Init();
	}

	public static SpawnResourceEvent BuildSpawnResourceEvent(string type, XmlNode eventSetting){
		if (eventSetting == null) {
			Debug.LogError("Type: "+type+" cannot be found.");
		}

		XmlNodeList requireNodes = eventSetting.SelectNodes ("./Require");
		List<Item> requires = BuildItemList (requireNodes);

		XmlNodeList produceNodes = eventSetting.SelectNodes ("./Produce");
		List<Item> produces = BuildItemList (produceNodes);

		XmlNodeList conditionNodes = eventSetting.SelectNodes ("./Condition");
		List<string> conditionXMLs = new List<string> ();
		foreach (XmlNode conditionNode in conditionNodes) {
			conditionXMLs.Add(conditionNode.OuterXml);
		}

		SpawnResourceEvent spawn = new SpawnResourceEvent (type, requires, produces, conditionXMLs);

		//if (eventSetting ["CoolDown"] == null) {
		//	Debug.LogError("Type: "+type+" CoolDown is null.");
		//}
		//spawn.CoolDown = int.Parse (eventSetting ["CoolDown"].InnerText);
		//
		//if (eventSetting ["Probability"] == null) {
		//	Debug.LogError("Type: "+type+" Probability is null.");
		//}
		//spawn.Probability = float.Parse (eventSetting ["Probability"].InnerText);

		Type spawnResourceEventType = typeof(SpawnStoryEvent);

		foreach (XmlNode xParam in eventSetting.ChildNodes) {
			if(IsXParam(xParam.Name)){
				FieldInfo info = spawnResourceEventType.GetField (xParam.Name, BindingFlags.NonPublic|BindingFlags.Public| BindingFlags.Instance);
				Type t = info.FieldType;
				if(Nullable.GetUnderlyingType(t) != null){
					t = Nullable.GetUnderlyingType(t);
				}
				if(t.IsEnum){
					info.SetValue(spawn, Convert.ChangeType(Enum.Parse(t, xParam.InnerText), t));
				}else{
					info.SetValue (spawn, Convert.ChangeType (xParam.InnerText, t));
				}
			}
		}

		return spawn;
	}

	public static SpawnStoryEvent BuildSpawnStoryEvent(string type, XmlNode eventSetting){
		if (eventSetting == null) {
			Debug.LogError("Type: "+type+" cannot be found.");
		}

		XmlNodeList requireItemNodes = eventSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList(requireItemNodes);

		XmlNodeList requireBuildingNodes = eventSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList(requireBuildingNodes);

		XmlNodeList conditionNodes = eventSetting.SelectNodes ("./Condition");
		List<string> conditionXMLs = new List<string> ();
		foreach (XmlNode conditionNode in conditionNodes) {
			conditionXMLs.Add(conditionNode.OuterXml);
		}

		XmlNode storyNode = eventSetting.SelectSingleNode ("./Story");
		Story story = StoryFactory.BuildFromXMLNode (storyNode);
		
		SpawnStoryEvent spawn = new SpawnStoryEvent (type, requireItems, requireBuildings, conditionXMLs, story);

		Type spawnStoryEventType = typeof(SpawnStoryEvent);
		
		foreach (XmlNode xParam in eventSetting.ChildNodes) {
			if(IsXParam(xParam.Name)){
				FieldInfo info = spawnStoryEventType.GetField (xParam.Name, BindingFlags.NonPublic|BindingFlags.Public| BindingFlags.Instance);
				if (info == null){
					Debug.LogError(xParam.Name + " is not a field.");
					Debug.LogError(type);
				}
				Type t = info.FieldType;
				if(Nullable.GetUnderlyingType(t) != null){
					t = Nullable.GetUnderlyingType(t);
				}
				if(t.IsEnum){
					info.SetValue(spawn, Convert.ChangeType(Enum.Parse(t, xParam.InnerText), t));
				}else{
					info.SetValue (spawn, Convert.ChangeType (xParam.InnerText, t));
				}
			}
		}

		return spawn;
	}

	public static IEvent Get(string type){
		return events [type].Clone ();
	}
}

