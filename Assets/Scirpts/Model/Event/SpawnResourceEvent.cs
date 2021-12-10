using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Reflection;
using System.Linq;

[Serializable]
public class SpawnResourceEvent: SpawnEvent
{
	private List<Item> produced_items;

	//for story spawn event
	public float? BrightnessAbove = null;
	public float? BrightnessBelow = null;
	public bool? ShouldLightOn = null;
	public Weather? CurrentWeather = null;
	public Season? CurrentSeason = null;
	public float? TempetureAbove = null;
	public float? TempetureBelow = null;
	public float? HumidityAbove = null;
	public float? HumidityBelow = null;
	public float? GameTimeAfter = null;
	public float? GameTimeBefore = null;
	public float? HealthAbove = null;
	public float? HealthBelow = null;
	public float? MaxHealthAbove = null;
	public float? MaxHealthBelow = null;
	public float? PopulationAbove = null;
	public float? PopulationBelow = null;
	public int? PopulationLimitAbove = null;
	public int? PopulationLimitBelow = null;
    public float? GoodnessAbove = null;
    public float? GoodnessBelow = null;


	public SpawnResourceEvent (string type, List<Item> requires, List<Item> produces, List<string> conditions): base(type, requires, null, conditions){
		produced_items = produces;
	}

	public SpawnResourceEvent (SpawnResourceEvent e): base(e){
		produced_items = e.produced_items.Select(i => i).ToList();

		BrightnessAbove = e.BrightnessAbove;
		BrightnessBelow = e.BrightnessBelow;
		ShouldLightOn = e.ShouldLightOn;
		CurrentWeather = e.CurrentWeather;
		CurrentSeason = e.CurrentSeason;
		TempetureAbove = e.TempetureAbove;
		TempetureBelow = e.TempetureBelow;
		HumidityAbove = e.HumidityAbove;
		HumidityBelow = e.HumidityBelow;
		GameTimeAfter = e.GameTimeAfter;
		GameTimeBefore = e.GameTimeBefore;
		HealthAbove = e.HealthAbove;
		HealthBelow = e.HealthBelow;
		MaxHealthAbove = e.MaxHealthAbove;
		MaxHealthBelow = e.MaxHealthBelow;
		PopulationAbove = e.PopulationAbove;
		PopulationBelow = e.PopulationBelow;
		PopulationLimitAbove = e.PopulationLimitAbove;
		PopulationLimitBelow = e.PopulationLimitBelow;
        GoodnessAbove = e.GoodnessAbove;
        GoodnessBelow = e.GoodnessBelow;
    }

	protected override bool condition(){
		if (!base.condition ()) {
			return false;
		}

		if (!ExtraCondition ()) {
			return false;
		}

		return true;
	}

	protected override void action(){
            Game.Current.ActionEngine.AddWork(this);
            // tell recorder that this event has been executed.
            Game.Current.Recorder.Track(Type);    
    }

	private bool ExtraCondition(){
		Game g = Game.Current;
		MainCharacter hero = g.Hero;
		
		if (BrightnessBelow.HasValue && g.Brightness > BrightnessBelow)
			return false;
		
		if (BrightnessAbove.HasValue && g.Brightness < BrightnessAbove)
			return false;
		
		if (ShouldLightOn.HasValue && g.IsLightOn != ShouldLightOn)
			return false;
		
		if (CurrentWeather.HasValue && hero.UserClimate.WeatherToday != CurrentWeather)
			return false;
		
		if (CurrentSeason.HasValue && hero.UserClimate.theSeason != CurrentSeason)
			return false;
		
		if (TempetureBelow.HasValue && hero.UserClimate.Tempature > TempetureBelow) { 
			return false;
		}
		if (TempetureAbove.HasValue && hero.UserClimate.Tempature < TempetureAbove)
			return false;
		
		if (HumidityBelow.HasValue && hero.UserClimate.Humidity > HumidityBelow)
			return false;
		
		if (HumidityAbove.HasValue && hero.UserClimate.Humidity < HumidityAbove)
			return false;
		
		if (GameTimeBefore.HasValue && g.GameTime > GameTimeBefore)
			return false;
		
		if (GameTimeAfter.HasValue && g.GameTime < GameTimeAfter)
			return false;
		
		if (HealthBelow.HasValue && hero.CurrentHealth > HealthBelow)
			return false;
		
		if (HealthAbove.HasValue && hero.CurrentHealth < HealthAbove)
			return false;
		
		if (MaxHealthBelow.HasValue && hero.HealthUpperLimit > MaxHealthBelow)
			return false;
		
		if (MaxHealthAbove.HasValue && hero.HealthUpperLimit < MaxHealthAbove)
			return false;
		
		if (PopulationBelow.HasValue && hero.MyPopulation.Total > PopulationBelow)
			return false;
		
		if (PopulationAbove.HasValue && hero.MyPopulation.Total < PopulationAbove)
			return false;
		
		if (PopulationLimitAbove.HasValue && hero.MyPopulation.UpperLimit < PopulationLimitAbove)
			return false;
		
		if (PopulationLimitBelow.HasValue && hero.MyPopulation.UpperLimit > PopulationLimitBelow)
			return false;

        if (GoodnessAbove.HasValue && hero.Goodness < GoodnessAbove)
            return false;

		if (GoodnessBelow.HasValue && hero.Goodness > GoodnessBelow)
            return false;

		return true;
	}
	
	public void GetResource(){
		bool toolUseUp;
		if (Doable (spawnIgnore: false)) {
			ConsumeItem (out toolUseUp);

			foreach (Item produce in produced_items) {
				Game.Current.Hero.UserInventory.Add (produce);

				if (produce is StackableItem) {		
					Game.Current.AddToast(Lang.Current["gain"] + " " + produce.Name + "*" + ((StackableItem)produce).Count);
				} else {
					Game.Current.AddToast(Lang.Current["gain"] + " " + produce.Name + "* 1");
				}
			}
			if(!toolUseUp){
				this.reset();
				Game.Current.EventEngine.AddEvent(Config.SpawnEventTriggerType, this);
			}
		}
	}

	public override IEvent Clone(){
		SpawnResourceEvent e = new SpawnResourceEvent(this);
		e.reset ();
		return e;
	}
}