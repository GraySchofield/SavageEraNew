using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

[Serializable]
abstract public class Action: BaseModel
{
	private List<Item> require_items;
	private List<Building> require_buildings;

	// hook for changing run behavior
	public delegate void Callback ();
    [NonSerialized]
	private List<Callback> on_success_callback = new List<Callback> ();
    [NonSerialized]
    private List<Callback> on_fail_callback = new List<Callback> ();

	public Action(string type, List<Item> requireItems, List<Building> requireBuildings):base(type){
		require_items = requireItems;
		require_buildings = requireBuildings;
	}

	public Action(Action a):base(a){
		require_items = a.require_items.Select(i => i).ToList();
		require_buildings = a.require_buildings.Select(i => i).ToList();
		on_success_callback = a.on_success_callback.Select(i => i).ToList();
		on_fail_callback = a.on_fail_callback.Select(i => i).ToList();
	}

	public virtual void Run (){
		if (TryRun ()) {
            if (on_success_callback != null)
            {
                foreach (Callback c in on_success_callback)
                {
                    c();
                }
            }
		} else {
            if (on_fail_callback != null)
            {
                foreach (Callback c in on_fail_callback)
                {
                    c();
                }
            }
		}
	}

	/// <summary>
	/// core run function to be implemented
	/// </summary>
	/// <returns><c>true</c>, if the run success</returns>
	abstract protected bool TryRun ();

	public virtual bool Doable(){
		Game g = Game.Current;

		// check require items
		if (require_items != null){
			Inventory inventory = Game.Current.Hero.UserInventory;
			foreach (Item require in require_items) {
                if (require != null)
                {
                    if (!inventory.Has(require))
                    {
                        g.AddToast(require.Name + " " + Lang.Current["not_enough"]);
                        return false;
                    }
                }
			}
		}
		
		// check require buildings
		if (require_buildings != null) {
			Constructions constructions = g.Hero.UserConstructions;
			foreach (Building require in require_buildings) {
				if (!constructions.Has (require)) {
					g.AddToast(Lang.Current["need"] + " " +require.Name);
					return false;
				}
			}
		}
		
		return true;
	}

	protected virtual void ConsumeItem(){
		// We will not handle resource in action ( in event instead )
		Inventory inventory = Game.Current.Hero.UserInventory;
		foreach (Item require in require_items){
			inventory.Remove(require);
		}
	}

	public string GetRequireString(){
		//get the require string for spawn events
		StringBuilder  sb = new StringBuilder(); 
		foreach (Item item in require_items) {
            if(item == null)
            {
              //  Debug.LogError("Required Item is null!");
            }
			if(item is StackableItem){
				StackableItem stackable = item as StackableItem;
				sb.Append(" " + stackable.Name + "x" + stackable.Count);
			} else {
                if (item != null)
                {
                    sb.Append(" " + item.Name + "x1");
                }
			}
		}
		return sb.ToString();
	}

	abstract public BaseModel GetProduce();

	public void OnSuccess(Callback c){
        if (on_success_callback == null)
        {
            on_success_callback = new List<Callback>();
            on_success_callback.Add(c);
        }
        else
        {
            on_success_callback.Clear();
            on_success_callback.Add(c);

        }
    }

	public void OnFail(Callback c){
        if (on_fail_callback == null)
        {
            on_fail_callback = new List<Callback>();
            on_fail_callback.Add(c);
        }
        else
        {
            on_fail_callback.Clear();
            on_fail_callback.Add(c);

        }
	}

	abstract public ButtonView CreateButtonView (GameObject parentObject, int index);

	abstract public Action Clone();
}

