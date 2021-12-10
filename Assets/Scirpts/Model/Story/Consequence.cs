using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Reflection;
using System;
using System.Linq;

// consequence of a story event
[System.Serializable]
public class Consequence: BaseModel
{
	protected List<Item> require_items;
	protected List<Building> require_buildings;
	protected List<Item> produced_items;
	protected List<Building> produced_buildings;

	private List<string> resultXMLs = null;
	
	//for story event special consequense
	public Weather? CurrentWeather = null;
	public Season? CurrentSeason = null;
	public float? TempetureUp = null;
	public float? TempetureDown = null;
	public float? HumidityUp = null;
	public float? HumidityDown = null;
	public float? HealthUp = null;
	public float? HealthDown = null;
	public int? CurrentPopulationUp = null;
	public int? CurrentPopulationDown = null;
	public float? MaxHealthUp = null;
	public float? MaxHealthDown = null;

	private float weight = 1;
	public float Weight{
		get{ return weight;}
		set{ weight = value; }
	}
	
	public float Probability {
		get;
		set;
	}

	public Consequence (string type, 
	                    List<Item> requireItems, 
	                    List<Building> requireBuildings, 
	                    List<Item> producedItems,
	                    List<Building> producedBuildings,
	                    List<string> resultXMLs):base(type)
	{
		require_items = requireItems;
		require_buildings = requireBuildings;
		produced_items = producedItems;
		produced_buildings = producedBuildings;
		this.resultXMLs = resultXMLs;
	}

	public Consequence (Consequence c): base(c){
		require_items = c.require_items.Select(i => i).ToList();
		require_buildings = c.require_buildings.Select(i => i).ToList();
		produced_items = c.produced_items.Select(i => i).ToList();
		produced_buildings = c.produced_buildings.Select(i => i).ToList();
		resultXMLs = c.resultXMLs.Select(i => i).ToList();

		CurrentWeather = c.CurrentWeather;
		CurrentSeason = c.CurrentSeason;
		TempetureUp = c.TempetureUp;
		TempetureDown = c.TempetureDown;
		HumidityUp = c.HumidityUp;
		HumidityDown = c.HumidityDown;
		HealthUp = c.HealthUp;
		HealthDown = c.HealthDown;
		CurrentPopulationUp = c.CurrentPopulationUp;
		CurrentPopulationDown = c.CurrentPopulationDown;
		MaxHealthUp = c.MaxHealthUp;
		MaxHealthDown = c.MaxHealthDown;
		
		Weight = c.Weight;
		Probability = c.Probability;
	}

	public void Run(){
		Game g = Game.Current;
		MainCharacter h = g.Hero;
		Inventory inventory = h.UserInventory;
		Constructions constructions = h.UserConstructions;

		ExtraEffects ();

		//lose
		if (require_items != null) {
			foreach (Item item in require_items) {
				inventory.Remove (item);
			}
		}
		if (require_buildings != null) {
			foreach (Building building in require_buildings) {
				constructions.Remove (building);
			}
		}

		//gain
		if (produced_items != null) {
			foreach (Item item in produced_items) {
				inventory.Add (item);
			}
		}
		if (produced_buildings != null) {
			foreach (Building building in produced_buildings) {
				constructions.Add (building);
			}
		}
		//special consequence methods
		if (resultXMLs != null) {
			foreach (string resultXML in resultXMLs) { 
				XmlDocument resultDoc = new XmlDocument();
				resultDoc.LoadXml(resultXML);
				XmlNode resultNode = resultDoc.DocumentElement;
				string methodName = resultNode["MethodName"].InnerText;
				Type consequenceResultFactoryType = typeof(ConsequenceResultFactory);
				MethodInfo info = consequenceResultFactoryType.GetMethod(methodName);
				info.Invoke(null, new object[] {resultNode});
			}
		}
	}

	private void ExtraEffects(){
		Game g = Game.Current;
		MainCharacter h = g.Hero;
		
		//handle special consequense
		if (CurrentWeather.HasValue) {
			h.UserClimate.WeatherToday = CurrentWeather.Value;
		}
		if (CurrentSeason.HasValue) {
			h.UserClimate.theSeason = CurrentSeason.Value;
		}
		if (TempetureUp.HasValue) {
			h.UserClimate.Tempature += TempetureUp.Value * Weight;
		}
		if (TempetureDown.HasValue) {
			h.UserClimate.Tempature -= TempetureDown.Value * Weight;
		}
		if (HumidityUp.HasValue) {
			h.UserClimate.Humidity += HumidityUp.Value * Weight;
		}
		if (HumidityDown.HasValue) {
			h.UserClimate.Humidity -= HumidityDown.Value * Weight;
		}
		
		if (HealthUp.HasValue) {
			h.CurrentHealth += HealthUp.Value * Weight;
		}
		if (HealthDown.HasValue) {
			h.CurrentHealth -= HealthDown.Value * Weight;
		}
		if (MaxHealthUp.HasValue) {
			h.HealthUpperLimit += MaxHealthUp.Value * Weight;
		}
		if (MaxHealthDown.HasValue) {
			h.HealthUpperLimit -= MaxHealthDown.Value * Weight;
		}
		
		if (CurrentPopulationUp.HasValue) {
			h.MyPopulation.AddPopulation(CurrentPopulationUp.Value);
		}
		
		if (CurrentPopulationDown.HasValue) {
			h.MyPopulation.RemovePopulation(CurrentPopulationDown.Value);
		}
	}

	public Consequence Clone(){
		return new Consequence(this);
	}
}
