using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

public class ItemFactory: Factory
{
	protected static Dictionary<string, Item> items ;


	private static Dictionary<string, Item> Init(){
		TextAsset resource = (TextAsset)Resources.Load("Config/Item");
		XmlDocument settings = new XmlDocument ();
		settings.LoadXml (resource.text);
		
		Dictionary<string, Item> items = new Dictionary<string, Item> ();

        XmlNodeList itemXMLs = settings.SelectNodes ("/Items/Item");
		foreach (XmlNode itemNode in itemXMLs) {
			if(itemNode.Attributes["type"] == null){
				Debug.LogError(itemNode.OuterXml);
			}
			string type = itemNode.Attributes["type"].Value;
			string clazz = itemNode.Attributes["class"].Value;

			Item item = null;
			if (clazz == "Resource") {
				item = BuildResource (type, itemNode);
			} else if (clazz == "Tool") {
				item = BuildTool (type, itemNode);
			} else if (clazz == "Weapon") {
				item = BuildWeapon(type, itemNode);
			} else if (clazz == "Armor") {
				item = BuildArmor(type, itemNode);
			} else if (clazz == "Accessory") {
				item = BuildAccessory(type, itemNode);
			} else if (clazz == "Scroll") {
				item = BuildScroll(type, itemNode);
			} else if (clazz == "Food") {
				item = BuildFood(type, itemNode);
			}
			
			if (itemNode ["SpawnIgnore"] != null) {
				bool spawnIgnore = bool.Parse(itemNode ["SpawnIgnore"].InnerText);
				item.SpawnIgnore = spawnIgnore;
			}

            if (itemNode["Tier"] != null)
            {
                int tier = int.Parse(itemNode["Tier"].InnerText);
                item.Tier = tier;
            }

            items[type] = item;
		}
		
		return items;
	}
	
	public static void Reload(){
		items = Init();
	}

	public static Resource BuildResource(string type, XmlNode itemSetting){ 
		Resource resource = new Resource(type, 1);
		return resource;
	}

	public static Resource BuildResource(string type, int count){ 
		Resource resource = new Resource(type, count);
		return resource;
	}

	public static Tool BuildTool(string type, XmlNode itemSetting){
       
		if (itemSetting ["InitRemaining"] == null) {
			Debug.LogError("Type: "+type+" InitRemaining is null.");
		}
		float initRemaining = float.Parse (itemSetting ["InitRemaining"].InnerText);

		bool isEquipment = false;
		if (itemSetting ["IsEquipment"] != null) {
			isEquipment = bool.Parse (itemSetting ["IsEquipment"].InnerText);
		}
		 
		bool isOneTime = false;
		if (itemSetting ["IsOneTime"] != null) {
			isOneTime = bool.Parse (itemSetting ["IsOneTime"].InnerText);
		}

		Tool tool = new Tool (type, initRemaining, isEquipment, isOneTime);

		return tool;
	}

/*
	public static Tool buildTool(string type){
		Tool tool;
		string[] equipemnts = {ItemType.WOOD_TRAP, ItemType.CAMP_FIRE, ItemType.TENT, ItemType.SPIKE_TRAP,
		ItemType.WARM_MACHINE,ItemType.COLD_MACHINE,ItemType.DRY_MACHINE, ItemType.HUMID_MACHINE,
			ItemType.MOONLIGHT_TOTEM, ItemType.MUSIC_BOX};
		string[] one_times = {ItemType.EVIL_TOTEM, ItemType.LUCKY_TOTEM, ItemType.SCARECROW, ItemType.CLIMATE_GLUE};

		if (Array.IndexOf (equipemnts, type) > -1){
			tool = new Tool (type, 1f, true, false);
			return tool;
		}

		if (Array.IndexOf (one_times, type) > -1) {
			tool = new Tool (type, 1f, false, true);
			return tool;
		}

		tool = new Tool (type, 1f, false, false);
		return tool;
	}
*/

