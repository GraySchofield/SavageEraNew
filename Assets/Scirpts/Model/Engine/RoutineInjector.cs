using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System;

[Serializable]
public class RoutineInjector
{
	//keep an index to reference the routine events
	public Dictionary<string, RoutineEvent> RoutineIndex {
		get;
		set;
	}

	public RoutineInjector ()
	{
		RoutineIndex = new Dictionary<string,RoutineEvent> ();
	}
	
	public void Run(bool skipAddEvent = false){	
		RoutineEvent re;
		Game g = Game.Current;
		Recorder r = Game.Current.Recorder;
		
		if(r.Get(EventType.HARVEST) == null){
			re =  new RoutineEvent(EventType.HARVEST, g.Hero.MyPopulation.Harvest);
			RoutineIndex.Add(EventType.HARVEST, re);
			g.EventEngine.AddEvent(Config.SecondsPerDay.ToString(), re);
		}
		
		if(r.Get(EventType.GENERATE_CLIMATE) == null){
			re = new RoutineEvent(EventType.GENERATE_CLIMATE, g.Hero.UserClimate.GenerateClimate);
			RoutineIndex.Add(EventType.GENERATE_CLIMATE, re);
			g.EventEngine.AddEvent(Config.SecondsPerDay.ToString(), re);
		}
		
		if(r.Get(EventType.UPDATE_CURRENT_WEATHER) == null){
			re = new RoutineEvent(EventType.UPDATE_CURRENT_WEATHER, g.Hero.UserClimate.GenerateCurrentWeather);
			g.EventEngine.AddEvent("1", re);
			RoutineIndex.Add(EventType.UPDATE_CURRENT_WEATHER, re);
		}
		
		if (r.Get(EventType.UPDATE_TOOLS) == null) {
			re = new RoutineEvent (EventType.UPDATE_TOOLS, g.Hero.UpdateEquippedTools);
			g.EventEngine.AddEvent ("1", re);
			RoutineIndex.Add (EventType.UPDATE_TOOLS, re);
		}
		
		if (r.Get(EventType.UPDATE_CURRENT_HEALTH) == null) {
			re = new RoutineEvent (EventType.UPDATE_CURRENT_HEALTH, g.Hero.UpdateHealth);
			g.EventEngine.AddEvent ("1", re);
			RoutineIndex.Add (EventType.UPDATE_CURRENT_HEALTH, re);
		}
		
		if (r.Get(EventType.UPDATE_TIMED_STATE) == null) {
			re = new RoutineEvent (EventType.UPDATE_TIMED_STATE, g.Hero.UpdateAllGlobalStates);
			g.EventEngine.AddEvent ("0.5", re);
			RoutineIndex.Add (EventType.UPDATE_TIMED_STATE, re);
		}

        if (r.Get(EventType.SAVE_BACKUP_GAME) == null && g.CurrentGameMode == GameMode.Normal)
        {
            re = new RoutineEvent(EventType.SAVE_BACKUP_GAME, g.SaveBackUpGameInMainThread);
            g.EventEngine.AddEvent("" + Config.SecondsPerDay * Config.BackUpSavePeriod, re);
            RoutineIndex.Add(EventType.SAVE_BACKUP_GAME, re);
        }


        if (r.Get(EventType.SAVE_DAILY_GAME) == null )
        {
            re = new RoutineEvent(EventType.SAVE_DAILY_GAME, g.SaveGameInMainThread);
            g.EventEngine.AddEvent("" + Config.SecondsPerDay * Config.BackUpSavePeriod, re);
            RoutineIndex.Add(EventType.SAVE_DAILY_GAME, re);
        }


        if (r.Get(EventType.UPDATE_DAY_COUNT_RANK) == null)
        {
            re = new RoutineEvent(EventType.UPDATE_DAY_COUNT_RANK, g.UpdateDayCountRank);
            g.EventEngine.AddEvent("" + Config.SecondsPerDay, re);
            RoutineIndex.Add(EventType.UPDATE_DAY_COUNT_RANK, re);
        }

    }
}


