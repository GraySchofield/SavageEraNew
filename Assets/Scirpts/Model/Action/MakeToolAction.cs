using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MakeToolAction: MakeItemAction
{
	public MakeToolAction (string type, List<Item> requireItems, List<Building> requireBuildings, Tool produce): base(type, requireItems, requireBuildings, produce)
	{
	}

	public MakeToolAction (MakeToolAction a): base(a)
	{
	}

	public Tool GetTool(){
		return item as Tool;
	}

	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new MakeToolButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new MakeToolAction(this);
	}
}