using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

[Serializable]
public class ResourceInjector
{
	// Tool -> Resource spawn events that require the tool 
	private Dictionary<string, List<string>> tool_to_resources = new Dictionary<string, List<string>> ();
	
	/// <summary>
	/// Equip a tool will generate certain spawn events
	/// </summary>
	/// <param name="type">Type.</param>
	public void ToolEquipped(string type){
		// no work need this tool, then return.
		if (!tool_to_resources.ContainsKey (type)) {
			return;
		}
		
		List<string> resourceSpawnEvents = tool_to_resources[type];
		
		foreach (string spawn in resourceSpawnEvents){
			IEvent e = EventFactory.Get(spawn);
			// spawn event will be checked every seconds
			Game.Current.EventEngine.AddEvent(Config.SpawnEventTriggerType, e);
		}
	}
	
	/// <summary>
	/// Remove a tool requires remove certain events and UI.
	/// </summary>
	/// <param name="type">Type.</param>
	public void ToolRemoved(string type){
		// no work need this tool, then return.
		if (!tool_to_resources.ContainsKey (type)) {
			return;
		}
		
		//remove spawn events and works
		List<string> resourceSpawnEvents = tool_to_resources [type];
		foreach (string spawn in resourceSpawnEvents) {
			// remove the button
			Game.Current.ActionEngine.RemoveWork (spawn);
			// remove the spawning event.
			Game.Current.EventEngine.RemoveEvent (spawn);
		}
	}
	
	// Initial the injector, basically loading the XML files.
	public void Run(){	
		// get the game obejct, so this function should be called before the game is constructed.
		Game g = Game.Current;

		string configFile = Config.IsDebugMode ? "Config/EventDebug" : "Config/Event";
		TextAsset eventResource = (TextAsset)Resources.Load(configFile);
		XmlDocument eventSettings = new XmlDocument ();
		eventSettings.LoadXml (eventResource.text);

		TextAsset itemResource = (TextAsset)Resources.Load("Config/Item");
		XmlDocument itemSettings = new XmlDocument ();
		itemSettings.LoadXml (itemResource.text);

		// clear all dictionary.
		Clear ();
		
		//handle no require event
		XmlNodeList eventNodes = eventSettings.SelectNodes ("/Events/Event[@class='spawn_resource_event']");
		foreach (XmlNode eventNode in eventNodes) {
			string type = eventNode.Attributes ["type"].Value;
			string clazz = eventNode.Attributes ["class"].Value;
			if(clazz == null){
				Debug.LogError("class is empty. for XML: "+eventNode.OuterXml);
			}
			XmlNodeList requireNodes = eventNode.SelectNodes("./Require");
			bool noRequire = true;
			if(requireNodes != null){
				foreach(XmlNode requireNode in requireNodes){
					if(requireNode["SpawnIgnore"] != null && bool.Parse(requireNode["SpawnIgnore"].InnerText) == true) continue;
					if(requireNode.Attributes["category"].Value == "Item"){
						string itemType = requireNode.Attributes["type"].Value;
						XmlNode itemNode = itemSettings.SelectSingleNode ("/Items/Item[@type='" + itemType + "']");
						if(itemNode != null){
							if(itemNode["SpawnIgnore"] != null && bool.Parse(itemNode["SpawnIgnore"].InnerText) == true) continue;
						}
					}
					noRequire = false;
					break;
				}
			}
			if(noRequire && Game.Current.Recorder.Get(type) == null) g.EventEngine.AddEvent (Config.SpawnEventTriggerType, EventFactory.Get (type));
		}
		
		//handle spawn event that requires tool
		XmlNodeList spawns = eventSettings.SelectNodes ("/Events/Event[Require/@class='Tool' and @class='spawn_resource_event']");
		foreach (XmlNode spawn in spawns) {
			string type = spawn.Attributes ["type"].Value;
			XmlNode toolNode = spawn.SelectSingleNode ("./Require[@class='Tool']");
			string tool = toolNode.Attributes ["type"].Value;
			if (tool_to_resources.ContainsKey (tool)) {
				tool_to_resources [tool].Add (type);
			} else {
				List<string> typeList = new List<string> ();
				typeList.Add (type);
				tool_to_resources [tool] = typeList;
			}
		}
	}
	
	private void Clear(){
		// Tool -> spawn events that require the tool 
		tool_to_resources.Clear();
	}
}
