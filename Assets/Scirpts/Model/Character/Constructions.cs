using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Constructions  {
	//the inventory of the main Character that hoding technology and academic contructions
	
	private List<AcademicConstruction> academics = new List<AcademicConstruction> ();
	// given a type of academic construction, we can find the corresponding object
	private Dictionary<string, AcademicConstruction> academic_index = new Dictionary<string, AcademicConstruction> ();

	private List<TechnologyConstruction> technologys = new List<TechnologyConstruction> ();
	// given a type of technology construction, we can find the corresponding object
	private Dictionary<string, TechnologyConstruction> technology_index = new Dictionary<string, TechnologyConstruction> ();

	// list to hold all kinds of buildings
	private List<Building> buildings = new List<Building> ();
	
	public List<AcademicConstruction> AllAcademicConstructions{
		get{
			return academics;
		}
	}

	public List<Building> AllBuildings{
		get{
			return buildings;
		}
	}

	public List<TechnologyConstruction> AllTechnologyConstructions {
		get {
			return technologys;
		}
	}
	
	public void Add(Building building){
		building = building.Clone ();
		//should use clone, test it to go when other part of the program done.
		//Building clone = building.Clone ();
		buildings.Add (building);
		if (building is House) {
			House house = building as House;
			Game.Current.Hero.MyHouse.Count += house.Count;
			Game.Current.Hero.MyPopulation.AddUpperLimit (house.Count * Game.Current.Hero.MyHouse.Capacity);
		} else if (building is ProductionConstruction) {
			ProductionConstruction pc = building as ProductionConstruction;
			Game.Current.Hero.MyPopulation.AddOccupation (pc.Occupation, pc);
		} else if (building is AcademicConstruction) {
			AcademicConstruction academic = building as AcademicConstruction;
			academics.Add (academic);
			academic_index [academic.Type] = academic;
		} else if (building is TechnologyConstruction) {
			TechnologyConstruction technology = building as TechnologyConstruction;
			technologys.Add (technology);
			technology_index [technology.Type] = technology;
		}

        Game.Current.Recorder.Track(building.Type);
    }

    public void Remove(Building building){
		// assume there is one building per type
		string type = building.Type;
		if (building is AcademicConstruction) {
			AcademicConstruction academic = academic_index [type];
			academics.Remove (academic);
			academic_index.Remove (type);
			buildings.Remove (academic);
		} else if (building is TechnologyConstruction) {
			TechnologyConstruction technology = technology_index [type];
			technologys.Remove(technology);
			technology_index.Remove(type);
			buildings.Remove(technology);
		}
	}
	
	public bool Has(Building building){
		string type = building.Type;
        if (building is AcademicConstruction) {
            return academic_index.ContainsKey(type);
        } else if (building is TechnologyConstruction) {
            return technology_index.ContainsKey(type);
        } else if (building is ProductionConstruction)
        {
            return Game.Current.Hero.MyPopulation.Has((ProductionConstruction)building);
        }
		return false;
	}


	public bool Has(string type, string category){
		if (category.Equals("AcademicConstruction")) {
			return academic_index.ContainsKey(type);
		}else if (category.Equals("TechnologyConstruction")){
			return technology_index.ContainsKey(type); 
		}
		return false;
	}


}