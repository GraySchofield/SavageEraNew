using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MakeArmorAction: MakeItemAction
{
	public MakeArmorAction (string type, List<Item> requireItems, List<Building> requireBuildings, Armor produce): base(type, requireItems, requireBuildings, produce)
	{
	}

	public MakeArmorAction (MakeArmorAction a): base(a)
	{
	}
	
	public Armor GetArmor(){
		return item as Armor;
	}

	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new MakeArmorButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new MakeArmorAction(this);
	}
}


