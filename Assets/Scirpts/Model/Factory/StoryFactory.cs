using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;
using System.Reflection;

public class StoryFactory: Factory
{
	protected static Dictionary<string, Story> stories ;
	
	private static Dictionary<string, Story> Init(){
		TextAsset resource = (TextAsset)Resources.Load("Config/Story");
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, Story> stories = new Dictionary<string, Story> ();
		
		XmlNodeList storyXMLs = settings.SelectNodes ("/Stories/Story");
		foreach (XmlNode storyNode in storyXMLs) {
			string type = storyNode.Attributes["type"].Value;
			Story story = Build(type, storyNode);
			
			stories[type] = story;
		}
		
		return stories;
	}
	
	public static void Reload(){
		stories = Init();
	}

	public static Story Build(string type, XmlNode storySetting){	
		List<Option> options = new List<Option> ();
		//Debug.Log ("Story type :" + type);
		XmlNodeList optionNodes = storySetting.SelectNodes ("./Option");
       
		foreach (XmlNode optionNode in optionNodes) {
			List<Consequence> conseqs = new List<Consequence>();
			XmlNodeList conseqNodes = optionNode.SelectNodes("./Consequence");
			foreach(XmlNode conseqNode in conseqNodes){
				Consequence conseq = ConsequenceFactory.BuildFromXMLNode(conseqNode);
				conseqs.Add(conseq);
			}

			Option option = new Option(optionNode.Attributes["type"].Value, conseqs);
			options.Add(option);
			XmlNode optionConfigsNode = optionNode.SelectSingleNode("./Configs");
			if(optionConfigsNode != null){
				foreach(XmlNode configNode in optionConfigsNode.ChildNodes){
					string name = configNode.Name;
					string value = configNode.InnerText;
					Type optionType = typeof(Option);
					PropertyInfo info = optionType.GetProperty(name, BindingFlags.NonPublic|BindingFlags.Public| BindingFlags.Instance);
					info.SetValue(option, 
					              Convert.ChangeType(value, info.PropertyType), 
					              null);
				}
			}
		}

		Story story = new Story (type, options);
        if(storySetting["NeedPop"] != null)
        {
            //need to pop the story anyway
            story.needPop = true;
        }
		return story;
	}

	// build from a xml node in the Event.xml
	// <Story>theType</...>
	public static Story BuildFromXMLNode(XmlNode node){
		if (node == null) {
			Debug.LogError("node is found.");
		}

		if (node.Attributes ["type"] == null) {
			Debug.LogError (node.OuterXml);
			return null;
		}
		string type = node.Attributes["type"].Value;
		Story story = stories [type].Clone();

		return story;
	}

	public static Story Get(string type){
		return stories [type].Clone ();
	}
}

