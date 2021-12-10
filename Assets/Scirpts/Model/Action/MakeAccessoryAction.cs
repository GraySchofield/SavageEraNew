using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MakeAccessoryAction: MakeItemAction
{
	public MakeAccessoryAction (string type, List<Item> requireItems, List<Building> requireBuildings, Accessory produce): base(type, requireItems, requireBuildings, produce)
	{
	}

	public MakeAccessoryAction (MakeAccessoryAction a): base(a)
	{
	}

	public Accessory GetAccessory(){
		return item as Accessory;
	}

	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new MakeAccessoryButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new MakeAccessoryAction(this);
	}
}