using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Inventory  {
	//the inventory of the main Character that holding items

	private List<Food> foods = new List<Food> (); // type on purpose
	private List<Resource> resources = new List<Resource> ();
	private List<Tool> tools = new List<Tool> ();
	private List<Scroll> scrolls = new List<Scroll> ();
	private List<Weapon> weapons = new List<Weapon> ();
	private List<Armor> armors = new List<Armor> ();
	private List<Accessory> accessories = new List<Accessory> ();

	//assume resource and food objects are unique with type
	//given the type of resource, we can find the corresponding object
	private Dictionary<string, Resource> resource_index = new Dictionary<string, Resource> ();
	//given the type of food, we can find the corresponding object
	private Dictionary<string, Food> food_index = new Dictionary<string, Food> ();
	private Dictionary<string, Scroll> scroll_index = new Dictionary<string, Scroll> ();
	private Dictionary<string, Tool> tool_index = new Dictionary<string, Tool> ();


	public List<Food> AllFood{
		get{
			return foods;
		}
	}
	public List<Resource> AllResources{
		get{
			return resources;
		}
	}
	public List<Tool> AllTools {
		get {
			return tools;
		}
	}
	public List<Weapon> AllWeapons{
		get{
			return weapons;
		}
	}
	public List<Armor> AllArmors{
		get{
			return armors;
		}
	}
	public List<Accessory> AllAccessories{
		get{
			return accessories;
		}
	}
	public List<Scroll> AllScrolls {
		get {
			return scrolls;
		}
	}

	//get the corresponding object with type and class
	public Item Get(string type, string clazz){
		if (clazz == "Food") {
			if (food_index.ContainsKey (type)) {
				return food_index [type];
			}
		} else if (clazz == "Resource") {
			if (resource_index.ContainsKey (type)) {
				return resource_index [type];
			}
		} else if (clazz == "Scroll") {
			if (scroll_index.ContainsKey(type)){
				return scroll_index[type];
			}
		} else if (clazz == "Tool") {
			if (tool_index.ContainsKey(type)){
				return tool_index[type];
			}
		}
		// if we are going to add tool to this function, type 
		// here is the unique key
		return null;
	}



    public bool ContainsToolOfType(string type) {
        for(int i = 0; i < tools.Count; i++)
        {
            if (tools[i].Type.Equals(type))
            {
                return true;
            }
        }

        return false;
    }

	
	public void Add(Item item){
		if (item is Resource) {
			Resource resource = item as Resource;
			if (resource_index.ContainsKey (resource.Type)) {
				resource_index [resource.Type].Count += resource.Count;
			} else {
				Resource clone = resource.Clone() as Resource;
				resource_index [resource.Type] = clone;
				resources.Add (clone);
			}
		} else if (item is Food) {
			Food food = item as Food;
			if (food_index.ContainsKey (food.Type)) {
				food_index [food.Type].Count += food.Count;
			} else {
                Food clone = food.Clone() as Food;
				food_index [food.Type] = clone;
				foods.Add (clone);
			}
		} else if (item is Tool) {
			Tool tool = item.Clone() as Tool;
			tools.Add (tool);
			tool_index[tool.UniqueKey] = tool;
		} else if (item is Weapon) {
			Weapon weapon = item.Clone() as Weapon;
			weapons.Add (weapon);
		} else if (item is Armor) {
			Armor armor = item.Clone() as Armor;
			armors.Add (armor);
		} else if (item is Accessory) {
			Accessory ac = item.Clone() as Accessory;
			accessories.Add (ac);
		}else if (item is Scroll) {
			Scroll scroll = item.Clone() as Scroll;
			if (!scroll_index.ContainsKey (scroll.Type)) {
				scrolls.Add (scroll);
				scroll_index[scroll.Type] = scroll;
			}
		}
		else {
			Debug.LogError("Inventory.add: Can not regconize the type of the item.");
			return; // force quit
		}

		//tell recorder
		Game.Current.Recorder.Track (item.Type);
	}

	public void Remove(Item item){
		if (item is Resource) {
			Resource resource = item as Resource;
			if (resource_index.ContainsKey (resource.Type)) {
				if (resource_index [resource.Type].Count > resource.Count) {
					resource_index [resource.Type].Count -= resource.Count;
				} else {
					//should remove the item 
					Resource resourceInInventory = resource_index [resource.Type];
					resources.Remove (resourceInInventory);
					resource_index.Remove (resource.Type);
				}
			}
		} else if (item is Food) {
			Food food = item as Food;
			if (food_index.ContainsKey (food.Type)) {
				if (food_index [food.Type].Count > food.Count) {
					food_index [food.Type].Count -= food.Count;
				} else {
                    //should remvo the item 
					Food foodInInventory = food_index [food.Type];
					foods.Remove (foodInInventory);
					food_index.Remove (food.Type);
                    if(foodInInventory == Game.Current.Hero.FoodInBattle)
                    {
                        Game.Current.Hero.FoodInBattle = null; // remove from battle
                    }
				}
			}
		} else if (item is Tool) {
			//the Tool to be removed should have the correct reference
			tools.Remove ((Tool)item);
            tool_index.Remove(((Tool)item).UniqueKey);
		} else if (item is Weapon) {
			weapons.Remove ((Weapon)item);
		} else if (item is Armor) {
			armors.Remove ((Armor)item);
		} else if (item is Accessory) {
			accessories.Remove ((Accessory)item);
		} else if (item is Scroll) {
			//Never Remove a scroll
		}
		else {
			Debug.LogError("Inventory.remove: Can not regconize the type of the item: ");
			return; // force quit
		}          
	}


    //return the first tool of a certain type,used to continuous feed same tool to the user
    public Tool getFirstToolOfType(string tool_type)
    {
        Tool tool = null;
        for(int i = 0; i < tools.Count; i++)
        {
            if (tools[i].Type.Equals(tool_type))
            {
                tool = tools[i];
            }
        }

        return tool;
    }



	public bool Has(Item item, bool ignoreCount = false){
		// Check both type and count (if the count is not ignored.).
		if (item is Resource) {
			Resource resource = item as Resource; 
			if (!resource_index.ContainsKey (resource.Type)) {
				return false;
			}
			if (!ignoreCount) {
				Resource resourceInInventory = resource_index [item.Type];
				if (resourceInInventory != null && resource.Count <= resourceInInventory.Count) {
					return true;
				}
			} else {
				// no check on count
				return true;
			}
		} else if (item is Food) {
			Food foodItem = item as Food; 
			if (!food_index.ContainsKey (foodItem.Type)) {
				return false;
			}
			if (!ignoreCount) {
				Food foodInInventory = food_index [foodItem.Type];
				if (foodInInventory != null && foodItem.Count <= foodInInventory.Count) {
					return true;
				}
			} else {
				// no check on count
				return true;
			}
		
		} else if (item is Scroll) {
			if (scroll_index.ContainsKey (item.Type)) {
				return true;
			}
		} else {
			Debug.LogError ("Inventory.has: Can not regconize the type of the item." + item.Name);
			return false;
		} 
		return false;
	}
}
