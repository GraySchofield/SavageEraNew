using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
abstract public class MakeItemAction: Action
{
	protected Item item;
	
	public MakeItemAction (string type, List<Item> requireItems, List<Building> requireBuildings, Item produce): base(type, requireItems, requireBuildings)
	{
		item = produce;
	}
	public MakeItemAction (MakeItemAction a): base(a)
	{
		item = a.item;
	}
	
	protected override bool TryRun(){
		if (Doable ()) {
			ConsumeItem ();
			
			Game g = Game.Current;
			Inventory inventory = g.Hero.UserInventory;
			inventory.Add(item);
			
			g.AddToast(Lang.Current["gain"] + " " + item.Name + "* 1");

			return true;
		}
		return false;
	}

	public override BaseModel GetProduce(){
		return item;
	}
}