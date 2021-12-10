using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GearListController : MonoBehaviour {
	public string GearType; //weapon , armor , accessory
	public Transform gearRowPrefab;
	public Transform gearRowEquippedPrefab;
	public Transform gearDetailPrefab;
    private Dictionary<Item, Transform> GearIndex;   // not really useful right now
  

    public void renderGearList(){
        GearIndex = new Dictionary<Item, Transform>();
        if (GearType.Equals("weapon")) {

			List<Weapon> weapons = Game.Current.Hero.UserInventory.AllWeapons;
			foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
			for (int i = 0; i < weapons.Count; i ++) {
				//add the items to the view 
				Weapon current_weapon = weapons[i];
				Transform clone;
				if(current_weapon == Game.Current.Hero.Gears.EquippedWeapon){
					clone = Instantiate(gearRowEquippedPrefab) as Transform;
				}else{
					 clone = Instantiate(gearRowPrefab) as Transform;
				}
                GearIndex.Add(current_weapon, clone);
				Text name = clone.GetComponentInChildren<Text>();
				RectTransform rf = clone.GetComponent<RectTransform>();
                name.text = getWeaponTitleString(current_weapon);
                switch (current_weapon.Tier)
                {
                    case 1:
                        name.color = Config.uncommonColor;
                        break;

                    case 2:
                        name.color = Config.rareColor;
                        break;

                    case 3:
                        name.color = Config.legendColor;
                        break;
                }
				clone.SetParent(transform);
				float height_constant = rf.rect.height;
				rf.anchorMin = new Vector2(0,1);
				rf.anchorMax = new Vector2(1,1);
				rf.localScale = new Vector3(1f,1f,1f);
				rf.offsetMin = new Vector2(22.5f, -(i+1)*(8f + height_constant));
				rf.offsetMax = new Vector2(-22.5f, -5 - i*(8f + height_constant));
				clone.GetComponent<Button>().onClick.AddListener(delegate {
					//show or hide gears detail
					clone.SetAsLastSibling(); //need to move front
					if(clone.FindChild("GearListDetail(Clone)") != null){
						//close 
						Destroy(clone.FindChild("GearListDetail(Clone)").gameObject);
					}else{
					   //open
						Transform gear_detail = Instantiate(gearDetailPrefab) as Transform;
						gear_detail.SetParent(clone);
						gear_detail.FindChild("Title").GetComponent<Text>().text = current_weapon.Name;
						gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getWeaponDetailString(current_weapon);
						RectTransform detail_rf  = gear_detail.GetComponent<RectTransform>();
						detail_rf.localScale = new Vector3(1f,1f,1f);
						detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
						detail_rf.offsetMax = new Vector2(-5f, 0f);


                        Button delete = gear_detail.FindChild("ButtonPannel").FindChild("Delete").GetComponent<Button>();
                        Button repair = gear_detail.FindChild("ButtonPannel").FindChild("Repair").GetComponent<Button>();
                        Button rebuild = gear_detail.FindChild("ButtonPannel").FindChild("Rebuild").GetComponent<Button>();


                        delete.onClick.AddListener(delegate {
                            Game.Current.Hero.loses(current_weapon);
                            renderGearList();
                        });
                        repair.onClick.AddListener(delegate {
                           
                            if (Shop.Current.UseProduct(ItemType.REPAIR_STONE, 1))
                            {
                                current_weapon.Fix();
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getWeaponDetailString(current_weapon);
                                name.text = getWeaponTitleString(current_weapon);
                                PlaySound("EquipWeapon");
                                Achievement.Current.UnlockAchievement(Achievement.AchievementType.FIRST_REPAIR);
                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REPAIR_STONE] +
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }


                        });
                        rebuild.onClick.AddListener(delegate {

                            if (Shop.Current.UseProduct(ItemType.REBUILD_STONE, 1))
                            {
                                bool re_equip = false;
                                if (Game.Current.Hero.Gears.EquippedWeapon == current_weapon)
                                {
                                    Game.Current.Hero.Gears.EquippedWeapon = null;
                                    re_equip = true;
                                }
                                current_weapon.GenerateProperties();
                                current_weapon.Fix();                  
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getWeaponDetailString(current_weapon);
                                name.text = getWeaponTitleString(current_weapon);
                                PlaySound("EquipWeapon");
                                if (re_equip)
                                {
                                    Game.Current.Hero.Gears.EquippedWeapon = current_weapon;
                                }

                                Achievement.Current.UnlockAchievement(Achievement.AchievementType.FIRST_REBUID);
                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REBUILD_STONE] +
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }

                        });


                    }
                });
				clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
					//add Weapon to player gear, or remove it when it is already equiped	
                    if(Game.Current.Hero.Gears.EquippedWeapon != current_weapon)
                    {
                        //equip
                        Game.Current.Hero.Gears.EquippedWeapon = current_weapon; //Equip the current weapon
                        Game.Current.Hero.UserInventory.AllWeapons.Remove(current_weapon);
                        Game.Current.Hero.UserInventory.AllWeapons.Insert(0, current_weapon);
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        PlaySound("EquipWeapon");
                    }
                    else
                    {
                        //unequip
                        Game.Current.Hero.Gears.EquippedWeapon = null;
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        PlaySound("EquipWeapon");
                
                    }

                });
			} 
		}
		else if (GearType.Equals("armor")){
			List<Armor> armors = Game.Current.Hero.UserInventory.AllArmors;
			foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
			for (int i = 0; i < armors.Count; i ++) {
				//add the items to the view 
				Armor current_armor = armors[i];
				Transform clone;
				if(current_armor == Game.Current.Hero.Gears.EquippedArmor){
					clone = Instantiate(gearRowEquippedPrefab) as Transform;
				}else{
					clone = Instantiate(gearRowPrefab) as Transform;
				}
                GearIndex.Add(current_armor, clone);


                Text name = clone.GetComponentInChildren<Text>();
				RectTransform rf = clone.GetComponent<RectTransform>();
                name.text = getArmorTitleString(current_armor);
                switch (current_armor.Tier)
                {
                    case 1:
                        name.color = Config.uncommonColor;
                        break;

                    case 2:
                        name.color = Config.rareColor;
                        break;

                    case 3:
                        name.color = Config.legendColor;
                        break;
                }

                clone.SetParent(transform);
				float height_constant = rf.rect.height;
				rf.anchorMin = new Vector2(0,1);
				rf.anchorMax = new Vector2(1,1);
				rf.localScale = new Vector3(1f,1f,1f);
				rf.offsetMin = new Vector2(22.5f, -(i+1)*(8f + height_constant));
				rf.offsetMax = new Vector2(-22.5f, -5 - i*(8f + height_constant));
				clone.GetComponent<Button>().onClick.AddListener(delegate {
					//show or hide gears detail
					clone.SetAsLastSibling(); //need to move front
					if(clone.FindChild("GearListDetail(Clone)") != null){
						//close 
						Destroy(clone.FindChild("GearListDetail(Clone)").gameObject);
					}else{
						//open
						Transform gear_detail = Instantiate(gearDetailPrefab) as Transform;
						gear_detail.SetParent(clone);
						gear_detail.FindChild("Title").GetComponent<Text>().text = current_armor.Name;					
						gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getArmorDetailString(current_armor);
						RectTransform detail_rf  = gear_detail.GetComponent<RectTransform>();
						detail_rf.localScale = new Vector3(1f,1f,1f);
						detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
						detail_rf.offsetMax = new Vector2(-5f, 0f);
        
                        Button delete = gear_detail.FindChild("ButtonPannel").FindChild("Delete").GetComponent<Button>();
                        Button repair = gear_detail.FindChild("ButtonPannel").FindChild("Repair").GetComponent<Button>();
                        Button rebuild = gear_detail.FindChild("ButtonPannel").FindChild("Rebuild").GetComponent<Button>();


                        delete.onClick.AddListener(delegate {
                            Game.Current.Hero.loses(current_armor);
                            renderGearList();
                        });
                        repair.onClick.AddListener(delegate {
                            
                            if (Shop.Current.UseProduct(ItemType.REPAIR_STONE, 1))
                            {
                                current_armor.Fix();
                                name.text = getArmorTitleString(current_armor);
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getArmorDetailString(current_armor);
                                PlaySound("EquipArmor");

                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REPAIR_STONE] +
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }


                        });
                        rebuild.onClick.AddListener(delegate {
                            if (Shop.Current.UseProduct(ItemType.REBUILD_STONE, 1))
                            {
                                bool re_equip = false;
                                if (Game.Current.Hero.Gears.EquippedArmor == current_armor)
                                {
                                    Game.Current.Hero.Gears.EquippedArmor = null;
                                    re_equip = true;

                                }
                                current_armor.GenerateProperties();
                                current_armor.Fix();            
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getArmorDetailString(current_armor);
                                name.text = getArmorTitleString(current_armor);
                                PlaySound("EquipArmor");
                                if (re_equip)
                                {
                                    Game.Current.Hero.Gears.EquippedArmor = current_armor;                                }
                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REBUILD_STONE] +
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }
                        });



                    }
                });
				clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
                    if(Game.Current.Hero.Gears.EquippedArmor != current_armor)
                    {
                        Game.Current.Hero.Gears.EquippedArmor = current_armor; //Equip the current weapon
                        Game.Current.Hero.UserInventory.AllArmors.Remove(current_armor);
                        Game.Current.Hero.UserInventory.AllArmors.Insert(0, current_armor);
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        PlaySound("EquipArmor");
                    }
                    else
                    {
                        Game.Current.Hero.Gears.EquippedArmor = null;
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        PlaySound("EquipArmor");
                    }                   
                });
			} 
		}
		else if (GearType.Equals("accessory")){
			List<Accessory> accessories = Game.Current.Hero.UserInventory.AllAccessories;
			foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
			for (int i = 0; i < accessories.Count; i ++) {
				//add the items to the view 
				Accessory current_accessory = accessories[i];
				Transform clone;
				if(current_accessory == Game.Current.Hero.Gears.EquippedAccessory){
					clone = Instantiate(gearRowEquippedPrefab) as Transform;
				}else{
					clone = Instantiate(gearRowPrefab) as Transform;
				}
                GearIndex.Add(current_accessory, clone);

                Text name = clone.GetComponentInChildren<Text>();
				RectTransform rf = clone.GetComponent<RectTransform>();
                name.text = getAccessoryTitleString(current_accessory);
                switch (current_accessory.Tier)
                {
                    case 1:
                        name.color = Config.uncommonColor;
                        break;

                    case 2:
                        name.color = Config.rareColor;
                        break;

                    case 3:
                        name.color = Config.legendColor;
                        break;
                }

                clone.SetParent(transform);
				float height_constant = rf.rect.height;
				rf.anchorMin = new Vector2(0,1);
				rf.anchorMax = new Vector2(1,1);
				rf.localScale = new Vector3(1f,1f,1f);
				rf.offsetMin = new Vector2(22.5f, -(i+1)*(8f + height_constant));
				rf.offsetMax = new Vector2(-22.5f, -5 - i*(8f + height_constant));
				clone.GetComponent<Button>().onClick.AddListener(delegate {
					//show or hide gears detail
					clone.SetAsLastSibling(); //need to move front
					if(clone.FindChild("GearListDetail(Clone)") != null){
						//close 
						Destroy(clone.FindChild("GearListDetail(Clone)").gameObject);
					}else{
						//open
						Transform gear_detail = Instantiate(gearDetailPrefab) as Transform;
						gear_detail.SetParent(clone);
						gear_detail.FindChild("Title").GetComponent<Text>().text = current_accessory.Name;                
                        gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getAccessoryDetailString(current_accessory);
						RectTransform detail_rf  = gear_detail.GetComponent<RectTransform>();
						detail_rf.localScale = new Vector3(1f,1f,1f);
						detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
						detail_rf.offsetMax = new Vector2(-5f, 0f);
                  
                        Button delete = gear_detail.FindChild("ButtonPannel").FindChild("Delete").GetComponent<Button>();
                        Button repair = gear_detail.FindChild("ButtonPannel").FindChild("Repair").GetComponent<Button>();
                        Button rebuild = gear_detail.FindChild("ButtonPannel").FindChild("Rebuild").GetComponent<Button>();

                        delete.onClick.AddListener(delegate {
                            Game.Current.Hero.loses(current_accessory);
                            renderGearList();
                        });
                        repair.onClick.AddListener(delegate {
                            if (Shop.Current.UseProduct(ItemType.REPAIR_STONE, 1))
                            {
                                current_accessory.Fix();
                                name.text = getAccessoryTitleString(current_accessory);
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getAccessoryDetailString(current_accessory);
                                PlaySound("EquipAccessory");
                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REPAIR_STONE] + 
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }

                        });
                        rebuild.onClick.AddListener(delegate {
                            if(Shop.Current.UseProduct(ItemType.REBUILD_STONE, 1))
                            {
                                bool re_equip = false;
                                if (Game.Current.Hero.Gears.EquippedAccessory == current_accessory)
                                {
                                    Game.Current.Hero.Gears.EquippedAccessory = null;
                                    re_equip = true;
                                }
                                current_accessory.GenerateProperties();
                                current_accessory.AddStaticValues(current_accessory.FixedData);
                                current_accessory.Fix();
                                name.text = getAccessoryTitleString(current_accessory);
                                gear_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = getAccessoryDetailString(current_accessory);
                                PlaySound("EquipAccessory");
                                if (re_equip)
                                {
                                    Game.Current.Hero.Gears.EquippedAccessory = current_accessory;
                                }                  

                            }
                            else
                            {
                                Game.Current.AddToast(Lang.Current[ItemType.REBUILD_STONE] +
                                    Lang.Current["not_enough"]);
                                PlaySound("Fail");

                            }

                        });

                    }
				});
				clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
                    if(Game.Current.Hero.Gears.EquippedAccessory != current_accessory)
                    {
                        Game.Current.Hero.Gears.EquippedAccessory = current_accessory; //Equip the current weapon
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        Game.Current.Hero.UserInventory.AllAccessories.Remove(current_accessory);
                        Game.Current.Hero.UserInventory.AllAccessories.Insert(0, current_accessory);
                        PlaySound("EquipAccessory");
                    }
                    else
                    {
                        Game.Current.Hero.Gears.EquippedAccessory = null;
                        transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                        PlaySound("EquipAccessory");

                    }



                });
			} 
		}

	            
	}


    private string getWeaponTitleString(Weapon current_weapon)
    {

        string title = current_weapon.Name + "(" + Element.getElementName(current_weapon.Element) + ")" + " " + Mathf.RoundToInt(current_weapon.Remaining * 100) + "%"
                        + " " + Lang.Current["attack_range"] + ":" + Math.Round(current_weapon.Attack, 1);
        return title;
    }

    private string getArmorTitleString(Armor current_armor)
    {
        string title = current_armor.Name + " " + Mathf.RoundToInt(current_armor.Remaining * 100) + "%"
            +  " " + Lang.Current["defense"] + ":" + Math.Round(current_armor.Defense,1);
        return title;
    }

    private string getAccessoryTitleString(Accessory current_accessory)
    {
        string title = current_accessory.Name + " " + Mathf.RoundToInt(current_accessory.Remaining * 100) + "%";
        return title;
    }

    private string getWeaponDetailString(Weapon weapon)
    {
        string content = "";
        content += Lang.Current["attack_range"] + ":" + Math.Round(weapon.Attack, 1) + "\n";
        content += Lang.Current["cool_down"] + ":" + weapon.CoolDown + "\n";
        content += Lang.Current["ring_speed"] + ":" + weapon.RingSpeed + "\n";
        content += Lang.Current["element_type"] + ":" + Element.getElementName(weapon.Element) + "\n";
        content += Lang.Current["durability"] + ":" + (int)(weapon.Remaining * 100) + "%" + "\n";
        content += Lang.Current["battle_cost"] + ":" + (weapon.Cost * 100) + "%" + "\n";
        List<WeaponSkill> skills = weapon.WeaponSkills;
        if (skills.Count > 0)
        {
            content += Lang.Current["weapon_skill"] + ":" + "\n";
            for (int k = 0; k < skills.Count; k++)
            {
                content += skills[k].Name + ":" + skills[k].Description + "\n";
            }
        }
        //content += current_weapon.Description;
        return content;
    }

    private string getArmorDetailString(Armor current_armor)
    {
        string content = "";
        content += Lang.Current["health_range"] + ":" + (int)current_armor.Health + "\n";
        content += Lang.Current["armor"] + ":" + (int)current_armor.Defense + "\n";
        content += Lang.Current["element_resis"] + ":" + "\n";
        foreach (ElementType key in current_armor.ElementResisIndex.Keys)
        {
            content += Element.getElementName(key) + ":" + (int)((1 - current_armor.ElementResisIndex[key]) * 100) + "% " + "\n";
        }

        content += "\n" + Lang.Current["durability"] + ":" + (int)(current_armor.Remaining * 100) + "%" + "\n";
        content += Lang.Current["battle_cost"] + ":" + (current_armor.Cost * 100) + "%" + "\n";
        List<ArmorSkill> skills = current_armor.ArmorSkills;
        if (skills.Count > 0)
        {
            content += Lang.Current["armor_skill"] + ":" + "\n";
            for (int k = 0; k < skills.Count; k++)
            {
                content += skills[k].Name + ":" + skills[k].Description + "\n";
            }
        }
        content += current_armor.Description;
        return content;
    }

    private string getAccessoryDetailString(Accessory current_accessory)
    {
        string content = "";
        if (current_accessory.Health != 0)
            content += Lang.Current["health_range"] + ":" + (int)current_accessory.Health + "\n";
        if (current_accessory.Defense != 0)
            content += Lang.Current["armor"] + ":" + (int)current_accessory.Defense + "\n";
        if (current_accessory.Attack != 0)
            content += Lang.Current["attack_range"] + ":" + (int)current_accessory.Attack + "\n";
        if (current_accessory.ElementResisIndex.Count > 0)
        {
            content += Lang.Current["element_resis"] + ":" + "\n";
            foreach (ElementType key in current_accessory.ElementResisIndex.Keys)
            {
                content += Element.getElementName(key) + ":" + (int)((1 - current_accessory.ElementResisIndex[key]) * 100) + "% " + "\n";
            }
        }

        if (current_accessory.ElementAttackIndex.Count > 0)
        {
            content += Lang.Current["element_attack_bonus"] + ":" + "\n";
            foreach (ElementType key in current_accessory.ElementAttackIndex.Keys)
            {
                content += Element.getElementName(key) + ":" + (int)(current_accessory.ElementAttackIndex[key] * 100) + "% " + "\n";
            }
        }


        content += Lang.Current["battle_cost"] + ":" + (current_accessory.Cost * 100) + "%" + "\n";

        content += current_accessory.Description;
        return content;
    }



    private void PlaySound(string name)
    {
        GameObject.Find(name).GetComponent<AudioSource>().Play();
    }
}