	public static Food BuildFood(string type, XmlNode itemSetting){

		if (itemSetting ["HealValue"] == null) {
			Debug.LogError("Type: "+type+" HealValue is null.");
		}
		float healValue = float.Parse (itemSetting ["HealValue"].InnerText);

		bool isIngredient = false;
		if (itemSetting ["IsIngredient"] != null) {
			isIngredient = bool.Parse (itemSetting ["IsIngredient"].InnerText);
		}

		string subClass = "";
		if (itemSetting ["SubClass"] != null) {
			subClass = itemSetting ["SubClass"].InnerText;
		}

		Food food = new Food (type, 1, healValue, isIngredient, subClass);

        if (itemSetting["CoolDown"] != null)
        {
            food.CoolDown = float.Parse(itemSetting["CoolDown"].InnerText);
        }

		return food;
	}

	public static Weapon BuildWeapon(string type, XmlNode itemSetting){

		if (itemSetting ["InitRemaining"] == null) {
			Debug.LogError("Type: "+type+" InitRemaining is null.");
		}
		float initRemaining = float.Parse (itemSetting ["InitRemaining"].InnerText);

		if (itemSetting ["Cost"] == null) {
			Debug.LogError("Type: "+type+" Cost is null.");
		}
		float cost = float.Parse (itemSetting ["Cost"].InnerText);

		if (itemSetting ["AttackRangeUpper"] == null) {
			Debug.LogError("Type: "+type+" AttackRangeUpper is null.");
		}
		float attackRangeUpper = float.Parse (itemSetting ["AttackRangeUpper"].InnerText);

		if (itemSetting ["AttackRangeLower"] == null) {
			Debug.LogError("Type: "+type+" AttackRangeLower is null.");
		}
		float attackRangeLower = float.Parse (itemSetting ["AttackRangeLower"].InnerText);

		if (itemSetting ["ElementType"] == null) {
			Debug.LogError("Type: "+type+" ElementType is null.");
		}
		ElementType elementType = (ElementType)Enum.Parse (typeof(ElementType), itemSetting ["ElementType"].InnerText);

		if (itemSetting ["CD"] == null) {
			Debug.LogError("Type: "+type+" CD is null.");
		}
		int cd = int.Parse (itemSetting ["CD"].InnerText);

		if (itemSetting ["RingSpeed"] == null) {
			Debug.LogError("Type: "+type+" RingSpeed is null.");
		}
		float ringSpeed = float.Parse (itemSetting ["RingSpeed"].InnerText);

		Weapon weapon = new Weapon (type, initRemaining, cost, new Range(attackRangeLower, attackRangeUpper), cd, ringSpeed, elementType);

        switch (type)
        {
            case ItemType.SMALL_ICE_KNIFE:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG_SKILL, 1f, 0.2f, 10f, 2f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_ICE, 1f, 0.25f, 10);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.NEW_FIRE_GUN:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.2f, 2f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_FIRE, 1f, 0.25f, 10);
                    weapon.AddSkill(skill);
                }
                break;



            case ItemType.CHUANJIA_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_POISON, 1f, 0.2f, 5, 0.005f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_WIND, 1f, 0.25f, 10);
                    weapon.AddSkill(skill);
                }
                break;



            case ItemType.BLOOD_DRINK_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.2f, 9.5f, 2.5f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_ELEMENT_CHANGE, 1f, 0.25f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.QUMO_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_TOUGH, 1f, 0.2f, 5.5f, 1.5f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_DARK, 1f, 0.2f, 3.5f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.METAL_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STUN, 1f, 0.16f, 6.5f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.WIND_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_TOUGH, 1f, 0.2f, 4.5f, 1.5f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.FIRE_BOW:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BURNT, 1f, 0.2f, 10f, 0.12f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_FIRE, 1f, 0.15f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ACIENT_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_SUCK, 1f, 0.35f, 0.59f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.2f, 3f, 2.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.DEEP_FISH_SWORD:
                {
                   WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_SUCK, 1f, 0.3f, 0.59f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.3f, 5f, 2f);
                    weapon.AddSkill(skill);

                }
                break;

            case ItemType.FISH_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_SUCK, 1f, 0.25f, 0.59f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.BREAK_BLADE_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_ARMOR_BREAK, 1f, 0.25f, 10f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.CRYSTAL_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.2f, 1.3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.YOUNG_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 1.2f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.OLD_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 1.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.GREAT_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.4f, 1.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_LANCER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STUN, 1f, 0.2f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_HAMMER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_SLOW, 1, 0.4f, 8f, 2f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_REAP:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_SLOW, 1, 0.3f, 10f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_GIANT_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_SLOW, 1, 0.3f, 12f, 3f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_SUCK, 1f, 0.15f, 0.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.FIRE_AXE:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BURNT, 1, 0.3f, 5f, 0.17f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.FIRE_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BURNT, 1f, 0.3f, 5f, 0.3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.FIRE_BIG_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BURNT, 1f, 0.3f, 6f, 0.45f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.FIRE_BIG_STAFF:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BURNT, 1f, 0.2f, 12f, 0.2f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.15f, 1.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.WIND_DART:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_VOID, 1, 0.25f, 4f, 0.4f);
                    weapon.AddSkill(skill);              
                }
                break;

            case ItemType.WIND_BIG_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_VOID, 1, 0.25f, 5f, 0.4f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.WIND_BIG_AXE:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.2f, 2f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.WIND_BIG_DART:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_VOID, 1, 0.25f, 5f, 0.4f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.15f, 2f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.DARK_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.25f, 6f, 3f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_VOID, 1f, 0.25f, 8f, 0.1f);
                    weapon.AddSkill(skill);

                }
                break;


            case ItemType.DARK_WUDU_STAFF:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG_SKILL, 1f, 0.22f, 7f, 2.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.DARK_CLAW:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_POISON, 1f, 0.25f, 5f, 0.006f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.DARK_LANCER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.22f, 7f, 5f);
                    weapon.AddSkill(skill);

                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_VOID, 1f, 0.25f, 10f, 0.1f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.DARK_GIANT_AXE:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG, 1f, 0.22f, 15f, 3f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.DARK_GIANT_REAPER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG, 1f, 0.22f, 15f, 3f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.22f, 15f, 3f);
                    weapon.AddSkill(skill);
                }
                break;


            case ItemType.DARK_DEMON_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG, 1f, 0.22f, 30f, 3f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_RAGE, 1f, 0.22f, 30f, 3f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_ARMOR_BREAK, 1f, 0.22f, 30f);
                    weapon.AddSkill(skill);
                }
                break;



            case ItemType.HOLY_JUSTICE_HAMMER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_DARK, 1f, 0.25f, 5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_JUDGE_STAFF:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STUN, 1f, 0.25f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_STAFF:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STUN, 1f, 0.3f, 1.8f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_HAMMER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_STUN, 1f, 0.3f, 2.3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_LIGHT_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.18f,1.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_LIGHT_LANCER:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f,  0.18f, 1.8f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.HOLY_SAINT_BOOK:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f,  0.15f, 1.7f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_SLOW, 1f, 0.25f, 3f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.NORMAL_BLACK_IRON_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.25f, 2f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.SPECIAL_BLACK_IRON_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 2.5f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.MASTER_BLACK_IRON_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 2.5f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_BLOOD_SUCK, 1f, 0.2f, 0.33f);
                    weapon.AddSkill(skill);
                    skill = new WeaponSkill(SkillType.WEAPON_SKILL_STRONG_SKILL, 1, 0.25f, 5f, 3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.LIGHT_SPEAR:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_TOUGH, 1f, 0.2f, 6f, 1.2f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.SPIKE_GLOVE:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 1.3f);
                    weapon.AddSkill(skill);
                }
                break;

            case ItemType.ICE_BLINK_SWORD:
                {
                    WeaponSkill skill = new WeaponSkill(SkillType.WEAPON_SKILL_FIRE, 1f, 0.25f, 10f);
                    weapon.AddSkill(skill);
                }
                break;


        }

		return weapon;
	}

