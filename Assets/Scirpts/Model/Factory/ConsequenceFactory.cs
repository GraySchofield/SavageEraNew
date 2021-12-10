using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;
using System.Reflection;

public class ConsequenceFactory: Factory
{
	protected static Dictionary<string, Consequence> consequences ;
	
	private static Dictionary<string, Consequence> Init(){
		TextAsset resource = (TextAsset)Resources.Load("Config/Consequence");
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, Consequence> consequences = new Dictionary<string, Consequence> ();
		
		XmlNodeList consequenceXMLs = settings.SelectNodes ("/Consequences/Consequence");
		foreach (XmlNode consequenceNode in consequenceXMLs) {
			string type = consequenceNode.Attributes["type"].Value;
			Consequence consequence = null;
			consequence = Build(type, consequenceNode);
			consequences[type] = consequence;
		}
		
		return consequences;
	}
	
	public static void Reload(){
		consequences = Init();
	}

	public static Consequence Build(string type, XmlNode conseqSetting){
		XmlNodeList requireItemNodes = conseqSetting.SelectNodes ("./Require[@category='Item']");
		List<Item> requireItems = BuildItemList (requireItemNodes);
	
		XmlNodeList requireBuildingNodes = conseqSetting.SelectNodes ("./Require[@category='Building']");
		List<Building> requireBuildings = BuildBuildingList (requireBuildingNodes);

		XmlNodeList producedItemNodes = conseqSetting.SelectNodes ("./Produce[@category='Item']");
		List<Item> producedItems = BuildItemList (producedItemNodes);

		XmlNodeList producedBuildingNodes = conseqSetting.SelectNodes ("./Produce[@category='Building']");
		List<Building> producedBuildings = BuildBuildingList (producedBuildingNodes);



		XmlNodeList resultNodes = conseqSetting.SelectNodes ("./Result");
		List<string> resultXMLs = new List<string> ();
		foreach (XmlNode resultNode in resultNodes) {
			resultXMLs.Add(resultNode.OuterXml);
		}

		Consequence c =  new Consequence (type, requireItems, requireBuildings, producedItems, producedBuildings, resultXMLs);

		Type conseqType = typeof(Consequence);

		foreach (XmlNode xParam in conseqSetting.ChildNodes) {
			if(IsXParam(xParam.Name)){
				FieldInfo info = conseqType.GetField (xParam.Name, BindingFlags.NonPublic|BindingFlags.Public| BindingFlags.Instance);
				Type t = info.FieldType;
				if(Nullable.GetUnderlyingType(t) != null){
					t = Nullable.GetUnderlyingType(t);
				}
				if(t.IsEnum){
					info.SetValue(c, Convert.ChangeType(Enum.Parse(t, xParam.InnerText), t));
				}else{
					info.SetValue (c, Convert.ChangeType (xParam.InnerText, t));
				}
			}
		}

		return c;
	}

	public static Consequence BuildFromXMLNode(XmlNode node){
		if (node == null) {
			Debug.LogError("node is null.");
		}

		if (node.Attributes["type"] == null) {
			Debug.LogError("type is null.");
		}
		string type = node.Attributes["type"].Value;

		Consequence conseq = consequences [type].Clone();

		XmlNode conseqConfigsNode = node.SelectSingleNode("./Configs");
		if (conseqConfigsNode != null) {
			foreach (XmlNode configNode in conseqConfigsNode.ChildNodes) {
				string name = configNode.Name;
				string value = configNode.InnerText;
				Type conseqType = typeof(Consequence);
				PropertyInfo info = conseqType.GetProperty (name, BindingFlags.NonPublic|BindingFlags.Public| BindingFlags.Instance);
				info.SetValue (conseq, 
			              Convert.ChangeType (value, info.PropertyType), 
			              null);
			}
		}
		return conseq;
	}

	public static Consequence Get(string type){
		return consequences [type].Clone();
	}
}

