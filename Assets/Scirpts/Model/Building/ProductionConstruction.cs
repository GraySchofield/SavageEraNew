using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ProductionConstruction: Building
{
	// the require items that this production contruction needed to produce product.
	public List<StackableItem> Requires{
		get;
		private set;
	}
	
	// the products that this production construction will produce as one set
	public List<StackableItem> Produces {
		get;
		private set;
	}
	
	// what type of workers works in this production construction.
	public string Occupation {
		get;
		private set;
	}
	
	// the number of workers work in this production construction.
	public int Workers {
		get;
		set;
	}

	public ProductionConstruction (string type, string occupation, List<StackableItem> requires, List<StackableItem> produces): base(type)
	{
		Occupation = occupation;
		Requires = requires;
		Produces = produces;
		Workers = 0;
	}

	public ProductionConstruction (ProductionConstruction p): base(p){
		Occupation = p.Occupation;
		Requires = p.Requires.Select(i => i).ToList();
		Produces = p.Produces.Select(i => i).ToList();
		Workers = p.Workers;
	}
	
	// ask this production construction to produce products.
	// outcomes - the output variable that specifies the number of item changes
	public void Produce(Inventory inventory, Dictionary<string, int> outcomes){
		//find out how many set of products we can produce in one batch
		int amount = int.MaxValue;
		// check requires
		if (Requires != null) {
			foreach (StackableItem require in Requires) {
				int denominator = require.Count;
				StackableItem stackable = inventory.Get (require.Type, require.GetType().ToString ()) as StackableItem;
				int numerator = (stackable == null)?0:stackable.Count;
				int doable = numerator / denominator;
				if (amount > doable) {
					amount = doable;
				}
			}
		}

		// check how many workers we have, assume that one workers can produce one set of products in one batch.
		if (amount > Workers)
			amount = Workers;

		// if nothing we can produce, return.
		if (amount == 0) {
			return;
		}

		// start producing products
		// deduct require items first
		if (Requires != null) {
			foreach (StackableItem require in Requires) {
				StackableItem clone = require.Clone () as StackableItem;
				clone.Count = amount * require.Count;
				inventory.Remove (clone);

				if(outcomes.ContainsKey(clone.Type)){
					outcomes[clone.Type] -= clone.Count;
				}else{
					outcomes[clone.Type] = -clone.Count;
				}
			}
		}
		// create products	
		foreach (StackableItem produce in Produces) {
			StackableItem clone = produce.Clone() as StackableItem;
			clone.Count = amount * produce.Count;
			inventory.Add(clone);

			if(outcomes.ContainsKey(clone.Type)){
				outcomes[clone.Type] += clone.Count;
			}else{
				outcomes[clone.Type] = clone.Count;
			}
		}
	}

	public override Building Clone(){
		return new ProductionConstruction (this);
	}
}