/*
	public static Weapon buildWeapon(string type){
		Weapon the_weapon = null;
		WeaponSkill skill;
		switch (type) {
		case ItemType.IRON_SWORD:
			the_weapon = new Weapon (type, 1f, 0.05f,new Range(2f,5f),3f,1.5f, ElementType.None);
			break;

		case ItemType.SPIKE_GLOVE:
			the_weapon = new Weapon (type, 1f, 0.05f,new Range(1f,2f),2,2f, ElementType.None);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 3f);
			the_weapon.AddSkill (skill);
			break;


		case ItemType.YOUNG_SWORD:
			the_weapon = new Weapon (type, 1f, 0.05f,new Range(6f, 12f), 3, 1f, ElementType.None);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 1.2f);
			the_weapon.AddSkill (skill);
			break;


		case ItemType.OLD_SWORD:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(28f, 45f), 3, 1f, ElementType.None);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.3f, 1.5f);
			the_weapon.AddSkill (skill);
			break;

		case ItemType.GREAT_SWORD:
			the_weapon = new Weapon (type, 1f, 0.01f,new Range(50f, 60f), 3, 1f, ElementType.None);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.4f, 1.5f);
			the_weapon.AddSkill (skill);
			break;

		case ItemType.CRYSTAL_SWORD:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(5f,10f),3,1.8f, ElementType.None);
			break;
		
		case ItemType.ICE_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(10f,13f),2, 1.5f, ElementType.Ice);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.2f, 1.5f);
			the_weapon.AddSkill (skill);
			break;

		case ItemType.ICE_GUN:
			the_weapon = new Weapon (type, 1f, 0.025f,new Range(11f,12f),2, 1.3f, ElementType.Ice);
			break;

		case ItemType.ICE_LANCER:
			the_weapon = new Weapon (type, 1f, 0.025f,new Range(8f,12f),1, 1.7f, ElementType.Ice);
			break;

		case ItemType.ICE_HAMMER:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(65f,75f),5, 1.7f, ElementType.Ice);
			break;

		case ItemType.ICE_REAP:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(80f,90f),5, 1.5f, ElementType.Ice);
			break;

		case ItemType.ICE_GIANT_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(70f,80f),3, 1.7f, ElementType.Ice);
			break;
			

		case ItemType.FIRE_BOW:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(15f,20f),4, 1.3f, ElementType.Fire);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.35f, 2f);
			the_weapon.AddSkill (skill);
			break;


		case ItemType.FIRE_KNIFE:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(32f,38f), 8, 1.2f, ElementType.Fire);
			break;

		case ItemType.FIRE_AXE:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(42f,48f), 6, 1.1f, ElementType.Fire);
			break;

		case ItemType.FIRE_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(17f,19f), 2, 1.7f, ElementType.Fire);
			break;

		case ItemType.FIRE_BIG_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(25f,30f), 2, 1.5f, ElementType.Fire);
			break;


		case ItemType.FIRE_BIG_STAFF:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(110f,140f), 6, 1.3f, ElementType.Fire);
			break;
			

		case ItemType.WIND_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(6f,8f),1, 2f, ElementType.Wind);
			skill = new WeaponSkill (SkillType.WEAPON_SKILL_CRITICAL, 1f, 0.2f, 2.5f);
			the_weapon.AddSkill (skill);
			break;


		case ItemType.WIND_WHIP:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(22f,25f	),5, 1.5f, ElementType.Wind);
			break;

		case ItemType.WIND_DART:
			the_weapon = new Weapon (type, 1f, 0.03f,new Range(7f,9f),1, 2f, ElementType.Wind);
			break;

		case ItemType.WIND_BIG_SWORD:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(8f,11f),1, 1.5f, ElementType.Wind);
			break;

		case ItemType.WIND_BIG_AXE:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(26f,33f), 2, 1.7f, ElementType.Wind);
			break;

		case ItemType.WIND_BIG_DART:
			the_weapon = new Weapon (type, 1f, 0.02f,new Range(38f,45f), 2, 1.8f, ElementType.Wind);
			break;
				
		
	
		}
		return the_weapon;
	}
*/

	public static Armor BuildArmor(string type, XmlNode itemSetting){	
		if (itemSetting ["InitRemaining"] == null) {
			Debug.LogError("Type: "+type+" InitRemaining is null.");
		}
		float initRemaining = float.Parse (itemSetting ["InitRemaining"].InnerText);
		
		if (itemSetting ["Cost"] == null) {
			Debug.LogError("Type: "+type+" Cost is null.");
		}
		float cost = float.Parse (itemSetting ["Cost"].InnerText);
		
		if (itemSetting ["DefenseRangeUpper"] == null) {
			Debug.LogError("Type: "+type+" DefenseRangeUpper is null.");
		}
		float defenseRangeUpper = float.Parse (itemSetting ["DefenseRangeUpper"].InnerText);
		
		if (itemSetting ["DefenseRangeLower"] == null) {
			Debug.LogError("Type: "+type+" DefenseRangeLower is null.");
		}
		float defenseRangeLower = float.Parse (itemSetting ["DefenseRangeLower"].InnerText);

		if (itemSetting ["HealthRangeUpper"] == null) {
			Debug.LogError("Type: "+type+" HealthRangeUpper is null.");
		}
		float healthRangeUpper = float.Parse (itemSetting ["HealthRangeUpper"].InnerText);
		
		if (itemSetting ["HealthRangeLower"] == null) {
			Debug.LogError("Type: "+type+" HealthRangeLower is null.");
		}
		float healthRangeLower = float.Parse (itemSetting ["HealthRangeLower"].InnerText);
		
		Armor armor = new Armor (type, initRemaining, cost, new Range(defenseRangeLower, defenseRangeUpper), new Range(healthRangeLower, healthRangeUpper));

        if (itemSetting["ElementResisUpper"] != null
            && itemSetting["ElementResisLower"] != null)
        {
            float upper = float.Parse(itemSetting["ElementResisUpper"].InnerText);
            float lower = float.Parse(itemSetting["ElementResisLower"].InnerText);
            armor.hasResistance = true;
            armor.ElementResisRange = new Range(lower, upper);
        }

        /*
        if (itemSetting ["ElementResisFire"] != null) {
			armor.ElementResisIndex.Add (ElementType.Fire, float.Parse (itemSetting ["ElementResisFire"].InnerText));
		}

		if (itemSetting ["ElementResisIce"] != null) {
			armor.ElementResisIndex.Add (ElementType.Ice, float.Parse (itemSetting ["ElementResisIce"].InnerText));
		}

		if (itemSetting ["ElementResisWind"] != null) {
			armor.ElementResisIndex.Add (ElementType.Wind, float.Parse (itemSetting ["ElementResisWind"].InnerText));
		}

		if (itemSetting ["ElementResisDark"] != null) {
			armor.ElementResisIndex.Add (ElementType.Dark, float.Parse (itemSetting ["ElementResisDark"].InnerText));
		}

		if (itemSetting ["ElementResisHoly"] != null) {
			armor.ElementResisIndex.Add (ElementType.Holy, float.Parse (itemSetting ["ElementResisHoly"].InnerText));
		}
        */


        switch (armor.Type)
        {
            case ItemType.LOG_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_ICE_REFLECT, 0.1f, 1);
                    armor.AddArmorSkill(skill);
                   
                }
                break;


            case ItemType.IRON_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_IRON_WALL, 0.1f, 3);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.FUR_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_WIND_SHIELD, 0.08f, 3f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.FOG_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_MISS, 0.1f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.JUSTICE_ARMOR:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_HOLY_REFLECT, 0.25f, 2f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.SPECIAL_BLACK_IRON_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_IRON_WALL, 0.1f, 5f);
                    armor.AddArmorSkill(skill);
                    skill = new ArmorSkill(SkillType.ARMOR_SKILL_FOCUS, 0.25f, 5f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.MASTER_BLACK_IRON_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_IRON_WALL, 0.1f, 5f);
                    armor.AddArmorSkill(skill);
                    skill = new ArmorSkill(SkillType.ARMOR_SKILL_REVIVE, 0.3f, 0.5f);
                    armor.AddArmorSkill(skill);
                    skill = new ArmorSkill(SkillType.ARMOR_SKILL_FOCUS, 0.25f, 8f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.FOX_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_FIRE_SHIELD, 0.2f, 2.5f);
                    armor.AddArmorSkill(skill);                
                }
                break;

            case ItemType.ICE_BEAST_ARMOR:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_ABSORB, 0.25f, 2.5f);
                    armor.AddArmorSkill(skill);
                    skill = new ArmorSkill(SkillType.ARMOR_SKILL_DIVINE_SHIELD, 0.04f, 10f);
                    armor.AddArmorSkill(skill);
                    skill = new ArmorSkill(SkillType.ARMOR_SKILL_FOCUS, 0.2f, 3f);
                    armor.AddArmorSkill(skill);
                }
                break;

            case ItemType.GLORIOUS_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_REVIVE, 0.5f, 0.3f);
                    armor.AddArmorSkill(skill);
                }
                break;


            case ItemType.DEMON_ENERGY_SUIT:
                {
                    ArmorSkill skill = new ArmorSkill(SkillType.ARMOR_SKILL_INSTANT_ULTI, 0.2f);
                    armor.AddArmorSkill(skill);
                }
                break;


        }
		
		return armor;
	}

