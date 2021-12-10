using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

public class SkillFactory: Factory
{
	protected static Dictionary<string, Skill> skills ;
	
	private static Dictionary<string, Skill> Init(){
		TextAsset resource = (TextAsset)Resources.Load("Config/Skill");
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, Skill> skills = new Dictionary<string, Skill> ();
		
		XmlNodeList skillXMLs = settings.SelectNodes ("/Skills/Skill");
		foreach (XmlNode skillNode in skillXMLs) {
			string clazz = skillNode.Attributes["class"].Value;
			string type = skillNode.Attributes["type"].Value;
			Skill skill = null;
			if (clazz == "ArmorSkill") {
				skill = BuildArmorSkill (type, skillNode);
			} else if (clazz == "WeaponSkill") {
				skill = BuildWeaponSkill (type, skillNode);
			} 
			skills[type] = skill;
		}
		
		return skills;
	}
	
	public static void Reload(){
		skills = Init();
	}
	
	public static Skill BuildArmorSkill(string type, XmlNode skillSetting){
		float arg1 = 0;
		if (skillSetting["Arg1"] != null) {
			arg1 = float.Parse(skillSetting["Arg1"].InnerText);
		}
		float arg2 = 0;
		if (skillSetting["Arg2"] != null) {
			arg2 = float.Parse(skillSetting["Arg2"].InnerText);
		}
		float probability = 0;
		if (skillSetting["Probability"] != null) {
			probability = float.Parse(skillSetting["Probability"].InnerText);
		}
		Skill skill = new ArmorSkill(type, probability, arg1, arg2);
		
		return skill;
	}

	public static Skill BuildWeaponSkill(string type, XmlNode skillSetting){

        float range = 0.3f;
        if (skillSetting["Range"] != null)
        {
            range = float.Parse(skillSetting["Range"].InnerText);
        }


        float arg1 = 0;
		if (skillSetting["Arg1"] != null) {
			arg1 = float.Parse(skillSetting["Arg1"].InnerText);
		}
		float arg2 = 0;
		if (skillSetting["Arg2"] != null) {
			arg2 = float.Parse(skillSetting["Arg2"].InnerText);
		}
		float probability = 0;
		if (skillSetting["Probability"] != null) {
			probability = float.Parse(skillSetting["Probability"].InnerText);
		}
		
		Skill skill = new WeaponSkill(type, probability, range, arg1, arg2);
		
		return skill;
	}

	// build from a xml node in the Event.xml
	// <Require/Product class="theClass">theType</...>
	public static Skill BuildFromXMLNode(XmlNode node){
		if (node == null) {
			Debug.LogError("node is null.");
		}
		
		if (node.Attributes ["type"] == null) {
			Debug.LogError("type is null.");
		}
		string type = node.Attributes ["type"].Value;
		
		Skill skill = skills[type].Clone();
		
		return skill;
	}
	
	public static Skill Get(string type){
		return skills[type].Clone();
	}
}