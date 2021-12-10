using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

[Serializable]
public class ActionInjector: IRecorderObserver
{
	// Require -> Subjects
	private Dictionary<string, List<string>> require_to_subjects = new Dictionary<string, List<string>> ();
	// Subject -> Requires
	private Dictionary<string, List<string>> subject_to_requires = new Dictionary<string, List<string>> ();

	public void Notify(Recorder recorder, string type){
		Recorder.Record r = recorder.Get (type);
		int count = r.Count;
		// Only care about type that first occurs.
		if (count == 1) {
			UnlockActionsWithRequire (type, recorder);
		}
	}

	// check if all the requires has been unlocked for an item/building.
	private bool IsUnlockable(string potential_unlockable, Recorder recorder){
		List<string> requires = subject_to_requires [potential_unlockable];
		foreach (string require in requires) {
			if (recorder.Get (require) == null)
				return false;
		}
		return true;
	}
	
	private void UnlockActionsWithRequire(string require, Recorder recorder){
		if (require_to_subjects.ContainsKey (require)) {
 
            List<string> potential_unlockables = require_to_subjects [require];
			foreach (string potential_unlockable in potential_unlockables) {
				if (IsUnlockable (potential_unlockable, recorder)) {
                   
                       Game.Current.ActionEngine.AddAction(ActionFactory.Get(potential_unlockable));
                    
                }
         
			}
		}
	}

	private void UnlockAction(string type, Recorder recorder){
		if (IsUnlockable (type, recorder)) {
			Game.Current.ActionEngine.AddAction (ActionFactory.Get (type));
		}
	}
	
	// Initial the injector, basically loading the XML files.
	// This function need to be reran when doing upgrade.
	public void Run(){	
		// get the game obejct, so this function should be called before the game is constructed.
		Game g = Game.Current;

		string configFile = Config.IsDebugMode ? "Config/ActionDebug" : "Config/Action";
		TextAsset actionResource = (TextAsset)Resources.Load(configFile);
		XmlDocument actionSettings = new XmlDocument ();
		actionSettings.LoadXml (actionResource.text);

		TextAsset itemResource = (TextAsset)Resources.Load("Config/Item");
		XmlDocument itemSettings = new XmlDocument ();
		itemSettings.LoadXml (itemResource.text);
		
		// clear all dictionary.
		Clear ();
		
		//handle no require action
		XmlNodeList actionNodes = actionSettings.SelectNodes ("/Actions/Action");
		foreach (XmlNode actionNode in actionNodes) {
			string type = actionNode.Attributes ["type"].Value;
			string clazz = actionNode.Attributes ["class"].Value;
			if(clazz == null){
				Debug.LogError("class is empty. for XML: "+actionNode.OuterXml);
			}
			XmlNodeList requireNodes = actionNode.SelectNodes("./Require");
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
			if(noRequire && g.Recorder.Get(type) == null) g.ActionEngine.AddAction (ActionFactory.Get(type));
		}
		
		//handle make action that requires item
		XmlNodeList action_subjects = actionSettings.SelectNodes ("/Actions/Action[Require]");
		UpdateRequires (action_subjects, itemSettings);
	}
	
	private void UpdateRequires(XmlNodeList nodeList, XmlDocument itemSettings){
		foreach (XmlNode node in nodeList) {
			string subject = node.Attributes["type"].Value;
			XmlNodeList requireNodes = node.SelectNodes("./Require");
			foreach (XmlNode requireNode in requireNodes){
				// If the require node should be ignored by action, don't put it as a require for injection.
				if(requireNode["SpawnIgnore"] != null && bool.Parse(requireNode["SpawnIgnore"].InnerText) == true) continue;
				if(requireNode.Attributes["category"].Value == "Item"){
					string itemType = requireNode.Attributes["type"].Value;
					XmlNode itemNode = itemSettings.SelectSingleNode ("/Items/Item[@type='" + itemType + "']");
					if(itemNode != null){
						if(itemNode["SpawnIgnore"] != null && bool.Parse(itemNode["SpawnIgnore"].InnerText) == true) continue;
					}
				}
				
				string require = requireNode.Attributes["type"].Value;
				if(subject_to_requires.ContainsKey(subject)){
					subject_to_requires[subject].Add(require);
				}else{
					List<string> requireList = new List<string>();
					requireList.Add(require);
					subject_to_requires[subject] = requireList;
				}
				if(require_to_subjects.ContainsKey(require)){
					require_to_subjects[require].Add(subject);
				}else{
					List<string> constructionList = new List<string>();
					constructionList.Add(subject);
					require_to_subjects[require] = constructionList;
				}

                /*
                if (require.Equals("science_lvl_1"))
                {
                    string r = "";
                    for (int k = 0; k < require_to_subjects[require].Count; k++)
                    {
                        r += " " + require_to_subjects[require][k];
                    }
                    Debug.LogError(require + " unlocks : " + r);
                }
                */
			}

			// We need to add the action if it hasn't been added but all its requires are unlocked.
			if(Game.Current.Recorder.Get(subject) == null && subject_to_requires.ContainsKey(subject)){
				UnlockAction(subject, Game.Current.Recorder);
			}
		}
	}
	
	private void Clear(){
		// Item -> makes that require this item
		require_to_subjects.Clear();
		// make -> items that used to make this item
		subject_to_requires.Clear();
	}
	

}


