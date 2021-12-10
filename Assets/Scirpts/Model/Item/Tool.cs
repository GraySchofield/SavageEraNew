/// <summary>
/// Tools are not stackable but consumable.
/// </summary>
using UnityEngine;

[System.Serializable]
public class Tool: Item
{
	//unique key is used to identify non stackable items
	public string UniqueKey {
		get;
		private set;
	}
	
	public float Remaining {
		get;
		set;
	}

	/// <summary>
	/// This property only use to indicate how much the tool should cost for a particular event
	/// </summary>
	/// <value>The cost.</value>
	public float Cost {
		get;
		set;
	}

	//whether the item is one time or not
	public bool IsOneTime {
		get;
		set;
	}
	
	//whether it is a handtool or Stationary Equipment
	public bool IsEquipment {
		get;
		set;
	}

	public Tool(string type, float remaining, bool is_equipment, bool is_onetime): base(type){
		Remaining = remaining;
		IsEquipment = is_equipment;
		IsOneTime = is_onetime;

		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();
	}
	
	public Tool(Tool t): base(t){
		Remaining = t.Remaining;
		Cost = t.Cost;
		IsEquipment = t.IsEquipment;
		IsOneTime = t.IsOneTime;

		UniqueKey = UniqueKeyGenerator.GenerateStringHashKey();
	}

	public override Item Clone(){
		return new Tool(this);
	}

	// use the tool, this method need to be re-written.
	public void Use(float cost){
		if (Remaining >= cost) {
			Remaining -= cost;
		} else {
			Remaining = 0f;
		}
	}

	// take the state effect of a tool
	// not all tools need this
	public bool TakeEffect(){
        GlobalState state;
		switch (Type) {
            case ItemType.TENT:
                state = new GlobalState(2 * Config.SecondsPerDay, StateType.STATE_TENT, 0f);
                Game.Current.Hero.AddGlobalState(state);
                break;

		    case ItemType.MUSIC_BOX:
                state = new GlobalState(1 * Config.SecondsPerDay, StateType.STATE_MUSIC_BOX, 0f);
                Game.Current.Hero.AddGlobalState(state);
                break;

		    case ItemType.CLIMATE_GLUE:
			    state = new GlobalState(5 * Config.SecondsPerDay, StateType.WEATHER_GLUE, 0f);
                Game.Current.Hero.AddGlobalState(state);
			    break;

		    case ItemType.LUCKY_TOTEM:
			    state = new GlobalState(1 * Config.SecondsPerDay, StateType.LUCKY, 0f);
                Game.Current.Hero.AddGlobalState(state);
                break;

            case ItemType.EVIL_TOTEM:
                state = new GlobalState(2 * Config.SecondsPerDay, StateType.STATE_EVIL, 0f);
                Game.Current.Hero.AddGlobalState(state);
                break;

            case ItemType.SCARECROW:
		    	state = new GlobalState(5 * Config.SecondsPerDay, StateType.STATE_SCARECROW, 0f);
                Game.Current.Hero.AddGlobalState(state);
                break;

            case ItemType.ELF_LIGHT_TOTEM:
                if(Game.Current.Hero.MyPopulation.Total >= 50)
                {
                    state = new GlobalState(3 * Config.SecondsPerDay, StateType.STATE_ELF_LIGHT, 0f);
                    Game.Current.Hero.AddGlobalState(state);
                    Game.Current.Hero.MyPopulation.RemovePopulation(50);

                }
                else
                {
                    //
                    Game.Current.AddToast(Lang.Current["not_enough_elf"]);
                    return false;
                }

                break;

		}


        return true;
	}

	public void RemoveEffect(){		
	}


}

