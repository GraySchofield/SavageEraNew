using UnityEngine;
using System.Collections;

[System.Serializable]
public class DropNode {
	//used to represent a drop node in a monster's drop list
	public string ItemType {
		get;
		set;
	}

	//Food, Resource, Tool, Weapon ....
	public string ItemCategory {
		get;
		set;
	}

	public float DropProbability {
		get;
		set;
	}

	//this value only meaningful for stackable items
	//otherwise will drop 1
	public int DropCountUpperLimit {
		get;
		set;
	}


	public DropNode(string item_type, string item_category, float probability, int count_limit){
		this.ItemType = item_type;
		this.ItemCategory = item_category;
		this.DropProbability = probability;
		this.DropCountUpperLimit = count_limit;
	}


	public Item GenerateDroppedItem(){
		if (Random.value <= DropProbability) {
			if(ItemCategory.Equals("Resource")){
				Resource item = ItemFactory.BuildResource(ItemType,(int)Random.Range(1,DropCountUpperLimit));
				return item;
			}
			else if(ItemCategory.Equals("Food")){
				Food item = ItemFactory.Get(ItemType) as Food;
				item.Count = (int)Random.Range(1,DropCountUpperLimit);
				return item;
			}
			else if(ItemCategory.Equals("Weapon")){
				Weapon item = ItemFactory.Get(ItemType) as Weapon;
				return item;
			}
			else if(ItemCategory.Equals("Armor")){
				Armor item = ItemFactory.Get(ItemType) as Armor;
				return item;
			}
			else if(ItemCategory.Equals("Accessory")){
				Accessory item = ItemFactory.Get(ItemType) as Accessory;
				return item;
			}
			else if (ItemCategory.Equals("Scroll")){
				Scroll item  = ItemFactory.BuildScroll(ItemType) as Scroll;
				return item;
			}
			else{
				Debug.LogError("Item Category not recogonized!");
				return null;
			}
		} else {
			return null;
		}
	}

}

