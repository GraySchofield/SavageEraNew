using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

[Serializable]
public class SpawnStoryEvent: SpawnEvent
{
	private Story story;

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
    public int? IsInSurvivalMode = null;

    public SpawnStoryEvent (string type, List<Item> requireItems, List<Building> requireBuildings, List<string> conditions, Story story): base(type, requireItems, requireBuildings, conditions){
		this.story = story;
	}
	public SpawnStoryEvent (SpawnStoryEvent e): base(e){
		story = e.story;

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
        IsInSurvivalMode = e.IsInSurvivalMode;
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
		// Generate a story here
		story.Run ();
		if (Repeatable) {
			this.reset();
            AddFollowUpEvent(this, Config.SpawnEventTriggerType);
		}
        //Debug.Log ("Story "+Type+" is ran");
        // tell record that this event has been executed.
        Game.Current.Recorder.Track(Type);
    }
	
	public void JustRunStory(){
		story.Run ();
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


        if (IsInSurvivalMode.HasValue && Game.Current.CurrentGameMode == GameMode.Normal)
        {
            return false;
        }

        return true;
	}

	public override IEvent Clone(){
		SpawnStoryEvent e = new SpawnStoryEvent(this);
		e.reset ();
		return e;
	}
}