/*
	public static Armor buildArmor(string type){	
		Armor the_armor = null;
		switch (type) {
		case ItemType.GRASS_SUIT:
			the_armor = new Armor (type, 1f, 0.02f, new Range(3f,5f), new Range(5f,10f));
			//the_armor.ElementResisIndex.Add (ElementType.Fire, 0.5f); //50% fire resistence
			//ArmorSkill miss = new ArmorSkill (SkillType.ARMOR_SKILL_MISS, 0.5f);
			//the_armor.ArmorSkills.Add (miss);
			break;

		case ItemType.LOG_SUIT:
			the_armor = new Armor (type, 1f, 0.03f, new Range(10f,15f), new Range(0f,0f));
			break;

		case ItemType.IRON_SUIT:
			the_armor = new Armor (type, 1f, 0.03f, new Range(25f, 35f), new Range(0f,0f));
			break;

		case ItemType.WOLF_SUIT:
			the_armor = new Armor (type, 1f, 0.05f, new Range(5f,8f), new Range(10f,15f));
			the_armor.ElementResisIndex.Add (ElementType.Wind, 0.95f);
			break;

		case ItemType.TIGER_SUIT:
			the_armor = new Armor (type, 1f, 0.05f, new Range(5f,8f), new Range(10f,15f));
			the_armor.ElementResisIndex.Add (ElementType.Fire, 0.95f);
			break;

		case ItemType.FUR_SUIT:
			the_armor = new Armor (type, 1f, 0.03f, new Range(8f,13f), new Range(15f,20f));
			break;
		}
		return the_armor;
	}
*/
	public static Accessory BuildAccessory(string type, XmlNode itemSetting){
		if (itemSetting ["InitRemaining"] == null) {
			Debug.LogError ("Type: " + type + " InitRemaining is null.");
		}
		float initRemaining = float.Parse (itemSetting ["InitRemaining"].InnerText);
		
		if (itemSetting ["Cost"] == null) {
			Debug.LogError ("Type: " + type + " Cost is null.");
		}
		float cost = float.Parse (itemSetting ["Cost"].InnerText);
		
		if (itemSetting ["DefenseRangeUpper"] == null) {
			Debug.LogError ("Type: " + type + " DefenseRangeUpper is null.");
		}
		float defenseRangeUpper = float.Parse (itemSetting ["DefenseRangeUpper"].InnerText);
		
		if (itemSetting ["DefenseRangeLower"] == null) {
			Debug.LogError ("Type: " + type + " DefenseRangeLower is null.");
		}
		float defenseRangeLower = float.Parse (itemSetting ["DefenseRangeLower"].InnerText);

		if (itemSetting ["AttackRangeUpper"] == null) {
			Debug.LogError ("Type: " + type + " AttackRangeUpper is null.");
		}
		float attackRangeUpper = float.Parse (itemSetting ["AttackRangeUpper"].InnerText);
		
		if (itemSetting ["AttackRangeLower"] == null) {
			Debug.LogError ("Type: " + type + " AttackRangeLower is null.");
		}
		float attackRangeLower = float.Parse (itemSetting ["AttackRangeLower"].InnerText);

		if (itemSetting ["HealthRangeUpper"] == null) {
			Debug.LogError ("Type: " + type + " HealthRangeUpper is null.");
		}
		float healthRangeUpper = float.Parse (itemSetting ["HealthRangeUpper"].InnerText);
		
		if (itemSetting ["HealthRangeLower"] == null) {
			Debug.LogError ("Type: " + type + " HealthRangeLower is null.");
		}
		float healthRangeLower = float.Parse (itemSetting ["HealthRangeLower"].InnerText);

		if (itemSetting ["ElementResisRangeUpper"] == null) {
			Debug.LogError ("Type: " + type + " ElementResisRangeUpper is null.");
		}
		float elementResisRangeUpper = float.Parse (itemSetting ["ElementResisRangeUpper"].InnerText);
		
		if (itemSetting ["ElementResisRangeLower"] == null) {
			Debug.LogError ("Type: " + type + " ElementResisRangeLower is null.");
		}
		float elementResisRangeLower = float.Parse (itemSetting ["ElementResisRangeLower"].InnerText);

        if (itemSetting["ElementAttackRangeUpper"] == null)
        {
            Debug.LogError("Type: " + type + " ElementAttackRangeUpper is null.");
        }
        float elementAttackRangeUpper = float.Parse(itemSetting["ElementAttackRangeUpper"].InnerText);

        if (itemSetting["ElementAttackRangeLower"] == null)
        {
            Debug.LogError("Type: " + type + " ElementAttackRangeLower is null.");
        }
        float elementAttackRangeLower = float.Parse(itemSetting["ElementAttackRangeLower"].InnerText);


        if (itemSetting ["AttributeCount"] == null) {
			Debug.LogError ("Type: " + type + " AttributeCount is null.");
		}
		int attributeCount = int.Parse (itemSetting ["AttributeCount"].InnerText);
		
		Accessory accessory = new Accessory (type, initRemaining, cost, new Range (defenseRangeLower, defenseRangeUpper), new Range (healthRangeLower, healthRangeUpper),
            new Range (attackRangeLower, attackRangeUpper),
            new Range (elementResisRangeLower, elementResisRangeUpper),
            new Range(elementAttackRangeLower, elementAttackRangeUpper), attributeCount);


        if (itemSetting["Attack"] != null)
        {
            accessory.FixedData.Attack = float.Parse(itemSetting["Attack"].InnerText);
        }

        if (itemSetting["Defense"] != null)
        {
            accessory.FixedData.Defense = float.Parse(itemSetting["Defense"].InnerText);
        }

        if (itemSetting["Health"] != null)
        {
            accessory.FixedData.Health = float.Parse(itemSetting["Health"].InnerText);
        }

        if (itemSetting["ElementResisIce"] != null)
        {
            accessory.FixedData.ElementResisIndex[ElementType.Ice] =
                float.Parse(itemSetting["ElementResisIce"].InnerText);
        }

        if (itemSetting["ElementResisFire"] != null)
        {
            accessory.FixedData.ElementResisIndex[ElementType.Fire] =
                float.Parse(itemSetting["ElementResisFire"].InnerText);
        }

        if (itemSetting["ElementResisWind"] != null)
        {
            accessory.FixedData.ElementResisIndex[ElementType.Wind] =
                float.Parse(itemSetting["ElementResisWind"].InnerText);
        }


        if (itemSetting["ElementResisDark"] != null)
        {
            accessory.FixedData.ElementResisIndex[ElementType.Dark] =
                float.Parse(itemSetting["ElementResisDark"].InnerText);
        }


        if (itemSetting["ElementResisHoly"] != null)
        {
            accessory.FixedData.ElementResisIndex[ElementType.Holy] =
                float.Parse(itemSetting["ElementResisHoly"].InnerText);
        }

        if(itemSetting["ElementAttackIce"] != null)
        {
            accessory.FixedData.ElementAttackIndex[ElementType.Ice] =
                float.Parse(itemSetting["ElementAttackIce"].InnerText);
        }

        if (itemSetting["ElementAttackFire"] != null)
        {
            accessory.FixedData.ElementAttackIndex[ElementType.Fire] =
                float.Parse(itemSetting["ElementAttackFire"].InnerText);
        }

        if (itemSetting["ElementAttackWind"] != null)
        {
            accessory.FixedData.ElementAttackIndex[ElementType.Wind] =
                float.Parse(itemSetting["ElementAttackWind"].InnerText);
        }

        if (itemSetting["ElementAttackHoly"] != null)
        {
            accessory.FixedData.ElementAttackIndex[ElementType.Holy] =
                float.Parse(itemSetting["ElementAttackHoly"].InnerText);
        }


        if (itemSetting["ElementAttackDark"] != null)
        {
            accessory.FixedData.ElementAttackIndex[ElementType.Dark] =
                float.Parse(itemSetting["ElementAttackDark"].InnerText);
        }



        return accessory;
	}
