using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class BuildConstructionAction: Action
{
	private Building building;
	
	public BuildConstructionAction (string type, List<Item> requireItems, List<Building> requireBuildings, Building produce): base(type, requireItems, requireBuildings)
	{
		building = produce;
	}

	public BuildConstructionAction (BuildConstructionAction a): base(a)
	{
		building = a.building;
	}

	protected override bool TryRun(){
		if (Doable ()) {
			ConsumeItem ();
			
			Game g = Game.Current;
			Constructions constructions = g.Hero.UserConstructions;
			constructions.Add(building);
			
			g.AddToast(Lang.Current["gain"] + " " + building.Name);

			return true;
		}
		return false;
	}

    public override bool Doable()
    {
        // buildings except house can only be built once.
        if (Type != "spawn_wooden_house" && Game.Current.Hero.UserConstructions.Has(building))
            return false;

        return base.Doable();
    }

    public Building GetBuilding(){
		return building;
	}

	public override BaseModel GetProduce(){
		return GetBuilding ();
	}

	public override ButtonView CreateButtonView (GameObject parentObject, int index){
		return new ConstructionButtonView (parentObject, this, index);
	}

	public override Action Clone(){
		return new BuildConstructionAction(this);
	}
}