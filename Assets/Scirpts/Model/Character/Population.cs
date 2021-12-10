using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// the class that manage workers, as well as production construction.
[System.Serializable]
public class Population
{
	public int UpperLimit {
		get;
		private set;
	}

	// the total population
	public int Total {
		get;
		private set;
	}
	// the number of people who don't have a job.
	public int Idles {
		get;
		private set;
	}

	//job -> production construction mapping, use to look for paticular production construction for a certain job
	Dictionary<string, ProductionConstruction> occupations = new Dictionary<string, ProductionConstruction>();
	
	Dictionary<string, int> productivity = new Dictionary<string, int> ();
	
	public Dictionary<string, int> Productivity {
		get{
			return productivity;
		}
	}
	public Population ()
	{
	}

	public void AddUpperLimit(int num){
		UpperLimit += num;
	}

	public void RemoveUpperLimit(int num){
		UpperLimit -= num;
		if (UpperLimit < 0) {
			UpperLimit = 0;
		}
	}
	
	public void AddPopulation(int num){
		int diff = UpperLimit - Total;
		if (diff <= num) {
			Total += diff;
			Idles += diff;
		} else {
			Total += num;
			Idles += num;
		}
	}

	public void RemovePopulation(int num){
		int numToRemove = num;
		if (Total < num) {
			numToRemove = Total;
		}
		Total -= numToRemove;
		Idles -= numToRemove;
		if (Idles < 0) {
			foreach (string occupation in occupations.Keys) {
				ProductionConstruction pc = occupations [occupation];
				if (pc.Workers + Idles > 0) {
					RemoveWorker (occupation, -Idles);
				} else {
					RemoveWorker (occupation, pc.Workers);
				}
				if (Idles >= 0)
					break;
			}
		}
	}
	

	//add a production construction for a job.
	//If the job has another production construction, replace it and add all its workers to the new one.
	public void AddOccupation(string occupation, ProductionConstruction pc){
		int curWorkers = 0;
		if(occupations.ContainsKey(occupation)){
			curWorkers = occupations[occupation].Workers;
			UpdateProductivity(occupations[occupation], -curWorkers);
		}
		ProductionConstruction clone = pc.Clone () as ProductionConstruction;
		clone.Workers = curWorkers;
		occupations [occupation] = clone;
		UpdateProductivity (clone, curWorkers);
	}

	private void UpdateProductivity(ProductionConstruction pc, int workers){
		List<StackableItem> requires = pc.Requires;
		List<StackableItem> products = pc.Produces;
		foreach(StackableItem require in requires){
			if(!productivity.ContainsKey(require.Type)){
				productivity[require.Type] = 0;
			}
			productivity[require.Type] -= workers * require.Count;
		}
		foreach(StackableItem product in products){
			if(!productivity.ContainsKey(product.Type)){
				productivity[product.Type] = 0;
			}
			productivity[product.Type] += workers * product.Count;
		}
	}

	// remove the production construction for the job. Put the workers back to idles.
	public void RemoveOccupation(string occupation){
		if (!occupations.ContainsKey (occupation)) {
			return;
		}
		int workers = occupations [occupation].Workers;
		Idles += workers;
		UpdateProductivity (occupations [occupation], -workers);
		occupations.Remove (occupation);
	}


	public Dictionary<string, ProductionConstruction> Occupations{
		get {return occupations;}
	}

	// add workers to a certain job
	public void AddWorker(string occupation, int num = 1){
		//not enough no job people
		if (Idles < num) {
			return;
		}
		//don't have this kind of job
		if (!occupations.ContainsKey (occupation)) {
			return;
		}

		ProductionConstruction pc = occupations [occupation];
		pc.Workers += num;
		Idles -= num;
		UpdateProductivity (pc, num);
	}

	public void RemoveWorker(string occupation, int num = 1){
		//don't have this kind of job
		if (!occupations.ContainsKey (occupation)) {
			return;
		}

		ProductionConstruction pc = occupations [occupation];

		//don't have enough workers for this occupation
		if (pc.Workers < num) {
			return;
		}

		pc.Workers -= num;
		Idles += num;
		UpdateProductivity (pc, -num);
	}

	// this function will be called everyday to calculate how many products production construction produce.
	public void Harvest(){
		Dictionary<string, int> outcomes = new Dictionary<string, int> ();
		foreach (string occupation in occupations.Keys) {
			ProductionConstruction pc = occupations [occupation];
			pc.Produce(Game.Current.Hero.UserInventory, outcomes);
		}

		foreach (string type in outcomes.Keys) {
			int count = outcomes[type];
			if (count > 0) {		
				Game.Current.AddToast (Lang.Current ["gain"] + " " + Lang.Current[type] + "*" + count);
			} 
		}

	}

	public bool Has(ProductionConstruction pc){
		string occupation = pc.Occupation;
		if (occupations.ContainsKey (occupation)) {
            
            if (pc.Type.Equals(BuildingType.BASIC_FARM))
            {
                return true;
            }
            else if (pc.Type.Equals(BuildingType.BASIC_PASTURE))
            {
                return true;
            }
            else
            {
                return pc.Type == occupations[occupation].Type;
            }
            
        } else {
			return false;
		}
	}
}