/*
	public static Accessory buildAccessory(string type){
		Accessory the_accessory = null;
		switch (type) {
		case ItemType.TOOTH_NECKLACE:
			the_accessory = new Accessory (type, 1f, 0.05f, new Range(1f,5f),new Range(1f,10f),new Range(1f,3f),new Range(0.85f,0.95f),2);
			the_accessory.GenerateProperties();
			break;

		case ItemType.FLOWER_RING:
			the_accessory = new Accessory (type, 1f, 0.02f, new Range(1f,3f),new Range(1f,3f),new Range(1f,8f),new Range(0.90f,0.98f),3);
			the_accessory.GenerateProperties();
			break;

		case ItemType.FEATHER_HAT:
			the_accessory = new Accessory (type, 1f, 0.05f, new Range(5f,8f),new Range(15f,20f),new Range(1f,1f),new Range(0.80f,0.90f),2);
			the_accessory.GenerateProperties();
			break;
		}
		return the_accessory;
	}
*/
	public static Scroll BuildScroll(string type, XmlNode itemSetting){

		Scroll scroll = new Scroll (type);

		return scroll;
	}

	public static Scroll BuildScroll(string type){
		
		Scroll scroll = new Scroll (type);
		
		return scroll;
	}

	// build from a xml node in the Item.xml
	// <Require/Product class="theClass">theType</...>
	public static Item BuildFromXMLNode(XmlNode node){
		if (node == null) {
			Debug.LogError("node is null.");
		}
		
		if (node.Attributes ["type"] == null) {
			Debug.LogError("type is null.");
		}
		string type = node.Attributes ["type"].Value;

		if (!items.ContainsKey (type)) {
		//	Debug.Log (type + " not found."); //TODO: why is this calling all the time
			return null;
		}
		Item item = items[type].Clone();

		if (node["Count"] != null) {
			StackableItem stackable = item as StackableItem;
			stackable.Count = int.Parse (node["Count"].InnerText);
		}

		// Only tool use this attribute right now.
		if (node ["Cost"] != null) {
			Tool tool = item as Tool;
			tool.Cost = float.Parse (node ["Cost"].InnerText);
		}
		
		if (node ["SpawnIgnore"] != null) {
			bool spawnIgnore = bool.Parse(node ["SpawnIgnore"].InnerText);
			item.SpawnIgnore = spawnIgnore;
		}

		return item;
	}

	public static Item Get(string type){
		if (!items.ContainsKey (type)) {
			Debug.Log (type + " not found.");
			return null;
		}
		return items [type].Clone ();
	}
}


