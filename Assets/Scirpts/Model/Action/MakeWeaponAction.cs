using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class MakeWeaponAction: MakeItemAction
{
	public MakeWeaponAction (string type, List<Item> requireItems, List<Building> requireBuildings, Weapon produce): base(type, requireItems, requireBuildings, produce)
	{
	}

	public MakeWeaponAction (MakeWeaponAction a): base(a)
	{
	}
	
	public Weapon GetWeapon(){
		return item as Weapon;
	}

	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new MakeWeaponButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new MakeWeaponAction(this);
	}
}

